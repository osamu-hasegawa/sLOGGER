using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	class D76_BME280:D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x76;
		//---

		public D76_BME280(int busadr)
		{
			this.busadr = busadr;
		}
		public override int BUSADR {
			get { return(this.busadr);}
		}
		public override int DEVADR {
			get { return(this.devadr);}
		}
		//---
		int[] digT = new int[3];
		int[] digP = new int[16];
		int[] digH = new int[16];
		double t_fine = 0.0;
		private void setup()
		{
			int osrs_t = 1;			//#Temperature oversampling x 1
			int osrs_p = 1;			//#Pressure oversampling x 1
			int osrs_h = 1;			//#Humidity oversampling x 1
			int mode   = 3;			//#Normal mode
//			int t_sb   = 5;			//#Tstandby 1000ms
			int t_sb   = 2;			//#Tstandby (1:62.5, 2:125, 3:250, 4:500, 5:1000ms)
//			int filter = 0;			//#Filter off
			int filter = 2;			//#Filter(0:off, 1:2, 2,4, 3:8, 4:16)
			int spi3w_en = 0;		//#3-wire SPI Disable

			int ctrl_meas_reg = (osrs_t << 5) | (osrs_p << 2) | mode;
			int config_reg    = (t_sb << 5) | (filter << 2) | spi3w_en;
			int ctrl_hum_reg  = osrs_h;

			WRITE_ONE(0xF2,ctrl_hum_reg);
			WRITE_ONE(0xF4,ctrl_meas_reg);
			WRITE_ONE(0xF5,config_reg);
		}
		private void get_calib_param()
		{
			byte[] calib = new byte[64];
			int h = 0;	
			for (int i = 0; i < 24; i++) {
				calib[h++] = (byte)READ_ONE(0x88+i);
			}
			if (true) {
				calib[h++] = (byte)READ_ONE(0xA1+0);
			}
			for (int i = 0; i < 7; i++) {
				calib[h++] = (byte)READ_ONE(0xE1+i);
			}

	
			digT[0] = (int  )((calib[1] << 8) | calib[0]);
			digT[1] = (short)((calib[3] << 8) | calib[2]);
			digT[2] = (int  )((calib[5] << 8) | calib[4]);
			//---
			digP[0] = (int  )((calib[7] << 8) | calib[6]);
			digP[1] = (short)((calib[9] << 8) | calib[8]);
			digP[2] = (short)((calib[11]<< 8) | calib[10]);
			digP[3] = (short)((calib[13]<< 8) | calib[12]);
			digP[4] = (short)((calib[15]<< 8) | calib[14]);
			digP[5] = (short)((calib[17]<< 8) | calib[16]);
			digP[6] = (short)((calib[19]<< 8) | calib[18]);
			digP[7] = (short)((calib[21]<< 8) | calib[20]);
			digP[8] = (int )((calib[23]<< 8) | calib[22]);
			//---
			digH[0] = (short)( calib[24] );
			digH[1] = (short)((calib[26]<< 8) | calib[25]);
			digH[2] = (short)( calib[27] );
			digH[3] = (short)((calib[28]<< 4) | (0x0F & calib[29]));
			digH[4] = (short)((calib[30]<< 4) | ((calib[29] >> 4) & 0x0F));
			digH[5] = (short)( calib[31] );
			//---
		}
		double compensate_T(int adc_T)
		{
			double v1, v2, temperature;

			v1 = (adc_T / 16384.0 - digT[0] / 1024.0) * digT[1];
			v2 = (adc_T / 131072.0 - digT[0] / 8192.0) * (adc_T / 131072.0 - digT[0] / 8192.0) * digT[2];
			t_fine = v1 + v2;
			temperature = t_fine / 5120.0;
	
			//print "temp : %-6.2f ℃" % (temperature) 

			return(temperature);
		}
		double compensate_H(int adc_H)
		{
			double var_h;

			var_h = t_fine - 76800.0;
			if (var_h != 0) {
				var_h = (adc_H - (digH[3] * 64.0 + digH[4]/16384.0 * var_h)) * (digH[1] / 65536.0 * (1.0 + digH[5] / 67108864.0 * var_h * (1.0 + digH[2] / 67108864.0 * var_h)));
			}
			else {
				return 0;
			}
			var_h = var_h * (1.0 - digH[0] * var_h / 524288.0);
			if (var_h > 100.0) {
				var_h = 100.0;
			}
			else if (var_h < 0.0) {
				var_h = 0.0;
			}
			//print "hum : %6.2f ％" % (var_h)

			return(var_h);
		}
		double compensate_P(int adc_P)
		{
			double pressure, v1, v2;

			pressure = 0.0;
	
			v1 = (t_fine / 2.0) - 64000.0;
			v2 = (((v1 / 4.0) * (v1 / 4.0)) / 2048) * digP[5];
			v2 = v2 + ((v1 * digP[4]) * 2.0);
			v2 = (v2 / 4.0) + (digP[3] * 65536.0);
			v1 = (((digP[2] * (((v1 / 4.0) * (v1 / 4.0)) / 8192)) / 8)  + ((digP[1] * v1) / 2.0)) / 262144;
			v1 = ((32768 + v1) * digP[0]) / 32768;
	
			if (v1 == 0) {
				return 0;
			}
			pressure = ((1048576 - adc_P) - (v2 / 4096)) * 3125;
			if (pressure < 0x80000000) {
				pressure = (pressure * 2.0) / v1;
			}
			else {
				pressure = (pressure / v1) * 2;
			}
			v1 = (digP[8] * (((pressure / 8.0) * (pressure / 8.0)) / 8192.0)) / 4096;
			v2 = ((pressure / 4.0) * digP[7]) / 8192.0;
			pressure = pressure + ((v1 + v2 + digP[6]) / 16.0);

//			print "pressure : %7.2f hPa" % (pressure/100)
			return(pressure/100);
		}
		//---
		public void SET_BUS(int busadr)
		{
			this.busadr = busadr;
		}
		public override bool INIT()
		{
			setup();
			get_calib_param();
			return(false);
		}
		public bool GET(out double T, out double H, out double P)
		{
			byte[] buf = new byte[9];
			int	pres_raw, temp_raw, hum_raw;
			for (int i = 0; i < 8; i++) {
				buf[i] = (byte)READ_ONE(0xF7+i);
			}
			pres_raw = (buf[0] << 12) | (buf[1] << 4) | (buf[2] >> 4);
			temp_raw = (buf[3] << 12) | (buf[4] << 4) | (buf[5] >> 4);
			hum_raw  = (buf[6] << 8)  |  buf[7];
	
			T = compensate_T(temp_raw);
			P = compensate_P(pres_raw);
			H = compensate_H(hum_raw);

			return(true);
		}
		override public bool GET(out object[] obj)
		{
			double T, H, P;
			obj = new object[3];
			GET(out T, out H, out P);
			obj[0] = T;
			obj[1] = H;
			obj[2] = P;
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(3);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = {"TEMP.", "HUMIDITY", "PRESSURE"};
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = {"[degC]", "[%]", "[hPa]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = {2, 2, 2};
			if (idx >= 0 && idx < prec.Length) {
				return(prec[idx]);
			}
			return(0);
		}
	}
}
#if false
///https://github.com/SWITCHSCIENCE/BME280/blob/master/Python27/bme280_sample.py
Skip to content

    Features
    Business
    Explore
    Marketplace
    Pricing

This repository
Sign in or Sign up

21
33

    17

SWITCHSCIENCE/BME280
Code
Issues 0
Pull requests 0
Projects 0
Insights
BME280/Python27/bme280_sample.py
a9f4f81 on 29 Aug 2016
@mitszo mitszo use smbus2 instead of python-smbus package.
@Shinichi-Ohki
@mitszo
152 lines (117 sloc) 3.78 KB
//#coding: utf-8

from smbus2 import SMBus
import time

bus_number  = 1
i2c_address = 0x76

bus = SMBus(bus_number)

digT = []
digP = []
digH = []

t_fine = 0.0


def writeReg(reg_address, data):
	bus.write_byte_data(i2c_address,reg_address,data)

def get_calib_param():
	calib = []
	
	for i in range (0x88,0x88+24):
		calib.append(bus.read_byte_data(i2c_address,i))
	
	calib.append(bus.read_byte_data(i2c_address,0xA1))

	for i in range (0xE1,0xE1+7):
		calib.append(bus.read_byte_data(i2c_address,i))

	digT.append((calib[1] << 8) | calib[0])
	digT.append((calib[3] << 8) | calib[2])
	digT.append((calib[5] << 8) | calib[4])
	digP.append((calib[7] << 8) | calib[6])
	digP.append((calib[9] << 8) | calib[8])
	digP.append((calib[11]<< 8) | calib[10])
	digP.append((calib[13]<< 8) | calib[12])
	digP.append((calib[15]<< 8) | calib[14])
	digP.append((calib[17]<< 8) | calib[16])
	digP.append((calib[19]<< 8) | calib[18])
	digP.append((calib[21]<< 8) | calib[20])
	digP.append((calib[23]<< 8) | calib[22])
	digH.append( calib[24] )
	digH.append((calib[26]<< 8) | calib[25])
	digH.append( calib[27] )
	digH.append((calib[28]<< 4) | (0x0F & calib[29]))
	digH.append((calib[30]<< 4) | ((calib[29] >> 4) & 0x0F))
	digH.append( calib[31] )
	
	for i in range(1,2):
		if digT[i] & 0x8000:
			digT[i] = (-digT[i] ^ 0xFFFF) + 1

	for i in range(1,8):
		if digP[i] & 0x8000:
			digP[i] = (-digP[i] ^ 0xFFFF) + 1

	for i in range(0,6):
		if digH[i] & 0x8000:
			digH[i] = (-digH[i] ^ 0xFFFF) + 1  

def readData():
	data = []
	for i in range (0xF7, 0xF7+8):
		data.append(bus.read_byte_data(i2c_address,i))
	pres_raw = (data[0] << 12) | (data[1] << 4) | (data[2] >> 4)
	temp_raw = (data[3] << 12) | (data[4] << 4) | (data[5] >> 4)
	hum_raw  = (data[6] << 8)  |  data[7]
	
	compensate_T(temp_raw)
	compensate_P(pres_raw)
	compensate_H(hum_raw)

def compensate_P(adc_P): 
 	global  t_fine 
 	pressure = 0.0 
 	 
 	v1 = (t_fine / 2.0) - 64000.0 
 	v2 = (((v1 / 4.0) * (v1 / 4.0)) / 2048) * digP[5] 
 	v2 = v2 + ((v1 * digP[4]) * 2.0) 
 	v2 = (v2 / 4.0) + (digP[3] * 65536.0) 
 	v1 = (((digP[2] * (((v1 / 4.0) * (v1 / 4.0)) / 8192)) / 8)  + ((digP[1] * v1) / 2.0)) / 262144 
 	v1 = ((32768 + v1) * digP[0]) / 32768 
 	 
 	if v1 == 0: 
 		return 0 
 	pressure = ((1048576 - adc_P) - (v2 / 4096)) * 3125 
 	if pressure < 0x80000000: 
 		pressure = (pressure * 2.0) / v1 
 	else: 
 		pressure = (pressure / v1) * 2 
 	v1 = (digP[8] * (((pressure / 8.0) * (pressure / 8.0)) / 8192.0)) / 4096 
 	v2 = ((pressure / 4.0) * digP[7]) / 8192.0 
 	pressure = pressure + ((v1 + v2 + digP[6]) / 16.0)   
  
 	print "pressure : %7.2f hPa" % (pressure/100) 
  
 def compensate_T(adc_T): 
 	global t_fine 
 	v1 = (adc_T / 16384.0 - digT[0] / 1024.0) * digT[1] 
 	v2 = (adc_T / 131072.0 - digT[0] / 8192.0) * (adc_T / 131072.0 - digT[0] / 8192.0) * digT[2] 
 	t_fine = v1 + v2 
 	temperature = t_fine / 5120.0 
 	print "temp : %-6.2f ℃" % (temperature)  
  
 def compensate_H(adc_H): 
 	global t_fine 
 	var_h = t_fine - 76800.0 
 	if var_h != 0: 
 		var_h = (adc_H - (digH[3] * 64.0 + digH[4]/16384.0 * var_h)) * (digH[1] / 65536.0 * (1.0 + digH[5] / 67108864.0 * var_h * (1.0 + digH[2] / 67108864.0 * var_h))) 
 	else: 
 		return 0 
 	var_h = var_h * (1.0 - digH[0] * var_h / 524288.0) 
 	if var_h > 100.0: 
 		var_h = 100.0 
 	elif var_h < 0.0: 
 		var_h = 0.0 
 	print "hum : %6.2f ％" % (var_h) 


def setup():
	osrs_t = 1			#Temperature oversampling x 1
	osrs_p = 1			#Pressure oversampling x 1
	osrs_h = 1			#Humidity oversampling x 1
	mode   = 3			#Normal mode
	t_sb   = 5			#Tstandby 1000ms
	filter = 0			#Filter off
	spi3w_en = 0			#3-wire SPI Disable

	ctrl_meas_reg = (osrs_t << 5) | (osrs_p << 2) | mode
	config_reg    = (t_sb << 5) | (filter << 2) | spi3w_en
	ctrl_hum_reg  = osrs_h

	writeReg(0xF2,ctrl_hum_reg)
	writeReg(0xF4,ctrl_meas_reg)
	writeReg(0xF5,config_reg)


setup()
get_calib_param()


if __name__ == '__main__':
	try:
		readData()
	except KeyboardInterrupt:
		pass





    © 2018 GitHub, Inc.
    Terms
    Privacy
    Security
    Status
    Help

    Contact GitHub
    API
    Training
    Shop
    Blog
    About

Press h to open a hovercard with more details.

#endif
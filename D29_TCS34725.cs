using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
/*
12	76(temp)
12	29(color sensor)
20	28(pot)
20	48(adc)
30	49(adc)
30	50(eeprom)
------------
END (5 devices)
 */
	/// <summary>
	/// https://github.com/adafruit/Adafruit_CircuitPython_TCS34725/blob/a305a54f1c3eb8ef98fa84a1585c3eb4b806045a/adafruit_tcs34725.py
	/// https://github.com/adafruit/Adafruit_TCS34725/blob/master/Adafruit_TCS34725.cpp
	/// </summary>
	/// <seealso cref="sLOGGER.D00_BASE" />
	class D29_TCS34725:D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x29;
		//---

		public D29_TCS34725(int busadr)
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
		const int _COMMAND_BIT       = 0x80;
		const int _REGISTER_ENABLE   = 0x00;
		const int _REGISTER_ATIME    = 0x01;
		const int _REGISTER_AILT     = 0x04;
		const int _REGISTER_AIHT     = 0x06;
		const int _REGISTER_ID       = 0x12;
		const int _REGISTER_APERS    = 0x0c;
		const int _REGISTER_CONTROL  = 0x0f;
		const int _REGISTER_SENSORID = 0x12;
		const int _REGISTER_STATUS   = 0x13;
		const int _REGISTER_CDATA    = 0x14;
		const int _REGISTER_RDATA    = 0x16;
		const int _REGISTER_GDATA    = 0x18;
		const int _REGISTER_BDATA    = 0x1a;
		const int _ENABLE_AIEN       = 0x10;
		const int _ENABLE_WEN        = 0x08;
		const int _ENABLE_AEN        = 0x02;
		const int _ENABLE_PON        = 0x01;
		int[] _GAINS  = {1, 4, 16, 60};
		int[] _CYCLES = {0, 1, 2, 3, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60};
		//---
		double _integration_time=24;
		int _gain=4;
		//int[] digT = new int[3];
		//int[] digP = new int[16];
		//int[] digH = new int[16];
		//double t_fine = 0.0;
		//---
//        private void setup()
//        {
//            int osrs_t = 1;			//#Temperature oversampling x 1
//            int osrs_p = 1;			//#Pressure oversampling x 1
//            int osrs_h = 1;			//#Humidity oversampling x 1
//            int mode   = 3;			//#Normal mode
////			int t_sb   = 5;			//#Tstandby 1000ms
//            int t_sb   = 2;			//#Tstandby (1:62.5, 2:125, 3:250, 4:500, 5:1000ms)
////			int filter = 0;			//#Filter off
//            int filter = 2;			//#Filter(0:off, 1:2, 2,4, 3:8, 4:16)
//            int spi3w_en = 0;		//#3-wire SPI Disable
//        }
		private void setup()
		{
			int sensor_id;
			
			sensor_id = _read_u8(_REGISTER_SENSORID);
			//self._active = False
			//integration_time(2.4);
			//# Check sensor ID is expectd value.
			//sensor_id = self._read_u8(_REGISTER_SENSORID)
			if (sensor_id == 0x44 || sensor_id == 0x10) {
				sensor_id = sensor_id;//ok
			}
			else {
				sensor_id = sensor_id;//ng
			}
			set_integration_time(_integration_time);
			set_gain(_gain);
			set_enable(true);
		}
		private int _read_u8(int reg)
		{
			//# Read an 8-bit unsigned value from the specified 8-bit address.

			int val = READ_ONE(_COMMAND_BIT | reg);
	//        with self._device as i2c:
	//            self._BUFFER[0] = (address | _COMMAND_BIT) & 0xFF
	//            i2c.write(self._BUFFER, end=1, stop=False)
	//            i2c.readinto(self._BUFFER, end=1)
	//return self._BUFFER[0]
			return(val);
		}
		private int _read_u16(int reg)
		{
			byte[] buf = {0,0};
			bool ret;
			ret = READ_BLK(_COMMAND_BIT | reg, 2, ref buf);

			//buf[0] = (byte)READ_ONE(_COMMAND_BIT | reg);
			//buf[1] = (byte)READ_ONE(_COMMAND_BIT | reg+1);

			return((int)buf[1] << 8 | (int)buf[0]);


		}
		private void _write_u8(int reg, int value)
		{
			WRITE_ONE(_COMMAND_BIT | reg, value & 0xFF); 
		}
		private void _write_u16(int address, int value)
		{
		}
		private void set_enable(bool val)
		{
			int enable;
	//        self._active = val
			enable = _read_u8(_REGISTER_ENABLE);
			if (val) {
				_write_u8(_REGISTER_ENABLE, enable | _ENABLE_PON);
				System.Threading.Thread.Sleep(3);
				_write_u8(_REGISTER_ENABLE, enable | _ENABLE_PON | _ENABLE_AEN);
			}
			else {
				_write_u8(_REGISTER_ENABLE, enable & ~(_ENABLE_PON | _ENABLE_AEN));
			}
		}
		private void set_integration_time(double val)
		{
			//assert 2.4 <= val <= 614.4
			int cycles = (int)(val / 2.4);
			
			this._integration_time = cycles * 2.4;// pylint: disable=attribute-defined-outside-init

			_write_u8(_REGISTER_ATIME, 256-cycles);
		}
		private void set_gain(int val)
		{
			this._gain = val;

			_write_u8(_REGISTER_CONTROL, System.Array.IndexOf(_GAINS, val));
		}
		private bool valid()
		{
			//# Check if the status bit is set and the chip is ready.
			int val = _read_u8(_REGISTER_STATUS);
			return((val&0x01)!= 0);
		}
		private int[] color_raw()
		{
			//Read the raw RGBC color detected by the sensor.  Returns a 4-tuple of
			// 16-bit red, green, blue, clear component byte values (0-65535).

			while (valid() == false) {
				System.Threading.Thread.Sleep((int)(this._integration_time + 0.9));
			}
			int[]data = {0,0,0,0};
        
			data[0] = _read_u16(_REGISTER_RDATA);
			data[1] = _read_u16(_REGISTER_GDATA);
			data[2] = _read_u16(_REGISTER_BDATA);
			data[3] = _read_u16(_REGISTER_CDATA);
			return(data);
		}
		private int[] color_rgb_bytes()
		{
			//Read the RGB color detected by the sensor.  Returns a 3-tuple of
			//red, green, blue component values as bytes (0-255).

			int[] rgbc = color_raw();
			int red, green, blue;
			double	r = rgbc[0],
					g = rgbc[1],
					b = rgbc[2],
					clear = rgbc[3];
			r = (int)( (r/clear) * 256) / 255.0;
			g = (int)( (g/clear) * 256) / 255.0;
			b = (int)( (b/clear) * 256) / 255.0;
	        red   = (int)(Math.Pow(r, 2.5) * 255);
		    green = (int)(Math.Pow(g, 2.5) * 255);
			blue  = (int)(Math.Pow(b, 2.5) * 255);
			return (new int[] {red, green, blue});
		}
#if false
def _temperature_and_lux(data):
    """Convert the 4-tuple of raw RGBC data to color temperature and lux values. Will return
       2-tuple of color temperature and lux."""
    r, g, b, _ = data
    x = -0.14282 * r + 1.54924 * g + -0.95641 * b
    y = -0.32466 * r + 1.57837 * g + -0.73191 * b
    z = -0.68202 * r + 0.77073 * g +  0.56332 * b
    divisor = x + y + z
    n = (x / divisor - 0.3320) / (0.1858 - y / divisor)
    cct = 449.0 * n**3 + 3525.0 * n**2 + 6823.3 * n + 5520.33
return cct, y
#endif
		//---
		public void SET_BUS(int busadr)
		{
			this.busadr = busadr;
		}
		public override bool INIT()
		{
			setup();
			//get_calib_param();
			return(false);
		}
		public bool GET(out int R, out int G, out int B, out int C)
		{
			//Read the raw RGBC color detected by the sensor.  Returns a 4-tuple of
			// 16-bit red, green, blue, clear component byte values (0-65535).

			while (valid() == false) {
				System.Threading.Thread.Sleep((int)(this._integration_time + 0.9));
			}
        
			R = _read_u16(_REGISTER_RDATA);
			G = _read_u16(_REGISTER_GDATA);
			B = _read_u16(_REGISTER_BDATA);
			C = _read_u16(_REGISTER_CDATA);

			return(true);
		}
		override public bool GET(out object[] obj)
		{
			int R, G, B, C;
			obj = new object[4];
			GET(out R, out G, out B, out C);
			obj[0] = R;
			obj[1] = G;
			obj[2] = B;
			obj[3] = C;
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(4);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = {"R", "G", "B", "C"};
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = {"[count]", "[count]", "[count]", "[count]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = {0, 0, 0, 0};
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
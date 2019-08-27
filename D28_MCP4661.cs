using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	/// <summary>
	/// https://github.com/pkrakow/MCP466_DigitalPot/blob/master/MCP466_DigitalPot.cpp
	/// https://github.com/riotnetwork/MCP4561/blob/master/MCP4561.cpp
	/// </summary>
	/// <seealso cref="sLOGGER.D00_BASE" />
	class D28_MCP4661:D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x28;
		//---

		public D28_MCP4661(int busadr)
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
			write_10bit(0, 0);	//wiper0 <- 0ohm
			write_10bit(1, 0);	//wiper0 <- 0ohm

			System.Threading.Thread.Sleep(50);
			int ret;
			
			ret = read_10bit(0);
			ret = read_10bit(1);

		}
		private void write_10bit(int reg, int val)
		{
			byte b1 = 0, b2 = 0;
			UInt16 tempWord = (UInt16)val;
			byte tempByte;
			reg <<= 4;					// Shift the value of Register to the left by four bits 
			b1 |= (byte)reg;					// Load the register address into the firstCommandByte 
			tempWord &= 0x0100;         // Clear the top 7 bits and the lower byte of the input value to pick up the two data bits 
			tempWord /= 256;            // Shift the top byte of the input value to the right by one byte 
			tempByte = (byte)tempWord;  // Store the top byte of the input value in a byte sized variable 
			b1 |= tempByte;				// Load the two top input data bits into the firstCommandByte 
			tempWord = (UInt16)val;     // Load the input value into the tempWord 
			tempWord &= 0x00FF;         // Clear the top byte 
			b2 = (byte)tempWord;		// Store the lower byte of the input value in the secondCommandByte 

			WRITE_ONE(b1, b2);
		/*
		 
uint8_t MCP4561::write(char mem_addr, uint16_t setValue) // mem_addr is 00-0F, setvalue is 0-257
{
//if you set the volatile output register, the same value is put to the non-volatile register
	if(setValue <0)
	{
	setValue =0;
	}
	if(setValue >257)
	{
	setValue =257;
	}
	if(mem_addr <0x00 || mem_addr >0x0F )
	{
	return 0;
	}

  byte cmd_byte = 0x00,data_byte = 0x00;   
  cmd_byte = ((mem_addr <<4) &B11110000) | (((setValue&0x01FF)>>8)&B00000011);  //  top 4 is now address   2 command   2 data (D9,D8)
  data_byte = lowByte(setValue);          // is now D7-D0
  Wire.beginTransmission(dev_ADDR);       // transmit to device this has the read byte ( LSB) set as 1
                                          // device address                 0101110x
  Wire.write(cmd_byte);                   // sends command byte             AAAACCDD
  Wire.write(data_byte);                  // sends potentiometer value byte DDDDDDDD  (D7-D0)
  Wire.endTransmission();                 // stop transmitting
  Wire.flush();
  if (mem_addr == WIPER_0_NON_VOLATILE || mem_addr == WIPER_1_NON_VOLATILE) {
	  delay(10); // EEPROM takes 5 - 10 ms to write ( datasheet page 12 )
  }
  else {
	  delay(2); // NV memory is faster
  }
  						  // give unit time to apply the value to non volatile register
  uint8_t set_reading = read(mem_addr);
  if (set_reading == setValue)
  {
  return 1; // it has accepted our setting ( EEPROM reflects what we set it to )
  }
  return 0;
}
		 
		 
		 */
		}
		private int read_10bit(int mem_addr)
		{
			byte[] buf = {0,0};
			byte cmd_byte =0x0F;//, highbyte,lowbyte;
			
			cmd_byte = (byte)((mem_addr<<4) | 0x0C);//B00001100 ; 
   
			READ_BLK(mem_addr, 2, ref buf);
			//Wire.beginTransmission(dev_ADDR);
			//Wire.write(cmd_byte); 
			//Wire.endTransmission(); 
			//Wire.requestFrom(dev_ADDR, 2); 
			//Wire.endTransmission();                 // stop transmitting
			//if(Wire.available())
			//{
			//highbyte =  Wire.read();  //high byte
			//lowbyte =   Wire.read();  //low byte
			//Wire.flush();
			//}
			//  unsigned int returnValue =0;
			//  returnValue = (((uint16_t)highbyte<<8)|lowbyte) & 0x01FF;
			//return returnValue;
			int ret;
			ret = ((UInt16)buf[0] << 8 | buf[1]) & 0x01FF;
			return(ret);
		}
		//---
		public void SET_BUS(int busadr)
		{
			this.busadr = busadr;
		}
		public override bool INIT()
		{
			setup();
			return(false);
		}
		public bool SET(int ch, int val)
		{
			write_10bit(ch, val);


			System.Threading.Thread.Sleep(50);
			int ret;
			
			ret = read_10bit(ch);

			return(true);
		}
		public bool GET()
		{
			return(true);
		}
		override public bool GET(out object[] obj)
		{
			obj = null;
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(0);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = null;//{""};
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = null;//{"[degC]", "[%]", "[hPa]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = null;//{2, 2, 2};
			if (idx >= 0 && idx < prec.Length) {
				return(prec[idx]);
			}
			return(0);
		}
	}
}

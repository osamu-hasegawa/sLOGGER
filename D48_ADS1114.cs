using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	/// <summary>
	/// https://github.com/ControlEverythingCommunity/ADS1114/blob/master/C/ADS1114.c
	/// </summary>
	/// <seealso cref="sLOGGER.D00_BASE" />
	class D48_ADS1114:D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x48;
		//---

		public D48_ADS1114(int busadr)
		{
			this.busadr = busadr;
		}
		public D48_ADS1114(int busadr, int devadr)
		{
			this.busadr = busadr;
			this.devadr = devadr;
		}
		public override int BUSADR {
			get { return(this.busadr);}
		}
		public override int DEVADR {
			get { return(this.devadr);}
		}
		//---
		// CONFIG register(addr=1, 16bit, def:8583h)
		//
		//	9.6.3 Config Register (P[1:0] = 1h) [reset = 8583h]
		//	The 16-bit Config register is used to control the operating mode, input selection, data rate, full-scale range, and
		//	comparator modes.

		// Table 8. Config Register Field Descriptions
		//Bit Field Type Reset Description
		//15 OS R/W 1h
		//				Operational status or single-shot conversion start
		//				This bit determines the operational status of the device. OS can only be written
		//				when in power-down state and has no effect when a conversion is ongoing.
		//				When writing:
/*						0 : No effect
						1 : Start a single conversion (when in power-down state)
						When reading:
						0 : Device is currently performing a conversion
						1 : Device is not currently performing a conversion
		14:12 MUX[2:0] R/W 0h
						Input multiplexer configuration (ADS1115 only)
						These bits configure the input multiplexer. These bits serve no function on the
						ADS1113 and ADS1114.
						000 : AINP = AIN0 and AINN = AIN1 (default)
						001 : AINP = AIN0 and AINN = AIN3
						010 : AINP = AIN1 and AINN = AIN3
						011 : AINP = AIN2 and AINN = AIN3
						100 : AINP = AIN0 and AINN = GND
						101 : AINP = AIN1 and AINN = GND
						110 : AINP = AIN2 and AINN = GND
						111 : AINP = AIN3 and AINN = GND
						11:9 PGA[2:0] R/W 2h
						Programmable gain amplifier configuration
						These bits set the FSR of the programmable gain amplifier. These bits serve no
						function on the ADS1113.
						000 : FSR = ±6.144 V(1)
						001 : FSR = ±4.096 V(1)
						010 : FSR = ±2.048 V (default)
						011 : FSR = ±1.024 V
						100 : FSR = ±0.512 V
						101 : FSR = ±0.256 V
						110 : FSR = ±0.256 V
						111 : FSR = ±0.256 V
						8 MODE R/W 1h
						Device operating mode
						This bit controls the operating mode.
						0 : Continuous-conversion mode
						1 : Single-shot mode or power-down state (default)
						7:5 DR[2:0] R/W 4h
						Data rate
						These bits control the data rate setting.
						000 : 8 SPS
						001 : 16 SPS
						010 : 32 SPS
						011 : 64 SPS
						100 : 128 SPS (default)
						101 : 250 SPS
						110 : 475 SPS
						111 : 860 SPS
						29
						ADS1113, ADS1114, ADS1115
						www.ti.com JAJS373D –MAY 2009–REVISED JANUARY 2018
						Copyright © 2009–2018, Texas Instruments Incorporated
						Table 8. Config Register Field Descriptions (continued)
						Bit Field Type Reset Description
						4 COMP_MODE R/W 0h
						Comparator mode (ADS1114 and ADS1115 only)
						This bit configures the comparator operating mode. This bit serves no function on
						the ADS1113.
						0 : Traditional comparator (default)
						1 : Window comparator
						3 COMP_POL R/W 0h
						Comparator polarity (ADS1114 and ADS1115 only)
						This bit controls the polarity of the ALERT/RDY pin. This bit serves no function on
						the ADS1113.
						0 : Active low (default)
						1 : Active high
						2 COMP_LAT R/W 0h
						Latching comparator (ADS1114 and ADS1115 only)
						This bit controls whether the ALERT/RDY pin latches after being asserted or
						clears after conversions are within the margin of the upper and lower threshold
						values. This bit serves no function on the ADS1113.
						0 : Nonlatching comparator . The ALERT/RDY pin does not latch when asserted
						(default).
						1 : Latching comparator. The asserted ALERT/RDY pin remains latched until
						conversion data are read by the master or an appropriate SMBus alert response
						is sent by the master. The device responds with its address, and it is the lowest
						address currently asserting the ALERT/RDY bus line.
						1:0 COMP_QUE[1:0] R/W 3h
						Comparator queue and disable (ADS1114 and ADS1115 only)
						These bits perform two functions. When set to 11, the comparator is disabled and
						the ALERT/RDY pin is set to a high-impedance state. When set to any other
						value, the ALERT/RDY pin and the comparator function are enabled, and the set
						value determines the number of successive conversions exceeding the upper or
						lower threshold required before asserting the ALERT/RDY pin. These bits serve
						no function on the ADS1113.
						00 : Assert after one conversion
						01 : Assert after two conversions
						10 : Assert after four conversions
						11 : Disable comparator and set ALERT/RDY pin to high-impedance (default)
		 */
		//---
		int CONF_OS  = 0;
		int CONF_MUX = 0;//AINP=AIN0, AINN=AIN1
//		int CONF_MUX = 4;//AINp=AIN0, AINn=GND
		int CONF_PGA = 2;//FSR=+/-2.048V
		int CONF_MODE = 1;//SINGLE-SHOT
		int CONF_DR   = 4;//128 SPS
		int CONF_COMP_MODE=0;//Traditional comparator
		int CONF_COMP_POL=0;//Active low
		int CONF_COMP_LAT=0;//Nonlatching comparator
		int CONF_COMP_QUE=3;//disable comparator and set ALERT/RDY pin to high-impedance
		double
			FULL_SCALE;
		private void setup()
		{
			int val = 0;
			byte[] buf = {0, 0};

			//CONF_PGA = 1;//+-4.096V
			CONF_MODE = 0;//CONTINUOUS
			//CONF_DR = 7;//860 SPS
			//CONF_DR = 4;//128 SPS

			val |= (CONF_OS       << 15);
			val |= (CONF_MUX      << 12);
			val |= (CONF_PGA      <<  9);
			val |= (CONF_MODE     <<  8);
			val |= (CONF_DR       <<  5);
			val |= (CONF_PGA      <<  3);
			val |= (CONF_COMP_QUE <<  0);

			buf[0] = (byte)(val >>  8);
			buf[1] = (byte)(val & 0xFF);

			WRITE_BLK(0x01, 2, buf);
			//---
			switch (CONF_PGA) {
			case  0:FULL_SCALE = 6.144;break;
			case  1:FULL_SCALE = 4.096;break;
			case  2:FULL_SCALE = 2.048;break;
			case  3:FULL_SCALE = 1.024;break;
			case  4:FULL_SCALE = 0.512;break;
			case  5:FULL_SCALE = 0.256;break;
			case  6:FULL_SCALE = 0.256;break;
			default:FULL_SCALE = 0.256;break;
			}
		}
		//---
		public double TO_VOLTAGE(int cnt)
		{
			double vol = (this.FULL_SCALE * cnt / 0x7FFF);
			return(vol);
		}
		//---

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
		public bool INIT(int pga, int sps)
		{
			this.CONF_PGA = pga;
			this.CONF_DR = sps;
			setup();
			return(false);
		}
		public bool GET(out int cnt, out double vol)
		{
			byte[] buf = {0, 0};

			if (CONF_MODE == 1) {//single.shot
				WRITE_ONE(0x01, 0x80);
			}
			READ_BLK(0x00, 2, ref buf);

			cnt = (int)buf[0] * 256 + buf[1];
			if (cnt > 32767) {
				cnt -= 65535;
			}
			vol = TO_VOLTAGE(cnt);
			return(true);
		}
		override public bool GET(out object[] obj)
		{
			int C;
			double V;
			obj = new object[1];
			GET(out C, out V);
			obj[0] = V;
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(1);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = {"ADC"};
			if (this.DEVADR == 0x48) {
				name = new string[] {"ADC(筋電)"};
			}
			else {
				name = new string[] {"ADC(圧力)"};
			}
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = {"[VOLT]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = {3};
			if (idx >= 0 && idx < prec.Length) {
				return(prec[idx]);
			}
			return(0);
		}
	}
}

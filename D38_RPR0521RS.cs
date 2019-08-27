using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	class D38_RPR0521RS : D00_BASE
	{
		int busadr = 0x00;
		int devadr = 0x38;
		//---

		public D38_RPR0521RS(int busadr)
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
		const int REG_DEVICE_ADDRESS                   =(0x38); // 7bit Addrss
		const int REG_PART_ID_VAL                      =(0x0A);
		const int REG_MANUFACT_ID_VAL                  =(0xE0);

		const int REG_SYSTEM_CONTROL                   =(0x40);
		const int REG_MODE_CONTROL                     =(0x41);
		const int REG_ALS_PS_CONTROL                   =(0x42);
		const int REG_PS_CONTROL                       =(0x43);
		const int REG_PS_DATA_LSB                      =(0x44);
		const int REG_ALS_DATA0_LSB                    =(0x46);
		const int REG_MANUFACT_ID                      =(0x92);
		//---
		const int MODE_CONTROL_MEASTIME_100_100MS  =(6 << 0);
		const int MODE_CONTROL_PS_EN               =(1 << 6);
		const int MODE_CONTROL_ALS_EN              =(1 << 7);

		const int ALS_PS_CONTROL_LED_CURRENT_100MA =(2 << 0);
		const int ALS_PS_CONTROL_DATA1_GAIN_X1     =(0 << 2);
		const int ALS_PS_CONTROL_DATA0_GAIN_X1     =(0 << 4);

		const int PS_CONTROL_PS_GAINX1             =(0 << 4);

		const int MODE_CONTROL_VAL                 =(MODE_CONTROL_MEASTIME_100_100MS | MODE_CONTROL_PS_EN | MODE_CONTROL_ALS_EN);
		const int ALS_PS_CONTROL_VAL               =(ALS_PS_CONTROL_DATA0_GAIN_X1 | ALS_PS_CONTROL_DATA1_GAIN_X1 | ALS_PS_CONTROL_LED_CURRENT_100MA);
		const int PS_CONTROL_VAL                   =(PS_CONTROL_PS_GAINX1);

		const int NEAR_THRESH                      =(1000); // example value
		const int FAR_VAL                          =(0);
		const int NEAR_VAL                         =(1);

		const int REG_ERROR                            =(-1);
		//---
		ushort _als_data0_gain;
		ushort _als_data1_gain;
		ushort _als_measure_time;

		private bool setup()
		{
			byte rc;
			int reg;
			int index;
			byte[] als_gain_table = {1, 2, 64, 128};
			ushort[] als_meas_time_table = {0,0,0,0,0,100,100,100,100,100,400,400,50,0,0,0};

			reg = READ_ONE(REG_SYSTEM_CONTROL);
			//if (rc != 0) {
			//    Serial.println(F("Can't access RPR0521RS"));
			//    return (rc);
			//}
			reg &= 0x3F;
			//Serial.print(F("RPR0521RS Part ID Register Value = 0x"));
			//Serial.println(reg, HEX);

			if (reg != REG_PART_ID_VAL) {
				//Serial.println(F("Can't find RPR0521RS"));
				return (false);
			}

			reg = READ_ONE(REG_MANUFACT_ID);
			//if (rc != 0) {
			//Serial.println(F("Can't access RPR0521RS"));
			//return (rc);
			//}
			//Serial.print(F("RPR0521RS MANUFACT_ID Register Value = 0x"));
			//Serial.println(reg, HEX);

			if (reg != REG_MANUFACT_ID_VAL) {
				//Serial.println(F("Can't find RPR0521RS"));
				return (false);
			}
			if (false) {
				WRITE_ONE(/*0x42*/REG_ALS_PS_CONTROL, ALS_PS_CONTROL_VAL);
			}
			else {
				reg = 0;
				reg |= G.SS.D38_424_ALG1 << 4;
				reg |= G.SS.D38_422_ALG2 << 2;
				reg |= G.SS.D38_420_LEDC << 0;
				WRITE_ONE(/*42h*/REG_ALS_PS_CONTROL, reg);
			}
			//if (rc != 0) {
			//Serial.println(F("Can't write RPR0521RS ALS_PS_CONTROL register"));
			//return (rc);
			//}

			reg = READ_ONE(REG_PS_CONTROL);
			//if (rc != 0) {
			//Serial.println(F("Can't read RPR0521RS PS_CONTROL register"));
			//return (rc);
			//}

			reg |= PS_CONTROL_VAL;
			if (false) {
				WRITE_ONE(/*0x43h*/REG_PS_CONTROL, reg);
			}
			else {
				reg = READ_ONE(REG_PS_CONTROL);
				reg |= G.SS.D38_434_PSG0 << 4;
				WRITE_ONE(/*43h*/REG_PS_CONTROL, reg);
			}
			//if (rc != 0) {
			//Serial.println(F("Can't write RPR0521RS PS_CONTROL register"));
			//}

			
			if (false) {
				WRITE_ONE(/*41h*/REG_MODE_CONTROL, MODE_CONTROL_VAL);
			}
			else {
				reg = 0;
				reg |= G.SS.D38_410_MSTM << 0;
				reg |= MODE_CONTROL_PS_EN;
				reg |= MODE_CONTROL_ALS_EN;
				WRITE_ONE(/*41h*/REG_MODE_CONTROL, reg);
			}
			//if (rc != 0) {
			//Serial.println(F("Can't write RPR0521RS MODE CONTROL register"));
			//return (rc);
			//}

			reg = ALS_PS_CONTROL_VAL;
			index = (reg >> 4) & 0x03;
			_als_data0_gain = als_gain_table[index];
			index = (reg >> 2) & 0x03;
			_als_data1_gain = als_gain_table[index];

			index = MODE_CONTROL_VAL & 0x0F;
			_als_measure_time = als_meas_time_table[index];
			return(true);
		}
		int check_near_far(ushort data)
		{
			if (data >= NEAR_THRESH) {
				return (NEAR_VAL);
			}
			else {
				return (FAR_VAL);
			}
		}
		double convert_lx(ushort[] data)
		{
			double lx;
			double d0, d1, d1_d0;

			if (_als_data0_gain == 0) {
				return (REG_ERROR);
			}

			if (_als_data1_gain == 0) {
				return (REG_ERROR);
			}

			if (_als_measure_time == 0) {
				return (REG_ERROR);
			}
			else if (_als_measure_time == 50) {
				if ((data[0] & 0x8000) == 0x8000) {
					data[0] = 0x7FFF;
				}
				if ((data[1] & 0x8000) == 0x8000) {
					data[1] = 0x7FFF;
				}
			}

			d0 = (float)data[0] * (100 / _als_measure_time) / _als_data0_gain;
			d1 = (float)data[1] * (100 / _als_measure_time) / _als_data1_gain;

			if (d0 == 0) {
				lx = 0;
				return (lx);
			}

			d1_d0 = d1 / d0;

			if (d1_d0 < 0.595) {
				lx = (1.682 * d0 - 1.877 * d1);
			}
			else if (d1_d0 < 1.015) {
				lx = (0.644 * d0 - 0.132 * d1);
			}
			else if (d1_d0 < 1.352) {
				lx = (0.756 * d0 - 0.243 * d1);
			}
			else if (d1_d0 < 3.053) {
				lx = (0.766 * d0 - 0.25 * d1);
			}
			else {
				lx = 0;
			}
			return (lx);
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
		public bool GET(out double L, out int P)
		{
			byte[] buf = new byte[6];
			ushort	rawps;
			ushort[] rawals = {0,0};

			for (int i = 0; i < 6; i++) {
				buf[i] = (byte)READ_ONE(REG_PS_DATA_LSB+i);
			}
			rawps     = (ushort)(((ushort)buf[1] << 8) | buf[0]);
			rawals[0] = (ushort)(((ushort)buf[3] << 8) | buf[2]);
			rawals[1] = (ushort)(((ushort)buf[5] << 8) | buf[4]);
	
			P = rawps;
			L = convert_lx(rawals);

			return(true);
		}
		override public bool GET(out object[] obj)
		{
			double L;
			int	P;
			obj = new object[2];
			GET(out L, out P);
			obj[0] = L;
			obj[1] = P;
			return(true);
		}
		override public int SENS_COUNT()
		{
			return(2);
		}
		override public string SENS_NAME(int idx)
		{
			string[] name = {"ALS", "PS"};
			if (idx >= 0 && idx < name.Length) {
				return(name[idx]);
			}
			return(null);
		}
		override public string SENS_UNIT(int idx)
		{
			string[] unit = {"[lx]", "[cnt]"};
			if (idx >= 0 && idx < unit.Length) {
				return(unit[idx]);
			}
			return(null);
		}
		override public int SENS_PREC(int idx)
		{
			int[] prec = {2, 0};
			if (idx >= 0 && idx < prec.Length) {
				return(prec[idx]);
			}
			return(0);
		}
	}
}

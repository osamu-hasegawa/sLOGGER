using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	abstract class D00_BASE
	{
		public abstract
		int BUSADR {get;}
		public abstract
		int DEVADR {get;}
		public abstract bool GET(out object[] obj);
		public abstract int SENS_COUNT();
		public abstract string SENS_NAME(int idx);
		public abstract string SENS_UNIT(int idx);
		public abstract int SENS_PREC(int idx);
		public abstract bool INIT();

		public void WRITE_ONE(int adr, int dat)
		{
			byte[] buf = {(byte)dat};
			int	ret, done;

			D.GET_I2C_WRT(BUSADR, DEVADR, adr, /*len*/1, buf, out ret, out done);
			if (ret != 1 || done != 1) {
				G.mlog("ret != 1 || done != 1");
			}
		}
		public int READ_ONE(int adr)
		{
			byte[] buf = {0};
			int	ret, done;

			D.GET_I2C_RED(BUSADR, DEVADR, adr, /*len*/1, out buf, out ret, out done);
			if (ret != 1 || done != 1) {
				System.Diagnostics.Debug.WriteLine("ret != 1 || done != 1");
				throw new Exception("ERROR@READ_ONE");
			}
			return(buf[0]);
		}
		public bool WRITE_BLK(int adr, int len, byte[] buf)
		{
			int	ret, done;

			D.GET_I2C_WRT(BUSADR, DEVADR, adr, len, buf, out ret, out done);
			if (ret != 1 || done != len) {
				G.mlog("ret != 1 || done != len");
				return(false);
			}
			return(true);
		}
		public bool READ_BLK(int adr, int len, ref byte[] buf)
		{
			int	ret, done;
			byte[] tmp;
			D.GET_I2C_RED(BUSADR, DEVADR, adr, len, out tmp, out ret, out done);
			for (int i = 0; i < done; i++) {
				buf[i] = tmp[i];
			}
			if (ret != 1 || done != len) {
				G.mlog("ret != 1 || done != len");
				return(false);
			}
			return(true);
		}
	}
}

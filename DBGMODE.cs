using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//---
using System.Threading;
using System.Threading.Tasks;
//using System.Threading.Thread;
//using Basler.Pylon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace sLOGGER
{
	class DBGMODE
	{
		static private
		Thread m_thrd_of_dev = null;
		static private
		AutoResetEvent m_event_dev = null;
		static private
		bool m_exit_req = false;
		static private
		int m_idx;
//		static public Form m_fm = null;
//		static public bool bLOWSPD = false;
	    static private int B4(int l) {
		    return ((byte)((l & 0xff000000) >> 24));
	    }
	    static private int B3(int l) {
		    return ((byte)((l & 0x00ff0000) >> 16));
	    }
	    static private int B2(int l) {
		    return ((byte)((l & 0x0000ff00) >> 8));
	    }
	    static private int B1(int l) {
		    return ((byte)((l & 0x000000ff) >> 0));
	    }
		static private int MAKELONG(int b4, int b3, int b2, int b1) {
		    return ((0xff & b4) << 24 | (0xff & b3) << 16 | (0xff & b2) << 8 | (0xff & b1));
	    }
		static public void INIT()
		{
			if (m_thrd_of_dev == null) {
				m_thrd_of_dev = new Thread( new ThreadStart( THRD_OF_DEVICE ) );
			}
			if (m_event_dev == null) {
				m_event_dev = new AutoResetEvent(false);
			}
			//---
			m_exit_req = false;
			m_thrd_of_dev.Start();
		}
		static public void TERM()
		{
			m_exit_req = true;
			if (m_event_dev != null) {
				m_event_dev.Set();
			}
			Thread.Sleep(250); 
			
			if (m_thrd_of_dev != null) {
				m_thrd_of_dev.Abort();
				m_thrd_of_dev = null;
			}
		}
		static public int HID_ENUM(uint vid, uint pid, out int cnt)
		{
			cnt = 0;
			if (vid == 0x04D8 && pid == 0xEED8) {
				cnt = 0;//sensor.mini
			}
			if (vid == 0x04D8 && pid == 0xEF22) {
				cnt = 1;//sensor.big
			}
			return(1);
		}
		static public int HID_OPEN(uint vid, uint pid, uint did)
		{
			INIT();
			return(1);
		}
		static public int HID_CLOSE()
		{
			TERM();
			return(1);
		}

		static byte[] d38_reg = new byte[256];

		static public void D38_RD(int adr, byte[] buf, int len)
		{
			if (adr == 0x00) {
				//for device scan
			}
			if (adr == 0x44) {
				int val;
				if ((Environment.TickCount % 30000) >= 15000) {
				val = (int)(3000 * Math.Abs(Math.Sin(m_idx/300.0*Math.PI)));
				}
				else {
				val = 0;
				m_idx = 150;
				}
				d38_reg[0x44] = (byte)B1(val);
				d38_reg[0x45] = (byte)B2(val);
			}
			if (adr == 0x46) {
				int val = (int)(100 +m_idx%1000);
				d38_reg[0x46] = (byte)B1(val);
				d38_reg[0x47] = (byte)B2(val);
				d38_reg[0x48] = (byte)B1(val+100);
				d38_reg[0x49] = (byte)B2(val+100);
				m_idx++;
			}
			for (int i = 0; i < len; i++) {
				buf[i] = d38_reg[adr+i];
			}
		}
		static public void D38_WR(int adr, byte[] buf, int len)
		{
		}
		public delegate void DLG_VOID_BYTEA(int adr, byte[] buf, int len);
		public class dbg_dev {
		//	uint bus;
		//	uint dev;
		// Auto-implemented properties.
			public int bus { get; set; }
			public int dev { get; set; }
			public DLG_VOID_BYTEA RD;
			public DLG_VOID_BYTEA WR;
			public dbg_dev(){}
			public dbg_dev(int bus, int dev) {
				this.bus = bus;
				this.dev = dev;
			}
		};
		static List<dbg_dev> dev_ps8 = new  List<dbg_dev> {
			new dbg_dev {bus=0x00,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x01,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x02,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x03,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x10,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x11,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x12,dev=0x38,RD=D38_RD,WR=D38_WR},
			new dbg_dev {bus=0x13,dev=0x38,RD=D38_RD,WR=D38_WR},
		}; 
		static private
		byte[]	RET_BUF = new byte[64];
		static void CMD_GET_I2C_RED(byte[] buf)
		{
			int bus = buf[0];
			int dev = buf[1];
			int reg = buf[2];
			int len = buf[3];
			int ret = 0, done = 0;
			byte[] _buf = new byte[64];

			for (int i = 0; i < DBGMODE.dev_ps8.Count; i++) {
				if (DBGMODE.dev_ps8[i].bus == bus && DBGMODE.dev_ps8[i].dev == dev) {
					DBGMODE.dev_ps8[i].RD(reg, _buf, len);
					for (int j = 0; j < len; j++) {
						RET_BUF[2+j] = _buf[j];
					}
					ret = 1;
					done = len;
					break;
				}
			}
			RET_BUF[0] = (byte)ret;
			RET_BUF[1] = (byte)done;
		}
		static void CMD_GET_I2C_WRT(byte[] buf)
		{
			int bus = buf[0];
			int dev = buf[1];
			int reg = buf[2];
			int len = buf[3];
			int ret = 0, done = 0;
			byte[] _buf = new byte[64];

			for (int i = 0; i < DBGMODE.dev_ps8.Count; i++) {
				if (DBGMODE.dev_ps8[i].bus == bus && DBGMODE.dev_ps8[i].dev == dev) {
					DBGMODE.dev_ps8[i].WR(reg, _buf, len);
					ret = 1;
					done = len;
					break;
				}
			}
			RET_BUF[0] = (byte)ret;
			RET_BUF[1] = (byte)done;
		}
		static public void WRITE_HID(byte[] buf)
		{
			int cmd = buf[0];
			byte[] _buf = new byte[64];

			System.Array.Copy(buf, 1, _buf, 0, buf.Length-1);

			switch (cmd) {
			case D.CMD_GET_I2C_RED:
				CMD_GET_I2C_RED(_buf);
			break;
			case D.CMD_GET_I2C_WRT:
				CMD_GET_I2C_WRT(_buf);
			break;
			case D.CMD_GET_PIO_BIT:
				// 15秒おきにHI/LOを繰り返し
				RET_BUF[0] = (Environment.TickCount % 30000) >= 15000 ? (byte)1: (byte)0;
				break;
			/*
			case D.CMD_SET_PLM_POS:
				break;
			case D.CMD_GET_PLM_STS:
				RET_BUF[0] = (0);//MSB
				RET_BUF[1] = (1);
				RET_BUF[2] = (2);
				RET_BUF[3] = (3);//LSB
				break;
			case D.CMD_SET_PLM_ORG:
				break;
			case D.CMD_SET_PLM_REL:
				m_event_dev.Set();
				break;
			case D.CMD_SET_PLM_JOG:
				m_event_dev.Set();
				break;
			case D.CMD_SET_PLM_STP:
				m_event_dev.Set();
				break;*/
			default:
				m_event_dev.Set();
				break;
			}
		}
		static public void READ_HID(byte[] cmd)
		{
			if (cmd != null) {
				int l = cmd.Length;
				for (int i = 0; i < l && i < RET_BUF.Length; i++) {
					cmd[i] = RET_BUF[i];
					if (i >= 4) {
						i = i;
						//G.mlog("READ_HID:length >= 4!!!");
					}
				}
			}
		}

		static private void THRD_OF_DEVICE()
		{
			while (true) {
				if (true/*PLM_MOD[0] == 0 && PLM_MOD[1] == 0 && PLM_MOD[2] == 0 && PLM_MOD[3] == 0*/) {
					while (m_event_dev.WaitOne(/*250*/) == false) {
						Thread.Sleep(0);
						if (m_exit_req) {
							break;
						}
					}
				}
				else {
					if (true) {
						Thread.Sleep(0);
					}
				}
				if (m_exit_req) {
					break;
				}
				//---
				int tic = Environment.TickCount;
				//---
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace sLOGGER
{
    class D
    {
#if true
		[DllImport("USBHIDHELPER32.DLL", EntryPoint = "HID_ENUM")]
		static extern int _HID_ENUM(uint vid, uint pid, IntPtr pcnt);

		[DllImport("USBHIDHELPER32.DLL")]
		static extern int HID_OPEN(uint vid, uint pid, uint did);
    
		[DllImport("USBHIDHELPER32.DLL")]
		static extern int HID_CLOSE();
    
		[DllImport("USBHIDHELPER32.DLL")]
		static extern int WRITE_HID(byte[] pbuf, int size);
    
		[DllImport("USBHIDHELPER32.DLL")]
		static extern int READ_HID(byte[] pbuf, int size);
		/*
		BOOL APIENTRY HID_ENUM(DWORD vid, DWORD pid, LPDWORD pcnt);
		BOOL APIENTRY HID_OPEN(DWORD vid, DWORD pid, DWORD did);
		BOOL APIENTRY HID_CLOSE(void);
		BOOL APIENTRY WRITE_HID(LPBYTE pbuf, DWORD size);
		BOOL APIENTRY READ_HID(LPBYTE pbuf, DWORD size);
		*/
#endif
	    /**/
	    public const int CMD_GET_I2C_RED				= 0x40;
	    public const int CMD_GET_I2C_WRT				= 0x41;
	    public const int CMD_SET_I2C_BUS				= 0x42;
	    public const int CMD_GET_I2C_STS				= 0x43;
		public const int CMD_SET_CUR_STS                = 0x44;
	    public const int CMD_GET_ECHOBAC				= 0x01;
	    public const int CMD_GET_BTN_STS			 	= 0x10;
	    public const int CMD_SET_LED_STS			 	= 0x11;
	    public const int CMD_SET_PIO_DIR			 	= 0x12;
	    public const int CMD_SET_PIO_BIT				= 0x13;
	    public const int CMD_GET_PIO_BIT				= 0x14;
	    public const int CMD_GET_VERSION				= 0x02;
	    //---
	    private static bool m_access = false;
	    //---
		static public int DEV_TYPE;	//0:mini, 1:汎用
	
	    static private int MAKELONG(int w1, int w2) {
	    // TODO 自動生成されたメソッド・スタブ
		    return((0xffff & w1) << 16 | (0xffff & w2));
	    }
	    static private int MAKELONG(int b4, int b3, int b2, int b1) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((0xff & b4) << 24 | (0xff & b3) << 16 | (0xff & b2) << 8 | (0xff & b1));
	    }
	    static private int MAKEWORD(int b2, int b1) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((0xff & b2) << 8 | (0xff & b1));
	    }
	    static private int WH(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((short)((l & 0xffff0000) >> 16));
	    }
	    /*private int WL(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((short)((l & 0x0000ffff) >> 0));
	    }*/
	    static private int B4(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((byte)((l & 0xff000000) >> 24));
	    }
	    static private int B3(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((byte)((l & 0x00ff0000) >> 16));
	    }
	    static private int B2(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((byte)((l & 0x0000ff00) >> 8));
	    }
	    static private int B1(int l) {
	    // TODO 自動生成されたメソッド・スタブ
		    return ((byte)((l & 0x000000ff) >> 0));
	    }
		static private int HID_ENUM(uint vid, uint pid, out int pcnt)
		{
#if true//2019.08.23()
			if ((G.AS.DEBUG_MODE & 1) != 0) {
				DBGMODE.HID_ENUM(vid, pid, out pcnt);
				return(1);
			}
#endif
			int		ret;
			IntPtr	buf = new IntPtr();

			buf = Marshal.AllocHGlobal(4);
			ret = _HID_ENUM(vid, pid, buf); ;
			pcnt = Marshal.ReadInt32(buf);
			Marshal.FreeHGlobal(buf);

			return (ret);
		}

		static public bool INIT() {
			int cnt_of_mini;
			int cnt_of_gene;
			int ret1, ret2;
			// 0x04D8, // Vendor ID
			// 0x003F, // Product ID(SAMPLE.PROGRAM)
			// 0xEF22, // Product ID(SENSOR.BIG)
			// 0xEED8, // Product ID(SENSOR.MINI)
			ret1 = HID_ENUM(0x04D8, 0xEED8, out cnt_of_mini);
			ret2 = HID_ENUM(0x04D8, 0xEF22, out cnt_of_gene);
			if (ret1 != 0 && cnt_of_mini > 0) {
#if true//2019.08.23()
				if ((G.AS.DEBUG_MODE & 1) != 0) {
					if (DBGMODE.HID_OPEN(0x04D8, 0xEED8, 0) == 0) {
						G.mlog("ERROR @ HID_OPEN");
						return(false);
					}
				}
				else
#endif
				if (HID_OPEN(0x04D8, 0xEED8, 0) == 0) {
					G.mlog("ERROR @ HID_OPEN");
					return (false);
				}
				DEV_TYPE = 0;
			}
			else
			if (ret2 != 0 && cnt_of_gene > 0) {
#if true//2019.08.23()
				if ((G.AS.DEBUG_MODE & 1) != 0) {
					if (DBGMODE.HID_OPEN(0x04D8, 0xEF22, 0) == 0) {
						G.mlog("ERROR @ HID_OPEN");
						return(false);
					}
				}
				else
#endif
				if (HID_OPEN(0x04D8, 0xEF22, 0) == 0) {
					G.mlog("ERROR @ HID_OPEN");
					return (false);
				}
				DEV_TYPE = 1;
			}
			else {
				G.mlog("CAN NOT DETECT KOP SENSOR DEVICE");
				return(false);
			}
			m_access = true;
			return (true);
		}
		static public void SET_I2C_BUS(int busadr, int busenb, int busspd, int wait01, int wait02) {
			byte[] ibuf = {0,0};

			CMDOUT05(CMD_SET_I2C_BUS, busadr, busenb, busspd, wait01, wait02);
		}
		static public void GET_I2C_STS(int busadr, out int con, out int sts) {

			byte[] buf = new byte[4];
			CMDOUT(CMD_GET_I2C_STS, busadr, buf);

			con = MAKEWORD(buf[0], buf[1]);
			sts = MAKEWORD(buf[2], buf[3]);
			return;
		}
		static public void GET_I2C_WRT(int busadr, int devadr, int regadr, int len, byte[] obuf, out int ret, out int done) {
			byte[] ibuf = {0,0};

			CMDOUT64(CMD_GET_I2C_WRT, busadr, devadr, regadr, len, obuf, ibuf);

			ret = ibuf[0];
			done = ibuf[1];
		}
		static public void GET_I2C_RED(int busadr, int devadr, int regadr, int len, out byte[] ibuf, out int ret, out int done) {
			byte[] buf = new byte[2+len];
			ibuf = new byte[len];

			CMDOUT64(CMD_GET_I2C_RED, busadr, devadr, regadr, len, null, buf);

			ret = buf[0];
			done = buf[1];
			if (len > 0) {
				System.Array.Copy(buf, 2, ibuf, 0, len);
			}
		}
		static public void SET_CUR_STS(int chan, int sts) {
			byte[] ibuf = {0,0};

			CMDOUT02(CMD_SET_CUR_STS, chan, sts);
		}
		
		static public int GET_SW_STS() {
			byte[] buf = new byte[5];
			CMDOUT(CMD_GET_BTN_STS, 0, buf);
			return(buf[0]);
		}
		static public void TERM() {
			if (m_access) {
 #if true//2019.08.23()
				if ((G.AS.DEBUG_MODE & 1) != 0) {
					DBGMODE.HID_CLOSE();
					m_access = false;
					return;
				}
#endif
				HID_CLOSE();
				m_access = false;
			}
		}
		static public void SET_ILED_STS(int sts) {
			CMDOUT(CMD_SET_LED_STS, sts, null);
		}
		static public void SET_PIO_DIR(int par) {
			CMDOUT(CMD_SET_PIO_DIR, par, null);
		}
		static public void SET_PIO_BIT(int par) {
			CMDOUT(CMD_SET_PIO_BIT, par, null);
		}
		static public int GET_PIO_BIT() {
			byte[] buf = new byte[5];
			CMDOUT(CMD_GET_PIO_BIT, 0, buf);
			return(buf[0]);
		}
		static public int GET_VERSION() {
		byte[] buf = new byte[5];
			CMDOUT(CMD_GET_VERSION, 0, buf);
			return(MAKELONG(buf[3], buf[2], buf[1], buf[0]));
		}
		static public bool isCONNECTED() {
			return(m_access);
		}
		/************************************************************/
		static public bool CMDOUT(int cmd, int par1, byte[] buf)
		{
			return(CMDOUT(cmd, par1, 0, 0, 0, buf));
		}
		/************************************************************/
		static public bool CMDOUT(int cmd, int par1, int par2, byte[] buf)
		{
			return(CMDOUT(cmd, par1, par2, 0, 0, buf));
		}
		/************************************************************/
		static public bool CMDOUT(int cmd, int par1, int par2, int par3, byte[] buf)
		{
			return(CMDOUT(cmd, par1, par2, par3, 0, buf));
		}
		/************************************************************/
		static public bool CMDOUT(int cmd, int par1, int par2, int par3, int par4, byte[] buf)
		{
			byte[] commandPacket = new byte[16];
			commandPacket[0] = (byte)cmd;
			commandPacket[1] = (byte)par1;
			commandPacket[2] = (byte)par2;
			commandPacket[3] = (byte)par3;
			commandPacket[4] = (byte)par4;
#if true//2019.08.23()
			if ((G.AS.DEBUG_MODE & 1) != 0) {
				DBGMODE.WRITE_HID(commandPacket);
				if (buf != null) {
				DBGMODE.READ_HID(buf);
				}
				return(true);
			}
#endif
			if (WRITE_HID(commandPacket, commandPacket.Length) == 0) {
		//			Toast.makeText(TEST24Activity.this, "USB COMMUNICATION ERROR!!!", Toast.LENGTH_SHORT).show();
			//	return(false);
			}
			if (buf != null) {
				if (READ_HID(commandPacket, commandPacket.Length) == 0)
				{
					return(false);
				}
				for (int i = 0; i < commandPacket.Length; i++)
				{
					if (i >= buf.Length)
					{
						break;
					}
					buf[i] = commandPacket[i];
				}
			}
			return(true);
		}
		/************************************************************/
		static public bool CMDOUT05(int cmd, int par1, int par2, int par3, int par4, int par5)
		{
			byte[] commandPacket = new byte[16];
			commandPacket[0] = (byte)cmd;
			commandPacket[1] = (byte)par1;
			commandPacket[2] = (byte)par2;
			commandPacket[3] = (byte)par3;
			commandPacket[4] = (byte)par4;
			commandPacket[5] = (byte)par5;
#if true//2019.08.23()
			if ((G.AS.DEBUG_MODE & 1) != 0) {
				DBGMODE.WRITE_HID(commandPacket);
				return(true);
			}
#endif
			if (WRITE_HID(commandPacket, commandPacket.Length) == 0) {
		//			Toast.makeText(TEST24Activity.this, "USB COMMUNICATION ERROR!!!", Toast.LENGTH_SHORT).show();
			//	return(false);
			}
			return(true);
		}
		/************************************************************/
		static public bool CMDOUT02(int cmd, int par1, int par2)
		{
			byte[] commandPacket = new byte[16];
			commandPacket[0] = (byte)cmd;
			commandPacket[1] = (byte)par1;
			commandPacket[2] = (byte)par2;
			commandPacket[3] = 0;
			commandPacket[4] = 0;
			commandPacket[5] = 0;
#if true//2019.08.23()
			if ((G.AS.DEBUG_MODE & 1) != 0) {
				DBGMODE.WRITE_HID(commandPacket);
				return(true);
			}
#endif
			if (WRITE_HID(commandPacket, commandPacket.Length) == 0) {
		//			Toast.makeText(TEST24Activity.this, "USB COMMUNICATION ERROR!!!", Toast.LENGTH_SHORT).show();
			//	return(false);
			}
			return(true);
		}
		/************************************************************/
		static public bool CMDOUT64(int cmd, int par1, int par2, int par3, int par4, byte[] obuf, byte[] ibuf)
		{
			byte[] commandPacket = new byte[64];
			commandPacket[0] = (byte)cmd;
			commandPacket[1] = (byte)par1;
			commandPacket[2] = (byte)par2;
			commandPacket[3] = (byte)par3;
			commandPacket[4] = (byte)par4;
			if (obuf != null) {
				for (int i = 0; i < obuf.Length; i++) {
					commandPacket[5+i] = obuf[i];
				}
			}
#if true//2019.08.23()
			if ((G.AS.DEBUG_MODE & 1) != 0) {
				DBGMODE.WRITE_HID(commandPacket);
				if (ibuf != null) {
				DBGMODE.READ_HID(ibuf);
				}
				return(true);
			}
#endif
			if (WRITE_HID(commandPacket, commandPacket.Length) == 0) {
			//	return(false);
			}
			if (ibuf != null) {
				if (READ_HID(commandPacket, commandPacket.Length) == 0) {
					return(false);
				}
				for (int i = 0; i < commandPacket.Length; i++) {
					if (i >= ibuf.Length) {
						break;
					}
					ibuf[i] = commandPacket[i];
				}
			}
			return(true);
		}
    }
}

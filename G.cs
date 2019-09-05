using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//-----------------------
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;

namespace sLOGGER
{
	public class G
	{
		public delegate void DLG_VOID_VOID();
		public delegate void DLG_VOID_BOOL(bool b);
		public delegate void DLG_VOID_INT(int n);
		public delegate bool DLG_BOOL_OBJECTS(out object[] obj);
		public delegate void DLG_VOID_OBJECTS(out object[] obj);
		public delegate void DLG_VOID_OBJECT_EVARG(object obj, EventArgs e);
		public delegate bool DLG_BOOL_VOID();
		public class APPSET : System.ICloneable
		{
			public int TRACE_LEVEL = 0;
			public int DEBUG_MODE = 0;
			public int APP_F01_LFT = 700;
			public int APP_F01_TOP =   5;
			public int APP_F02_LFT =  10;
			public int APP_F02_TOP =   5;
			public int APP_F02_WID = 600;
			public int APP_F02_HEI = 800;
			public string AUT_BEF_PATH = null;
			public string BEFORE_PATH = null;
		public Object Clone()
			{
				APPSET cln = (APPSET)this.MemberwiseClone();
				return (cln);
			}
			public bool load(ref APPSET ss)
			{
				string path = GET_DOC_PATH("sLOGGER.xml");
				bool ret = false;
				try {
					XmlSerializer sz = new XmlSerializer(typeof(APPSET));
					System.IO.StreamReader fs = new System.IO.StreamReader(path, System.Text.Encoding.Default);
					APPSET obj;
					obj = (APPSET)sz.Deserialize(fs);
					fs.Close();
					obj = (APPSET)obj.Clone();
					ss = obj;
					ret = true;
				}
				catch (Exception /*ex*/) {
				}
				return(ret);
			}
			//
			public bool save(APPSET ss)
			{
				string path = GET_DOC_PATH("sLOGGER.xml");
				bool ret = false;
				try {
					XmlSerializer sz = new XmlSerializer(typeof(APPSET));
					System.IO.StreamWriter fs = new System.IO.StreamWriter(path, false, System.Text.Encoding.Default);
					sz.Serialize(fs, ss);
					fs.Close();
					ret = true;
				}
				catch (Exception /*ex*/) {
				}
				return (ret);
			}
		}
		public class SYSSET : System.ICloneable
		{
			public SENSOR_TBL[]
						SEN_TBL = null;
			public int AD1_DTR_IDX = 1;		//± 1.024 V(筋電)
			public int AD1_SPS_IDX = 4;		//128 SPS
			public int AD2_DTR_IDX = 0;		//± 2.048 V(圧力)
			public int AD2_SPS_IDX = 4;		//128 SPS
			public int POT_WIP_VAL = 64;	//(D.POT)
#if true
			public int D38_410_MSTM = 6;	//Mes.Time(0:ALS/PS=STB/STB,...,6:100ms/100ms, 7:100,400ms,...,12:50/50ms)
			public int D38_424_ALG1 = 1;	//Gain(0:x1, 1:x2, 2:x64, 3:x128)
			public int D38_422_ALG2 = 1;	//Gain(0:x1, 1:x2, 2:x64, 3:x128)
			public int D38_420_LEDC = 0;	//LED.Current(0:25, 1:50, 2:100, 3:200mA)
			public int D38_434_PSG0 = 1;	//Gain(0:x1, 1:x2, 2:x4)
#endif
#if true
			public int MES_INT_MSEC = 200;
			public int MES_WID_SECS =  60;
			//---
			public int SCL_MIN_D38 = -500;
			public int SCL_MAX_D38 = 5000;
			public int THD_VAL_PS  = 2500;
			public string THD_OVR_STR = "OK";
			public string THD_LES_STR = "NG";
			public int THD_FNT_SIZ = 36;
#endif
#if true//2019.09.06(近接時間測定)
			public bool THD_MES_TIM = true;
#endif
			public int		GRP_ROW_CNT = 2;
			public int		GRP_COL_CNT = 4;
			//---
			public Object Clone()
			{
				SYSSET cln = (SYSSET)this.MemberwiseClone();
				if (this.SEN_TBL != null) {
				cln.SEN_TBL  =(SENSOR_TBL[])this.SEN_TBL.Clone();
				}
				return (cln);
			}
			//
			public bool load(ref SYSSET ss)
			{
				string path = GET_SET_PATH();
				bool ret = false;
				try {
					XmlSerializer sz = new XmlSerializer(typeof(SYSSET));
					System.IO.StreamReader fs = new System.IO.StreamReader(path, System.Text.Encoding.Default);
					SYSSET obj;
					obj = (SYSSET)sz.Deserialize(fs);
					fs.Close();
					obj = (SYSSET)obj.Clone();
					ss = obj;
					ret = true;
				}
				catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
				return(ret);
			}
			//
			public bool save(SYSSET ss)
			{
				string path = GET_SET_PATH();
				bool ret = false;
				try {
					XmlSerializer sz = new XmlSerializer(typeof(SYSSET));
					System.IO.StreamWriter fs = new System.IO.StreamWriter(path, false, System.Text.Encoding.Default);
					sz.Serialize(fs, ss);
					fs.Close();
					ret = true;
				}
				catch (Exception ex) {
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
				return (ret);
			}
			static private string GET_SET_PATH()
			{
				string path;
				path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				path += @"\KOP";
				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}
				path += @"\" + Application.ProductName;
				if (!System.IO.Directory.Exists(path)) {
					System.IO.Directory.CreateDirectory(path);
				}
				path += @"\settings.xml";

				return (path);
			}
		};
		public class DEVICE_TBL/*:System.ICloneable*/
		{
			public int	BID;	//バスID(アドレス: 0x00,0x01,,0x10)
			public int	DID;	//デバイスID(アドレス: 0x36,0x68,0x76,...)
			public int	CNT_OF_SENS;
			public SENSOR_TBL[]
						SEN;
			public G.DLG_BOOL_OBJECTS
						FUN;
			public G.DLG_BOOL_VOID
						INI;
#if false//2019.08.23
			public int SID;	//sensor id
			public int GID;		//graph id
			public string NAME;	//ACCEL.X, GYRO.X,...TEMP.,HUMIDITY
			public string UNIT;	//[g/s],[deg/s],...,[degC],[%]
			//public int PREC;	//小数点以下桁数
			//public double DATA;	//センサーデータ
			public double SMAX;
			public double SMIN;
#endif
		}
		public class SENSOR_TBL:System.ICloneable
		{
			//public bool HIST_RECT;
			//public int[]	BUS;
			//public int[]	DEV;
			[System.Xml.Serialization.XmlIgnore]
			public DEVICE_TBL
							_DEV;	//センサーデバイス
			public string	NAME;	//ACCEL.X, GYRO.X,...TEMP.,HUMIDITY
			public string	UNIT;	//[g/s],[deg/s],...,[degC],[%]
			public int		G_ID;	//0:NONE, 1:CHART1-LEFT, 2:CHART2:RIGHT,...
#if true//2019.08.23
			public double SMAX;
			public double SMIN;
#endif
			//---
			//---
			public int		PREC;	//小数点以下桁数
			public double	DATA;	//センサーデータ
			//---
			[System.Xml.Serialization.XmlIgnore]
			public object	OBJE;	//チャート・シリーズを格納
			//---
			public Object Clone()
			{
				SYSSET cln = (SYSSET)this.MemberwiseClone();
				return (cln);
			}
			public void clear()
			{
			}
		}
#if false//2019.08.23
		public enum PLM_STS_BITS
		{
			BIT_ONMOV = 0x0001,	//移動中
			BIT______ = 0x0002,	//
			BIT_LMT_M = 0x0004,	//現在LIMIT.MがON(SOFT.LIMIT.CCW側)
			BIT_END_N = 0x0008,	//正常動作にて終了
			BIT_END_A = 0x0010,	//リミット or STOP.REQ or原点未検出にて終了
			BIT_LMT_H = 0x0020,	//現在LIMIT.HがON
			BIT_LMT_P = 0x0040,	//現在LIMIT.PがON(SOFT.LIMIT.CW側)
			BIT_ORGOK = 0x0080,	//ORG検出実行済み
			//	BIT_START = 0x0080	//マイコン側処理用
			BIT_ACCEL = 0x0100,	//加速中
			BIT_SLOWS = 0x0200,	//減速中
		};
		public enum CAM_STS
		{
			STS_NONE = 0,
			STS_HIST = 1,
			STS_HAIR = 2,
			STS_FCUS = 4,
			STS_AUTO = 5,
			STS_ATIR = 6,
			STS_CUTI = 7,
		};
#endif
		//---
		static public APPSET AS = new APPSET();
		static public SYSSET SS = new SYSSET();
		//static public IP_RESULT	IR = new IP_RESULT();
		static public DEVICE_TBL[] DT = null;
		//---	
		//static public bool		bDEBUG=false;
		//static public bool		bONLINE=false;
		//static public bool		bONLINE_OF_NI=false;
		static public bool		bCANCEL=false;
		static public Form01	FORM01 = null;
		static public Form02	FORM02 = null;
		//static public Form03	FORM03 = null;
		//static public Form10	FORM10 = null;
		//static public Form11	FORM11 = null;
		//static public Form12	FORM12 = null;
#if false//2019.08.23
		static public int PLM_STS = 0;
		static public int[] PLM_POS = { 0, 0, 0, 0 };
		static public byte[] PLM_STS_BIT = new byte[16];
		//static public int CAM_STS;
		static public CAM_STS CAM_PRC = CAM_STS.STS_NONE;

		//static public int AUT_STS;
		//static public int MOK_STS;
		static public int CAM_WID;
		static public int CAM_HEI;
#endif
		static public int LED_PWR_STS;
#if false//2019.08.23
		static public int CAM_GAI_STS=2;//0:固定, 1:自動, 2:不定
		static public int CAM_EXP_STS=2;//0:固定, 1:自動, 2:不定
		static public int CAM_WBL_STS=2;//0:固定, 1:自動, 2:不定
		static public bool bJITAN=false;
		static public int CNT_MOD;
#endif
		//-----------------------
		static public DialogResult mlog(string str)
		{
			return(mlog(str, Form.ActiveForm));
		}
		//-----------------------
		static public DialogResult mlog(string str, Form frm)
        {
			MessageBoxIcon	icons = MessageBoxIcon.Exclamation;
			MessageBoxButtons
							butns = MessageBoxButtons.OK;
			DialogResult	rc;
			/**/
			 
			if (frm == null) {
				if (G.FORM01.Visible) {
					frm = G.FORM01;
				}
				else if (G.FORM02.Visible) {
					frm = G.FORM02;
				}
			}
			/**/
			if (str.Length > 0 && str[0]== '#') {
				switch (char.ToLower(str[1])) {
				case 's':
					icons = MessageBoxIcon.Stop;
				break;
				case 'q':
					icons = MessageBoxIcon.Question;
					if (str[1] == 'q') {// small q
						butns = MessageBoxButtons.YesNo;
					}
					else {				// large Q
						butns = MessageBoxButtons.OKCancel;
					}
				break;
				case 'c':
					icons = MessageBoxIcon.Question;
					butns = MessageBoxButtons.YesNoCancel;
				break;
				case 'i':
					icons = MessageBoxIcon.Information;
				break;
				case 'e':
					icons = MessageBoxIcon.Exclamation;
				break;
				default:
				break;
				}
				str = str.Substring(2);
			}
			using (new CenterWinDialog(frm)) {
				rc = MessageBox.Show(G.FORM01, str, Application.ProductName, butns, icons);
			}
			return(rc);
        }
		static public void lerr(string str)
		{
			mlog("internal error %s %d");//(, __FILE__, __LINE__);
		}
#if false
#endif
		static public string GET_DOC_PATH(string file)
		{
			string path;
			path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			path += @"\KOP";
			if (!System.IO.Directory.Exists(path)) {
				System.IO.Directory.CreateDirectory(path);
			}
			path += @"\" + Application.ProductName;
			if (!System.IO.Directory.Exists(path)) {
				System.IO.Directory.CreateDirectory(path);
			}
			if (!string.IsNullOrEmpty(file)) {
				if (file[0] != '\\') {
					path += "\\";
				}
				path += file;
			}

			return (path);
		}
	}
}

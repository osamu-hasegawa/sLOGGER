using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//---
using System.Collections;

namespace sLOGGER
{
	public partial class Form01 : Form
	{
		int MAX_OF_DEV = 5;
		int[] BUSADR = {0x00, 0x01, 0x02, 0x03, 0x10};
		string[] BUSSTR = {"BUS:0Xh", "BUS:1Xh"};

		D76_BME280[] bme280 = null;//new D76_BME280(0);
		D38_RPR0521RS[] rpr0521 = null;//new D38_RPR0521RS(0);
		D68_MPU9250[] mpu9250 = null;//new D68_MPU9250(0);
		D29_TCS34725[] tcs34725 = null;
		D48_ADS1114[] ads1114_48h = null;
		D48_ADS1114[] ads1114_49h = null;
		D28_MCP4661[] mcp4661 = null;

		public Form01()
		{
			InitializeComponent();
		}
		private void POST_INIT()
		{
			//mini基板で最大5ヶのデバイス(BUS数5),汎用基盤で最大10ヶ
			bme280 = new D76_BME280[MAX_OF_DEV];
			rpr0521 = new D38_RPR0521RS[MAX_OF_DEV];
			mpu9250 = new D68_MPU9250[MAX_OF_DEV];
			tcs34725 = new D29_TCS34725[MAX_OF_DEV];
			ads1114_48h = new D48_ADS1114[MAX_OF_DEV];
			ads1114_49h = new D48_ADS1114[MAX_OF_DEV];
			mcp4661 = new D28_MCP4661[MAX_OF_DEV];

			this.comboBox7.Items.Clear();
			//this.comboBox8.Items.Clear();
			foreach (int i in BUSADR) {
				string tmp = string.Format("{0:X02}", i);
				this.comboBox7.Items.Add(tmp);
				//this.comboBox8.Items.Add(tmp);
			}
			this.comboBox7.SelectedIndex = 0;
			//this.comboBox8.SelectedIndex = 0;
			//---
			this.comboBox5.Items.Clear();
			this.comboBox6.Items.Clear();
			foreach (string s in BUSSTR) {
				this.comboBox5.Items.Add(s);
				this.comboBox6.Items.Add(s);
			}
			this.comboBox5.SelectedIndex = 0;
			this.comboBox6.SelectedIndex = 0;
			//---
		}
		private void Form01_Load(object sender, EventArgs e)
		{
			//---
			G.AS.load(ref G.AS);
			G.SS.load(ref G.SS);
			//---
			this.timer1.Interval = 500;
			this.button2.Enabled = false;
			this.comboBox2.Items.Clear();
			this.comboBox3.Items.Clear();
			this.comboBox4.Items.Clear();
			this.comboBox9.Items.Clear();
			this.comboBox10.Items.Clear();
			this.comboBox5.SelectedIndex = 0;
			this.comboBox6.SelectedIndex = 0;
			//---
			this.textBox6.Text = "";
			this.textBox23.Text = "";
			//---
			this.comboBox11.SelectedIndex = G.SS.AD1_DTR_IDX;//± 2.048 V
			this.comboBox12.SelectedIndex = G.SS.AD1_SPS_IDX;
			//---
			this.comboBox14.SelectedIndex = G.SS.AD2_DTR_IDX;//± 2.048 V
			this.comboBox15.SelectedIndex = G.SS.AD2_SPS_IDX;
			//---
			this.numericUpDown12.Value = G.SS.POT_WIP_VAL;
			//---
			G.FORM01 = this;
			POST_INIT();
		}
		private void Form01_FormClosing(object sender, FormClosingEventArgs e)
		{
			G.SS.AD1_DTR_IDX = this.comboBox11.SelectedIndex;
			G.SS.AD1_SPS_IDX = this.comboBox12.SelectedIndex;
			G.SS.AD2_DTR_IDX = this.comboBox14.SelectedIndex;
			G.SS.AD2_SPS_IDX = this.comboBox15.SelectedIndex;
			G.SS.POT_WIP_VAL = (int)this.numericUpDown12.Value;
			//---
			G.AS.save(G.AS);
			G.SS.save(G.SS);
			if (D.isCONNECTED()) {
				D.TERM();
			}
			G.FORM01 = null;
			if (G.FORM02 != null) {
			G.FORM02.Close();
			}
		}
		public void OnClicks(object sender, EventArgs e)
		{
			if (false) {
			}
			else if (sender == this.button1) {
#if false
				Form20 frm = new Form20();
				frm.ShowDialog(this);
#else
				if (D.INIT()) {
					 this.button1.Enabled = false;
					 this.button2.Enabled = true;
		 			 this.button3.Enabled = true;
		 			 this.button4.Enabled = true;
		 			 this.button5.Enabled = true;
					 if (D.DEV_TYPE == 0) {
						this.BUSADR = new int[] {0x00, 0x01, 0x02, 0x03, 0x10};
						this.BUSSTR = new string[] {"BUS:0Xh", "BUS:1Xh"};
						this.MAX_OF_DEV = 5;
						this.button21.Enabled = false;
						this.button20.Enabled = false;
					}
					else {
						this.BUSADR = new int[] {0x00, 0x01, 0x02, 0x03, 0x10, 0x11, 0x12, 0x13, 0x20, 0x30};
						this.BUSSTR = new string[] {"BUS:0Xh", "BUS:1Xh", "BUS:2Xh", "BUS:3Xh"};
						this.MAX_OF_DEV = 10;
						this.button21.Enabled = true;
						this.button20.Enabled = false;
					}
					POST_INIT();
				}
#endif
			}
			else if (sender == this.button2) {
				D.TERM();
				this.button1.Enabled = true;
				this.button2.Enabled = false;
		 		this.button3.Enabled = false;
		 		this.button4.Enabled = false;
		 		this.button5.Enabled = false;
			}
			else if (sender == this.button15) {
				if (G.FORM02 == null) {
				G.FORM02 = new Form02();
				}
				G.FORM02.Show();
				this.Hide();
			}
			else if (sender == this.checkBox1
			 || sender == this.checkBox2
			 || sender == this.checkBox3
			 || sender == this.checkBox4) {
				int sts = 0;
				sts |= this.checkBox1.Checked ? 1:0;
				sts |= this.checkBox2.Checked ? 2:0;
				sts |= this.checkBox3.Checked ? 4:0;
				sts |= this.checkBox4.Checked ? 8:0;
				D.SET_ILED_STS(sts);
			}
			else if (sender == this.checkBox5) {
				if (this.checkBox5.Checked) {
					this.button3.Enabled = false;
					this.timer1.Enabled = true;
				}
				else {
					this.timer1.Enabled = false;
					this.button3.Enabled = true;
				}
			}
			else if (sender == this.button3) {
				int ret = D.GET_SW_STS();
				if ((ret & 1)!=0) {
					this.label1.BackColor = Color.LimeGreen;
				}
				else {
					this.label1.BackColor = Color.DarkGray;
				}
				if ((ret & 2)!=0) {
					this.label2.BackColor = Color.LimeGreen;
				}
				else {
					this.label2.BackColor = Color.DarkGray;
				}
				if ((ret & 4)!=0) {
					this.label49.BackColor = Color.LimeGreen;
				}
				else {
					this.label49.BackColor = Color.DarkGray;
				}
			}
			else if (sender == this.button4) {
				i2c_read();
			}
			else if (sender == this.button5) {
				i2c_write();
			}
			else if (sender == this.button6) {
				int i;
				if ((i = ba2i(this.comboBox2)) >= 0) {
					bme280[i].INIT();
				}
			}
			else if (sender == this.button7) {
				get_bme280();
			}
			else if (sender == this.checkBox6) {
				if (this.checkBox6.Checked) {
					this.timer2.Enabled = true;
				}
				else {
					this.timer2.Enabled = false;
				}
			}
			else if (sender == this.button8) {
				int i;
				if ((i = ba2i(this.comboBox3)) >= 0) {
					rpr0521[i].INIT();
				}
			}
			else if (sender == this.button9) {
				get_rpr0521();
			}
			else if (sender == this.checkBox7) {
				if (this.checkBox7.Checked) {
					this.timer3.Enabled = true;
				}
				else {
					this.timer3.Enabled = false;
				}
			}
			//---------------------------------------------------
			else if (sender == this.button10) {
				int i;
				if ((i = ba2i(this.comboBox4)) >= 0) {
					mpu9250[i].INIT();
				}
			}
			else if (sender == this.button11) {
				get_mpu9250();
			}
			else if (sender == this.checkBox8) {
				if (this.checkBox8.Checked) {
					this.timer4.Enabled = true;
				}
				else {
					this.timer4.Enabled = false;
				}
			}
			//----------------------------------------------------
			else if (sender == this.button16) {
				int i;
				if ((i = ba2i(this.comboBox9)) >= 0) {
					tcs34725[i].INIT();
				}
			}
			else if (sender == this.button17) {
				get_tcs34725();
			}
			else if (sender == this.checkBox11) {
				if (this.checkBox11.Checked) {
					this.timer5.Enabled = true;
				}
				else {
					this.timer5.Enabled = false;
				}
			}
			//----------------------------------------------------
			else if (sender == this.button18) {
				int i;
				if ((i = ba2i(this.comboBox10)) >= 0) {
					int pga = this.comboBox11.SelectedIndex;
					int sps = this.comboBox12.SelectedIndex;
					ads1114_48h[i].INIT(pga, sps);
				}
			}
			else if (sender == this.button19) {
				get_ads1114_48();
			}
			else if (sender == this.checkBox12) {
				if (this.checkBox12.Checked) {
					this.timer6.Enabled = true;
				}
				else {
					this.timer6.Enabled = false;
				}
			}
			//----------------------------------------------------
			else if (sender == this.button22) {
				int i;
				if ((i = ba2i(this.comboBox8)) >= 0) {
					int pga = this.comboBox14.SelectedIndex;
					int sps = this.comboBox15.SelectedIndex;
					ads1114_49h[i].INIT(pga, sps);
				}
			}
			else if (sender == this.button23) {
				get_ads1114_49();
			}
			else if (sender == this.checkBox9) {
				if (this.checkBox9.Checked) {
					this.timer7.Enabled = true;
				}
				else {
					this.timer7.Enabled = false;
				}
			}
			//----------------------------------------------------
			else if (sender == this.button12) {
				detect();
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			OnClicks(this.button3, null);
		}
		private void i2c_read()
		{
			try {
				int busadr = int.Parse(this.comboBox7.Text, System.Globalization.NumberStyles.HexNumber);
				int devadr = (int)this.numericUpDown2.Value;
				int regadr = (int)this.numericUpDown3.Value;
				int length = (int)this.numericUpDown4.Value;
				byte[] buf = new byte[length];
				int ret, done;
				string txt="";
				this.textBox6.Clear();
				this.textBox6.Update();
				
				D.GET_I2C_RED(busadr, devadr, regadr, length, out buf, out ret, out done);
				
				this.textBox4.Text = ret.ToString();
				this.textBox5.Text = done.ToString();
				for (int i = 0; i < done; i++) {
					txt += string.Format("{0:X02}\t{1:X02}\r\n", regadr+i, buf[i]);
				}
				this.textBox6.Text = txt;
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void i2c_write()
		{
			try {
				int busadr = int.Parse(this.comboBox7.Text, System.Globalization.NumberStyles.HexNumber);
				int devadr = (int)this.numericUpDown2.Value;
				int regadr = (int)this.numericUpDown3.Value;
				int[] vals = null;
				byte[] buf = null;
				 
				int ret, done;
				DDV.DDX(false, this.textBox9, ref vals, 16, 0, 255, /*bHEX*/true);

				buf = new byte[vals.Length];
				for (int i = 0; i < buf.Length; i++) {
					buf[i] = (byte)vals[i];
				}

				D.GET_I2C_WRT(busadr, devadr, regadr, buf.Length, buf, out ret, out done);
				
				this.textBox4.Text = ret.ToString();
				this.textBox5.Text = done.ToString();
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private int m_chk1, m_chk2;
		private int m_chk3, m_chk4;
		private int m_chk5, m_chk6;
		private int m_chk7, m_chk8;
		private int m_chk9, m_chk10;
		private int m_chk11, m_chk12;

		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timer2.Enabled = false;
			try {
				if (m_chk1 == 0) {
					m_chk2 = 0;
					m_chk1 = System.Environment.TickCount;
				}
				get_bme280();
				if (this.checkBox6.Checked) {
					this.timer2.Enabled = true;
				}
				m_chk2++;
				if (m_chk2 >= 10) {
					m_chk1 = System.Environment.TickCount-m_chk1;
					this.label20.Text = string.Format("{0:F1} SPS", 10.0/(m_chk1/1000.0));
					m_chk1 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void timer3_Tick(object sender, EventArgs e)
		{
			this.timer3.Enabled = false;
			try {
				if (m_chk3 == 0) {
					m_chk4 = 0;
					m_chk3 = System.Environment.TickCount;
				}
				get_rpr0521();
				if (this.checkBox7.Checked) {
					this.timer3.Enabled = true;
				}
				m_chk4++;
				if (m_chk4 >= 10) {
					m_chk3 = System.Environment.TickCount-m_chk3;
					this.label27.Text = string.Format("{0:F1} SPS", 10.0/(m_chk3/1000.0));
					m_chk3 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void timer4_Tick(object sender, EventArgs e)
		{
			this.timer4.Enabled = false;
			try {
				if (m_chk5 == 0) {
					m_chk6 = 0;
					m_chk5 = System.Environment.TickCount;
				}
				get_mpu9250();
				if (this.checkBox8.Checked) {
					this.timer4.Enabled = true;
				}
				m_chk6++;
				if (m_chk6 >= 10) {
					m_chk5 = System.Environment.TickCount-m_chk5;
					this.label35.Text = string.Format("{0:F1} SPS", 10.0/(m_chk5/1000.0));
					m_chk5 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void timer5_Tick(object sender, EventArgs e)
		{
			this.timer5.Enabled = false;
			try {
				if (m_chk7 == 0) {
					m_chk8 = 0;
					m_chk7 = System.Environment.TickCount;
				}
				get_tcs34725();
				if (this.checkBox11.Checked) {
					this.timer5.Enabled = true;
				}
				m_chk8++;
				if (m_chk8 >= 10) {
					m_chk7 = System.Environment.TickCount-m_chk7;
					this.label58.Text = string.Format("{0:F1} SPS", 10.0/(m_chk7/1000.0));
					m_chk7 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void timer6_Tick(object sender, EventArgs e)
		{
			this.timer6.Enabled = false;
			try {
				if (m_chk9 == 0) {
					m_chk10 = 0;
					m_chk9 = System.Environment.TickCount;
				}
				get_ads1114_48();
				if (this.checkBox12.Checked) {
					this.timer6.Enabled = true;
				}
				m_chk10++;
				if (m_chk10 >= 10) {
					m_chk9 = System.Environment.TickCount-m_chk9;
					this.label72.Text = string.Format("{0:F1} SPS", 10.0/(m_chk9/1000.0));
					m_chk9 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}
		private void timer7_Tick(object sender, EventArgs e)
		{
			this.timer7.Enabled = false;
			try {
				if (m_chk11 == 0) {
					m_chk12 = 0;
					m_chk11 = System.Environment.TickCount;
				}
				get_ads1114_49();
				if (this.checkBox9.Checked) {
					this.timer7.Enabled = true;
				}
				m_chk12++;
				if (m_chk12 >= 10) {
					m_chk11 = System.Environment.TickCount-m_chk11;
					this.label40.Text = string.Format("{0:F1} SPS", 10.0/(m_chk11/1000.0));
					m_chk11 = 0;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.ToString());
			}
		}

		private int ba2i(int busadr)
		{
			for (int i = 0; i < BUSADR.Length; i++) {
				if (busadr == BUSADR[i]) {
					return(i);
				}
			}
			return(-1);
		}
		private int ba2i(ComboBox cmb)
		{
			int busadr;
			string tmp = cmb.Text;
			if (tmp.Length < 2) {
				return(-1);
			}
			tmp = tmp.Substring(0, 2);
			if (string.IsNullOrEmpty(tmp)) {
				return(-1);
			}
			busadr = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
			for (int i = 0; i < BUSADR.Length; i++) {
				if (busadr == BUSADR[i]) {
					return(i);
				}
			}
			return(-1);
		}
		private void get_bme280()
		{
			double T,H,P;
			int i;
			if ((i = ba2i(this.comboBox2)) < 0) {
				return;
			}
			bme280[i].GET(out T, out H, out P);
			this.textBox12.Text = string.Format("{0:F2}", T);
			this.textBox13.Text = string.Format("{0:F2}", H);
			this.textBox14.Text = string.Format("{0:F2}", P);
		}
		private void get_rpr0521()
		{
			double L;
			int P;
			int i;
			if ((i = ba2i(this.comboBox3)) < 0) {
				return;
			}
			rpr0521[i].GET(out L, out P);
			this.textBox15.Text = string.Format("{0:F2}", L);
			this.textBox16.Text = string.Format("{0:F0}", P);
		}
		private void get_mpu9250()
		{
			double[] a = {0,0,0};
			double[] g = {0,0,0};
			int i;
			if ((i = ba2i(this.comboBox4)) < 0) {
				return;
			}
			mpu9250[i].GET(a, g);
			this.textBox17.Text = string.Format("{0:F2}", a[0]);
			this.textBox18.Text = string.Format("{0:F2}", a[1]);
			this.textBox19.Text = string.Format("{0:F2}", a[2]);
			this.textBox20.Text = string.Format("{0:F2}", g[0]);
			this.textBox21.Text = string.Format("{0:F2}", g[1]);
			this.textBox22.Text = string.Format("{0:F2}", g[2]);
			System.Threading.Thread.Sleep((int)this.numericUpDown10.Value);
		}
		private void get_tcs34725()
		{
			int R, G, B, C;
			int i;
			if ((i = ba2i(this.comboBox9)) < 0) {
				return;
			}
			tcs34725[i].GET(out R, out G, out B, out C);
			this.textBox3.Text = string.Format("{0:F0}", R);
			this.textBox7.Text = string.Format("{0:F0}", G);
			this.textBox8.Text = string.Format("{0:F0}", B);
			this.textBox24.Text = string.Format("{0:F0}", C);
			System.Threading.Thread.Sleep((int)this.numericUpDown1.Value);
		}
		private void get_ads1114_48()
		{
			int val;
			double vol;
			int	i;
			if ((i = ba2i(this.comboBox10)) < 0) {
				return;
			}
			ads1114_48h[i].GET(out val, out vol);
			this.textBox25.Text = string.Format("{0:F0}", val);
			this.textBox26.Text = string.Format("{0:F3}", vol);
			System.Threading.Thread.Sleep((int)this.numericUpDown5.Value);
		}
		private void get_ads1114_49()
		{
			int val;
			double vol;
			int	i;
			if ((i = ba2i(this.comboBox8)) < 0) {
				return;
			}
			ads1114_49h[i].GET(out val, out vol);
			this.textBox10.Text = string.Format("{0:F0}", val);
			this.textBox11.Text = string.Format("{0:F3}", vol);
			System.Threading.Thread.Sleep((int)this.numericUpDown6.Value);
		}

		private void scroll_to_end(TextBox tb, string txt)
		{
			tb.Text = txt;
			tb.SelectionStart = tb.Text.Length;
			tb.Focus();
			tb.ScrollToCaret();
			tb.Update();
		}
		private void sel_combo(ComboBox cmb)
		{
			if (cmb.Items.Count > 0) {
				cmb.SelectedIndex = 0;
			}
		}
		private string devadr_to_str(int devadr)
		{
			string str;
			switch (devadr) {
			case 0x76:str="温湿度";break;
			case 0x38:str="照度近接";break;
			case 0x68:str="加速度・ジャイロ";break;
			case 0x29:str="カラーセンサー";break;
			case 0x48:str="ADC(筋電)";break;
			case 0x49:str="ADC(圧力)";break;
			case 0x28:str="LED(POT)";break;
			case 0x50:str="EEPROM";break;
			default  :str="(UNKNOWN)";break;
			}
			return(str);
		}
		private void detect()
		{
			this.textBox23.Clear();
			this.textBox23.Update();
			this.comboBox2.Items.Clear();
			this.comboBox3.Items.Clear();
			this.comboBox4.Items.Clear();
			this.comboBox8.Items.Clear();
			this.comboBox9.Items.Clear();
			this.comboBox10.Items.Clear();
			this.comboBox13.Items.Clear();
			string txt="";
			int cnt = 0;
			ArrayList devar = new ArrayList();

			for (int i = 0; i < BUSADR.Length; i++) {
				int busadr = BUSADR[i];
				//---
				for (int devadr = 1; devadr < 0x7F; devadr++) {
					int ret, done;
					byte[] buf = {0};
					int	rcnt = 0;
retry:
					System.Threading.Thread.Sleep(1);
					D.GET_I2C_RED(busadr, devadr, 0x00, 1, out buf, out ret, out done);
					if (ret != 0) {
						txt += string.Format("{0:X02}    {1:X02} {2}\r\n", busadr, devadr, devadr_to_str(devadr));
						scroll_to_end(this.textBox23, txt);
						cnt++;
					}
					else {
						if (busadr == 0x00 && devadr == 0x38) {
							if (rcnt++ < 3) {
								goto retry;
							}
						}
					}
					string tmp = string.Format("{0:X02}-{1:X02}h\r\n", busadr, devadr);
					D00_BASE d00 = null;
					switch (devadr) {
					case 0x76://0x76:bme280:温湿度
						if (ret!=0) {
							this.comboBox2.Items.Add(tmp);
						}
						if (ret != 0) {
							if (bme280[i] != null) {
								d00 = bme280[i];
							}
							else {
								d00 = new D76_BME280(busadr);
								bme280[i] = (D76_BME280)d00;
							}
						}
						else if (bme280[i] != null) {
							bme280[i] = null;//途中でコネクタを抜いた？
						}
					break;
					case 0x38://0x38:rpr0521:照度近接
						if (ret!=0) {
							this.comboBox3.Items.Add(tmp);
						}
						if (ret != 0) {
							if (rpr0521[i] != null) {
								d00 = rpr0521[i];
							}
							else {
								d00 = new D38_RPR0521RS(busadr);
								rpr0521[i] = (D38_RPR0521RS)d00;
							}
						}
						else if (rpr0521[i] != null) {
							rpr0521[i] = null;//途中でコネクタを抜いた？
						}
					break;
					case 0x68://0x68:mpu9250:加速度・ジャイロ
						if (ret!=0) {
							this.comboBox4.Items.Add(tmp);
						}
						if (ret != 0) {
							if (mpu9250[i] != null) {
								d00 = mpu9250[i];
							}
							else {
								d00 = new D68_MPU9250(busadr);
								mpu9250[i] = (D68_MPU9250)d00;
							}
						}
						else if (mpu9250[i] != null) {
							mpu9250[i] = null;//途中でコネクタを抜いた？
						}
					break;
					case 0x29://0x29:カラーセンサー
						if (ret!=0) {
							this.comboBox9.Items.Add(tmp);
						}
						if (ret != 0) {
							if (tcs34725[i] != null) {
								d00 = tcs34725[i];
							}
							else {
								d00 = new D29_TCS34725(busadr);
								tcs34725[i] = (D29_TCS34725)d00;
							}
						}
						else if (tcs34725[i] != null) {
							tcs34725[i] = null;//途中でコネクタを抜いた？
						}
					break;
					case 0x48://0x48:adc
						if (ret!=0) {
							this.comboBox10.Items.Add(tmp);
						}
						if (ret != 0) {
							if (ads1114_48h[i] != null) {
								d00 = ads1114_48h[i];
							}
							else {
								d00 = new D48_ADS1114(busadr, devadr);
								ads1114_48h[i] = (D48_ADS1114)d00;
							}
						}
						else if (ads1114_48h[i] != null) {
							if (ads1114_48h[i].DEVADR == devadr) {
							ads1114_48h[i] = null;//途中でコネクタを抜いた？
							}
						}
					break;
					case 0x49://0x49:adc
						if (ret!=0) {
							this.comboBox8.Items.Add(tmp);
						}
						if (ret != 0) {
							if (ads1114_49h[i] != null) {
								d00 = ads1114_49h[i];
							}
							else {
								d00 = new D48_ADS1114(busadr, devadr);
								ads1114_49h[i] = (D48_ADS1114)d00;
							}
						}
						else if (ads1114_49h[i] != null) {
							if (ads1114_49h[i].DEVADR == devadr) {
							ads1114_49h[i] = null;//途中でコネクタを抜いた？
							}
						}
					break;
					case 0x28://0x28:digital pot
						if (ret!=0) {
							this.comboBox13.Items.Add(tmp);
						}
						if (ret != 0) {
							if (mcp4661[i] != null) {
								d00 = mcp4661[i];
							}
							else {
								d00 = new D28_MCP4661(busadr);
								mcp4661[i] = (D28_MCP4661)d00;
							}
						}
						else if (mcp4661[i] != null) {
							if (mcp4661[i].DEVADR == devadr) {
							mcp4661[i] = null;//途中でコネクタを抜いた？
							}
						}
					break;
					}
					if (d00 != null) {
						var dev = new G.DEVICE_TBL();
						//---
						dev.BID = busadr;
						dev.DID = devadr;
						dev.CNT_OF_SENS = d00.SENS_COUNT();
						dev.SEN = new G.SENSOR_TBL[dev.CNT_OF_SENS];
						dev.FUN = new G.DLG_BOOL_OBJECTS(d00.GET);
						dev.INI = new G.DLG_BOOL_VOID(d00.INIT);
						//---
						for (int j = 0; j < dev.CNT_OF_SENS; j++) {
							dev.SEN[j] = new G.SENSOR_TBL();
							dev.SEN[j]._DEV = dev;
							dev.SEN[j].NAME = d00.SENS_NAME(j);
							dev.SEN[j].UNIT = d00.SENS_UNIT(j);
							dev.SEN[j].PREC = d00.SENS_PREC(j);
							dev.SEN[j].DATA = double.NaN;
						}
						devar.Add(dev);
					}
				}
			}
			if (txt == "") {
				txt = "device not found\r\n";
			}
			txt += "------------\r\nEND (" + cnt.ToString() + " devices)\r\n";
			scroll_to_end(this.textBox23, txt);
			//---
			if (cnt == 0) {
				G.DT = null;
			}
			else {
				G.DT = (G.DEVICE_TBL[])devar.ToArray(typeof(G.DEVICE_TBL));
			}

			//--- I2Cバスのリセット(OFF->ON)しないとハングしてしまう
			button13_Click(0x00, null);
			button13_Click(0x10, null);
			if (D.DEV_TYPE == 1) {
			button13_Click(0x20, null);
			button13_Click(0x30, null);
			}
			//---
			if (this.checkBox10.Checked && cnt > 0) {
				txt += "INIT実行中...";
				scroll_to_end(this.textBox23, txt);
				for (int i = 0; i < this.comboBox2.Items.Count; i++) {
					this.comboBox2.SelectedIndex = i;
					OnClicks(this.button6, null);
				}
				for (int i = 0; i < this.comboBox3.Items.Count; i++) {
					this.comboBox3.SelectedIndex = i;
					OnClicks(this.button8, null);
				}
				for (int i = 0; i < this.comboBox4.Items.Count; i++) {
					this.comboBox4.SelectedIndex = i;
					OnClicks(this.button10, null);
				}
				for (int i = 0; i < this.comboBox8.Items.Count; i++) {
					this.comboBox8.SelectedIndex = i;
					OnClicks(this.button22, null);
				}
				for (int i = 0; i < this.comboBox9.Items.Count; i++) {
					this.comboBox9.SelectedIndex = i;
					OnClicks(this.button16, null);
				}
				for (int i = 0; i < this.comboBox10.Items.Count; i++) {
					this.comboBox10.SelectedIndex = i;
					OnClicks(this.button18, null);
				}
				for (int i = 0; i < this.comboBox13.Items.Count; i++) {
					this.comboBox13.SelectedIndex = i;
					int q;
					if ((q = ba2i(this.comboBox13)) >= 0) {
						mcp4661[q].INIT();
						numericUpDown12_ValueChanged(null, null);
						button20_Click(this.button21, null);//LED.ON
					}
				}
				txt += "終了\r\n";
				scroll_to_end(this.textBox23, txt);
			}
			//---
			sel_combo(comboBox2);
			sel_combo(comboBox3);
			sel_combo(comboBox4);
			sel_combo(comboBox8);
			sel_combo(comboBox9);
			sel_combo(comboBox10);
			sel_combo(comboBox13);
			//---
			this.button15.Enabled = (cnt!=0);
		}

		private void button13_Click(object sender, EventArgs e)
		{
			int	busspd = (int)this.numericUpDown11.Value;
			int wait01 = (int)this.numericUpDown8.Value;
			int wait02 = (int)this.numericUpDown9.Value;
//			int busadr = this.comboBox5.SelectedIndex == 0 ? 0x00:0x10;
			int busadr = this.comboBox5.SelectedIndex * 0x10;
			if (sender is int) {
				busadr = (int)sender;
			}
			D.SET_I2C_BUS(busadr, /*enb*/0, /*spd*/busspd, wait01, wait02);
			System.Threading.Thread.Sleep(10);
			D.SET_I2C_BUS(busadr, /*enb*/1, /*spd*/busspd, wait01, wait02);
		}
		private string n2b_str(int n, int len)
		{
			string str = "";
			for (int i = 0; i < len; i++) {
				if (i > 0 && (i%4)==0) {
					str = "-" + str;
				}
				if ((n&0x01) != 0) {
					str = "1" + str;
				}
				else {
					str = "0" + str;
				}
				if (((i+1)%4)==0) {
				}
				n >>= 1;
			}
			return(str);
		}
		private void button14_Click(object sender, EventArgs e)
		{
			//int busadr = this.comboBox6.SelectedIndex == 0 ? 0x00:0x10;
			int busadr = this.comboBox6.SelectedIndex * 0x10;
			int con, sts;
			D.GET_I2C_STS(busadr, out con, out sts);

			this.textBox1.Text = n2b_str(con, 16);
			this.textBox2.Text = n2b_str(sts, 16);
		}

		private void numericUpDown12_ValueChanged(object sender, EventArgs e)
		{
			if (mcp4661 == null || mcp4661[8] == null) {
				return;
			}
			mcp4661[8].SET(/*chan*/0, (int)this.numericUpDown12.Value);
		}
		private void button20_Click(object sender, EventArgs e)
		{
			if (sender == this.button21) {
				D.SET_CUR_STS(/*CHAN*/0, /*ON*/0);
				this.button21.Enabled = false;
				this.button20.Enabled = true;
			}
			else if (sender == this.button20) {
				D.SET_CUR_STS(/*CHAN*/0, /*OFF*/1);
				this.button21.Enabled = true;
				this.button20.Enabled = false;
			}
		}
	}
}

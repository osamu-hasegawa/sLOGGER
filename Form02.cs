using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//---
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.IO;

namespace sLOGGER
{
	public partial class Form02 : Form
	{
		const
		int MAX_OF_CHART = 30;
		int C_ROWCNT;
		int C_COLCNT;
		int C_CELCNT;
		int C_CHTCNT;
//		Random r = new Random();
		G.SENSOR_TBL[]
			SENS = null;
		int		mes_itv = 200;
		int		mes_cnt;
		int		mes_nxt;
		int		mes_wid = 60;
		int		mes_pts_in_wid;
//		Chart[]	charts_old = null;
		PictureBox[] charts = null;
//		Bitmap[] canvas = null;
		ChartSub[]
				csubs = null;
		Panel[]
				panels = null;
		StreamWriter m_wr;
		TimeSpan m_ts;
		int		tic_cnt;
		int		tic1, tic2;

		public Form02()
		{
			InitializeComponent();
		}
		private void ResetLayout()
		{
			C_ROWCNT = G.SS.GRP_ROW_CNT;
			C_COLCNT = G.SS.GRP_COL_CNT;
			C_CELCNT = (C_ROWCNT * C_COLCNT);
			C_CHTCNT = (C_ROWCNT * C_COLCNT);
		//	panels = new Panel[C_CELCNT];
		}
		private void reset_graph()
		{
			if (G.DT == null) {
				this.SENS = null;
			}
			else {
				var ar = new ArrayList();
				for (int i = 0; i < G.DT.Length; i++) {
					for (int h = 0; h < G.DT[i].CNT_OF_SENS; h++) {
						ar.Add(G.DT[i].SEN[h]);
					}
				}
				this.SENS = (G.SENSOR_TBL[])ar.ToArray(typeof(G.SENSOR_TBL));
			}
			if (false/*is_equal(this.SENS, G.SS.SEN_TBL)*/) {
				for (int i = 0; i < G.SS.SEN_TBL.Length; i++) {
					this.SENS[i].G_ID = G.SS.SEN_TBL[i].G_ID;
				}
			}
			else if (this.SENS != null) {
				int g_id = 0;
				int n_id = 1;
				for (int i = 0; i < this.SENS.Length; i++) {
					this.SENS[i].G_ID = 0;
					if (this.SENS[i]._DEV.DID == 0x38 && this.SENS[i].UNIT == "[cnt]") {
					this.SENS[i].G_ID = ++g_id;
					this.SENS[i].SMIN = G.SS.SCL_MIN_D38;
					this.SENS[i].SMAX = G.SS.SCL_MAX_D38;
					}
					if (this.SENS[i].NAME == "PS") {
						this.SENS[i].NAME = string.Format("近接{0}", n_id++);
					}
				}
			}
			if (this.SENS != null) {
				for (int i = 0; i < this.SENS.Length; i++) {
					this.SENS[i].OBJE = null;
				}
			}
			set_graph_tbl();
			if (check_graph_layout()) {
				set_graph_layout();
			}
		}
		private bool is_equal(G.SENSOR_TBL[] s1, G.SENSOR_TBL[] s2)
		{
			if (s1 == null || s2 == null) {
				return(false);
			}
			if (s1.Length != s2.Length) {
				return(false);
			}
			for (int i = 0; i < s1.Length; i++) {
				if (s1[i].NAME != s2[i].NAME) {
					return(false);
				}
				if (s1[i].UNIT != s2[i].UNIT) {
					return(false);
				}
			}
			return(true);
		}
		private void Form02_Load(object sender, EventArgs e)
		{
//			List<Label> l_lbl = new List<Label>();
		#if false
			//Math.DivRem();
			int s_b = sizeof(bool  );//1B
			int s_l = sizeof(long  );//8B!!!
			int s_i = sizeof(int   );//4B
			int s_f = sizeof(float );//4B
			int s_d = sizeof(double);//8B
		#endif
			G.FORM02 = this;
			G.FORM01 = new Form01();
			G.FORM01.Show();
			G.FORM01.Hide();
#if false//2019.08.23
			this.tabControl1.Dock = DockStyle.Fill;
#endif
			//---
			//Random r = new Random();
			//for (int i = 0; i < 50; i++) {
			//    timer1_Tick(null, null);
			//}
			this.numericUpDown1.Value = G.SS.MES_INT_MSEC;
			this.numericUpDown2.Value = G.SS.MES_WID_SECS;

#if true//2019.08.23
			ResetLayout();
			this.csubs = new ChartSub[MAX_OF_CHART];
			this.charts = new PictureBox[MAX_OF_CHART];
//			this.canvas = new Bitmap[C_CHTCNT];
			this.panels = new Panel[] {
				this.panel1, this.panel2,this.panel3,this.panel4,this.panel5,this.panel6,
				this.panel7, this.panel8,this.panel9,this.panel10,this.panel11,this.panel12,
				this.panel13, this.panel14,this.panel15,this.panel16,this.panel17,this.panel18,
				this.panel19, this.panel20,this.panel21,this.panel22,this.panel23,this.panel24,
				this.panel25, this.panel26,this.panel27,this.panel28,this.panel29,this.panel30
			};

			//this.tabControl1.Dock = DockStyle.Fill;

			for (int i = 0; i < MAX_OF_CHART; i++) {
				PictureBox pb = new PictureBoxEx();
				pb.Tag = i;
				pb.Paint += new PaintEventHandler(pictureBox1_Paint);
				pb.Resize += new EventHandler(pictureBox1_Resize);
				pb.Dock = DockStyle.Fill;
				this.charts[i] = pb;
				this.panels[i].Controls.Add(pb);
				//---
/*				Label lb = new Label();
				this.panels[i].Controls.Add(lb);
				lb.Visible = true;//(i < C_CHTCNT) ? true: false;
				lb.BringToFront();
				l_lbl.Add(lb);*/
			}
			/*for (int i = C_CHTCNT; i < MAX_OF_CHART; i++) {
				this.panels[i].Visible = false;
			}*/
			foreach (PictureBox c in this.charts) {
				c.Dock = DockStyle.Fill;
			}
#endif

			numericUpDown1_ValueChanged(null, null);
			numericUpDown2_ValueChanged(null, null);

#if true
			if (true) {
				int i = 0;
				foreach (PictureBox c in this.charts) {
					csubs[i] = new ChartSub();

					//---
//					csubs[i].GDEF_INIT(3, c, new Rectangle(25,13,-13,-20));
					csubs[i].GDEF_INIT(3, c, new Rectangle(25, 13, -13 - 12, -20));
					csubs[i].GDEF_PSET( 0, this.mes_wid, this.mes_wid/2, -1, +1, 1);
#if true//TEST
					Graphics g = c.CreateGraphics();
					csubs[i].GDEF_GRID(g);
					g.Dispose();
#endif
					//---
//					csubs[i].lbl = l_lbl[i];

					//---
					i++;
				}
			}
			Form02_Resize(null, null);
#endif

			reset_graph();

		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			this.mes_itv = (int)this.numericUpDown1.Value;
			//this.timer1.Interval = (int)this.numericUpDown1.Value;
			this.mes_pts_in_wid = this.mes_wid *1000 / this.mes_itv;
		}
		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			this.mes_wid = (int)this.numericUpDown2.Value; //sec
			this.mes_pts_in_wid = this.mes_wid *1000 / this.mes_itv;

			if (this.csubs == null || this.csubs[0] == null) {
				return;
			}
			for (int i = 0; i < csubs.Length; i++) {
				this.csubs[i].GDEF_PSET(0, this.mes_wid, double.NaN,double.NaN,double.NaN,double.NaN);
				this.csubs[i].Update();
			}
		}
		private void do_auto_scale()
		{
			for (int i = 0; i < this.charts.Length; i++ ) {
				this.csubs[i].DoAutoScale();
			}
		}
		private void check_free_folder(string path)
		{
#if false
			const
			int MAX_OF_FILES = 20;
			string[]files;
			try {
				if (System.IO.Directory.Exists(path) == false) {
					System.IO.Directory.CreateDirectory(path);
				}
				files = System.IO.Directory.GetFiles(path, "????????-??????.csv");
				Array.Sort(files);
				if (files.Length > (MAX_OF_FILES-1)) {
					int cnt = files.Length-(MAX_OF_FILES-1);
					for (int i = 0; i < cnt; i++) {
						System.IO.File.Delete(files[i]);
					}
				}
			}
			catch (Exception ex) {
			}
#endif
		}
		private void button3_Click(object sender, EventArgs e)
		{//FREE.RUN / MEASURE
			for (int i = 0; i < G.DT.Length; i++) {
				G.DT[i].INI();
			}

			if (sender == this.button3) {
				//FREE RUN
				DateTime dt = DateTime.Now;
				string path = G.GET_DOC_PATH(null);
				path += "\\FREERUN";
				check_free_folder(path);
				path += "\\";
				path += string.Format("{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}",
											dt.Year, dt.Month, dt.Day,
											dt.Hour, dt.Minute, dt.Second);
				path += ".csv";
				f_open(path);
			}
			this.mes_wid = (int)this.numericUpDown2.Value; //sec
			this.mes_pts_in_wid = this.mes_wid *1000 / this.mes_itv;

			this.mes_cnt = 0;
			this.mes_nxt = System.Environment.TickCount;
			this.m_ts = new TimeSpan();
			this.timer1.Interval = 1;
			this.timer1.Enabled = true;
			this.button4.Enabled = true;
			this.button3.Enabled = false;
			this.numericUpDown1.Enabled = false;
			this.numericUpDown2.Enabled = false;
			if (true) {
				int i = 0;
				foreach (PictureBox c in this.charts) {
					this.csubs[i].GDEF_PSET(0, this.mes_wid, double.NaN, double.NaN, double.NaN, double.NaN);
					this.csubs[i].rbuf.clear();
					this.csubs[i].interval = this.mes_itv/1000.0;
					this.csubs[i].Update();
					i++;
				}
			}
			//do_auto_scale();
		}

		private void button4_Click(object sender, EventArgs e)
		{//stop
			this.timer1.Enabled = false;
			this.button3.Enabled = true;
			this.button4.Enabled = false;
			this.numericUpDown1.Enabled = true;
			this.numericUpDown2.Enabled = true;
			f_close();
		}
		private void add_data(G.SENSOR_TBL sen)
		{
#if true//2019.08.23
			if (sen.G_ID > 0) {
				int h = sen.G_ID-1;
				if (h < 0 || h >= csubs.Length) {
					h = h;
				}
				else {
					csubs[h].AddData(sen.DATA, true);
					//add_data(i, G.DT[i].SEN[h]);
				}
			}
#endif
		}
		private void remove_data()
		{
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.mes_nxt > System.Environment.TickCount) {
				return;
			}

			this.timer1.Enabled = false;
			if (true) {
				this.tic_cnt++;
				this.tic2 = System.Environment.TickCount;
				double fps, ela;
				if ((ela = (this.tic2 - this.tic1)) >= 1000) {
					fps = tic_cnt / (ela / 1000.0);
					this.label5.Text = string.Format("SPS:{0:F2}", fps);
					tic_cnt = 0;
					tic1 = tic2;
				}
			}
			if (true) {
				int bits = D.GET_PIO_BIT();
				if ((bits&0x01) == 1) {
					G.EXT_PWR_STS = 1;//power-on
				}
				else {
					G.EXT_PWR_STS = 0;//power-off
				}
			}
			for (int i = 0; i < G.DT.Length; i++) {
				object[] objs;
				if (G.DT[i].DID != 0x38) {
					continue;
				}
				G.DT[i].FUN(out objs);
				for (int h = 0; h < G.DT[i].CNT_OF_SENS; h++) {
				//	G.DT[i].SEN[h].DATA = 1000;
					if (objs[h] is int) {
						G.DT[i].SEN[h].DATA = (int)objs[h];
					}
					else {
						G.DT[i].SEN[h].DATA = (double)objs[h];
					}
					add_data(G.DT[i].SEN[h]);
				}
			}
			if (true) {
#if true
				if (mes_cnt > 0 && (mes_cnt % this.mes_pts_in_wid) == 0) {
					for (int i = 0; i < this.csubs.Length; i++) {
						csubs[i].PageFeed();
					}
					//button7_Click(null, null);
				}
#endif
			}
			f_write(m_ts, /*G.MES_VAL*/null);
			m_ts += new TimeSpan(0,0,0,0,mes_itv);
			/*if ((this.mes_cnt%10) == 0) {
				do_auto_scale();
				remove_data();
			}*/

			this.mes_nxt += mes_itv;
			this.mes_cnt++;
			this.timer1.Enabled = true;
		}
		private void f_open(string path)
		{
			string buf;
			try {
				/*				rd = new StreamReader(filename, Encoding.GetEncoding("Shift_JIS"));*/
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
				m_wr = new StreamWriter(path, true, Encoding.Default);
				buf = "TIME";
				for (int i = 0; i < this.SENS.Length/*G.DT.Length*/; i++) {
					if (this.SENS[i].G_ID <= 0) {
						continue;
					}
					buf += string.Format(",{0}{1}", this.SENS[i].NAME, this.SENS[i].UNIT/*G.DT[i].NAME, G.DT[i].UNIT*/);
				}
				m_wr.WriteLine(buf);
			}
			catch (Exception) {
			}
		}
		private void f_close()
		{//無圧力時の平均値:0.6784321459

			try {
				if (m_wr != null) {
					m_wr.Close();
					m_wr.Dispose();
				}
			}
			catch (Exception) {
			}
			m_wr = null;
		}
		private void f_write(TimeSpan ts, double[] mbuf)
		{
			string buf;
			try {
				buf = string.Format("{0:F3}", ts.TotalSeconds);
				for (int i = 0; i < this.SENS.Length/*mbuf.Length*/; i++) {
					if (this.SENS[i].G_ID <= 0) {
						continue;
					}
					buf += string.Format(",{0}", this.SENS[i].DATA);
				
					/*switch (prec[i]) {
						case  0:buf += string.Format(",{0:F0}", mbuf[i]); break;
						case  1:buf += string.Format(",{0:F1}", mbuf[i]); break;
						case  2:buf += string.Format(",{0:F2}", mbuf[i]); break;
						case  3:buf += string.Format(",{0:F3}", mbuf[i]); break;
						default:buf += string.Format(",{0:F0}", mbuf[i]); break;

					}*/
				}
				m_wr.WriteLine(buf);
			}
			catch (Exception) {
			}
		}
		void set_graph_tbl()
		{
		}


		private bool check_graph_layout()
		{
			//GRAPH設定を確認
			return(true);
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (this.timer1.Enabled) {
				return;
			}
			G.FORM01.Show();
			this.Hide();
		}
		private void set_graph_layout()
		{
			// seriesの数を２に揃える
			// seriesをprimary, secondaryの順にする
			if (true) {
			}
			//---
			if (this.SENS == null) {
				return;
			}
#if true//2019.08.23
			if (true) {
			}
			//---
			if (G.DT == null) {
				return;
			}
			for (int i = 0; i < this.SENS.Length/*G.DT.Length*/; i++) {
				if (/*G.DT[i].GID*/this.SENS[i].G_ID == 0) {
					continue;
				}
				int h = (/*G.DT[i].GID*/this.SENS[i].G_ID - 1);///2;
				//---
				if (h >= this.csubs.Length) {
					continue;
				}
				if (this.SENS[i]._DEV.DID == 0x38 && this.SENS[i].UNIT == "[cnt]") {
				this.SENS[i].SMIN = G.SS.SCL_MIN_D38;
				this.SENS[i].SMAX = G.SS.SCL_MAX_D38;
				}				
				this.csubs[h].GDEF_LSET(this.SENS[i].NAME, this.SENS[i].UNIT/*G.DT[i].NAME, G.DT[i].UNIT*/);
				this.csubs[h].GDEF_PSET(0, this.mes_wid, double.NaN, this.SENS[i].SMIN, this.SENS[i].SMAX/*G.DT[i].SMIN, G.DT[i].SMAX*/, double.NaN);
				//---
				//this.SENS[i].OBJE = this.csubs[h];
				//---
				this.csubs[h].Update();
			}
#endif
		}
#if false//2019.08.23
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboBox1.SelectedIndex == 0) {
				this.propertyGrid1.SelectedObject = null;
			}
			else {
				this.propertyGrid1.SelectedObject = this.charts[this.comboBox1.SelectedIndex-1];
			}
		}
#endif
		private void Form02_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (timer1.Enabled) {
				button4_Click(null, null);
			}
			G.FORM02 = null;
			if (G.FORM01 != null) {
				G.FORM01.Close();
			}
		}
		private void Form02_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.SENS != null) {
				for (int i = 0; i < this.SENS.Length; i++) {
					this.SENS[i].OBJE = null;
				}
				G.SS.SEN_TBL = this.SENS;
			}
			G.SS.MES_INT_MSEC = (int)this.numericUpDown1.Value;
			G.SS.MES_WID_SECS = (int)this.numericUpDown2.Value;
		}

		private void Form02_Resize(object sender, EventArgs e)
		{
			const
			int gap = 3;
#if true
			int wid;// = this.panel32.Width;
			int hei;// = this.panel32.Height;
			wid = this.tabPage1.Width;
			hei = this.tabPage1.Height;
#else
			int wid = this.ClientSize.Width;
			int hei = this.ClientSize.Height;
#endif
			int uniw = (wid - (C_COLCNT - 1) * gap) / C_COLCNT;
			int unih = (hei - (C_ROWCNT - 1) * gap) / C_ROWCNT;
			int i = 0, xoff, yoff;
			for (int y = 0; y < C_ROWCNT; y++) {
				for (int x = 0; x < C_COLCNT; x++) {
					xoff =  uniw * x;
					xoff+= gap * x;
					yoff = unih * y;
					yoff+= gap * y;
					this.panels[i].Left = xoff;
					this.panels[i].Top = yoff;
					this.panels[i].Width = uniw;
					this.panels[i].Height = unih;
					this.panels[i].Visible = true;
					i++;
				}
			}
			for (; i < this.panels.Length; i++) {
				this.panels[i].Visible = false;
			}
		}
		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			object obj = ((PictureBox)sender).Tag;
			if (obj == null) {
				return;
			}
			int i = (int)obj;
			if (csubs[i] == null) {
				return;
			}
			if (i == 0) {
				i = i;
			}
			if (((PictureBox)sender).Image != null) {
				Graphics g = Graphics.FromImage(((PictureBox)sender).Image);
				g.Clear(Color.Blue);
				csubs[i].GDEF_GRID(g);
				g.Dispose();
			}
			else {
				Graphics g = e.Graphics;// ((PictureBox)sender).CreateGraphics();
				g.Clear(Color.Blue);
				csubs[i].GDEF_GRID(g);
				//			g.Dispose();
			}
		}
		private void pictureBox1_Resize(object sender, EventArgs e)
		{
			object obj = ((PictureBox)sender).Tag;
			if (obj == null) {
				return;
			}
			int i = (int)obj;
			if (csubs[i] == null) {
				return;
			}
			if (i == 0) {
				i = i;
			}
			if (true) {
				PictureBox pb = (PictureBox)sender;
				Image img = pb.Image;
				if (img == null || img.Width != pb.Width || img.Height != pb.Height) {
					if (img != null) {
						img.Dispose();
						img = null;
					}
					//pb.Image = null;// new Bitmap(pb.Width, pb.Height);
				}
			}
			if (true) /*for (int i = 0; i < this.charts.Length; i++)*/ {
				PictureBox c = this.charts[i];
				csubs[i].GDEF_INIT(3, c, new Rectangle(25, 13, -13-12, -20));
				//csubs[i].GDEF_PSET(0, 120, 60, -1, +1, 1);
				c.Invalidate();
			}
		}

		private void button97_Click(object sender, EventArgs e)
		{
			if (true/*G.UIF_LEVL > 0*/) {
				Form20 frm = new Form20();
				//frmSettings frm = new frmSettings();
				frm.m_ss = (G.SYSSET)G.SS.Clone();;
			//	frm.m_sp = (G.SENPAR)G.SP.Clone();;
				if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					G.SS = (G.SYSSET)frm.m_ss.Clone();
				//	G.SP = (G.SENPAR)frm.m_sp.Clone();
				//	check_rewrite();
					if (G.SS.GRP_ROW_CNT != C_ROWCNT || G.SS.GRP_COL_CNT != C_COLCNT) {
						ResetLayout();
						Form02_Resize(null, null);
						//G.mlog("#iグラフレイアウトの変更はソフトの再起動後に有効になります.");
					}
					set_graph_layout();
				}
			}
			else {
				/*Form20 frm = new Form20();
				frm.m_sp = (G.SENPAR)G.SP.Clone();
				if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					G.SP = (G.SENPAR)frm.m_sp.Clone();
					check_rewrite();
				}*/
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{//connect
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			
			try {
			G.FORM01.Invoke(new G.DLG_VOID_OBJECT_EVARG(G.FORM01.OnClicks), new object[]{G.FORM01.button1, null});
			if (D.isCONNECTED()) {
				//detect
				G.FORM01.Invoke(new G.DLG_VOID_OBJECT_EVARG(G.FORM01.OnClicks), new object[]{G.FORM01.button12, null});
				reset_graph();
				set_graph_layout();
				this.button1.Enabled = false;
				this.button2.Enabled = true;
				this.button3.Enabled = true;
			}
			}
			catch (Exception ex) {
				G.mlog(ex.Message);
			}
			System.Windows.Forms.Cursor.Current = Cursors.Default;
		}

		private void button2_Click(object sender, EventArgs e)
		{//disconnect
			if (this.timer1.Enabled) {
				button4_Click(null, null);
			}
			G.FORM01.Invoke(new G.DLG_VOID_OBJECT_EVARG(G.FORM01.OnClicks), new object[]{G.FORM01.button2, null});
			this.button1.Enabled = true;
			this.button2.Enabled = false;
			this.button3.Enabled = false;
		}

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sLOGGER
{
	public partial class Form31 : Form
	{
		//public string	m_ser1;

		public G.SYSSET	m_ss;

		private const int MAX_OF_CELLS = 30;
		private Label[] lblGRAPH;
		private Label[] lblSENSR;

		public Form31()
		{
			InitializeComponent();
		}

		private void Form31_Load(object sender, EventArgs e)
		{
			lblGRAPH = new Label[] {
				this.label10, this.label11, this.label12, this.label13, this.label14, 
				this.label15, this.label16, this.label17, this.label18, this.label19, 
				this.label20, this.label21, this.label22, this.label23, this.label24, 
				this.label25, this.label26, this.label27, this.label28, this.label29, 
				this.label30, this.label31, this.label32, this.label33, this.label34, 
				this.label35, this.label36, this.label37, this.label38, this.label39,
			};
			lblSENSR = new Label[] {
				this.label45, this.label46, this.label47, this.label48, 
				this.label49, this.label50, this.label51,
				this.label52, this.label53, this.label54, 
				this.label55, this.label56, this.label57, 
				this.label58, this.label59, 
				this.label60, this.label61, 
				this.label62, this.label63, 
				this.label64, this.label65, this.label66
			};
			for (int i = 0; i < this.lblGRAPH.Length; i++) {
				this.lblGRAPH[i].Text = "";
				this.lblGRAPH[i].TextAlign = ContentAlignment.MiddleCenter;
				this.lblGRAPH[i].Tag = (i+1);
			}
			//---
			this.numericUpDown1.Value = m_ss.GRP_ROW_CNT;
			this.numericUpDown2.Value = m_ss.GRP_COL_CNT;
#if false
			//
			if (lblSENSR.Length != m_ss.SEN_GRH_GID.Length) {
				G.mlog("Internal Error");
			}
			//
			for (int i = 0; i < m_ss.SEN_GRH_GID.Length; i++) {
				int gid = m_ss.SEN_GRH_GID[i];
				if (gid <= 0) {
					continue;
				}
				gid--;
				lblSENSR[i].Tag = lblSENSR[i].Text;
				lblGRAPH[gid].Text = lblSENSR[i].Text;
				lblSENSR[i].Text = null;
			}
#endif
			//---
			ReseTableLayout();
			//---
			DDX(true);
		}

		private void Form31_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK) {
				return;
			}
			if (DDX(false) == false) {
				e.Cancel = true;
			}
		}
		private bool DDX(bool bUpdate)
        {
            bool rc=true;
			//
			try {
#if false
				//---
				DDV.DDX(bUpdate, this.textBox10, ref m_ss.SEN_SCL_MAX[0]);
				DDV.DDX(bUpdate, this.textBox11, ref m_ss.SEN_SCL_MIN[0]);
				DDV.DDX(bUpdate, this.textBox12, ref m_ss.SEN_SCL_MAX[1]);
				DDV.DDX(bUpdate, this.textBox13, ref m_ss.SEN_SCL_MIN[1]);
				DDV.DDX(bUpdate, this.textBox14, ref m_ss.SEN_SCL_MAX[2]);
				DDV.DDX(bUpdate, this.textBox15, ref m_ss.SEN_SCL_MIN[2]);
				DDV.DDX(bUpdate, this.textBox16, ref m_ss.SEN_SCL_MAX[3]);
				DDV.DDX(bUpdate, this.textBox17, ref m_ss.SEN_SCL_MIN[3]);
				DDV.DDX(bUpdate, this.textBox18, ref m_ss.SEN_SCL_MAX[4]);
				DDV.DDX(bUpdate, this.textBox19, ref m_ss.SEN_SCL_MIN[4]);
				DDV.DDX(bUpdate, this.textBox20, ref m_ss.SEN_SCL_MAX[5]);
				DDV.DDX(bUpdate, this.textBox21, ref m_ss.SEN_SCL_MIN[5]);
				DDV.DDX(bUpdate, this.textBox22, ref m_ss.SEN_SCL_MAX[6]);
				DDV.DDX(bUpdate, this.textBox23, ref m_ss.SEN_SCL_MIN[6]);
				DDV.DDX(bUpdate, this.textBox24, ref m_ss.SEN_SCL_MAX[7]);
				DDV.DDX(bUpdate, this.textBox25, ref m_ss.SEN_SCL_MIN[7]);
				DDV.DDX(bUpdate, this.textBox26, ref m_ss.SEN_SCL_MAX[8]);
				DDV.DDX(bUpdate, this.textBox27, ref m_ss.SEN_SCL_MIN[8]);
				DDV.DDX(bUpdate, this.textBox28, ref m_ss.SEN_SCL_MAX[9]);
				DDV.DDX(bUpdate, this.textBox29, ref m_ss.SEN_SCL_MIN[9]);
				DDV.DDX(bUpdate, this.textBox30, ref m_ss.SEN_SCL_MAX[10]);
				DDV.DDX(bUpdate, this.textBox31, ref m_ss.SEN_SCL_MIN[10]);
				DDV.DDX(bUpdate, this.textBox32, ref m_ss.SEN_SCL_MAX[11]);
				DDV.DDX(bUpdate, this.textBox33, ref m_ss.SEN_SCL_MIN[11]);
				DDV.DDX(bUpdate, this.textBox34, ref m_ss.SEN_SCL_MAX[12]);
				DDV.DDX(bUpdate, this.textBox35, ref m_ss.SEN_SCL_MIN[12]);
				DDV.DDX(bUpdate, this.textBox36, ref m_ss.SEN_SCL_MAX[13]);
				DDV.DDX(bUpdate, this.textBox37, ref m_ss.SEN_SCL_MIN[13]);
				DDV.DDX(bUpdate, this.textBox38, ref m_ss.SEN_SCL_MAX[14]);
				DDV.DDX(bUpdate, this.textBox39, ref m_ss.SEN_SCL_MIN[14]);
				DDV.DDX(bUpdate, this.textBox40, ref m_ss.SEN_SCL_MAX[15]);
				DDV.DDX(bUpdate, this.textBox41, ref m_ss.SEN_SCL_MIN[15]);
				DDV.DDX(bUpdate, this.textBox42, ref m_ss.SEN_SCL_MAX[16]);
				DDV.DDX(bUpdate, this.textBox43, ref m_ss.SEN_SCL_MIN[16]);
				DDV.DDX(bUpdate, this.textBox44, ref m_ss.SEN_SCL_MAX[17]);
				DDV.DDX(bUpdate, this.textBox45, ref m_ss.SEN_SCL_MIN[17]);
				DDV.DDX(bUpdate, this.textBox46, ref m_ss.SEN_SCL_MAX[18]);
				DDV.DDX(bUpdate, this.textBox47, ref m_ss.SEN_SCL_MIN[18]);
				//ADC基準
				//ADC基準
				DDV.DDX(bUpdate, this.textBox48, ref m_ss.SEN_SCL_MAX[20]);
				DDV.DDX(bUpdate, this.textBox49, ref m_ss.SEN_SCL_MIN[20]);
				DDV.DDX(bUpdate, this.textBox50, ref m_ss.SEN_SCL_MAX[21]);
				DDV.DDX(bUpdate, this.textBox51, ref m_ss.SEN_SCL_MIN[21]);	
				//-----
#endif
#if true
				DDV.DDX(bUpdate, this.textBox36, ref m_ss.SCL_MAX_D38);
				DDV.DDX(bUpdate, this.textBox37, ref m_ss.SCL_MIN_D38);
				DDV.DDX(bUpdate, this.textBox1 , ref m_ss.THD_VAL_PS );
				DDV.DDX(bUpdate, this.textBox2 , ref m_ss.THD_LES_STR);
				DDV.DDX(bUpdate, this.textBox3 , ref m_ss.THD_OVR_STR);
				DDV.DDX(bUpdate, this.numericUpDown3, ref m_ss.THD_FNT_SIZ);
#endif
                rc = true;
            }
            catch (Exception e) {
                G.mlog(e.Message);
                rc = false;
            }
            return (rc);
		}

		private void Form31_Validating(object sender, CancelEventArgs e)
		{
			if (DDX(false) == false) {
				e.Cancel = true;
			}
			else {
				m_ss.GRP_ROW_CNT = (int)this.numericUpDown1.Value;
				m_ss.GRP_COL_CNT = (int)this.numericUpDown2.Value;
#if false
				List<int> lst = new List<int>();

				for (int i = 0; i < lblSENSR.Length; i++) {
					string text = lblSENSR[i].Text;
					if (!string.IsNullOrEmpty(text)) {
						lst.Add(/*gid=0:グラフ無し*/0);
					}
					else {
						int j;
						text = (string)lblSENSR[i].Tag;
						for (j = 0; j < lblGRAPH.Length; j++) {
							if (lblGRAPH[j].Text == text) {
								lst.Add(/*gid*/j + 1);
								break;
							}
						}
						if (j >= lblGRAPH.Length) {
							G.mlog("Internal Error");
						}
					}
				}
				m_ss.SEN_GRH_GID = lst.ToArray();
#endif
			}

		}

		private void label9_DragEnter(object sender, DragEventArgs e)
		{//戻す
			if (e.Data.GetDataPresent(typeof(Label))) {
				Label item = (Label)e.Data.GetData(typeof(Label));
				if (item.Tag.GetType() == typeof(int)) {
					e.Effect = DragDropEffects.Move;
				}
				else {
					e.Effect = DragDropEffects.None;
				}
			}
			else {
				e.Effect = DragDropEffects.None;
			}
		}

		private void label9_DragDrop(object sender, DragEventArgs e)
		{//戻す
			if (e.Data.GetDataPresent(typeof(Label))) {
				Label self = (Label)sender;
				Label item = (Label)e.Data.GetData(typeof(Label));
				item.Text = null;
				ResetDragState();
			}
		}
		private void label10_DragEnter(object sender, DragEventArgs e)
		{
			//ドラッグされているデータがstring型か調べ、
			//そうであればドロップ効果をMoveにする
			if (e.Data.GetDataPresent(typeof(Label)))
				e.Effect = DragDropEffects.Move;
			else
				//string型でなければ受け入れない
				e.Effect = DragDropEffects.None;
		}

		private void label10_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(Label))) {
				Label self = (Label)sender;
				Label item = (Label)e.Data.GetData(typeof(Label));
				if (item.Tag != null && item.Tag.GetType() == typeof(int)) {
					//グラフリストからドロップ
					Cursor curs = self.Cursor;
					string text = self.Text;
					object objt = self.Tag;
					self.Cursor = item.Cursor;
					self.Text   = item.Text;
					self.Tag    = item.Tag;
					item.Cursor = curs;
					item.Text   = text;
					item.Tag    = objt;
				}
				else {
					//センサリストからドロップ
					self.Text = item.Text;
					self.Cursor = Cursors.Hand;
				}
			}
		}
		private void label10_MouseDown(object sender, MouseEventArgs e)
		{
			Label lbl = (Label)sender;
			if (string.IsNullOrEmpty(lbl.Text)) {
				return;
			}
			DragDropEffects ret = lbl.DoDragDrop(lbl, DragDropEffects.All);
			if (ret == DragDropEffects.Move) {
				//lbl.Text = "";
				//lbl.Cursor = null;
				//ResetDragState();
			}
		}
		private void label40_MouseDown(object sender, MouseEventArgs e)
		{
			Label lbl = (Label)sender;
			if (string.IsNullOrEmpty(lbl.Text)) {
				return;
			}
			DragDropEffects ret = lbl.DoDragDrop(lbl, DragDropEffects.All);
			if (ret == DragDropEffects.Move) {
				lbl.Tag = lbl.Text;
				lbl.Text = "";
				lbl.Cursor = null;
			}
		}
		private void ResetDragState()
		{
			int	gcnt = (int)(this.numericUpDown1.Value * this.numericUpDown2.Value);
			for (int i = 0; i < lblSENSR.Length; i++) {
				Label lbl = lblSENSR[i];
				bool bFlag = false;
				string txt = lbl.Text;
				if (string.IsNullOrEmpty(txt)) {
					txt = (string)lbl.Tag;
				}
				for (int h = 0; h < gcnt; h++) {
					if (lblGRAPH[h].Text == txt) {
						bFlag = true;//グラフに登録済み
						break;
					}
				}
				if (bFlag) {
					if (!string.IsNullOrEmpty(lbl.Text)) {
						lbl.Tag = lbl.Text;
						lbl.Text = null;
					}
				}
				else {
					if (string.IsNullOrEmpty(lbl.Text)) {
						lbl.Text = (string)lbl.Tag;
						lbl.Tag = null;
					}
				}
			}
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			int gcnt = (int)(this.numericUpDown1.Value*this.numericUpDown2.Value);

			if (gcnt > MAX_OF_CELLS) {
				if (sender == this.numericUpDown1) {
					this.numericUpDown2.Value = MAX_OF_CELLS / (int)this.numericUpDown1.Value;
				}
				else {
					this.numericUpDown1.Value = MAX_OF_CELLS / (int)this.numericUpDown2.Value;
				}
			}
			ReseTableLayout();
		}
		private void ReseTableLayout()
		{
			int row = (int)this.numericUpDown1.Value;
			int col = (int)this.numericUpDown2.Value;
			int cnt = row * col;
			int wid = this.panel1.Width / col;
			int hei = this.panel1.Height / row;
			int i;
			//---
			//---
			for (i = 0; i < cnt; i++) {
				int rid = i%row;
				int cid = i / row;
				this.lblGRAPH[i].Left = cid * wid;
				this.lblGRAPH[i].Top = rid * hei;
				this.lblGRAPH[i].Width = wid;
				this.lblGRAPH[i].Height = hei;
				this.lblGRAPH[i].Visible = true;
				//this.lblGRAPH[i].Text = (i + 1).ToString();
				//this.lblGRAPH[i].TextAlign = ContentAlignment.MiddleCenter;

			}
			for (i = i; i < this.lblGRAPH.Length; i++) {
				this.lblGRAPH[i].Visible = false;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{//自動割り当て
			int gcnt = (int)(this.numericUpDown1.Value * this.numericUpDown2.Value);
			int i, j;
			button2_Click(null, null);
			for (j = i = 0; i < this.lblSENSR.Length; i++) {
				if (j >= gcnt) {
					break;
				}
				if (lblSENSR[i].Text == "ADC:基準" || lblSENSR[i].Text == "ADC:電池電圧") {
					continue;
				}
				lblGRAPH[j].Text = lblSENSR[i].Text;
				lblGRAPH[j].Cursor = Cursors.Hand;
				j++;
				lblSENSR[i].Tag = lblSENSR[i].Text;
				lblSENSR[i].Text = null;
				lblSENSR[i].Cursor = null;
			}
			for (i = i; i < this.lblSENSR.Length; i++) {
				if (string.IsNullOrEmpty(lblSENSR[i].Text)) {
					lblSENSR[i].Text = (string)lblSENSR[i].Tag;
					lblSENSR[i].Tag = null;
				}
				lblSENSR[i].Cursor = Cursors.Hand;
			}
			for (i = gcnt; i < this.lblGRAPH.Length; i++) {
				lblGRAPH[i].Text = null;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{//クリア
			for (int i = 0; i < this.lblGRAPH.Length; i++) {
				this.lblGRAPH[i].Text = null;
			}
			ResetDragState();
		}

		private void button3_Click(object sender, EventArgs e)
		{//デフォルト設定
			this.numericUpDown1.Value = 4;//行
			this.numericUpDown2.Value = 5;//列
			int gcnt = (int)(this.numericUpDown1.Value * this.numericUpDown2.Value);
			int i, j,k;
			int[] ia = {
						    0, 1, 2, 3,	//R,G,B,C
						    7, 8, 9, 4,	//AX,AY,AZ,?
						   10,11,12, 5,	//GX,GY,GZ,?
						   13,15,17, 6,	//LX1,LX2,LX3,?
						   14,16,18,20	//PS1,PS2,PS3,?
					   };
			button2_Click(null, null);
			for (j = k = 0; k < this.lblSENSR.Length; k++) {
				if (j >= gcnt) {
					break;
				}
				i = ia[k];
				//if (lblSENSR[i].Text == "ADC:基準" || lblSENSR[i].Text == "ADC:電池電圧") {
				//    continue;
				//}
				lblGRAPH[j].Text = lblSENSR[i].Text;
				lblGRAPH[j].Cursor = Cursors.Hand;
				j++;
				lblSENSR[i].Tag = lblSENSR[i].Text;
				lblSENSR[i].Text = null;
				lblSENSR[i].Cursor = null;
			}
			//for (i = 0; i < this.lblSENSR.Length; i++) {
			//    if (string.IsNullOrEmpty(lblSENSR[i].Text)) {
			//        lblSENSR[i].Text = (string)lblSENSR[i].Tag;
			//        lblSENSR[i].Tag = null;
			//    }
			//    lblSENSR[i].Cursor = Cursors.Hand;
			//}
			for (i = gcnt; i < this.lblGRAPH.Length; i++) {
				lblGRAPH[i].Text = null;
			}
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sLOGGER
{
	public partial class Form20 : Form
	{
		//public G.SENPAR	m_sp;
		public G.SYSSET	m_ss;
		static
		private int m_last_idx = 0;

		public Form20()
		{
			InitializeComponent();
		}

		private void Form21_Load(object sender, EventArgs e)
		{
			this.numericUpDown1.Value = m_ss.GRP_ROW_CNT;
			this.numericUpDown2.Value = m_ss.GRP_COL_CNT;
			//---
			DDX(true);
#if true//2019.09.06(近接時間測定)
			checkBox1_CheckedChanged(null, null);
#endif
		}

		private void Form21_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK) {
				return;
			}
			if (DDX(false) == false) {
				e.Cancel = true;
			}
			if (((int)this.numericUpDown1.Value * (int)this.numericUpDown2.Value) > 30) {
				G.mlog("グラフ数は３０以下にしてください.");
				e.Cancel = true;
				return;
			}
			//---
			m_ss.GRP_ROW_CNT = (int)this.numericUpDown1.Value;
			m_ss.GRP_COL_CNT = (int)this.numericUpDown2.Value;
		}
		private bool DDX(bool bUpdate)
        {
            bool rc=true;
			try {
#if true
				DDV.DDX(bUpdate, this.comboBox12  , ref m_ss.D38_410_MSTM);
				DDV.DDX(bUpdate, this.comboBox13  , ref m_ss.D38_424_ALG1);
				DDV.DDX(bUpdate, this.comboBox14  , ref m_ss.D38_422_ALG2);
				DDV.DDX(bUpdate, this.comboBox15  , ref m_ss.D38_420_LEDC);
				DDV.DDX(bUpdate, this.comboBox16  , ref m_ss.D38_434_PSG0);

				DDV.DDX(bUpdate, this.textBox36, ref m_ss.SCL_MAX_D38);
				DDV.DDX(bUpdate, this.textBox37, ref m_ss.SCL_MIN_D38);
				DDV.DDX(bUpdate, this.textBox1 , ref m_ss.THD_VAL_PS );
				DDV.DDX(bUpdate, this.textBox2 , ref m_ss.THD_LES_STR);
				DDV.DDX(bUpdate, this.textBox3 , ref m_ss.THD_OVR_STR);
				DDV.DDX(bUpdate, this.numericUpDown3, ref m_ss.THD_FNT_SIZ);
#endif
#if true//2019.09.06(近接時間測定)
				DDV.DDX(bUpdate, this.checkBox1, ref m_ss.THD_MES_TIM);
				DDV.DDX(bUpdate, this.checkBox2, ref m_ss.EXT_MES_TIM);
#endif

                rc = true;
            }
            catch (Exception e) {
                G.mlog(e.Message);
                rc = false;
            }
            return (rc);
		}
#if true//2019.09.06(近接時間測定)
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (sender == this.checkBox1 && this.checkBox1.Enabled == false) {
				return;
			}
			if (sender == this.checkBox2 && this.checkBox2.Enabled == false) {
				return;
			}
			if (this.checkBox1.Checked || this.checkBox2.Checked) {
				this.textBox2.Enabled = false;
				this.textBox3.Enabled = false;
			}
			else {
				this.textBox2.Enabled = true;
				this.textBox3.Enabled = true;
			}
			if (this.checkBox2.Checked && sender == this.checkBox1) {
				this.checkBox2.Enabled = false;
				this.checkBox2.Checked = false;
				this.checkBox2.Enabled = true;
			}
			if (this.checkBox1.Checked && sender == this.checkBox2) {
				this.checkBox1.Enabled = false;
				this.checkBox1.Checked = false;
				this.checkBox1.Enabled = true;
			}
		}
#endif
	}
}

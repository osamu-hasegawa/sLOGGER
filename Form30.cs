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
	public partial class Form30 : Form
	{
		//public string	m_ser1;

		public G.SYSSET	m_ss;
		private double m_integ;
		//private double m_srate;
		private double[] m_duty = {0,0};
		public Form30()
		{
			InitializeComponent();
		}

		private void Form30_Load(object sender, EventArgs e)
		{
#if false
			m_integ = (256-m_sp.D29_010_ATIM)*2.4;
			m_duty[0] = m_sp.PWM_LED_DTY[0];
			m_duty[1] = m_sp.PWM_LED_DTY[1];
#endif
			//m_srate = 1000;
			DDX(true);
		}

		private void Form30_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK) {
				return;
			}
			if (DDX(false) == false) {
				e.Cancel = true;
			}
			else {/*
				for (int i = 0; i < 4; i++) {
					bool flag = false;
					if (m_sp.PLM_LSPD[i] != G.SS.PLM_LSPD[i]) { flag = true; }
					if (m_sp.PLM_JSPD[i] != G.SS.PLM_JSPD[i]) { flag = true; }
					if (m_sp.PLM_HSPD[i] != G.SS.PLM_HSPD[i]) { flag = true; }
					if (m_sp.PLM_ACCL[i] != G.SS.PLM_ACCL[i]) { flag = true; }
					if (m_sp.PLM_MLIM[i] != G.SS.PLM_MLIM[i]) { flag = true; }
					if (m_sp.PLM_PLIM[i] != G.SS.PLM_PLIM[i]) { flag = true; }
					if (flag) {
						G.mlog("#i速度、加速度、リミットの設定変更は「CONNECT」ボタン押下時に反映されます。");
						break;
					}
				}*/
			}
		}
		private bool DDX(bool bUpdate)
        {
            bool rc=false;
			//
			try {
#if false
				DDV.DDX(bUpdate, this.textBox4, ref m_sp.DEV_ID          );

				DDV.DDX(bUpdate, this.numericUpDown1, ref m_integ          );
				DDV.DDX(bUpdate, this.comboBox2     , ref m_sp.D29_0F0_GAIN);
				//---
				DDV.DDX(bUpdate, this.comboBox3     , ref m_sp.D76_F20_HUMI);
				DDV.DDX(bUpdate, this.comboBox4     , ref m_sp.D76_F42_PRES);
				DDV.DDX(bUpdate, this.comboBox5     , ref m_sp.D76_F45_TEMP);
				DDV.DDX(bUpdate, this.comboBox6     , ref m_sp.D76_F52_FILT);
				DDV.DDX(bUpdate, this.comboBox7     , ref m_sp.D76_F55_STBY);
				//---
				DDV.DDX(bUpdate, this.numericUpDown4, ref m_sp.D68_190_SMRT);
				DDV.DDX(bUpdate, this.comboBox8     , ref m_sp.D68_1A0_GLPF);
				DDV.DDX(bUpdate, this.comboBox9     , ref m_sp.D68_1B3_GSCL);
				DDV.DDX(bUpdate, this.comboBox10    , ref m_sp.D68_1D0_ALPF);
				DDV.DDX(bUpdate, this.comboBox11    , ref m_sp.D68_1C3_ASCL);
#endif
				//---
				DDV.DDX(bUpdate, this.comboBox12    , ref m_ss.D38_410_MSTM);
				DDV.DDX(bUpdate, this.comboBox13    , ref m_ss.D38_424_ALG1);
				DDV.DDX(bUpdate, this.comboBox14    , ref m_ss.D38_422_ALG2);
				DDV.DDX(bUpdate, this.comboBox15    , ref m_ss.D38_420_LEDC);
				DDV.DDX(bUpdate, this.comboBox16    , ref m_ss.D38_434_PSG0);
#if false
				//---
				DDV.DDX(bUpdate, this.comboBox17    , ref m_sp.D38_410_MSTM[1]);
				DDV.DDX(bUpdate, this.comboBox18    , ref m_sp.D38_424_ALG1[1]);
				DDV.DDX(bUpdate, this.comboBox19    , ref m_sp.D38_422_ALG2[1]);
				DDV.DDX(bUpdate, this.comboBox20    , ref m_sp.D38_420_LEDC[1]);
				DDV.DDX(bUpdate, this.comboBox21    , ref m_sp.D38_434_PSG0[1]);
				//---
				DDV.DDX(bUpdate, this.comboBox22    , ref m_sp.D38_410_MSTM[2]);
				DDV.DDX(bUpdate, this.comboBox23    , ref m_sp.D38_424_ALG1[2]);
				DDV.DDX(bUpdate, this.comboBox24    , ref m_sp.D38_422_ALG2[2]);
				DDV.DDX(bUpdate, this.comboBox25    , ref m_sp.D38_420_LEDC[2]);
				DDV.DDX(bUpdate, this.comboBox26    , ref m_sp.D38_434_PSG0[2]);
				//---
				DDV.DDX(bUpdate, this.numericUpDown2, ref m_duty[0]);
				DDV.DDX(bUpdate, this.numericUpDown3, ref m_duty[1]);
				//---
				DDV.DDX(bUpdate, this.textBox76     , ref m_sp.ADC_COF_GRD[1]);
				DDV.DDX(bUpdate, this.textBox1      , ref m_sp.ADC_COF_OFS[1]);
				//---
				DDV.DDX(bUpdate, this.textBox3      , ref m_sp.ADC_COF_GRD[2]);
				DDV.DDX(bUpdate, this.textBox2      , ref m_sp.ADC_COF_OFS[2]);
				//---
				DDV.DDX(bUpdate, this.textBox10, ref m_sp.SEN_COF_GRD[0]);
				DDV.DDX(bUpdate, this.textBox11, ref m_sp.SEN_COF_OFS[0]);
				DDV.DDX(bUpdate, this.textBox12, ref m_sp.SEN_COF_GRD[1]);
				DDV.DDX(bUpdate, this.textBox13, ref m_sp.SEN_COF_OFS[1]);
				DDV.DDX(bUpdate, this.textBox14, ref m_sp.SEN_COF_GRD[2]);
				DDV.DDX(bUpdate, this.textBox15, ref m_sp.SEN_COF_OFS[2]);
				DDV.DDX(bUpdate, this.textBox16, ref m_sp.SEN_COF_GRD[3]);
				DDV.DDX(bUpdate, this.textBox17, ref m_sp.SEN_COF_OFS[3]);
				DDV.DDX(bUpdate, this.textBox18, ref m_sp.SEN_COF_GRD[4]);
				DDV.DDX(bUpdate, this.textBox19, ref m_sp.SEN_COF_OFS[4]);
				DDV.DDX(bUpdate, this.textBox20, ref m_sp.SEN_COF_GRD[5]);
				DDV.DDX(bUpdate, this.textBox21, ref m_sp.SEN_COF_OFS[5]);
				DDV.DDX(bUpdate, this.textBox22, ref m_sp.SEN_COF_GRD[6]);
				DDV.DDX(bUpdate, this.textBox23, ref m_sp.SEN_COF_OFS[6]);
				DDV.DDX(bUpdate, this.textBox24, ref m_sp.SEN_COF_GRD[7]);
				DDV.DDX(bUpdate, this.textBox25, ref m_sp.SEN_COF_OFS[7]);
				DDV.DDX(bUpdate, this.textBox26, ref m_sp.SEN_COF_GRD[8]);
				DDV.DDX(bUpdate, this.textBox27, ref m_sp.SEN_COF_OFS[8]);
				DDV.DDX(bUpdate, this.textBox28, ref m_sp.SEN_COF_GRD[9]);
				DDV.DDX(bUpdate, this.textBox29, ref m_sp.SEN_COF_OFS[9]);
				DDV.DDX(bUpdate, this.textBox30, ref m_sp.SEN_COF_GRD[10]);
				DDV.DDX(bUpdate, this.textBox31, ref m_sp.SEN_COF_OFS[10]);
				DDV.DDX(bUpdate, this.textBox32, ref m_sp.SEN_COF_GRD[11]);
				DDV.DDX(bUpdate, this.textBox33, ref m_sp.SEN_COF_OFS[11]);
				DDV.DDX(bUpdate, this.textBox34, ref m_sp.SEN_COF_GRD[12]);
				DDV.DDX(bUpdate, this.textBox35, ref m_sp.SEN_COF_OFS[12]);
				DDV.DDX(bUpdate, this.textBox36, ref m_sp.SEN_COF_GRD[13]);
				DDV.DDX(bUpdate, this.textBox37, ref m_sp.SEN_COF_OFS[13]);
				DDV.DDX(bUpdate, this.textBox38, ref m_sp.SEN_COF_GRD[14]);
				DDV.DDX(bUpdate, this.textBox39, ref m_sp.SEN_COF_OFS[14]);
				DDV.DDX(bUpdate, this.textBox40, ref m_sp.SEN_COF_GRD[15]);
				DDV.DDX(bUpdate, this.textBox41, ref m_sp.SEN_COF_OFS[15]);
				DDV.DDX(bUpdate, this.textBox42, ref m_sp.SEN_COF_GRD[16]);
				DDV.DDX(bUpdate, this.textBox43, ref m_sp.SEN_COF_OFS[16]);
				DDV.DDX(bUpdate, this.textBox44, ref m_sp.SEN_COF_GRD[17]);
				DDV.DDX(bUpdate, this.textBox45, ref m_sp.SEN_COF_OFS[17]);
				DDV.DDX(bUpdate, this.textBox46, ref m_sp.SEN_COF_GRD[18]);
				DDV.DDX(bUpdate, this.textBox47, ref m_sp.SEN_COF_OFS[18]);
				//ADC基準
				//ADC基準
				DDV.DDX(bUpdate, this.textBox48, ref m_sp.SEN_COF_GRD[20]);//圧力
				DDV.DDX(bUpdate, this.textBox49, ref m_sp.SEN_COF_OFS[20]);//圧力
				DDV.DDX(bUpdate, this.textBox50, ref m_sp.SEN_COF_GRD[21]);//電池
				DDV.DDX(bUpdate, this.textBox51, ref m_sp.SEN_COF_OFS[21]);//電池
				//-----
#endif
                rc = true;
            }
            catch (Exception e)
            {
                G.mlog(e.Message);
                rc = false;
            }
            return (rc);
		}

		private void Form30_Validating(object sender, CancelEventArgs e)
		{
			if (DDX(false) == false) {
				e.Cancel = true;
			}
			else {
#if false
				m_sp.D29_010_ATIM = 256 - (int)(m_integ / 2.4);
				m_sp.PWM_LED_DTY[0] = (int)m_duty[0];
				m_sp.PWM_LED_DTY[1] = (int)m_duty[1];
#endif
			}
		}

		private void numericUpDown4_ValueChanged(object sender, EventArgs e)
		{
			double f = (double)this.numericUpDown4.Value;
			f = 1000/(1+f);
			this.label9.Text = string.Format("{0:F1}Hz", f);
		}
	}
}

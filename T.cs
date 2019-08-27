using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace sLOGGER
{
    class T
    {
		static public string GetDocFolder()
		{
			return(GetDocFolder(null));
		}
		static public string GetDocFolder(string file)
		{
			string	path;
//			path = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
//			path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			path += "\\KOP";
			if (!System.IO.Directory.Exists(path)) {
				System.IO.Directory.CreateDirectory(path);
			}
			path += "\\" + Application.ProductName;
			if (!System.IO.Directory.Exists(path)) {
				System.IO.Directory.CreateDirectory(path);
			}
			if (file != null && file.Length > 0) {
				path += "\\" + file;
			}
			return(path);
		}		//
		static public double DT2DBL(DateTime tm)
		{
			DateTime	tmBase = new DateTime(2007,1,1,0,0,0);
			const
			double		dbBase = 63303206400.0;
			TimeSpan	span;

			span = tm-tmBase;
			return(dbBase + span.TotalSeconds);
		}
		/**/
		static public DateTime DBL2DT(double f)
		{
			DateTime tm;
			DateTime	tmBase = new DateTime(2007,1,1,0,0,0);
			const
			double		dbBase = 63303206400.0;

			tm = tmBase.AddSeconds(f-dbBase);
			return(tm);
		}
		static public bool IsNumeric(string str)
        {
            double dNullable;

            return double.TryParse(
                str,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }
		/**/
		static public double AXpB(double x1, double x2, double y1, double y2, double x)
		{
			double	A, B;
			double	Y;
			if ((x2 - x1) == 0.0) {
				return(-99999);
			}
			else if (x == x1) {
				Y = y1;
			}
			else if (x == x2) {
				Y = y2;
			}
			else {
				A = (y2 - y1) / (x2 - x1);
				B = y1 - A * x1;
				Y = A * x + B;
			}
			return(Y);
		}
		/****************************************************************************/
		/* ラグランジュ補間
		/* x1, x2 ... x ... x3, x4
		/* y1, y2 ... y ... y3, y4
		/****************************************************************************/
		static public double lagran(	double x1, double x2, double x3, double x4,
						double y1, double y2, double y3, double y4,
						double x)
		{
			double	Y1, Y2, Y3, Y4, YY;

		//	if (x <= x3) {
				Y1 = y1 * (x-x2)*(x-x3)*(x-x4) / ((x1-x2)*(x1-x3)*(x1-x4));
				Y2 = y2 * (x-x1)*(x-x3)*(x-x4) / ((x2-x1)*(x2-x3)*(x2-x4));
				Y3 = y3 * (x-x1)*(x-x2)*(x-x4) / ((x3-x1)*(x3-x2)*(x3-x4));
				Y4 = y4 * (x-x1)*(x-x2)*(x-x3) / ((x4-x1)*(x4-x2)*(x4-x3));
				YY = Y1 + Y2 + Y3 + Y4;
		//	}
			return(YY);
		}
		/**/
		static public void MakeLaglan(double[] fWav, double[] fVal, double[] xx, double[] yy)
		{
			double	w1, w2, w3, w4, ww;
			double	v1, v2, v3, v4, vv;
			int		i, h = 0;

			xx[h] = fWav[0];
			yy[h] = fVal[0];
			h++;
			/*
			 Ex. m_cnt: 100
				loop 0-98
			*/
			for (i = 0; i < (fWav.Length-1); i++) {
				if (i == 0) {
					w1 = fWav[0] - 1;
					v1 = fVal[0];
				}
				else {
					w1 = fWav[i-1];
					v1 = fVal[i-1];
				}
				/*if (i == (m_cnt-2)) {
					i = i;
				}*/

				w2 = fWav[i];
				v2 = fVal[i];

				if (i < (fWav.Length-2)) {
					w3 = fWav[i+1];
					v3 = fVal[i+1];
					w4 = fWav[i+2];
					v4 = fVal[i+2];
				}
				else {
					w3 = fWav[i+1];
					v3 = fVal[i+1];
					w4 = fWav[i+1] + 1;
					v4 = fVal[i+1];
				}
				for (ww = w2+1; ww <= w3; ww+=1.0) {	/* w2 ～ w3 の波長間を1nm毎に補間する */
					vv = lagran( w1, w2, w3, w4,
								v1, v2, v3, v4,
								ww);
					xx[h] = ww;
					yy[h] = vv;
					h++;
				}
			}
		}
		/**/
		static public void MakeLinear(double[] fWav, double[] fVal, ref double[] xx, ref double[] yy)
		{
#if true
//			double		v1, v2, v3, v4;
			int			w;
			double		v;
			int			i;
			int			i0, i1/*, i2, i3*/;
			
			bool		bLinearInterpolation = true;
			int			LMIN = (int)fWav[0];
			int			LMAX = (int)fWav[fWav.Length-1];

			/**/
			int		len = (int)(LMAX-LMIN+1);
			xx = new double[len];
			yy = new double[len];
			/**/
			if (bLinearInterpolation) {
				i0 = i1 = 0;
				//for (w = LMIN; w < m_tbl[0].fWaveLength; w++) {
				//	m_dat[w-LMIN] = 0;
				//}
				for (w = LMIN, i = 0; w <= LMAX; i++, w++) {
					//w = LMIN; w <= LMAX; w++) {
					while (fWav[i1] <= w) {
						if ((i1+1) < fWav.Length) {
							i1++;
						}
						else {
							break;
						}
					}
					if (i1 > 0) {
						i0 = i1 - 1;
					}
					else {
						i0 = 0;
					}
					v = AXpB(
							fWav[i0], fWav[i1],
							fVal[i0], fVal[i1],
							w);
//					System.Diagnostics.Debug.Write(String.Format("{0} [{1}-{2}]: {3}\n",
//								w, fWav[i0], fWav[i1], v));
					xx[i] = w;
					yy[i] = v;
				}
			}
#else
			double	w1, w2, ww,vv;
			int		i, h = 0;

			xx[h] = fWav[0];
			yy[h] = fVal[0];
			h++;
			for (i = 0; i < (fWav.Length-1); i++) {
				w1 = fWav[i];
				w2 = fWav[i+1];

				for (ww = w1+1.0; ww <= w2; ww+=1.0) {
					vv = AXpB(
							fWav[i], fWav[i+1],
							fVal[i], fVal[i+1],
							ww);
					xx[h] = ww;
					yy[h] = vv;
					h++;
				}
			}
#endif
		}
		static public double GetAVG(double[] p, int n)
		{
			double	avg = 0;
			int		i;

			for (i = 0; i < n; i++) {
				avg += p[i];
			}
			avg /= n;
			return(avg);
		}
		static public double GetSTD(double[] p, int n)
		{
			double	avg = GetAVG(p, n);
			int		i;
			double	s=0;
			double	f;

			for (i = 0; i < n; i++) {
				f = (p[i]-avg);
				s += f*f;
			}
			s /= n;			// <- 分散
			s = Math.Sqrt(s);	// <- 標準偏差
			return(s);
		}
		//static public DateTime HM2DT(DateTime dt, double secs1, double secs2)
		//{
		//}
		//static public double GetTicByTime(DateTime tm1, DateTime tm2)
		//{
		//}
		static public Color GetColor(int idx)
		{
			Color	col;
#if false
			switch (idx) {
				case 0: col = G.SS.gcol[11]; break;	/*G*/
				case 1: col = G.SS.gcol[12]; break;	/*B*/
				case 2: col = G.SS.gcol[13]; break;	/*R*/
				case 3: col = G.SS.gcol[14]; break;
				case 4: col = G.SS.gcol[15]; break;
				case 5: col = G.SS.gcol[16]; break;
				case 6: col = G.SS.gcol[17]; break;
				default:col = G.SS.gcol[18]; break;
			}
#else
			switch (idx%8) {
				case 0: col = Color.Lime; break;	/*G*/
				case 1: col = Color.Blue; break;	/*B*/
				case 2: col = Color.Red; break;		/*R*/
				case 3: col = Color.Yellow; break;
				case 4: col = Color.Violet; break;
				case 5: col = Color.Cyan; break;
				case 6: col = Color.Orange; break;
				default:col = Color.LightGray; break;
			}
#endif
			return(col);
		}
		///************************************************************/
		//static public String GRID2CSV(DataGridView dg)
		//{
		//}
		static public string DT2STR(DateTime dt)
		{
			return(String.Format(
				"{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
					dt.Year,
					dt.Month,
					dt.Day,
					dt.Hour,
					dt.Minute,
					dt.Second)
			);
		}
 		/************************************************************/
		static private Color GetHoshoku(Color col)
		{
			int	r = col.R,
				g = col.G,
				b = col.B;
			r = 255-r;
			g = 255-g;
			b = 255-b;
			return(Color.FromArgb(r, g, b));
		}
		///************************************************************/
		//static private int M2D(float mm)
		//{
		//}
		///************************************************************/
		//static public void PrintGridPairStr(Graphics g, Rectangle rt, ArrayList ary, int c_max, int r_max)
		//{
		//}
		///************************************************************/
		//static public void PrintDataGrid(DataGridView grid, Graphics g, Rectangle rt, int c_max, int r_max)
		//{
		//}
		///************************************************************/
		//static public void PrintDataGrid2(DataGridView grid, Graphics g, Rectangle rt, int c_max, int r_max)
		//{
		//}
		///************************************************************/
		//static public void PrintDataGrid3(DataGridView grid, Graphics g, Rectangle rt, int C_WID, int C_HEI, float[] C_WID_TIMES, String title)
		//{
		//}
		///************************************************************/
		//static public void PrintText(Graphics g, Rectangle rt, int rows, int clms, ArrayList strs)
		//{
		//}
		///************************************************************/
		//static public void PrintDate(Graphics g, Rectangle rt, DateTime dt)
		//{
		//}
		///************************************************************/
		//static public void GetPairStrGrid(DataGridView grid, ArrayList ary)
		//{
		//}
		///************************************************************/
		//static public Rectangle GetYohakuRect(Rectangle rtBounds)
		//{
		//}
		///************************************************************/
		//static public void GetCellSize(out int C_WID, out int C_HEI)
		//{
		//    C_WID = M2D(20);
		//    C_HEI = M2D( 7);
		//}
		/************************************************************/
		static public double GetMax(double[] f)
		{
			double	max = f[0];

			for (int i = 1; i < f.Length; i++) {
				if (max < f[i]) {
					max = f[i];
				}
			}
			return(max);
		}
		/************************************************************/
		static public void GetLstMst(double[] ary, out int lst, out int mst)
		{
			lst = (int)ary[0];
			mst = (int)ary[ary.Length-1];
		}
		/************************************************************/
		/*
		static public double GetNearWavVal(double[] wav, double[] val, int wget)
		{
			int		idx = 0;
			double	min = double.MaxValue,
					dif;

			if (wav.Length != val.Length) {
				throw new Exception("logical error");
			}
			for (int i = 0; i < wav.Length; i++) {
				dif = Math.Abs(wav[i]-wget);
				if (dif < min) {
					min = dif;
					idx = i;
					if (min == 0) {
						break;
					}
				}
			}
			if (min != 0) {
				Debug.WriteLine(String.Format("MIN={0}", min));
			}
			return(val[idx]);
		}*/
		static private void SET_TAB_SEL_ENTER(object sender, EventArgs e)
		{
			TextBox t;
			if (false) {
			}
			else if (sender.GetType() == typeof(TextBox)) {
				t = (TextBox)sender;
			}
			else if (sender.GetType() == typeof(NumericUpDown)) {
				t = (TextBox)((NumericUpDown)sender).Controls[1];
			}
			else {
				return;
			}
			if (t.Text.Length > 0) {
				t.SelectAll();
			}
		}
		/*
		 * TextBoxフォーカス移動時の全選択処理
		 */
		static public int SET_TAB_SEL(Control ctl)
		{
			int	cnt = 0;
			foreach (Control c in ctl.Controls) {
				if (c.Controls.Count > 0) {
					if (c.HasChildren == false) {
						//int j = 0;
					}
					cnt += SET_TAB_SEL(c);
				}
				if (c.GetType() == typeof(TextBox)
				 || c.GetType() == typeof(NumericUpDown)
					) {
					c.Enter += new EventHandler(SET_TAB_SEL_ENTER);
					cnt++;
				}
			}
			return(cnt);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//---
using System.Drawing;
using System.Windows.Forms;

namespace sLOGGER
{
	public struct RECT {
		public int	left, top, bottom, right;
		public void Offset(int dx, int dy) {
			left -= dx;
			right -= dx;
			top -= dy;
			bottom -= dy;
		}
		public RECT(Rectangle rt) {
			this.left   = rt.Left;
			this.right  = rt.Right;
			this.top    = rt.Top;
			this.bottom = rt.Bottom;
		}
		public int Width() {
			return(this.right-this.left);
		}
		public int Height() {
			return(this.bottom-this.top);
		}
		static public implicit operator Rectangle(RECT rt)
		{
			return(new Rectangle(rt.left, rt.top, rt.Width(), rt.Height()));
		}
	};
	public struct GDEF {
		public RECT	rt_cl;			//クライアント領域全体
		public RECT	rt_gr;			//グラフエリア
		public int		rt_wid;		//グラフエリア幅
		public int		rt_hei;		//グラフエリア高
		//---
		public double	xmin;
		public double	xmax;
		public double	xtic;
		public double	ymin;
		public double	ymax;
		public double	ytic;
		//---
		public int		bitLBL;		//1:X軸ラベル, 2:Y軸ラベル
		public int		bGRID_X;
		public int		bGRID_Y;
		public int		bTIC_X;
		public int		bTIC_Y;
		public int		bNUM_X;
		public int		bNUM_Y;
		//---
		public double	xwid;
		public double	yhei;
		//---
		public string	lblTitle;
		public string	lblXAxis;
		public string	lblYAxis;
		//---
	} ;
	class ChartSub
	{
		private
		GDEF	pdef;
		public
		RingBuff
				rbuf = new RingBuff(3000);
		PictureBox pbox;
		public
		double interval = 0.1;
//		double interval = C.INTERVAL/1000.0;
//		public Label	lbl;
		public int th_bak = -1;
		public Font m_fnt = null;
		void Draw4Edge(Graphics pDC, RECT rt, Pen pen)
		{
			const
			int GAP = 0;
			Point[] pts = new Point[5];
			pts[0] = new Point(rt.left, rt.top);
			pts[1] = new Point(rt.right + GAP, rt.top);
			pts[2] = new Point(rt.right + GAP, rt.bottom + GAP);
			pts[3] = new Point(rt.left, rt.bottom + GAP);
			pts[4] = new Point(rt.left, rt.top - GAP);
			pDC.DrawLines(pen, pts);
		}
		public void GDEF_INIT(/*GDEF pdef,*/int bitLBL, Control pWnd, Rectangle poff)
		{
			RECT	rt = new RECT(pWnd.ClientRectangle);
			rt.Offset(-rt.left, -rt.top);//(0,0)基準にする
			pdef.rt_cl = rt;
			//---
			rt.top    += 5;
			rt.bottom -= 7;
			rt.left   += 5;
			rt.right  -= 5;	
			if ((bitLBL & 1) != 0) {
				//X軸にラベル表示
				rt.bottom -= 8;
				rt.right  -= 7;
			}
			if ((bitLBL & 2) != 0) {
				//Y軸にラベル表示
				rt.left += 16;//+7:符号があるとき
				rt.top  += 1;
			}
			if (poff != null) {
				rt.top    += poff.Top;
				rt.bottom += poff.Bottom;
				rt.left   += poff.Left;
				rt.right  += poff.Right;
			}
			//---
			pdef.rt_gr = rt;
			//---
			pdef.rt_wid = rt.Width();
			pdef.rt_hei = rt.Height();
			//---
			pdef.bitLBL = bitLBL;
			//---
			this.pbox = (PictureBox)pWnd;
		}
		public void GDEF_LSET(string title, string unit)
		{
			pdef.lblTitle = title;
			pdef.lblYAxis = unit;
		}
		public void GDEF_PSET(/*GDEF pdef,*/double xmin, double xmax, double xtic, double ymin, double ymax, double ytic)
		{
			if (!double.IsNaN(xmin) && !double.IsNaN(xmax)) {
				pdef.xmin = xmin;
				pdef.xmax = xmax;
			}
			if (!double.IsNaN(xtic)) {
				pdef.xtic = xtic;
			}
			else {
				pdef.xtic =(pdef.xmax - pdef.xmin)/2;
			}
			if (!double.IsNaN(ymin) && !double.IsNaN(ymax)) {
				pdef.ymin = ymin;
				pdef.ymax = ymax;
			}
			if (!double.IsNaN(ytic)) {
				pdef.ytic = ytic;
			}
			else {
				pdef.ytic =(pdef.ymax - pdef.ymin)/2;
			}
			//---
			pdef.xwid = pdef.xmax-pdef.xmin;
			pdef.yhei = pdef.ymax-pdef.ymin;
		}
		public int GDEF_XPOS(/*GDEF pdef,*/double f)
		{
			int	x;
			x = (int)(pdef.rt_gr.left + pdef.rt_wid * (f-pdef.xmin)/(pdef.xwid));
			return(x);
		}
		public int GDEF_YPOS(/*GDEF pdef,*/double f)
		{
			int	y;
			if (true) {
				pdef.yhei = pdef.ymax-pdef.ymin;
			}
			if (pdef.yhei == 0) {
				return(0);
			}
			y = (int)(pdef.rt_gr.bottom - pdef.rt_hei * (f-pdef.ymin)/(pdef.yhei));
			return(y);
		}
		public void GDEF_GRID(/*GDEF pdef,*/Graphics pDC)
		{
			Pen		penLine = Pens.White;
			Pen		penEdge = Pens.White;
		//	Brush	brsBody = new SolidBrush(Color.FromArgb(224,224,224));
		//	Brush	brsBody = Brushes.LightGreen;
			Brush	brsBody = Brushes.Black;
			Brush	brsBack = Brushes.Black;
			Brush	brsText = Brushes.White;
			//CPen*	old_pen = (CPen*)pDC.SelectStockObject(BLACK_PEN);
			//CFont*	old_fnt = (CFont*)pDC.SelectObject(CKOP::GET_FONT(6));
			RECT	rt;
			string	fmt, buf;
			bool	bZERO;
			bool	bMAXMIN;
		//	double	ep=0.1;
			int		dot, tmp;
			const
			int		SLEN=3;	
		#if true//2015.07.24
			//pDC.FillRectangle(0, 0, pdef.rt_cl.right-2, pdef.rt_cl.bottom-2, RGB(224,224,224));
			pDC.FillRectangle(brsBack,0, 0, pdef.rt_cl.right/*-2*/, pdef.rt_cl.bottom/*-2*/);
		#else
			pDC.FillSolidRect(&pdef.rt_cl, RGB(224,224,224));
		#endif
			pDC.FillRectangle(brsBody, pdef.rt_gr);
			Draw4Edge(pDC, pdef.rt_gr, penEdge);
			//--------------------------------
			rt = pdef.rt_gr;
			rt.right = rt.left-SLEN+1;
			rt.left  = rt.right-100;
			//Y軸のめもり描画
			EnumGridFirst(pdef.ymax, pdef.ymin, pdef.ytic, pdef.rt_hei, out fmt, /*bALIGN=*/false);
			while (EnumGridNext(out dot, out buf, out bZERO, 0.1, out bMAXMIN)) {
				tmp = pdef.rt_gr.bottom - dot;
				rt.top    = tmp - 300;
				rt.bottom = tmp + 300;

		//		pDC.SelectObject(&rpen);//太
				pDC.DrawLine(penLine, pdef.rt_gr.left, tmp, pdef.rt_gr.left-SLEN, tmp);
				//pDC.MoveTo(pdef.rt_gr.left     , tmp);
				//pDC.LineTo(pdef.rt_gr.left-SLEN, tmp);
				if ((pdef.bitLBL & 2)!=0) {
					//pDC.DrawText(buf, -1, &rt, DT_SINGLELINE|DT_RIGHT|DT_VCENTER);
					DrawText(pDC, brsText, buf, rt, StringAlignment.Center, StringAlignment.Far);
				}
				if (bMAXMIN) {
					continue;
				}
				if (false) {
				//pDC.MoveTo(pdef.rt_gr.left , tmp);
				//pDC.LineTo(pdef.rt_gr.right, tmp);
				}
			}
			//Ｘ軸のめもり描画
			//--------------------------------
			rt = pdef.rt_gr;
			rt.top    = rt.bottom+SLEN;
			rt.bottom = rt.top + 100;
			//--------------------------------
			if (true) {
		//		int		d;
				//bool bZERO, bMAXMIN;

				EnumGridFirst(pdef.xmax, pdef.xmin, pdef.xtic, pdef.rt_wid, out fmt, true);
				while (EnumGridNext(out dot, out buf, out bZERO, 0.1, out bMAXMIN)) {
					tmp = pdef.rt_gr.left + dot;
					rt.left  = tmp - 500;
					rt.right = tmp + 500;

		//			pDC.SelectObject(&rpen);//太
					//pDC.MoveTo(tmp, pdef.rt_gr.bottom     );
					//pDC.LineTo(tmp, pdef.rt_gr.bottom+SLEN);
					pDC.DrawLine(penLine, tmp, pdef.rt_gr.bottom, tmp, pdef.rt_gr.bottom+SLEN);
					if ((pdef.bitLBL & 1)!=0) {
						//pDC.DrawText(buf, -1, &rt, DT_SINGLELINE|DT_CENTER|DT_TOP);
						DrawText(pDC, brsText, buf, rt, StringAlignment.Near, StringAlignment.Center);
					}
					if (bMAXMIN) {
						continue;
					}
					//if (!bZERO) {
					//pDC.SelectObject(&gpen);//細
					//}
					if (false) {
					//pDC.MoveTo(tmp, pdef.rt_gr.top   );
					//pDC.LineTo(tmp, pdef.rt_gr.bottom);
					}
				}
			}
			if (true) {
				rt = pdef.rt_gr;
				rt.top    = pdef.rt_cl.top;
				rt.bottom = pdef.rt_gr.top;
				buf = pdef.lblTitle + " " + pdef.lblYAxis;
				DrawText(pDC, brsText, buf, rt, StringAlignment.Center, StringAlignment.Near);
			}
			//pDC.SelectObject(old_fnt);
			//pDC.SelectObject(old_pen);
			if (true) {
				Pen		penDash = new Pen(Color.White);
				int y = GDEF_YPOS(G.SS.THD_VAL_PS);
				penDash.DashStyle =  System.Drawing.Drawing2D.DashStyle.Dot;
				pDC.DrawLine(penDash, pdef.rt_gr.left, y, pdef.rt_gr.right, y);
			}
#if true
			if (true) {
				Point pt = new Point(pdef.rt_gr.left + 10, pdef.rt_gr.top + 10);
				DrawText(pDC, G.SS.THD_OVR_STR, pt, Color.White, Color.White, "MS UI Gothic", G.SS.THD_FNT_SIZ);
				th_bak = -1;
			}
#endif

		}
		void DrawText(Graphics pDC, string buf, Point pt, Color fcolor, Color bcolor, string fname="Arial", int fsize=10)
		{
			// Create font and brush.
			Font drawFont = new Font(fname, fsize);
 
			TextRenderer.DrawText(pDC, buf, drawFont, pt, fcolor, bcolor);


			drawFont.Dispose();
			drawFont = null;
		}
		void DrawText(Graphics pDC, Brush brs, string buf, RECT rt, StringAlignment vfmt, StringAlignment hfmt)
		{
			// Create font and brush.
			Font drawFont = new Font("Arial", 10);
//			SolidBrush drawBrush = new SolidBrush(Color.Black);
			RectangleF drawRect = (Rectangle)rt;
             
			// Draw rectangle to screen.
			//Pen blackPen = new Pen(Color.Black);
			//pDC.DrawRectangle(blackPen, rt);
             
			// Set format of string.
			StringFormat drawFormat = new StringFormat();
			drawFormat.Alignment = hfmt;
			drawFormat.LineAlignment = vfmt;
             
			// Draw string to screen.
			pDC.DrawString(buf, drawFont, brs, drawRect, drawFormat);
			drawFont.Dispose();
			drawFont = null;
		}
		private const int MIN_OF_FLT_DGP = 4;
		/****************************************************************************/
		int GetPeriod(double f) 
		{
			string	tmp;
			int		h, i, l;

			tmp = string.Format("{0:F6}", f);
			l = tmp.Length;

			for (i = l - 1; tmp[i] != '.' && tmp[i] == '0'; i--) ;
			if (tmp[i] == '.') {
				return(0);		/* 少数点以下なし */
			}
			h = i;				/* 小数点以下の有効桁数検索 */
			for (; tmp[i] != '.'; i--) {
			}
			l = (h-i);
		#if true//2015.01.01
			return(l > MIN_OF_FLT_DGP ? MIN_OF_FLT_DGP: l);
		#else
		#if true//2012.06.27
			return(l > 5 ? 5: l);
		#else
			return(l > 4 ? 4: l);
		#endif
		#endif
		}
		//***********************************************************
		double	m_d;
		double	m_dpgm;
		double	m_gmax;
		double	m_gmin;
		double	m_gtic;
		double	m_dots;
		int		m_nums;
		int		m_bs10;
		string	m_fmt;
		//***********************************************************
		void EnumGridFirst(double gmax, double gmin, double gtic, int dots, out string fmt, bool bALIGN)
		{
			int	tmp;
			m_gmax = gmax;
			m_gmin = gmin;
			m_gtic = gtic;
			m_dpgm = dots / (gmax-gmin);
			if (bALIGN) {
				m_d = gtic * (double)((int)(gmin/gtic));
				if (m_d < gmin) {
					m_d += gtic;
				}
			}
			else {
				m_d = gmin;
			}
			tmp = GetPeriod(gtic);
			fmt = "{0:F"+tmp.ToString() + "}";

			m_fmt = fmt;
		}
		//***********************************************************
		bool EnumGridNext(out int pd, out string buf, out bool pbZERO, double ep, out bool pbMAXMIN)
		{
			int		tmp;

			pd = 0;
			buf = null;
			pbZERO = false;
			pbMAXMIN = false;

			if (m_d > (m_gmax+0.001) || m_gtic <= 0.0) {
				return(false);
			}

			while (true) {
				if (m_d <= 1E-15 && m_d >= -1E-15) {
					m_d = 0;
				}
				buf = string.Format(m_fmt, m_d);

				tmp = (int)(m_dpgm*(m_d-m_gmin));

				if (m_d >= m_gmax) {
					pbMAXMIN = true;
				}
				if (m_d <= m_gmin) {
					pbMAXMIN = true;
				}
				if (m_d <= ep && m_d >= -ep) {
					pbZERO = true;
				}
				else {
					pbZERO = false;
				}
				break;
			}
			pd = tmp;
			if (m_d < m_gmax) {
				if ((m_d += m_gtic) > m_gmax) {
#if false//2015.06.18
					m_d = m_gmax;
#endif
				}
			}
			else {
				m_d += m_gtic;
			}
			return (true);
		}
		public void DoAutoScale()
		{
			double fmax, fmin;
			int imax, imin;

			imax = (int)(pdef.xmax / this.interval);
			imin = (int)(pdef.xmin / this.interval);

			this.rbuf.GetMaxMin(imin, imax, out fmax, out fmin);

			if (fmax < 0) {
				fmax = 0;
			}
			else {
				fmax = Math.Ceiling(fmax / 5);
				fmax = fmax*5;
			}
			if (fmin > 0) {
				fmin = 0;
			}
			else {
				fmin = Math.Floor(fmin / 5);
				fmin = fmin * 5;
			}
			if (fmin == fmax) {
				fmax = fmin + 1;
			}
			if (pdef.ymax >= fmax) {
				//return;
			}
			if (fmin >= pdef.ymin && fmax <= pdef.ymax) {
				return;
			}
			GDEF_PSET(pdef.xmin, pdef.xmax, pdef.xtic, fmin, fmax, (fmax-fmin)/2);
			if (true) {
				Graphics g = this.pbox.CreateGraphics();
				GDEF_GRID(g);
				if (imin < this.rbuf.GetiMin()) {
					imin = this.rbuf.GetiMin();
				}
				if (imax > this.rbuf.GetiMax()) {
					imax = this.rbuf.GetiMax();
				}
				if (imax > imin) {
					DrawGraph(g, imin, (imax - imin) + 1);
				}
				g.Dispose();
			}
		}
		public void Update()
		{
			Graphics g = this.pbox.CreateGraphics();
			GDEF_GRID(g);
			g.Dispose();
		}
		public double sub_ymax, sub_ymin, rat_ymax, rat_ymin;

		public bool CheckScale(double f)
		{
			if (sub_ymax < f) {
				sub_ymax = f;
			}
			if (sub_ymin > f) {
				sub_ymin = f;
			}
			double rat;
			if ((rat = f / this.pdef.ymax) > 0) {
				if (rat_ymax < rat) {
					rat_ymax = rat;
				}
			}
			if ((rat = f / this.pdef.ymin) < 0) {
				if (rat_ymin > rat) {
					rat_ymin = rat;
				}
			}
			return (true);
		}
		public void PageFeed()
		{
			this.pdef.xmin += this.pdef.xwid;
			this.pdef.xmax += this.pdef.xwid;
			Update();
		}
		public void AddData(double f, bool bUpdateGraph)
		{
			this.rbuf.add(f);
			if (bUpdateGraph) {
				Graphics g = this.pbox.CreateGraphics();
				DrawGraph(g, this.rbuf.Count() - 1, 1);
				g.Dispose();
			}
#if true
			int flag = (f >= G.SS.THD_VAL_PS) ? 1: 0;
			if (this.rbuf.Count() == 1 || flag != th_bak) {
#if true
				Graphics g = this.pbox.CreateGraphics();
				Point pt = new Point(pdef.rt_gr.left + 10, pdef.rt_gr.top + 10);
				if (flag == 1) {
					DrawText(g, G.SS.THD_OVR_STR, pt, Color.Green, Color.White, "MS UI Gothic", G.SS.THD_FNT_SIZ);
				}
				else {
					DrawText(g, G.SS.THD_LES_STR, pt, Color.Red, Color.White, "MS UI Gothic", G.SS.THD_FNT_SIZ);
				}
				th_bak = flag;
#else
				if (flag) {
					this.lbl.Text = G.SS.THD_OVR_STR;
					this.lbl.ForeColor = Color.Green;
				}
				else {
					this.lbl.Text = G.SS.THD_LES_STR;
					this.lbl.ForeColor = Color.Red;
				}
				th_bak = flag;
				this.lbl.AutoSize = true;
#endif
			}
#endif
		}
		public void DrawGraph(Graphics pDC, int idx, int cnt)
		{
			double fx, fy;
			int i, x, y, xbak=0, ybak=0;
			if (cnt == 1) {
				cnt++;
				idx--;
			}
			if (idx < 0) {
				return;
			}
			pDC.SetClip(pdef.rt_gr);
			try {
				for (i = 0; i < cnt; i++, idx++) {
					fx = idx * interval;
					fy = this.rbuf[idx];
					//---
					if (double.IsNaN(fy)) {
						continue;
					}
					x = GDEF_XPOS(fx);
					y = GDEF_YPOS(fy);
					if ((uint)y == 0x80000000) {
						y = y;
					}
					if (i > 0) {
	//					pDC.DrawLine(Pens.DarkBlue, xbak, ybak, x, y);
	//					pDC.DrawLine(Pens.DarkRed, xbak, ybak, x, y);
						pDC.DrawLine(Pens.Yellow, xbak, ybak, x, y);
					}
					xbak = x;
					ybak = y;
				}
			}
			catch (Exception ex) {
				G.mlog(ex.Message);
			}
			pDC.ResetClip();
		}

	}
}

namespace sLOGGER
{
	partial class Form02
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form02));
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel33 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.button97 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel25 = new System.Windows.Forms.Panel();
			this.panel28 = new System.Windows.Forms.Panel();
			this.panel27 = new System.Windows.Forms.Panel();
			this.panel29 = new System.Windows.Forms.Panel();
			this.panel26 = new System.Windows.Forms.Panel();
			this.panel30 = new System.Windows.Forms.Panel();
			this.panel24 = new System.Windows.Forms.Panel();
			this.panel23 = new System.Windows.Forms.Panel();
			this.panel20 = new System.Windows.Forms.Panel();
			this.panel22 = new System.Windows.Forms.Panel();
			this.panel21 = new System.Windows.Forms.Panel();
			this.panel19 = new System.Windows.Forms.Panel();
			this.panel16 = new System.Windows.Forms.Panel();
			this.panel15 = new System.Windows.Forms.Panel();
			this.panel17 = new System.Windows.Forms.Panel();
			this.panel14 = new System.Windows.Forms.Panel();
			this.panel18 = new System.Windows.Forms.Panel();
			this.panel13 = new System.Windows.Forms.Panel();
			this.panel10 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.panel9 = new System.Windows.Forms.Panel();
			this.panel12 = new System.Windows.Forms.Panel();
			this.panel8 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.panel33.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(350, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "横スケール(s)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(349, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "測定間隔(ms)";
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Increment = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericUpDown2.Location = new System.Drawing.Point(431, 23);
			this.numericUpDown2.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(73, 19);
			this.numericUpDown2.TabIndex = 1;
			this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown2.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDown1.Location = new System.Drawing.Point(431, 1);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(73, 19);
			this.numericUpDown1.TabIndex = 1;
			this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDown1.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// button4
			// 
			this.button4.Enabled = false;
			this.button4.Location = new System.Drawing.Point(258, 1);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(68, 40);
			this.button4.TabIndex = 0;
			this.button4.Text = "STOP";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button3
			// 
			this.button3.Enabled = false;
			this.button3.Location = new System.Drawing.Point(183, 1);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(68, 40);
			this.button3.TabIndex = 0;
			this.button3.Text = "START";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(810, 8);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(96, 30);
			this.button5.TabIndex = 2;
			this.button5.Text = "→TEST.VIEW";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// panel33
			// 
			this.panel33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel33.Controls.Add(this.button2);
			this.panel33.Controls.Add(this.button1);
			this.panel33.Controls.Add(this.label5);
			this.panel33.Controls.Add(this.button97);
			this.panel33.Controls.Add(this.button5);
			this.panel33.Controls.Add(this.button3);
			this.panel33.Controls.Add(this.button4);
			this.panel33.Controls.Add(this.numericUpDown2);
			this.panel33.Controls.Add(this.label2);
			this.panel33.Controls.Add(this.label1);
			this.panel33.Controls.Add(this.numericUpDown1);
			this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel33.Location = new System.Drawing.Point(0, 0);
			this.panel33.Name = "panel33";
			this.panel33.Size = new System.Drawing.Size(1366, 45);
			this.panel33.TabIndex = 3;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(78, 1);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(68, 40);
			this.button2.TabIndex = 240;
			this.button2.Text = "DISCONNECT";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(3, 1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(68, 40);
			this.button1.TabIndex = 239;
			this.button1.Text = "CONNECT";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(549, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(28, 12);
			this.label5.TabIndex = 238;
			this.label5.Text = "SPS:";
			// 
			// button97
			// 
			this.button97.Location = new System.Drawing.Point(666, 3);
			this.button97.Name = "button97";
			this.button97.Size = new System.Drawing.Size(59, 40);
			this.button97.TabIndex = 11;
			this.button97.Text = "設定";
			this.button97.UseVisualStyleBackColor = true;
			this.button97.Click += new System.EventHandler(this.button97_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 45);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1366, 671);
			this.tabControl1.TabIndex = 4;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel25);
			this.tabPage1.Controls.Add(this.panel28);
			this.tabPage1.Controls.Add(this.panel27);
			this.tabPage1.Controls.Add(this.panel29);
			this.tabPage1.Controls.Add(this.panel26);
			this.tabPage1.Controls.Add(this.panel30);
			this.tabPage1.Controls.Add(this.panel24);
			this.tabPage1.Controls.Add(this.panel23);
			this.tabPage1.Controls.Add(this.panel20);
			this.tabPage1.Controls.Add(this.panel22);
			this.tabPage1.Controls.Add(this.panel21);
			this.tabPage1.Controls.Add(this.panel19);
			this.tabPage1.Controls.Add(this.panel16);
			this.tabPage1.Controls.Add(this.panel15);
			this.tabPage1.Controls.Add(this.panel17);
			this.tabPage1.Controls.Add(this.panel14);
			this.tabPage1.Controls.Add(this.panel18);
			this.tabPage1.Controls.Add(this.panel13);
			this.tabPage1.Controls.Add(this.panel10);
			this.tabPage1.Controls.Add(this.panel11);
			this.tabPage1.Controls.Add(this.panel9);
			this.tabPage1.Controls.Add(this.panel12);
			this.tabPage1.Controls.Add(this.panel8);
			this.tabPage1.Controls.Add(this.panel7);
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Controls.Add(this.panel3);
			this.tabPage1.Controls.Add(this.panel4);
			this.tabPage1.Controls.Add(this.panel6);
			this.tabPage1.Controls.Add(this.panel5);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1358, 645);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "グラフ";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel25
			// 
			this.panel25.Location = new System.Drawing.Point(8, 242);
			this.panel25.Name = "panel25";
			this.panel25.Size = new System.Drawing.Size(60, 49);
			this.panel25.TabIndex = 35;
			// 
			// panel28
			// 
			this.panel28.Location = new System.Drawing.Point(227, 242);
			this.panel28.Name = "panel28";
			this.panel28.Size = new System.Drawing.Size(60, 49);
			this.panel28.TabIndex = 36;
			// 
			// panel27
			// 
			this.panel27.Location = new System.Drawing.Point(154, 242);
			this.panel27.Name = "panel27";
			this.panel27.Size = new System.Drawing.Size(60, 49);
			this.panel27.TabIndex = 37;
			// 
			// panel29
			// 
			this.panel29.Location = new System.Drawing.Point(300, 242);
			this.panel29.Name = "panel29";
			this.panel29.Size = new System.Drawing.Size(60, 49);
			this.panel29.TabIndex = 32;
			// 
			// panel26
			// 
			this.panel26.Location = new System.Drawing.Point(81, 242);
			this.panel26.Name = "panel26";
			this.panel26.Size = new System.Drawing.Size(60, 49);
			this.panel26.TabIndex = 33;
			// 
			// panel30
			// 
			this.panel30.Location = new System.Drawing.Point(373, 242);
			this.panel30.Name = "panel30";
			this.panel30.Size = new System.Drawing.Size(60, 49);
			this.panel30.TabIndex = 34;
			// 
			// panel24
			// 
			this.panel24.Location = new System.Drawing.Point(373, 187);
			this.panel24.Name = "panel24";
			this.panel24.Size = new System.Drawing.Size(60, 49);
			this.panel24.TabIndex = 29;
			// 
			// panel23
			// 
			this.panel23.Location = new System.Drawing.Point(300, 187);
			this.panel23.Name = "panel23";
			this.panel23.Size = new System.Drawing.Size(60, 49);
			this.panel23.TabIndex = 30;
			// 
			// panel20
			// 
			this.panel20.Location = new System.Drawing.Point(81, 187);
			this.panel20.Name = "panel20";
			this.panel20.Size = new System.Drawing.Size(60, 49);
			this.panel20.TabIndex = 31;
			// 
			// panel22
			// 
			this.panel22.Location = new System.Drawing.Point(227, 187);
			this.panel22.Name = "panel22";
			this.panel22.Size = new System.Drawing.Size(60, 49);
			this.panel22.TabIndex = 26;
			// 
			// panel21
			// 
			this.panel21.Location = new System.Drawing.Point(154, 187);
			this.panel21.Name = "panel21";
			this.panel21.Size = new System.Drawing.Size(60, 49);
			this.panel21.TabIndex = 27;
			// 
			// panel19
			// 
			this.panel19.Location = new System.Drawing.Point(8, 187);
			this.panel19.Name = "panel19";
			this.panel19.Size = new System.Drawing.Size(60, 49);
			this.panel19.TabIndex = 28;
			// 
			// panel16
			// 
			this.panel16.Location = new System.Drawing.Point(227, 132);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(60, 49);
			this.panel16.TabIndex = 23;
			// 
			// panel15
			// 
			this.panel15.Location = new System.Drawing.Point(154, 132);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(60, 49);
			this.panel15.TabIndex = 24;
			// 
			// panel17
			// 
			this.panel17.Location = new System.Drawing.Point(300, 132);
			this.panel17.Name = "panel17";
			this.panel17.Size = new System.Drawing.Size(60, 49);
			this.panel17.TabIndex = 25;
			// 
			// panel14
			// 
			this.panel14.Location = new System.Drawing.Point(81, 132);
			this.panel14.Name = "panel14";
			this.panel14.Size = new System.Drawing.Size(60, 49);
			this.panel14.TabIndex = 20;
			// 
			// panel18
			// 
			this.panel18.Location = new System.Drawing.Point(373, 132);
			this.panel18.Name = "panel18";
			this.panel18.Size = new System.Drawing.Size(60, 49);
			this.panel18.TabIndex = 21;
			// 
			// panel13
			// 
			this.panel13.Location = new System.Drawing.Point(8, 132);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(60, 49);
			this.panel13.TabIndex = 22;
			// 
			// panel10
			// 
			this.panel10.Location = new System.Drawing.Point(227, 77);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(60, 49);
			this.panel10.TabIndex = 14;
			// 
			// panel11
			// 
			this.panel11.Location = new System.Drawing.Point(300, 77);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(60, 49);
			this.panel11.TabIndex = 15;
			// 
			// panel9
			// 
			this.panel9.Location = new System.Drawing.Point(154, 77);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(60, 49);
			this.panel9.TabIndex = 19;
			// 
			// panel12
			// 
			this.panel12.Location = new System.Drawing.Point(373, 77);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(60, 49);
			this.panel12.TabIndex = 16;
			// 
			// panel8
			// 
			this.panel8.Location = new System.Drawing.Point(81, 77);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(60, 49);
			this.panel8.TabIndex = 18;
			// 
			// panel7
			// 
			this.panel7.Location = new System.Drawing.Point(8, 77);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(60, 49);
			this.panel7.TabIndex = 17;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Location = new System.Drawing.Point(8, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(60, 49);
			this.panel1.TabIndex = 11;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Transparent;
			this.panel2.Location = new System.Drawing.Point(81, 22);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(60, 49);
			this.panel2.TabIndex = 12;
			// 
			// panel3
			// 
			this.panel3.Location = new System.Drawing.Point(154, 22);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(60, 49);
			this.panel3.TabIndex = 13;
			// 
			// panel4
			// 
			this.panel4.Location = new System.Drawing.Point(227, 22);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(60, 49);
			this.panel4.TabIndex = 8;
			// 
			// panel6
			// 
			this.panel6.Location = new System.Drawing.Point(373, 22);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(60, 49);
			this.panel6.TabIndex = 9;
			// 
			// panel5
			// 
			this.panel5.Location = new System.Drawing.Point(300, 22);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(60, 49);
			this.panel5.TabIndex = 10;
			// 
			// Form02
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1366, 716);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel33);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form02";
			this.Text = "sLOGGER:GRAPH(近接専用)";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form02_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form02_FormClosed);
			this.Load += new System.EventHandler(this.Form02_Load);
			this.Resize += new System.EventHandler(this.Form02_Resize);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.panel33.ResumeLayout(false);
			this.panel33.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Panel panel33;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel25;
		private System.Windows.Forms.Panel panel28;
		private System.Windows.Forms.Panel panel27;
		private System.Windows.Forms.Panel panel29;
		private System.Windows.Forms.Panel panel26;
		private System.Windows.Forms.Panel panel30;
		private System.Windows.Forms.Panel panel24;
		private System.Windows.Forms.Panel panel23;
		private System.Windows.Forms.Panel panel20;
		private System.Windows.Forms.Panel panel22;
		private System.Windows.Forms.Panel panel21;
		private System.Windows.Forms.Panel panel19;
		private System.Windows.Forms.Panel panel16;
		private System.Windows.Forms.Panel panel15;
		private System.Windows.Forms.Panel panel17;
		private System.Windows.Forms.Panel panel14;
		private System.Windows.Forms.Panel panel18;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button button97;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;

	}
}
namespace KeyStroke
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ctx1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ni1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.chkCombined = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDisplay = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkBack = new System.Windows.Forms.CheckBox();
            this.chkReturn = new System.Windows.Forms.CheckBox();
            this.chkArrows = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkCTRL = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkWin = new System.Windows.Forms.CheckBox();
            this.chkOEM = new System.Windows.Forms.CheckBox();
            this.chkNum = new System.Windows.Forms.CheckBox();
            this.ctx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ctx1
            // 
            this.ctx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.ctx1.Name = "ctx1";
            this.ctx1.Size = new System.Drawing.Size(104, 54);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ni1
            // 
            this.ni1.ContextMenuStrip = this.ctx1;
            this.ni1.Icon = ((System.Drawing.Icon)(resources.GetObject("ni1.Icon")));
            this.ni1.Text = "KeyStroke";
            this.ni1.Visible = true;
            this.ni1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ni1_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::KeyStroke.Properties.Resources.keyboard;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(183, 162);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(177, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 108);
            this.label1.TabIndex = 2;
            this.label1.Text = "Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(199, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 39);
            this.label2.TabIndex = 3;
            this.label2.Text = "Stroke";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Developed by Afshin Rahimi ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(291, 167);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "arahimi.ca";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // chkCombined
            // 
            this.chkCombined.AutoSize = true;
            this.chkCombined.Location = new System.Drawing.Point(12, 224);
            this.chkCombined.Name = "chkCombined";
            this.chkCombined.Size = new System.Drawing.Size(288, 17);
            this.chkCombined.TabIndex = 6;
            this.chkCombined.Text = "Don\'t show alphabet unless combined with special keys";
            this.chkCombined.UseVisualStyleBackColor = true;
            this.chkCombined.CheckedChanged += new System.EventHandler(this.chkCombined_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Preferences";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Display to show";
            // 
            // cmbDisplay
            // 
            this.cmbDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplay.FormattingEnabled = true;
            this.cmbDisplay.Location = new System.Drawing.Point(100, 248);
            this.cmbDisplay.Name = "cmbDisplay";
            this.cmbDisplay.Size = new System.Drawing.Size(246, 21);
            this.cmbDisplay.TabIndex = 9;
            this.cmbDisplay.SelectedIndexChanged += new System.EventHandler(this.cmbDisplay_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Special Keys to Capture";
            // 
            // chkBack
            // 
            this.chkBack.AutoSize = true;
            this.chkBack.Location = new System.Drawing.Point(15, 312);
            this.chkBack.Name = "chkBack";
            this.chkBack.Size = new System.Drawing.Size(51, 17);
            this.chkBack.TabIndex = 10;
            this.chkBack.Text = "Back";
            this.chkBack.UseVisualStyleBackColor = true;
            this.chkBack.CheckedChanged += new System.EventHandler(this.chkBack_CheckedChanged);
            // 
            // chkReturn
            // 
            this.chkReturn.AutoSize = true;
            this.chkReturn.Location = new System.Drawing.Point(15, 335);
            this.chkReturn.Name = "chkReturn";
            this.chkReturn.Size = new System.Drawing.Size(88, 17);
            this.chkReturn.TabIndex = 11;
            this.chkReturn.Text = "Return/Enter";
            this.chkReturn.UseVisualStyleBackColor = true;
            this.chkReturn.CheckedChanged += new System.EventHandler(this.chkReturn_CheckedChanged);
            // 
            // chkArrows
            // 
            this.chkArrows.AutoSize = true;
            this.chkArrows.Location = new System.Drawing.Point(15, 359);
            this.chkArrows.Name = "chkArrows";
            this.chkArrows.Size = new System.Drawing.Size(79, 17);
            this.chkArrows.TabIndex = 12;
            this.chkArrows.Text = "Arrow Keys";
            this.chkArrows.UseVisualStyleBackColor = true;
            this.chkArrows.CheckedChanged += new System.EventHandler(this.chkArrows_CheckedChanged);
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Location = new System.Drawing.Point(15, 383);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(47, 17);
            this.chkShift.TabIndex = 13;
            this.chkShift.Text = "Shift";
            this.chkShift.UseVisualStyleBackColor = true;
            this.chkShift.CheckedChanged += new System.EventHandler(this.chkShift_CheckedChanged);
            // 
            // chkCTRL
            // 
            this.chkCTRL.AutoSize = true;
            this.chkCTRL.Location = new System.Drawing.Point(15, 407);
            this.chkCTRL.Name = "chkCTRL";
            this.chkCTRL.Size = new System.Drawing.Size(54, 17);
            this.chkCTRL.TabIndex = 14;
            this.chkCTRL.Text = "CTRL";
            this.chkCTRL.UseVisualStyleBackColor = true;
            this.chkCTRL.CheckedChanged += new System.EventHandler(this.chkCTRL_CheckedChanged);
            // 
            // chkAlt
            // 
            this.chkAlt.AutoSize = true;
            this.chkAlt.Location = new System.Drawing.Point(15, 431);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(38, 17);
            this.chkAlt.TabIndex = 15;
            this.chkAlt.Text = "Alt";
            this.chkAlt.UseVisualStyleBackColor = true;
            this.chkAlt.CheckedChanged += new System.EventHandler(this.chkAlt_CheckedChanged);
            // 
            // chkWin
            // 
            this.chkWin.AutoSize = true;
            this.chkWin.Location = new System.Drawing.Point(15, 457);
            this.chkWin.Name = "chkWin";
            this.chkWin.Size = new System.Drawing.Size(91, 17);
            this.chkWin.TabIndex = 16;
            this.chkWin.Text = "Windows Key";
            this.chkWin.UseVisualStyleBackColor = true;
            this.chkWin.CheckedChanged += new System.EventHandler(this.chkWin_CheckedChanged);
            // 
            // chkOEM
            // 
            this.chkOEM.AutoSize = true;
            this.chkOEM.Location = new System.Drawing.Point(15, 481);
            this.chkOEM.Name = "chkOEM";
            this.chkOEM.Size = new System.Drawing.Size(101, 17);
            this.chkOEM.TabIndex = 17;
            this.chkOEM.Text = "OEM ({ } \\ ; \' ...)";
            this.chkOEM.UseVisualStyleBackColor = true;
            this.chkOEM.CheckedChanged += new System.EventHandler(this.chkOEM_CheckedChanged);
            // 
            // chkNum
            // 
            this.chkNum.AutoSize = true;
            this.chkNum.Location = new System.Drawing.Point(15, 506);
            this.chkNum.Name = "chkNum";
            this.chkNum.Size = new System.Drawing.Size(122, 17);
            this.chkNum.TabIndex = 18;
            this.chkNum.Text = "Numbers (1, 2, 3, ...)";
            this.chkNum.UseVisualStyleBackColor = true;
            this.chkNum.CheckedChanged += new System.EventHandler(this.chkNum_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 535);
            this.Controls.Add(this.chkNum);
            this.Controls.Add(this.chkOEM);
            this.Controls.Add(this.chkWin);
            this.Controls.Add(this.chkAlt);
            this.Controls.Add(this.chkCTRL);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkArrows);
            this.Controls.Add(this.chkReturn);
            this.Controls.Add(this.chkBack);
            this.Controls.Add(this.cmbDisplay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkCombined);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KeyStroke";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ctx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ctx1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon ni1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox chkCombined;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDisplay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkBack;
        private System.Windows.Forms.CheckBox chkReturn;
        private System.Windows.Forms.CheckBox chkArrows;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.CheckBox chkCTRL;
        private System.Windows.Forms.CheckBox chkAlt;
        private System.Windows.Forms.CheckBox chkWin;
        private System.Windows.Forms.CheckBox chkOEM;
        private System.Windows.Forms.CheckBox chkNum;
    }
}
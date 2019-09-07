namespace ImageViewer
{
    partial class FormWindows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWindows));
            this.listBoxActiveWindows = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkShowInTaskBar = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnSideBySide = new System.Windows.Forms.Button();
            this.btnCascade = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxActiveWindows
            // 
            this.listBoxActiveWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxActiveWindows.FormattingEnabled = true;
            this.listBoxActiveWindows.Location = new System.Drawing.Point(10, 23);
            this.listBoxActiveWindows.Name = "listBoxActiveWindows";
            this.listBoxActiveWindows.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxActiveWindows.Size = new System.Drawing.Size(294, 259);
            this.listBoxActiveWindows.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkShowInTaskBar);
            this.groupBox1.Controls.Add(this.btnShow);
            this.groupBox1.Controls.Add(this.btnHide);
            this.groupBox1.Controls.Add(this.btnSideBySide);
            this.groupBox1.Controls.Add(this.btnCascade);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnActivate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(319, 5);
            this.groupBox1.MaximumSize = new System.Drawing.Size(140, 0);
            this.groupBox1.MinimumSize = new System.Drawing.Size(140, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 292);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chkShowInTaskBar
            // 
            this.chkShowInTaskBar.AutoSize = true;
            this.chkShowInTaskBar.Checked = true;
            this.chkShowInTaskBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowInTaskBar.Location = new System.Drawing.Point(18, 219);
            this.chkShowInTaskBar.Name = "chkShowInTaskBar";
            this.chkShowInTaskBar.Size = new System.Drawing.Size(102, 17);
            this.chkShowInTaskBar.TabIndex = 6;
            this.chkShowInTaskBar.Text = "Show in taskbar";
            this.chkShowInTaskBar.UseVisualStyleBackColor = true;
            this.chkShowInTaskBar.CheckedChanged += new System.EventHandler(this.chkShowInTaskBar_CheckedChanged);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(18, 176);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(110, 28);
            this.btnShow.TabIndex = 5;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(18, 142);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(110, 28);
            this.btnHide.TabIndex = 4;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnSideBySide
            // 
            this.btnSideBySide.Location = new System.Drawing.Point(18, 91);
            this.btnSideBySide.Name = "btnSideBySide";
            this.btnSideBySide.Size = new System.Drawing.Size(110, 28);
            this.btnSideBySide.TabIndex = 3;
            this.btnSideBySide.Text = "SideBySide";
            this.btnSideBySide.UseVisualStyleBackColor = true;
            this.btnSideBySide.Click += new System.EventHandler(this.btnSideBySide_Click);
            // 
            // btnCascade
            // 
            this.btnCascade.Location = new System.Drawing.Point(18, 57);
            this.btnCascade.Name = "btnCascade";
            this.btnCascade.Size = new System.Drawing.Size(110, 28);
            this.btnCascade.TabIndex = 2;
            this.btnCascade.Text = "Cascade";
            this.btnCascade.UseVisualStyleBackColor = true;
            this.btnCascade.Click += new System.EventHandler(this.btnCascade_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(15, 254);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close Window(s)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(18, 23);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(110, 28);
            this.btnActivate.TabIndex = 0;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxActiveWindows);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(314, 292);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Open Windows";
            // 
            // FormWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 340);
            this.Name = "FormWindows";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWindows_FormClosed);
            this.Load += new System.EventHandler(this.FormWindows_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxActiveWindows;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnSideBySide;
        private System.Windows.Forms.Button btnCascade;
        private System.Windows.Forms.CheckBox chkShowInTaskBar;
    }
}
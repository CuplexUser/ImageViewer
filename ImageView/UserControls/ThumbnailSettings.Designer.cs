namespace ImageViewer.UserControls
{
    partial class ThumbnailSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblThumbnailCount = new System.Windows.Forms.Label();
            this.trackBarThumbnailCount = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb64 = new System.Windows.Forms.RadioButton();
            this.rb128 = new System.Windows.Forms.RadioButton();
            this.rb256 = new System.Windows.Forms.RadioButton();
            this.rb512 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThumbnailCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblThumbnailCount);
            this.groupBox1.Controls.Add(this.trackBarThumbnailCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 225);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // lblThumbnailCount
            // 
            this.lblThumbnailCount.AutoSize = true;
            this.lblThumbnailCount.Location = new System.Drawing.Point(222, 102);
            this.lblThumbnailCount.Name = "lblThumbnailCount";
            this.lblThumbnailCount.Size = new System.Drawing.Size(16, 17);
            this.lblThumbnailCount.TabIndex = 6;
            this.lblThumbnailCount.Text = "0";
            // 
            // trackBarThumbnailCount
            // 
            this.trackBarThumbnailCount.LargeChange = 128;
            this.trackBarThumbnailCount.Location = new System.Drawing.Point(9, 122);
            this.trackBarThumbnailCount.Maximum = 512;
            this.trackBarThumbnailCount.Minimum = 32;
            this.trackBarThumbnailCount.Name = "trackBarThumbnailCount";
            this.trackBarThumbnailCount.Size = new System.Drawing.Size(293, 56);
            this.trackBarThumbnailCount.SmallChange = 32;
            this.trackBarThumbnailCount.TabIndex = 5;
            this.trackBarThumbnailCount.TickFrequency = 32;
            this.trackBarThumbnailCount.Value = 32;
            this.trackBarThumbnailCount.ValueChanged += new System.EventHandler(this.trackBarThumbnailCount_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maximum number of thumbnails:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Thumbnail Resolution:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(219, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(113, 189);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rb64);
            this.panel1.Controls.Add(this.rb128);
            this.panel1.Controls.Add(this.rb256);
            this.panel1.Controls.Add(this.rb512);
            this.panel1.Location = new System.Drawing.Point(161, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(141, 61);
            this.panel1.TabIndex = 0;
            // 
            // rb64
            // 
            this.rb64.AutoSize = true;
            this.rb64.Location = new System.Drawing.Point(64, 30);
            this.rb64.Name = "rb64";
            this.rb64.Size = new System.Drawing.Size(45, 21);
            this.rb64.TabIndex = 3;
            this.rb64.Text = "64";
            this.rb64.UseVisualStyleBackColor = true;
            // 
            // rb128
            // 
            this.rb128.AutoSize = true;
            this.rb128.Location = new System.Drawing.Point(64, 3);
            this.rb128.Name = "rb128";
            this.rb128.Size = new System.Drawing.Size(53, 21);
            this.rb128.TabIndex = 2;
            this.rb128.Text = "128";
            this.rb128.UseVisualStyleBackColor = true;
            // 
            // rb256
            // 
            this.rb256.AutoSize = true;
            this.rb256.Location = new System.Drawing.Point(3, 30);
            this.rb256.Name = "rb256";
            this.rb256.Size = new System.Drawing.Size(53, 21);
            this.rb256.TabIndex = 1;
            this.rb256.Text = "256";
            this.rb256.UseVisualStyleBackColor = true;
            // 
            // rb512
            // 
            this.rb512.AutoSize = true;
            this.rb512.Checked = true;
            this.rb512.Location = new System.Drawing.Point(3, 3);
            this.rb512.Name = "rb512";
            this.rb512.Size = new System.Drawing.Size(53, 21);
            this.rb512.TabIndex = 0;
            this.rb512.TabStop = true;
            this.rb512.Text = "512";
            this.rb512.UseVisualStyleBackColor = true;
            // 
            // ThumbnailSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ThumbnailSettings";
            this.Size = new System.Drawing.Size(330, 235);
            this.Load += new System.EventHandler(this.ThumbnailSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThumbnailCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rb64;
        private System.Windows.Forms.RadioButton rb128;
        private System.Windows.Forms.RadioButton rb256;
        private System.Windows.Forms.RadioButton rb512;
        private System.Windows.Forms.Label lblThumbnailCount;
        private System.Windows.Forms.TrackBar trackBarThumbnailCount;
        private System.Windows.Forms.Label label2;
    }
}

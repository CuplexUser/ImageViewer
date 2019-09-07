namespace ImageViewer
{
    partial class FormThumbnailView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThumbnailView));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.grpBoxControls = new System.Windows.Forms.GroupBox();
            this.btnOptimize = new System.Windows.Forms.Button();
            this.btnScanDirectory = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.picBoxMaximized = new System.Windows.Forms.PictureBox();
            this.contextMenuFullSizeImg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemOpenInDefApp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMaximized)).BeginInit();
            this.contextMenuFullSizeImg.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Location = new System.Drawing.Point(4, 635);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(826, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 57);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(824, 580);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // grpBoxControls
            // 
            this.grpBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxControls.Controls.Add(this.btnOptimize);
            this.grpBoxControls.Controls.Add(this.btnScanDirectory);
            this.grpBoxControls.Controls.Add(this.btnSettings);
            this.grpBoxControls.Controls.Add(this.btnGenerate);
            this.grpBoxControls.Location = new System.Drawing.Point(4, 6);
            this.grpBoxControls.Margin = new System.Windows.Forms.Padding(2);
            this.grpBoxControls.Name = "grpBoxControls";
            this.grpBoxControls.Padding = new System.Windows.Forms.Padding(2);
            this.grpBoxControls.Size = new System.Drawing.Size(827, 45);
            this.grpBoxControls.TabIndex = 2;
            this.grpBoxControls.TabStop = false;
            // 
            // btnOptimize
            // 
            this.btnOptimize.Location = new System.Drawing.Point(243, 15);
            this.btnOptimize.Margin = new System.Windows.Forms.Padding(2);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(69, 24);
            this.btnOptimize.TabIndex = 3;
            this.btnOptimize.Text = "Optimize";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // btnScanDirectory
            // 
            this.btnScanDirectory.Location = new System.Drawing.Point(149, 15);
            this.btnScanDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.btnScanDirectory.Name = "btnScanDirectory";
            this.btnScanDirectory.Size = new System.Drawing.Size(90, 24);
            this.btnScanDirectory.TabIndex = 2;
            this.btnScanDirectory.Text = "Scan Directory";
            this.btnScanDirectory.UseVisualStyleBackColor = true;
            this.btnScanDirectory.Click += new System.EventHandler(this.btnScanDirectory_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(76, 15);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(69, 24);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(2, 15);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(69, 24);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // picBoxMaximized
            // 
            this.picBoxMaximized.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxMaximized.Location = new System.Drawing.Point(7, 56);
            this.picBoxMaximized.Margin = new System.Windows.Forms.Padding(2);
            this.picBoxMaximized.Name = "picBoxMaximized";
            this.picBoxMaximized.Size = new System.Drawing.Size(821, 580);
            this.picBoxMaximized.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxMaximized.TabIndex = 0;
            this.picBoxMaximized.TabStop = false;
            this.picBoxMaximized.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picBoxMaximized_MouseClick);
            // 
            // contextMenuFullSizeImg
            // 
            this.contextMenuFullSizeImg.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuFullSizeImg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenInDefApp,
            this.menuItemBookmark,
            this.menuItemCopyPath});
            this.contextMenuFullSizeImg.Name = "contextMenuFullSizeImg";
            this.contextMenuFullSizeImg.Size = new System.Drawing.Size(222, 70);
            // 
            // menuItemOpenInDefApp
            // 
            this.menuItemOpenInDefApp.Name = "menuItemOpenInDefApp";
            this.menuItemOpenInDefApp.Size = new System.Drawing.Size(221, 22);
            this.menuItemOpenInDefApp.Text = "Open In Default Application";
            this.menuItemOpenInDefApp.Click += new System.EventHandler(this.menuItemOpenInDefApp_Click);
            // 
            // menuItemBookmark
            // 
            this.menuItemBookmark.Name = "menuItemBookmark";
            this.menuItemBookmark.Size = new System.Drawing.Size(221, 22);
            this.menuItemBookmark.Text = "Bookmark Image";
            this.menuItemBookmark.Click += new System.EventHandler(this.menuItemBookmark_Click);
            // 
            // menuItemCopyPath
            // 
            this.menuItemCopyPath.Name = "menuItemCopyPath";
            this.menuItemCopyPath.Size = new System.Drawing.Size(221, 22);
            this.menuItemCopyPath.Text = "Copy Filepath";
            this.menuItemCopyPath.Click += new System.EventHandler(this.menuItemCopyPath_Click);
            // 
            // FormThumbnailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 661);
            this.Controls.Add(this.grpBoxControls);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.picBoxMaximized);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(304, 251);
            this.Name = "FormThumbnailView";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thumbnails";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormThumbnailView_FormClosing);
            this.Load += new System.EventHandler(this.FormThumbnailView_Load);
            this.grpBoxControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMaximized)).EndInit();
            this.contextMenuFullSizeImg.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox grpBoxControls;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.PictureBox picBoxMaximized;
        private System.Windows.Forms.Button btnScanDirectory;
        private System.Windows.Forms.ContextMenuStrip contextMenuFullSizeImg;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenInDefApp;
        private System.Windows.Forms.ToolStripMenuItem menuItemBookmark;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyPath;
        private System.Windows.Forms.Button btnOptimize;
    }
}
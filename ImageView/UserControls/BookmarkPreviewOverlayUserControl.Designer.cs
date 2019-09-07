namespace ImageViewer.UserControls
{
    partial class BookmarkPreviewOverlayUserControl
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
            this.BookmarkPreviewContentPanel = new System.Windows.Forms.Panel();
            this.OverlayPictureBox = new System.Windows.Forms.PictureBox();
            this.BookmarkPreviewContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OverlayPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BookmarkPreviewContentPanel
            // 
            this.BookmarkPreviewContentPanel.BackColor = System.Drawing.Color.Transparent;
            this.BookmarkPreviewContentPanel.Controls.Add(this.OverlayPictureBox);
            this.BookmarkPreviewContentPanel.ForeColor = System.Drawing.Color.Transparent;
            this.BookmarkPreviewContentPanel.Location = new System.Drawing.Point(3, 3);
            this.BookmarkPreviewContentPanel.Name = "BookmarkPreviewContentPanel";
            this.BookmarkPreviewContentPanel.Padding = new System.Windows.Forms.Padding(2);
            this.BookmarkPreviewContentPanel.Size = new System.Drawing.Size(600, 450);
            this.BookmarkPreviewContentPanel.TabIndex = 0;
            // 
            // OverlayPictureBox
            // 
            this.OverlayPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OverlayPictureBox.Location = new System.Drawing.Point(2, 2);
            this.OverlayPictureBox.Name = "OverlayPictureBox";
            this.OverlayPictureBox.Size = new System.Drawing.Size(596, 446);
            this.OverlayPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OverlayPictureBox.TabIndex = 0;
            this.OverlayPictureBox.TabStop = false;
            // 
            // BookmarkPreviewOverlayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.BookmarkPreviewContentPanel);
            this.Name = "BookmarkPreviewOverlayUserControl";
            this.Size = new System.Drawing.Size(672, 492);
            this.Load += new System.EventHandler(this.BookmarkPreviewOverlayUserControl_Load);
            this.BookmarkPreviewContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OverlayPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BookmarkPreviewContentPanel;
        private System.Windows.Forms.PictureBox OverlayPictureBox;
    }
}

namespace ImageViewer.UserControls
{
    partial class FolderListView
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
            this.customPanel1 = new ImageViewer.UserControls.CustomPanel();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            this.customPanel1.Location = new System.Drawing.Point(151, 101);
            this.customPanel1.Name = "customPanel1";

            this.customPanel1.Size = new System.Drawing.Size(432, 236);
            this.customPanel1.TabIndex = 0;
            // 
            // FolderListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customPanel1);
            this.Name = "FolderListView";
            this.Size = new System.Drawing.Size(711, 444);
            this.Load += new System.EventHandler(this.FolderListView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPanel customPanel1;
    }
}

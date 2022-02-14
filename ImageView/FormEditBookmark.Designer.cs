using ImageViewer.UserControls;

namespace ImageViewer
{
    partial class FormEditBookmark
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditBookmark));
            this.editBookmark1 = new ImageViewer.UserControls.EditBookmark();
            this.SuspendLayout();
            // 
            // editBookmark1
            // 
            this.editBookmark1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editBookmark1.Location = new System.Drawing.Point(5, 5);
            this.editBookmark1.Margin = new System.Windows.Forms.Padding(5);
            this.editBookmark1.Name = "editBookmark1";
            this.editBookmark1.Size = new System.Drawing.Size(344, 141);
            this.editBookmark1.TabIndex = 0;
            // 
            // FormEditBookmark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 151);
            this.Controls.Add(this.editBookmark1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 180);
            this.Name = "FormEditBookmark";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditBookmark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormEditBookmark_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.EditBookmark editBookmark1;
    }
}
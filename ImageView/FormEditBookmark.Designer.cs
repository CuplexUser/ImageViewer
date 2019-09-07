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
            this.renameBookmark1 = new RenameBookmark();
            this.SuspendLayout();
            // 
            // renameBookmark1
            // 
            this.renameBookmark1.Location = new System.Drawing.Point(12, 12);
            this.renameBookmark1.Name = "renameBookmark1";
            this.renameBookmark1.Size = new System.Drawing.Size(425, 165);
            this.renameBookmark1.TabIndex = 0;
            // 
            // FormEditBookmark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 175);
            this.Controls.Add(this.renameBookmark1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditBookmark";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditBookmark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormEditBookmark_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.RenameBookmark renameBookmark1;
    }
}
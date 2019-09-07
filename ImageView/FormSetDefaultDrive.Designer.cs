namespace ImageViewer
{
    partial class FormSetDefaultDrive
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSetDefaultDrive = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDriveList = new System.Windows.Forms.ComboBox();
            this.grpSetDefaultDrive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(141, 73);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(222, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpSetDefaultDrive
            // 
            this.grpSetDefaultDrive.Controls.Add(this.cbDriveList);
            this.grpSetDefaultDrive.Controls.Add(this.label1);
            this.grpSetDefaultDrive.Controls.Add(this.btnCancel);
            this.grpSetDefaultDrive.Controls.Add(this.btnOk);
            this.grpSetDefaultDrive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSetDefaultDrive.Location = new System.Drawing.Point(3, 3);
            this.grpSetDefaultDrive.Name = "grpSetDefaultDrive";
            this.grpSetDefaultDrive.Size = new System.Drawing.Size(308, 107);
            this.grpSetDefaultDrive.TabIndex = 2;
            this.grpSetDefaultDrive.TabStop = false;
            this.grpSetDefaultDrive.Text = "Set default Drive to search in";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Drive:";
            // 
            // cbDriveList
            // 
            this.cbDriveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDriveList.FormattingEnabled = true;
            this.cbDriveList.Location = new System.Drawing.Point(47, 23);
            this.cbDriveList.Name = "cbDriveList";
            this.cbDriveList.Size = new System.Drawing.Size(250, 21);
            this.cbDriveList.TabIndex = 3;
            // 
            // FormSetDefaultDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 113);
            this.Controls.Add(this.grpSetDefaultDrive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetDefaultDrive";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Default Drive";
            this.Load += new System.EventHandler(this.FormSetDefaultDrive_Load);
            this.grpSetDefaultDrive.ResumeLayout(false);
            this.grpSetDefaultDrive.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpSetDefaultDrive;
        private System.Windows.Forms.ComboBox cbDriveList;
        private System.Windows.Forms.Label label1;
    }
}
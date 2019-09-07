using ImageViewer.UserControls;

namespace ImageViewer
{
    partial class FormRestartWithAdminPrivileges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRestartWithAdminPrivileges));
            this.grpBoxUACElevation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIntegrityLevel = new System.Windows.Forms.Label();
            this.lblProcessElevated = new System.Windows.Forms.Label();
            this.lblApplicationRunningAsAdmin = new System.Windows.Forms.Label();
            this.lblCurrenUserInAdminGrp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRestartWithAdminAccess = new System.Windows.Forms.Button();
            this.toolTipStartWithAdminRights = new System.Windows.Forms.ToolTip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlMainImage = new CustomPanel();
            this.grpBoxUACElevation.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxUACElevation
            // 
            this.grpBoxUACElevation.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxUACElevation.Controls.Add(this.pnlMainImage);
            this.grpBoxUACElevation.Controls.Add(this.tableLayoutPanel1);
            this.grpBoxUACElevation.Controls.Add(this.btnCancel);
            this.grpBoxUACElevation.Controls.Add(this.btnRestartWithAdminAccess);
            this.grpBoxUACElevation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxUACElevation.Location = new System.Drawing.Point(5, 5);
            this.grpBoxUACElevation.Name = "grpBoxUACElevation";
            this.grpBoxUACElevation.Size = new System.Drawing.Size(435, 176);
            this.grpBoxUACElevation.TabIndex = 0;
            this.grpBoxUACElevation.TabStop = false;
            this.grpBoxUACElevation.Text = "Restarting the application as Admin will reset access to settings in the registry" +
    "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.lblIntegrityLevel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblProcessElevated, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblApplicationRunningAsAdmin, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrenUserInAdminGrp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(150, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 115);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lblIntegrityLevel
            // 
            this.lblIntegrityLevel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblIntegrityLevel.AutoSize = true;
            this.lblIntegrityLevel.Location = new System.Drawing.Point(220, 93);
            this.lblIntegrityLevel.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblIntegrityLevel.Name = "lblIntegrityLevel";
            this.lblIntegrityLevel.Size = new System.Drawing.Size(31, 13);
            this.lblIntegrityLevel.TabIndex = 7;
            this.lblIntegrityLevel.Text = "........";
            // 
            // lblProcessElevated
            // 
            this.lblProcessElevated.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProcessElevated.AutoSize = true;
            this.lblProcessElevated.Location = new System.Drawing.Point(220, 63);
            this.lblProcessElevated.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblProcessElevated.Name = "lblProcessElevated";
            this.lblProcessElevated.Size = new System.Drawing.Size(31, 13);
            this.lblProcessElevated.TabIndex = 6;
            this.lblProcessElevated.Text = "........";
            // 
            // lblApplicationRunningAsAdmin
            // 
            this.lblApplicationRunningAsAdmin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblApplicationRunningAsAdmin.AutoSize = true;
            this.lblApplicationRunningAsAdmin.Location = new System.Drawing.Point(220, 35);
            this.lblApplicationRunningAsAdmin.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblApplicationRunningAsAdmin.Name = "lblApplicationRunningAsAdmin";
            this.lblApplicationRunningAsAdmin.Size = new System.Drawing.Size(31, 13);
            this.lblApplicationRunningAsAdmin.TabIndex = 5;
            this.lblApplicationRunningAsAdmin.Text = "........";
            // 
            // lblCurrenUserInAdminGrp
            // 
            this.lblCurrenUserInAdminGrp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCurrenUserInAdminGrp.AutoSize = true;
            this.lblCurrenUserInAdminGrp.Location = new System.Drawing.Point(220, 7);
            this.lblCurrenUserInAdminGrp.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.lblCurrenUserInAdminGrp.Name = "lblCurrenUserInAdminGrp";
            this.lblCurrenUserInAdminGrp.Size = new System.Drawing.Size(31, 13);
            this.lblCurrenUserInAdminGrp.TabIndex = 4;
            this.lblCurrenUserInAdminGrp.Text = "........";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current user is administrator:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application running in admin mode:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Process Elevated:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Integrity Level:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(354, 142);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCansel_Click);
            // 
            // btnRestartWithAdminAccess
            // 
            this.btnRestartWithAdminAccess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestartWithAdminAccess.Location = new System.Drawing.Point(203, 142);
            this.btnRestartWithAdminAccess.Name = "btnRestartWithAdminAccess";
            this.btnRestartWithAdminAccess.Size = new System.Drawing.Size(145, 28);
            this.btnRestartWithAdminAccess.TabIndex = 0;
            this.btnRestartWithAdminAccess.Text = "Restart Application";
            this.btnRestartWithAdminAccess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTipStartWithAdminRights.SetToolTip(this.btnRestartWithAdminAccess, "Restarts the application with Administrator privilegious.");
            this.btnRestartWithAdminAccess.UseVisualStyleBackColor = true;
            this.btnRestartWithAdminAccess.Click += new System.EventHandler(this.btnRestartWithAdminAccess_Click);
            // 
            // toolTipStartWithAdminRights
            // 
            this.toolTipStartWithAdminRights.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "WindowsIcon.ico");
            // 
            // pnlMainImage
            // 
            this.pnlMainImage.BackgroundImage = global::ImageViewer.Properties.Resources.WindowsIcon;
            this.pnlMainImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMainImage.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pnlMainImage.InnerBorderColor = System.Drawing.Color.LightSkyBlue;
            this.pnlMainImage.Location = new System.Drawing.Point(4, 19);
            this.pnlMainImage.Name = "pnlMainImage";
            this.pnlMainImage.OuterBorderColor = System.Drawing.Color.Gray;
            this.pnlMainImage.Size = new System.Drawing.Size(128, 128);
            this.pnlMainImage.TabIndex = 3;
            // 
            // FormRestartWithAdminPrivileges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 186);
            this.Controls.Add(this.grpBoxUACElevation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRestartWithAdminPrivileges";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restart Program as Admin";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormRestartWithAdminPrivileges_Load);
            this.grpBoxUACElevation.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxUACElevation;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRestartWithAdminAccess;
        private System.Windows.Forms.ToolTip toolTipStartWithAdminRights;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblIntegrityLevel;
        private System.Windows.Forms.Label lblProcessElevated;
        private System.Windows.Forms.Label lblApplicationRunningAsAdmin;
        private System.Windows.Forms.Label lblCurrenUserInAdminGrp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ImageList imageList1;
        private UserControls.CustomPanel pnlMainImage;
    }
}
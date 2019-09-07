using ImageViewer.Models;

namespace ImageViewer
{
    partial class FormImageDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageDetails));
            this.ImgInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.ImageDetailsDataGrid = new System.Windows.Forms.DataGridView();
            this.imageInformationModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageDimenstionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeFormatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileDirectoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastModifiedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImgInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDetailsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInformationModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ImgInfoGroupBox
            // 
            this.ImgInfoGroupBox.Controls.Add(this.ImageDetailsDataGrid);
            this.ImgInfoGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ImgInfoGroupBox.Name = "ImgInfoGroupBox";
            this.ImgInfoGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.ImgInfoGroupBox.Size = new System.Drawing.Size(533, 455);
            this.ImgInfoGroupBox.TabIndex = 0;
            this.ImgInfoGroupBox.TabStop = false;
            this.ImgInfoGroupBox.Text = "Image information";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(440, 473);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ImageDetailsDataGrid
            // 
            this.ImageDetailsDataGrid.AllowUserToAddRows = false;
            this.ImageDetailsDataGrid.AllowUserToDeleteRows = false;
            this.ImageDetailsDataGrid.AllowUserToResizeRows = false;
            this.ImageDetailsDataGrid.AutoGenerateColumns = false;
            this.ImageDetailsDataGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ImageDetailsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImageDetailsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.imageDimenstionsDataGridViewTextBoxColumn,
            this.sizeFormatedDataGridViewTextBoxColumn,
            this.fileDirectoryDataGridViewTextBoxColumn,
            this.createDateDataGridViewTextBoxColumn,
            this.lastModifiedDateDataGridViewTextBoxColumn});
            this.ImageDetailsDataGrid.DataSource = this.imageInformationModelBindingSource;
            this.ImageDetailsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageDetailsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ImageDetailsDataGrid.Location = new System.Drawing.Point(5, 20);
            this.ImageDetailsDataGrid.Margin = new System.Windows.Forms.Padding(5);
            this.ImageDetailsDataGrid.MultiSelect = false;
            this.ImageDetailsDataGrid.Name = "ImageDetailsDataGrid";
            this.ImageDetailsDataGrid.ReadOnly = true;
            this.ImageDetailsDataGrid.RowTemplate.Height = 24;
            this.ImageDetailsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ImageDetailsDataGrid.ShowEditingIcon = false;
            this.ImageDetailsDataGrid.Size = new System.Drawing.Size(523, 430);
            this.ImageDetailsDataGrid.TabIndex = 9;
            // 
            // imageInformationModelBindingSource
            // 
            this.imageInformationModelBindingSource.DataSource = typeof(ImageInformation);
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // imageDimenstionsDataGridViewTextBoxColumn
            // 
            this.imageDimenstionsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.imageDimenstionsDataGridViewTextBoxColumn.DataPropertyName = "ImageDimensions";
            this.imageDimenstionsDataGridViewTextBoxColumn.HeaderText = "ImageDimensions";
            this.imageDimenstionsDataGridViewTextBoxColumn.Name = "imageDimenstionsDataGridViewTextBoxColumn";
            this.imageDimenstionsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sizeFormatedDataGridViewTextBoxColumn
            // 
            this.sizeFormatedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sizeFormatedDataGridViewTextBoxColumn.DataPropertyName = "SizeFormatted";
            this.sizeFormatedDataGridViewTextBoxColumn.HeaderText = "SizeFormatted";
            this.sizeFormatedDataGridViewTextBoxColumn.Name = "sizeFormatedDataGridViewTextBoxColumn";
            this.sizeFormatedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fileDirectoryDataGridViewTextBoxColumn
            // 
            this.fileDirectoryDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileDirectoryDataGridViewTextBoxColumn.DataPropertyName = "FileDirectory";
            this.fileDirectoryDataGridViewTextBoxColumn.FillWeight = 75F;
            this.fileDirectoryDataGridViewTextBoxColumn.HeaderText = "FileDirectory";
            this.fileDirectoryDataGridViewTextBoxColumn.Name = "fileDirectoryDataGridViewTextBoxColumn";
            this.fileDirectoryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createDateDataGridViewTextBoxColumn
            // 
            this.createDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.createDateDataGridViewTextBoxColumn.DataPropertyName = "CreateDate";
            this.createDateDataGridViewTextBoxColumn.FillWeight = 70F;
            this.createDateDataGridViewTextBoxColumn.HeaderText = "CreateDate";
            this.createDateDataGridViewTextBoxColumn.Name = "createDateDataGridViewTextBoxColumn";
            this.createDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastModifiedDateDataGridViewTextBoxColumn
            // 
            this.lastModifiedDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lastModifiedDateDataGridViewTextBoxColumn.DataPropertyName = "LastModifiedDate";
            this.lastModifiedDateDataGridViewTextBoxColumn.FillWeight = 70F;
            this.lastModifiedDateDataGridViewTextBoxColumn.HeaderText = "LastModifiedDate";
            this.lastModifiedDateDataGridViewTextBoxColumn.Name = "lastModifiedDateDataGridViewTextBoxColumn";
            this.lastModifiedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FormImageDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 523);
            this.Controls.Add(this.ImgInfoGroupBox);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImageDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Details";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormImageDetails_Load);
            this.ImgInfoGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageDetailsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInformationModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ImgInfoGroupBox;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView ImageDetailsDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageDimenstionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeFormatedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileDirectoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastModifiedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource imageInformationModelBindingSource;
    }
}
using ImageViewer.Models;

namespace ImageViewer
{
    partial class FileBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileBrowser));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpImportSection = new System.Windows.Forms.GroupBox();
            this.txtBaseDirectory = new System.Windows.Forms.TextBox();
            this.lblImagesLoaded = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRefreshList = new System.Windows.Forms.Button();
            this.btnOpenImporter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewLoadedImages = new System.Windows.Forms.DataGridView();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeInKbDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastWriteTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastAccessTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileExtentionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyFilepathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithDefaultApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteSelectedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSourceForImageService = new System.Windows.Forms.BindingSource(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.grpImportSection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLoadedImages)).BeginInit();
            this.FileListMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceForImageService)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(425, 22);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(33, 20);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // grpImportSection
            // 
            this.grpImportSection.Controls.Add(this.txtBaseDirectory);
            this.grpImportSection.Controls.Add(this.lblImagesLoaded);
            this.grpImportSection.Controls.Add(this.label2);
            this.grpImportSection.Controls.Add(this.btnRefreshList);
            this.grpImportSection.Controls.Add(this.btnOpenImporter);
            this.grpImportSection.Controls.Add(this.label1);
            this.grpImportSection.Controls.Add(this.btnBrowse);
            this.grpImportSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpImportSection.Location = new System.Drawing.Point(0, 0);
            this.grpImportSection.Name = "grpImportSection";
            this.grpImportSection.Size = new System.Drawing.Size(774, 82);
            this.grpImportSection.TabIndex = 2;
            this.grpImportSection.TabStop = false;
            this.grpImportSection.Text = "Import section - Drag and drop enabled";
            this.grpImportSection.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpImportSection_DragDrop);
            this.grpImportSection.DragEnter += new System.Windows.Forms.DragEventHandler(this.grpImportSection_DragEnter);
            // 
            // txtBaseDirectory
            // 
            this.txtBaseDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtBaseDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtBaseDirectory.Location = new System.Drawing.Point(44, 22);
            this.txtBaseDirectory.Name = "txtBaseDirectory";
            this.txtBaseDirectory.Size = new System.Drawing.Size(375, 20);
            this.txtBaseDirectory.TabIndex = 0;
            this.txtBaseDirectory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBaseDirectory_KeyUp);
            // 
            // lblImagesLoaded
            // 
            this.lblImagesLoaded.AutoSize = true;
            this.lblImagesLoaded.Location = new System.Drawing.Point(91, 51);
            this.lblImagesLoaded.Name = "lblImagesLoaded";
            this.lblImagesLoaded.Size = new System.Drawing.Size(13, 13);
            this.lblImagesLoaded.TabIndex = 7;
            this.lblImagesLoaded.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Images loaded:";
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.Location = new System.Drawing.Point(628, 51);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(134, 25);
            this.btnRefreshList.TabIndex = 3;
            this.btnRefreshList.Text = "Refresh Loaded Images";
            this.btnRefreshList.UseVisualStyleBackColor = true;
            this.btnRefreshList.Click += new System.EventHandler(this.btnRefreshList_Click);
            // 
            // btnOpenImporter
            // 
            this.btnOpenImporter.Location = new System.Drawing.Point(474, 20);
            this.btnOpenImporter.Name = "btnOpenImporter";
            this.btnOpenImporter.Size = new System.Drawing.Size(89, 25);
            this.btnOpenImporter.TabIndex = 2;
            this.btnOpenImporter.Text = "Open Importer";
            this.btnOpenImporter.UseVisualStyleBackColor = true;
            this.btnOpenImporter.Click += new System.EventHandler(this.btnOpenImporter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewLoadedImages);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(774, 580);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Loaded images";
            // 
            // dataGridViewLoadedImages
            // 
            this.dataGridViewLoadedImages.AllowUserToAddRows = false;
            this.dataGridViewLoadedImages.AllowUserToDeleteRows = false;
            this.dataGridViewLoadedImages.AllowUserToResizeRows = false;
            this.dataGridViewLoadedImages.AutoGenerateColumns = false;
            this.dataGridViewLoadedImages.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewLoadedImages.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewLoadedImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewLoadedImages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.directoryDataGridViewTextBoxColumn,
            this.sizeInKbDataGridViewTextBoxColumn,
            this.creationTimeDataGridViewTextBoxColumn,
            this.lastWriteTimeDataGridViewTextBoxColumn,
            this.lastAccessTimeDataGridViewTextBoxColumn,
            this.fileExtentionDataGridViewTextBoxColumn});
            this.dataGridViewLoadedImages.ContextMenuStrip = this.FileListMenuStrip;
            this.dataGridViewLoadedImages.DataSource = this.bindingSourceForImageService;
            this.dataGridViewLoadedImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLoadedImages.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewLoadedImages.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewLoadedImages.Name = "dataGridViewLoadedImages";
            this.dataGridViewLoadedImages.ReadOnly = true;
            this.dataGridViewLoadedImages.RowHeadersVisible = false;
            this.dataGridViewLoadedImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLoadedImages.Size = new System.Drawing.Size(768, 561);
            this.dataGridViewLoadedImages.TabIndex = 0;
            this.dataGridViewLoadedImages.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewLoadedImages_DataBindingComplete);
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.MinimumWidth = 75;
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // directoryDataGridViewTextBoxColumn
            // 
            this.directoryDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.directoryDataGridViewTextBoxColumn.DataPropertyName = "Directory";
            this.directoryDataGridViewTextBoxColumn.HeaderText = "Directory";
            this.directoryDataGridViewTextBoxColumn.MinimumWidth = 125;
            this.directoryDataGridViewTextBoxColumn.Name = "directoryDataGridViewTextBoxColumn";
            this.directoryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sizeInKbDataGridViewTextBoxColumn
            // 
            this.sizeInKbDataGridViewTextBoxColumn.DataPropertyName = "SizeInKb";
            this.sizeInKbDataGridViewTextBoxColumn.HeaderText = "SizeInKb";
            this.sizeInKbDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.sizeInKbDataGridViewTextBoxColumn.Name = "sizeInKbDataGridViewTextBoxColumn";
            this.sizeInKbDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creationTimeDataGridViewTextBoxColumn
            // 
            this.creationTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.creationTimeDataGridViewTextBoxColumn.DataPropertyName = "CreationTime";
            this.creationTimeDataGridViewTextBoxColumn.HeaderText = "CreationTime";
            this.creationTimeDataGridViewTextBoxColumn.Name = "creationTimeDataGridViewTextBoxColumn";
            this.creationTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.creationTimeDataGridViewTextBoxColumn.Width = 94;
            // 
            // lastWriteTimeDataGridViewTextBoxColumn
            // 
            this.lastWriteTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.lastWriteTimeDataGridViewTextBoxColumn.DataPropertyName = "LastWriteTime";
            this.lastWriteTimeDataGridViewTextBoxColumn.HeaderText = "LastWriteTime";
            this.lastWriteTimeDataGridViewTextBoxColumn.Name = "lastWriteTimeDataGridViewTextBoxColumn";
            this.lastWriteTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastAccessTimeDataGridViewTextBoxColumn
            // 
            this.lastAccessTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.lastAccessTimeDataGridViewTextBoxColumn.DataPropertyName = "LastAccessTime";
            this.lastAccessTimeDataGridViewTextBoxColumn.HeaderText = "LastAccessTime";
            this.lastAccessTimeDataGridViewTextBoxColumn.Name = "lastAccessTimeDataGridViewTextBoxColumn";
            this.lastAccessTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastAccessTimeDataGridViewTextBoxColumn.Width = 110;
            // 
            // fileExtentionDataGridViewTextBoxColumn
            // 
            this.fileExtentionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.fileExtentionDataGridViewTextBoxColumn.DataPropertyName = "FileExtention";
            this.fileExtentionDataGridViewTextBoxColumn.HeaderText = "FileExtention";
            this.fileExtentionDataGridViewTextBoxColumn.Name = "fileExtentionDataGridViewTextBoxColumn";
            this.fileExtentionDataGridViewTextBoxColumn.ReadOnly = true;
            this.fileExtentionDataGridViewTextBoxColumn.Width = 92;
            // 
            // FileListMenuStrip
            // 
            this.FileListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyFilepathToolStripMenuItem,
            this.openWithDefaultApplicationToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteSelectedFilesToolStripMenuItem});
            this.FileListMenuStrip.Name = "contextMenuStrip1";
            this.FileListMenuStrip.Size = new System.Drawing.Size(232, 76);
            this.FileListMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.FileListMenuStrip_Opening);
            // 
            // copyFilepathToolStripMenuItem
            // 
            this.copyFilepathToolStripMenuItem.Name = "copyFilepathToolStripMenuItem";
            this.copyFilepathToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.copyFilepathToolStripMenuItem.Text = "Copy filepath";
            this.copyFilepathToolStripMenuItem.Click += new System.EventHandler(this.copyFilepathToolStripMenuItem_Click);
            // 
            // openWithDefaultApplicationToolStripMenuItem
            // 
            this.openWithDefaultApplicationToolStripMenuItem.Name = "openWithDefaultApplicationToolStripMenuItem";
            this.openWithDefaultApplicationToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openWithDefaultApplicationToolStripMenuItem.Text = "Open with default application";
            this.openWithDefaultApplicationToolStripMenuItem.Click += new System.EventHandler(this.openWithDefaultApplicationToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
            // 
            // deleteSelectedFilesToolStripMenuItem
            // 
            this.deleteSelectedFilesToolStripMenuItem.Name = "deleteSelectedFilesToolStripMenuItem";
            this.deleteSelectedFilesToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.deleteSelectedFilesToolStripMenuItem.Text = "Delete selected files";
            this.deleteSelectedFilesToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedFilesToolStripMenuItem_Click);
            // 
            // bindingSourceForImageService
            // 
            this.bindingSourceForImageService.DataSource = typeof(ImageReferenceElement);
            // 
            // FileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 662);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpImportSection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FileBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileBrowser_FormClosing);
            this.Load += new System.EventHandler(this.FileBrowser_Load);
            this.grpImportSection.ResumeLayout(false);
            this.grpImportSection.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLoadedImages)).EndInit();
            this.FileListMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceForImageService)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox grpImportSection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpenImporter;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridView dataGridViewLoadedImages;
        private System.Windows.Forms.Button btnRefreshList;
        private System.Windows.Forms.ContextMenuStrip FileListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyFilepathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithDefaultApplicationToolStripMenuItem;
        private System.Windows.Forms.Label lblImagesLoaded;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedFilesToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSourceForImageService;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn directoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeInKbDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastWriteTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastAccessTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileExtentionDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txtBaseDirectory;
    }
}
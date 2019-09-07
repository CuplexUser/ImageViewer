using ImageViewer.DataContracts;

namespace ImageViewer
{
    partial class FormBookmarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBookmarks));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FolderImages = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.bookmarksTree = new System.Windows.Forms.TreeView();
            this.contextMenuStripFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.renameFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.bookmarksDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeFormatedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookmarkBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mainWinMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAndIncludeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tryToFixBrokenLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOverlayPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.maximizePreviewAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restorePreviewAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStripBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarkFolderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStripFolders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarksDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource)).BeginInit();
            this.mainWinMenu.SuspendLayout();
            this.contextMenuStripBookmarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FolderImages
            // 
            this.FolderImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FolderImages.ImageStream")));
            this.FolderImages.TransparentColor = System.Drawing.Color.Transparent;
            this.FolderImages.Images.SetKeyName(0, "folder.ico");
            this.FolderImages.Images.SetKeyName(1, "opened_folder.ico");
            this.FolderImages.Images.SetKeyName(2, "Folder_Icon.ico");
            this.FolderImages.Images.SetKeyName(3, "Folder_yellow_Icon.ico");
            this.FolderImages.Images.SetKeyName(4, "Folder_IconOpen.ico");
            this.FolderImages.Images.SetKeyName(5, "Folder_my_pictures_Icon.ico");
            this.FolderImages.Images.SetKeyName(6, "Folder-Opened-icon.ico");
            this.FolderImages.Images.SetKeyName(7, "Folder-Closed-icon.ico");
            this.FolderImages.Images.SetKeyName(8, "Documents-icon.ico");
            this.FolderImages.Images.SetKeyName(9, "Folder-icon.ico");
            this.FolderImages.Images.SetKeyName(10, "normal_folder.ico");
            this.FolderImages.Images.SetKeyName(11, "Open_Folder_Icon.ico");
            this.FolderImages.Images.SetKeyName(12, "Closed_Folder_Icon.ico");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bookmarksDataGridView);
            this.splitContainer1.Panel2.Controls.Add(this.mainWinMenu);
            this.splitContainer1.Size = new System.Drawing.Size(832, 603);
            this.splitContainer1.SplitterDistance = 233;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.bookmarksTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxPreview);
            this.splitContainer2.Size = new System.Drawing.Size(233, 603);
            this.splitContainer2.SplitterDistance = 293;
            this.splitContainer2.TabIndex = 0;
            // 
            // bookmarksTree
            // 
            this.bookmarksTree.AllowDrop = true;
            this.bookmarksTree.ContextMenuStrip = this.contextMenuStripFolders;
            this.bookmarksTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookmarksTree.ImageIndex = 0;
            this.bookmarksTree.ImageList = this.FolderImages;
            this.bookmarksTree.Location = new System.Drawing.Point(0, 0);
            this.bookmarksTree.Name = "bookmarksTree";
            this.bookmarksTree.SelectedImageIndex = 1;
            this.bookmarksTree.ShowPlusMinus = false;
            this.bookmarksTree.ShowRootLines = false;
            this.bookmarksTree.Size = new System.Drawing.Size(233, 293);
            this.bookmarksTree.TabIndex = 0;
            this.bookmarksTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.bookmarksTree_DragDrop);
            this.bookmarksTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.bookmarksTree_DragEnter);
            this.bookmarksTree.DragOver += new System.Windows.Forms.DragEventHandler(this.bookmarksTree_DragOver);
            // 
            // contextMenuStripFolders
            // 
            this.contextMenuStripFolders.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToolStripMenuItem,
            this.deleteFolderToolStripMenuItem,
            this.toolStripSeparator2,
            this.renameFolderMenuItem});
            this.contextMenuStripFolders.Name = "contextMenuStripFolders";
            this.contextMenuStripFolders.Size = new System.Drawing.Size(154, 76);
            this.contextMenuStripFolders.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripFolders_Opening);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // renameFolderMenuItem
            // 
            this.renameFolderMenuItem.Name = "renameFolderMenuItem";
            this.renameFolderMenuItem.Size = new System.Drawing.Size(153, 22);
            this.renameFolderMenuItem.Text = "Rename Folder";
            this.renameFolderMenuItem.Click += new System.EventHandler(this.renameFolderMenuItem_Click);
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.Color.White;
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(233, 306);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 1;
            this.pictureBoxPreview.TabStop = false;
            // 
            // bookmarksDataGridView
            // 
            this.bookmarksDataGridView.AllowUserToOrderColumns = true;
            this.bookmarksDataGridView.AllowUserToResizeRows = false;
            this.bookmarksDataGridView.AutoGenerateColumns = false;
            this.bookmarksDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bookmarksDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.bookmarksDataGridView.ColumnHeadersHeight = 25;
            this.bookmarksDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.sizeFormatedDataGridViewTextBoxColumn});
            this.bookmarksDataGridView.DataSource = this.bookmarkBindingSource;
            this.bookmarksDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookmarksDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.bookmarksDataGridView.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.bookmarksDataGridView.Location = new System.Drawing.Point(0, 24);
            this.bookmarksDataGridView.MultiSelect = false;
            this.bookmarksDataGridView.Name = "bookmarksDataGridView";
            this.bookmarksDataGridView.RowHeadersVisible = false;
            this.bookmarksDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(227)))), ((int)(((byte)(252)))));
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.bookmarksDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bookmarksDataGridView.ShowCellErrors = false;
            this.bookmarksDataGridView.ShowCellToolTips = false;
            this.bookmarksDataGridView.ShowEditingIcon = false;
            this.bookmarksDataGridView.ShowRowErrors = false;
            this.bookmarksDataGridView.Size = new System.Drawing.Size(595, 579);
            this.bookmarksDataGridView.StandardTab = true;
            this.bookmarksDataGridView.TabIndex = 0;
            this.bookmarksDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.bookmarksDataGridView_CellMouseDoubleClick);
            this.bookmarksDataGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.bookmarksDataGridView_CellMouseEnter);
            this.bookmarksDataGridView.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.bookmarksDataGridView_CellMouseLeave);
            this.bookmarksDataGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.bookmarksDataGridView_CellMouseMove);
            this.bookmarksDataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.bookmarksDataGridView_ColumnHeaderMouseClick);
            this.bookmarksDataGridView.SelectionChanged += new System.EventHandler(this.bookmarksDataGridView_SelectionChanged);
            this.bookmarksDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bookmarksDataGridView_KeyDown);
            this.bookmarksDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bookmarksDataGridView_MouseDown);
            this.bookmarksDataGridView.MouseEnter += new System.EventHandler(this.bookmarksDataGridView_MouseEnter);
            this.bookmarksDataGridView.MouseLeave += new System.EventHandler(this.bookmarksDataGridView_MouseLeave);
            this.bookmarksDataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bookmarksDataGridView_MouseMove);
            this.bookmarksDataGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bookmarksDataGridView_MouseUp);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "BoookmarkName";
            this.dataGridViewTextBoxColumn1.HeaderText = "BoookmarkName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "FileName";
            this.dataGridViewTextBoxColumn2.HeaderText = "FileName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CompletePath";
            this.dataGridViewTextBoxColumn3.FillWeight = 75F;
            this.dataGridViewTextBoxColumn3.HeaderText = "CompletePath";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // sizeFormatedDataGridViewTextBoxColumn
            // 
            this.sizeFormatedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sizeFormatedDataGridViewTextBoxColumn.DataPropertyName = "SizeFormatted";
            this.sizeFormatedDataGridViewTextBoxColumn.FillWeight = 50F;
            this.sizeFormatedDataGridViewTextBoxColumn.HeaderText = "File Size";
            this.sizeFormatedDataGridViewTextBoxColumn.MinimumWidth = 20;
            this.sizeFormatedDataGridViewTextBoxColumn.Name = "sizeFormatedDataGridViewTextBoxColumn";
            this.sizeFormatedDataGridViewTextBoxColumn.ReadOnly = true;
            this.sizeFormatedDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // bookmarkBindingSource
            // 
            this.bookmarkBindingSource.DataSource = typeof(Bookmark);
            // 
            // mainWinMenu
            // 
            this.mainWinMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainWinMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.mainWinMenu.Location = new System.Drawing.Point(0, 0);
            this.mainWinMenu.Name = "mainWinMenu";
            this.mainWinMenu.Size = new System.Drawing.Size(595, 24);
            this.mainWinMenu.TabIndex = 1;
            this.mainWinMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openAndIncludeToolStripMenuItem,
            this.openAndReplaceToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // openAndIncludeToolStripMenuItem
            // 
            this.openAndIncludeToolStripMenuItem.Name = "openAndIncludeToolStripMenuItem";
            this.openAndIncludeToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.openAndIncludeToolStripMenuItem.Text = "Append To Current Bookmarks";
            this.openAndIncludeToolStripMenuItem.Click += new System.EventHandler(this.openAndIncludeToolStripMenuItem_Click);
            // 
            // openAndReplaceToolStripMenuItem
            // 
            this.openAndReplaceToolStripMenuItem.Name = "openAndReplaceToolStripMenuItem";
            this.openAndReplaceToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.openAndReplaceToolStripMenuItem.Text = "Replace Current Bookmarks";
            this.openAndReplaceToolStripMenuItem.Click += new System.EventHandler(this.openAndReplaceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(111, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tryToFixBrokenLinksToolStripMenuItem,
            this.removeDuplicatesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // tryToFixBrokenLinksToolStripMenuItem
            // 
            this.tryToFixBrokenLinksToolStripMenuItem.Name = "tryToFixBrokenLinksToolStripMenuItem";
            this.tryToFixBrokenLinksToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.tryToFixBrokenLinksToolStripMenuItem.Text = "Fix Broken Links";
            this.tryToFixBrokenLinksToolStripMenuItem.Click += new System.EventHandler(this.tryToFixBrokenLinksToolStripMenuItem_Click);
            // 
            // removeDuplicatesToolStripMenuItem
            // 
            this.removeDuplicatesToolStripMenuItem.Name = "removeDuplicatesToolStripMenuItem";
            this.removeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.removeDuplicatesToolStripMenuItem.Text = "Remove Duplicates";
            this.removeDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.removeDuplicatesToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOverlayPreviewToolStripMenuItem,
            this.toolStripSeparator4,
            this.maximizePreviewAreaToolStripMenuItem,
            this.restorePreviewAreaToolStripMenuItem,
            this.toolStripSeparator3});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showOverlayPreviewToolStripMenuItem
            // 
            this.showOverlayPreviewToolStripMenuItem.Name = "showOverlayPreviewToolStripMenuItem";
            this.showOverlayPreviewToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.showOverlayPreviewToolStripMenuItem.Text = "Show Overlay Preview";
            this.showOverlayPreviewToolStripMenuItem.Click += new System.EventHandler(this.showOverlayPreviewToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // maximizePreviewAreaToolStripMenuItem
            // 
            this.maximizePreviewAreaToolStripMenuItem.Name = "maximizePreviewAreaToolStripMenuItem";
            this.maximizePreviewAreaToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.maximizePreviewAreaToolStripMenuItem.Text = "Maximize Preview Area";
            this.maximizePreviewAreaToolStripMenuItem.Click += new System.EventHandler(this.maximizePreviewAreaToolStripMenuItem_Click);
            // 
            // restorePreviewAreaToolStripMenuItem
            // 
            this.restorePreviewAreaToolStripMenuItem.Name = "restorePreviewAreaToolStripMenuItem";
            this.restorePreviewAreaToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.restorePreviewAreaToolStripMenuItem.Text = "Restore Preview Area";
            this.restorePreviewAreaToolStripMenuItem.Click += new System.EventHandler(this.restorePreviewAreaToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
            // 
            // contextMenuStripBookmarks
            // 
            this.contextMenuStripBookmarks.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripBookmarks.Name = "contextMenuStripBookmarks";
            this.contextMenuStripBookmarks.Size = new System.Drawing.Size(118, 70);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // FormBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 603);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainWinMenu;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormBookmarks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bookmarks";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBookmarks_FormClosing);
            this.Load += new System.EventHandler(this.FormBookmarks_Load);
            this.Shown += new System.EventHandler(this.FormBookmarks_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStripFolders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarksDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource)).EndInit();
            this.mainWinMenu.ResumeLayout(false);
            this.mainWinMenu.PerformLayout();
            this.contextMenuStripBookmarks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ImageList FolderImages;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView bookmarksTree;
        private System.Windows.Forms.DataGridView bookmarksDataGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFolders;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBookmarks;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip mainWinMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tryToFixBrokenLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem renameFolderMenuItem;
        private System.Windows.Forms.BindingSource bookmarkFolderBindingSource;
        private System.Windows.Forms.BindingSource bookmarkBindingSource;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAndIncludeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAndReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showOverlayPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem maximizePreviewAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem restorePreviewAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeFormatedDataGridViewTextBoxColumn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
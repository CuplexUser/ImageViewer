namespace ImageViewer
{
    partial class FormAddImageSource
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node13");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node10", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node11");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node12");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3,
            treeNode4,
            treeNode5});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddImageSource));
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Folder1");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Sub 1");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Folder2", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Subfolder 1");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Subfolder 2");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Subfolder 3");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Folder 3", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12});
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeViewFileSystem = new System.Windows.Forms.TreeView();
            this.contextMenuAddSource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderRecursiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.updateFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgListIcons = new System.Windows.Forms.ImageList(this.components);
            this.cbDrives = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpTargetCollection = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeViewImgCollection = new System.Windows.Forms.TreeView();
            this.contextMenuOutputImageList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAddFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddFlderRecursive = new System.Windows.Forms.ToolStripMenuItem();
            this.UnpdateResultInTargetDirMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemRemoveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.imgListViewIcons = new System.Windows.Forms.ImageList(this.components);
            this.lblImageCount = new System.Windows.Forms.Label();
            this.lstBoxOutputFiles = new System.Windows.Forms.ListBox();
            this.groupBoxSummary = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRootFolders = new System.Windows.Forms.Label();
            this.lblImages = new System.Windows.Forms.Label();
            this.lblFolders = new System.Windows.Forms.Label();
            this.lblDriveCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCombinedSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAsyncStateInfo = new System.Windows.Forms.Label();
            this.lblWorkingFileName = new System.Windows.Forms.Label();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newCollectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCollectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentCollectionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplicatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeMissingFilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoloadPreviousFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.updateFileListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuAddSource.SuspendLayout();
            this.grpTargetCollection.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuOutputImageList.SuspendLayout();
            this.groupBoxSummary.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.cbDrives);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 587);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source selection";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.treeViewFileSystem);
            this.panel2.Location = new System.Drawing.Point(3, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 537);
            this.panel2.TabIndex = 7;
            // 
            // treeViewFileSystem
            // 
            this.treeViewFileSystem.AllowDrop = true;
            this.treeViewFileSystem.ContextMenuStrip = this.contextMenuAddSource;
            this.treeViewFileSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFileSystem.ImageKey = "Folder";
            this.treeViewFileSystem.ImageList = this.imgListIcons;
            this.treeViewFileSystem.Location = new System.Drawing.Point(0, 0);
            this.treeViewFileSystem.Name = "treeViewFileSystem";
            treeNode1.ImageKey = "Folder";
            treeNode1.Name = "Node1";
            treeNode1.SelectedImageKey = "FolderSelected";
            treeNode1.Text = "Node1";
            treeNode2.ImageKey = "Folder";
            treeNode2.Name = "Node13";
            treeNode2.SelectedImageKey = "FolderSelected";
            treeNode2.Text = "Node13";
            treeNode3.ImageIndex = 1;
            treeNode3.Name = "Node10";
            treeNode3.SelectedImageKey = "FolderSelected";
            treeNode3.Text = "Node10";
            treeNode4.ImageKey = "Folder";
            treeNode4.Name = "Node11";
            treeNode4.SelectedImageKey = "FolderSelected";
            treeNode4.Text = "Node11";
            treeNode5.ImageKey = "Folder";
            treeNode5.Name = "Node12";
            treeNode5.SelectedImageKey = "FolderSelected";
            treeNode5.Text = "Node12";
            treeNode6.ImageKey = "Drive";
            treeNode6.Name = "Node0";
            treeNode6.SelectedImageKey = "Drive";
            treeNode6.Text = "Node0";
            this.treeViewFileSystem.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.treeViewFileSystem.SelectedImageIndex = 2;
            this.treeViewFileSystem.Size = new System.Drawing.Size(227, 537);
            this.treeViewFileSystem.TabIndex = 6;
            this.treeViewFileSystem.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewFileSystem_ItemDrag);
            this.treeViewFileSystem.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewFileSystem_MouseClick);
            this.treeViewFileSystem.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewFileSystem_MouseDoubleClick);
            // 
            // contextMenuAddSource
            // 
            this.contextMenuAddSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderRecursiveMenuItem,
            this.addFolderMenuItem,
            this.toolStripMenuItem7,
            this.updateFolderMenuItem});
            this.contextMenuAddSource.Name = "contextMenuAddSource";
            this.contextMenuAddSource.Size = new System.Drawing.Size(186, 76);
            // 
            // addFolderRecursiveMenuItem
            // 
            this.addFolderRecursiveMenuItem.Name = "addFolderRecursiveMenuItem";
            this.addFolderRecursiveMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addFolderRecursiveMenuItem.Text = "Add Folder Recursive";
            this.addFolderRecursiveMenuItem.Click += new System.EventHandler(this.addFolderRecursiveMenuItem_Click);
            // 
            // addFolderMenuItem
            // 
            this.addFolderMenuItem.Name = "addFolderMenuItem";
            this.addFolderMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addFolderMenuItem.Text = "Add Folder";
            this.addFolderMenuItem.Click += new System.EventHandler(this.addFolderMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(182, 6);
            // 
            // updateFolderMenuItem
            // 
            this.updateFolderMenuItem.Name = "updateFolderMenuItem";
            this.updateFolderMenuItem.Size = new System.Drawing.Size(185, 22);
            this.updateFolderMenuItem.Text = "Update";
            this.updateFolderMenuItem.Click += new System.EventHandler(this.updateFolderMenuItem_Click);
            // 
            // imgListIcons
            // 
            this.imgListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListIcons.ImageStream")));
            this.imgListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListIcons.Images.SetKeyName(0, "Drive");
            this.imgListIcons.Images.SetKeyName(1, "Folder");
            this.imgListIcons.Images.SetKeyName(2, "FolderSelected");
            this.imgListIcons.Images.SetKeyName(3, "FolderDocs");
            this.imgListIcons.Images.SetKeyName(4, "Folder2");
            this.imgListIcons.Images.SetKeyName(5, "Folder2Open");
            // 
            // cbDrives
            // 
            this.cbDrives.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDrives.FormattingEnabled = true;
            this.cbDrives.Location = new System.Drawing.Point(3, 18);
            this.cbDrives.Name = "cbDrives";
            this.cbDrives.Size = new System.Drawing.Size(227, 23);
            this.cbDrives.TabIndex = 5;
            this.cbDrives.SelectedIndexChanged += new System.EventHandler(this.cbDrives_SelectedIndexChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Enabled = false;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLoad.Location = new System.Drawing.Point(582, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(95, 28);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(683, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpTargetCollection
            // 
            this.grpTargetCollection.Controls.Add(this.panel3);
            this.grpTargetCollection.Controls.Add(this.groupBoxSummary);
            this.grpTargetCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTargetCollection.Location = new System.Drawing.Point(5, 2);
            this.grpTargetCollection.Name = "grpTargetCollection";
            this.grpTargetCollection.Padding = new System.Windows.Forms.Padding(5);
            this.grpTargetCollection.Size = new System.Drawing.Size(525, 587);
            this.grpTargetCollection.TabIndex = 5;
            this.grpTargetCollection.TabStop = false;
            this.grpTargetCollection.Text = "Target collection";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Location = new System.Drawing.Point(8, 18);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(509, 493);
            this.panel3.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeViewImgCollection);
            this.splitContainer2.Panel1MinSize = 150;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblImageCount);
            this.splitContainer2.Panel2.Controls.Add(this.lstBoxOutputFiles);
            this.splitContainer2.Panel2MinSize = 125;
            this.splitContainer2.Size = new System.Drawing.Size(509, 493);
            this.splitContainer2.SplitterDistance = 319;
            this.splitContainer2.TabIndex = 2;
            // 
            // treeViewImgCollection
            // 
            this.treeViewImgCollection.AllowDrop = true;
            this.treeViewImgCollection.ContextMenuStrip = this.contextMenuOutputImageList;
            this.treeViewImgCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewImgCollection.ImageIndex = 0;
            this.treeViewImgCollection.ImageList = this.imgListViewIcons;
            this.treeViewImgCollection.Location = new System.Drawing.Point(0, 0);
            this.treeViewImgCollection.Name = "treeViewImgCollection";
            treeNode7.Name = "Node0";
            treeNode7.SelectedImageKey = "Folder2";
            treeNode7.Text = "Folder1";
            treeNode8.ImageKey = "Folder";
            treeNode8.Name = "Node6";
            treeNode8.SelectedImageKey = "Folder2";
            treeNode8.Text = "Sub 1";
            treeNode9.Name = "Node1";
            treeNode9.SelectedImageKey = "Folder2";
            treeNode9.Text = "Folder2";
            treeNode10.ImageKey = "Image";
            treeNode10.Name = "Node3";
            treeNode10.SelectedImageKey = "Folder2";
            treeNode10.Text = "Subfolder 1";
            treeNode11.ImageKey = "Image";
            treeNode11.Name = "Node4";
            treeNode11.SelectedImageKey = "Folder2";
            treeNode11.Text = "Subfolder 2";
            treeNode12.ImageIndex = 3;
            treeNode12.Name = "Node5";
            treeNode12.SelectedImageKey = "Folder2";
            treeNode12.Text = "Subfolder 3";
            treeNode13.Name = "Node2";
            treeNode13.SelectedImageKey = "Folder2";
            treeNode13.Text = "Folder 3";
            this.treeViewImgCollection.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode9,
            treeNode13});
            this.treeViewImgCollection.SelectedImageIndex = 1;
            this.treeViewImgCollection.Size = new System.Drawing.Size(319, 493);
            this.treeViewImgCollection.TabIndex = 0;
            this.treeViewImgCollection.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewImgCollection_BeforeExpand);
            this.treeViewImgCollection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImgCollection_AfterSelect);
            this.treeViewImgCollection.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewImgCollection_DragDrop);
            this.treeViewImgCollection.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewImgCollection_DragEnter);
            this.treeViewImgCollection.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewImgCollection_KeyUp);
            // 
            // contextMenuOutputImageList
            // 
            this.contextMenuOutputImageList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddFiles,
            this.toolStripMenuItemAddFolder,
            this.toolStripMenuItemAddFlderRecursive,
            this.toolStripMenuItem10,
            this.UnpdateResultInTargetDirMenuItem,
            this.toolStripMenuItem6,
            this.toolStripMenuItemRemoveItem,
            this.toolStripMenuItem12,
            this.clearAllToolStripMenuItem,
            this.toolStripMenuItem9});
            this.contextMenuOutputImageList.Name = "contextMenuOutputImageList";
            this.contextMenuOutputImageList.Size = new System.Drawing.Size(236, 160);
            // 
            // toolStripMenuItemAddFiles
            // 
            this.toolStripMenuItemAddFiles.Name = "toolStripMenuItemAddFiles";
            this.toolStripMenuItemAddFiles.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemAddFiles.Text = "Add Files";
            this.toolStripMenuItemAddFiles.Click += new System.EventHandler(this.toolStripMenuItemAddFiles_Click);
            // 
            // toolStripMenuItemAddFolder
            // 
            this.toolStripMenuItemAddFolder.Name = "toolStripMenuItemAddFolder";
            this.toolStripMenuItemAddFolder.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemAddFolder.Text = "Add Folder";
            this.toolStripMenuItemAddFolder.Click += new System.EventHandler(this.toolStripMenuItemAddFolder_Click);
            // 
            // toolStripMenuItemAddFlderRecursive
            // 
            this.toolStripMenuItemAddFlderRecursive.Name = "toolStripMenuItemAddFlderRecursive";
            this.toolStripMenuItemAddFlderRecursive.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemAddFlderRecursive.Text = "Add Folder Recursive";
            this.toolStripMenuItemAddFlderRecursive.Click += new System.EventHandler(this.toolStripMenuItemAddFlderRecursive_Click);
            // 
            // UnpdateResultInTargetDirMenuItem
            // 
            this.UnpdateResultInTargetDirMenuItem.Name = "UnpdateResultInTargetDirMenuItem";
            this.UnpdateResultInTargetDirMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.UnpdateResultInTargetDirMenuItem.Size = new System.Drawing.Size(235, 22);
            this.UnpdateResultInTargetDirMenuItem.Text = "Update Folder Content";
            this.UnpdateResultInTargetDirMenuItem.Click += new System.EventHandler(this.UnpdateResultInTargetDirMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(232, 6);
            // 
            // toolStripMenuItemRemoveItem
            // 
            this.toolStripMenuItemRemoveItem.Name = "toolStripMenuItemRemoveItem";
            this.toolStripMenuItemRemoveItem.Size = new System.Drawing.Size(235, 22);
            this.toolStripMenuItemRemoveItem.Text = "Remove Selected Item (Del)";
            this.toolStripMenuItemRemoveItem.Click += new System.EventHandler(this.toolStripMenuItemRemoveItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(232, 6);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(232, 6);
            // 
            // imgListViewIcons
            // 
            this.imgListViewIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListViewIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListViewIcons.ImageStream")));
            this.imgListViewIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListViewIcons.Images.SetKeyName(0, "Folder");
            this.imgListViewIcons.Images.SetKeyName(1, "Folder2");
            this.imgListViewIcons.Images.SetKeyName(2, "Image2");
            this.imgListViewIcons.Images.SetKeyName(3, "Image");
            // 
            // lblImageCount
            // 
            this.lblImageCount.AutoSize = true;
            this.lblImageCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblImageCount.Location = new System.Drawing.Point(6, 8);
            this.lblImageCount.Name = "lblImageCount";
            this.lblImageCount.Size = new System.Drawing.Size(85, 17);
            this.lblImageCount.TabIndex = 2;
            this.lblImageCount.Text = "Images: 1000";
            // 
            // lstBoxOutputFiles
            // 
            this.lstBoxOutputFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBoxOutputFiles.FormattingEnabled = true;
            this.lstBoxOutputFiles.ItemHeight = 15;
            this.lstBoxOutputFiles.Location = new System.Drawing.Point(3, 33);
            this.lstBoxOutputFiles.Name = "lstBoxOutputFiles";
            this.lstBoxOutputFiles.Size = new System.Drawing.Size(180, 454);
            this.lstBoxOutputFiles.TabIndex = 1;
            this.lstBoxOutputFiles.DoubleClick += new System.EventHandler(this.lstBoxOutputFiles_DoubleClick);
            // 
            // groupBoxSummary
            // 
            this.groupBoxSummary.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxSummary.Location = new System.Drawing.Point(5, 512);
            this.groupBoxSummary.Name = "groupBoxSummary";
            this.groupBoxSummary.Size = new System.Drawing.Size(515, 70);
            this.groupBoxSummary.TabIndex = 6;
            this.groupBoxSummary.TabStop = false;
            this.groupBoxSummary.Text = "Summary";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lblRootFolders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblImages, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblFolders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDriveCount, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCombinedSize, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.92308F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.07692F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(509, 48);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lblRootFolders
            // 
            this.lblRootFolders.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRootFolders.AutoSize = true;
            this.lblRootFolders.Location = new System.Drawing.Point(104, 28);
            this.lblRootFolders.Name = "lblRootFolders";
            this.lblRootFolders.Size = new System.Drawing.Size(13, 15);
            this.lblRootFolders.TabIndex = 11;
            this.lblRootFolders.Text = "0";
            // 
            // lblImages
            // 
            this.lblImages.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblImages.AutoSize = true;
            this.lblImages.Location = new System.Drawing.Point(3, 28);
            this.lblImages.Name = "lblImages";
            this.lblImages.Size = new System.Drawing.Size(13, 15);
            this.lblImages.TabIndex = 10;
            this.lblImages.Text = "0";
            // 
            // lblFolders
            // 
            this.lblFolders.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFolders.AutoSize = true;
            this.lblFolders.Location = new System.Drawing.Point(205, 28);
            this.lblFolders.Name = "lblFolders";
            this.lblFolders.Size = new System.Drawing.Size(13, 15);
            this.lblFolders.TabIndex = 9;
            this.lblFolders.Text = "0";
            // 
            // lblDriveCount
            // 
            this.lblDriveCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDriveCount.AutoSize = true;
            this.lblDriveCount.Location = new System.Drawing.Point(407, 28);
            this.lblDriveCount.Name = "lblDriveCount";
            this.lblDriveCount.Size = new System.Drawing.Size(13, 15);
            this.lblDriveCount.TabIndex = 8;
            this.lblDriveCount.Text = "0";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Images";
            // 
            // lblCombinedSize
            // 
            this.lblCombinedSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCombinedSize.AutoSize = true;
            this.lblCombinedSize.Location = new System.Drawing.Point(306, 28);
            this.lblCombinedSize.Name = "lblCombinedSize";
            this.lblCombinedSize.Size = new System.Drawing.Size(13, 15);
            this.lblCombinedSize.TabIndex = 1;
            this.lblCombinedSize.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Root folders";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Folders";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(306, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Combined Size";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Drive Count";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 250;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(5, 2, 5, 5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpTargetCollection);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5, 2, 5, 5);
            this.splitContainer1.Size = new System.Drawing.Size(782, 594);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Source collection:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAsyncStateInfo);
            this.panel1.Controls.Add(this.lblWorkingFileName);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 621);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(784, 40);
            this.panel1.TabIndex = 8;
            // 
            // lblAsyncStateInfo
            // 
            this.lblAsyncStateInfo.AutoSize = true;
            this.lblAsyncStateInfo.Location = new System.Drawing.Point(242, 13);
            this.lblAsyncStateInfo.Name = "lblAsyncStateInfo";
            this.lblAsyncStateInfo.Size = new System.Drawing.Size(107, 15);
            this.lblAsyncStateInfo.TabIndex = 4;
            this.lblAsyncStateInfo.Text = "[lblAsyncStateInfo]";
            // 
            // lblWorkingFileName
            // 
            this.lblWorkingFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWorkingFileName.AutoSize = true;
            this.lblWorkingFileName.Location = new System.Drawing.Point(113, 13);
            this.lblWorkingFileName.Name = "lblWorkingFileName";
            this.lblWorkingFileName.Size = new System.Drawing.Size(123, 15);
            this.lblWorkingFileName.TabIndex = 3;
            this.lblWorkingFileName.Text = "[lblWorkingFileName]";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(784, 24);
            this.menuMain.TabIndex = 9;
            this.menuMain.Text = "menuMain";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.newCollectionMenuItem,
            this.openCollectionMenuItem,
            this.openRecentCollectionsMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(143, 6);
            // 
            // newCollectionMenuItem
            // 
            this.newCollectionMenuItem.Name = "newCollectionMenuItem";
            this.newCollectionMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newCollectionMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newCollectionMenuItem.Text = "New";
            this.newCollectionMenuItem.Click += new System.EventHandler(this.newCollectionMenuItem_Click);
            // 
            // openCollectionMenuItem
            // 
            this.openCollectionMenuItem.Name = "openCollectionMenuItem";
            this.openCollectionMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openCollectionMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openCollectionMenuItem.Text = "Open";
            this.openCollectionMenuItem.Click += new System.EventHandler(this.openCollectionMenuItem_Click);
            // 
            // openRecentCollectionsMenuItem
            // 
            this.openRecentCollectionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file1ToolStripMenuItem});
            this.openRecentCollectionsMenuItem.Name = "openRecentCollectionsMenuItem";
            this.openRecentCollectionsMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openRecentCollectionsMenuItem.Text = "Open Recent";
            // 
            // file1ToolStripMenuItem
            // 
            this.file1ToolStripMenuItem.Name = "file1ToolStripMenuItem";
            this.file1ToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.file1ToolStripMenuItem.Text = "File 1";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(143, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.foldersToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem1.Text = "Add";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // foldersToolStripMenuItem
            // 
            this.foldersToolStripMenuItem.Name = "foldersToolStripMenuItem";
            this.foldersToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.foldersToolStripMenuItem.Text = "Subfolders";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(143, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeDuplicatesMenuItem,
            this.toolStripMenuItem8,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem11,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // removeDuplicatesMenuItem
            // 
            this.removeDuplicatesMenuItem.Name = "removeDuplicatesMenuItem";
            this.removeDuplicatesMenuItem.Size = new System.Drawing.Size(175, 22);
            this.removeDuplicatesMenuItem.Text = "Remove Duplicates";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(172, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(172, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeMissingFilesMenuItem,
            this.updateFileListMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // removeMissingFilesMenuItem
            // 
            this.removeMissingFilesMenuItem.Name = "removeMissingFilesMenuItem";
            this.removeMissingFilesMenuItem.Size = new System.Drawing.Size(185, 22);
            this.removeMissingFilesMenuItem.Text = "Remove missing files";
            this.removeMissingFilesMenuItem.Click += new System.EventHandler(this.removeMissingFilesMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoloadPreviousFileToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // autoloadPreviousFileToolStripMenuItem
            // 
            this.autoloadPreviousFileToolStripMenuItem.Checked = true;
            this.autoloadPreviousFileToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoloadPreviousFileToolStripMenuItem.Name = "autoloadPreviousFileToolStripMenuItem";
            this.autoloadPreviousFileToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.autoloadPreviousFileToolStripMenuItem.Text = "Autoload Previous File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(232, 6);
            // 
            // updateFileListMenuItem
            // 
            this.updateFileListMenuItem.Name = "updateFileListMenuItem";
            this.updateFileListMenuItem.Size = new System.Drawing.Size(185, 22);
            this.updateFileListMenuItem.Text = "Update Selected Dir";
            this.updateFileListMenuItem.Click += new System.EventHandler(this.updateFileListMenuItem_Click);
            // 
            // FormAddImageSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "FormAddImageSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Image Collection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAddImageSource_FormClosing);
            this.Load += new System.EventHandler(this.FormAddImageSource_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuAddSource.ResumeLayout(false);
            this.grpTargetCollection.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuOutputImageList.ResumeLayout(false);
            this.groupBoxSummary.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpTargetCollection;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCombinedSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWorkingFileName;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem newCollectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCollectionMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem foldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView treeViewFileSystem;
        private System.Windows.Forms.ComboBox cbDrives;
        private System.Windows.Forms.ContextMenuStrip contextMenuAddSource;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuOutputImageList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddFiles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddFlderRecursive;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ImageList imgListIcons;
        private System.Windows.Forms.ToolStripMenuItem addFolderRecursiveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem updateFolderMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxSummary;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblRootFolders;
        private System.Windows.Forms.Label lblImages;
        private System.Windows.Forms.Label lblFolders;
        private System.Windows.Forms.Label lblDriveCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem openRecentCollectionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem file1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicatesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imgListViewIcons;
        private System.Windows.Forms.TreeView treeViewImgCollection;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblImageCount;
        private System.Windows.Forms.ListBox lstBoxOutputFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem autoloadPreviousFileToolStripMenuItem;
        private ToolStripMenuItem removeMissingFilesMenuItem;
        private Label lblAsyncStateInfo;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem UnpdateResultInTargetDirMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem updateFileListMenuItem;
    }
}
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
            components = new System.ComponentModel.Container();
            TreeNode treeNode1 = new TreeNode("Node1");
            TreeNode treeNode2 = new TreeNode("Node13");
            TreeNode treeNode3 = new TreeNode("Node10", new TreeNode[] { treeNode2 });
            TreeNode treeNode4 = new TreeNode("Node11");
            TreeNode treeNode5 = new TreeNode("Node12");
            TreeNode treeNode6 = new TreeNode("Node0", new TreeNode[] { treeNode1, treeNode3, treeNode4, treeNode5 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddImageSource));
            TreeNode treeNode7 = new TreeNode("Folder1");
            TreeNode treeNode8 = new TreeNode("Sub 1");
            TreeNode treeNode9 = new TreeNode("Folder2", new TreeNode[] { treeNode8 });
            TreeNode treeNode10 = new TreeNode("Subfolder 1");
            TreeNode treeNode11 = new TreeNode("Subfolder 2");
            TreeNode treeNode12 = new TreeNode("Subfolder 3");
            TreeNode treeNode13 = new TreeNode("Folder 3", new TreeNode[] { treeNode10, treeNode11, treeNode12 });
            groupBox1 = new GroupBox();
            panel2 = new Panel();
            treeViewFileSystem = new TreeView();
            contextMenuAddSource = new ContextMenuStrip(components);
            addFolderRecursiveMenuItem = new ToolStripMenuItem();
            addFolderMenuItem = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripSeparator();
            updateFolderMenuItem = new ToolStripMenuItem();
            imgListIcons = new ImageList(components);
            cbDrives = new ComboBox();
            btnLoad = new Button();
            btnCancel = new Button();
            grpTargetCollection = new GroupBox();
            panel3 = new Panel();
            splitContainer2 = new SplitContainer();
            treeViewImgCollection = new TreeView();
            contextMenuOutputImageList = new ContextMenuStrip(components);
            toolStripMenuItemAddFiles = new ToolStripMenuItem();
            toolStripMenuItemAddFolder = new ToolStripMenuItem();
            toolStripMenuItemAddFlderRecursive = new ToolStripMenuItem();
            toolStripMenuItem10 = new ToolStripSeparator();
            UnpdateResultInTargetDirMenuItem = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripSeparator();
            toolStripMenuItemRemoveItem = new ToolStripMenuItem();
            toolStripMenuItem12 = new ToolStripSeparator();
            clearAllToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripSeparator();
            imgListViewIcons = new ImageList(components);
            lblImageCount = new Label();
            lstBoxOutputFiles = new ListBox();
            groupBoxSummary = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblRootFolders = new Label();
            lblImages = new Label();
            lblFolders = new Label();
            lblDriveCount = new Label();
            label4 = new Label();
            lblCombinedSize = new Label();
            label1 = new Label();
            label2 = new Label();
            label6 = new Label();
            label7 = new Label();
            columnHeader1 = new ColumnHeader();
            splitContainer1 = new SplitContainer();
            panel1 = new Panel();
            panel5 = new Panel();
            lblWorkingFileName = new Label();
            txtFullNameOfNode = new TextBox();
            lblAsyncStateInfo = new Label();
            label3 = new Label();
            panel4 = new Panel();
            menuMain = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            newCollectionMenuItem = new ToolStripMenuItem();
            openCollectionMenuItem = new ToolStripMenuItem();
            openRecentCollectionsMenuItem = new ToolStripMenuItem();
            file1ToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            toolStripMenuItem1 = new ToolStripMenuItem();
            filesToolStripMenuItem = new ToolStripMenuItem();
            foldersToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            closeToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            removeDuplicatesMenuItem = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripSeparator();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem11 = new ToolStripSeparator();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            removeMissingFilesMenuItem = new ToolStripMenuItem();
            updateFileListMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            autoloadPreviousFileToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            saveFileDialog1 = new SaveFileDialog();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            contextMenuAddSource.SuspendLayout();
            grpTargetCollection.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            contextMenuOutputImageList.SuspendLayout();
            groupBoxSummary.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            menuMain.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(panel2);
            groupBox1.Controls.Add(cbDrives);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(5, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(233, 587);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Source selection";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(treeViewFileSystem);
            panel2.Location = new Point(3, 47);
            panel2.Name = "panel2";
            panel2.Size = new Size(227, 537);
            panel2.TabIndex = 7;
            // 
            // treeViewFileSystem
            // 
            treeViewFileSystem.AllowDrop = true;
            treeViewFileSystem.ContextMenuStrip = contextMenuAddSource;
            treeViewFileSystem.Dock = DockStyle.Fill;
            treeViewFileSystem.ImageKey = "Folder";
            treeViewFileSystem.ImageList = imgListIcons;
            treeViewFileSystem.Location = new Point(0, 0);
            treeViewFileSystem.Name = "treeViewFileSystem";
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
            treeViewFileSystem.Nodes.AddRange(new TreeNode[] { treeNode6 });
            treeViewFileSystem.SelectedImageIndex = 2;
            treeViewFileSystem.Size = new Size(227, 537);
            treeViewFileSystem.TabIndex = 6;
            treeViewFileSystem.ItemDrag += treeViewFileSystem_ItemDrag;
            treeViewFileSystem.MouseClick += treeViewFileSystem_MouseClick;
            treeViewFileSystem.MouseDoubleClick += treeViewFileSystem_MouseDoubleClick;
            // 
            // contextMenuAddSource
            // 
            contextMenuAddSource.Items.AddRange(new ToolStripItem[] { addFolderRecursiveMenuItem, addFolderMenuItem, toolStripMenuItem7, updateFolderMenuItem });
            contextMenuAddSource.Name = "contextMenuAddSource";
            contextMenuAddSource.Size = new Size(186, 76);
            // 
            // addFolderRecursiveMenuItem
            // 
            addFolderRecursiveMenuItem.Name = "addFolderRecursiveMenuItem";
            addFolderRecursiveMenuItem.Size = new Size(185, 22);
            addFolderRecursiveMenuItem.Text = "Add Folder Recursive";
            addFolderRecursiveMenuItem.Click += addFolderRecursiveMenuItem_Click;
            // 
            // addFolderMenuItem
            // 
            addFolderMenuItem.Name = "addFolderMenuItem";
            addFolderMenuItem.Size = new Size(185, 22);
            addFolderMenuItem.Text = "Add Folder";
            addFolderMenuItem.Click += addFolderMenuItem_Click;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(182, 6);
            // 
            // updateFolderMenuItem
            // 
            updateFolderMenuItem.Name = "updateFolderMenuItem";
            updateFolderMenuItem.Size = new Size(185, 22);
            updateFolderMenuItem.Text = "Update";
            updateFolderMenuItem.Click += updateFolderMenuItem_Click;
            // 
            // imgListIcons
            // 
            imgListIcons.ColorDepth = ColorDepth.Depth32Bit;
            imgListIcons.ImageStream = (ImageListStreamer)resources.GetObject("imgListIcons.ImageStream");
            imgListIcons.TransparentColor = Color.Transparent;
            imgListIcons.Images.SetKeyName(0, "Drive");
            imgListIcons.Images.SetKeyName(1, "Folder");
            imgListIcons.Images.SetKeyName(2, "FolderSelected");
            imgListIcons.Images.SetKeyName(3, "FolderDocs");
            imgListIcons.Images.SetKeyName(4, "Folder2");
            imgListIcons.Images.SetKeyName(5, "Folder2Open");
            // 
            // cbDrives
            // 
            cbDrives.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbDrives.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDrives.FormattingEnabled = true;
            cbDrives.Location = new Point(3, 18);
            cbDrives.Name = "cbDrives";
            cbDrives.Size = new Size(227, 23);
            cbDrives.TabIndex = 5;
            cbDrives.SelectedIndexChanged += cbDrives_SelectedIndexChanged;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLoad.Enabled = false;
            btnLoad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLoad.Location = new Point(4, 2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(95, 28);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(104, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(95, 28);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Close";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // grpTargetCollection
            // 
            grpTargetCollection.Controls.Add(panel3);
            grpTargetCollection.Controls.Add(groupBoxSummary);
            grpTargetCollection.Dock = DockStyle.Fill;
            grpTargetCollection.Location = new Point(5, 2);
            grpTargetCollection.Margin = new Padding(3, 0, 3, 0);
            grpTargetCollection.Name = "grpTargetCollection";
            grpTargetCollection.Padding = new Padding(5, 5, 6, 3);
            grpTargetCollection.Size = new Size(525, 587);
            grpTargetCollection.TabIndex = 5;
            grpTargetCollection.TabStop = false;
            grpTargetCollection.Text = "Target collection";
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(splitContainer2);
            panel3.Location = new Point(8, 18);
            panel3.Name = "panel3";
            panel3.Size = new Size(508, 495);
            panel3.TabIndex = 7;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(treeViewImgCollection);
            splitContainer2.Panel1MinSize = 150;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(lblImageCount);
            splitContainer2.Panel2.Controls.Add(lstBoxOutputFiles);
            splitContainer2.Panel2MinSize = 125;
            splitContainer2.Size = new Size(508, 495);
            splitContainer2.SplitterDistance = 318;
            splitContainer2.TabIndex = 2;
            // 
            // treeViewImgCollection
            // 
            treeViewImgCollection.AllowDrop = true;
            treeViewImgCollection.ContextMenuStrip = contextMenuOutputImageList;
            treeViewImgCollection.Dock = DockStyle.Fill;
            treeViewImgCollection.ImageIndex = 0;
            treeViewImgCollection.ImageList = imgListViewIcons;
            treeViewImgCollection.Location = new Point(0, 0);
            treeViewImgCollection.Name = "treeViewImgCollection";
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
            treeViewImgCollection.Nodes.AddRange(new TreeNode[] { treeNode7, treeNode9, treeNode13 });
            treeViewImgCollection.SelectedImageIndex = 1;
            treeViewImgCollection.Size = new Size(318, 495);
            treeViewImgCollection.TabIndex = 0;
            treeViewImgCollection.BeforeExpand += treeViewImgCollection_BeforeExpand;
            treeViewImgCollection.AfterSelect += treeViewImgCollection_AfterSelect;
            treeViewImgCollection.DragDrop += treeViewImgCollection_DragDrop;
            treeViewImgCollection.DragEnter += treeViewImgCollection_DragEnter;
            treeViewImgCollection.KeyUp += treeViewImgCollection_KeyUp;
            treeViewImgCollection.MouseMove += treeViewImgCollection_MouseMove;
            // 
            // contextMenuOutputImageList
            // 
            contextMenuOutputImageList.Items.AddRange(new ToolStripItem[] { toolStripMenuItemAddFiles, toolStripMenuItemAddFolder, toolStripMenuItemAddFlderRecursive, toolStripMenuItem10, UnpdateResultInTargetDirMenuItem, toolStripMenuItem6, toolStripMenuItemRemoveItem, toolStripMenuItem12, clearAllToolStripMenuItem, toolStripMenuItem9 });
            contextMenuOutputImageList.Name = "contextMenuOutputImageList";
            contextMenuOutputImageList.Size = new Size(236, 160);
            // 
            // toolStripMenuItemAddFiles
            // 
            toolStripMenuItemAddFiles.Name = "toolStripMenuItemAddFiles";
            toolStripMenuItemAddFiles.Size = new Size(235, 22);
            toolStripMenuItemAddFiles.Text = "Add Files";
            toolStripMenuItemAddFiles.Click += toolStripMenuItemAddFiles_Click;
            // 
            // toolStripMenuItemAddFolder
            // 
            toolStripMenuItemAddFolder.Name = "toolStripMenuItemAddFolder";
            toolStripMenuItemAddFolder.Size = new Size(235, 22);
            toolStripMenuItemAddFolder.Text = "Add Folder";
            toolStripMenuItemAddFolder.Click += toolStripMenuItemAddFolder_Click;
            // 
            // toolStripMenuItemAddFlderRecursive
            // 
            toolStripMenuItemAddFlderRecursive.Name = "toolStripMenuItemAddFlderRecursive";
            toolStripMenuItemAddFlderRecursive.Size = new Size(235, 22);
            toolStripMenuItemAddFlderRecursive.Text = "Add Folder Recursive";
            toolStripMenuItemAddFlderRecursive.Click += toolStripMenuItemAddFolderRecursive_Click;
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new Size(232, 6);
            // 
            // UnpdateResultInTargetDirMenuItem
            // 
            UnpdateResultInTargetDirMenuItem.Name = "UnpdateResultInTargetDirMenuItem";
            UnpdateResultInTargetDirMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            UnpdateResultInTargetDirMenuItem.Size = new Size(235, 22);
            UnpdateResultInTargetDirMenuItem.Text = "Update Folder Content";
            UnpdateResultInTargetDirMenuItem.Click += UpdateResultInTargetDirMenuItem_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(232, 6);
            // 
            // toolStripMenuItemRemoveItem
            // 
            toolStripMenuItemRemoveItem.Name = "toolStripMenuItemRemoveItem";
            toolStripMenuItemRemoveItem.Size = new Size(235, 22);
            toolStripMenuItemRemoveItem.Text = "Remove Selected Item (Del)";
            toolStripMenuItemRemoveItem.Click += toolStripMenuItemRemoveItem_Click;
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new Size(232, 6);
            // 
            // clearAllToolStripMenuItem
            // 
            clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            clearAllToolStripMenuItem.Size = new Size(235, 22);
            clearAllToolStripMenuItem.Text = "Clear All";
            clearAllToolStripMenuItem.Click += clearAllToolStripMenuItem_Click;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(232, 6);
            // 
            // imgListViewIcons
            // 
            imgListViewIcons.ColorDepth = ColorDepth.Depth32Bit;
            imgListViewIcons.ImageStream = (ImageListStreamer)resources.GetObject("imgListViewIcons.ImageStream");
            imgListViewIcons.TransparentColor = Color.Transparent;
            imgListViewIcons.Images.SetKeyName(0, "Folder");
            imgListViewIcons.Images.SetKeyName(1, "Folder2");
            imgListViewIcons.Images.SetKeyName(2, "Image2");
            imgListViewIcons.Images.SetKeyName(3, "Image");
            // 
            // lblImageCount
            // 
            lblImageCount.AutoSize = true;
            lblImageCount.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblImageCount.Location = new Point(3, 3);
            lblImageCount.Name = "lblImageCount";
            lblImageCount.Size = new Size(85, 17);
            lblImageCount.TabIndex = 2;
            lblImageCount.Text = "Images: 1000";
            // 
            // lstBoxOutputFiles
            // 
            lstBoxOutputFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstBoxOutputFiles.FormattingEnabled = true;
            lstBoxOutputFiles.ItemHeight = 15;
            lstBoxOutputFiles.Location = new Point(1, 27);
            lstBoxOutputFiles.Margin = new Padding(3, 3, 3, 0);
            lstBoxOutputFiles.Name = "lstBoxOutputFiles";
            lstBoxOutputFiles.Size = new Size(182, 469);
            lstBoxOutputFiles.TabIndex = 1;
            lstBoxOutputFiles.DoubleClick += lstBoxOutputFiles_DoubleClick;
            // 
            // groupBoxSummary
            // 
            groupBoxSummary.Controls.Add(tableLayoutPanel1);
            groupBoxSummary.Dock = DockStyle.Bottom;
            groupBoxSummary.Location = new Point(5, 519);
            groupBoxSummary.Name = "groupBoxSummary";
            groupBoxSummary.Size = new Size(514, 65);
            groupBoxSummary.TabIndex = 6;
            groupBoxSummary.TabStop = false;
            groupBoxSummary.Text = "Summary";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(lblRootFolders, 0, 1);
            tableLayoutPanel1.Controls.Add(lblImages, 0, 1);
            tableLayoutPanel1.Controls.Add(lblFolders, 0, 1);
            tableLayoutPanel1.Controls.Add(lblDriveCount, 0, 1);
            tableLayoutPanel1.Controls.Add(label4, 0, 0);
            tableLayoutPanel1.Controls.Add(lblCombinedSize, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 2, 0);
            tableLayoutPanel1.Controls.Add(label6, 3, 0);
            tableLayoutPanel1.Controls.Add(label7, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 51.92308F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.07692F));
            tableLayoutPanel1.Size = new Size(508, 43);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // lblRootFolders
            // 
            lblRootFolders.Anchor = AnchorStyles.Left;
            lblRootFolders.AutoSize = true;
            lblRootFolders.Location = new Point(104, 25);
            lblRootFolders.Name = "lblRootFolders";
            lblRootFolders.Size = new Size(13, 15);
            lblRootFolders.TabIndex = 11;
            lblRootFolders.Text = "0";
            // 
            // lblImages
            // 
            lblImages.Anchor = AnchorStyles.Left;
            lblImages.AutoSize = true;
            lblImages.Location = new Point(3, 25);
            lblImages.Name = "lblImages";
            lblImages.Size = new Size(13, 15);
            lblImages.TabIndex = 10;
            lblImages.Text = "0";
            // 
            // lblFolders
            // 
            lblFolders.Anchor = AnchorStyles.Left;
            lblFolders.AutoSize = true;
            lblFolders.Location = new Point(205, 25);
            lblFolders.Name = "lblFolders";
            lblFolders.Size = new Size(13, 15);
            lblFolders.TabIndex = 9;
            lblFolders.Text = "0";
            // 
            // lblDriveCount
            // 
            lblDriveCount.Anchor = AnchorStyles.Left;
            lblDriveCount.AutoSize = true;
            lblDriveCount.Location = new Point(407, 25);
            lblDriveCount.Name = "lblDriveCount";
            lblDriveCount.Size = new Size(13, 15);
            lblDriveCount.TabIndex = 8;
            lblDriveCount.Text = "0";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(3, 3);
            label4.Name = "label4";
            label4.Size = new Size(45, 15);
            label4.TabIndex = 3;
            label4.Text = "Images";
            // 
            // lblCombinedSize
            // 
            lblCombinedSize.Anchor = AnchorStyles.Left;
            lblCombinedSize.AutoSize = true;
            lblCombinedSize.Location = new Point(306, 25);
            lblCombinedSize.Name = "lblCombinedSize";
            lblCombinedSize.Size = new Size(13, 15);
            lblCombinedSize.TabIndex = 1;
            lblCombinedSize.Text = "0";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(104, 3);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 4;
            label1.Text = "Root folders";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(205, 3);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 5;
            label2.Text = "Folders";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Location = new Point(306, 3);
            label6.Name = "label6";
            label6.Size = new Size(86, 15);
            label6.TabIndex = 6;
            label6.Text = "Combined Size";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Left;
            label7.AutoSize = true;
            label7.Location = new Point(407, 3);
            label7.Name = "label7";
            label7.Size = new Size(70, 15);
            label7.TabIndex = 7;
            label7.Text = "Drive Count";
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 250;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(2, 27);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            splitContainer1.Panel1.Padding = new Padding(5, 2, 5, 5);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(grpTargetCollection);
            splitContainer1.Panel2.Padding = new Padding(5, 2, 5, 5);
            splitContainer1.Size = new Size(782, 594);
            splitContainer1.SplitterDistance = 243;
            splitContainer1.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(2, 626);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(3);
            panel1.Size = new Size(780, 35);
            panel1.TabIndex = 8;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel5.Controls.Add(lblWorkingFileName);
            panel5.Controls.Add(txtFullNameOfNode);
            panel5.Controls.Add(lblAsyncStateInfo);
            panel5.Controls.Add(label3);
            panel5.Location = new Point(7, 624);
            panel5.Name = "panel5";
            panel5.Size = new Size(566, 35);
            panel5.TabIndex = 14;
            panel5.Resize += panel5_Resize;
            // 
            // lblWorkingFileName
            // 
            lblWorkingFileName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblWorkingFileName.AutoSize = true;
            lblWorkingFileName.Location = new Point(102, 8);
            lblWorkingFileName.Margin = new Padding(3);
            lblWorkingFileName.Name = "lblWorkingFileName";
            lblWorkingFileName.Size = new Size(123, 15);
            lblWorkingFileName.TabIndex = 11;
            lblWorkingFileName.Text = "[lblWorkingFileName]";
            // 
            // txtFullNameOfNode
            // 
            txtFullNameOfNode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtFullNameOfNode.BackColor = Color.WhiteSmoke;
            txtFullNameOfNode.ForeColor = Color.Black;
            txtFullNameOfNode.Location = new Point(344, 7);
            txtFullNameOfNode.Margin = new Padding(3, 0, 0, 3);
            txtFullNameOfNode.MinimumSize = new Size(50, 10);
            txtFullNameOfNode.Name = "txtFullNameOfNode";
            txtFullNameOfNode.ReadOnly = true;
            txtFullNameOfNode.Size = new Size(220, 23);
            txtFullNameOfNode.TabIndex = 13;
            txtFullNameOfNode.Text = "[txtFullNameOfNode]";
            // 
            // lblAsyncStateInfo
            // 
            lblAsyncStateInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAsyncStateInfo.AutoSize = true;
            lblAsyncStateInfo.Location = new Point(231, 8);
            lblAsyncStateInfo.Margin = new Padding(3);
            lblAsyncStateInfo.Name = "lblAsyncStateInfo";
            lblAsyncStateInfo.Size = new Size(107, 15);
            lblAsyncStateInfo.TabIndex = 12;
            lblAsyncStateInfo.Text = "[lblAsyncStateInfo]";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(4, 8);
            label3.Margin = new Padding(3);
            label3.Name = "label3";
            label3.Size = new Size(101, 15);
            label3.TabIndex = 10;
            label3.Text = "Source collection:";
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panel4.Controls.Add(btnLoad);
            panel4.Controls.Add(btnCancel);
            panel4.Location = new Point(575, 624);
            panel4.Margin = new Padding(2, 3, 3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(209, 35);
            panel4.TabIndex = 9;
            // 
            // menuMain
            // 
            menuMain.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, viewToolStripMenuItem, helpToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(784, 24);
            menuMain.TabIndex = 9;
            menuMain.Text = "menuMain";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem4, newCollectionMenuItem, openCollectionMenuItem, openRecentCollectionsMenuItem, toolStripMenuItem3, toolStripMenuItem1, toolStripMenuItem2, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripMenuItem5, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(143, 6);
            // 
            // newCollectionMenuItem
            // 
            newCollectionMenuItem.Name = "newCollectionMenuItem";
            newCollectionMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newCollectionMenuItem.Size = new Size(146, 22);
            newCollectionMenuItem.Text = "New";
            newCollectionMenuItem.Click += newCollectionMenuItem_Click;
            // 
            // openCollectionMenuItem
            // 
            openCollectionMenuItem.Name = "openCollectionMenuItem";
            openCollectionMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openCollectionMenuItem.Size = new Size(146, 22);
            openCollectionMenuItem.Text = "Open";
            openCollectionMenuItem.Click += openCollectionMenuItem_Click;
            // 
            // openRecentCollectionsMenuItem
            // 
            openRecentCollectionsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { file1ToolStripMenuItem });
            openRecentCollectionsMenuItem.Name = "openRecentCollectionsMenuItem";
            openRecentCollectionsMenuItem.Size = new Size(146, 22);
            openRecentCollectionsMenuItem.Text = "Open Recent";
            // 
            // file1ToolStripMenuItem
            // 
            file1ToolStripMenuItem.Name = "file1ToolStripMenuItem";
            file1ToolStripMenuItem.Size = new Size(101, 22);
            file1ToolStripMenuItem.Text = "File 1";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(143, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { filesToolStripMenuItem, foldersToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(146, 22);
            toolStripMenuItem1.Text = "Add";
            // 
            // filesToolStripMenuItem
            // 
            filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            filesToolStripMenuItem.Size = new Size(130, 22);
            filesToolStripMenuItem.Text = "Files";
            // 
            // foldersToolStripMenuItem
            // 
            foldersToolStripMenuItem.Name = "foldersToolStripMenuItem";
            foldersToolStripMenuItem.Size = new Size(130, 22);
            foldersToolStripMenuItem.Text = "Subfolders";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(146, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(146, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(143, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(146, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { removeDuplicatesMenuItem, toolStripMenuItem8, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, toolStripMenuItem11, deleteToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // removeDuplicatesMenuItem
            // 
            removeDuplicatesMenuItem.Name = "removeDuplicatesMenuItem";
            removeDuplicatesMenuItem.Size = new Size(175, 22);
            removeDuplicatesMenuItem.Text = "Remove Duplicates";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(172, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new Size(175, 22);
            cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(175, 22);
            copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(175, 22);
            pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new Size(172, 6);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(175, 22);
            deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { removeMissingFilesMenuItem, updateFileListMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // removeMissingFilesMenuItem
            // 
            removeMissingFilesMenuItem.Name = "removeMissingFilesMenuItem";
            removeMissingFilesMenuItem.Size = new Size(185, 22);
            removeMissingFilesMenuItem.Text = "Remove missing files";
            removeMissingFilesMenuItem.Click += removeMissingFilesMenuItem_Click;
            // 
            // updateFileListMenuItem
            // 
            updateFileListMenuItem.Name = "updateFileListMenuItem";
            updateFileListMenuItem.Size = new Size(185, 22);
            updateFileListMenuItem.Text = "Update Selected Dir";
            updateFileListMenuItem.Click += updateFileListMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { autoloadPreviousFileToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "&View";
            // 
            // autoloadPreviousFileToolStripMenuItem
            // 
            autoloadPreviousFileToolStripMenuItem.Checked = true;
            autoloadPreviousFileToolStripMenuItem.CheckState = CheckState.Checked;
            autoloadPreviousFileToolStripMenuItem.Name = "autoloadPreviousFileToolStripMenuItem";
            autoloadPreviousFileToolStripMenuItem.Size = new Size(192, 22);
            autoloadPreviousFileToolStripMenuItem.Text = "Autoload Previous File";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormAddImageSource
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 661);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Controls.Add(splitContainer1);
            Controls.Add(menuMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuMain;
            MinimumSize = new Size(500, 350);
            Name = "FormAddImageSource";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select Image Collection";
            FormClosing += FormAddImageSource_FormClosing;
            Load += FormAddImageSource_Load;
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            contextMenuAddSource.ResumeLayout(false);
            grpTargetCollection.ResumeLayout(false);
            panel3.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            contextMenuOutputImageList.ResumeLayout(false);
            groupBoxSummary.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpTargetCollection;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblCombinedSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
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
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem UnpdateResultInTargetDirMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem updateFileListMenuItem;
        private Panel panel4;
        private TextBox txtFullNameOfNode;
        private Label label3;
        private Label lblWorkingFileName;
        private Label lblAsyncStateInfo;
        private Panel panel5;
    }
}
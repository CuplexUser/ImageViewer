namespace ImageViewer.UserControls
{
    partial class CreateBookmarkFolder
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateBookmarkFolder));
            this.bookmarksTree = new System.Windows.Forms.TreeView();
            this.contextMenuStripFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.renameFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderImages = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBookmarkName = new System.Windows.Forms.TextBox();
            this.bookmarkFolderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.contextMenuStripFolders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bookmarksTree
            // 
            this.bookmarksTree.AllowDrop = true;
            this.bookmarksTree.ContextMenuStrip = this.contextMenuStripFolders;
            this.bookmarksTree.ImageIndex = 0;
            this.bookmarksTree.ImageList = this.FolderImages;
            this.bookmarksTree.Location = new System.Drawing.Point(17, 79);
            this.bookmarksTree.Name = "bookmarksTree";
            this.bookmarksTree.SelectedImageIndex = 1;
            this.bookmarksTree.ShowPlusMinus = false;
            this.bookmarksTree.ShowRootLines = false;
            this.bookmarksTree.Size = new System.Drawing.Size(500, 450);
            this.bookmarksTree.TabIndex = 1;
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
            this.contextMenuStripFolders.Size = new System.Drawing.Size(179, 82);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // renameFolderMenuItem
            // 
            this.renameFolderMenuItem.Name = "renameFolderMenuItem";
            this.renameFolderMenuItem.Size = new System.Drawing.Size(178, 24);
            this.renameFolderMenuItem.Text = "Rename Folder";
            this.renameFolderMenuItem.Click += new System.EventHandler(this.renameFolderMenuItem_Click);
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
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(311, 539);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(417, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.Location = new System.Drawing.Point(17, 539);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(125, 30);
            this.btnNewFolder.TabIndex = 5;
            this.btnNewFolder.Text = "New Folder";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Edit Bookmark";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name:";
            // 
            // txtBookmarkName
            // 
            this.txtBookmarkName.Location = new System.Drawing.Point(69, 46);
            this.txtBookmarkName.MaxLength = 500;
            this.txtBookmarkName.Name = "txtBookmarkName";
            this.txtBookmarkName.Size = new System.Drawing.Size(448, 22);
            this.txtBookmarkName.TabIndex = 8;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.btnSave);
            this.mainPanel.Controls.Add(this.txtBookmarkName);
            this.mainPanel.Controls.Add(this.btnCancel);
            this.mainPanel.Controls.Add(this.label2);
            this.mainPanel.Controls.Add(this.btnNewFolder);
            this.mainPanel.Controls.Add(this.bookmarksTree);
            this.mainPanel.Location = new System.Drawing.Point(3, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(535, 575);
            this.mainPanel.TabIndex = 10;
            // 
            // CreateBookmarkFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Name = "CreateBookmarkFolder";
            this.Size = new System.Drawing.Size(553, 590);
            this.Load += new System.EventHandler(this.CreateBookmarkFolder_Load);
            this.contextMenuStripFolders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView bookmarksTree;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNewFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBookmarkName;
        private System.Windows.Forms.ImageList FolderImages;
        private System.Windows.Forms.BindingSource bookmarkFolderBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFolders;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem renameFolderMenuItem;
        private System.Windows.Forms.Panel mainPanel;
    }
}

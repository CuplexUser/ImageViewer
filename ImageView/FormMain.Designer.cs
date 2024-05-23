namespace ImageViewer
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openFileCollectionToolStripMenuItem = new ToolStripMenuItem();
            openBrowserToolStripMenuItem = new ToolStripMenuItem();
            menuItemOpenImage = new ToolStripMenuItem();
            menuItemLoadBookmarkedImages = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            autoLoadPreviousFolderToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            copyFilepathToolStripMenuItem = new ToolStripMenuItem();
            copyFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            openInDefaultApplicationToolStripMenuItem1 = new ToolStripMenuItem();
            deleteImageToolStripMenuItem = new ToolStripMenuItem();
            vivewToolStripMenuItem = new ToolStripMenuItem();
            showFullscreenToolStripMenuItem = new ToolStripMenuItem();
            topMostToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            setImageScalingModeToolStripMenuItem = new ToolStripMenuItem();
            zoomToolStripMenuItem = new ToolStripMenuItem();
            zoomInToolStripMenuItem = new ToolStripMenuItem();
            zoomOutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            resetZoomToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            imageDetailsToolStripMenuItem = new ToolStripMenuItem();
            openInDefaultApplicationToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            slideshowToolStripMenuItem = new ToolStripMenuItem();
            startToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            setIntervalToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            randomizeCollectionToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            openSettingsToolStripMenuItem = new ToolStripMenuItem();
            bookmarksToolStripMenuItem = new ToolStripMenuItem();
            openBookmarksToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator12 = new ToolStripSeparator();
            addBookmarkToolStripMenuItem = new ToolStripMenuItem();
            thumbnailsToolStripMenuItem = new ToolStripMenuItem();
            openThumbnailsToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator13 = new ToolStripSeparator();
            thumbnailDBSettingsToolStripMenuItem = new ToolStripMenuItem();
            windowsToolStripMenuItem = new ToolStripMenuItem();
            newWindowToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            autoArrangeToolStripMenuItem = new ToolStripMenuItem();
            showAllToolStripMenuItem = new ToolStripMenuItem();
            hideAllToolStripMenuItem = new ToolStripMenuItem();
            closeAllToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            windowsToolStripMenuItem1 = new ToolStripMenuItem();
            otherToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            folderBrowserDialog1 = new FolderBrowserDialog();
            timerSlideShow = new System.Windows.Forms.Timer(components);
            openFileDialog1 = new OpenFileDialog();
            pictureBox1 = new PictureBox();
            toolTipSlideshowState = new ToolTip(components);
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, vivewToolStripMenuItem, slideshowToolStripMenuItem, settingsToolStripMenuItem, bookmarksToolStripMenuItem, thumbnailsToolStripMenuItem, windowsToolStripMenuItem, otherToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(728, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openFileCollectionToolStripMenuItem, openBrowserToolStripMenuItem, menuItemOpenImage, menuItemLoadBookmarkedImages, toolStripSeparator4, autoLoadPreviousFolderToolStripMenuItem, toolStripSeparator11, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // openFileCollectionToolStripMenuItem
            // 
            openFileCollectionToolStripMenuItem.Name = "openFileCollectionToolStripMenuItem";
            openFileCollectionToolStripMenuItem.Size = new Size(211, 22);
            openFileCollectionToolStripMenuItem.Text = "&Open File Browser";
            openFileCollectionToolStripMenuItem.Click += openFileCollectionToolStripMenuItem_Click;
            // 
            // openBrowserToolStripMenuItem
            // 
            openBrowserToolStripMenuItem.Name = "openBrowserToolStripMenuItem";
            openBrowserToolStripMenuItem.Size = new Size(211, 22);
            openBrowserToolStripMenuItem.Text = "&Open Legacy File Browser";
            openBrowserToolStripMenuItem.Click += OpenFolderToolStripMenuItem_Click;
            // 
            // menuItemOpenImage
            // 
            menuItemOpenImage.Name = "menuItemOpenImage";
            menuItemOpenImage.Size = new Size(211, 22);
            menuItemOpenImage.Text = "Open &Image";
            menuItemOpenImage.Click += menuItemOpenImage_Click;
            // 
            // menuItemLoadBookmarkedImages
            // 
            menuItemLoadBookmarkedImages.Name = "menuItemLoadBookmarkedImages";
            menuItemLoadBookmarkedImages.Size = new Size(211, 22);
            menuItemLoadBookmarkedImages.Text = "Load Bookmarked images";
            menuItemLoadBookmarkedImages.Click += menuItemLoadBookmarkedImages_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(208, 6);
            // 
            // autoLoadPreviousFolderToolStripMenuItem
            // 
            autoLoadPreviousFolderToolStripMenuItem.Enabled = false;
            autoLoadPreviousFolderToolStripMenuItem.Name = "autoLoadPreviousFolderToolStripMenuItem";
            autoLoadPreviousFolderToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            autoLoadPreviousFolderToolStripMenuItem.ShowShortcutKeys = false;
            autoLoadPreviousFolderToolStripMenuItem.Size = new Size(211, 22);
            autoLoadPreviousFolderToolStripMenuItem.Text = "&Auto Load Previous folder";
            autoLoadPreviousFolderToolStripMenuItem.Click += autoLoadPreviousFolderToolStripMenuItem_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(208, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(211, 22);
            exitToolStripMenuItem.Text = "&Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyFilepathToolStripMenuItem, copyFileToolStripMenuItem, toolStripMenuItem1, openInDefaultApplicationToolStripMenuItem1, deleteImageToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // copyFilepathToolStripMenuItem
            // 
            copyFilepathToolStripMenuItem.Name = "copyFilepathToolStripMenuItem";
            copyFilepathToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyFilepathToolStripMenuItem.Size = new Size(221, 22);
            copyFilepathToolStripMenuItem.Text = "Copy Filepath";
            copyFilepathToolStripMenuItem.Click += copyFilepathToolStripMenuItem_Click;
            // 
            // copyFileToolStripMenuItem
            // 
            copyFileToolStripMenuItem.Name = "copyFileToolStripMenuItem";
            copyFileToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            copyFileToolStripMenuItem.Size = new Size(221, 22);
            copyFileToolStripMenuItem.Text = "Copy File";
            copyFileToolStripMenuItem.Click += copyFileToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(218, 6);
            // 
            // openInDefaultApplicationToolStripMenuItem1
            // 
            openInDefaultApplicationToolStripMenuItem1.Name = "openInDefaultApplicationToolStripMenuItem1";
            openInDefaultApplicationToolStripMenuItem1.Size = new Size(221, 22);
            openInDefaultApplicationToolStripMenuItem1.Text = "Open In Default Application";
            openInDefaultApplicationToolStripMenuItem1.Click += openInDefaultApplicationToolStripMenuItem1_Click;
            // 
            // deleteImageToolStripMenuItem
            // 
            deleteImageToolStripMenuItem.Name = "deleteImageToolStripMenuItem";
            deleteImageToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Delete;
            deleteImageToolStripMenuItem.Size = new Size(221, 22);
            deleteImageToolStripMenuItem.Text = "Delete Image";
            deleteImageToolStripMenuItem.Click += DeleteImageToolStripMenuItem_Click;
            // 
            // vivewToolStripMenuItem
            // 
            vivewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showFullscreenToolStripMenuItem, topMostToolStripMenuItem, toolStripSeparator7, setImageScalingModeToolStripMenuItem, zoomToolStripMenuItem, toolStripSeparator10, imageDetailsToolStripMenuItem, openInDefaultApplicationToolStripMenuItem, toolStripSeparator9 });
            vivewToolStripMenuItem.Name = "vivewToolStripMenuItem";
            vivewToolStripMenuItem.Size = new Size(44, 20);
            vivewToolStripMenuItem.Text = "&View";
            // 
            // showFullscreenToolStripMenuItem
            // 
            showFullscreenToolStripMenuItem.Name = "showFullscreenToolStripMenuItem";
            showFullscreenToolStripMenuItem.ShortcutKeys = Keys.F11;
            showFullscreenToolStripMenuItem.Size = new Size(221, 22);
            showFullscreenToolStripMenuItem.Text = "Show Fullscreen ";
            showFullscreenToolStripMenuItem.Click += showFullscreenToolStripMenuItem_Click;
            // 
            // topMostToolStripMenuItem
            // 
            topMostToolStripMenuItem.Name = "topMostToolStripMenuItem";
            topMostToolStripMenuItem.Size = new Size(221, 22);
            topMostToolStripMenuItem.Text = "Allways Ontop";
            topMostToolStripMenuItem.Click += topMostToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(218, 6);
            // 
            // setImageScalingModeToolStripMenuItem
            // 
            setImageScalingModeToolStripMenuItem.Name = "setImageScalingModeToolStripMenuItem";
            setImageScalingModeToolStripMenuItem.Size = new Size(221, 22);
            setImageScalingModeToolStripMenuItem.Text = "Set Image Scaling Mode";
            setImageScalingModeToolStripMenuItem.Click += setImageScalingModeToolStripMenuItem_Click;
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zoomInToolStripMenuItem, zoomOutToolStripMenuItem, toolStripSeparator8, resetZoomToolStripMenuItem });
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new Size(221, 22);
            zoomToolStripMenuItem.Text = "Zoom";
            // 
            // zoomInToolStripMenuItem
            // 
            zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            zoomInToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Up;
            zoomInToolStripMenuItem.Size = new Size(233, 22);
            zoomInToolStripMenuItem.Text = "Zoom In";
            // 
            // zoomOutToolStripMenuItem
            // 
            zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            zoomOutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Down;
            zoomOutToolStripMenuItem.Size = new Size(233, 22);
            zoomOutToolStripMenuItem.Text = "Zoom out";
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(230, 6);
            // 
            // resetZoomToolStripMenuItem
            // 
            resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
            resetZoomToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D0;
            resetZoomToolStripMenuItem.Size = new Size(233, 22);
            resetZoomToolStripMenuItem.Text = "Reset Zoom To Default";
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(218, 6);
            // 
            // imageDetailsToolStripMenuItem
            // 
            imageDetailsToolStripMenuItem.Name = "imageDetailsToolStripMenuItem";
            imageDetailsToolStripMenuItem.Size = new Size(221, 22);
            imageDetailsToolStripMenuItem.Text = "Show Image Details";
            imageDetailsToolStripMenuItem.Click += imageDetailsToolStripMenuItem_Click;
            // 
            // openInDefaultApplicationToolStripMenuItem
            // 
            openInDefaultApplicationToolStripMenuItem.Name = "openInDefaultApplicationToolStripMenuItem";
            openInDefaultApplicationToolStripMenuItem.Size = new Size(221, 22);
            openInDefaultApplicationToolStripMenuItem.Text = "Open In Default Application";
            openInDefaultApplicationToolStripMenuItem.Click += openInDefaultApplicationToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(218, 6);
            // 
            // slideshowToolStripMenuItem
            // 
            slideshowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem, setIntervalToolStripMenuItem, toolStripSeparator5 });
            slideshowToolStripMenuItem.Name = "slideshowToolStripMenuItem";
            slideshowToolStripMenuItem.Size = new Size(72, 20);
            slideshowToolStripMenuItem.Text = "&Slideshow";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.ShortcutKeyDisplayString = "";
            startToolStripMenuItem.ShortcutKeys = Keys.F5;
            startToolStripMenuItem.Size = new Size(132, 22);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += startToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.ShortcutKeys = Keys.F6;
            stopToolStripMenuItem.Size = new Size(132, 22);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // setIntervalToolStripMenuItem
            // 
            setIntervalToolStripMenuItem.Name = "setIntervalToolStripMenuItem";
            setIntervalToolStripMenuItem.Size = new Size(132, 22);
            setIntervalToolStripMenuItem.Text = "Set Interval";
            setIntervalToolStripMenuItem.Click += setIntervalToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(129, 6);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { randomizeCollectionToolStripMenuItem, toolStripSeparator6, openSettingsToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(46, 20);
            settingsToolStripMenuItem.Text = "&Tools";
            // 
            // randomizeCollectionToolStripMenuItem
            // 
            randomizeCollectionToolStripMenuItem.Name = "randomizeCollectionToolStripMenuItem";
            randomizeCollectionToolStripMenuItem.Size = new Size(186, 22);
            randomizeCollectionToolStripMenuItem.Text = "&Randomize sequence";
            randomizeCollectionToolStripMenuItem.Click += randomizeCollectionToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(183, 6);
            // 
            // openSettingsToolStripMenuItem
            // 
            openSettingsToolStripMenuItem.Name = "openSettingsToolStripMenuItem";
            openSettingsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            openSettingsToolStripMenuItem.Size = new Size(186, 22);
            openSettingsToolStripMenuItem.Text = "Settings";
            openSettingsToolStripMenuItem.Click += openSettingsToolStripMenuItem_Click;
            // 
            // bookmarksToolStripMenuItem
            // 
            bookmarksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openBookmarksToolStripMenuItem, toolStripSeparator12, addBookmarkToolStripMenuItem });
            bookmarksToolStripMenuItem.Name = "bookmarksToolStripMenuItem";
            bookmarksToolStripMenuItem.Size = new Size(78, 20);
            bookmarksToolStripMenuItem.Text = "&Bookmarks";
            // 
            // openBookmarksToolStripMenuItem
            // 
            openBookmarksToolStripMenuItem.Name = "openBookmarksToolStripMenuItem";
            openBookmarksToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.M;
            openBookmarksToolStripMenuItem.Size = new Size(210, 22);
            openBookmarksToolStripMenuItem.Text = "Open Bookmarks";
            openBookmarksToolStripMenuItem.Click += openBookmarksToolStripMenuItem_Click_1;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(207, 6);
            // 
            // addBookmarkToolStripMenuItem
            // 
            addBookmarkToolStripMenuItem.Name = "addBookmarkToolStripMenuItem";
            addBookmarkToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.B;
            addBookmarkToolStripMenuItem.Size = new Size(210, 22);
            addBookmarkToolStripMenuItem.Text = "Bookmark Image";
            addBookmarkToolStripMenuItem.Click += addBookmarkToolStripMenuItem_Click;
            // 
            // thumbnailsToolStripMenuItem
            // 
            thumbnailsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openThumbnailsToolStripMenuItem1, toolStripSeparator13, thumbnailDBSettingsToolStripMenuItem });
            thumbnailsToolStripMenuItem.Name = "thumbnailsToolStripMenuItem";
            thumbnailsToolStripMenuItem.Size = new Size(81, 20);
            thumbnailsToolStripMenuItem.Text = "&Thumbnails";
            // 
            // openThumbnailsToolStripMenuItem1
            // 
            openThumbnailsToolStripMenuItem1.Name = "openThumbnailsToolStripMenuItem1";
            openThumbnailsToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.T;
            openThumbnailsToolStripMenuItem1.Size = new Size(212, 22);
            openThumbnailsToolStripMenuItem1.Text = "Open Thumbnails";
            openThumbnailsToolStripMenuItem1.Click += openThumbnailsToolStripMenuItem1_Click;
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(209, 6);
            // 
            // thumbnailDBSettingsToolStripMenuItem
            // 
            thumbnailDBSettingsToolStripMenuItem.Name = "thumbnailDBSettingsToolStripMenuItem";
            thumbnailDBSettingsToolStripMenuItem.Size = new Size(212, 22);
            thumbnailDBSettingsToolStripMenuItem.Text = "Thumbnail Cache Settings";
            thumbnailDBSettingsToolStripMenuItem.Click += thumbnailDBSettingsToolStripMenuItem_Click;
            // 
            // windowsToolStripMenuItem
            // 
            windowsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newWindowToolStripMenuItem, toolStripSeparator2, autoArrangeToolStripMenuItem, showAllToolStripMenuItem, hideAllToolStripMenuItem, closeAllToolStripMenuItem, toolStripSeparator3, windowsToolStripMenuItem1 });
            windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            windowsToolStripMenuItem.Size = new Size(63, 20);
            windowsToolStripMenuItem.Text = "&Window";
            // 
            // newWindowToolStripMenuItem
            // 
            newWindowToolStripMenuItem.Name = "newWindowToolStripMenuItem";
            newWindowToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newWindowToolStripMenuItem.Size = new Size(194, 22);
            newWindowToolStripMenuItem.Text = "New Window";
            newWindowToolStripMenuItem.Click += newWindowToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(191, 6);
            // 
            // autoArrangeToolStripMenuItem
            // 
            autoArrangeToolStripMenuItem.Name = "autoArrangeToolStripMenuItem";
            autoArrangeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            autoArrangeToolStripMenuItem.Size = new Size(194, 22);
            autoArrangeToolStripMenuItem.Text = "Auto Arrange";
            autoArrangeToolStripMenuItem.Click += autoArrangeToolStripMenuItem_Click;
            // 
            // showAllToolStripMenuItem
            // 
            showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            showAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
            showAllToolStripMenuItem.Size = new Size(194, 22);
            showAllToolStripMenuItem.Text = "Show All";
            showAllToolStripMenuItem.Click += showAllToolStripMenuItem_Click;
            // 
            // hideAllToolStripMenuItem
            // 
            hideAllToolStripMenuItem.Name = "hideAllToolStripMenuItem";
            hideAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
            hideAllToolStripMenuItem.Size = new Size(194, 22);
            hideAllToolStripMenuItem.Text = "Hide All";
            hideAllToolStripMenuItem.Click += hideAllToolStripMenuItem_Click;
            // 
            // closeAllToolStripMenuItem
            // 
            closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            closeAllToolStripMenuItem.Size = new Size(194, 22);
            closeAllToolStripMenuItem.Text = "Close All";
            closeAllToolStripMenuItem.Click += closeAllToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(191, 6);
            // 
            // windowsToolStripMenuItem1
            // 
            windowsToolStripMenuItem1.Name = "windowsToolStripMenuItem1";
            windowsToolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.W;
            windowsToolStripMenuItem1.Size = new Size(194, 22);
            windowsToolStripMenuItem1.Text = "Windows...";
            windowsToolStripMenuItem1.Click += windowsToolStripMenuItem1_Click;
            // 
            // otherToolStripMenuItem
            // 
            otherToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, checkForUpdatesToolStripMenuItem });
            otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            otherToolStripMenuItem.Size = new Size(44, 20);
            otherToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(173, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new Size(173, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            // 
            // timerSlideShow
            // 
            timerSlideShow.Interval = 5000;
            timerSlideShow.Tick += timerSlideShow_Tick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Multiselect = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(0, 24);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(728, 486);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.LoadCompleted += pictureBox1_LoadCompleted;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            pictureBox1.MouseDoubleClick += pictureBox1_MouseClick;
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
            pictureBox1.MouseHover += pictureBox1_MouseHover;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            // 
            // toolTipSlideshowState
            // 
            toolTipSlideshowState.AutoPopDelay = 5000;
            toolTipSlideshowState.InitialDelay = 100;
            toolTipSlideshowState.ReshowDelay = 500;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(728, 510);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(463, 338);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Image Viewer";
            Activated += FormMain_Activated;
            FormClosing += FormMain_FormClosing;
            FormClosed += FormMain_FormClosed;
            Load += FormMain_Load;
            KeyDown += FormMain_KeyDown;
            KeyUp += FormMain_KeyUp;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slideshowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setIntervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSettingsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer timerSlideShow;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomizeCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyFilepathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem autoArrangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem autoLoadPreviousFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem vivewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topMostToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem resetZoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInDefaultApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setImageScalingModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem thumbnailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openThumbnailsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem thumbnailDBSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openInDefaultApplicationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoadBookmarkedImages;
        private System.Windows.Forms.ToolTip toolTipSlideshowState;
        private System.Windows.Forms.ToolStripMenuItem deleteImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileCollectionToolStripMenuItem;
    }
}


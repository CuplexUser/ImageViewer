using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;
using JetBrains.Annotations;

namespace ImageViewer
{
    [UsedImplicitly]
    public partial class FormAddBookmark : Form
    {
        private readonly BookmarkManager _bookmarkManager;
        private readonly BookmarkService _bookmarkService;
        private ImageReferenceElement _imageReference;
        private readonly ApplicationSettingsService _applicationSettingsService;
        public FormAddBookmark(BookmarkManager bookmarkManager, BookmarkService bookmarkService, ApplicationSettingsService applicationSettingsService)
        {
            InitializeComponent();

            _bookmarkManager = bookmarkManager;
            _bookmarkService = bookmarkService;
            _applicationSettingsService = applicationSettingsService;
        }

        public void Init(Point startupPosition, ImageReferenceElement imageReference)
        {
            SetDesktopLocation(startupPosition.X, startupPosition.Y);
            _imageReference = imageReference;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int csDropshadow = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= csDropshadow;
                return cp;
            }
        }

        private void FormAddBookmark_Load(object sender, EventArgs e)
        {

            if (!_bookmarkManager.LoadedFromFile && !_applicationSettingsService.Settings.PasswordProtectBookmarks)
            {
                _bookmarkService.OpenBookmarks();
            }

            if (_imageReference == null)
            {
                Close();
                return;
            }

            txtBookmarkName.Text = _imageReference.FileName;
            InitFolderDropdownList();
            txtBookmarkName.Focus();
            txtBookmarkName.SelectAll();
        }

        private void InitFolderDropdownList()
        {
            var bokmarkFoldersToBind = new List<BookmarkFolder> { _bookmarkManager.RootFolder };
            bokmarkFoldersToBind.AddRange(_bookmarkManager.RootFolder.BookmarkFolders);
            bokmarkFoldersToBind.Add(new BookmarkFolder { Name = "Chose another folder...", Id = "n/a", SortOrder = bokmarkFoldersToBind.Count });
            bookmarkFolderBindingSource.DataSource = bokmarkFoldersToBind;

            if (comboBoxBookmarkFolders.Items.Count > 0)
                comboBoxBookmarkFolders.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BookmarkFolder bookmarkFolder = comboBoxBookmarkFolders.SelectedItem as BookmarkFolder ?? _bookmarkManager.RootFolder;

            _bookmarkManager.AddBookmark(bookmarkFolder.Id, txtBookmarkName.Text, _imageReference);
            _bookmarkService.SaveBookmarks();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, IntPtr.Zero);
            }
        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            CreateTreeFolder();
        }

        private void txtBookmarkName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _bookmarkManager.AddBookmark(_bookmarkManager.RootFolder.Id, txtBookmarkName.Text, _imageReference);
                e.Handled = true;
                Close();
            }
        }

        private void FormAddBookmark_Shown(object sender, EventArgs e)
        {
            txtBookmarkName.Focus();
            txtBookmarkName.SelectAll();
        }

        private void comboBoxBookmarkFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBookmarkFolders.SelectedIndex == comboBoxBookmarkFolders.Items.Count - 1)
            {
                CreateTreeFolder();
            }
        }

        private void CreateTreeFolder()
        {
            var createFolderUserControl = new CreateBookmarkFolder(_bookmarkService, _bookmarkManager, _imageReference) {DefaultBookmarkName = _imageReference.FileName};
            var addFolderForm = FormFactory.CreateModalForm(createFolderUserControl);

            addFolderForm.ShowDialog(this);
            Close();
        }
    }
}
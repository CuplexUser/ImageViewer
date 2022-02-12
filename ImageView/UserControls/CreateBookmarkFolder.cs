using System;
using System.Windows.Forms;
using ImageViewer.DataBinding;
using ImageViewer.DataContracts;
using ImageViewer.InputForms;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Models.Enums;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.Utility;

namespace ImageViewer.UserControls
{
    public partial class CreateBookmarkFolder : UserControl
    {
        private readonly BookmarkManager _bookmarkManager;
        private readonly BookmarkService _bookmarkService;
        private TreeViewDataContext _treeViewDataContext;
        private readonly ImageReferenceElement _imageReference;

        public string DefaultBookmarkName { get; set; }

        public CreateBookmarkFolder(BookmarkService bookmarkService, BookmarkManager bookmarkManager, ImageReferenceElement imageReference)
        {
            _bookmarkManager = bookmarkManager;
            _imageReference = imageReference;
            _bookmarkService = bookmarkService;
            InitializeComponent();
        }

        private void CreateBookmarkFolder_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            txtBookmarkName.Text = DefaultBookmarkName;
            InitTreeView();
        }

        #region  Init

        private void InitBookmarksDataSource()
        {
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, _bookmarkManager.RootFolder);
            _treeViewDataContext.BindData();
        }

        private void InitTreeView()
        {
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, _bookmarkManager.RootFolder);
            _treeViewDataContext.BindData();
            bookmarksTree.AfterSelect += BookmarksTree_AfterSelect;

            if (!_bookmarkManager.LoadedFromFile)
            {
                _bookmarkService.OpenBookmarks();
            }

            InitBookmarksDataSource();
        }

        #endregion

        #region Helpers

        private bool ReLoadBookmarks()
        {
            TreeNode selectedNode = bookmarksTree.SelectedNode;

            if (!(selectedNode.Tag is BookmarkFolder selectedBookmarkfolder)) return false;
            _bookmarkManager.VerifyIntegrityOfBookmarkFolder(selectedBookmarkfolder);

            return true;
        }


        private void AddBooknmarkFolder()
        {
            if (bookmarksTree.SelectedNode == null)
                return;

            var inputFormData = new FormInputRow.InputFormData
            {
                GroupBoxText = "Bookmark folder name",
                LabelText = "Name:",
                WindowText = "Add new bookmark folder",
                MinimumCharacters = 1,
                MaximumCharacters = 50,
            };
            var formInputRow = new FormInputRow(inputFormData);

            if (formInputRow.ShowDialog(this) == DialogResult.OK)
            {
                string folderName = formInputRow.UserInputText;
                TreeNode selectedNode = bookmarksTree.SelectedNode;

                if (selectedNode.Tag is BookmarkFolder selectedBookmarkfolder)
                {
                    BookmarkFolder newFolder = _bookmarkManager.AddBookmarkFolder(selectedBookmarkfolder.Id, folderName);
                    AlterTreeViewState(TreeViewFolderStateChange.FolderAdded, newFolder);
                }
            }
        }

        #endregion

        #region Menu

        private void renameFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (bookmarksTree.SelectedNode?.Tag is BookmarkFolder bookmarkFolder)
            {
                var ucRenameFolder = new RenameBookmarkFolder();
                ucRenameFolder.InitControl(bookmarkFolder.Name, bookmarkFolder.Bookmarks.Count);
                var renameForm = FormFactory.CreateModalForm(ucRenameFolder);

                if (renameForm.ShowDialog(this) == DialogResult.OK)
                {
                    string newName = ucRenameFolder.GetNewFolderName();
                    bookmarkFolder.Name = newName;
                    AlterTreeViewState(TreeViewFolderStateChange.FolderRenamed, bookmarkFolder);
                }
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBooknmarkFolder();
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(bookmarksTree.SelectedNode?.Tag is BookmarkFolder treeNode))
                return;

            if (
                MessageBox.Show(this, Resources.Are_you_sure_you_want_to_delete_this_folder_, Resources.Remove_folder_,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _bookmarkManager.DeleteBookmarkFolder(treeNode);
                AlterTreeViewState(TreeViewFolderStateChange.FolderRemoved, treeNode);
            }
        }

        #endregion

        #region Buttons

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateBookmarkName())
            {
                MessageBox.Show(this, "Invalid bookmark name", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (ParentForm != null)
            {
                BookmarkFolder bookmarkFolder = bookmarksTree.SelectedNode.Tag as BookmarkFolder ?? _bookmarkManager.RootFolder;
                _bookmarkManager.AddBookmark(bookmarkFolder.Id, txtBookmarkName.Text, _imageReference);
                _bookmarkService.SaveBookmarks();

                ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            AddBooknmarkFolder();
        }

        #endregion

        #region BookmarkTreeViewEvents

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ReLoadBookmarks();
        }

        private void AlterTreeViewState(TreeViewFolderStateChange stateChange, BookmarkFolder folder)
        {
            if (stateChange == TreeViewFolderStateChange.FolderAdded)
            {
                _treeViewDataContext.ExpandNode(folder);
            }
            else if (stateChange == TreeViewFolderStateChange.FolderRenamed)
            {
                InitBookmarksDataSource();
                _treeViewDataContext.ExpandNode(folder);
            }
            else
            {
                _treeViewDataContext.ExpandNode(bookmarksTree.TopNode.Tag as BookmarkFolder);
            }
        }

        #endregion


        private bool ValidateBookmarkName()
        {
            return txtBookmarkName.Text.Length >= 1 && txtBookmarkName.Text.Length <= 500;
        }
    }
}

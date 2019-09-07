using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ImageViewer.DataBinding;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.InputForms;
using ImageViewer.Managers;
using ImageViewer.Models.Enums;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;
using Serilog;

namespace ImageViewer
{
    public partial class FormBookmarks : Form
    {
        private const int CustomContentHeight = 4;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly BookmarkManager _bookmarkManager;
        private readonly BookmarkService _bookmarkService;
        private readonly Color _gridViewGradientBackgroundColorStart = ColorTranslator.FromHtml("#b2e1ff");
        private readonly Color _gridViewGradientBackgroundColorStop = ColorTranslator.FromHtml("#66b6fc");
        private readonly Color _gridViewSelectionBorderColor = ColorTranslator.FromHtml("#7da2ce");
        private readonly OverlayFormManager _overlayFormManager;
        private Rectangle _dragBoxFromMouseDown;
        private TreeViewDataContext _treeViewDataContext;
        private object _valueFromMouseDown;


        public FormBookmarks(BookmarkService bookmarkService, BookmarkManager bookmarkManager, ApplicationSettingsService applicationSettingsService)
        {
            _bookmarkService = bookmarkService;
            _bookmarkManager = bookmarkManager;
            _applicationSettingsService = applicationSettingsService;
            _overlayFormManager = new OverlayFormManager();
            InitializeComponent();
        }

        private void FormBookmarks_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            bookmarksDataGridView.RowPrePaint += bookmarksDataGridView_RowPrePaint;
            _bookmarkManager.OnBookmarksUpdate += Instance_OnBookmarksUpdate;
            bookmarksTree.AfterSelect += BookmarksTree_AfterSelect;
            InitBookmarksDataGridViev();
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, _bookmarkManager.RootFolder);
            _overlayFormManager.HideImageDelay = 250;
            _overlayFormManager.ShowImageDelay = 500;

            var settings = _applicationSettingsService.Settings.ExtendedAppSettings;
            if (settings.BookmarksShowMaximizedImageArea)
            {
                Invoke(new EventHandler(maximizePreviewAreaToolStripMenuItem_Click));
            }

            if (settings.BookmarksShowOverlayWindow)
            {
                Invoke(new EventHandler(showOverlayPreviewToolStripMenuItem_Click));
            }

            if (_applicationSettingsService.Settings.ExtendedAppSettings.FormStateDictionary.ContainsKey(GetType().Name))
            {
                var formState = settings.FormStateDictionary[GetType().Name];
                RestoreFormState.SetFormSizeAndPosition(this, formState.Size.ToSize(), formState.Location.ToPoint(), formState.ScreenArea.ToRectangle());
            }

            if (_applicationSettingsService.Settings.PasswordProtectBookmarks)
                using (var formGetPassword = new FormGetPassword
                {
                    PasswordDerivedString = _applicationSettingsService.Settings.PasswordDerivedString
                })
                {
                    if (formGetPassword.ShowDialog() == DialogResult.OK)
                    {
                        if (!formGetPassword.PasswordVerified)
                        {
                            MessageBox.Show(this, Resources.Invalid_password_);
                            Close();
                            return;
                        }

                        if (_bookmarkService.OpenBookmarks(formGetPassword.PasswordString))
                            InitBookmarksDataSource();
                        else
                        {
                            MessageBox.Show(Resources.failed_to_decode_file_);
                            Close();
                        }
                    }
                    else
                        Close();
                }
            else
            {
                if (!_bookmarkManager.LoadedFromFile)
                {
                    if (!_bookmarkService.OpenBookmarks())
                    {
                        Log.Error("Load Bookmarks failed");
                    }
                }

                InitBookmarksDataSource();
            }

        }

        private void FormBookmarks_Shown(object sender, EventArgs e)
        {

        }

        private void FormBookmarks_FormClosing(object sender, FormClosingEventArgs e)
        {
            _applicationSettingsService.RegisterFormStateOnClose(this);
             _applicationSettingsService.SaveSettings();
        }

        private bool ReLoadBookmarks()
        {
            TreeNode selectedNode = bookmarksTree.SelectedNode;

            if (!(selectedNode.Tag is BookmarkFolder selectedBookmarkfolder)) return false;
            _bookmarkManager.VerifyIntegrityOfBookmarFolder(selectedBookmarkfolder);
            bookmarkBindingSource.DataSource = selectedBookmarkfolder.Bookmarks.OrderBy(x => x.SortOrder).ToList();

            bookmarksDataGridView.Refresh();
            bookmarksDataGridView.Update();

            return true;
        }

        private void SelectFirstRow()
        {
            try
            {
                // Select first row!
                if (bookmarksDataGridView.Rows.Count > 0)
                {
                    bookmarksDataGridView.ClearSelection();
                    bookmarksDataGridView.FirstDisplayedScrollingRowIndex = 0;

                    bookmarksDataGridView.Rows[0].Selected = true;
                    bookmarksDataGridView.CurrentCell = bookmarksDataGridView.Rows[0].Cells[0];

                    bookmarksDataGridView.Refresh();
                    LoadPreviewImageFromSelectedRow();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception thrown when trying to select the first bokmark row");
            }
        }

        private void DeleteSelectedBookmark(bool showConfirmDialog = false)
        {
            if (bookmarksDataGridView.CurrentRow == null)
            {
                return;
            }

            if (showConfirmDialog && MessageBox.Show(this, Resources.Are_you_sure_you_want_to_delete_this_bookmark_, Resources.Delete, MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;
            if (!(selectedRow.DataBoundItem is Bookmark bookmark)) return;

            int selectedIndex = bookmarksDataGridView.CurrentRow.Index;
            if (selectedIndex > 0)
            {
                bookmarksDataGridView.Rows[selectedIndex].Selected = false;
                bookmarksDataGridView.Rows[selectedIndex - 1].Selected = true;
            }
            else if (bookmarksDataGridView.Rows.Count > 1)
            {
                bookmarksDataGridView.Rows[0].Selected = true;
            }

            _bookmarkManager.DeleteBookmark(bookmark);
        }


        private void LoadImageFromSelectedRow()
        {
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;

            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;

            if (!(selectedRow?.DataBoundItem is Bookmark bookmark)) return;
            try
            {
                Process.Start(bookmark.CompletePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in LoadImageFromSelectedRow()");
            }
        }

        private void LoadPreviewImageFromSelectedRow()
        {
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;

            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;

            if (selectedRow?.DataBoundItem is Bookmark bookmark)
                pictureBoxPreview.ImageLocation = bookmark.CompletePath;
            else
            {
                ReLoadBookmarks();
            }
        }

        private void bookmarksTree_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Bookmark)) is Bookmark bookmark)
            {
                // The mouse locations are relative to the screen, so they must be 
                // converted to client coordinates.
                Point clientPoint = bookmarksTree.PointToClient(new Point(e.X, e.Y));

                // If the drag operation was a move then add the row to the other control.
                if (e.Effect == DragDropEffects.Move)
                {
                    TreeViewHitTestInfo hittest = bookmarksTree.HitTest(clientPoint.X, clientPoint.Y);
                    if (hittest.Node == null) return;
                    TreeNode dropNode = hittest.Node;
                    if (!(dropNode.Tag is BookmarkFolder dropFolder)) return;

                    _bookmarkManager.MoveBookmark(bookmark, dropFolder.Id);
                    ReLoadBookmarks();
                }
            }
        }

        private void bookmarksTree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(Bookmark)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void bookmarksTree_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bookmark)))
                e.Effect = e.AllowedEffect;
        }

        private string SelectBookmarksFile()
        {
            openFileDialog1.Filter = "(BookmarkFiles *.dat)|*.dat";
            openFileDialog1.FileName = "bookmarks.dat";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }

            return null;
        }

        private string SelectBookmarksFilePassword()
        {
            var uc = new GetPassword();
            var paswordForm = FormFactory.CreateModalForm(uc);
            paswordForm.StartPosition = FormStartPosition.CenterParent;
            paswordForm.ShowInTaskbar = false;
            paswordForm.FormBorderStyle = FormBorderStyle.FixedSingle;

            if (paswordForm.ShowDialog(this) == DialogResult.OK)
            {
                return uc.SelectedPassword;
            }

            return null;
        }

        private void contextMenuStripFolders_Opening(object sender, CancelEventArgs e)
        {
            if (bookmarksTree.SelectedNode == bookmarksTree.TopNode)
            {
                var deleteMenuItem = contextMenuStripFolders.Items.Find("deleteFolderToolStripMenuItem", false).FirstOrDefault();
                if (deleteMenuItem != null)
                {
                    deleteMenuItem.Enabled = false;
                }
            }
            else
            {
                var deleteMenuItem = contextMenuStripFolders.Items.Find("deleteFolderToolStripMenuItem", false).FirstOrDefault();
                if (deleteMenuItem != null)
                {
                    deleteMenuItem.Enabled = true;
                }
            }
        }


        private void bookmarksDataGridView_MouseEnter(object sender, EventArgs e)
        {
        }

        private async void bookmarksDataGridView_MouseLeave(object sender, EventArgs e)
        {
            await _overlayFormManager.HideFormWithDelay();
        }

        private void bookmarksDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_overlayFormManager.IsEnabled && e.RowIndex >= 0 && _overlayFormManager.ActiveRow != e.RowIndex)
            {
                _overlayFormManager.ActiveRow = e.RowIndex;
                var row = bookmarksDataGridView.Rows[e.RowIndex];
                if (row.DataBoundItem is Bookmark dataItem)
                {
                    _overlayFormManager.LoadImageAndDisplayForm(dataItem.CompletePath, Cursor.Position);
                }
            }
        }

        private void bookmarksDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void bookmarksDataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        #region DataGridViewEvents

        private void bookmarksDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            LoadPreviewImageFromSelectedRow();
        }

        private void bookmarksDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
                LoadImageFromSelectedRow();
        }

        private void bookmarksDataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStripBookmarks.Show(bookmarksDataGridView, new Point(e.X, e.Y));
        }

        private void bookmarksDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            DataGridView.HitTestInfo hittestInfo = bookmarksDataGridView.HitTest(e.X, e.Y);

            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                _valueFromMouseDown = bookmarksDataGridView.Rows[hittestInfo.RowIndex].DataBoundItem;
                if (_valueFromMouseDown != null)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    _dragBoxFromMouseDown = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void bookmarksDataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y) && _valueFromMouseDown != null)
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    bookmarksDataGridView.DoDragDrop(_valueFromMouseDown, DragDropEffects.Move);
                }
            }
        }

        private void bookmarksDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;

            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                LoadImageFromSelectedRow();
            }

            if (e.KeyData == Keys.Delete)
            {
                DeleteSelectedBookmark(true);
            }
        }

        #endregion

        #region Init

        private void InitBookmarksDataSource()
        {
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, _bookmarkManager.RootFolder);
            _treeViewDataContext.BindData();
        }

        private void InitBookmarksDataGridViev()
        {
            // Set a cell padding to provide space for the top of the focus 
            // rectangle and for the content that spans multiple columns. 
            var newPadding = new Padding(0, 1, 0, CustomContentHeight);
            bookmarksDataGridView.RowTemplate.DefaultCellStyle.Padding = newPadding;

            // Set the selection background color to transparent so 
            // the cell won't paint over the custom selection background.
            bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Transparent;

            // Set the row height to accommodate the content that 
            // spans multiple columns.
            bookmarksDataGridView.RowTemplate.Height += CustomContentHeight;

            // Initialize other DataGridView properties.
            bookmarksDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            bookmarksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Instance_OnBookmarksUpdate(object sender, BookmarkUpdatedEventArgs e)
        {
            if ((e.BookmarkAction & (BookmarkActions.CreatedBookmark | BookmarkActions.DeletedBookmark)) != 0)
            {
                ReLoadBookmarks();
            }
            else if ((e.BookmarkAction & (BookmarkActions.CreatedBookmarkFolder | BookmarkActions.DeletedBookmarkFolder)) != 0)
            {
                _treeViewDataContext.BindData();
                _treeViewDataContext.ExpandNode(_bookmarkManager.RootFolder);
            }
        }

        #endregion

        #region BookmarkTreeViewEvents

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ReLoadBookmarks();
            SelectFirstRow();
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

            _overlayFormManager.ActiveRow = -1;
        }

        #endregion

        #region bookmarksDataGridViewEvents

        // Sort the list based on clicked column
        private async void bookmarksDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var column = bookmarksDataGridView.Columns[e.ColumnIndex];
                if (column.Tag == null)
                {
                    column.Tag = SortOrder.Ascending;
                }
                else
                {
                    var currentSortOrder = (SortOrder)(int)column.Tag;
                    column.Tag = currentSortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                }

                string sortBy = column.DataPropertyName;

                TreeNode selectedNode = bookmarksTree.SelectedNode;

                if (!(selectedNode.Tag is BookmarkFolder selectedBookmarkfolder)) return;


                _bookmarkManager.UpdateSortOrder(selectedBookmarkfolder, sortBy, (SortOrder)column.Tag);
                ReLoadBookmarks();
                await _bookmarkService.SaveBookmarksAsync();
            }
        }

        // Paints the content that spans multiple columns and the focus rectangle.
        private void bookmarksDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush forebrush = null;
            try
            {
                // Determine the foreground color.
                forebrush = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected
                    ? new SolidBrush(e.InheritedRowStyle.SelectionForeColor)
                    : new SolidBrush(e.InheritedRowStyle.ForeColor);
            }
            finally
            {
                forebrush?.Dispose();
            }
        }

        private void bookmarksDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                // Do not automatically paint the focus rectangle.
                e.PaintParts &= ~DataGridViewPaintParts.Focus;

                // Determine whether the cell should be painted
                // with the custom selection background.
                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    // Calculate the bounds of the row.
                    var rowBounds = new Rectangle(0, e.RowBounds.Top,
                        bookmarksDataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                        bookmarksDataGridView.HorizontalScrollingOffset + 1,
                        e.RowBounds.Height);

                    // Paint the custom selection background.
                    using (Brush backbrush = new LinearGradientBrush(rowBounds, _gridViewGradientBackgroundColorStart, _gridViewGradientBackgroundColorStop, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                        var p = new Pen(backbrush, 1) { Color = _gridViewSelectionBorderColor };
                        e.Graphics.DrawRectangle(p, rowBounds);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception in bookmarksDataGridView_RowPrePaint()", ex);
            }
        }

        #endregion

        #region MenuEventHandlers

        private void setDefaultDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSetDefaultDrive = new FormSetDefaultDrive();
            if (formSetDefaultDrive.ShowDialog(this) == DialogResult.OK)
            {
            }
        }

        private void renameFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (bookmarksTree.SelectedNode?.Tag is BookmarkFolder bookmarkFolder)
            {
                var ucRenameFolder = new RenameBookmarkFolder();
                ucRenameFolder.InitControl(bookmarkFolder.Name, bookmarkFolder.Bookmarks.Count);
                Form renameForm = FormFactory.CreateModalForm(ucRenameFolder);

                if (renameForm.ShowDialog(this) == DialogResult.OK)
                {
                    string newName = ucRenameFolder.GetNewFolderName();
                    bookmarkFolder.Name = newName;
                    AlterTreeViewState(TreeViewFolderStateChange.FolderRenamed, bookmarkFolder);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool result = _bookmarkService.SaveBookmarks();
            if (!result)
            {
                MessageBox.Show("Unable to save bookmarks", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("Unable to save bookmarks");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(BookmarkFiles *.dat)|*.dat";
            saveFileDialog1.FileName = "bookmarks.dat";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var userControl = new SelectPassword();
            Form paswordForm = FormFactory.CreateModalForm(userControl);

            paswordForm.Controls.Add(userControl);
            paswordForm.StartPosition = FormStartPosition.CenterParent;
            paswordForm.ShowInTaskbar = false;
            paswordForm.FormBorderStyle = FormBorderStyle.FixedSingle;


            if (paswordForm.ShowDialog(this) == DialogResult.OK)
            {
                string password = userControl.SelectedPassword;

                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    _bookmarkManager.SaveToFile(filename, password);
                    MessageBox.Show("Save was sussessful", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int removedItems = _bookmarkManager.RemoveDuplicates();
            ReLoadBookmarks();
            MessageBox.Show(this, $"{removedItems} Duplicate bookmarks pointing to the same file where removed", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openAndIncludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = SelectBookmarksFile();

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            string password = SelectBookmarksFilePassword();
            if (string.IsNullOrEmpty(password))
            {
                return;
            }

            if (_bookmarkManager.LoadFromFileAndAppendBookmarks(filename, password))
            {
                _bookmarkManager.BookmarkDatasourceUpdated();
                MessageBox.Show("Bookmarksfile was loaded and appended to current bookmarks", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReLoadBookmarks();
                InitBookmarksDataSource();
            }
            else
            {
                MessageBox.Show("Unable to load file, incorrect password?", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void openAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = SelectBookmarksFile();

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            string password = SelectBookmarksFilePassword();
            if (string.IsNullOrEmpty(password))
            {
                return;
            }

            if (_bookmarkManager.LoadFromFile(filename, password))
            {
                _bookmarkManager.BookmarkDatasourceUpdated();
                ReLoadBookmarks();
                InitBookmarksDataSource();
                MessageBox.Show("Bookmarksfile was loaded succesfully", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to load file, incorrect password?", "Bookmarks", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;
            if (!(selectedRow?.DataBoundItem is Bookmark bookmark)) return;

            var editBookmark = new FormEditBookmark();
            editBookmark.InitForRename(bookmark.BoookmarkName, bookmark.FileName);
            if (editBookmark.ShowDialog(this) == DialogResult.OK)
            {
                string name = editBookmark.GetNewName();
                bookmark.BoookmarkName = name;
                ReLoadBookmarks();
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bookmarksTree.SelectedNode == null)
                return;

            var inputFormData = new FormInputRow.InputFormData
            {
                GroupBoxText = "Bookmark folder name",
                LabelText = "Name:",
                WindowText = "Add new bookmark folder",
                MinimumCharacters = 1,
                MaximumCharacters = 50
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

        private async void tryToFixBrokenLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bookmarkManager == null)
                return;

            folderBrowserDialog1.Description = "Select base directory to link from";
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog1.SelectedPath;
                int linkesFixed = await _bookmarkManager.FixBrokenLinksFromBaseDir(selectedPath);

                if (linkesFixed > 0)
                {
                    _bookmarkService.SaveBookmarks();
                }


                MessageBox.Show($"Corrected {linkesFixed} incorrect file paths", "Fix broken links", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedBookmark();
        }

        private void showOverlayPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _overlayFormManager.IsEnabled = !_overlayFormManager.IsEnabled;
            showOverlayPreviewToolStripMenuItem.Checked = _overlayFormManager.IsEnabled;
            _applicationSettingsService.Settings.ExtendedAppSettings.BookmarksShowOverlayWindow = showOverlayPreviewToolStripMenuItem.Checked;
        }

        private void maximizePreviewAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = Convert.ToInt32(splitContainer1.Width * 0.75);
            splitContainer2.SplitterDistance = Convert.ToInt32(splitContainer2.Height * 0.25);
            _applicationSettingsService.Settings.ExtendedAppSettings.BookmarksShowMaximizedImageArea = true;
        }

        private void restorePreviewAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = Convert.ToInt32(splitContainer1.Width * 0.25);
            splitContainer2.SplitterDistance = Convert.ToInt32(splitContainer2.Height * 0.5);
            _applicationSettingsService.Settings.ExtendedAppSettings.BookmarksShowMaximizedImageArea = false;
        }

        #endregion
    }
}
#region Includes

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using GeneralToolkitLib.Converters;
using ImageViewer.DataContracts.Import;
using ImageViewer.Managers;
using ImageViewer.Models.Import;
using ImageViewer.Models.UserInteraction;
using ImageViewer.Services;
using ImageViewer.Utility;
using Serilog;

#endregion

namespace ImageViewer
{
    public partial class FormAddImageSource : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;

        private readonly ImageLoaderService _imageLoaderService;

        private readonly UserInteractionService _interactionService;

        private readonly IMapper _mapper;

        private readonly NodeModelComparer _nodeModelComparer;

        private readonly List<ListViewSourceModel> _importList;

        //private const string ValidFileTypes = "*.jpg;*.jpeg;*.png;*.bmp";
        private readonly string[] ValidFileTypes = new[] { ".jpg", ".jpeg", ".png", ".bmp" };

        public FormAddImageSource(ApplicationSettingsService applicationSettingsService, IMapper mapper, ImageLoaderService imageLoaderService, UserInteractionService interactionService)
        {
            _applicationSettingsService = applicationSettingsService;
            _mapper = mapper;
            _imageLoaderService = imageLoaderService;
            _interactionService = interactionService;
            RootNodeChanged += FormAddImageSource_RootNodeChanged;
            InitializeComponent();
            _nodeModelComparer = new NodeModelComparer();
            _importList = new List<ListViewSourceModel>();
        }

        private bool Initialized { get; set; }
        private RootObjectModel RootNode { get; set; }

        private event EventHandler RootNodeChanged;

        #region Form Events

        private void FormAddImageSource_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            treeViewFileSystem.Nodes.Clear();

            // Restore previous form state
            var settings = _applicationSettingsService.Settings;
            FormStateManager.RestoreFormState(settings, this);


            // Clear ListView
            listViewSource.Columns.Clear();
            listViewSource.Groups.Clear();
            listViewSource.Items.Clear();

            listViewSource.DataBindings.Add(new Binding("Name", _importList, "ImageList"));
            listViewSource.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;


            EnumerateDrives();
            treeViewFileSystem.AfterSelect += TreeViewFileSystem_AfterSelect;
            treeViewFileSystem.AfterExpand += TreeViewFileSystem_AfterExpand;
            treeViewFileSystem.BeforeExpand += TreeViewFileSystem_BeforeExpand;

            DriveModel selectedDrive = cbDrives.SelectedItem as DriveModel;
            RootNode = RootObjectModel.CreateRootObject(selectedDrive);
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
            Initialized = true;
        }

        #endregion


        private void RecursiveNodeExpansion(ref TreeNode node, ref SourceFolderModel sourceFolder, int maxLevel)
        {
            if (node.Level >= maxLevel)
            {
                return;
            }

            foreach (var folder in sourceFolder.Folders)
            {
                var tn = CreateTreeNode(folder);
                node.Nodes.Add(tn);

                if (folder.Folders.Count > 0)
                {
                    SourceFolderModel sourceFolderModel = folder;
                    RecursiveNodeExpansion(ref tn, ref sourceFolderModel, maxLevel);
                }
            }
        }


        private TreeNode CreateTreeNode(SourceFolderModel folder)
        {
            var node = new TreeNode(folder.Name)
            {
                Tag = folder,
                ImageKey = "Folder",
                SelectedImageKey = "FolderSeletced"
            };

            return node;
        }

        private void FormAddImageSource_RootNodeChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateTreeViewWithNewRoot();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FormAddImageSource_RootNodeChanged error: ");
            }
        }

        private void UpdateTreeViewWithNewRoot()
        {
            treeViewFileSystem.Nodes.Clear();
            var treeView = treeViewFileSystem;


            var treeNode = CreateTopNode(RootNode);
            treeView.Nodes.Add(treeNode);
            treeView.TopNode = treeNode;
            treeView.TopNode.Expand();
            treeViewFileSystem.TreeViewNodeSorter = _nodeModelComparer;
            treeViewFileSystem.Sort();
        }


        private TreeNode CreateTopNode(RootObjectModel rootObject)
        {
            TreeNode[] childNodes = new TreeNode[rootObject.Folders.Count];
            int index = 0;

            foreach (var childNode in rootObject.Folders)
            {
                childNodes[index] = CreateTreeNode(childNode);
                childNodes[index].Tag = childNode;

                if (childNode.Folders.Count > 0)
                {
                    foreach (var grandChildNode in childNode.Folders)
                    {
                        TreeNode gcNode = CreateTreeNode(grandChildNode);
                        childNodes[index].Nodes.Add(gcNode);
                    }
                }

                index++;
            }

            var topNode = new TreeNode(rootObject.Name, childNodes)
            {
                Tag = rootObject.Id,
                ImageKey = "Drive",
                SelectedImageKey = "Drive"
            };


            return topNode;
        }


        private void EnumerateDrives()
        {
            var driveModelList = new List<DriveModel>();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (!drive.IsReady)
                    continue;

                driveModelList.Add(new DriveModel
                {
                    DriveName = drive.Name,
                    DriveFormat = drive.DriveFormat,
                    Removable = drive.DriveType != DriveType.Fixed,
                    RootDirectory = drive.RootDirectory,
                    TotalSize = drive.TotalSize,
                    VolumeLabel = drive.VolumeLabel
                });
            }

            cbDrives.Items.Clear();
            foreach (var driveModel in driveModelList)
            {
                cbDrives.Items.Add(driveModel);
            }

            if (cbDrives.Items.Count > 0)
                cbDrives.SelectedIndex = 0;
        }


        private void UpdateFileSystemTreeRoot()
        {
            DriveModel driveModel = cbDrives.SelectedItem as DriveModel;
            if (driveModel == null)
                return;

            RootObjectModel rootObject = RootObjectModel.CreateRootObject(driveModel);
            treeViewFileSystem.Nodes.Clear();
            RootNode = RootObjectModel.CreateRootObject(driveModel);
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
        }


        private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Initialized)
                UpdateFileSystemTreeRoot();
        }


        private void FormAddImageSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            var appSettings = _applicationSettingsService.Settings;
            FormStateManager.SaveFormState(appSettings, this);
            _applicationSettingsService.SaveSettings();
            e.Cancel = false;
        }

        #region TreeView Events

        private void TreeViewFileSystem_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Log.Information($"TreeViewFileSystem_BeforeExpand({e.Action}, {e.Node?.Name})");

            var node = e.Node;
            if (node == null)
                return;


            if (node.Nodes.Count == 0)
            {
                if (node.Tag is SourceFolderModel sourceFolder)
                {
                    RecursiveNodeExpansion(ref node, ref sourceFolder, node.Level + 2);
                    treeViewFileSystem.BeginUpdate();
                    treeViewFileSystem.Sort();
                    treeViewFileSystem.EndUpdate();
                }
                else if (node.Tag is RootObjectModel rootObject)
                {
                    foreach (SourceFolderModel folder in rootObject.Folders)
                    {
                        var tn = CreateTreeNode(folder);
                        node.Nodes.Add(tn);
                    }

                    treeViewFileSystem.BeginUpdate();
                    treeViewFileSystem.Sort();
                    treeViewFileSystem.EndUpdate();
                }
            }
        }


        // After expansion, Add next available colection of ChildNodes for each node at the new expanded level 
        private void TreeViewFileSystem_AfterExpand(object sender, TreeViewEventArgs e)
        {
            Log.Information($"TreeViewFileSystem_AfterExpand({e.Action}, {e.Node?.Name})");
        }

        // Update internal State over Source IMage Path used OnAdd
        private void TreeViewFileSystem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Log.Information($"TreeViewFileSystem_AfterSelect({e.Action}, {e.Node?.Name})");
            TreeNode node = e.Node;

            if (node == null)
            {
                return;
            }

            if (!node.IsExpanded && node.Nodes.Count == 0)
            {
                SourceFolderModel model = node.Tag as SourceFolderModel;
                RecursiveNodeExpansion(ref node, ref model, node.Level + 2);
                //node.Expand();
            }
            else
            {
                //node.Collapse(false);
            }
        }

        #endregion

        private void UpdateSelectionStats()
        {
            try
            {
                List<ImageRefModel> imageRefModels = new List<ImageRefModel>();

                foreach (var folderModel in _importList)
                {
                    imageRefModels.AddRange(getImageRefModels(folderModel));
                }

                lblImages.Text = imageRefModels.Count.ToString();
                lblRootFolders.Text = _importList.Count.ToString();
                lblFolders.Text = GetUniqueFolderCount(_importList).ToString();
                long fileSize = imageRefModels.Sum(x => x.FileSize);

                if (fileSize > 0)
                {
                    lblCombinedSize.Text = SystemIOHelper.FormatFileSizeToString(fileSize, 1);
                }

                lblDriveCount.Text = _importList.Select(x => x.FullName).ToString()?[..3].Distinct().Count().ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "UpdateSelectionStats");
            }

            btnLoad.Enabled = _importList.Count > 0;
        }

        private int GetUniqueFolderCount(ICollection<ListViewSourceModel> importList)
        {
            return importList.Count + importList.Sum(model => GetUniqueFolderCount(model.Folders));
        }

        #region Button Events

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            List<ImageRefModel> imageRefModels = new List<ImageRefModel>();

            foreach (var folderModel in _importList)
            {
                imageRefModels.AddRange(getImageRefModels(folderModel));
            }

            //Load Image Collection and close
            btnLoad.Enabled = false;
            bool result = await _imageLoaderService.RunIMageImportAsync(() => imageRefModels);
            if (!result)
            {
                _interactionService.InformUser(new UserInteractionInformation { Buttons = MessageBoxButtons.OK, Icon = MessageBoxIcon.Error, Message = "Image import failed", Label = "Error" });
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }

            btnLoad.Enabled = _importList.Count > 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!_imageLoaderService.IsRunningImport)
                Close();
        }

        private void lstBoxSourceImages_Click(object sender, EventArgs e)
        {
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void treeViewFileSystem_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuAddSource.Show(e.Location);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var node = treeViewFileSystem.SelectedNode;
                //node.Toggle();
            }
        }

        private void addFolderRecursiveMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void addFolderMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void treeViewFileSystem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hittest = treeViewFileSystem.HitTest(e.Location);
                //hittest.Node?.Toggle();
            }
        }

        #endregion

        private void treeViewFileSystem_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (e.Button == MouseButtons.Left)
            {
                if (node?.Tag is SourceFolderModel item)
                {
                    var dataModel = _mapper.Map<SourceFolderDataModel>(item);
                    treeViewFileSystem.DoDragDrop(dataModel, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        private void lstBoxSourceImages_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof(SourceFolderDataModel)))
            {
                if (e.Data?.GetData(typeof(SourceFolderDataModel)) is SourceFolderDataModel sourceFolderDataModel)
                {
                    if (_importList.Any(x => x.Id == sourceFolderDataModel.Id))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lstBoxSourceImages_DragDrop(object sender, DragEventArgs e)
        {
            var sourceFolderDataModel = e.Data?.GetData(typeof(SourceFolderDataModel));
            if (sourceFolderDataModel != null)
            {
                try
                {
                    var sourceFolder = _mapper.Map<ListViewSourceModel>(sourceFolderDataModel);

                    AddSourceFolderToListView(sourceFolder);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "DragDrop exception");
                }
            }

        }

        private void AddSourceFolderToListView(ListViewSourceModel sourceFolder)
        {
            if (_importList.All(x => x.Id != sourceFolder.Id))
            {
                EnumerateFiles(sourceFolder, ValidFileTypes);
                _importList.Add(sourceFolder);

                var listViewItem = new ListViewItem
                {
                    Name = sourceFolder.FullName,
                    Text = sourceFolder.Name,
                    ImageKey = "Folder2",
                    Tag = sourceFolder
                };

                foreach (var folder in sourceFolder.Folders)
                {
                    listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, folder.Name));
                }

                listViewSource.Items.Add(listViewItem);
                UpdateSelectionStats();
            }
        }

        private IEnumerable<ImageRefModel> getImageRefModels(ListViewSourceModel rootModel)
        {
            List<ImageRefModel> result = rootModel.ImageList;
            foreach (var folder in rootModel.Folders)
            {
                result.AddRange(folder.ImageList);
                result.AddRange(getImageRefModels(folder));
            }

            return result;
        }

        private void EnumerateFiles(ListViewSourceModel sourceFolder, string[] searchPattern, bool recursive = true)
        {
            try
            {
                sourceFolder.ImageList = new List<ImageRefModel>();
                var dirInfo = new DirectoryInfo(sourceFolder.FullName);

                var files = dirInfo
                    .GetFiles("*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => ValidFileTypes.Any(file.Extension.ToLower().Equals))
                    .ToList();

                //var files=dirInfo.GetFiles(searchPattern);

                int index = 0;
                foreach (var file in files)
                {
                    var imgRef = new ImageRefModel
                    {
                        SortOrder = index++,
                        CompletePath = file.FullName,
                        FileSize = file.Length,
                        FileName = file.Name,
                        Directory = file.DirectoryName,
                        ImageType = file.Extension,
                        LastModified = file.LastWriteTime,
                        CreationTime = file.CreationTime,
                        ParentFolderId = sourceFolder.Id,
                    };
                    sourceFolder.ImageList.Add(imgRef);
                }

                if (sourceFolder.Folders != null && recursive)
                {
                    foreach (var sourceFolderModel in sourceFolder.Folders)
                    {
                        EnumerateFiles(sourceFolderModel, searchPattern);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "EnumerateFiles exception");
            }
        }


        private void listViewSource_Click(object sender, EventArgs e)
        {

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewSource.Items.Clear();
            _importList.Clear();
            UpdateSelectionStats();
        }

        private void toolStripMenuItemRemoveItem_Click(object sender, EventArgs e)
        {
            var selected = listViewSource.SelectedItems;
            if (selected.Count > 0)
            {
                foreach (ListViewItem listViewItem in selected)
                {
                    listViewSource.Items.Remove(listViewItem);
                    _importList.RemoveAll(x => x.Id == (listViewItem.Tag as ListViewSourceModel)?.Id);
                }
                UpdateSelectionStats();
            }
        }

        private SourceFolderModel GetChildNode(string path, SourceCollectionBase rootNode)
        {
            foreach (var folder in rootNode.Folders)
            {
                if (folder.FullPath == path)
                {
                    return folder;
                }

                var subFolder = GetChildNodeRecursive(path, folder);
                if (subFolder != null)
                {
                    return subFolder;
                }

            }

            return null;
        }

        private SourceFolderModel GetChildNodeRecursive(string path, SourceCollectionBase rootNode)
        {
            foreach (var folder in rootNode.Folders)
            {
                if (folder.FullPath == path)
                {
                    return folder;
                }

                var subFolder = GetChildNodeRecursive(path, folder);

                if (subFolder != null)
                {
                    return subFolder;
                }
            }

            return null;
        }

        private void toolStripMenuItemAddFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.InitialDirectory = RootNode.RootDirectory;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                AddSourceFolder(path,false);
            }
        }

        private void AddSourceFolder(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                var node = GetChildNode(path, RootNode);

                if (node != null)
                {
                    try
                    {
                        ListViewSourceModel listViewSourceModel = _mapper.Map<ListViewSourceModel>(node);
                        EnumerateFiles(listViewSourceModel, ValidFileTypes, recursive);
                        if (!recursive)
                            listViewSourceModel.Folders.Clear();
                        AddSourceFolderToListView(listViewSourceModel);
                        UpdateSelectionStats();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "toolStripMenuItemAddFolder_Click exception");
                    }
                }
            }
        }

        private void toolStripMenuItemAddFlderRecursive_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.InitialDirectory = RootNode.RootDirectory;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                AddSourceFolder(path, true);
            }
        }
    }
}
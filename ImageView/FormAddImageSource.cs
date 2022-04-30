#region Includes

using System.Diagnostics;
using AutoMapper;
using ImageViewer.DataContracts.Import;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Models.Import;
using ImageViewer.Models.UserInteraction;
using ImageViewer.Services;
using ImageViewer.Utility;
using Serilog;

// ReSharper disable All

#endregion

namespace ImageViewer
{
    public partial class FormAddImageSource : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;

        private readonly Dictionary<string, string> _controlStateDictionary;

        private readonly ImageLoaderService _imageLoaderService;

        private readonly List<ListViewSourceModel> _importList;

        private readonly UserInteractionService _interactionService;

        private readonly IMapper _mapper;

        private readonly NodeModelComparer _nodeModelComparer;

        //private const string ValidFileTypes = "*.jpg;*.jpeg;*.png;*.bmp";
        private readonly string[] ValidFileTypes = {".jpg", ".jpeg", ".png", ".bmp", ".gif"};

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
            _controlStateDictionary = new Dictionary<string, string>();
        }

        private bool Initialized { get; set; }
        private RootObjectModel RootNode { get; set; }

        private event EventHandler RootNodeChanged;

        #region Form Events

        private void FormAddImageSource_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            //Recent Files
            recentCollectionsMenuItem.DropDownItems.Clear();

            treeViewFileSystem.Nodes.Clear();

            // Restore previous form state
            ApplicationSettingsModel settings = _applicationSettingsService.Settings;
            FormStateManager.RestoreFormState(settings, this);

            // Clear ListView


            EnumerateDrives();
            treeViewFileSystem.AfterSelect += TreeViewFileSystem_AfterSelect;
            treeViewFileSystem.AfterExpand += TreeViewFileSystem_AfterExpand;
            treeViewFileSystem.BeforeExpand += TreeViewFileSystem_BeforeExpand;

            //Additional Settings
            var additionalParameters = FormStateManager.GetAdditionalParameters(settings, this);
            if (additionalParameters != null && additionalParameters.ContainsKey("cbDrives.SelectedIndex"))
            {
                var selectedIndex = additionalParameters["cbDrives.SelectedIndex"];
                _controlStateDictionary.Add("cbDrives.SelectedIndex", selectedIndex);

                if (int.TryParse(selectedIndex, out var index))
                {
                    if (index >= 0 && index < cbDrives.Items.Count)
                    {
                        cbDrives.SelectedIndex = index;
                    }
                }
            }

            var selectedDrive = cbDrives.SelectedItem as DriveModel;
            RootNode = RootObjectModel.CreateRootObject(selectedDrive);
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
            Initialized = true;
        }

        #endregion


        private void RecursiveNodeExpansion(ref TreeNode node, ref SourceFolderModel sourceFolder, int maxLevel)
        {
            if (node.Level > maxLevel)
            {
                return;
            }

            foreach (SourceFolderModel folder in sourceFolder.Folders)
            {
                TreeNode tn = CreateTreeNode(folder);
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
            TreeView treeView = treeViewFileSystem;


            TreeNode treeNode = CreateTopNode(RootNode);
            treeView.Nodes.Add(treeNode);
            treeView.TopNode = treeNode;
            treeView.TopNode.Expand();
            treeViewFileSystem.TreeViewNodeSorter = _nodeModelComparer;
            treeViewFileSystem.Sort();
        }


        private TreeNode CreateTopNode(RootObjectModel rootObject)
        {
            var childNodes = new TreeNode[rootObject.Folders.Count];
            var index = 0;

            foreach (SourceFolderModel childNode in rootObject.Folders)
            {
                childNodes[index] = CreateTreeNode(childNode);
                childNodes[index].Tag = childNode;

                if (childNode.Folders.Count > 0)
                {
                    foreach (SourceFolderModel grandChildNode in childNode.Folders)
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
            foreach (DriveInfo drive in drives)
            {
                if (!drive.IsReady)
                {
                    continue;
                }

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
            foreach (DriveModel driveModel in driveModelList)
            {
                cbDrives.Items.Add(driveModel);
            }

            if (cbDrives.Items.Count > 0)
            {
                cbDrives.SelectedIndex = 0;
            }
        }


        private void UpdateFileSystemTreeRoot()
        {
            var driveModel = cbDrives.SelectedItem as DriveModel;
            if (driveModel == null)
            {
                return;
            }

            if (_controlStateDictionary.ContainsKey("cbDrives.SelectedIndex"))
            {
                _controlStateDictionary["cbDrives.SelectedIndex"] = cbDrives.SelectedIndex.ToString();
            }
            else
            {
                _controlStateDictionary.Add("cbDrives.SelectedIndex", cbDrives.SelectedIndex.ToString());
            }

            var rootObject = RootObjectModel.CreateRootObject(driveModel);
            treeViewFileSystem.Nodes.Clear();
            RootNode = RootObjectModel.CreateRootObject(driveModel);
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
        }


        private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Initialized)
            {
                UpdateFileSystemTreeRoot();
            }
        }


        private void FormAddImageSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationSettingsModel appSettings = _applicationSettingsService.Settings;
            FormStateManager.SaveFormState(appSettings, this);
            FormStateManager.UpdateAdditionallParameters(appSettings, this, _controlStateDictionary);
            _applicationSettingsService.SaveSettings();
            e.Cancel = false;
        }

        private void UpdateSelectionStats()
        {
            try
            {
                var imageRefModels = new List<ImageRefModel>();

                foreach (ListViewSourceModel folderModel in _importList)
                {
                    imageRefModels.AddRange(getImageRefModels(folderModel));
                }

                lblImages.Text = imageRefModels.Count.ToString();
                lblRootFolders.Text = _importList.Count.ToString();
                lblFolders.Text = GetUniqueFolderCount(_importList).ToString();
                var fileSize = imageRefModels.Sum(x => x.FileSize);

                if (fileSize > 0)
                {
                    lblCombinedSize.Text = SystemIOHelper.FormatFileSizeToString(fileSize);
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

        private void treeViewFileSystem_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var node = e.Item as TreeNode;
            if (e.Button == MouseButtons.Left)
            {
                if (node?.Tag is SourceFolderModel item)
                {
                    var dataModel = _mapper.Map<SourceFolderDataModel>(item);
                    treeViewFileSystem.DoDragDrop(dataModel, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        private void AddSourceFolderToListView(ListViewSourceModel sourceFolder, bool recursive = true)
        {
            if (_importList.All(x => x.Id != sourceFolder.Id))
            {
                ApplicationIOHelper.EnumerateFiles(sourceFolder, ValidFileTypes, recursive);
                _importList.Add(sourceFolder);


                //foreach (var folder in sourceFolder.Folders)
                //{
                //    lstBoxSourceItems.Items.Add(folder);
                //}

                //lstBoxSourceItems.Items.Add(listViewItem);
                UpdateSelectionStats();
                //lstBoxSourceItems.DataSource = _importList;
                //lstBoxSourceItems.ValueMember = "Id";
                //lstBoxSourceItems.DisplayMember = "Name";
            }
        }

        private IEnumerable<ImageRefModel> getImageRefModels(ListViewSourceModel rootModel)
        {
            var result = rootModel.ImageList;
            foreach (ListViewSourceModel folder in rootModel.Folders)
            {
                result.AddRange(folder.ImageList);
                result.AddRange(getImageRefModels(folder));
            }

            return result;
        }


        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _importList.Clear();
            //lstBoxSourceItems.DataSource=_importList;

            UpdateSelectionStats();
        }

        private void toolStripMenuItemRemoveItem_Click(object sender, EventArgs e)
        {
            TreeNode selected = treeViewSelectedFolders.SelectedNode;


            //    foreach (ListViewItem listViewItem in selected)
            //    {
            //        //lstBoxSourceItems.Items.Remove(listViewItem);
            //        _importList.RemoveAll(x => x.Id == (listViewItem.Tag as ListViewSourceModel)?.Id);
            //    }
            //    UpdateSelectionStats();
        }

        private SourceFolderModel GetChildNode(string path, SourceCollectionBase rootNode)
        {
            foreach (SourceFolderModel folder in rootNode.Folders)
            {
                if (folder.FullPath == path)
                {
                    return folder;
                }

                SourceFolderModel subFolder = GetChildNodeRecursive(path, folder);
                if (subFolder != null)
                {
                    return subFolder;
                }
            }

            return null;
        }

        private SourceFolderModel GetChildNodeRecursive(string path, SourceCollectionBase rootNode)
        {
            foreach (SourceFolderModel folder in rootNode.Folders)
            {
                if (folder.FullPath == path)
                {
                    return folder;
                }

                SourceFolderModel subFolder = GetChildNodeRecursive(path, folder);

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
                var path = folderBrowserDialog1.SelectedPath;
                AddSourceFolder(path, false);
            }
        }

        private void AddSourceFolder(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                SourceFolderModel node = GetChildNode(path, RootNode);

                if (node != null)
                {
                    try
                    {
                        var listViewSourceModel = _mapper.Map<ListViewSourceModel>(node);
                        ApplicationIOHelper.EnumerateFiles(listViewSourceModel, ValidFileTypes, recursive);
                        if (!recursive)
                        {
                            listViewSourceModel.Folders.Clear();
                        }

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
                var path = folderBrowserDialog1.SelectedPath;
                AddSourceFolder(path, true);
            }
        }

        private void toolStripMenuItemAddFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = RootNode.RootDirectory;
            openFileDialog1.RestoreDirectory = true;
            var filter = string.Join('.', ValidFileTypes.Select(x => $"*{x};").ToArray());
            filter = $"(Image Files)|{filter}|All Files (*.*)|*.*";
            openFileDialog1.Filter = filter;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                // TODO Create ListViewSourceModel, append selected files and add to collection
                for (var i = 0; i < openFileDialog1.FileNames.Length; i++)
                {
                    var fileName = openFileDialog1.FileNames[i];
                    Debug.WriteLine($"Selected file: {fileName}");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_importList.Count > 0)
            {
                saveFileDialog1.FileName = "SourceCollection.sco";
                saveFileDialog1.Filter = "SourceCollection(*.sco)|*.sco";

                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    //if (!false)
                    //{
                    //    MessageBox.Show("File operation Failed", "Unable to save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void newCollectionMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openCollectionMenuItem_Click(object sender, EventArgs e)
        {
        }


        private void treeViewSelectedFolders_DragDrop(object sender, DragEventArgs e)
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

        private void treeViewSelectedFolders_DragEnter(object sender, DragEventArgs e)
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

        private void treeViewSelectedFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }

        #region TreeView Events

        private void TreeViewFileSystem_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Log.Information($"TreeViewFileSystem_BeforeExpand({e.Action}, {e.Node?.Name})");

            TreeNode node = e.Node;
            if (node == null)
            {
                return;
            }


            if (node.Nodes.Count == 0)
            {
                if (node.Tag is SourceFolderModel sourceFolder)
                {
                    RecursiveNodeExpansion(ref node, ref sourceFolder, node.Level + 3);
                    treeViewFileSystem.BeginUpdate();
                    treeViewFileSystem.Sort();
                    treeViewFileSystem.EndUpdate();
                }
                else if (node.Tag is RootObjectModel rootObject)
                {
                    treeViewFileSystem.BeginUpdate();
                    foreach (SourceFolderModel folder in rootObject.Folders)
                    {
                        TreeNode tn = CreateTreeNode(folder);
                        node.Nodes.Add(tn);
                    }


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
                var model = node.Tag as SourceFolderModel;
                RecursiveNodeExpansion(ref node, ref model, node.Level + 2);
                //node.Expand();
            }
        }

        #endregion

        #region Button Events

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            var imageRefModels = new List<ImageRefModel>();

            foreach (ListViewSourceModel folderModel in _importList)
            {
                imageRefModels.AddRange(getImageRefModels(folderModel));
            }

            //Load Image Collection and close
            btnLoad.Enabled = false;
            var result = await _imageLoaderService.RunImageImportAsync(() => imageRefModels);
            if (!result)
            {
                _interactionService.InformUser(new UserInteractionInformation {Buttons = MessageBoxButtons.OK, Icon = MessageBoxIcon.Error, Message = "Image import failed", Label = "Error"});
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
            {
                Close();
            }
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
                TreeNode node = treeViewFileSystem.SelectedNode;
                //node.Toggle();
            }
        }

        private void addFolderRecursiveMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode folder = treeViewFileSystem.SelectedNode;

            if (folder?.Tag is SourceFolderModel model)
            {
                AddSourceFolder(model.FullPath, true);
            }
        }

        private void addFolderMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode folder = treeViewFileSystem.SelectedNode;

            if (folder?.Tag is SourceFolderModel model)
            {
                AddSourceFolder(model.FullPath, false);
            }
        }

        private void updateFolderMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode folder = treeViewFileSystem.SelectedNode;

            if (folder?.Tag is SourceFolderModel model)
            {
                model.ImageList.Clear();
                model.Folders.Clear();
                folder.Nodes.Clear();

                RecursiveNodeExpansion(ref folder, ref model, folder.Level + 2);
            }
        }

        private void treeViewFileSystem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeViewHitTestInfo hittest = treeViewFileSystem.HitTest(e.Location);
                //hittest.Node?.Toggle();
            }
        }

        #endregion
    }
}
#region Includes

using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoMapper;
using ImageViewer.DataContracts.Import;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Models.Import;
using ImageViewer.Models.UserInteraction;
using ImageViewer.Repositories;
using ImageViewer.Services;
using ImageViewer.Utility;
using Serilog;

#endregion

namespace ImageViewer
{
    public partial class FormAddImageSource : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly Dictionary<string, string> _controlStateDictionary;
        private readonly ImageCollectionRepository _imageCollectionRepository;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly UserInteractionService _interactionService;
        private readonly IMapper _mapper;
        private readonly NodeModelComparer _nodeModelComparer;
        private readonly List<OutputDirectoryModel> _outputDirList;

        //private const string ValidFileTypes = "*.jpg;*.jpeg;*.png;*.bmp";
        private readonly string[] ValidFileTypes = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        private ImageCollectionFile _imageCollectionFile;

        public FormAddImageSource(ApplicationSettingsService applicationSettingsService, IMapper mapper, ImageLoaderService imageLoaderService, UserInteractionService interactionService, ImageCollectionRepository imageCollectionRepository)
        {
            _applicationSettingsService = applicationSettingsService;
            _mapper = mapper;
            _imageLoaderService = imageLoaderService;
            _interactionService = interactionService;
            _imageCollectionRepository = imageCollectionRepository;

            RootNodeChanged += FormAddImageSource_RootNodeChanged;
            InitializeComponent();
            _nodeModelComparer = new NodeModelComparer();
            _outputDirList = new List<OutputDirectoryModel>();
            _controlStateDictionary = new Dictionary<string, string>();
            _imageCollectionFile = ImageCollectionFile.CreateNew();
        }

        private bool Initialized { get; set; }
        private RootObjectModel RootNode { get; set; }

        private event EventHandler RootNodeChanged;

        #region Form Events

        private void FormAddImageSource_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            //Recent Files
            openRecentCollectionsMenuItem.DropDownItems.Clear();
            treeViewFileSystem.Nodes.Clear();
            lblImageCount.Text = "Images:";
            lblWorkingFileName.Text = "";
            lblAsyncStateInfo.Text = "";

            // Restore previous form state
            ApplicationSettingsModel settings = _applicationSettingsService.Settings;
            FormStateManager.RestoreFormState(settings, this);

            // Clear demo objects
            treeViewImgCollection.Nodes.Clear();
            lblWorkingFileName.Text = _imageCollectionFile.FileName;

            EnumerateDrives();
            treeViewFileSystem.AfterSelect += TreeViewFileSystem_AfterSelect;
            treeViewFileSystem.AfterExpand += TreeViewFileSystem_AfterExpand;
            treeViewFileSystem.BeforeExpand += TreeViewFileSystem_BeforeExpand;

            //Additional Settings
            var additionalParameters = FormStateManager.GetAdditionalParameters(settings, this);
            if (additionalParameters != null && additionalParameters.ContainsKey("cbDrives.SelectedIndex"))
            {
                string selectedIndex = additionalParameters["cbDrives.SelectedIndex"];
                _controlStateDictionary.Add("cbDrives.SelectedIndex", selectedIndex);

                if (int.TryParse(selectedIndex, out int index))
                {
                    if (index >= 0 && index < cbDrives.Items.Count)
                        cbDrives.SelectedIndex = index;
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

        private void ExpandNode(ref TreeNode node, ref SourceFolderModel sourceFolder)
        {
            node.Nodes.Clear();
            foreach (SourceFolderModel folder in sourceFolder.Folders)
            {
                TreeNode tn = CreateTreeNode(folder);
                node.Nodes.Add(tn);

                if (folder.Folders.Count > 0)
                {
                    tn.Nodes.Clear();
                    foreach (SourceFolderModel subFolder in folder.Folders)
                    {
                        tn.Nodes.Add(CreateTreeNode(subFolder));
                    }
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
                Tag = rootObject,
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
            foreach (DriveModel driveModel in driveModelList)
            {
                cbDrives.Items.Add(driveModel);
            }

            if (cbDrives.Items.Count > 0)
                cbDrives.SelectedIndex = 0;
        }

        private void UpdateFileSystemTreeRoot()
        {
            var driveModel = cbDrives.SelectedItem as DriveModel;
            if (driveModel == null)
                return;

            if (_controlStateDictionary.ContainsKey("cbDrives.SelectedIndex"))
            {
                _controlStateDictionary["cbDrives.SelectedIndex"] = cbDrives.SelectedIndex.ToString();
            }
            else
            {
                _controlStateDictionary.Add("cbDrives.SelectedIndex", cbDrives.SelectedIndex.ToString());
            }

            //var rootObject = RootObjectModel.CreateRootObject(driveModel);
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
                var imageRefModels = GetAllImageRefModelsRecursive(_outputDirList);

                lblImages.Text = imageRefModels.Count.ToString();
                lblRootFolders.Text = _outputDirList.Count.ToString();
                lblFolders.Text = GetUniqueFolderCount(_outputDirList).ToString();
                long fileSize = imageRefModels.Sum(x => x.FileSize);

                if (fileSize > 0)
                {
                    lblCombinedSize.Text = SystemIOHelper.FormatFileSizeToString(fileSize);
                }

                lblDriveCount.Text = _outputDirList.Select(x => x.FullName).ToString()?[..3].Distinct().Count().ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "UpdateSelectionStats");
            }

            btnLoad.Enabled = _outputDirList.Count > 0;
        }

        private List<ImageRefModel> GetAllImageRefModelsRecursive(List<OutputDirectoryModel> outputDirList)
        {
            var imageRefModels = new List<ImageRefModel>();
            foreach (OutputDirectoryModel model in outputDirList)
            {
                imageRefModels.AddRange(model.ImageList);
                if (model.SubFolders.Count > 0)
                    imageRefModels.AddRange(GetAllImageRefModelsRecursive(model.SubFolders));
            }

            return imageRefModels;
        }

        private int GetUniqueFolderCount(ICollection<OutputDirectoryModel> importList)
        {
            return importList.Count + importList.Sum(model => GetUniqueFolderCount(model.SubFolders));
        }

        private void AddOutputDirectoryToTreeView(OutputDirectoryModel sourceFolder, bool recursive = true)
        {
            if (_outputDirList.All(x => x.Id != sourceFolder.Id))
            {
                ApplicationIOHelper.EnumerateFiles(ref sourceFolder, ValidFileTypes, recursive);
                _outputDirList.Add(sourceFolder);
                if (recursive)
                {
                    treeViewImgCollection.Nodes.Add(CreateTreeNodeRecursive(sourceFolder));
                }
                else
                {
                    var node = new TreeNode
                    {
                        Name = sourceFolder.FullName,
                        Text = sourceFolder.Name,
                        Tag = sourceFolder
                    };
                    treeViewImgCollection.Nodes.Add(node);
                }

                UpdateSelectionStats();
            }
        }

        private TreeNode CreateTreeNodeRecursive(OutputDirectoryModel outputDir)
        {
            var node = new TreeNode(outputDir.Name)
            {
                Name = outputDir.FullName,
                Text = outputDir.Name,
                Tag = outputDir
            };

            if (outputDir.SubFolders != null)
            {
                foreach (OutputDirectoryModel folder in outputDir.SubFolders)
                {
                    node.Nodes.Add(CreateTreeNodeRecursive(folder));
                }
            }

            return node;
        }

        private IEnumerable<ImageRefModel> GetImageRefModels(OutputDirectoryModel rootModel)
        {
            var result = rootModel.ImageList;
            foreach (OutputDirectoryModel folder in rootModel.SubFolders)
            {
                result.AddRange(folder.ImageList);
                result.AddRange(GetImageRefModels(folder));
            }

            return result;
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearCollection(false);
        }

        private void toolStripMenuItemRemoveItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedOutputDir();
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

        private void RemoveSelectedOutputDir()
        {
            TreeNode node = treeViewImgCollection.SelectedNode;
            if (node != null)
            {
                var model = (OutputDirectoryModel) node.Tag;
                TreeNode nextNode = node.NextNode;
                treeViewImgCollection.Nodes.Remove(node);

                if (_outputDirList.Count == 1 && _outputDirList[0] == model)
                {
                    _outputDirList.Clear();
                }
                else
                {
                    OutputDirectoryModel parent = model.ParentDirectory;
                    if (parent == null)
                        _outputDirList.Remove(model);
                    else
                        parent.SubFolders.Remove(model);
                }


                if (_outputDirList.Count == 0)
                    lstBoxOutputFiles.Items.Clear();

                if (nextNode != null)
                {
                    treeViewImgCollection.Select();
                    treeViewImgCollection.SelectedNode = nextNode;
                    treeViewImgCollection.Update();
                }

                UpdateSelectionStats();
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
                        var outputDirModel = _mapper.Map<OutputDirectoryModel>(node);
                        if (!recursive)
                            outputDirModel.SubFolders.Clear();

                        ApplicationIOHelper.EnumerateFiles(ref outputDirModel, ValidFileTypes, recursive);

                        AddOutputDirectoryToTreeView(outputDirModel, recursive);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "toolStripMenuItemAddFolder_Click exception");
                    }
                }
            }
        }

        private async Task<int> RemoveMissingFiles()
        {
            int fileCnt = 0;
            CancellationToken token = new CancellationToken(false);

            await Parallel.ForEachAsync(source: _outputDirList, body: async (item, cancellationToken) =>
            {
                int cnt = await Task<int>.Factory.StartNew(() => RemoveMissingItems(item, cancellationToken), cancellationToken);
                fileCnt += cnt;
            });

            return fileCnt;
        }

        private int RemoveMissingItems(OutputDirectoryModel model, CancellationToken token)
        {
            int fileCnt = 0;
            foreach (ImageRefModel imgRefModel in model.ImageList)
            {
                if (!File.Exists(imgRefModel.CompletePath))
                {
                    fileCnt++;
                    imgRefModel.MarkedForDeletion = true;
                    imgRefModel.FileSize = 0;
                }
            }

            foreach (OutputDirectoryModel subFolder in model.SubFolders)
            {
                fileCnt += RemoveMissingItems(subFolder, token);
            }

            return fileCnt;
        }

        private void ClearCollection(bool createNew)
        {
            _outputDirList.Clear();
            treeViewImgCollection.Nodes.Clear();
            lstBoxOutputFiles.Items.Clear();
            UpdateSelectionStats();
            if (createNew)
            {
                _imageCollectionFile = ImageCollectionFile.CreateNew();
                lblWorkingFileName.Text = _imageCollectionFile.FileName;
            }
        }
        #region Button Events

        private void toolStripMenuItemAddFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.InitialDirectory = RootNode.RootDirectory;
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                AddSourceFolder(path, false);
            }
        }

        private void toolStripMenuItemAddFlderRecursive_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.InitialDirectory = RootNode.RootDirectory;
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;

            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                folderBrowserDialog1.InitialDirectory = path;
                AddSourceFolder(path, true);
            }
        }

        private void toolStripMenuItemAddFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = RootNode.RootDirectory;
            openFileDialog1.RestoreDirectory = true;
            string filter = string.Join('.', ValidFileTypes.Select(x => $"*{x};").ToArray());
            filter = $"(Image Files)|{filter}|All Files (*.*)|*.*";
            openFileDialog1.Filter = filter;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                // TODO Create ListViewSourceModel, append selected files and add to collection
                for (var i = 0; i < openFileDialog1.FileNames.Length; i++)
                {
                    string fileName = openFileDialog1.FileNames[i];
                    Debug.WriteLine($"Selected file: {fileName}");
                }
            }
        }

        private void SaveImageCollectionToFile(bool selectPath)
        {
            if (!_imageCollectionFile.IsSaved || selectPath)
            {
                saveFileDialog1.Filter = "ImageViewCollection(*.ivc)|*.ivc";
                saveFileDialog1.FileName = _imageCollectionFile.FileName;
                saveFileDialog1.DefaultExt = ".ivc";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    _imageCollectionFile.FullPath = saveFileDialog1.FileName;
                    _imageCollectionFile.FileName = Path.GetFileName(saveFileDialog1.FileName);

                    SaveImageCollection();
                }
            }
            else
            {
                SaveImageCollection();
            }
        }

        private string GetFullPathFromFileName(string fileName)
        {
            if (treeViewImgCollection.SelectedNode?.Tag is OutputDirectoryModel nodeData)
            {
                return Path.Join(nodeData.FullName, fileName);
            }

            return null;
        }

        private void SaveImageCollection()
        {
            if (_outputDirList.Count > 0)
            {
                var container = new OutputDirectoryModel
                {
                    Name = "RootContainer",
                    Id = Guid.NewGuid().ToString(),
                    FullName = _imageCollectionFile.FullPath,
                    SubFolders = _outputDirList
                };
                bool result = _imageCollectionRepository.SaveOutputDirectoryModel(_imageCollectionFile.FullPath, container);
                if (result)
                {
                    _imageCollectionFile.IsSaved = true;
                    _imageCollectionFile.IsChanged = false;
                    lblWorkingFileName.Text = _imageCollectionFile.FileName;
                    MessageBox.Show("Save was successful", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Could not save file", "File save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImageCollectionToFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveImageCollectionToFile(true);
        }

        private void newCollectionMenuItem_Click(object sender, EventArgs e)
        {
            ClearCollection(true);
        }



        private void openCollectionMenuItem_Click(object sender, EventArgs e)
        {
            if (_imageCollectionFile.IsChanged && !_imageCollectionFile.IsSaved && _outputDirList.Count > 0)
            {
                if (MessageBox.Show("Do you want to save the current file before opening a new?", "Save current?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SaveImageCollectionToFile(string.IsNullOrEmpty(_imageCollectionFile.FullPath));
                }
            }

            openFileDialog1.Filter = "ImageViewCollection(*.ivc)|*.ivc";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                _imageCollectionFile.FullPath = openFileDialog1.FileName;
                _imageCollectionFile.FileName = Path.GetFileName(_imageCollectionFile.FullPath);
                lblWorkingFileName.Text = _imageCollectionFile.FileName;

                OutputDirectoryModel outputDirRootModel = _imageCollectionRepository.LoadImageCollection(_imageCollectionFile.FullPath);

                if (outputDirRootModel != null)
                {
                    _imageCollectionFile.IsChanged = false;
                    _imageCollectionFile.IsSaved = true;

                    _outputDirList.Clear();
                    lstBoxOutputFiles.Items.Clear();
                    treeViewImgCollection.Nodes.Clear();
                    _outputDirList.AddRange(outputDirRootModel.SubFolders);

                    //Reload treeViewImgCollection from data collection
                    foreach (OutputDirectoryModel model in _outputDirList)
                    {
                        treeViewImgCollection.Nodes.Add(CreateTreeNodeRecursive(model));
                    }

                    UpdateSelectionStats();
                }
                else
                {
                    MessageBox.Show("Failed to open the selected file", "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion



        #region TreeView Events
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

        private void treeViewImgCollection_DragDrop(object sender, DragEventArgs e)
        {
            object sourceFolderDataModel = e.Data?.GetData(typeof(SourceFolderDataModel));
            if (sourceFolderDataModel != null)
            {
                try
                {
                    var sourceFolder = _mapper.Map<OutputDirectoryModel>(sourceFolderDataModel);

                    AddOutputDirectoryToTreeView(sourceFolder);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "DragDrop exception");
                }
            }
        }

        private void treeViewImgCollection_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof(SourceFolderDataModel)))
            {
                if (e.Data?.GetData(typeof(SourceFolderDataModel)) is SourceFolderDataModel sourceFolderDataModel)
                {
                    if (_outputDirList.Any(x => x.Id == sourceFolderDataModel.Id || x.ParentDirectory?.FullName == sourceFolderDataModel.FullName) ||
                        _outputDirList.Any(x => x.FullName == sourceFolderDataModel.FullName))
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

        private void treeViewImgCollection_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                var model = (OutputDirectoryModel) node.Tag;
                lstBoxOutputFiles.BeginUpdate();
                lstBoxOutputFiles.Items.Clear();

                foreach (ImageRefModel imageRefModel in model.ImageList)
                {
                    lstBoxOutputFiles.Items.Add($"{imageRefModel.FileName}");
                }

                lstBoxOutputFiles.EndUpdate();
                lblImageCount.Text = $"Images: {model.ImageList.Count.ToString()}";
            }
        }

        private void treeViewImgCollection_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && treeViewImgCollection.SelectedNode != null)
            {
                RemoveSelectedOutputDir();
            }
        }

        private void TreeViewFileSystem_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Log.Information("TreeViewFileSystem_BeforeExpand({Action}, {Name})", e.Action, e.Node?.Name);

            TreeNode node = e.Node;
            if (node == null)
                return;


            switch (node.Tag)
            {
                case SourceFolderModel sourceFolder:
                    ExpandNode(ref node, ref sourceFolder);
                    break;
                case RootObjectModel rootObject when node.Nodes.Count == 0:
                    {
                        treeViewFileSystem.BeginUpdate();
                        foreach (SourceFolderModel folder in rootObject.Folders)
                        {
                            TreeNode tn = CreateTreeNode(folder);
                            node.Nodes.Add(tn);
                        }


                        treeViewFileSystem.Sort();
                        treeViewFileSystem.EndUpdate();
                        break;
                    }
            }

        }

        // After expansion, Add next available colection of ChildNodes for each node at the new expanded level 
        private void TreeViewFileSystem_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //Log.Information($"TreeViewFileSystem_AfterExpand({e.Action}, {e.Node?.Name})");
        }

        // Update internal State over Source IMage Path used OnAdd
        private void TreeViewFileSystem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Log.Information($"TreeViewFileSystem_AfterSelect({e.Action}, {e.Node?.Name})");
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

        private void treeViewImgCollection_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }


        private async void btnLoad_Click(object sender, EventArgs e)
        {
            var imageRefModels = new List<ImageRefModel>();

            foreach (OutputDirectoryModel folderModel in _outputDirList)
            {
                imageRefModels.AddRange(GetImageRefModels(folderModel));
            }

            //Load Image Collection and close
            btnLoad.Enabled = false;
            bool result = await _imageLoaderService.RunImageImportAsync(() => imageRefModels);
            if (!result)
            {
                _interactionService.InformUser(new UserInteractionInformation { Buttons = MessageBoxButtons.OK, Icon = MessageBoxIcon.Error, Message = "Image import failed", Label = "Error" });
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }

            btnLoad.Enabled = _outputDirList.Count > 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!_imageLoaderService.IsRunningImport)
                Close();
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

        private void lstBoxOutputFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lstBoxOutputFiles.SelectedItem is string fileName)
            {
                var fullPath = GetFullPathFromFileName(fileName);
                ApplicationIOHelper.OpenImageInDefaultAplication(fullPath);
            }
        }

        private async void removeMissingFilesMenuItem_Click(object sender, EventArgs e)
        {
            removeMissingFilesMenuItem.Enabled = false;
            lblAsyncStateInfo.Text = "Awaiting background task";
            int removedItems = await RemoveMissingFiles();

            if (removedItems > 0)
            {
                await CleanupResultCollection();
                UpdateSelectionStats();
                lblAsyncStateInfo.Text = "Task Completed";
                MessageBox.Show($"Removed {removedItems} files from the collection.", "Found missing files", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"No missing files where found.", "No missing files found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            lblAsyncStateInfo.Text = "";
            removeMissingFilesMenuItem.Enabled = true;
        }

        private async Task CleanupResultCollection()
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (var model in _outputDirList)
                {
                    var toBeRemoved = model.ImageList.Where(x => x.MarkedForDeletion).ToList();
                    foreach (var item in toBeRemoved)
                        model.ImageList.Remove(item);

                    if (model.SubFolders.Count > 0)
                    {
                        RecursiveCleanupResultCollection(model.SubFolders);
                    }
                }

            });
        }

        private void RecursiveCleanupResultCollection(IEnumerable<OutputDirectoryModel> subFolders)
        {
            foreach (var model in subFolders)
            {
                var toBeRemoved = model.ImageList.Where(x => x.MarkedForDeletion).ToList();
                foreach (var item in toBeRemoved)
                    model.ImageList.Remove(item);

                if (model.SubFolders.Count > 0)
                    RecursiveCleanupResultCollection(model.SubFolders);
            }
        }
    }
}
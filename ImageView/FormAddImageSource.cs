using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AutoMapper;
using ImageViewer.Managers;
using ImageViewer.Models.Import;
using ImageViewer.Services;
using Log = Serilog.Log;

namespace ImageViewer
{
    public partial class FormAddImageSource : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private bool Initialized { get; set; }
        private RootObjectModel RootNode { get; set; }

        private event EventHandler RootNodeChanged;

        private readonly IMapper _mapper;

        private readonly NodeModelComparer _nodeModelComparer;

        public FormAddImageSource(ApplicationSettingsService applicationSettingsService, IMapper mapper)
        {
            _applicationSettingsService = applicationSettingsService;
            _mapper = mapper;
            RootNodeChanged += FormAddImageSource_RootNodeChanged;
            InitializeComponent();
            _nodeModelComparer = new NodeModelComparer();

        }

        private void FormAddImageSource_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            treeViewFileSystem.Nodes.Clear();

            // Restore previous form state
            var settings = _applicationSettingsService.Settings;
            FormStateManager.RestoreFormState(settings, this);

            EnumerateDrives();
            treeViewFileSystem.AfterSelect += TreeViewFileSystem_AfterSelect;
            treeViewFileSystem.AfterExpand += TreeViewFileSystem_AfterExpand;
            treeViewFileSystem.BeforeExpand += TreeViewFileSystem_BeforeExpand;

            DriveModel selectedDrive = cbDrives.SelectedItem as DriveModel;
            RootNode = RootObjectModel.CreateRootObject(selectedDrive);
            RootNodeChanged?.Invoke(this, EventArgs.Empty);
            Initialized = true;
        }

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
                    RecursiveNodeExpansion(ref node, ref sourceFolder, node.Level + 5);
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

        private void RecursiveNodeExpansion(ref TreeNode node, ref SourceFolderModel sourceFolder, int maxLevel){
            if (node.Level >=maxLevel)
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

        // Update internal State over Source IMage Path used OnAdd
        private void TreeViewFileSystem_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Log.Information($"TreeViewFileSystem_AfterSelect({e.Action}, {e.Node?.Name})");
            TreeNode node = e.Node;

            if (node == null)
            {
                return;
            }

            if (!node.IsExpanded && node.Nodes.Count==0)
            {
                SourceFolderModel model=node.Tag as SourceFolderModel;
                RecursiveNodeExpansion(ref node, ref model, node.Level + 5);
                //node.Expand();
            }
            else
            {
                //node.Collapse(false);
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
            //treeView.TopNode.NextVisibleNode?.Expand();
            treeViewFileSystem.TreeViewNodeSorter = _nodeModelComparer;
            treeViewFileSystem.Sort();

            //treeView.SelectedNode = treeNode;
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
                SelectedImageKey = "Drive",
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
                    VolumeLabel = drive.VolumeLabel,
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


        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Initialized)
                UpdateFileSystemTreeRoot();
        }

        private void lstBoxSourceImages_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormAddImageSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            var appSettings = _applicationSettingsService.Settings;
            FormStateManager.SaveFormState(appSettings, this);
            _applicationSettingsService.SaveSettings();
            e.Cancel = false;
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
    }

}

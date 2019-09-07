using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.Utility;

namespace ImageViewer
{
    public partial class FileBrowser : Form
    {
        private string _selectedPath;
        private bool _enableLoadFormOnEnterKey = true;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly ILifetimeScope _scope;

        public FileBrowser(ApplicationSettingsService applicationSettingsService, ImageLoaderService imageLoaderService, Autofac.ILifetimeScope scope)
        {
            _applicationSettingsService = applicationSettingsService;
            _applicationSettingsService.LoadSettings();
            _imageLoaderService = imageLoaderService;
            _scope = scope;
            InitializeComponent();
        }

        public string SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                if (value == null || !File.Exists(value))
                    _selectedPath = value;
            }
        }

        public AutoCompleteStringCollection PathCollection { get; } = new AutoCompleteStringCollection();

        private void FileBrowser_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            dataGridViewLoadedImages.DataBindingComplete += dataGridViewLoadedImages_DataBindingComplete;
            var lastUsedSearchPathsList = new List<string>();
            var searchDirsFromSettings = _applicationSettingsService.Settings.LastUsedSearchPaths;
            bool validList = true;

            // Validate every directory and check for duplicates. 
            // If any changes has been made then save settings.
            foreach (string searchPath in searchDirsFromSettings)
            {
                string pathToAdd = searchPath.Trim();
                if (lastUsedSearchPathsList.Contains(pathToAdd))
                {
                    validList = false;
                    continue;
                }

                if (Directory.Exists(pathToAdd))
                    lastUsedSearchPathsList.Add(pathToAdd);
                else
                {
                    validList = false;
                }
            }

            if (!validList)
            {
                _applicationSettingsService.Settings.LastUsedSearchPaths = lastUsedSearchPathsList;
                _applicationSettingsService.SaveSettings();
            }

            if (lastUsedSearchPathsList.Count > 0)
            {
                PathCollection.AddRange(lastUsedSearchPathsList.ToArray());
                txtBaseDirectory.AutoCompleteCustomSource = PathCollection;
                txtBaseDirectory.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }

            txtBaseDirectory.CausesValidation = true;
            txtBaseDirectory.Validating += txtBaseDirectory_Validating;
        }

        private void txtBaseDirectory_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBaseDirectory.Text))
            {
                SelectedPath = null;
                return;
            }
            if (!Directory.Exists(txtBaseDirectory.Text))
            {
                txtBaseDirectory.Text = SelectedPath;
                e.Cancel = true;
            }

            SelectedPath = txtBaseDirectory.Text;
        }

        private void FileBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePathListToSettings();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (txtBaseDirectory.Text.Length > 0 && Directory.Exists(txtBaseDirectory.Text))
                folderBrowserDialog1.SelectedPath = txtBaseDirectory.Text;

            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtBaseDirectory.Text = folderBrowserDialog1.SelectedPath;
                SelectedPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnOpenImporter_Click(object sender, EventArgs e)
        {
            OpenImporterForm(true);
        }

        private void OpenImporterForm(bool showErrors)
        {
            if (!Directory.Exists(SelectedPath))
            {
                if (showErrors)
                    MessageBox.Show(Resources.FileBrowser_OpenImporterForm_No_valid_path_selected);
                return;
            }
            if (!PathCollection.Contains(SelectedPath))
                PathCollection.Add(SelectedPath);

            FormLoad formLoad = _scope.Resolve<FormLoad>();
            formLoad.SetBasePath(SelectedPath);
            formLoad.ShowDialog(this);
            formLoad.Dispose();

            if (_applicationSettingsService.Settings.EnableAutoLoadFunctionFromMenu)
            {
                _applicationSettingsService.Settings.LastFolderLocation = SelectedPath;
                _applicationSettingsService.SaveSettings();
            }

            if (_imageLoaderService.ImageReferenceList != null)
                dataGridViewLoadedImages.DataSource = GetSortableBindingSource();
            DelayOperation.DelayAction(delegate { _enableLoadFormOnEnterKey = true; }, 2000);
        }

        private SortableBindingList<ImageReferenceElement> GetSortableBindingSource()
        {
            return new SortableBindingList<ImageReferenceElement>(_imageLoaderService.ImageReferenceList);
        }

        private void dataGridViewLoadedImages_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lblImagesLoaded.Text = dataGridViewLoadedImages.Rows.Count.ToString();
        }

        private void grpImportSection_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void grpImportSection_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            if (_imageLoaderService.ImageReferenceList != null)
                dataGridViewLoadedImages.DataSource = GetSortableBindingSource();
        }

        private void deleteSelectedFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewLoadedImages.SelectedRows.Count == 0) return;
            if (
                MessageBox.Show(Resources.Are_you_sure_that_you_want_to_delete_the_selected_files_,
                    Resources.Confirm_delete, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataGridViewSelectedRowCollection selectedRows = dataGridViewLoadedImages.SelectedRows;
                foreach (DataGridViewRow row in selectedRows)
                {
                    var imgRefElement = row.DataBoundItem as ImageReferenceElement;
                    if (imgRefElement != null)
                        _imageLoaderService.PermanentlyRemoveFile(imgRefElement);
                }
                dataGridViewLoadedImages.DataSource = GetSortableBindingSource();
            }
        }

        private void copyFilepathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewLoadedImages.SelectedRows.Count == 0) return;
            var imgRefElement = dataGridViewLoadedImages.SelectedRows[0].DataBoundItem as ImageReferenceElement;
            if (imgRefElement != null)
            {
                Clipboard.Clear();
                Clipboard.SetText(imgRefElement.Directory);
            }
        }

        private void openWithDefaultApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imgRefElement = dataGridViewLoadedImages.SelectedRows[0].DataBoundItem as ImageReferenceElement;
            if (imgRefElement != null)
                Process.Start(imgRefElement.CompletePath);
        }

        private void FileListMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewLoadedImages.SelectedRows.Count == 0)
                e.Cancel = true;
            else if (dataGridViewLoadedImages.SelectedRows.Count > 1)
            {
                copyFilepathToolStripMenuItem.Visible = false;
                openWithDefaultApplicationToolStripMenuItem.Visible = false;
            }
            else
                foreach (ToolStripItem menuItem in FileListMenuStrip.Items)
                    menuItem.Visible = true;
        }

        private void SavePathListToSettings()
        {
            var lastUsedSearchPaths = PathCollection.Cast<string>().ToList();
            _applicationSettingsService.Settings.LastUsedSearchPaths = lastUsedSearchPaths;
            _applicationSettingsService.SaveSettings();
        }

        private void txtBaseDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _enableLoadFormOnEnterKey)
            {
                _enableLoadFormOnEnterKey = false;
                SelectedPath = txtBaseDirectory.Text;
                OpenImporterForm(false);
                e.Handled = true;
            }
        }
    }
}
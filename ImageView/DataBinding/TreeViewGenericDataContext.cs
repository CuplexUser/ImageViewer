using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImageViewer.DataBinding
{
    public class TreeViewGenericDataContext<T> where T : class , IExpandableNode
    {
        private readonly TreeView _treeView;
        private readonly T _rootFolder;

        public TreeViewGenericDataContext(TreeView treeView, T rootFolder)
        {
            _treeView = treeView;
            _rootFolder = rootFolder;

            treeView.AfterCheck += TreeView_AfterCheck;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.AfterCollapse += TreeView_AfterCollapse;
            treeView.AfterExpand += TreeView_AfterExpand;
            treeView.ControlAdded += TreeView_ControlAdded;
        }

        private void TreeView_ControlAdded(object? sender, ControlEventArgs e)
        {

        }

        private void TreeView_AfterExpand(object? sender, TreeViewEventArgs e)
        {
            e.Node?.Expand();
        }

        private void TreeView_AfterCollapse(object? sender, TreeViewEventArgs e)
        {

        }

        private void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            e.Node?.Expand();
        }

        private void TreeView_AfterCheck(object? sender, TreeViewEventArgs e)
        {
            e.Node?.Expand();
        }

        public void ExpandNode(T folderToExpand)
        {
            if (folderToExpand == null)
                return;

            BindData();
            ExpandNode(_treeView.Nodes, folderToExpand);
        }

        private void ExpandNode(TreeNodeCollection treeNodeCollection, T rootFolder)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (treeNode.Tag == rootFolder)
                {
                    _treeView.SelectedNode = treeNode;
                    treeNode.Expand();
                    return;
                }

                if (treeNode.Nodes.Count > 0)
                    ExpandNode(treeNode.Nodes, rootFolder);
            }
        }

        public void BindData()
        {
            _treeView.Nodes.Clear();
            var rootTreeNode = new TreeNode("Subfolders") { Tag = _rootFolder };
            rootTreeNode.Nodes.AddRange(RecursiveAddTreeNodes(_rootFolder).ToArray());
            _treeView.Nodes.Add(rootTreeNode);
        }

        private List<TreeNode> RecursiveAddTreeNodes(T rootFolder)
        {
            var treeNodeList = new List<TreeNode>();

            foreach (var folderNode in rootFolder.GetChildNodes().OrderBy(x => x.SortOrder))
            {
                var folder = (T) folderNode;
                var treeView = new TreeNode(folder.Name) { Tag = folder };
                treeNodeList.Add(treeView);

                if (folder.GetChildNodes() != null && (folder.GetChildNodes().Any()))
                    treeView.Nodes.AddRange(RecursiveAddTreeNodes(folder).ToArray());
            }


            return treeNodeList;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ImageViewer.DataContracts;

namespace ImageViewer.DataBinding
{
    public class TreeViewDataContext
    {
        private readonly BookmarkFolder _rootFolder;
        private readonly TreeView _treeView;


        public TreeViewDataContext(TreeView treeView, BookmarkFolder rootFolder)
        {
            _treeView = treeView;
            _rootFolder = rootFolder;

            treeView.AfterCheck += TreeView_AfterCheck;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.AfterCollapse += TreeView_AfterCollapse;
            treeView.AfterExpand += TreeView_AfterExpand;
            treeView.ControlAdded += TreeView_ControlAdded;
        }

        private void TreeView_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        private void TreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();
        }

        public void ExpandNode(BookmarkFolder folderToExpand)
        {
            if (folderToExpand == null)
                return;

            BindData();
            ExpandNode(_treeView.Nodes, folderToExpand);
        }

        private void ExpandNode(TreeNodeCollection treeNodeCollection, BookmarkFolder rootFolder)
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
            var rootTreeNode = new TreeNode("Bookmarks") {Tag = _rootFolder};
            rootTreeNode.Nodes.AddRange(RecursiveAddTreeNodes(_rootFolder).ToArray());
            _treeView.Nodes.Add(rootTreeNode);
        }

        private List<TreeNode> RecursiveAddTreeNodes(BookmarkFolder rootFolder)
        {
            var treeNodeList = new List<TreeNode>();

            foreach (BookmarkFolder folder in rootFolder.BookmarkFolders.OrderBy(x => x.SortOrder))
            {
                var treeView = new TreeNode(folder.Name) {Tag = folder};
                treeNodeList.Add(treeView);

                if (folder.BookmarkFolders!=null && folder.BookmarkFolders.Count > 0)
                    treeView.Nodes.AddRange(RecursiveAddTreeNodes(folder).ToArray());
            }


            return treeNodeList;
        }
    }
}
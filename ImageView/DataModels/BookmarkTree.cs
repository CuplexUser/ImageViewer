using System;
using System.Collections.Generic;

namespace ImageView.DataModels
{
    [Serializable]
    public class BookmarkTree
    {
        protected BookmarkTree()
        {
        }

        public List<BookmarkTreeNode> BookmarksFolders { get; protected set; }
        public List<Bookmark> RootBookmarks { get; protected set; }

        public static BookmarkTree CreateEmpty()
        {
            return new BookmarkTree {BookmarksFolders = new List<BookmarkTreeNode>(), RootBookmarks = new List<Bookmark>()};
        }
    }

    [Serializable]
    public class BookmarkTreeNode
    {
        public BookmarkTreeNode()
        {
            ChildNodes = new List<BookmarkTreeNode>();
        }

        public List<BookmarkTreeNode> ChildNodes { get; protected set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<Bookmark> Bookmarks { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
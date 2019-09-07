using System;
using System.Collections.Generic;

namespace ImageView.DataModels
{
    [Serializable]
    public class BookmarkTree
    {
        protected BookmarkTree()
        {
            RootNode= new BookmarkTreeNode(null);
        }

        public BookmarkTreeNode RootNode { get; set; }  

        public static BookmarkTree CreateEmpty()
        {
            BookmarkTree bookmarkTree = new BookmarkTree {RootNode = new BookmarkTreeNode(null) {SortOrder = 0}};

            return bookmarkTree;
        }
    }

    [Serializable]
    public class BookmarkTreeNode
    {
        public BookmarkTreeNode(BookmarkTreeNode parentBookmark)
        {
            ParentBookmark = parentBookmark;
            BookmarkFolders = new List<BookmarkTreeNode>();
            Bookmarks= new List<Bookmark>();
        }

        public List<BookmarkTreeNode> BookmarkFolders { get; private set; }
        public BookmarkTreeNode ParentBookmark { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<Bookmark> Bookmarks { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace ImageViewer.Models.Import
{
    public class NodeModelComparer : IComparer, IComparer<SourceFolderModel>
    {
        public enum CompareMode
        {
            ByName,
            BySortOrder
        }

        public NodeModelComparer()
        {
            Mode = CompareMode.ByName;
        }

        public CompareMode Mode { get; set; }

        public int Compare(object x, object y)
        {
            if (x is SourceFolderModel model1 && y is SourceFolderModel model2)
            {
                if (Mode == CompareMode.ByName)
                    return string.Compare(model1.Name, model2.Name, StringComparison.Ordinal);
                else
                    return model1.SortOrder.CompareTo(model2.SortOrder);
            }

            return 0;
        }

        public int Compare(SourceFolderModel x, SourceFolderModel y)
        {
            if ((x == null) || (y == null))
            {
                return 0;
            }

            if (Mode == CompareMode.ByName)
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            else
                return x.SortOrder.CompareTo(y.SortOrder);
        }
    }
}
namespace ImageViewer.DataBinding;

public interface IExpandableNode
{
    string Id { get; }
    string Name { get; }
    int SortOrder { get; set; }

    string FullName { get; }
    string ParentFolderId { get; set; }

    IEnumerable<IExpandableNode> GetChildNodes();
}
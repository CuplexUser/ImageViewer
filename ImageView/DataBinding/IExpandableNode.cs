namespace ImageViewer.DataBinding
{
    public interface IExpandableNode
    {
        string Id { get; }
        string Name { get; }
        int SortOrder { get; set; }

        IEnumerable<IExpandableNode> GetChildNodes();

        string FullName { get; }
        string ParentFolderId { get; set; }
    }
}
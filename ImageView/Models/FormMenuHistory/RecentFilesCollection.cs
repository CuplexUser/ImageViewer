using ImageViewer.DataContracts;
using ImageViewer.Library.CustomAttributes;

namespace ImageViewer.Models.FormMenuHistory;

public class RecentFilesCollection
{
    private readonly ItemCountMinMax _countMinMax;
    private int _maxNoItems;

    public RecentFilesCollection()
    {
        Id = Guid.NewGuid();
        RecentFiles = new List<RecentFileModel>();
        _countMinMax = new ItemCountMinMax(1, 20);
    }

    public RecentFilesCollection(ApplicationSettingsModel settings)
    {
        RecentFiles = new List<RecentFileModel>();
        _countMinMax = new ItemCountMinMax(1, 20);
        Id = Guid.NewGuid();

        if (settings == null)
        {
            RecentFiles = new List<RecentFileModel>();
            return;
        }

        if (settings.RecentFilesCollection != null)
        {
            RecentFiles.Clear();
            foreach (var file in settings.RecentFilesCollection.RecentFiles) RecentFiles.Add(file.Clone() as RecentFileModel);
        }
    }

    public List<RecentFileModel> RecentFiles { get; }

    [ValidRange(1, 20)]
    public int MaxNoItems
    {
        get => _maxNoItems;
        set
        {
            if (value > _countMinMax.Max || value < _countMinMax.Min)
            {
                return;
            }

            _maxNoItems = value;
        }
    }

    public Guid Id { get; }

    public string OwnerFormName { get; set; }

    public int Count => RecentFiles.Count;

    public void ClearRecentFiles()
    {
        RecentFiles.Clear();
    }

    public void AddRecentFile(RecentFileModel model)
    {
        if (RecentFiles.Count >= MaxNoItems)
        {
            RemoveOldestItem();
        }

        RecentFiles.Add(model);
    }

    private void RemoveOldestItem()
    {
        if (RecentFiles.Count > 0)
        {
            var item = RecentFiles.OrderBy(x => x.CreatedDate).Last();
            RecentFiles.Remove(item);
        }
    }


    public static void CreateMapping(IProfileExpression expression)
    {
        expression.CreateMap<RecentFilesCollection, RecentFilesCollectionDataModel>()
            .ForMember(s => s.MaxNoItems, o => o.MapFrom(d => d.MaxNoItems))
            .ForMember(s => s.RecentFiles, o => o.MapFrom(d => d.RecentFiles))
            .ReverseMap()
            .ForMember(s => s.Count, o => o.Ignore());
    }

    private record ItemCountMinMax(int Min, int Max);
}
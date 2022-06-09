using AutoMapper;
using ImageViewer.DataContracts;
using ImageViewer.Library.CustomAttributes;

namespace ImageViewer.Models.FormMenuHistory
{
    public class RecentFilesCollection
    {
        private int _maxNoItems;
        private record ItemCountMinMax(int Min, int Max);

        private readonly ItemCountMinMax _countMinMax;

        public List<RecentFileModel> RecentFiles { get; private set; }

        [ValidRange(1, 20)]
        public int MaxNoItems
        {
            get => _maxNoItems;
            set
            {
                if (value > _countMinMax.Max || value < _countMinMax.Min)
                    return;

                _maxNoItems = value;
            }
        }

        public int Count => RecentFiles.Count;

        public RecentFilesCollection(ApplicationSettingsModel settings)
        {
            RecentFiles = new List<RecentFileModel>();
            _countMinMax = new ItemCountMinMax(1, 20);

            if (settings == null)
            {
                RecentFiles = new List<RecentFileModel>();
                return;
            }

            if (settings.RecentFilesCollection != null)
            {
                RecentFiles.Clear();
                foreach (var file in settings.RecentFilesCollection.RecentFiles)
                {
                    RecentFiles.Add(file.Clone() as RecentFileModel);
                }
            }

        }

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
    }
}
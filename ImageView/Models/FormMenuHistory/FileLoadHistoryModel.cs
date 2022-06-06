using System.Configuration;
using System.Runtime.CompilerServices;
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

                _maxNoItems = value; }
        }

        public int Count => RecentFiles.Count;

        public RecentFilesCollection()
        {
            RecentFiles = new List<RecentFileModel>();
            var memberInfo = typeof(RecentFilesCollection).GetProperty("MaxNoItems");
            if (memberInfo != null)
            {
                var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(ValidRangeAttribute));
                _countMinMax = new ItemCountMinMax(Min: (attribute as ValidRangeAttribute).MinValue, (attribute as ValidRangeAttribute).MaxValue);
            }
            else
            {
                _countMinMax = new ItemCountMinMax(1, 20);
            }
            



            
   
        }


    }

    public class RecentFileModel
    {
        public RecentFileModel()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; init; }

        public int SortOrder { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
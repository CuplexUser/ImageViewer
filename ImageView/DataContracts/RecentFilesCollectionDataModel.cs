using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    [DataContract(Name = "RecentFilesCollectionDataModel", Namespace = "ImageViewer.DataContracts")]
    public class RecentFilesCollectionDataModel
    {
        [DataMember(Name = "MaxNoItems", Order = 1)]
        public int MaxNoItems { get; set; }

        [DataMember(Name = "RecentFiles", Order = 2)]
        public List<RecentFileDataModel> RecentFiles { get; set; }
    }
}
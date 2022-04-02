using System.IO;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts.Import
{
    /// <summary>
    /// DriveModel
    /// </summary>
    [DataContract(Name = "DriveDataModel")]
    public class DriveDataModel
    {
        /// <summary>
        /// Gets or sets the name of the drive.
        /// </summary>
        /// <value>
        /// The name of the drive.
        /// </value>
        [DataMember(Name = "DriveName")]
        public string DriveName { get; set; }
        /// <summary>
        /// Gets or sets the drive format.
        /// </summary>
        /// <value>
        /// The drive format.
        /// </value>
        [DataMember(Name = "DriveFormat")]
        public string DriveFormat { get; set; }
        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        /// <value>
        /// The root directory.
        /// </value>
        [DataMember(Name = "RootDirectory")]
        public DirectoryInfo RootDirectory { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DriveDataModel"/> is removable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if removable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "Removable")]
        public bool Removable { get; set; }

        /// <summary>
        /// Gets or sets the total size.
        /// </summary>
        /// <value>
        /// The total size.
        /// </value>
        [DataMember(Name = "TotalSize")]
        public long TotalSize { get; set; }

        /// <summary>
        /// Gets or sets the volume label removable.
        /// </summary>
        /// <value>
        /// The volume label removable.
        /// </value>
        [DataMember(Name = "VolumeLabel")]
        public string VolumeLabel { get; set; }

    }
}
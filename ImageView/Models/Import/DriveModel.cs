using System.IO;
using GeneralToolkitLib.Converters;
using ImageViewer.Utility;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// DriveModel
    /// </summary>
    public class DriveModel
    {
        /// <summary>
        /// The drive size formatted
        /// </summary>
        private string _driveSizeFormatted;
        /// <summary>
        /// The total size
        /// </summary>
        private long _totalSize;

        /// <summary>
        /// Gets or sets the name of the drive.
        /// </summary>
        /// <value>
        /// The name of the drive.
        /// </value>
        public string DriveName { get; set; }
        /// <summary>
        /// Gets or sets the drive format.
        /// </summary>
        /// <value>
        /// The drive format.
        /// </value>
        public string DriveFormat { get; set; }
        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        /// <value>
        /// The root directory.
        /// </value>
        public DirectoryInfo RootDirectory { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DriveModel"/> is removable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if removable; otherwise, <c>false</c>.
        /// </value>
        public bool Removable { get; set; }

        /// <summary>
        /// Gets or sets the total size.
        /// </summary>
        /// <value>
        /// The total size.
        /// </value>
        public long TotalSize
        {
            get => _totalSize;
            set
            {
                if (_totalSize != value)
                {
                    _totalSize = value;
                    _driveSizeFormatted = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the volume label removable.
        /// </summary>
        /// <value>
        /// The volume label removable.
        /// </value>
        public string VolumeLabel { get; set; }

        /// <summary>
        /// Gets the drive size formatted.
        /// </summary>
        /// <value>
        /// The drive size formatted.
        /// </value>
        public string DriveSizeFormatted
        {
            get => _driveSizeFormatted ?? (_driveSizeFormatted = getFormattedSize());
        }

        /// <summary>
        /// Gets the size of the formatted.
        /// </summary>
        /// <returns></returns>
        private string getFormattedSize()
        {
            var size = TotalSize;
            return SystemIOHelper.FormatFileSizeToString(size, 0);
        }

        public override string ToString()
        {
            return $"{DriveName} ({VolumeLabel}) ({DriveSizeFormatted})";
        }
    }
}
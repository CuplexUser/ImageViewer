using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    ///   VectorDataModel
    /// </summary>
    /// <seealso cref="ImageViewer.DataContracts" />
    [DataContract(Name = "VectorDataModel")]
    public class VectorDataModel : PointDataModel
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [DataMember(Name = "Width", Order = 1)]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [DataMember(Name = "Height", Order = 2)]
        public int Height { get; set; }
    }
}
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "SizeDataModel")]
    public class SizeDataModel
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeDataModel"/> class.
        /// </summary>
        public SizeDataModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeDataModel"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SizeDataModel(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// To the size.
        /// </summary>
        /// <returns></returns>
        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        /// <summary>
        /// Creates from size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static SizeDataModel CreateFromSize(Size size)
        {
            return new SizeDataModel(size.Width, size.Height);
        }
    }
}

using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "PointDataModel")]
    public class PointDataModel
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        [DataMember(Name = "X", Order = 1)]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        [DataMember(Name = "Y", Order = 2)]
        public int Y { get; set; }

        /// <summary>
        /// To the point.
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointDataModel"/> class.
        /// </summary>
        public PointDataModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointDataModel"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public PointDataModel(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates from point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static PointDataModel CreateFromPoint(Point point)
        {
            return new PointDataModel { X = point.X, Y = point.Y };
        }
    }
}

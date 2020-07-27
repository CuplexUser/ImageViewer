using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// Data contract used for storing FormWindow UI properties
    /// </summary>
    [DataContract( Name = "WindowPropertiesDataModel")]
    public class WindowPropertiesDataModel
    {
        /// <summary>
        /// Gets or sets the windows position expressed in screen coordinates.
        /// </summary>
        /// <value>
        /// The windows position.
        /// </value>
        public PointDataModel WindowsPosition { get; set; }

        public RectangleDataModel WindowClientRect { get; set; }


    }
}

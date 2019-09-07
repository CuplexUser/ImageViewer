using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "FormSizeAndPositionModel")]
    public class FormSizeAndPositionModel
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember(Name = "Location", Order = 1)]
        public PointDataModel Location { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [DataMember(Name = "Size", Order = 2)]
        public SizeDataModel Size { get; set; }

        /// <summary>
        /// Gets or sets the screen area.
        /// </summary>
        /// <value>
        /// The screen area.
        /// </value>
        [DataMember(Name = "ScreenArea", Order = 3)]
        public RectangleDataModel ScreenArea { get; set; }

        /// <summary>
        /// Gets or sets the type of the form.
        /// </summary>
        /// <value>
        /// The type of the form.
        /// </value>
        [DataMember(Name = "FormType", Order = 4)]
        public string FormType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSizeAndPositionModel"/> class.
        /// </summary>
        public FormSizeAndPositionModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSizeAndPositionModel"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="screenArea"></param>
        /// <param name="formType"></param>
        public FormSizeAndPositionModel(PointDataModel location, SizeDataModel size, RectangleDataModel screenArea, string formType)
        {
            Location = location;
            Size = size;
            ScreenArea = screenArea;
            FormType = formType;
        }
    }
}
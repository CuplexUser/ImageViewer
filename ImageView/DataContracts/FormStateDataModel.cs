using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ImageViewer.DataContracts
{
    /// <summary>
    ///   FormStateDataModel
    /// </summary>
    [Serializable]
    [DataContract(Name = "FormStateDataModel")]

    public class FormStateDataModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormStateDataModel"/> class.
        /// </summary>
        /// <param name="formType">The form.</param>
        /// <param name="windowState">State of the window.</param>
        public FormStateDataModel(Form formType, FormWindowState windowState)
        {
            WindowState = (int)windowState;
            var fullName = formType.GetType().FullName;
            if (fullName != null) FormTypeFullName = fullName.ToString();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "FormTypeFullName", Order = 1)]
        public string FormTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the screen rect.
        /// </summary>
        /// <value>
        /// The screen rect.
        /// </value>
        [DataMember(Name = "ScreenRect", Order = 2)]
        public RectangleDataModel ScreenRect { get; set; }

        /// <summary>
        /// Gets or sets the screen location.
        /// </summary>
        /// <value>
        /// The screen location.
        /// </value>
        [DataMember(Name = "ScreenLocation", Order = 3)]
        public PointDataModel ScreenLocation { get; set; }

        /// <summary>
        /// Gets or sets the form snap location.
        /// </summary>
        /// <value>
        /// The form snap location.
        /// </value>
        [DataMember(Name = "SnapLocation", Order = 4)]
        public int FormSnapLocation { get; set; }

        /// <summary>
        /// Gets or sets the state of the window.
        /// </summary>
        /// <value>
        /// The state of the window.
        /// </value>
        [DataMember(Name = "FormWindowState", Order = 5)]
        public int WindowState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [full screen].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "FullScreen", Order = 6)]
        public bool FullScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [top most].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [top most]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "TopMost", Order = 7)]
        public bool TopMost { get; set; }


    }
}

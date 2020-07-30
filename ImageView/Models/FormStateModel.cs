using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageViewer.Models
{
    /// <summary>
    /// Internal state model for a Windows UI properties. Where T has base class Form
    /// </summary>
    public class FormStateModel<T> where T: Form
    {
        public FormStateModel(T form, FormWindowState windowState)
        {
            WindowState = windowState;
            FormType = form;
        }

        public FormStateModel(T form)
        {
            SaveFormState(form);
        }

        public void SaveFormState(T form)
        {
            ScreenRect = form.RectangleToScreen(form.ClientRectangle);
            ScreenLocation = form.DesktopLocation;
            FormType = form;
            WindowState = form.WindowState;
            TopMost = form.TopMost;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public T FormType { get; set; }

        /// <summary>
        /// Gets or sets the screen rect.
        /// </summary>
        /// <value>
        /// The screen rect.
        /// </value>
        public Rectangle ScreenRect { get; set; }

        /// <summary>
        /// Gets or sets the screen location.
        /// </summary>
        /// <value>
        /// The screen location.
        /// </value>
        public Point ScreenLocation { get; set; }

        /// <summary>
        /// Gets or sets the form snap location.
        /// </summary>
        /// <value>
        /// The form snap location.
        /// </value>
        public SnapLocation FormSnapLocation { get; set; }

        /// <summary>
        /// Gets or sets the state of the window.
        /// </summary>
        /// <value>
        /// The state of the window.
        /// </value>
        public FormWindowState WindowState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [full screen].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen]; otherwise, <c>false</c>.
        /// </value>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [top most].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [top most]; otherwise, <c>false</c>.
        /// </value>
        public bool TopMost { get; set; }


        /// <summary>
        /// Flags specifying which edges to anchor the form at.
        /// </summary>
        [Flags]
        [Serializable]
        public enum SnapLocation
        {
            /// <summary>
            /// The none
            /// </summary>
            None = 0,
            /// <summary>
            /// The left
            /// </summary>
            Left = 1 << 0,
            /// <summary>
            /// The top
            /// </summary>
            Top = 1 << 1,
            /// <summary>
            /// The right
            /// </summary>
            Right = 1 << 2,
            /// <summary>
            /// The bottom
            /// </summary>
            Bottom = 1 << 3,
            /// <summary>
            /// All
            /// </summary>
            All = Left | Top | Right | Bottom
        }
    }
}
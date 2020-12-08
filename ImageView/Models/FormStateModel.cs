using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AutoMapper;
using ImageViewer.DataContracts;
using ImageViewer.Library.AutoMapperProfile;

namespace ImageViewer.Models
{
    /// <summary>
    /// Internal state model for a Windows UI properties. Where T has base class Form
    /// </summary>
    public class FormStateModel<T> where T : Form
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


        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<FormStateModel<T>, FormStateDataModel>()
                      .ForMember(s => s.FormTypeFullName, o => o.MapFrom(d => d.FormType.GetType().FullName))
                      .ForMember(s => s.FormSnapLocation, o => o.MapFrom(d => (int) d.FormSnapLocation))
                      .ForMember(s => s.ScreenLocation, o => o.MapFrom(d => new PointDataModel(d.ScreenLocation.X,d.ScreenLocation.Y)))
                      .ForMember(s => s.FormSnapLocation, o => o.MapFrom(d => (int) d.FormSnapLocation))
                      .ForMember(s => s.ScreenRect, o => o.MapFrom(d =>  new RectangleDataModel(d.ScreenRect)))
                      
                      .ForMember(s => s.WindowState, o => o.MapFrom(d => (int) d.WindowState))
                      .ReverseMap()
                      .ForMember(s => s.ScreenLocation, o => o.MapFrom(d => new Point(d.ScreenLocation.X, d.ScreenLocation.Y)))
                      .ForMember(s => s.FormType, o => o.MapFrom(d => CreateNewObjectFromAssemblyTypeName(d.FormTypeFullName)))
                      .ForMember(s => s.ScreenRect, o => o.MapFrom(d => new Rectangle(new Point(d.ScreenRect.X, d.ScreenRect.Y), new Size(d.ScreenRect.Width, d.ScreenRect.Height))))
                      .ForMember(s => s.FormType, o => o.MapFrom(d => Assembly.GetExecutingAssembly().GetType(d.FormTypeFullName)))
                      .ForMember(s => s.WindowState, o => o.MapFrom(d => (FormWindowState) d.WindowState))
                      .ForMember(s => s.FormSnapLocation, o => o.MapFrom(d => Enum.Parse(typeof(SnapLocation), d.FormSnapLocation.ToString())));
        }

        public static object CreateNewObjectFromAssemblyTypeName(string name)
        {
            var assemblyClassInstance = Assembly.GetExecutingAssembly().CreateInstance(name);
            return assemblyClassInstance;
        }
    }
}
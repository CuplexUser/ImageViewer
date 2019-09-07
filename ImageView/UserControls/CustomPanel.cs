using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    [Designer("System.Windows.Forms.Design.DocumentDesigner, System.Windows.Forms.Design", typeof(IRootDesigner)), DesignerCategory("Form")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [Docking(DockingBehavior.Ask)]
    public partial class CustomPanel : Panel
    {
        private readonly Size _defaultSize;


        private readonly Pen _pen;

        public CustomPanel()
        {
            _defaultSize = new Size(250, 250);
            Size = _defaultSize;
            if (DesignMode)
                return;

            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            _pen = new Pen(InnerBorderColor) {Width = BorderWidthInner};

            InitializeComponent();
        }

        [DispId(1)]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "0x333333")]
        [AttributeProvider(typeof(Color))]
        [Description("The outmost border color"), Category("Appearance")]
        public Color OuterBorderColor { get; set; }

        [Description("The inset border color"), Category("Appearance")]
        [DispId(2)]
        [Browsable(true)]
        [AttributeProvider(typeof(Color))]
        [DefaultValue(typeof(Color), "0xCCCCCC")]
        public Color InnerBorderColor { get; set; }

        [Description("The outer most border with inside the control"), Category("Appearance")]
        [DispId(3)]
        [Browsable(true)]
        [AttributeProvider(typeof(int))]
        [DefaultValue(typeof(int), "0x1")]
        public int BorderWidthOuter { get; set; }

        [Description("The Inner border with after the outer border is painted"), Category("Appearance")]
        [DispId(4)]
        [DefaultValue(typeof(int), "1")]
        [Browsable(true)]
        public int BorderWidthInner { get; set; }

        [Description("The Inner border with after the outer border is painted"), Category("Design")]
        [DispId(5)]
        [DefaultValue(typeof(int), "0x1")]
        [Browsable(true)]
        protected new BorderStyle BorderStyle { get; set; }


        protected override Size DefaultSize => _defaultSize;


        //protected override void OnNotifyMessage(Message m)
        //{
        //    base.OnNotifyMessage(m);

        //}


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (BackgroundImage == null)
            {
                base.OnPaintBackground(e);
                return;
            }


            var bufferedGraphicsContext = new BufferedGraphicsContext();
            BufferedGraphics bufferedGraphics = bufferedGraphicsContext.Allocate(e.Graphics, e.ClipRectangle);
            using (Graphics g = bufferedGraphics.Graphics)
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //IntPtr graphicsBufferHfc;
                //IntPtr hdcIntPtr;
                switch (BackgroundImageLayout)
                {
                    case ImageLayout.None:
                        break;
                    case ImageLayout.Tile:

                        break;
                    case ImageLayout.Center:

                        break;
                    case ImageLayout.Stretch:
                        float scaleX = BackgroundImage.Width/(float) Width;
                        float scaleY = BackgroundImage.Height/(float) Height;


                        //if (y > 0)
                        //{
                        //    y = y * (BackgroundImage.Width / e.ClipRectangle.Width);
                        //}
                        var srcRect = new Rectangle(Convert.ToInt32(scaleX*e.ClipRectangle.X),
                            Convert.ToInt32(scaleY*e.ClipRectangle.Y), Convert.ToInt32(e.ClipRectangle.Width*scaleX),
                            Convert.ToInt32(e.ClipRectangle.Height*scaleY));
                        g.DrawImage(BackgroundImage, e.ClipRectangle, srcRect, GraphicsUnit.Pixel);

                        break;
                    case ImageLayout.Zoom:
                        var renderImage = (Image) BackgroundImage.Clone();
                        g.Clear(Color.White);
                        g.DrawImageUnscaled(renderImage, 0, 0, renderImage.Width, renderImage.Height);
                        g.ScaleTransform(BackgroundImage.Width/(float) Width, renderImage.Height/(float) Height,
                            MatrixOrder.Prepend);


                        //hdcIntPtr = e.Graphics.GetHdc();
                        bufferedGraphics.Render(e.Graphics);
                        //e.Graphics.ReleaseHdc(hdcIntPtr);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                bufferedGraphics.Render();
                bufferedGraphics.Dispose();
            }

            //bufferedGraphics.Graphics.ReleaseHdc(graphicsBufferHfc);
            //bufferedGraphics.Graphics.Dispose();
            bufferedGraphicsContext.Dispose();
            //bufferedGraphicsContext.Invalidate();
            //base.OnPaintBackground(e);
        }


        private void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{            
        //    Rectangle drawRectangle = Rectangle.Inflate(e.ClipRectangle, -1, -1);

        //    _pen.DashStyle = DashStyle.Solid;
        //    _pen.DashCap = DashCap.Round;
        //    _pen.DashOffset = 25;

        //    using (Graphics g = e.Graphics)
        //    {
        //        _pen.Color = OuterBorderColor;
        //        _pen.Width = BorderWidthOuter;

        //        g.DrawRectangle(_pen, drawRectangle);

        //        drawRectangle.Inflate(-1 * BorderWidthOuter, -1 * BorderWidthOuter);
        //        _pen.Color = OuterBorderColor;
        //        _pen.Width = BorderWidthInner;

        //        g.DrawRectangle(_pen, drawRectangle);
        //    }
        //}
    }
}
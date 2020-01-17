using GeneralToolkitLib.Events;
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
    public partial class CustomPanel : UserControl
    {
        private readonly Size _defaultSize;
        private Pen _pen;
        private Color outerBorderColor;
        private Color innerBorderColor;
        private int borderWidthOuter;
        private int borderWidthInner;

        public event EventHandler<BorderChangedEventArgs> InnerBorderColorChanged;
        public event EventHandler<BorderChangedEventArgs> OuterBorderColorChanged;

        public event EventHandler<BorderChangedEventArgs> InnerBorderWithChanged;
        public event EventHandler<BorderChangedEventArgs> OuterBorderWithChanged;

        public CustomPanel()
        {
            _defaultSize = new Size(250, 250);
            Size = _defaultSize;
            if (DesignMode)
                return;

            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            _pen = new Pen(InnerBorderColor) { Width = BorderWidthInner };

            InnerBorderColorChanged += OnInnerBorderColorChanged;
            OuterBorderColorChanged += OnOuterBorderColorChanged;

            InnerBorderWithChanged += OnInnerBorderWithChanged;
            OuterBorderWithChanged += OnOuterBorderWithChanged;

            EnableDoubleBuffering();
            InitializeComponent();
        }



        [DispId(1)]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "0x333333")]
        [AttributeProvider(typeof(Color))]
        [Description("The outmost border color"), Category("Appearance")]
        public Color OuterBorderColor
        {
            get => outerBorderColor;
            set
            {
                if (outerBorderColor != value)
                {
                    outerBorderColor = value;
                    OuterBorderColorChanged?.Invoke(this, new BorderChangedEventArgs(BorderChangedEventArgs.TypeOfBorderUpdate.OuterBorderColorChanged));

                }
            }
        }

        [DispId(2)]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "0xCCCCCC")]
        [AttributeProvider(typeof(Color))]

        [Description("The inner border color"), Category("Appearance")]
        public Color InnerBorderColor
        {
            get => innerBorderColor;
            set
            {
                if (innerBorderColor != value)
                {
                    innerBorderColor = value;
                    InnerBorderColorChanged?.Invoke(this, new BorderChangedEventArgs(BorderChangedEventArgs.TypeOfBorderUpdate.InnerBorderColorChanged));
                }
            }
        }

        [Description("The outer most border with inside the control"), Category("Appearance")]
        [DispId(3)]
        [Browsable(true)]
        [AttributeProvider(typeof(int))]
        [DefaultValue(typeof(int), "0")]
        public int BorderWidthOuter
        {
            get => borderWidthOuter;
            set
            {
                if (BorderWidthOuter != value)
                {
                    borderWidthOuter = value;
                    OuterBorderWithChanged?.Invoke(this, new BorderChangedEventArgs(BorderChangedEventArgs.TypeOfBorderUpdate.OuterBorderWithChanged));
                }
            }
        }

        [Description("The Inner border with after the outer border is painted"), Category("Appearance")]
        [DispId(4)]
        [DefaultValue(typeof(int), "0")]
        [Browsable(true)]
        public int BorderWidthInner
        {
            get { return borderWidthInner; }
            set
            {
                if (borderWidthInner != value)
                {
                    borderWidthInner = value;
                    InnerBorderWithChanged?.Invoke(this, new BorderChangedEventArgs(BorderChangedEventArgs.TypeOfBorderUpdate.InnerBorderWithChanged));
                }
            }
        }

        [Description("The Inner border with after the outer border is painted"), Category("Design")]
        [DispId(5)]
        [DefaultValue(typeof(int), "0")]
        [Browsable(true)]
        protected new BorderStyle BorderStyle { get; set; }


        protected override Size DefaultSize => _defaultSize;

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
        }


        protected virtual void OnInnerBorderColorChanged(object sender, BorderChangedEventArgs e)
        {
            Invalidate();
        }

        protected virtual void OnOuterBorderColorChanged(object sender, BorderChangedEventArgs e)
        {
            Invalidate();
        }

        protected virtual void OnInnerBorderWithChanged(object sender, BorderChangedEventArgs e)
        {
            Invalidate();
        }

        protected virtual void OnOuterBorderWithChanged(object sender, BorderChangedEventArgs e)
        {
            Invalidate();
        }
        private void EnableDoubleBuffering()
        {
            // Set the value of the double-buffering style bits to true.
            //SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //UpdateStyles();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {          

            Rectangle drawRectangle = new Rectangle(-2, -2, this.Width+2, this.Height+2);
            //if (e.ClipRectangle.Width >= drawRectangle.Width && e.ClipRectangle.Height>= drawRectangle.Height)
            
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                _pen.Color = BackColor;                
                g.Clear(_pen.Color);


                if (BorderWidthOuter > 0)
                {
                    _pen.Color = outerBorderColor;
                    _pen.Width = borderWidthOuter;
                    _pen.Brush = new SolidBrush(OuterBorderColor);
                    drawRectangle.Inflate(borderWidthInner * -1, borderWidthOuter * -1);
                    drawRectangle.Height = Height;
                    g.FillRectangle(_pen.Brush, drawRectangle);
                }

                if (BorderWidthInner > 0)
                {
                    drawRectangle.Inflate(BorderWidthInner * -1, -BorderWidthInner * -1);
                    _pen.Color = innerBorderColor;
                    _pen.Width = borderWidthInner;
                    _pen.Brush = new SolidBrush(InnerBorderColor);

                    g.FillRectangle(_pen.Brush, drawRectangle);
                }
            //}
            //else
            //{
            //    //(base.OnPaintBackground(e);
            //}
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            System.Drawing.Rectangle drawRectangle = new System.Drawing.Rectangle(-1, -1, this.Width+2, this.Height+2);

            //if (e.ClipRectangle.Width >= drawRectangle.Width && e.ClipRectangle.Height >= drawRectangle.Height)
            //{
                drawRectangle.Width = e.ClipRectangle.Width;
                drawRectangle.Height = e.ClipRectangle.Height;
                drawRectangle.X = 0;
                drawRectangle.Y = Math.Max(e.ClipRectangle.Y, this.Height);

                var g = e.Graphics;
                var container = g.BeginContainer();

                //g.SetClip(drawRectangle);

                _pen = new Pen(OuterBorderColor, BorderWidthOuter);
                _pen.Brush = new SolidBrush(_pen.Color);


                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.DrawRectangle(_pen, drawRectangle);

                drawRectangle.Inflate(-1 * BorderWidthOuter, -1 * BorderWidthOuter);
                _pen.Color = OuterBorderColor;
                _pen.Width = BorderWidthInner;

                g.DrawRectangle(_pen, drawRectangle);

                g.EndContainer(container); ;
                g.Save();
            //}
            //else
            //{
            //    //base.OnPaint(e);
            //}
        }
    }
}
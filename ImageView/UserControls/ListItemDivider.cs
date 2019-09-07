using System.Drawing;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    public class ListItemDivider
    {
        public enum DividerPositions
        {
            Top,
            Bottom
        }

        private Pen _pen;

        public ListItemDivider()
        {
        }

        public ListItemDivider(string text, string value)
        {
            DividerMargin = 2;
            Text = text;
            Value = value;
        }

        private Pen DividerPen
        {
            get { return _pen ?? (_pen = new Pen(DividerColor)); }
        }

        public Color DividerColor { get; set; } = Color.DarkGray;

        public DividerPositions DividerPosition { get; set; }

        public int DividerMargin { get; set; } = 2;

        public int Height { get; set; } = 20;

        public string Value { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public void DrawItem(object sender, DrawItemEventArgs e)
        {
            var userControl = sender as ComboBox;
            if (userControl == null) return;


            if (DividerPosition == DividerPositions.Top)
            {
                Color back = e.State == DrawItemState.Focus ? e.ForeColor : e.BackColor;

                var backgrounRectangle = new Rectangle(0, e.Bounds.Top + DividerMargin*2 + (int) DividerPen.Width,
                    e.Bounds.Width, e.Bounds.Height - DividerMargin*2);
                e.Graphics.FillRectangle(new SolidBrush(back), backgrounRectangle);

                e.Graphics.DrawLine(DividerPen, new Point(e.Bounds.Left, e.Bounds.Top + DividerMargin),
                    new Point(e.Bounds.Right, e.Bounds.Top + DividerMargin));


                float fontTopPosition = (float) e.Bounds.Top + DividerMargin*2 + e.Bounds.Height/2f -
                                        userControl.Font.GetHeight()/2f;
                var textRectangle = new Rectangle(0, (int) fontTopPosition, e.Bounds.Width, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, Text, userControl.Font, textRectangle, userControl.ForeColor,
                    TextFormatFlags.Left);
            }
            else
            {
                e.Graphics.DrawLine(DividerPen, new Point(e.Bounds.Left, e.Bounds.Bottom - DividerMargin),
                    new Point(e.Bounds.Right, e.Bounds.Bottom - DividerMargin));
                TextRenderer.DrawText(e.Graphics, Text, userControl.Font, e.Bounds, userControl.ForeColor,
                    TextFormatFlags.Left);
            }
        }
    }
}
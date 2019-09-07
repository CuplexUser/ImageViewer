using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    public class ColorSelectComboBox : ComboBox
    {
        public ColorSelectComboBox()
        {
            Initialize();
        }

        private void Initialize()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawVariable;
            DrawItem += ColorSelectComboBox_DrawItem;
            MeasureItem += OnMeasureItem;
            ForeColor = Color.Transparent;
            AutoSize = true;
        }


        private void OnMeasureItem(object sender, MeasureItemEventArgs measureItemEventArgs)
        {
            measureItemEventArgs.ItemHeight = 20;
            measureItemEventArgs.ItemWidth = Width - 10;
        }

        private void ColorSelectComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw background
            // e.DrawBackground();

            if (Items.Count == 0)
            {
                return;
            }

            var item = (Color) Items[e.Index];
            Pen p = new Pen(Color.Black);

            // Draw color rectangle border
            var colorRect = new Rectangle(4, e.Bounds.Top + 2, e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.DrawRectangle(p, colorRect);

            colorRect.Inflate(new Size(-1, -1));
            Brush brush = new SolidBrush(item);

            // Fill rectangle with the selected color
            e.Graphics.FillRectangle(brush, colorRect);


            // Draw item text
            var font = Parent.Font;
            brush = new SolidBrush(Color.Black);
            Point beginTextDrawPoint = new Point(colorRect.Width + 10, e.Bounds.Top + 2);
            e.Graphics.DrawString(item.Name, font, brush, beginTextDrawPoint);

            //e.DrawFocusRectangle();
        }
    }
}
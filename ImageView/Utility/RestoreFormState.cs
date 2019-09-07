using System.Drawing;
using System.Windows.Forms;
using ImageViewer.DataContracts;

namespace ImageViewer.Utility
{
    public static class RestoreFormState
    {
        public static bool SetFormSizeAndPosition(Form form, Size size, Point location, Rectangle screenArea)
        {
            if (size.Height <= 0 || size.Width <= 0)
                return false;

            if (form.MinimumSize.Height > 0 && size.Height < form.MinimumSize.Height)
            {
                return false;
            }

            if (form.MinimumSize.Width > 0 && size.Width < form.MinimumSize.Width)
                return false;

            Rectangle formRect = new Rectangle(location, size);
            if (formRect.Right < screenArea.Right || formRect.Left < screenArea.Left)
            {
                return false;
            }

            if (formRect.Top > screenArea.Bottom || formRect.Top < screenArea.Top)
            {
                return false;
            }

            form.Size = size;
            form.Location = location;

            return true;
        }

        public static FormSizeAndPositionModel GetFormState(Form form)
        {
            return new FormSizeAndPositionModel(new PointDataModel(form.Left, form.Top), new SizeDataModel(form.Width, form.Height), new RectangleDataModel(form.ClientRectangle), form.GetType().Name);
        }

        internal static void SetFormSizeAndPosition(Form form, FormSizeAndPositionModel formState)
        {
            SetFormSizeAndPosition(form, formState.Size.ToSize(), formState.Location.ToPoint(), formState.ScreenArea.ToRectangle());
        }
    }
}

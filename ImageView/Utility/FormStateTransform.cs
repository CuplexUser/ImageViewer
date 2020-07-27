using System.Windows.Forms;
using ImageViewer.Models.Enums;
using ImageViewer.Services;

namespace ImageViewer.Utility
{
    /// <summary>
    /// Calculate Screen SnapLocation. Default no form location Update. Just setting the correct snap location.
    /// </summary>
    public static class FormStateTransform
    {
        /// <summary>
        /// The snap margin
        /// </summary>
        public const int SnapMargin = 10;

        /// <summary>
        /// Updates the form state snap location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form">The form.</param>
        /// <param name="formState">State of the form.</param>
        public static void UpdateFormStateSnapLocation(this Form form, FormStateModel<Form> formState)
        {
            if (formState.WindowState != FormWindowState.Normal)
            {
                formState.FormSnapLocation = formState.WindowState == FormWindowState.Maximized ? FormStateModel<Form>.SnapLocation.All : FormStateModel<Form>.SnapLocation.None;

                return;
            }

            formState.FormSnapLocation = FormStateModel<Form>.SnapLocation.None;
            var formRect = form.RectangleToScreen(form.ClientRectangle);

            var workingArea = Screen.PrimaryScreen.WorkingArea;
            workingArea.Inflate(SnapMargin * -1, SnapMargin * -1);

            int x = formRect.Left;
            int y = formRect.Top;
            int height = formRect.Height;
            int width = formRect.Width;


            if (formRect.Top <= workingArea.Top)
            {
                y = Screen.PrimaryScreen.WorkingArea.Top;
                formState.FormSnapLocation |= FormStateModel<Form>.SnapLocation.Top;

            }
            if (formRect.Left <= workingArea.Left)
            {
                x = Screen.PrimaryScreen.WorkingArea.Left;
                formState.FormSnapLocation |= FormStateModel<Form>.SnapLocation.Left;
            }

            if (formRect.Bottom >= workingArea.Bottom)
            {
                height = Screen.PrimaryScreen.WorkingArea.Height - y;
                formState.FormSnapLocation |= FormStateModel<Form>.SnapLocation.Bottom;
            }

            if (formRect.Right >= workingArea.Right)
            {
                width = workingArea.Width - x;
                formState.FormSnapLocation |= FormStateModel<Form>.SnapLocation.Right;
            }

        }


        public static void SaveFormState(this Form form, ApplicationSettingsService appSettingsService)
        {

            FormStateModel<Form> formState;
            if (appSettingsService.Settings.FormStateDictionary.ContainsKey(form.GetType().Name))
            {
                formState = appSettingsService.Settings.FormStateDictionary[form.GetType().Name];
            }
            else
            {
                formState = new FormStateModel<Form>(form);
                appSettingsService.Settings.FormStateDictionary.Add(formState.FormType.GetType().Name, formState);
            }

            UpdateFormStateSnapLocation(form, formState);
            formState.FormType = form;
            formState.ScreenLocation = form.PointToScreen(form.Location);
            formState.ScreenRect = form.RectangleToScreen(form.ClientRectangle);
            formState.TopMost = form.TopMost;
            formState.WindowState = form.WindowState;

            appSettingsService.SaveSettings();
        }

        internal static void LoadFormState(Form form, FormStateModel<Form> formState)
        {
            var b = formState.ScreenRect;
            form.SetDesktopLocation(formState.ScreenLocation.X, formState.ScreenLocation.Y);
            form.SetDesktopBounds(b.X, b.Y, b.Width, b.Height);
            form.TopMost = formState.TopMost;
            form.WindowState = formState.WindowState;
        }
    }
}

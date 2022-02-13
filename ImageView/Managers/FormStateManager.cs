using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Windows.Forms;
using ImageViewer.Models;
using Serilog;

namespace ImageViewer.Managers
{
    public class FormStateManager
    {
        public static bool RestoreFormState(ApplicationSettingsModel settings, Form form)
        {
            if (settings.FormStateModels.ContainsKey(form.Name))
            {
                try
                {
                    var model = settings.FormStateModels[form.Name];
                    form.Width = model.FormSize.Width;
                    form.Height = model.FormSize.Height;

                    form.Location = new Point(model.FormPosition.X, model.FormPosition.Y);
                    form.WindowState = (FormWindowState)model.WindowState;

                    return true;
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "RestoreFormState Exception");
                }
            }

            return false;
        }

        public static void SaveFormState(ApplicationSettingsModel appSettings, Form form)
        {
            FormStateModel model;
            if (appSettings.FormStateModels == null)
                appSettings.FormStateModels = new ConcurrentDictionary<string, FormStateModel>();

            if (appSettings.FormStateModels.ContainsKey(form.Name))
                model = appSettings.FormStateModels[form.Name];
            else
            {
                model = new FormStateModel();
                appSettings.FormStateModels.Add(form.Name, model);
            }

            model.FormPosition = form.Location;
            model.FormSize = new Size(form.Width, form.Height);
            model.FormName = form.Name;

            switch (form.WindowState)
            {
                case FormWindowState.Normal:
                    model.WindowState = FormState.Normal;
                    break;
                case FormWindowState.Minimized:
                    model.WindowState = FormState.Minimized;
                    break;
                case FormWindowState.Maximized:
                    model.WindowState = FormState.Maximized;
                    model.FormSize = form.MinimumSize;
                    break;
                default:
                    model.WindowState = FormState.Normal;
                    break;
            }
        }

        public static void ToggleFullscreen(WindowStateModel windowState, Form form)
        {
            if (windowState.IsFullscreen)
            {
                // Restore normal window state
                form.FormBorderStyle = windowState.BorderStyle;
                form.WindowState = windowState.WindowState;
                form.BackColor = windowState.BackgroundColor;
                windowState.IsFullscreen = false;
            }
            else
            {
                // Set initial values to be restored when leaving fullscreen mode
                windowState.BorderStyle = form.FormBorderStyle;
                windowState.WindowState = form.WindowState;
                windowState.BackgroundColor = form.BackColor;

                // Set fullscreen
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;
                form.BackColor = Color.Black;
                windowState.IsFullscreen = true;
            }
        }
    }
}
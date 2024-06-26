﻿using ImageViewer.Models;
using System.Collections.Concurrent;

namespace ImageViewer.Managers;

public static class FormStateManager
{
    public static bool RestoreFormState(ApplicationSettingsModel settings, Form form)
    {
        if (settings.FormStateModels.ContainsKey(form.Name))
        {
            try
            {
                var model = settings.FormStateModels[form.Name];

                //Validate FormStateModel
                if (!ValidateFormStateModel(model))
                {
                    Log.Warning($"The FormStateModel for {form.Name} was invalid. Replacing it with a generic. FormSize={model.FormSize}", form.Name, model.FormSize);
                    settings.FormStateModels.Remove(model.FormName);

                    // Creating a new generic FormState for specified Form and with center screen start
                    model = new FormStateModel
                    {
                        FormName = form.Name,
                        WindowState = FormState.Normal,
                        FormSize = form.Bounds.Size,
                        FormPosition = new Point()
                    };

                    if (model.FormSize.IsEmpty)
                    {
                        model.FormSize = form.MinimumSize;
                    }

                    //model.FormPosition = GetOffsetCenterPointInRect(Screen.PrimaryScreen.Bounds, form.Size);
                    settings.FormStateModels.Add(form.Name, model);
                }

                form.Bounds = new Rectangle(model.FormPosition, model.FormSize);
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

    public static Dictionary<string, string> GetAdditionalParameters(ApplicationSettingsModel settings, Form form)
    {
        if (settings.FormStateModels.TryGetValue(form.Name, out var model))
        {
            return model.AdditionalParameters;
        }

        return null;
    }

    public static void UpdateAdditionalParameters(ApplicationSettingsModel settings, Form form, Dictionary<string, string> additionalParameters, bool replace = false)
    {
        if (!settings.FormStateModels.TryGetValue(form.Name, out var model)) return;
        var parameters = model.AdditionalParameters;

        if (replace)
        {
            parameters = additionalParameters;
        }
        else
        {
            if (parameters == null)
            {
                parameters = additionalParameters;
            }
            else
            {
                foreach (var key in additionalParameters.Keys)
                    if (parameters.ContainsKey(key))
                    {
                        parameters[key] = additionalParameters[key];
                    }
                    else
                    {
                        parameters.Add(key, additionalParameters[key]);
                    }
            }
        }

        settings.FormStateModels[form.Name].AdditionalParameters = parameters;
    }

    private static Point GetOffsetCenterPointInRect(Rectangle boundingRect, Size winSize)
    {
        var center = new Point(boundingRect.Width / 2, boundingRect.Height / 2);
        center.X -= winSize.Width / 2;
        center.Y -= winSize.Height / 2;

        return center;
    }

    //Validates that the Form is located on any screen
    private static bool ValidateFormStateModel(FormStateModel model)
    {
        var modelBounds = new Rectangle(model.FormPosition, model.FormSize);
        bool intersect = Screen.AllScreens.Any(x => x.Bounds.IntersectsWith(modelBounds));

        return intersect;
    }

    public static void SaveFormState(ApplicationSettingsModel appSettings, Form form)
    {
        FormStateModel model;
        appSettings.FormStateModels ??= new ConcurrentDictionary<string, FormStateModel>();

        if (appSettings.FormStateModels.TryGetValue(form.Name, out var stateModel))
        {
            model = stateModel;
        }
        else
        {
            model = new FormStateModel();
            appSettings.FormStateModels.Add(form.Name, model);
        }

        model.FormName = form.Name;
        model.WindowState = (FormState)form.WindowState;
        if (form.WindowState == FormWindowState.Normal)
        {
            model.FormSize = form.Bounds.Size;
            model.FormPosition = form.Bounds.Location;
        }
        else
        {
            model.FormSize = form.RestoreBounds.Size;
            model.FormPosition = form.RestoreBounds.Location;
        }
    }

    public static void ToggleFullscreen(Form form, WindowStateModel windowState, bool fullscreen)
    {
        if (fullscreen)
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
        else
        {
            // Restore normal window state
            form.FormBorderStyle = windowState.BorderStyle;
            form.WindowState = windowState.WindowState;
            form.BackColor = windowState.BackgroundColor;
            windowState.IsFullscreen = false;
        }
    }
}
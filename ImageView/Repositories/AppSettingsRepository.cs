﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AutoMapper;
using GeneralToolkitLib.ConfigHelper;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using ImageViewer.Providers;
using Serilog;

namespace ImageViewer.Repositories
{
    public class AppSettingsRepository : RepositoryBase
    {
        private readonly FileSystemIOProvider _ioProvider;
        private readonly string appConfigSettingsFilePath;
        private const string SettingsFilename = "ImageConverterSettings.bin";
        private readonly IMapper _mapper;
        private ApplicationSettingsModel _settingsModel;

        public AppSettingsRepository(IMapper mapper)
        {
            _mapper = mapper;
            _ioProvider = new FileSystemIOProvider();
            appConfigSettingsFilePath = Path.Combine(GlobalSettings.Settings.GetUserDataDirectoryPath(), SettingsFilename);
        }

        public static ApplicationSettingsModel GetDefaultApplicationSettings()
        {
            var settings = new ApplicationSettingsModel
            {
                AlwaysOntop = false,
                AutoRandomizeCollection = true,
                LastUsedSearchPaths = new List<string>(),
                ShowImageViewFormsInTaskBar = true,
                NextImageAnimation = ApplicationSettingsModel.ChangeImageAnimation.None,
                ImageTransitionTime = 1000,
                SlideshowInterval = 5000,
                PrimaryImageSizeMode = (int) PictureBoxSizeMode.Zoom,
                PasswordProtectBookmarks = false,
                PasswordDerivedString = "",
                ShowNextPrevControlsOnEnterWindow = false,
                ThumbnailSize = 256,
                MaxThumbnails = 256,
                ConfirmApplicationShutdown = true,
                AutomaticUpdateCheck = true,
                LastUpdateCheck = new DateTime(2010, 1, 1),
                ImageCacheSize = 134217728, // 128 Mb,
                ToggleSlideshowWithThirdMouseButton = true,
                AutoHideCursor = true,
                AutoHideCursorDelay = 2000,
                AppSettingsGuid = Guid.NewGuid(),
                IsLoadedFromDisk = false,
                FormStateModels = new Dictionary<string, FormStateModel>()
            };

            return settings;
        }

        public ApplicationSettingsModel LoadSettings()
        {
            if (_settingsModel is {IsLoadedFromDisk: true})
            {
                return _settingsModel;
            }

            if (File.Exists(appConfigSettingsFilePath))
            {
                var applicationConfig = _ioProvider.LoadApplicationSettings(appConfigSettingsFilePath);
                _settingsModel = _mapper.Map<ApplicationSettingsModel>(applicationConfig);
                _settingsModel.IsLoadedFromDisk = true;
            }
            else
            {
                _settingsModel = GetDefaultApplicationSettings();
                Log.Warning("Failed to load settings from disk. {appConfigSettingsFilePath}", appConfigSettingsFilePath);
            }

            if (_settingsModel.FormStateModels == null)
            {
                _settingsModel.FormStateModels = new ConcurrentDictionary<string, FormStateModel>();
                Log.Warning("Settings FormStateModels where null at LoadSettings");
            }

            if (_settingsModel.AppSettingsGuid == Guid.Empty)
                _settingsModel.AppSettingsGuid = Guid.NewGuid();

            OnLoadSettingsCompleted();
            return _settingsModel;
        }

        public bool SaveSettings(ApplicationSettingsModel settings)
        {
            ApplicationSettingsDataModel settingsDataModel = _mapper.Map<ApplicationSettingsModel, ApplicationSettingsDataModel>(settings);
            bool result = _ioProvider.SaveApplicationSettings(appConfigSettingsFilePath, settingsDataModel);
            if (result)
            {
                _settingsModel = settings;
                OnSaveSettingsCompleted();
            }

            return result;
        }

        /// <summary>
        /// Occurs when [load settings completed].
        /// </summary>
        public event EventHandler LoadSettingsCompleted;

        /// <summary>
        /// Occurs when [save settings completed].
        /// </summary>
        public event EventHandler SaveSettingsCompleted;

        public bool IsDirty { get; set; }

        /// <summary>
        /// Called when [load settings completed].
        /// </summary>
        protected void OnLoadSettingsCompleted()
        {
            LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when [save settings completed].
        /// </summary>
        protected void OnSaveSettingsCompleted()
        {
            SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
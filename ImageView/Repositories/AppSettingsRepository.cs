using System.Collections.Concurrent;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using ImageViewer.Models.FormMenuHistory;
using ImageViewer.Models.Interface;
using ImageViewer.Providers;

namespace ImageViewer.Repositories;

public class AppSettingsRepository : RepositoryBase, IAppSettingsRepository
{
    private const string SettingsFilename = "ImageViewSettings.dat";
    private const string MockPwd = "kpGSuZwV2gvjnyHNDhS6q2*%nU7WB4F6xpyhWn%Nrhs49BZKeiFbee!fzh2MTQ%d";
    private readonly string _appConfigSettingsFilePath;
    private readonly FileSystemIOProvider _ioProvider;
    private readonly IMapper _mapper;
    private ApplicationSettingsModel _settingsModel;

    public AppSettingsRepository(IMapper mapper)
    {
        _mapper = mapper;
        _ioProvider = new FileSystemIOProvider();
        _appConfigSettingsFilePath = Path.Combine(GlobalSettings.Settings.GetUserDataDirectoryPath(), SettingsFilename);
    }

    public bool IsDirty { get; set; }

    public virtual ApplicationSettingsModel LoadSettings()
    {
        if (_settingsModel is { IsLoadedFromDisk: true })
        {
            return _settingsModel;
        }

        if (File.Exists(_appConfigSettingsFilePath))
        {
            try
            {
                var applicationConfig = _ioProvider.LoadApplicationSettings(_appConfigSettingsFilePath, MockPwd);
                _settingsModel = _mapper.Map<ApplicationSettingsModel>(applicationConfig);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ApplicationSettingsModel:LoadSettings()");
            }

            if (_settingsModel == null || _settingsModel.AppSettingsGuid == Guid.Empty)
            {
                var dataModel = ApplicationSettingsDataModel.CreateDefaultSettings();
                _settingsModel = _mapper.Map<ApplicationSettingsModel>(dataModel);
                Log.Warning("Failed to Deserialize Settings file");
            }

            _settingsModel.IsLoadedFromDisk = true;
        }
        else
        {
            _settingsModel = GetDefaultApplicationSettings();
            SaveSettings(_settingsModel);
            _settingsModel.IsLoadedFromDisk = true;
            Log.Warning("Failed to load settings from disk. {appConfigSettingsFilePath}", _appConfigSettingsFilePath);
        }

        if (_settingsModel.FormStateModels == null)
        {
            _settingsModel.FormStateModels = new ConcurrentDictionary<string, FormStateModel>();
            Log.Warning("Settings FormStateModels where null at LoadSettings");
        }

        if (_settingsModel.AppSettingsGuid == Guid.Empty)
        {
            _settingsModel.AppSettingsGuid = Guid.NewGuid();
        }

        OnLoadSettingsCompleted();
        return _settingsModel;
    }

    public bool SaveSettings(ApplicationSettingsModel settings)
    {
        var settingsDataModel = _mapper.Map<ApplicationSettingsModel, ApplicationSettingsDataModel>(settings);
        bool result = _ioProvider.SaveApplicationSettings(_appConfigSettingsFilePath, settingsDataModel, MockPwd);
        if (result)
        {
            _settingsModel = settings;
            OnSaveSettingsCompleted();
        }

        return result;
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
            PrimaryImageSizeMode = (int)PictureBoxSizeMode.Zoom,
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
            FormStateModels = new Dictionary<string, FormStateModel>(),
            UseRecycleBin = true,
            RecentFilesCollection = new RecentFilesCollection(null)
        };

        return settings;
    }

    /// <summary>
    ///     Occurs when [load settings completed].
    /// </summary>
    public event EventHandler LoadSettingsCompleted;

    /// <summary>
    ///     Occurs when [save settings completed].
    /// </summary>
    public event EventHandler SaveSettingsCompleted;

    /// <summary>
    ///     Called when [load settings completed].
    /// </summary>
    public void OnLoadSettingsCompleted()
    {
        LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     Called when [save settings completed].
    /// </summary>
    protected void OnSaveSettingsCompleted()
    {
        SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
    }
}
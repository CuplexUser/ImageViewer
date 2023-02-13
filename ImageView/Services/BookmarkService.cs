using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Utility.RandomGenerator;
using ImageViewer.Managers;

namespace ImageViewer.Services;

[UsedImplicitly]
public sealed class BookmarkService : ServiceBase, IDisposable
{
    private const string BookmarkFileName = "ImageViewBookmarks.dat";
    private static readonly object LockObj = new();
    private readonly ApplicationSettingsService _applicationSettingsService;
    private readonly string _directory;
    private readonly PasswordStorage _passwordStorage;
    private readonly string _protectedMemoryStorageKey;

    public BookmarkService(BookmarkManager bookmarkManager, ApplicationSettingsService applicationSettingsService)
    {
        BookmarkManager = bookmarkManager;
        _applicationSettingsService = applicationSettingsService;
        _protectedMemoryStorageKey = new SecureRandomGenerator().GetAlphaNumericString(256);
        _directory = ApplicationBuildConfig.UserDataPath;

        applicationSettingsService.LoadSettings();

        _passwordStorage = new PasswordStorage();
        _passwordStorage.Set(_protectedMemoryStorageKey, GetDefaultPassword());
    }

    public BookmarkManager BookmarkManager { get; }

    public void Dispose()
    {
        _passwordStorage?.Dispose();
    }


    public bool OpenBookmarks()
    {
        lock (LockObj)
        {
            return OpenBookmarks(GetDefaultPassword());
        }
    }

    public bool OpenBookmarks(string password)
    {
        string filename = _directory + BookmarkFileName;
        if (!File.Exists(filename))
            return false;

        bool loadSuccessful = BookmarkManager.LoadFromFile(filename, password);
        if (loadSuccessful)
        {
            _passwordStorage.Set(_protectedMemoryStorageKey, password);
            Log.Information("Loaded bookmarks from file");
            return true;
        }

        Log.Error("Failed to load bookmarks from file");
        return false;
    }

    public bool SaveBookmarks(bool savedAsync = false)
    {
        var result = true;
        lock (LockObj)
        {
            if (BookmarkManager.IsModified)
            {
                string password = _passwordStorage.Get(_protectedMemoryStorageKey);
                result = BookmarkManager.SaveToFile(Path.Combine(_directory, BookmarkFileName), password);
                Log.Debug("SaveBookmarks called with Result: {result}, SavedAsync: {savedAsync}, ManagedThreadId: {ManagedThreadId}", result, savedAsync, Thread.CurrentThread.ManagedThreadId);
            }

            return result;
        }
    }

    private string GetDefaultPassword()
    {
        try
        {
            string defaultKey = _applicationSettingsService.Settings.DefaultKey;

            if (defaultKey != null && defaultKey.Length == 256)
                return defaultKey;

            string previousKey = defaultKey;
            defaultKey = new SecureRandomGenerator().GetAlphaNumericString(256);
            _applicationSettingsService.Settings.DefaultKey = defaultKey;
            _applicationSettingsService.SetSettingsStateModified();
            _applicationSettingsService.SaveSettings();
            Log.Information("New default bookmark key was created and saved. Previous key was: {previousKey}", previousKey);

            return defaultKey;
        }
        catch (Exception e)
        {
            Log.Error(e, "GetDefaultPassword");
            return "CodeRed";
        }
    }

    public async Task SaveBookmarksAsync()
    {
        await Task.Factory.StartNew(() => { SaveBookmarks(true); });
    }
}
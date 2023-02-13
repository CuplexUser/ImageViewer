namespace ImageViewer.Services;

/// <summary>
/// </summary>
[Flags]
public enum ThumbnailServiceState
{
    /// <summary>
    ///     The none
    /// </summary>
    Idle = 0x1,

    /// <summary>
    ///     Loading database state
    /// </summary>
    LoadingDatabase = 0x2,

    /// <summary>
    ///     Saving database state
    /// </summary>
    SavingDatabase = 0x4,

    /// <summary>
    ///     Scanning thumbnails state
    /// </summary>
    ScanningThumbnails = 0x8,

    /// <summary>
    ///     Scanning directory state
    /// </summary>
    ScanningDirectory = 0x10,

    /// <summary>
    ///     Database maintenance state
    /// </summary>
    DatabaseMaintenance = 0x20
}
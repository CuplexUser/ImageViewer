namespace ImageViewer.Configuration;

/// <summary>
///     Generic App Constants
/// </summary>
public static class GenericConstants
{
    /// <summary>
    ///     The minimum cursor delay value
    /// </summary>
    public const int MinCursorDelayValue = 250;

    /// <summary>
    ///     The maximum cursor delay value
    /// </summary>
    public const int MaxCursorDelayValue = 10000;

    /// <summary>
    ///     The default cursor delay value
    /// </summary>
    public const int DefaultCursorDelayValue = 2500;

    /// <summary>
    ///     The image search pattern
    ///     Regular Expression used program wide to identify compatible image files
    /// </summary>
    public const string ImageSearchPattern = @"^.+((\.jpeg$)|(\.jpg$)|(\.webp$)|(\.gif$)|(\.bmp$)|(\.png$))";
}
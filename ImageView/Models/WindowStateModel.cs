namespace ImageViewer.Models;

/// <summary>
///     WindowStateModel
/// </summary>
public class WindowStateModel
{
    /// <summary>
    ///     Gets or sets the border style.
    /// </summary>
    /// <value>
    ///     The border style.
    /// </value>
    public FormBorderStyle BorderStyle { get; set; }

    /// <summary>
    ///     Gets or sets the state of the window.
    /// </summary>
    /// <value>
    ///     The state of the window.
    /// </value>
    public FormWindowState WindowState { get; set; }

    /// <summary>
    ///     Gets or sets the color of the background.
    /// </summary>
    /// <value>
    ///     The color of the background.
    /// </value>
    public Color BackgroundColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is fullscreen.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is fullscreen; otherwise, <c>false</c>.
    /// </value>
    public bool IsFullscreen { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether [cursor visible].
    /// </summary>
    /// <value>
    ///     <c>true</c> if [cursor visible]; otherwise, <c>false</c>.
    /// </value>
    public bool CursorVisible { get; set; } = true;
}
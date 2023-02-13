namespace ImageViewer.Events;

/// <summary>
/// </summary>
/// <seealso cref="System.EventArgs" />
public class IntervalEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IntervalEventArgs" /> class.
    /// </summary>
    public IntervalEventArgs()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="IntervalEventArgs" /> class.
    /// </summary>
    /// <param name="interval">The interval.</param>
    public IntervalEventArgs(int interval)
    {
        Interval = interval;
    }

    /// <summary>
    ///     Gets or sets the interval.
    /// </summary>
    /// <value>
    ///     The interval.
    /// </value>
    public int Interval { get; set; }
}
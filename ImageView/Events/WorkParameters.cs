namespace ImageViewer.Events;

/// <summary>
///     Application eventHandlers and eventArgs
/// </summary>
public class WorkParameters
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkParameters" /> class.
    /// </summary>
    public WorkParameters()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkParameters" /> class.
    /// </summary>
    /// <param name="maxSize">The maximum size.</param>
    public WorkParameters(long maxSize)
    {
        MaxSize = maxSize;
    }

    /// <summary>
    ///     Gets the maximum size.
    /// </summary>
    /// <value>
    ///     The maximum size.
    /// </value>
    public long MaxSize { get; }

    /// <summary>
    ///     Gets the empty.
    /// </summary>
    /// <value>
    ///     The empty.
    /// </value>
    public static WorkParameters Empty { get; } = new();
}
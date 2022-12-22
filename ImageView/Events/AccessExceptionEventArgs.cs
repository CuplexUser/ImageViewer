namespace ImageViewer.Events;

/// <summary>
/// </summary>
/// <seealso cref="System.EventArgs" />
public class AccessExceptionEventArgs : EventArgs
{
    /// <summary>
    ///     The exception
    /// </summary>
    private readonly Exception _exception;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AccessExceptionEventArgs" /> class.
    /// </summary>
    /// <param name="ex">The ex.</param>
    public AccessExceptionEventArgs(Exception ex)
    {
        _exception = ex;
    }

    /// <summary>
    ///     Gets the exception.
    /// </summary>
    /// <returns></returns>
    public Exception GetException()
    {
        return _exception;
    }

    /// <summary>
    ///     Gets the exception message.
    /// </summary>
    /// <returns></returns>
    public string GetExceptionMessage()
    {
        return _exception?.Message;
    }
}
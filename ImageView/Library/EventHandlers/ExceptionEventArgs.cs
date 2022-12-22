namespace ImageViewer.Library.EventHandlers;

public class ExceptionEventArgs : EventArgs
{
    public ExceptionEventArgs()
    {
    }

    public ExceptionEventArgs(Exception exception, params object[] parameters)
    {
        AdditionalPentameters = parameters;
        Exception = exception;
    }

    public Exception Exception { get; set; }

    public string FunctionName { get; set; }

    public Type SourceClass { get; set; }

    public Type TargetClass { get; set; }

    public object[] AdditionalPentameters { get; }
}
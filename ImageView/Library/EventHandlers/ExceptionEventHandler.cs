using System;

namespace ImageViewer.Library.EventHandlers
{
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public string FunctionName { get; set; }

        public Type SourceClass { get; set; }

        public Type TargetClass { get; set; }

        public object[] AdditionalPentameters { get; }

        public ExceptionEventArgs()
        {

        }

        public ExceptionEventArgs(Exception exception, params object[] parameters)
        {
            AdditionalPentameters = parameters;
            Exception = exception;
        }
    }
}
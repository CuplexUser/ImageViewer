using ImageViewer.Library.EventHandlers;

namespace ImageViewer.Interfaces
{
    public interface IExceptionEventHandler
    {
        event ExceptionEventHandler OtherExceptionEventHandler;
    }
}

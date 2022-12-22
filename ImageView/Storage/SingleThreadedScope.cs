namespace ImageViewer.Storage;

public class SingleThreadedScope : IDisposable
{
    private readonly object _lockObj = new();


    public void Dispose()
    {
    }

    public bool PerformWork(Func<bool> workAction)
    {
        lock (_lockObj)
        {
            return workAction.Invoke();
        }
    }
}
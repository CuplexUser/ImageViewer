namespace ImageViewer.Models.UserInteraction;

public class UserInteractionQuestion : UserInteractionBase
{
    public Action CancelResponse;
    public Action OkResponse;
    public event EventHandler OnQueryCompleted;

    public void Execute()
    {
        OnQueryCompleted?.Invoke(this, EventArgs.Empty);
    }
}
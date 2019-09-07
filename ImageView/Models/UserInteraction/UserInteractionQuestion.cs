using System;

namespace ImageViewer.Models.UserInteraction
{
    public class UserInteractionQuestion : UserInteractionBase
    {
        public Action OkResponse;
        public Action CancelResponse;
        public event EventHandler OnQueryCompleted;

        public void Execute()
        {
            OnQueryCompleted?.Invoke(this, new EventArgs());
        }
    }
}

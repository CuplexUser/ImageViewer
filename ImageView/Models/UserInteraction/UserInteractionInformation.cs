using System;

namespace ImageViewer.Models.UserInteraction
{
    public class UserInteractionInformation : UserInteractionBase
    {
        public event EventHandler OnQueryCompleted;

        public void Execute()
        {
            OnQueryCompleted?.Invoke(this, new EventArgs());
        }
    }
}

using System;

namespace ImageViewer.Models.UserInteraction
{
    public class UserQuestionEventArgs : EventArgs
    {
        public UserQuestionEventArgs(UserInteractionQuestion userQuestion)
        {
            UserQuestion = userQuestion;
        }

        public UserInteractionQuestion UserQuestion { get; }
    }
}

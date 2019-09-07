using System;

namespace ImageViewer.Models.UserInteraction
{
    public class UserInformationEventArgs : EventArgs
    {
        public UserInformationEventArgs(UserInteractionInformation userInformation)
        {
            UserInformation = userInformation;
        }

        public UserInteractionInformation UserInformation { get; }
    }
}

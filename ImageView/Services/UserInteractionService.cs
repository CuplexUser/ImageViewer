using System;
using System.Collections.Generic;
using System.Linq;
using ImageViewer.Models.UserInteraction;
using JetBrains.Annotations;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class UserInteractionService : ServiceBase
    {
        private readonly Queue<UserInteractionInformation> _informationQueue;
        private readonly Queue<UserInteractionQuestion> _questionQueue;
        private FormMain _formMain;

        public UserInteractionService()
        {
            _questionQueue = new Queue<UserInteractionQuestion>();
            _informationQueue = new Queue<UserInteractionInformation>();
        }

        public bool IsInitialized { get; private set; }

        public bool AnyUserQuestionsInQueue => _questionQueue.Count > 0;
        public bool AnyUserInformationInQueue => _informationQueue.Count > 0;
        public event EventHandler<UserQuestionEventArgs> UserQuestionReceived;
        public event EventHandler<UserInformationEventArgs> UserInformationReceived;

        public void Initialize(FormMain formMain)
        {
            _formMain = formMain ?? throw new ArgumentException("Form main can not be null");
            _formMain.Shown += _formMain_Shown;
        }

        private void _formMain_Shown(object sender, EventArgs e)
        {
            IsInitialized = true;

            if (UserInformationReceived != null && _informationQueue.Any())
                while (_informationQueue.Any())
                {
                    var infoItem = _informationQueue.Dequeue();
                    UserInformationReceived.Invoke(this, new UserInformationEventArgs(infoItem));
                }

            if (UserQuestionReceived != null && _questionQueue.Any())
                while (_questionQueue.Any())
                {
                    var questionItem = _questionQueue.Dequeue();
                    UserQuestionReceived.Invoke(this, new UserQuestionEventArgs(questionItem));
                }
        }

        public Queue<UserInteractionQuestion>.Enumerator GetInteractionQuestions()
        {
            return _questionQueue.GetEnumerator();
        }

        public Queue<UserInteractionInformation>.Enumerator GetInteractionInformation()
        {
            return _informationQueue.GetEnumerator();
        }

        public void RequestUserAccept(UserInteractionQuestion userAccept)
        {
            if (UserQuestionReceived == null)
            {
                _questionQueue.Enqueue(userAccept);
                return;
            }

            UserQuestionReceived.Invoke(this, new UserQuestionEventArgs(userAccept));
        }

        public void InformUser(UserInteractionInformation userInform)
        {
            if (UserInformationReceived == null)
            {
                _informationQueue.Enqueue(userInform);
                return;
            }


            UserInformationReceived?.Invoke(this, new UserInformationEventArgs(userInform));
        }
    }
}
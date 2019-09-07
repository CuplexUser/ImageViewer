using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.Models.UserInteraction;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class StartupService : ServiceBase
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly UpdateService _updateService;
        private readonly UserInteractionService _interactionService;

        public StartupService(ApplicationSettingsService applicationSettingsService, UpdateService updateService, UserInteractionService interactionService)
        {
            _applicationSettingsService = applicationSettingsService;
            _updateService = updateService;
            _interactionService = interactionService;
        }

        public void ScheduleAndRunStartupJobs()
        {
            Task.Factory.StartNew(async () =>
            {
                await RunStartupJobsAsync();
            });
        }

        private async Task RunStartupJobsAsync()
        {
            var taskList = new List<Task>();
            var settings = _applicationSettingsService.Settings;

            if (settings.AutomaticUpdateCheck)
            {
                if (settings.LastUpdateCheck.AddDays(1) < DateTime.Now)
                {
                    bool isLatestVersion = await _updateService.IsLatestVersion();
                    settings.LastUpdateCheck = DateTime.Now;
                    _applicationSettingsService.SaveSettings();

                    if (!isLatestVersion)
                    {
                        taskList.Add(new Task(EnqueueUpdateRequest));
                    }
                }
            }

            //taskList.Add(TestJob());
            foreach (var task in taskList)
            {
                task.Start();
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);
        }

        private void EnqueueUpdateRequest()
        {
            _interactionService.RequestUserAccept(new UserInteractionQuestion
            {
                Buttons = MessageBoxButtons.OKCancel,
                CancelResponse = NoUpdate,
                OkResponse = UpdateProgramJob,
                Icon = MessageBoxIcon.Question,
                Message = "There is a newer version available, do you want to download and update?",
                Label = "Update program?"
            });
        }

        private void NoUpdate()
        {
            Log.Information("User selected cancel on update request");
        }

        private void UpdateProgramJob()
        {
            Log.Information("User selected ok on update request");
            Task.Factory.StartNew(async () =>
            {
                await UpdateProgramJobAsync();
            });
        }

        [NotNull]
        private Task UpdateProgramJobAsync()
        {
            return _updateService.DownloadAndRunLatestVersionInstaller();
        }

        private async Task<bool> TestJob()
        {
            await Task.Delay(5000);
            _interactionService.InformUser(new UserInteractionInformation {Buttons = MessageBoxButtons.OK,Icon = MessageBoxIcon.Asterisk,Message = "Hello this is  a test", Label = "Test"});
            return true;
        }
    }
}

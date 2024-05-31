using GeneralToolkitLib.Configuration;
using ImageViewer.Configuration;
using ImageViewer.Services;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ImageViewer;

internal static class Program
{
    private static IContainer Container { get; set; }

    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();


    /// <summary>
    ///     The main entryModel point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        InitializeAutofac();

        if (Environment.OSVersion.Version.Major >= 6)
        {
            SetProcessDPIAware();
        }

        Application.EnableVisualStyles();
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.SetCompatibleTextRenderingDefault(true);
        bool debugMode = ApplicationBuildConfig.DebugMode;
        GlobalSettings.Settings.Initialize(Assembly.GetExecutingAssembly().GetName().Name, !debugMode);

        Log.Verbose("Application started");

        using (var scope = Container.BeginLifetimeScope())
        {
            // Begin startup async jobs
            var settingsService = scope.Resolve<ApplicationSettingsService>();
            var startupService = scope.Resolve<StartupService>();

            bool readSuccessful = settingsService.LoadSettings();

            if (!readSuccessful)
            {
                string userDataPath = GlobalSettings.Settings.GetUserDataDirectoryPath();
                Log.Error("Failed to load application settings on program load. User data path {userDataPath}", userDataPath);

                // Use Default Application Settings and save them to disk
                settingsService.RestoreSettingsToDefault();
            }


            startupService.ScheduleAndRunStartupJobs();
            Task.Delay(1000);
            try
            {
                var frmMain = scope.Resolve<FormMain>();
                Application.Run(frmMain);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Main program failureException: {Message}", ex.Message);
            }


            //settingsService.SaveSettings();
        }

        //Application.Run(new FormMain());
        Log.Information("Application ended");
    }

    private static void InitializeAutofac()
    {
        Container = AutofacConfig.CreateContainer();
    }
}
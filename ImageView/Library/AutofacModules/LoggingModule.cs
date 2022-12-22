using System.Globalization;
using Autofac;
using AutofacSerilogIntegration;
using GeneralToolkitLib.Configuration;
using Serilog;
using Serilog.Events;

namespace ImageViewer.Library.AutofacModules;

public class LoggingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var logLevel = LogEventLevel.Debug;
        string logFilePath;
        if (!ApplicationBuildConfig.DebugMode)
        {
            logLevel = LogEventLevel.Warning;
            logFilePath = ApplicationBuildConfig.ApplicationLogFilePath();
        }
        else
        {
            logFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"ImageViewer{DateTime.Today.ToString("yyyy-MM-dd")}.log");
        }

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(LogEventLevel.Debug, standardErrorFromLevel: LogEventLevel.Error, formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.File(logFilePath,
                fileSizeLimitBytes: 1048576,
                retainedFileCountLimit: 31,
                restrictedToMinimumLevel: logLevel,
                buffered: false,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}")
            .Enrich.FromLogContext()
            .MinimumLevel.Is(logLevel)
            .CreateLogger();

        builder.RegisterLogger();
    }
}
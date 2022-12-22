using Autofac;
using AutofacSerilogIntegration;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.IO;

namespace ImageView.UnitTests.Configuration
{
    public class LoggingModule : Module
    {
        private static readonly string TestLogFile = Path.Combine(Path.GetTempPath(), "testrunner.log");

        protected override void Load(ContainerBuilder builder)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Debug, standardErrorFromLevel: LogEventLevel.Error, formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.File(TestLogFile,
                    fileSizeLimitBytes: 1048576,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    buffered: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff} [{Level}] {Message}{NewLine}{Exception}{Data}")
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .CreateLogger();

            builder.RegisterLogger();
        }
    }
}
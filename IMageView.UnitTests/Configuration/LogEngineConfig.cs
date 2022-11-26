using Serilog;
using Serilog.Events;


namespace ImageView.UnitTests.Configuration
{
    internal class LogEngineConfig
    {

        private ILogger _logger;
        public LogEngineConfig()
        {

        }

        public ILogger CReateLogger()
        {
            if (_logger != null)
                return _logger;

            _logger = Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            

            return _logger;

        }

    }
}


using Serilog;
using ZimLabs.CoreLib;

namespace Demo.Common
{
    /// <summary>
    /// Provides several helper functions
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Init the logger (SeriLog)
        /// </summary>
        /// <param name="verbose">true to active a verbose log, otherwise false</param>
        public static void InitLogger(bool verbose)
        {
            const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            var baseDir = Path.Combine(Core.GetBaseDirPath(), "log");
            var logFilePath = Path.Combine(baseDir, "log_.log");

            if (verbose)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(outputTemplate: template) // Console output
                    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, outputTemplate: template) // File output
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: template) // Console output
                    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, outputTemplate: template) // File output
                    .CreateLogger();
            }
        }
    }
}
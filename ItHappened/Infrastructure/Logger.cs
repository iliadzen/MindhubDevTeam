using Serilog;

namespace ItHappened.Infrastructure
{
    public class Logger
    {
        public Logger(string logFileName)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFileName)
                .WriteTo.Console()
                .CreateLogger();
        }
        
        public static void Debug(string text) { Log.Debug(text); }
        public static void Information(string text) { Log.Information(text); }
        public static void Warning(string text) { Log.Warning(text); }
        public static void Error(string text) { Log.Error(text); }
        public static void Fatal(string text) { Log.Fatal(text); }
    }
}
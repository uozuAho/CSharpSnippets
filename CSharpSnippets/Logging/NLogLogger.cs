using NLog;
using System.Globalization;

namespace CSharpSnippets.Logging
{
    class NLogLogger : ILogger
    {
        private Logger _logger;
        private static readonly CultureInfo culture = new CultureInfo("en-US");

        public NLogLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Trace(string msg)
        {
            Log(LogLevel.Trace, msg);
        }

        public void Trace(string msg, params object[] args)
        {
            Log(LogLevel.Trace, msg, args);
        }

        public void Debug(string msg)
        {
            Log(LogLevel.Debug, msg);
        }

        public void Debug(string msg, params object[] args)
        {
            Log(LogLevel.Debug, msg, args);
        }

        public void Info(string msg)
        {
            Log(LogLevel.Info, msg);
        }

        public void Info(string msg, params object[] args)
        {
            Log(LogLevel.Info, msg, args);
        }

        public void Warn(string msg)
        {
            Log(LogLevel.Warn, msg);
        }

        public void Warn(string msg, params object[] args)
        {
            Log(LogLevel.Warn, msg, args);
        }

        public void Error(string msg)
        {
            Log(LogLevel.Error, msg);
        }

        public void Error(string msg, params object[] args)
        {
            Log(LogLevel.Error, msg, args);
        }

        public void Fatal(string msg)
        {
            Log(LogLevel.Fatal, msg);
        }

        public void Fatal(string msg, params object[] args)
        {
            Log(LogLevel.Fatal, msg, args);
        }

        public void Log(LogLevel level, string msg, params object[] args)
        {
            var e = new LogEventInfo(level, _logger.Name, culture, msg, args);
            // passing the type of this wrapper lets NLog work out the location of where 
            // this log call came from, useful for logging 'callsite' info. See 
            // http://stackoverflow.com/questions/7412156/how-to-retain-callsite-information-when-wrapping-nlog
            _logger.Log(typeof(NLogLogger), e);
        }

        public void Log(LogLevel level, string msg)
        {
            Log(level, msg, null);
        }
    }
}

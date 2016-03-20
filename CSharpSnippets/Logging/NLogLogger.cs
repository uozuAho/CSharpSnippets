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
            Trace(msg, null);
        }

        public void Trace(string msg, params object[] args)
        {
            var e = new LogEventInfo(LogLevel.Trace, _logger.Name, culture, msg, args);
            // passing the type of this wrapper lets NLog work out the location of where 
            // this log call came from, useful for logging 'callsite' info. See 
            // http://stackoverflow.com/questions/7412156/how-to-retain-callsite-information-when-wrapping-nlog
            _logger.Log(typeof(NLogLogger), e);
        }
    }
}

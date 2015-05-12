using System;
using Framework.Common.Utility;
using Framework.Common.Validation;
using log4net;
using log4net.Config;
using log4net.Core;

namespace Framework.Logging.Log4Net
{
    /// <summary>
    /// ILogger implementation using Log4Net.
    /// </summary>
    public class Logger : ILogger
    {
        private const string LoggerName = "AdoNetAppender";

        private readonly ILog _logger = null;
        private readonly IClock _clock;

        private Logger(string loggerName, IClock clock)
        {
            // Create the log4net logger
            if (_logger == null)
            {
                _logger = LogManager.GetLogger(loggerName);
            }

            _clock = clock;
        }

        public void Log(LogLevel logLevel, Exception exception, Type loggingType,
                        bool notifyAdministrator, string stringFormat, params object[] args)
        {
            ParamValidator.AssertIsNotEmpty(stringFormat, "stringFormat");

            try
            {
                LoggingEventData logData = new LoggingEventData()
                                                   {
                                                       Level = TransformToLevel(logLevel),
                                                       ExceptionString = exception != null
                                                                            ? string.Format("{0}|StackTrace: {1}",
                                                                                            exception.Message,
                                                                                            exception.StackTrace)
                                                                            : null,
                                                       ThreadName = loggingType != null
                                                                        ? loggingType.ToString()
                                                                        : null,
                                                       TimeStamp = _clock.Now,
                                                       Message = string.Format(stringFormat, args),
                                                       LoggerName = LoggerName,
                                                   };

                LoggingEvent logEvent = new LoggingEvent(logData);

                _logger.Logger.Log(logEvent);
            }
            catch
            {
                //// Don't do nothing, logger classes don't have to throw any exceptions.
            }
        }

        public void Log(Exception exception)
        {
            _logger.Error(exception);
        }

        public static void Initialize()
        {
            XmlConfigurator.Configure();
        }

        private static Level TransformToLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Information:
                    return Level.Info;
                case LogLevel.Warning:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    return Level.Debug;
            }
        }
    }
}

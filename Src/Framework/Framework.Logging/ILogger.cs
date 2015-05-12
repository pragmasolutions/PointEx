using System;

namespace Framework.Logging
{
    /// <summary>
    /// Interface for logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the system.
        /// </summary>
        /// <param name="logLevel">The level of the message.</param>
        /// <param name="exception">The exception that generated the message, if any.</param>
        /// <param name="loggingType">The type which log the exception.</param>
        /// <param name="notifyAdministrator">If set, send an email to the administrator.</param>
        /// <param name="stringFormat">A string for format string for the message.</param>
        /// <param name="args">Format data matching the previous argument.</param>
        void Log(LogLevel logLevel, Exception exception, Type loggingType, bool notifyAdministrator, string stringFormat, params object[] args);

        /// <summary>
        /// Logs a message to the system.
        /// </summary>
        /// <param name="exception">The exception that generated the message, if any.</param>
        void Log(Exception exception);
    }
}


namespace Framework.Logging
{
    /// <summary>
    /// Supported log message levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used during development and diagnostic
        /// </summary>
        Debug,

        /// <summary>
        /// Verbose status updates
        /// </summary>
        Information,

        /// <summary>
        /// Non-standard behavior that is handled but may cause issues
        /// </summary>
        Warning,

        /// <summary>
        /// Problematic behavior that degrades application performance or behavior
        /// </summary>
        Error,

        /// <summary>
        /// Fatal error that stops the application from functioning altogether
        /// </summary>
        Fatal
    }
}

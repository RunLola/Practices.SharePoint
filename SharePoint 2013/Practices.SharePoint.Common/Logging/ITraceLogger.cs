namespace Practices.SharePoint.Logging {
    using Microsoft.SharePoint.Administration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for classes that log to the trace log.
    /// </summary>
    public interface ITraceLogger {
        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, 
        /// and <paramref name="category"/> using the default severity for the category.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="category">The category of the log message.</param>
        void Trace(string message, int eventId, string category);

        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event.
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        void Trace(string message, int eventId, TraceSeverity severity, string category);
    }
}

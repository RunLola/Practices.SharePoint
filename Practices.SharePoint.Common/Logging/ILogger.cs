namespace Practices.SharePoint.Logging {
    using Microsoft.SharePoint.Administration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for logging implementations
    /// </summary>
    public interface ILogger {
        /// <summary>
        /// Writes an error message into the log, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        void LogToOperations(string message);

        /// <summary>
        /// Writes an error message into the log with specified event Id, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        void LogToOperations(string message, int eventId);

        /// <summary>
        /// Writes an error message into the log, do not use in sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        void LogToOperations(string message, EventSeverity severity);

        /// <summary>
        /// Writes an error message into the log with specified category, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="category">The category to write the message to.</param>
        void LogToOperations(string message, string category);

        /// <summary>
        /// Writes an error message into the log, don't use in sandbox.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        void LogToOperations(string message, int eventId, EventSeverity severity);

        /// <summary>
        /// Writes an error message into the log with specified event Id, works from sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        void LogToOperations(string message, int eventId, string category);

        /// <summary>
        /// Log a message with specified <paramref name="message"/>, <paramref name="eventId"/>, <paramref name="severity"/>
        /// and <paramref name="category"/>.  Don't use in sandbox.
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="severity">How serious the event is. </param>
        /// <param name="category">The category of the log message.</param>
        void LogToOperations(string message, int eventId, EventSeverity severity, string category);
        
        /// <summary>
        /// Writes information about an exception into the log to be read by operations, 
        /// works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        void LogToOperations(Exception exception);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations. Don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        void LogToOperations(Exception exception, int eventId, EventSeverity severity, string category);

        /// <summary>
        /// Writes information about an exception into the log to be read by operations, , works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log. </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage);

        /// <summary>
        /// Writes information about an exception into the log, works from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage, int eventId);

        /// <summary>
        /// Writes information about an exception into the log, don't use from sandbox.
        /// </summary>
        /// <param name="exception">The exception to write into the log to be read by operations. </param>
        /// <param name="eventId">
        /// The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
        /// administrators and IT PRo's to monitor the EventLog of a system. 
        /// </param>
        /// <param name="category">The category to write the message to.</param>
        /// <param name="severity">The severity of the exception.</param>
        /// <param name="additionalMessage">Additional information about the exception message.</param>
        void LogToOperations(Exception exception, string additionalMessage, int eventId, EventSeverity severity, string category);
    }
}

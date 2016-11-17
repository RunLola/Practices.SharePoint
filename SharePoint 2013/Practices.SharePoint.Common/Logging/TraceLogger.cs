namespace Practices.SharePoint.Logging {
    using Microsoft.SharePoint.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class that can log messages into the SharePoint trace log. 
    /// </summary>
    public class TraceLogger : ITraceLogger {
        /// <summary>
        /// Write messages into the SharePoint ULS using the default severity for
        /// the category. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the trace.</param>
        /// <param name="category">The category of the trace message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Trace(string message, int eventId, string category) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write messages into the SharePoint ULS. 
        /// </summary>
        /// <param name="message">The message to write into the log.</param>
        /// <param name="eventId">The eventId that corresponds to the trace.</param>
        /// <param name="severity">How serious the trace is.</param>
        /// <param name="category">The category of the log message.</param>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public void Trace(string message, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category) {
            throw new NotImplementedException();
        }
    }
}

namespace Practices.SharePoint.Logging {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    public class Logger : ILogger {
        public void LogToOperations(string message) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, int eventId) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, EventSeverity severity) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, string category) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, int eventId, EventSeverity severity) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, int eventId, string category) {
            throw new NotImplementedException();
        }

        public void LogToOperations(string message, int eventId, EventSeverity severity, string category) {
            throw new NotImplementedException();
        }

        public void LogToOperations(Exception exception) {
            throw new NotImplementedException();
        }

        public void LogToOperations(Exception exception, string additionalMessage) {
            throw new NotImplementedException();
        }

        public void LogToOperations(Exception exception, string additionalMessage, int eventId) {
            throw new NotImplementedException();
        }

        public void LogToOperations(Exception exception, int eventId, EventSeverity severity, string category) {
            throw new NotImplementedException();
        }

        public void LogToOperations(Exception exception, string additionalMessage, int eventId, EventSeverity severity, string category) {
            throw new NotImplementedException();
        }
    }
}

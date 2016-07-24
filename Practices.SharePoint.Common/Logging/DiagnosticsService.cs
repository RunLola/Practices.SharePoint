namespace Practices.SharePoint.Logging {
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a diagnostic logging for Windows SharePoint Services.
    /// </summary>
    [Guid("1BA36583-1EDB-4AB6-92C5-ACF18FB742AA")] 
    public class DiagnosticsService : SPDiagnosticsServiceBase {
        /// <summary>
        /// Initializes a new instance of the DiagnosticService class.
        /// </summary>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DiagnosticsService() {
        }

        /// <summary>
        /// /// Initializes a new instance of the DiagnosticService class.
        /// </summary>
        /// <param name="name">Gets or Sets the name that identifies the particular instance of the object.</param>
        /// <param name="parent">Gets the ID of the parent class that declares de object</param>

        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public DiagnosticsService(string name, SPFarm parent)
            : base(name, parent) {
        }

        /// <summary>
        /// Gets the local instance of the class and registers it.
        /// </summary>
        public static DiagnosticsService Local {
            [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get {
                return SPDiagnosticsServiceBase.GetLocal<DiagnosticsService>();
            }
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas() {
            throw new NotImplementedException();
        }
    }
}
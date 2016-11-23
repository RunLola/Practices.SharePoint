namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class FabricPageLayout {
        public SPControlMode ControlMode {
            get;
            set;
        }

        public IEnumerable<IEnumerable<Field>> Rows {
            get;
            set;
        }
        
        public class Field {
            public string InternalName {
                get;
                set;
            }

            public string Title {
                get;
                set;
            }

            public string ClassName {
                get;
                set;
            }
        }
    }
}

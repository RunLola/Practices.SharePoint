namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MemberEditor : PeopleEditor {
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            this.PickerDialogType = typeof(MemberPickerDialog);
        }
    }
}

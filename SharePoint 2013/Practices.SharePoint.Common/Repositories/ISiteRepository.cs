namespace Practices.SharePoint.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISiteRepository {

        DataTable Get(string queryString);

        DataTable Get(string queryString, uint startRow, uint maxRows);
    }
}

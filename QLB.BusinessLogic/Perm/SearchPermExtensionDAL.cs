using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLB.Info;
namespace QLB.BusinessLogic
{
    public class SearchPermExtensionDAL
    {
        public System.Data.DataTable GetTableValue(SearchPermExtension obj) {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "SearchExtentsion", obj).Tables[0];
        }
    }
}

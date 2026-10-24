using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using prjInfo;
namespace prjBusinessLogic
{
    public class GroupAddressDAL
    {
        public DataTable GetAll()
        {
            return new clsResuftAPI().GetPostTableApiExtension("MESSAGE_PKG", "GroupAddress_GetAll_Air", new { });
        }        
    }
}

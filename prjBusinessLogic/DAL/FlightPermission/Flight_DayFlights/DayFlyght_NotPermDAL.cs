using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace prjBusinessLogic
{
    public class DayFlyght_NotPermDAL
    {
        public DataTable GetAll()
        {
            //return new clsResuftAPI().GetTableApiExtension("MESSAGE_PKG", "GroupAddress_GetAll", new { });
            return new clsResuftAPI().GetTableApiExtension("MESSAGE_PKG", "DayFlightNotPerm_GetAll", new { });
        }
        public DataTable GetHisBy(string sValue)
        {
            return new clsResuftAPI().GetTableApiExtension("MESSAGE_PKG", "DayFlightNotPerm_GetHistory", new { P_VALUE = sValue });
        }
        public DataTable GetBySearch(object objSearch)
        {
            return new clsResuftAPI().GetTableApiExtension("MESSAGE_PKG", "DayFlightNotPerm_GetBySearch", objSearch);
        }
    }
}

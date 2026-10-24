using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjBusinessLogic
{
    public class QlbOutBoxDAL
    {
        public bool Insert(object obj)
        {
            var ax = Int64.Parse(new clsResuftAPI().GetPostValueApiExtension("MESSAGE_PKG", "QlbOutBox_Insert", obj).ToString());
            return ax == -1 ? false : true;
        }
        public bool UpdateStatus(string _date,string _partNo, string _messType)
        {
            var ax = Int64.Parse(new clsResuftAPI().GetPostValueApiExtension("FLIGHT_DAYFLIGHT", "SP_Update_PLAN_MESSAGE", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType }).ToString());
            return ax == -1 ? false : true;
        }
        public bool UpdateStatusNew(string _date, string _partNo, string _messType)
        {
            var ax = Int64.Parse(new clsResuftAPI().GetPostValueApiExtension("FLIGHT_DAYFLIGHT", "SP_Update_PLAN_MESSAGENew", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType }).ToString());
            return ax == -1 ? false : true;
        }
    }
}

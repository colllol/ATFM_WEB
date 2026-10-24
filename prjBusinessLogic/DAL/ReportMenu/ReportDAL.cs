using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace prjBusinessLogic.DAL
{
    public class ReportDAL
    {
        public Int64 Insert_Report (string nameReport, string addressFile, string remark)
        {
            return (Int64)new clsResuftAPI().GetValueApiExtension("DIC_PKG", "REPORT_INSERT", new { P_NAME_REPORT = nameReport, P_ADDRESS_FILE = addressFile, P_REMARK = remark });
        }
        public DataTable GetPageReport(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableApiExtension("DIC_PKG", "REPORT_GET_PAGE", new { P_PAGE_SIZE = pageSize, P_PAGE_INDEX = pageIndex, P_WHERE = where });
        }
        public DataTable GetAllReport()
        {
            return new clsResuftAPI().GetTableApiExtension("DIC_PKG", "REPORT_GET_ALL", new { });
        }
        public Int64 Update_Report(int id, string nameReport, string addressFile, string remark)
        {
            return (Int64)new clsResuftAPI().GetValueApiExtension("", "", new { P_ID = id, P_NAME_REPORT = nameReport, P_ADDRESS_FILE = addressFile, P_REMARK = remark });
        }     
        public Int64 Delete_Report(int id)
        {
            return (Int64)new clsResuftAPI().GetValueApiExtension("", "", new { P_ID = id });
        }
    }
}

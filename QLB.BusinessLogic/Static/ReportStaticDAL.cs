using System;
using System.Collections.Generic;
using System.Linq;
using ShareDLL;
using System.Data;
using QLB.Info;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class ReportStaticDAL
    {
        public DataSet GetAllReportStatic()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORT_GET_ALL");
        }
        public DataSet GetAllReportNameStatic()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORT_NAME_GET_ALL");
        }
        public DataSet GetPageReportStatic(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORT_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }

        public DataSet GetPageReportStaticExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORT_GET_PAGE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }

        public DataSet GetByIdReportStatic(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORT_GET_ID", new OracleParameter("p_ID", ID));

        }
        public void DeleteReportStatic(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORT_DELETE", new OracleParameter("p_ID", ID));
        }
        public int CreateReportStatic(ReportStatic obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORT_INSERT", obj);
        }
        public int UpdateReportStatic(ReportStatic obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORT_UPDATE", obj);
        }
    }
}

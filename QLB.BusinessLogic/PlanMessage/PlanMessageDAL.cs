using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class PlanMessageDAL
    {
        public DataTable GetPage(clsSearchPlanMessageNew obj)
        {
            return new oDataProvider().ExecuteDatase("PLANMESSAGENEW_PKG", "GetPage", obj).Tables[0];

        }
        public DataTable GetPageRealplanLetter(clsSearchRealplanLetter obj)
        {
            return new oDataProvider().ExecuteDatase("REALPLAN_PKG", "realplan_letter_get_page", obj).Tables[0];
        }

        public DataTable GetByDatePart(string _date, int _part, string mestype)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("PLANMESSAGENEW_PKG", "GetByDateAndNum", new OracleParameter("P_DATE", _date), new OracleParameter("P_ID", _part), new OracleParameter("P_MESS_TYPE", mestype)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

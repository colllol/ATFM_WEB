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

namespace QLB.BusinessLogic.Cant
{
    public class CantRealizeDAL
    {
        public DataTable GetPage(int page_size, int page_index, string where)
        {
            try
            {

                DataSet ax = new DataSet();
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", where));               
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("CANT_PKG", "GETPAGE", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            try
            {
                
                DataSet ax = new DataSet();                
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", DateTimeHelper.ConvertToDateTime(FromDate).Date));
                lis.Add(new OracleParameter("P_TO_DATE", DateTimeHelper.ConvertToDateTime(ToDate).Date));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("CANT_PKG", "CANT_REALIZE_GET_PAGE", lis.ToArray()).Tables[0];                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetById(Int64 Id)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("CANT_PKG", "CANT_REALIZE_GET_ID", new OracleParameter("p_ID", Id)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetContentByNBR(string Nbr)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("CANT_PKG.CANT_REALIZE_GETMASSAGE_NBR", new string[] { "NBR" }, new object[] { Nbr }).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

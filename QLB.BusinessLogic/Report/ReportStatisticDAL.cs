using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Oracle.DataAccess.Client;
using System.Text;
using System.Threading.Tasks;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class ReportStatisticDAL
    {

        #region FlightsGeneral

        public DataTable THSLB_Get_HHKVN(string Date)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("NGAY", Date));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "THSLB_GET_HHKVN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SUM_PUR_VN_BY_DATE(string Oper, string Date)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Oper));
                lis.Add(new OracleParameter("NGAY", Date));

                return new oDataProvider().ExecuteNonQueryForApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_PUR_VN", new { OPER = Oper, NGAY = Date }).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SUM_PUR_VN_BY_DATE_obj(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteNonQuery("REPORT_STATISTIC_PKG", "THSLB_SUM_PUR_VN", obj).ToString();
        }
        public string SUM_PUR_QT_BY_DATE(string Date)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("NGAY", Date));
                return new oDataProvider().ExecuteNonQueryForApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_PUR_QT", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SUM_QN_BY_HHK(string Oper, string Date)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Oper));
                lis.Add(new OracleParameter("NGAY", Date));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_STATISTIC_PKG", "THSLB_SUM_QN", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SUM_QT_BY_HHK(string Oper, string Date)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Oper));
                lis.Add(new OracleParameter("NGAY", Date));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_STATISTIC_PKG", "THSLB_SUM_QT", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }











        public DataTable THSLB_DAY_N(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "THSLB_DAY_N", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable THSLB_Month_N(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "THSLB_Month_N", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable THSLB_Pur_Day_N(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "THSLB_Pur_Day_N", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable THSLB_Pur_Month_N(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "THSLB_Pur_Month_N", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
        #region FlightsSummarize
        public DataTable CBTheoLMB_1(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "CBTheoLMB_1", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable CBTheoLMB_1_Sum(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "CBTheoLMB_1_Sum", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable CBTheoLMB_3(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "CBTheoLMB_3", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TheoCBay_2(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TheoCBay_2", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
        #region ForeignerFlights
        public DataTable BQTeNoSum_8(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "BQTeNoSum_8", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TKCBQTe_7(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TKCBQTe_7", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TKSLCBQTe_8(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TKSLCBQTe_8", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
        #region NoneScheduledFlights
        public DataTable ThongKe15(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe15", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TKCBDX_16(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TKCBDX_16", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
        #region OverFlights
        public DataTable By_Route_Month(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "By_Route_Month", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable FIR_HCM(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "FIR_HCM", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable FIR_HCM_1(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "FIR_HCM_1", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable FIR_HN(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "FIR_HN", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable FIR_HN_1(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "FIR_HN_1", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable OF_Oper(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "OF_Oper", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable Over_NOSC(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "Over_NOSC", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe12(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe12", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe13(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe13", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe13_O(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe13_O", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe14(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe14", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe17(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe17", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable ThongKe17_2(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "ThongKe17_2", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
        #region VietNameseFlights
        public DataTable BTNuoc_NoSum(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "BTNuoc_NoSum", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TheoCBay_VN(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TheoCBay_VN", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TKCBTNuoc(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TKCBTNuoc", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable TKSLCBQN_8(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_STATISTIC_PKG", "TKSLCBQN_8", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        #endregion
    }
}

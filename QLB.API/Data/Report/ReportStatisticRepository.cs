using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    public class ReportStatisticRepository
    {
        #region FlightsGeneral
        public ReponseReportEntity THSLB_Get_HHKVN(string Date)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().THSLB_Get_HHKVN(Date));
        }
        public ReponseReportEntity SUM_PUR_VN_BY_DATE(string Oper,string Date)
        {
            return new ReponseEntityHelper().GetString(new ReportStatisticDAL().SUM_PUR_VN_BY_DATE(Oper,Date));
        }
        public ReponseReportEntity SUM_PUR_QT_BY_DATE(string Date)
        {
            return new ReponseEntityHelper().GetString(new ReportStatisticDAL().SUM_PUR_QT_BY_DATE(Date));
        }
        public ReponseReportEntity SUM_QN_BY_HHK(string Oper, string Date)
        {
            return new ReponseEntityHelper().GetString(new ReportStatisticDAL().SUM_QN_BY_HHK(Oper, Date));
        }
        public ReponseReportEntity SUM_QT_BY_HHK(string Oper, string Date)
        {
            return new ReponseEntityHelper().GetString(new ReportStatisticDAL().SUM_QT_BY_HHK(Oper, Date));
        }


        public ReponseReportEntity THSLB_DAY_N(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().THSLB_DAY_N(FromDate, ToDate));
        }
        public ReponseReportEntity THSLB_Month_N(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().THSLB_Month_N(FromDate, ToDate));
        }
        public ReponseReportEntity THSLB_Pur_Day_N(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().THSLB_Pur_Day_N(FromDate, ToDate));
        }
        public ReponseReportEntity THSLB_Pur_Month_N(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().THSLB_Pur_Month_N(FromDate, ToDate));
        }
        #endregion
        #region FlightsSummarize
        public ReponseReportEntity CBTheoLMB_1(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().CBTheoLMB_1(FromDate, ToDate));
        }
        public ReponseReportEntity CBTheoLMB_1_Sum(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().CBTheoLMB_1_Sum(FromDate, ToDate));
        }
        public ReponseReportEntity CBTheoLMB_3(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().CBTheoLMB_3(FromDate, ToDate));
        }
        public ReponseReportEntity TheoCBay_2(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TheoCBay_2(FromDate, ToDate));
        }
        #endregion
        #region ForeignerFlights
        public ReponseReportEntity BQTeNoSum_8(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().BQTeNoSum_8(FromDate, ToDate));
        }
        public ReponseReportEntity TKCBQTe_7(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TKCBQTe_7(FromDate, ToDate));
        }
        public ReponseReportEntity TKSLCBQTe_8(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TKSLCBQTe_8(FromDate, ToDate));
        }
        #endregion
        #region NoneScheduledFlights
        public ReponseReportEntity ThongKe15(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe15(FromDate, ToDate));
        }
        public ReponseReportEntity TKCBDX_16(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TKCBDX_16(FromDate, ToDate));
        }
        #endregion
        #region OverFlights
        public ReponseReportEntity By_Route_Month(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().By_Route_Month(FromDate, ToDate));
        }
        public ReponseReportEntity FIR_HCM(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().FIR_HCM(FromDate, ToDate));
        }
        public ReponseReportEntity FIR_HCM_1(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().FIR_HCM_1(FromDate, ToDate));
        }
        public ReponseReportEntity FIR_HN(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().FIR_HN(FromDate, ToDate));
        }
        public ReponseReportEntity FIR_HN_1(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().FIR_HN_1(FromDate, ToDate));
        }
        public ReponseReportEntity OF_Oper(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().OF_Oper(FromDate, ToDate));
        }
        public ReponseReportEntity Over_NOSC(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().Over_NOSC(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe12(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe12(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe13(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe13(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe13_O(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe13_O(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe14(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe14(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe17(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe17(FromDate, ToDate));
        }
        public ReponseReportEntity ThongKe17_2(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().ThongKe17_2(FromDate, ToDate));
        }
        #endregion
        #region VietNameseFlights
        public ReponseReportEntity BTNuoc_NoSum(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().BTNuoc_NoSum(FromDate, ToDate));
        }
        public ReponseReportEntity TheoCBay_VN(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TheoCBay_VN(FromDate, ToDate));
        }
        public ReponseReportEntity TKCBTNuoc(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TKCBTNuoc(FromDate, ToDate));
        }
        public ReponseReportEntity TKSLCBQN_8(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportStatisticDAL().TKSLCBQN_8(FromDate, ToDate));
        }
        #endregion

    }
}
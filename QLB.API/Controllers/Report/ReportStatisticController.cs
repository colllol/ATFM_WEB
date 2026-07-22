using QLB.API.Models;
using System.Web.Http;
using QLB.API.Data;
using QLB.Info;
using QLB.BusinessLogic;
namespace QLB.API.Controllers
{
    public class ReportStatisticController : ApiController
    {
        //[AcceptVerbs("Post", "Put")]
        //public ReponseReportEntity SumByObj(clsSearchValue obj)
        //{
        //    return new ReponseEntityHelper().GetValueByObject<ReportStatisticDAL>(new ReportStatisticDAL().SUM_PUR_VN_BY_DATE_obj(obj));
        //}

        [AcceptVerbs("Get")]
        public ReponseReportEntity THSLB_Get_HHKVN(string Date)
        {
            return new ReportStatisticRepository().THSLB_Get_HHKVN(Date);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity SUM_PUR_VN_BY_DATE(string Oper,string Date)
        {
            return new ReportStatisticRepository().SUM_PUR_VN_BY_DATE(Oper,Date);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity SUM_PUR_QT_BY_DATE(string Date)
        {
            return new ReportStatisticRepository().SUM_PUR_QT_BY_DATE(Date);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity SUM_QN_BY_HHK(string Oper, string Date)
        {
            return new ReportStatisticRepository().SUM_QN_BY_HHK(Oper, Date);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity SUM_QT_BY_HHK(string Oper, string Date)
        {
            return new ReportStatisticRepository().SUM_QT_BY_HHK(Oper, Date);
        }



        [AcceptVerbs("Get")]
        public ReponseReportEntity THSLB_DAY_N(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().THSLB_DAY_N(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity THSLB_Month_N(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().THSLB_Month_N(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity THSLB_Pur_Day_N(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().THSLB_Pur_Day_N(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity THSLB_Pur_Month_N(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().THSLB_Pur_Month_N(FromDate, ToDate);
        }
        #region FlightsSummarize
        [AcceptVerbs("Get")]
        public ReponseReportEntity CBTheoLMB_1(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().CBTheoLMB_1(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity CBTheoLMB_1_Sum(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().CBTheoLMB_1_Sum(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity CBTheoLMB_3(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().CBTheoLMB_3(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TheoCBay_2(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TheoCBay_2(FromDate, ToDate);
        }
        #endregion
        #region ForeignerFlights
        [AcceptVerbs("Get")]
        public ReponseReportEntity BQTeNoSum_8(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().BQTeNoSum_8(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TKCBQTe_7(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TKCBQTe_7(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TKSLCBQTe_8(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TKSLCBQTe_8(FromDate, ToDate);
        }
        #endregion
        #region NoneScheduledFlights
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe15(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe15(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TKCBDX_16(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TKCBDX_16(FromDate, ToDate);
        }
        #endregion
        #region OverFlights
        [AcceptVerbs("Get")]
        public ReponseReportEntity By_Route_Month(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().By_Route_Month(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity FIR_HCM(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().FIR_HCM(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity FIR_HCM_1(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().FIR_HCM_1(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity FIR_HN(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().FIR_HN(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity FIR_HN_1(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().FIR_HN_1(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity OF_Oper(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().OF_Oper(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity Over_NOSC(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().Over_NOSC(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe12(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe12(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe13(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe13(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe13_O(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe13_O(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe14(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe14(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe17(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe17(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity ThongKe17_2(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().ThongKe17_2(FromDate, ToDate);
        }

        #endregion
        #region VietNameseFlights
        [AcceptVerbs("Get")]
        public ReponseReportEntity BTNuoc_NoSum(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().BTNuoc_NoSum(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TheoCBay_VN(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TheoCBay_VN(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TKCBTNuoc(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TKCBTNuoc(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity TKSLCBQN_8(string FromDate, string ToDate)
        {
            return new ReportStatisticRepository().TKSLCBQN_8(FromDate, ToDate);
        }
        #endregion

    }
}

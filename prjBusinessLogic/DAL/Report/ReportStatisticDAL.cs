using prjInfo;
using System;
using System.Data;


namespace prjBusinessLogic
{
    public class ReportStatisticDAL
    {
        public DataTable THSLB_GET_SCHEDULE_SLB(string _fromdate, string _todate,string _aecode, string _hourBegin, string _hourEnd, string _type)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_SCHEDULE_SLB", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd, P_TYPE = _type });
        }
        public DataTable THSLB_GET_FINISH_SLB(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd, string _type)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_SLB", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd, P_TYPE = _type });
        }




        public DataTable THSLB_GET_FINISH_KHUNGGIO(string _fromdate, string _todate, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_KHUNGGIO", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd });
        }

        public string THSLB_GET_FINISH_CAT_ATD(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_CAT_ATD", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }
        public string THSLB_GET_FINISH_HA_ATD(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_HA_ATD", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }

        public string THSLB_GET_FINISH_CAT_ATA(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_CAT_ATA", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }
        public string THSLB_GET_FINISH_HA_ATA(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_FINISH_HA_ATA", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }


        public DataTable THSLB_GET_KHB_KHUNGGIO(string _fromdate, string _todate, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_KHUNGGIO", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd });
        }

        public string THSLB_GET_KHB_CAT_ETD(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_CAT_ETD", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }
        public string THSLB_GET_KHB_HA_ETD(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_HA_ETD", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }

        public string THSLB_GET_KHB_CAT_ETA(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_CAT_ETA", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }
        public string THSLB_GET_KHB_HA_ETA(string _fromdate, string _todate, string _aecode, string _oper, string _hourBegin, string _hourEnd)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_HA_ETA", new { P_FROMDATE = _fromdate, P_TODATE = _todate, P_AECODE = _aecode, P_OPER = _oper, P_HOURBEGIN = _hourBegin, P_HOUREND = _hourEnd }).ToString();
        }
             
            

        public DataTable THSLB_GET_GIOIHAN_SANBAY(string _code)
        {
            try
            {
                return new clsResuftAPI().GetPostTableApiExtension("REPORT_DHB", "DHB_GET_GIOIHAN_SANBAY", new { P_CODE = _code });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string THSLB_GET_KHB_DUTHAO(string _fromdate, string _aecode, string _oper)
        {
            try
            {
                return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_DUTHAO", new { P_FROMDATE = _fromdate, P_AECODE = _aecode, P_OPER = _oper }).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string THSLB_GET_KHB_THUCTE(string _fromdate, string _aecode, string _oper)
        {
            try
            {
                return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_KHB_THUCTE", new { P_FROMDATE = _fromdate, P_AECODE = _aecode, P_OPER = _oper }).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        #region BAO CAO NGAY
        public DataTable THSLB_GET_FINISH_BAY_QN(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_DHB", "DHB_Fun_HQN_SumQN_LD", new { P_FROMDATE = _fromdate, P_TODATE = _todate});
        }
        public DataTable THSLB_GET_FINISH_BAY_QT(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_DHB", "DHB_Fun_HQN_SumQT_LD", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }
        public string THSLB_GET_SLB_HQT(string _fromdate, string _todate)
        {
            try
            {
                return new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB_Fun_SumHQT_LD", new { P_FROMDATE = _fromdate, P_TODATE = _todate }).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        public DataTable DHB_GET_SLB_HHKVN_KTG(string _mien,string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetPostTableApiExtension("REPORT_DHB", "DHB_GET_SLB_HHKVN_KTG", new { P_MIEN= _mien, P_FROMDATE = _fromdate, P_TODATE = _todate });
        }


        #region table finish flight by search
        /// <summary>
        /// Ham lay du lieu bang finishedFlight theo search object
        /// </summary>
        /// <param name="p_FromDate">Ngay lay du lieu</param>
        /// <param name="p_ToDate">Ngay lay du lieu</param>
        /// <param name="p_Domistic">Quoc noi hoac quoc te (value 0 or 1)</param>
        /// <param name="p_FirHn">Fir ha noi (value 0 or 1)</param>
        /// <param name="p_firHcm">Fir hcm (value 0 or 1)</param>
        /// <param name="p_FirAll">Ca 2 fir (value 0 or 1)</param>
        /// <param name="p_Oper">Theo hang~ (value: full-> null or '0'; 1 hang~: oper= 'oper_id hang')</param>
        /// <param name="p_Pupose">giong p_oper</param>
        /// <returns></returns>
        public DataTable GetTableFinishBySearch(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "GetTableFinishedFlight", obj);
        }
        #endregion

        #region FlightsGeneral
        /* Sum Month*/
       
        /* Sum Month*/
        public string SumFirHn(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "SumFirHn", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }
        
        public string SumFirHcm(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "SumFirHcm", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-")});
            return ax.ToString();
        }
        

        /// <summary>
        /// Edit by Chuyennt
        /// </summary>
        /// <param name="17/09/2018"></param>
        /// <returns></returns>
        #region BAO CAO TONG HOP THEO NGAY
        public DataTable THSLB_Get_HHKVN(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_HHKVN", new { FROMDATE = _fromdate, TODATE = _todate });
        }
        public DataTable THSLB_GET_HHKQT(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_HHKQT", new { FROMDATE = _fromdate, TODATE = _todate });
        }
        public DataTable THSLB_Get_HHKVN_MONTH(string _month)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_HHKVN_MONTH", new { MONTH = _month });
        }
        public DataTable THSLB_GET_HHKQT_MONTH(string _month)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "THSLB_GET_HHKQT", new { MONTH = _month});
        }
        public string THSLB_PUR_QT(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_PUR_QT", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string THSLB_SUM_PUR_QT(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_PUR_QT", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string SUM_QN_BY_HHK(string _oper, string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_QN", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate }).ToString();
        }
        public string SUM_QT_BY_HHK(string _oper, string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_QT", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate }).ToString();
        }
        public string SUM_PUR_QN_BY_OPERDATE(string _oper, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "THSLB_SUM_PUR_VN", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string Sum_Of_Day(string _fromdate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Sum_Of_Day", new { P_FROMDATE = _fromdate.Replace("/", "-") });
            return ax.ToString();
        }
        public string Sum_Of_Month(string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Sum_Of_Month", new { P_MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        #endregion

        #region BAO CAO TONG HOP THEO THANG
        public string Thslb_Pur_Qt_Month(string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Thslb_Pur_Qt_Month", new { MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        public string Thslb_Sum_Pur_Qt_Month(string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Thslb_Sum_Pur_Qt_Month", new { MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        public string Thslb_Sum_Pur_Vn_Month(string Oper, string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Thslb_Sum_Pur_Vn_Month", new { OPER = Oper, MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        public string Thslb_Sum_Qn_Month(string Oper, string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Thslb_Sum_Qn_Month", new { OPER = Oper, MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        public string Thslb_Sum_Qt_Month(string Oper, string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Thslb_Sum_Qt_Month", new { OPER = Oper, MONTH = _month.Replace("/", "-") });
            return ax.ToString();
        }
        public string SumFirHnMonth(string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "SumFirHnMonth", new { P_MONTH = _month.Replace("/", "-")});
            return ax.ToString();
        }
        public string SumFirHcmMonth(string _month)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "SumFirHcmMonth", new { P_MONTH = _month.Replace("/", "-")});
            return ax.ToString();
        }
        #endregion

        /// <summary>
        /// Chuyennt
        /// </summary>
        /// <param name="17/09/2018"></param>
        /// <returns></returns>


        public DataTable THSLB_DAY_N(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/THSLB_DAY_N", FromDate, ToDate);
        }
        public DataTable THSLB_Month_N(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/THSLB_Month_N", FromDate, ToDate);
        }
        public DataTable THSLB_Pur_Day_N(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/THSLB_Pur_Day_N", FromDate, ToDate);
        }
        public DataTable THSLB_Pur_Month_N(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/THSLB_Pur_Month_N", FromDate, ToDate);
        }
        #endregion
        #region FlightsStatistic
        public DataTable TKSLCBAYYDD_5(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TKSLCBAYYDD_5", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TKTheoNVB_6(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TKTheoNVB_6", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable Thongke11(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "Thongke11", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        #endregion
        #region FlightsSummarize
        public DataTable CBTheoLMB_1(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/CBTheoLMB_1", FromDate, ToDate);
        }
        public DataTable TheoCBay_2(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/TheoCBay_2", FromDate, ToDate);
        }
        public DataTable CBTheoLMB_3(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/CBTheoLMB_3", FromDate, ToDate);
        }
        public DataTable CBTheoLMB_1_Sum(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/CBTheoLMB_1_Sum", FromDate, ToDate);
        }
        #endregion
        #region ForeignerFlights
        //public DataTable BQTeNoSum_8(string FromDate, string ToDate)
        //{
        //    return new clsResuftAPI().GetReport("api/ReportStatistic/BQTeNoSum_8", FromDate, ToDate);
        //}
        public DataTable BQTeNoSum_8(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "BQTeNoSum_8", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TKCBQTe_7(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/TKCBQTe_7", FromDate, ToDate);
        }
        public DataTable TKSLCBQTe_8(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/TKSLCBQTe_8", FromDate, ToDate);
        }

        #endregion
        #region NoneScheduledFlights
        public DataTable ThongKe15(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe15", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TKCBDX_16(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TKCBDX_16", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        #endregion
        #region OverFlights
        public DataTable By_Route_Month(string P_Month,string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "By_Route_Month", new { P_MONTH = P_Month.Replace("/", "-"), P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
        }
        public DataTable FIR_HCM(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "FIR_HCM", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable FIR_HCM_1(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "FIR_HCM_1", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable FIR_HN(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "FIR_HN", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable FIR_HN_1(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "FIR_HN_1", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable OF_Oper(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/OF_Oper", FromDate, ToDate);
        }
        public DataTable Over_NOSC(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportStatistic/Over_NOSC", FromDate, ToDate);
        }
        public string Over_NOSC_Sum(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Over_NOSC_Sum", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }
        public string Over_NOSC_FirHn(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Over_NOSC_FirHn", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }
        public string Over_NOSC_FirHcm(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Over_NOSC_FirHcm", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }
        public DataTable ThongKe12(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe12", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe12_1(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe12_1", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe13(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe13", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe13_O(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe13_O", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe14(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe14", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe17(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe17", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable ThongKe17_2(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "ThongKe17_2", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        #endregion
        #region VietNameseFlights
        public DataTable BTNuoc_NoSum(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "BTNuoc_NoSum", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TheoCBay_VN(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TheoCBay_VN", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TKCBTNuoc(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TKCBTNuoc", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        public DataTable TKSLCBQN_8(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "TKSLCBQN_8", new { P_FROMDATE = FromDate, P_TODATE = ToDate });
            return ax;
        }
        #endregion

    }
}

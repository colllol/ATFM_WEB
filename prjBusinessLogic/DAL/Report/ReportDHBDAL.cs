using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using prjInfo;
namespace prjBusinessLogic
{
    public class ReportDHBDAL
    {

        public DataTable REPORTS_GET_ALL()
        {
            return new clsResuftAPI().GetTableObj("api/ReportDHB/REPORTS_GET_ALL");
        }

        #region BCDHB01
        public string BCDHB01_TH_Tuan_TypeLD(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB01_TH_Tuan_TypeLD", FromDate, ToDate);           
        }
        public string BCDHB01_TH_Tuan_TypeOF(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB01_TH_Tuan_TypeOF", FromDate, ToDate);
        }
        #endregion

        #region BCDHB02

        public string BCDHB02_TH_Thang_TypeLD(string Month, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB02_TH_Thang_TypeLD", Month, FromDate, ToDate);
        }
        public string BCDHB02_TH_Thang_TypeOF(string Month, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB02_TH_Thang_TypeOF", Month,FromDate, ToDate);
        }

        public string BCDHB02_TH_ThangTruoc_TypeLD(string Month)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB02_TH_ThangTruoc_TypeLD", Month);
        }
        public string BCDHB02_TH_ThangTruoc_TypeOF(string Month)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/BCDHB02_TH_ThangTruoc_TypeOF", Month);
        }

        public string GetDaThucHien_RP_Total(string _valTuanLD, string _valTuanOF)
        {
            string _return = null;
            try
            {
                double _total = 0;

                if (_valTuanLD != "0")
                {
                    _total += Convert.ToDouble(_valTuanLD);
                }

                if (_valTuanOF != "0")
                {
                    _total += Convert.ToDouble(_valTuanOF);
                }
                _return = _total.ToString();

            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        #endregion
        #region BCDHB03

        public DataTable BCDHB03_Get_HHKVN(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB03_Get_HHKVN", FromDate, ToDate);
        }

        public DataTable BCDHB03_Get_HHKQT(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB03_Get_HHKQT", FromDate, ToDate);
        }


        public string DHB03_TH_HHKVN_SumQN_Thang(string Hang, string Month, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/DHB03_TH_HHKVN_SumQN_Thang", Hang, Month, FromDate, ToDate);
        }

        public string DHB03_TH_HHKVN_SumQN_ThangTruoc(string Hang, string Month)
        {
            return new clsResuftAPI().GetValueRp_BC3("api/ReportDHB/DHB03_TH_HHKVN_SumQN_ThangTruoc", Hang, Month);
        }


        public string DHB03_TH_HHKVN_SumQT_Thang(string Hang, string Month, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/DHB03_TH_HHKVN_SumQT_Thang", Hang, Month, FromDate, ToDate);
        }

        public string DHB03_TH_HHKVN_SumQT_ThangTruoc(string Hang, string Month)
        {
            return new clsResuftAPI().GetValueRp_BC3("api/ReportDHB/DHB03_TH_HHKVN_SumQT_ThangTruoc", Hang, Month);
        }
        #endregion

        #region BCDHB04
        public DataTable DHB04_Get_DuongBay(string Route_Name)
        {
            return new clsResuftAPI().GetValue("api/ReportDHB/DHB04_Get_DuongBay", Route_Name);
        }

        public string DHB04_TH_DuongBay_Sum_Thang(string Duongbay, string Month, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueR_DB("api/ReportDHB/DHB04_TH_DuongBay_Sum_Thang", Duongbay, Month, FromDate, ToDate);
        }

        public string DHB04_TH_DuongBay_Sum_ThangTruoc(string Duongbay, string Month)
        {
            return new clsResuftAPI().GetValueR_DB("api/ReportDHB/DHB04_TH_DuongBay_Sum_ThangTruoc", Duongbay, Month);
        }
        #endregion

        #region BCDHB05
        public DataTable BCDHB05_GET_OPER_VN(string _date)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB05_Get_HHKVN", new { NGAY = _date.Replace("/", "-") });
        }

        public DataTable BCDHB05_GET_OPER_QT(string _date)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB05_Get_HHKQT", new { NGAY = _date.Replace("/", "-") });
        }
        public string BCDHB05_GET_SLB_FIRHN(string _date)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB05_Fn_THSLB_FIRHN", new { NGAY = _date.Replace("/", "-") });
            return ax.ToString();
        }

        public string BCDHB05_GET_SLB_FIRHCM(string _date)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB05_Fn_THSLB_FIRHCM", new { NGAY = _date.Replace("/", "-") });
            return ax.ToString();
        }
        public string BCDHB05_GET_SLB_BY_HHKVN(string _oper, string _date)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB05_Fn_SUM_SLB_BYOPER_VN", new { OPER = _oper, NGAY = _date.Replace("/", "-")});
            return ax.ToString();
        }
        public string BCDHB05_GET_SLB_BY_HHKQT(string _oper, string _date)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB05_Fn_SUM_SLB_BYOPER_QT", new { OPER = _oper, NGAY = _date.Replace("/", "-") });
            return ax.ToString();
        }
        #endregion





        #region BCDHB06

        public DataTable BCDHB06_Get_SLB_ByTimes(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB06_Get_THSLB_BYTIMES", FromDate, ToDate);
        }
        #endregion

        #region BCDHB10
        public DataTable BCDHB10_Get_Oper_FplVia(string hang, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB10_Get_THSLB_FIRHN", hang,FromDate, ToDate);
        }

        public string BCDHB10_Get_ThSlb_byOper(string hang,string Route,string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_FIR("api/ReportDHB/BCDHB10_GET_THSLB_FIRHN_ByOPER", hang, Route, FromDate, ToDate);
        }
        #endregion

        #region BCDHB11
        public DataTable BCDHB11_Get_Oper_FplVia(string hang, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB11_Get_THSLB_FIRHCM", hang, FromDate, ToDate);
        }

        public string BCDHB11_Get_ThSlb_byOper(string hang, string Route, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_FIR("api/ReportDHB/BCDHB11_GET_THSLB_FIRHCM_ByOPER", hang, Route, FromDate, ToDate);
        }
        #endregion

        #region BCDHB12
        public DataTable BCDHB12_Get_Oper_Craft(string Craft, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReportByCraft("api/ReportDHB/BCDHB12_Get_THSLB_FIRHN", Craft, FromDate, ToDate);
        }

        public string BCDHB12_Get_ThSlb_byCraft(string Craft, string Route, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_ByCraft("api/ReportDHB/BCDHB12_GET_THSLB_FIRHN_ByCraft", Craft, Route, FromDate, ToDate);
        }
        #endregion

        #region BCDHB13
        public DataTable BCDHB13_Get_Oper_Craft(string Craft, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReportByCraft("api/ReportDHB/BCDHB13_Get_THSLB_FIRHCM", Craft, FromDate, ToDate);
        }

        public string BCDHB13_Get_ThSlb_byCraft(string Craft, string Route, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_ByCraft("api/ReportDHB/BCDHB13_GET_THSLB_FIRHCM_ByCraft", Craft, Route, FromDate, ToDate);
        }
        #endregion


        #region BCDHB14

        public string SumFirHn(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "SumFirHnBySC", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }

        public string SumFirHcm(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "SumFirHcmBySC", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }

        public string SumFirHnByNo(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "SumFirHnByNo", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }

        public string SumFirHcmByNo(string FromDate, string ToDate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "SumFirHcmByNo", new { P_FROMDATE = FromDate.Replace("/", "-"), P_TODATE = ToDate.Replace("/", "-") });
            return ax.ToString();
        }
        #endregion


        #region BCDHB15
       
        public DataTable BCDHB15_GET_FROMAIR_BY_HHK(string _oper, string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB15_GET_FROMAIR_BYOPER", new { P_OPER = _oper, P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public string BCDHB15_GET_SLBQN_BY_HHK_SB(string _oper,string _san, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB15_THSLB_SUM_QN", new { OPER = _oper, FROMAIRP = _san, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        public string BCDHB15_GET_SLBQT_BY_HHK_SB(string _oper, string _san, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB15_THSLB_SUM_QT", new { OPER = _oper, FROMAIRP = _san, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        #endregion

        #region BCDHB16
        public DataTable DHB16_Get_HHKVN_BAYQN(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB16_Get_HHKVN_BAYQN", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public DataTable DHB16_Get_HHKVN_BAYQT(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB16_Get_HHKVN_BAYQT", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public string DHB16_THSLB_SUM_QT(string _oper, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB16_THSLB_SUM_QT", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        public string DHB16_THSLB_SUM_QN(string _oper, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB16_THSLB_SUM_QN", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        #endregion



        #region BCDHB17
        public DataTable DHB17_Get_HHKQT_BAYQT(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB17_Get_HHKQT_BAYQT", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public DataTable DHB17_Get_HHKQT_BAYQN(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB17_Get_HHKQT_BAYQN", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }
        public string DHB17_THSLB_SUM_QT(string _oper, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB17_THSLB_SUM_QT", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        public string DHB17_THSLB_SUM_QN(string _oper, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB17_THSLB_SUM_QN", new { OPER = _oper, FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        #endregion

        #region BCDHB18

        public string DHB18_THSLB_SUM_QN_MB(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QN_MB", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string DHB18_THSLB_SUM_QT_MB(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QT_MB", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        public string DHB18_THSLB_SUM_QN_MT(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QN_MT", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string DHB18_THSLB_SUM_QT_MT(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QT_MT", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }

        public string DHB18_THSLB_SUM_QN_MN(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QN_MN", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }
        public string DHB18_THSLB_SUM_QT_MN(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB18_THSLB_SUM_QT_MN", new { FROMDATE = _fromdate, TODATE = _todate });
            return ax.ToString();
        }



        #endregion

        #region BCDHB19
        public DataTable DHB19_Get_FLIGHTDATE(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB19_Get_FLIGHTDATE", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public string DHB19_THSLB_SUM_CBDI_BYSB(string _fromair, string _fromdate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB19_THSLB_SUM_CBDI_BYSB", new { FROMAIR = _fromair, FROMDATE = _fromdate });
            return ax.ToString();
        }
        public string DHB19_THSLB_SUM_CBDEN_BYSB(string _toair,string _fromdate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB19_THSLB_SUM_CBDEN_BYSB", new { TOAIR = _toair, FROMDATE = _fromdate });
            return ax.ToString();
        }
        #endregion


        #region BCDHB20
        public DataTable DHB20_Get_MONTH_FLIGHTDATE(string _fromdate, string _todate)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_DHB", "DHB20_Get_MONTH_FLIGHTDATE", new { P_FROMDATE = _fromdate, P_TODATE = _todate });
        }

        public string DHB20_Fn_THSLB_FIRHN(string _month, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB20_Fn_THSLB_FIRHN", new { P_MONTH = _month, P_FROMDATE = _fromdate.Replace("/", "-"), P_TODATE= _todate.Replace("/", "-") });
            return ax.ToString();
        }

        public string DHB20_Fn_THSLB_FIRHCM(string _month, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB20_Fn_THSLB_FIRHCM", new { P_MONTH = _month, P_FROMDATE = _fromdate.Replace("/", "-"), P_TODATE = _todate.Replace("/", "-") });
            return ax.ToString();
        }

        #endregion


        #region BCDHB24

        public string DHB24_Fn_THSLB_BY_OPER_QN(string _oper,string _from, string _month, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB24_Fn_THSLB_BY_OPER_QN", new { OPER= _oper, FROMAIR= _from, P_MONTH = _month, P_FROMDATE = _fromdate.Replace("/", "-"), P_TODATE = _todate.Replace("/", "-") });
            return ax.ToString();
        }

        public string DHB24_Fn_THSLB_BY_OPER_QT(string _oper, string _from, string _month, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB24_Fn_THSLB_BY_OPER_QT", new { OPER = _oper, FROMAIR = _from, P_MONTH = _month, P_FROMDATE = _fromdate.Replace("/", "-"), P_TODATE = _todate.Replace("/", "-") });
            return ax.ToString();
        }

        public string DHB24_Fn_THSLB_BY_OPER_QTV(string _oper, string _from, string _month, string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB24_Fn_THSLB_BY_OPER_QTV", new { OPER = _oper, FROMAIR = _from, P_MONTH = _month, P_FROMDATE = _fromdate.Replace("/", "-"), P_TODATE = _todate.Replace("/", "-") });
            return ax.ToString();
        }
        #endregion






        public DataTable BCDHB05(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB05", obj);
        }
        public DataTable BCDHB06(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB06", obj);
        }
      
        public DataTable BCDHB07(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB07", FromDate, ToDate);
        }
                
        public DataTable BCDHB08(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB08", FromDate, ToDate);
        }
        public DataTable BCDHB09(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB09_Get_THSLB_BY2FIRHN_HCM", FromDate, ToDate);
        }
        public DataTable BCDHB10(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB10", FromDate, ToDate);
        }
        public DataTable BCDHB11(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB11", FromDate, ToDate);
        }
        public DataTable BCDHB12(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB12", FromDate, ToDate);
        }
        public DataTable BCDHB13(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/BCDHB13", FromDate, ToDate);
        }
        public DataTable BCDHB14(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB14", obj);
        }
        public DataTable BCDHB15(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB15", obj);
        }
        public DataTable BCDHB16(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB16", obj);
        }
        public DataTable BCDHB17(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB17", obj);
        }
        public DataTable BCDHB18(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB18", obj);
        }
        public DataTable BCDHB19(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB19", obj);
        }
        public DataTable BCDHB20(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB20", obj);
        }
        public DataTable BCDHB21(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB21", obj);
        }
        public DataTable BCDHB22(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB22", obj);
        }
        public DataTable BCDHB23(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB23", obj);
        }
        public DataTable BCDHB24(clsSearchValue obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/ReportDHB/BCDHB24", obj);
        }

        public DataTable Char01(string FromDate,string _areo)
        {
            var ax = new clsResuftAPI().GetPostTableApiExtension("REPORT_PKG", "ThSlb_Char_Day", new { P_DATE = FromDate, P_AERO = _areo });
            return ax;
        }
        #region PHAN MOI ADD BY NVTHAI
        public string DHB_HQN_SumQN_MUCDICH(string Hang, string Pupose, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_Pupose("api/ReportDHB/DHB_TH_HQN_SumQN_Pupose", Hang, Pupose, FromDate, ToDate);
        }

        public string DHB_HQN_SumQT_MUCDICH(string Hang, string Pupose, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_Pupose("api/ReportDHB/DHB_TH_HQN_SumQT_Pupose", Hang, Pupose, FromDate, ToDate);
        }

        public string DHB_SumTotal(string _fromdate, string _todate)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("REPORT_DHB", "DHB_Fun_SumTotal_LD", new { P_FromDate = _fromdate, P_ToDate = _todate });
            return ax.ToString();
        }

        public string DHB_SumTotalLD(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport("api/ReportDHB/DHB_SumTotal",FromDate, ToDate);
        }

        public DataTable DHB_HQT_SumTotal_LD(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_Get_HQT_SumTotal_LD", FromDate, ToDate);
        }

        public DataTable DHB_Fun_HQT_SumTotal_QN_LD(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_Fun_HQT_SumTotal_QN_LD", FromDate, ToDate);
        }

        public string DHB_Fun_SUM_CBQN_BYSB_DAL(string from, string oper, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_air("api/ReportDHB/DHB_Fun_SUM_CBQN_BYSB", from, oper, FromDate, ToDate);
        }

        public string DHB_Fun_SUM_CBQT_BYSB_DAL(string to, string oper, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_air("api/ReportDHB/DHB_Fun_SUM_CBQT_BYSB", to, oper, FromDate, ToDate);
        }

        public string DHB_Fun_SUM_QNDIQN_BYSB_DAL(string to, string oper, string Month)
        {
            return new clsResuftAPI().GetValueReport_Month("api/ReportDHB/DHB_Fun_SUM_QNDIQN_BYSB", to, oper, Month);
        }

        public string DHB_Fun_SUM_QNDIQT_BYSB_DAL(string to, string oper, string Month)
        {
            return new clsResuftAPI().GetValueReport_Month("api/ReportDHB/DHB_Fun_SUM_QNDIQT_BYSB", to, oper, Month);
        }

        public string DHB_Fun_SUM_QTVE_BYSB_DAL(string to, string oper, string Month)
        {
            return new clsResuftAPI().GetValueReport_Month("api/ReportDHB/DHB_Fun_SUM_QTVE_BYSB", to, oper, Month);
        }


        public DataTable DHB_GET_SB_BYMIEN(string Mien)
        {
            return new clsResuftAPI().GetReportMien("api/ReportDHB/DHB_GET_SB_BYMIEN", Mien);
        }

        public DataTable DHB_GET_SLB_HHKVN(string Mien,string Month)
        {
            return new clsResuftAPI().GetReportMien("api/ReportDHB/DHB_GET_SLB_HHKVN", Mien, Month);
        }

        public string DHB_Fun_SUM_CBQT_BY_MIEN(string Mien, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_Mien("api/ReportDHB/DHB_Fun_SUM_CBQT_BY_MIEN", Mien, FromDate, ToDate);
        }
        public string DHB_Fun_SUM_CBQN_BY_MIEN(string Mien, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_Mien("api/ReportDHB/DHB_Fun_SUM_CBQN_BY_MIEN", Mien, FromDate, ToDate);
        }

        public string DHB_Fun_SUM_CBQN_BY_MIEN_HA(string Mien, string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetValueReport_Mien("api/ReportDHB/DHB_Fun_SUM_CBQN_BY_MIEN_HA", Mien, FromDate, ToDate);
        }

        public DataTable DHB_GET_SLB_HHKQT_OF(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_GET_SLB_HHKQT_OF",Month);
        }
        public DataTable DHB_Fun_List_OF_2FIR_HN_HCM(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_2FIR_HN_HCM", Month);
        }

        public DataTable DHB_Fun_List_OF_FIR_HN(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_FIR_HN", Month);
        }

        public DataTable DHB_Fun_List_OF_FIR_HCM(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_FIR_HCM", Month);
        }

        public DataTable DHB_Fun_List_OF_FIR_HN_CRAFT(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_FIR_HN_CRAFT", Month);
        }

        public DataTable DHB_Fun_List_OF_FIR_HCM_CRAFT(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_FIR_HCM_CRAFT", Month);
        }

        public DataTable DHB_Fun_List_OF_VIP_QS(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_List_OF_VIP_QS", Month);
        }
        public DataTable DHB_Fun_SLB_OF(string Month)
        {
            return new clsResuftAPI().GetReportMonth("api/ReportDHB/DHB_Fun_SLB_OF", Month);
        }

        public DataTable DHB_Fun_SLB_SumTotal_OF(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_Fun_SLB_SumTotal_OF", FromDate, ToDate);
        }

        public DataTable DHB_GET_SLB_HHKVN_BAY_QN_QT(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_GET_SLB_HHKVN_BAY_QN_QT", FromDate, ToDate);
        }

        public DataTable DHB_GET_SLB_HHKVN_DI_DEN_MIEN(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_GET_SLB_HHKVN_DI_DEN_MIEN", FromDate, ToDate);
        }

        public DataTable GET_ALL_ROUTE_NAME()
        {
            return new clsResuftAPI().GetTableObj("api/ReportDHB/GET_ALL_ROUTE_NAME");
        }
        public DataTable DHB_GET_SLB_BY_VIA_MONTH(string FromDate, string ToDate)
        {
            return new clsResuftAPI().GetReport("api/ReportDHB/DHB_GET_SLB_BY_VIA", FromDate, ToDate);
        }
        #endregion
    }
}

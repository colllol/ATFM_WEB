using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    public class ReportDHBRepository
    {

        public ReponseReportEntity REPORT_GET_ALL_REPORTS()
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().REPORT_GET_ALL_REPORTS());
        }

        #region DHB01  --Lay du lieu theo tuan     
        public ReponseReportEntity BCDHB01_TH_Tuan_TypeLD(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB01_TH_Tuan_TypeLD(FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB01_TH_Tuan_TypeOF(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB01_TH_Tuan_TypeOF(FromDate, ToDate));
        }

        #endregion

        #region DHB02 --Lay du lieu theo thang
        public ReponseReportEntity BCDHB02_TH_Thang_TypeLD(string MONTH,string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB02_TH_Thang_TypeLD(MONTH,FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB02_TH_Thang_TypeOF(string MONTH, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB02_TH_Thang_TypeOF(MONTH,FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB02_TH_ThangTruoc_TypeLD(string MONTH)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB02_TH_ThangTruoc_TypeLD(MONTH));
        }

        public ReponseReportEntity BCDHB02_TH_ThangTruoc_TypeOF(string MONTH)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB02_TH_ThangTruoc_TypeOF(MONTH));
        }
        #endregion

        #region DHB03 --Lay hang hk theo khoang thoi gian

        public ReponseReportEntity BCDHB03_Get_HHKVN(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB03_Get_HHKVN(FromDate, ToDate));
        }
        public ReponseReportEntity BCDHB03_Get_HHKQT(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB03_Get_HHKQT(FromDate, ToDate));
        }

        public ReponseReportEntity DHB03_TH_HHKVN_SumQN_Thang(string Hang, string Month, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB03_TH_HHKVN_SumQN_Thang(Hang, Month, FromDate, ToDate));
        }

        public ReponseReportEntity DHB03_TH_HHKVN_SumQN_ThangTruoc(string Hang, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB03_TH_HHKVN_SumQN_ThangTruoc(Hang, Month));
        }

        public ReponseReportEntity DHB03_TH_HHKVN_SumQT_Thang(string Hang, string Month, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB03_TH_HHKVN_SumQT_Thang(Hang, Month, FromDate, ToDate));
        }

        public ReponseReportEntity DHB03_TH_HHKVN_SumQT_ThangTruoc(string Hang, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB03_TH_HHKVN_SumQT_ThangTruoc(Hang, Month));
        }
        #endregion

        #region BCDHB04
        public ReponseReportEntity DHB04_Get_DuongBay(string ROUTE_NAME)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB04_Get_DuongBay(ROUTE_NAME));
        }

        public ReponseReportEntity DHB04_Get_DuongBay_ThangTruoc(string ROUTE_NAME, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB04_TH_DuongBay_Sum_ThangTruoc(ROUTE_NAME, Month));
        }

        public ReponseReportEntity DHB04_Get_DuongBay_Thang(string ROUTE_NAME, string Month, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB04_TH_DuongBay_Sum_Thang(ROUTE_NAME, Month, FromDate, ToDate));
        }
        #endregion

        #region BCDHB06
        public ReponseReportEntity BCDHB06_THSLB_BYTIMES(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB06_THSLB_BYTIMES(FromDate, ToDate));
        }
        #endregion

        #region BCDHB09
        public ReponseReportEntity BCDHB09_Get_THSLB_BY2FIRHN_HCM(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB09_THSLB_BY2FIRHN_HCM(FromDate, ToDate));
        }
        #endregion

        #region BCDHB10
        public ReponseReportEntity BCDHB10_THSLB_FIRHN(string Oper, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB10_THSLB_FIRHN(Oper,FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB10_GET_THSLB_FIRHN_BYOPER(string Oper,string Route, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB10_GET_THSLB_FIRHN_BYOPER(Oper, Route, FromDate, ToDate));
        }
        #endregion


        #region BCDHB11
        public ReponseReportEntity BCDHB11_THSLB_FIRHCM(string Oper, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB11_THSLB_FIRHCM(Oper, FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB11_GET_THSLB_FIRHCM_BYOPER(string Oper, string Route, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB11_GET_THSLB_FIRHCM_BYOPER(Oper, Route, FromDate, ToDate));
        }
        #endregion

        #region BCDHB12
        public ReponseReportEntity BCDHB12_THSLB_FIRHN(string Craft, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB12_THSLB_FIRHN(Craft, FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB12_GET_THSLB_FIRHN_BYCRAFT(string Craft, string Route, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB12_GET_THSLB_FIRHN_BYCRAFT(Craft, Route, FromDate, ToDate));
        }
        #endregion

        #region BCDHB13
        public ReponseReportEntity BCDHB13_THSLB_FIRHCM(string Craft, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB13_THSLB_FIRHCM(Craft, FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB13_GET_THSLB_FIRHCM_BYCRAFT(string Craft, string Route, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB13_GET_THSLB_FIRHCM_BYCRAFT(Craft, Route, FromDate, ToDate));
        }
        #endregion

        #region BCDHB14
       
        public ReponseReportEntity BCDHB14_GET_THSLB_FIRHN(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB14_GET_THSLB_FIRHN(FromDate, ToDate));
        }

        public ReponseReportEntity BCDHB14_GET_THSLB_FIRHCM(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().BCDHB14_GET_THSLB_FIRHCM(FromDate, ToDate));
        }

        #endregion





        public ReponseReportEntity BCDHB02(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB02(FromDate, ToDate));
        }
        public ReponseEntity BCDHB03(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB03(obj));
        }
        public ReponseEntity BCDHB04(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB04(obj));
        }
        public ReponseEntity BCDHB05(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB05(obj));
        }
        public ReponseEntity BCDHB06(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB06(obj));
        }
       
        public ReponseReportEntity BCDHB07(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB07(FromDate, ToDate));
        }

       
        public ReponseReportEntity BCDHB08(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB08(FromDate, ToDate));
        }
        public ReponseReportEntity BCDHB09(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB09(FromDate, ToDate));
        }
        public ReponseReportEntity BCDHB10(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB10(FromDate,ToDate));
        }
        public ReponseReportEntity BCDHB11(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB11(FromDate,ToDate));
        }
        public ReponseReportEntity BCDHB12(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB12(FromDate,ToDate));
        }
        public ReponseReportEntity BCDHB13(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().BCDHB13(FromDate,ToDate));
        }
        public ReponseEntity BCDHB14(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB14(obj));
        }
        public ReponseEntity BCDHB15(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB15(obj));
        }
        public ReponseEntity BCDHB16(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB16(obj));
        }
        public ReponseEntity BCDHB17(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB17(obj));
        }
        public ReponseEntity BCDHB18(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB18(obj));
        }
        public ReponseEntity BCDHB19(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB19(obj));
        }
        public ReponseEntity BCDHB20(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB20(obj));
        }
        public ReponseEntity BCDHB21(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB21(obj));
        }
        public ReponseEntity BCDHB22(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB22(obj));
        }
        public ReponseEntity BCDHB23(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB23(obj));
        }
        public ReponseEntity BCDHB24(clsSearchValue obj)
        {
            return new ReponseEntityHelper().GetReport<FinishedFlights>(new ReportDHBDAL().BCDHB24(obj));
        }

        #region PHAN MOI
        public ReponseReportEntity DHB_TH_HQN_SumQN_PuposeRepon(string Hang, string PURPOSE, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_TH_HQN_SumQN_PuposeDAL(Hang, PURPOSE, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_TH_HQN_SumQT_Pupose(string Hang, string PURPOSE, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_TH_HQN_SumQT_PuposeDAL(Hang, PURPOSE, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_SumTotal_Pupose(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_SumTotalDAL(FromDate, ToDate));
        }


        public ReponseReportEntity DHB_Get_HQT_SumTotal_LD_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Get_HQT_SumTotal_LD_DAL(FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_HQT_SumTotal_QN_LD_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_HQT_SumTotal_QN_LD_DAL(FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_SUM_CBQN_BYSB_Repon(string air, string oper, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_CBQN_BYSB_DAL(air, oper, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_SUM_CBQT_BYSB_Repon(string air, string oper, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_CBQT_BYSB_DAL(air, oper, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_SUM_QNDIQN_BYSB_Repon(string air, string oper, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_QNDIQN_BYSB_DAL(air, oper, Month));
        }
        public ReponseReportEntity DHB_Fun_SUM_QNDIQT_BYSB_Repon(string air, string oper, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_QNDIQT_BYSB_DAL(air, oper, Month));
        }
        public ReponseReportEntity DHB_Fun_SUM_QTVE_BYSB_Repon(string air, string oper, string Month)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_QTVE_BYSB_DAL(air, oper, Month));
        }

        public ReponseReportEntity DHB_GET_SB_BYMIEN_Repon(string Mien)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_GET_SB_BYMIEN_DAL(Mien));
        }

        public ReponseReportEntity DHB_GET_SLB_HHKVN_Repon(string Mien,string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_GET_SLB_HHKVN_DAL(Mien, Month));
        }


        public ReponseReportEntity DHB_Fun_SUM_CBQT_BY_MIEN_Repon(string Mien, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_CBQT_BY_MIEN_DAL(Mien, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_SUM_CBQN_BY_MIEN_Repon(string Mien, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_CBQN_BY_MIEN_DAL(Mien, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_Fun_SUM_CBQN_BY_MIEN_HA_Repon(string Mien, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetString(new ReportDHBDAL().DHB_Fun_SUM_CBQN_BY_MIEN_HA_DAL(Mien, FromDate, ToDate));
        }

        public ReponseReportEntity DHB_GET_SLB_HHKQT_OF_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_GET_SLB_HHKQT_OF_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_2FIR_HN_HCM_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_2FIR_HN_HCM_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_FIR_HN_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_FIR_HN_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_FIR_HCM_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_FIR_HCM_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_FIR_HN_CRAFT_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_FIR_HN_CRAFT_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_FIR_HCM_CRAFT_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_FIR_HCM_CRAFT_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_List_OF_VIP_QS_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_List_OF_VIP_QS_DAL(Month));
        }

        public ReponseReportEntity DHB_Fun_SLB_OF_DAL_Repon(string Month)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_SLB_OF_DAL(Month));
        }


        public ReponseReportEntity DHB_Fun_SLB_SumTotal_OF_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_SLB_SumTotal_OF_DAL(FromDate, ToDate));
        }

        public ReponseReportEntity DHB_GET_SLB_HHKVN_BAY_QN_QT_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_GET_SLB_HHKVN_BAY_QN_QT_DAL(FromDate, ToDate));
        }

        public ReponseReportEntity DHB_GET_SLB_HHKVN_DI_DEN_MIEN_DAL_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_GET_SLB_HHKVN_DI_DEN_MIEN_DAL(FromDate, ToDate));
        }


        public ReponseReportEntity GET_ALL_ROUTE_NAME_RES()
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().GET_ALL_ROUTENAME());
        }

        public ReponseReportEntity DHB_Fun_Via_SumTotal_Repon(string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetTable(new ReportDHBDAL().DHB_Fun_Via_SumTotal_DAL(FromDate, ToDate));
        }
        #endregion



    }
}
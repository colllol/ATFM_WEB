using QLB.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using QLB.API.Data;
using QLB.Info;
using Newtonsoft.Json;
using System.IO;

namespace QLB.API.Controllers
{
    public class ReportDHBController : ApiController
    {
        const string _App01 = "*";


        [AcceptVerbs("Get")]
        public ReponseReportEntity REPORTS_GET_ALL()
        {
            return new ReportDHBRepository().REPORT_GET_ALL_REPORTS();
        }

        #region BAO CAO BCDHB01

        [AcceptVerbs("Get")]        
        public ReponseReportEntity BCDHB01_TH_Tuan_TypeLD(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB01_TH_Tuan_TypeLD(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB01_TH_Tuan_TypeOF(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB01_TH_Tuan_TypeOF(FromDate, ToDate);
        }

        #endregion
        #region BCDHB02
        [AcceptVerbs("Get")]        
        public ReponseReportEntity BCDHB02_TH_Thang_TypeLD(string MONTH, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB02_TH_Thang_TypeLD(MONTH, FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB02_TH_Thang_TypeOF(string MONTH, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB02_TH_Thang_TypeOF(MONTH, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB02_TH_ThangTruoc_TypeLD(string MONTH)
        {
            return new ReportDHBRepository().BCDHB02_TH_ThangTruoc_TypeLD(MONTH);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB02_TH_ThangTruoc_TypeOF(string MONTH)
        {
            return new ReportDHBRepository().BCDHB02_TH_ThangTruoc_TypeOF(MONTH);
        }
        #endregion


        #region BCDHB03
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB03_Get_HHKVN(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB03_Get_HHKVN(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB03_Get_HHKQT(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB03_Get_HHKQT(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB03_TH_HHKVN_SumQN_Thang(string Oper, string Month, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB03_TH_HHKVN_SumQN_Thang(Oper, Month, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB03_TH_HHKVN_SumQN_ThangTruoc(string Oper, string Month)
        {
            return new ReportDHBRepository().DHB03_TH_HHKVN_SumQN_ThangTruoc(Oper, Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB03_TH_HHKVN_SumQT_Thang(string Oper, string Month, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB03_TH_HHKVN_SumQT_Thang(Oper, Month, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB03_TH_HHKVN_SumQT_ThangTruoc(string Oper, string Month)
        {
            return new ReportDHBRepository().DHB03_TH_HHKVN_SumQT_ThangTruoc(Oper, Month);
        }
        #endregion

        #region BCDHB04
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB04_Get_DuongBay(string Route_Name)
        {
            return new ReportDHBRepository().DHB04_Get_DuongBay(Route_Name);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB04_TH_DuongBay_Sum_Thang(string ROUTE_NAME, string Month, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB04_Get_DuongBay_Thang(ROUTE_NAME, Month, FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB04_TH_DuongBay_Sum_ThangTruoc(string ROUTE_NAME, string Month)
        {
            return new ReportDHBRepository().DHB04_Get_DuongBay_ThangTruoc(ROUTE_NAME, Month);
        }

        #endregion

        #region BCDHB06
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB06_Get_THSLB_BYTIMES(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB06_THSLB_BYTIMES(FromDate, ToDate);
        }
        #endregion

        #region BCDHB09
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB09_Get_THSLB_BY2FIRHN_HCM(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB09_Get_THSLB_BY2FIRHN_HCM(FromDate, ToDate);
        }
        #endregion




        #region BCDHB10
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB10_Get_THSLB_FIRHN(string Oper, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB10_THSLB_FIRHN(Oper,FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB10_GET_THSLB_FIRHN_ByOPER(string Oper,string Route, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB10_GET_THSLB_FIRHN_BYOPER(Oper, Route, FromDate, ToDate);
        }


        #endregion

        #region BCDHB11
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB11_Get_THSLB_FIRHCM(string Oper, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB11_THSLB_FIRHCM(Oper, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB11_GET_THSLB_FIRHCM_ByOPER(string Oper, string Route, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB11_GET_THSLB_FIRHCM_BYOPER(Oper, Route, FromDate, ToDate);
        }


        #endregion

        #region BCDHB12
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB12_Get_THSLB_FIRHN(string Craft, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB12_THSLB_FIRHN(Craft, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB12_GET_THSLB_FIRHN_ByCraft(string Craft, string Route, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB12_GET_THSLB_FIRHN_BYCRAFT(Craft, Route, FromDate, ToDate);
        }
        #endregion

        #region BCDHB13
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB13_Get_THSLB_FIRHCM(string Craft, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB13_THSLB_FIRHCM(Craft, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB13_GET_THSLB_FIRHCM_ByCraft(string Craft, string Route, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB13_GET_THSLB_FIRHCM_BYCRAFT(Craft, Route, FromDate, ToDate);
        }
        #endregion

        #region BCDHB14
        
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB14_GET_THSLB_FIRHN(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB14_GET_THSLB_FIRHN(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB14_GET_THSLB_FIRHCM(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB14_GET_THSLB_FIRHCM(FromDate, ToDate);
        }

        #endregion










        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB03(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB03(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB04(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB04(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB05(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB05(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB06(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB06(obj);
        }
        
        [AcceptVerbs("Get")]
        public ReponseReportEntity BCDHB07(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB07(FromDate, ToDate);
        }        
        [AcceptVerbs("Get")]        
        public ReponseReportEntity BCDHB08(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB08(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        
        public ReponseReportEntity BCDHB09(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB09(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        
        public ReponseReportEntity BCDHB10(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB10(FromDate,ToDate);
        }
        [AcceptVerbs("Get")]
        
        public ReponseReportEntity BCDHB11(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB11(FromDate, ToDate);
        }
        [AcceptVerbs("Get")]
        
        public ReponseReportEntity BCDHB12(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB12(FromDate,ToDate);
        }
        [AcceptVerbs("Get")]
        
        public ReponseReportEntity BCDHB13(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().BCDHB13(FromDate,ToDate);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB14(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB14(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB15(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB15(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB16(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB16(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB17(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB17(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB18(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB18(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB19(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB19(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB20(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB20(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB21(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB21(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB22(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB22(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB23(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB23(obj);
        }
        [AcceptVerbs("Put")]
        
        public ReponseEntity BCDHB24(clsSearchValue obj)
        {
            return new ReportDHBRepository().BCDHB24(obj);
        }

        #region PHAN MOI
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_TH_HQN_SumQN_Pupose(string Oper, string Pupose, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_TH_HQN_SumQN_PuposeRepon(Oper, Pupose, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_TH_HQN_SumQT_Pupose(string Oper, string Pupose, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_TH_HQN_SumQT_Pupose(Oper, Pupose, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_SumTotal(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_SumTotal_Pupose(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Get_HQT_SumTotal_LD(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Get_HQT_SumTotal_LD_Repon(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_HQT_SumTotal_QN_LD(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_HQT_SumTotal_QN_LD_Repon(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_CBQN_BYSB(string air, string oper, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_CBQN_BYSB_Repon(air, oper,FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_CBQT_BYSB(string air, string oper, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_CBQT_BYSB_Repon(air, oper, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_QNDIQN_BYSB(string air, string oper, string Month)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_QNDIQN_BYSB_Repon(air, oper, Month);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_QNDIQT_BYSB(string air, string oper, string Month)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_QNDIQT_BYSB_Repon(air, oper, Month);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_QTVE_BYSB(string air, string oper, string Month)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_QTVE_BYSB_Repon(air, oper, Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SB_BYMIEN(string Mien)
        {
            return new ReportDHBRepository().DHB_GET_SB_BYMIEN_Repon(Mien);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SLB_HHKVN(string Mien,string Month)
        {
            return new ReportDHBRepository().DHB_GET_SLB_HHKVN_Repon(Mien,Month);
        }


        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_CBQT_BY_MIEN(string Mien, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_CBQT_BY_MIEN_Repon(Mien, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_CBQN_BY_MIEN(string Mien, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_CBQN_BY_MIEN_Repon(Mien, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SUM_CBQN_BY_MIEN_HA(string Mien, string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SUM_CBQN_BY_MIEN_HA_Repon(Mien, FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SLB_HHKQT_OF(string Month)
        {
            return new ReportDHBRepository().DHB_GET_SLB_HHKQT_OF_Repon(Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_2FIR_HN_HCM(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_2FIR_HN_HCM_Repon(Month);
        }


        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_FIR_HN(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_FIR_HN_DAL_Repon(Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_FIR_HCM(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_FIR_HCM_DAL_Repon(Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_FIR_HN_CRAFT(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_FIR_HN_CRAFT_DAL_Repon(Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_FIR_HCM_CRAFT(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_FIR_HCM_CRAFT_DAL_Repon(Month);
        }
        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_List_OF_VIP_QS(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_List_OF_VIP_QS_DAL_Repon(Month);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SLB_OF(string Month)
        {
            return new ReportDHBRepository().DHB_Fun_SLB_OF_DAL_Repon(Month);
        }


        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_Fun_SLB_SumTotal_OF(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_SLB_SumTotal_OF_Repon(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SLB_HHKVN_BAY_QN_QT(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_GET_SLB_HHKVN_BAY_QN_QT_Repon(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SLB_HHKVN_DI_DEN_MIEN(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_GET_SLB_HHKVN_DI_DEN_MIEN_DAL_Repon(FromDate, ToDate);
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity GET_ALL_ROUTE_NAME()
        {
            return new ReportDHBRepository().GET_ALL_ROUTE_NAME_RES();
        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity DHB_GET_SLB_BY_VIA(string FromDate, string ToDate)
        {
            return new ReportDHBRepository().DHB_Fun_Via_SumTotal_Repon(FromDate, ToDate);
        }

        #endregion

    }
}

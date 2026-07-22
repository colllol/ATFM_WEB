using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
   public class ReportDHBDAL
    {

        #region REPORT ALL
        public DataTable REPORT_GET_ALL_REPORTS()
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();               
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "REPORT_GET_REPORTNAME", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region BCDHB01
        public string DHB01_TH_Tuan_TypeLD(string FromDate, string ToDate)
        {
            try
            {
                
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));                
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB01_Fn_TH_Tuan_TypeLD", lis.ToArray()).ToString();
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public string DHB01_TH_Tuan_TypeOF(string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB01_Fn_TH_Tuan_TypeOF", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region BCDHB02

        public string DHB02_TH_Thang_TypeLD(string Thang, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("MONTH", Thang));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB02_Fn_TH_Thang_TypeLD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB02_TH_ThangTruoc_TypeLD(string Thang)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("MONTH", Thang));                
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB02_Fn_TH_ThangTruoc_TypeLD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB02_TH_Thang_TypeOF(string Thang, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("MONTH", Thang));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB02_Fn_TH_Thang_TypeOF", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB02_TH_ThangTruoc_TypeOF(string Thang)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("MONTH", Thang));                
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB02_Fn_TH_ThangTruoc_TypeOF", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable DHB03_Get_HHKVN(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB03_Get_HHKVN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB03_Get_HHKQT(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB03_Get_HHKQT", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DHB03_TH_HHKVN_SumQN_Thang(string Hang,string Month, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));
                lis.Add(new OracleParameter("MONTH", Month));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB03_Fn_SumQN_HangVN_Thang_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB03_TH_HHKVN_SumQN_ThangTruoc(string Hang, string Month)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));
                lis.Add(new OracleParameter("MONTH", Month));             
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB03_Fn_SumQN_HangVN_TTR_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB03_TH_HHKVN_SumQT_Thang(string Hang, string Month, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));
                lis.Add(new OracleParameter("MONTH", Month));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB03_Fn_SumQT_HangVN_Thang_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB03_TH_HHKVN_SumQT_ThangTruoc(string Hang, string Month)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));
                lis.Add(new OracleParameter("MONTH", Month));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB03_Fn_SumQT_HangVN_TTR_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region

        public DataTable DHB04_Get_DuongBay(string P_ROUTE_NAME)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_ROUTE_NAME", P_ROUTE_NAME));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB04_GET_DuongBay", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DHB04_TH_DuongBay_Sum_Thang(string Duongbay, string Month, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_ROUTE_NAME", Duongbay));
                lis.Add(new OracleParameter("MONTH", Month));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB04_Get_DuongBay_TH_Thang", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string DHB04_TH_DuongBay_Sum_ThangTruoc(string Duongbay, string Month)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_ROUTE_NAME", Duongbay));
                lis.Add(new OracleParameter("MONTH", Month));               
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB04_Get_DuongBay_TH_ThTruoc", lis.ToArray()).ToString();



            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region BAO CAO SO 06

        public DataTable DHB06_THSLB_BYTIMES(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB06_FnTongHop_KhoangThoiGian", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region BAO CAO SO 09

        public DataTable DHB09_THSLB_BY2FIRHN_HCM(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB09_FnTongHop_2FirHN_HCM", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region BAO CAO SO 10

        public DataTable DHB10_THSLB_FIRHN(string Oper, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Oper", Oper));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB10_Fn_THSLB_FIRHN", lis.ToArray()).Tables[0];                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string BCDHB10_GET_THSLB_FIRHN_BYOPER(string Oper,string Route, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Oper", Oper));
                lis.Add(new OracleParameter("P_ROUTE_NAME", Route));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB10_Fn_SUM_FIRHN_BYOPER", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region BAO CAO SO 11

        public DataTable DHB11_THSLB_FIRHCM(string Oper, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Oper", Oper));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB11_Fn_THSLB_FIRHCM", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string BCDHB11_GET_THSLB_FIRHCM_BYOPER(string Oper, string Route, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Oper", Oper));
                lis.Add(new OracleParameter("P_ROUTE_NAME", Route));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB11_Fn_SUM_FIRHCM_BYOPER", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region BAO CAO SO 12

        public DataTable DHB12_THSLB_FIRHN(string Craft, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Craft", Craft));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB12_Fn_THSLB_FIRHN", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string BCDHB12_GET_THSLB_FIRHN_BYCRAFT(string Craft, string Route, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Craft", Craft));
                lis.Add(new OracleParameter("P_ROUTE_NAME", Route));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB12_Fn_SUM_FIRHN_BYCRAFT", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion



        #region BAO CAO SO 13

        public DataTable DHB13_THSLB_FIRHCM(string Craft, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Craft", Craft));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB13_Fn_THSLB_FIRHCM", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string BCDHB13_GET_THSLB_FIRHCM_BYCRAFT(string Craft, string Route, string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Craft", Craft));
                lis.Add(new OracleParameter("P_ROUTE_NAME", Route));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB13_Fn_SUM_FIRHCM_BYCRAFT", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region BAO CAO SO 14

        public string BCDHB14_GET_THSLB_FIRHN(string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "SumFirHn", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string BCDHB14_GET_THSLB_FIRHCM(string FromDate, string ToDate)
        {
            try
            {

                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "SumFirHcm", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion










        public DataTable BCDHB02(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));                
                return new oDataProvider().ExecuteDatase("REPORT_DHB01", "SL_View", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BCDHB03(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB03", obj).Tables[0];
        }
        public DataTable BCDHB04(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB04", obj).Tables[0];
        }
        public DataTable BCDHB05(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB05", obj).Tables[0];
        }
        public DataTable BCDHB06(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB06", obj).Tables[0];
        }
       
        public DataTable BCDHB07(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();               
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB07_Fn_TH_KhoangThoiGian", lis.ToArray()).Tables[0];               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            

        public DataTable BCDHB08(string FromDate, string ToDate)
        {
            
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB08_Fn_TH_KhoangThoiGian", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BCDHB09(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB09", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable BCDHB10(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB10", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable BCDHB11(string FromDate, string ToDate)
        { 
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB11", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable BCDHB12(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB12", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable BCDHB13(string FromDate, string ToDate)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB13", new OracleParameter("P_FromDate", FromDate), new OracleParameter("P_ToDate", ToDate)).Tables[0];
        }
        public DataTable BCDHB14(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB14", obj).Tables[0];
        }
        public DataTable BCDHB15(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB15", obj).Tables[0];
        }
        public DataTable BCDHB16(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB16", obj).Tables[0];
        }
        public DataTable BCDHB17(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB17", obj).Tables[0];
        }
        public DataTable BCDHB18(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB18", obj).Tables[0];
        }
        public DataTable BCDHB19(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB19", obj).Tables[0];
        }
        public DataTable BCDHB20(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB20", obj).Tables[0];
        }
        public DataTable BCDHB21(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB21", obj).Tables[0];
        }
        public DataTable BCDHB22(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB22", obj).Tables[0];
        }
        public DataTable BCDHB23(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB23", obj).Tables[0];
        }
        public DataTable BCDHB24(clsSearchValue obj)
        {
            return new oDataProvider().ExecuteDatase("REPORT_PKG", "BCDHB24", obj).Tables[0];
        }

        #region PHAN MOI
        public string DHB_TH_HQN_SumQN_PuposeDAL(string Hang, string PURPOSE, string FromDate, string ToDate)
        {
            try
            {
                string _return = "0";
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));                
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));

                if(PURPOSE=="PAX")
                    _return= new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQN_HangVN_LD_PAX", lis.ToArray()).ToString();
                else if(PURPOSE == "VIP")
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQN_HangVN_LD_VIP", lis.ToArray()).ToString();
                else if (PURPOSE == "FER")
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQN_HangVN_LD_FER", lis.ToArray()).ToString();
                else
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQN_HangVN_LD_QRF", lis.ToArray()).ToString();

                return _return;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }









        public string DHB_TH_HQN_SumQT_PuposeDAL(string Hang, string PURPOSE, string FromDate, string ToDate)
        {
            try
            {
                string _return = "0";
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("OPER", Hang));                
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));

                if (PURPOSE == "PAX")
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQT_HangVN_PAX", lis.ToArray()).ToString();
                else if (PURPOSE == "VIP")
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQT_HangVN_VIP", lis.ToArray()).ToString();
                else if (PURPOSE == "FER")
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQT_HangVN_FER", lis.ToArray()).ToString();
                else
                    _return = new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQT_HangVN_QRF", lis.ToArray()).ToString();

                return _return;
                //return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumQT_HangVN_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string DHB_SumTotalDAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();               
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SumTotal_LD", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable DHB_Get_HQT_SumTotal_LD_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_HQT_SumTotal_LD", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_HQT_SumTotal_QN_LD_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_HQT_SumTotal_QN_LD", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DHB_Fun_SUM_CBQN_BYSB_DAL(string air, string oper, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("AIR", air));
                lis.Add(new OracleParameter("OPER", oper));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_CBQN_BYSB", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB_Fun_SUM_CBQT_BYSB_DAL(string air, string oper, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("AIR", air));
                lis.Add(new OracleParameter("OPER", oper));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_CBQT_BYSB", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string DHB_Fun_SUM_QNDIQN_BYSB_DAL(string air, string oper, string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("FROMAIR", air));
                lis.Add(new OracleParameter("OPER", oper));
                lis.Add(new OracleParameter("P_MONTH", Month));                
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_QNDIQN_BYSB", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB_Fun_SUM_QNDIQT_BYSB_DAL(string air, string oper, string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("FROMAIR", air));
                lis.Add(new OracleParameter("OPER", oper));
                lis.Add(new OracleParameter("P_MONTH", Month));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_QNDIQT_BYSB", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB_Fun_SUM_QTVE_BYSB_DAL(string air, string oper, string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("FROMAIR", air));
                lis.Add(new OracleParameter("OPER", oper));
                lis.Add(new OracleParameter("P_MONTH", Month));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_QTVE_BYSB", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable DHB_GET_SB_BYMIEN_DAL(string Mien)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_Mien", Mien));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_AERO_BYMIEN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DHB_GET_SLB_HHKVN_DAL(string Mien,string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Mien", Mien));
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_SLB_HHKVN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DHB_Fun_SUM_CBQT_BY_MIEN_DAL(string Mien, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_Mien", Mien));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_CBQT_BY_MIEN", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB_Fun_SUM_CBQN_BY_MIEN_DAL(string Mien, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Mien", Mien));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_CBQN_BY_MIEN", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DHB_Fun_SUM_CBQN_BY_MIEN_HA_DAL(string Mien, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Mien", Mien));
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                return new oDataProvider().ExecuteScalar_ForReport("REPORT_DHB", "DHB_Fun_SUM_CBQN_BY_MIEN_HA", lis.ToArray()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable DHB_GET_SLB_HHKQT_OF_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();                
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_DOTXUAT", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_List_OF_2FIR_HN_HCM_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_2FIR_HN_HCM", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_List_OF_FIR_HN_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_FIR_HN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_List_OF_FIR_HCM_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_FIR_HCM", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DHB_Fun_List_OF_FIR_HN_CRAFT_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_FIR_HN_CRAFT", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_List_OF_FIR_HCM_CRAFT_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_FIR_HCM_CRAFT", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_List_OF_VIP_QS_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_List_OF_VIP_QS", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_SLB_OF_DAL(string Month)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", Month));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_SLB_OF", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_Fun_SLB_SumTotal_OF_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_Fun_SLB_SumTotal_OF", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DHB_GET_SLB_HHKVN_BAY_QN_QT_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_SLB_HHKVN_BAY_QN_QT", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DHB_GET_SLB_HHKVN_DI_DEN_MIEN_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_FromDate", FromDate));
                lis.Add(new OracleParameter("P_ToDate", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_SLB_HHKVN_DI_DEN_MIEN", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GET_ALL_ROUTENAME()
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_ROUTE_NAME", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DHB_Fun_Via_SumTotal_DAL(string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_MONTH", FromDate));
                lis.Add(new OracleParameter("P_MONTHPRE", ToDate));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("REPORT_DHB", "DHB_GET_SLB_BY_VIA", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}

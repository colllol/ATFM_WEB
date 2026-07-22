using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
   public class FinishedFlightsDAL
    {
        public DataTable GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", FromDate));
                lis.Add(new OracleParameter("P_TO_DATE", ToDate));
                return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "FINISHED_FLIGHTS_GET_PAGE", lis.ToArray()).Tables[0];
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetExportBySearch(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "ExportBySearch", obj).Tables[0];
        }

        public DataTable GetPage(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch", obj).Tables[0];
        }

        #region PHAN MOI
        public DataTable GET_T_FINISHED_FLIGHTS_BY_TIME(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_T_FINISHED_FLIGHTS_BY_TIME", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_KHUNGTIME(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_KHUNGTIME", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_HANG_QNOI(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_HANG_QNOI", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_HANG_QTE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_HANG_QTE", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_BAY_QNOI(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_BAY_QNOI", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_BAY_QTE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_BAY_QTE", obj).Tables[0];
        }

        public DataTable GET_FIN_FLIGHTS_HQN_BAY_QNOI(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_HQN_BAY_QNOI", obj).Tables[0];
        }

        public DataTable GET_FIN_FLIGHTS_HQN_BAY_QTE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_HQN_BAY_QTE", obj).Tables[0];
        }

        public DataTable GET_FIN_FLIGHTS_HQT_BAY_QNOI(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_HQT_BAY_QNOI", obj).Tables[0];
        }

        public DataTable GET_FIN_FLIGHTS_HQT_BAY_QTE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_HQT_BAY_QTE", obj).Tables[0];
        }



        public DataTable GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_QNDI", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_QTVE_MOVE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_QTVE", obj).Tables[0];
        }

        public DataTable GET_FINISHED_FLIGHTS_CHOTSL_MOVE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_FLIGHTS_CHOT_SL", obj).Tables[0];
        }

        public DataTable GET_FIN_FLIGHTS_BAY_FIRHN(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_BAY_FIRHN", obj).Tables[0];
        }
        public DataTable GET_FIN_FLIGHTS_BAY_FIRHCM(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FIN_FLIGHTS_BAY_FIRHCM", obj).Tables[0];
        }


        public DataTable GET_FINISHED_F_NOTCOMPLATE(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_FINISHED_F_NOTCOMPLATE", obj).Tables[0];
        }


        public DataTable GET_FINISHED_FLIGHTS(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", FromDate));
                lis.Add(new OracleParameter("P_TO_DATE", ToDate));
                return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GET_T_FINISHED_FLIGHTS_BY_TIME", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Int64 ID)
        {
            try
            {
                new oDataProvider().ExecuteNonQuery("FINISH_FLIGHTS_PKG", "oDelete", new OracleParameter("p_value", ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 UpdateMoveQNDI(clsFinishedFlightSearch obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FINISH_FLIGHTS_PKG", "oUpdateMove", obj);
            return (Int64)u;
        }
        public Int64 UpdateMoveQTVE(clsFinishedFlightSearch obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FINISH_FLIGHTS_PKG", "oUpdateMoveQTVE", obj);
            return (Int64)u;
        }
        public Int64 UpdateMoveCHOTSL(clsFinishedFlightSearch obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FINISH_FLIGHTS_PKG", "oUpdateMoveCHOTSL", obj);
            return (Int64)u;
        }

       
        public void Make_FinishedDAL()
        {
            try
            {
                new oDataProvider().ExecuteNonQuery("MAKE_FINISHED", "make_finished_flights_news", new OracleParameter("p_string", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public DataTable GetPageMBNC(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBNOICHUYEN", obj).Tables[0];
        }

        public DataTable GetPageMBATA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBATA", obj).Tables[0];
        }
        public DataTable GetPageMBVIA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBVIA", obj).Tables[0];
        }

        public DataTable GetPageMBBCS(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBBCS", obj).Tables[0];
        }

        public DataTable GetPageMBSAMECAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBSameCal", obj).Tables[0];
        }

        public DataTable GetPageMBTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMBTrungCal", obj).Tables[0];
        }

        
        public DataTable GetPageMN(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchMN", obj).Tables[0];
        }
        public DataTable GetPageNC(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchNOICHUYEN", obj).Tables[0];
        }

        public DataTable GetPageATA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchATA", obj).Tables[0];
        }
        public DataTable GetPageVIA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchVIA", obj).Tables[0];
        }





        public DataTable GetPageBCS(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchBCS", obj).Tables[0];
        }

        public DataTable GetPageSAMECAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchSameCal", obj).Tables[0];
        }
        public DataTable GetPage7(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch7", obj).Tables[0];
        }
        public DataTable GetPage8(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch8", obj).Tables[0];
        }
        public DataTable GetPage9(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch9", obj).Tables[0];
        }
        public DataTable GetPage10(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch10", obj).Tables[0];
        }
        public DataTable GetPage11(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch11", obj).Tables[0];
        }
        public DataTable GetPage12(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch12", obj).Tables[0];
        }
        public DataTable GetPage13(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch13", obj).Tables[0];
        }
        public DataTable GetPage14(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearch14", obj).Tables[0];
        }

        public DataTable GetPageTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchTrungCal", obj).Tables[0];
        }



        public DataTable GetPageDN(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDN", obj).Tables[0];
        }

        public DataTable GetPageDNNC(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNNOICHUYEN", obj).Tables[0];
        }

        public DataTable GetPageDNATA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNATA", obj).Tables[0];
        }
        public DataTable GetPageDNVIA(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNVIA", obj).Tables[0];
        }

        public DataTable GetPageDNBCS(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNBCS", obj).Tables[0];
        }

        public DataTable GetPageDNSAMECAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNSameCal", obj).Tables[0];
        }

        public DataTable GetPageDNTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageBySearchDNTrungCal", obj).Tables[0];
        }





        public DataTable GetPage(int page_size, int page_index, string where)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GETPAGE_ALL"
                            , new OracleParameter("P_PAGE_SIZE", page_size)
                            , new OracleParameter("P_PAGE_INDEX", page_index)
                            , new OracleParameter("P_WHERE", where)).Tables[0];

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
                //return DataProvider.Instance().GetStoreDataSet_Oracle("FINISH_FLIGHTS_PKG.FINISHED_FLIGHTS_GET_ID", new string[] { "P_FLIGHT_ID" }, new object[] { Id }).Tables[0];
                return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "FINISHED_FLIGHTS_GET_ID", new OracleParameter("P_FLIGHT_ID", Id)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int64 Update(FinishedFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FINISH_FLIGHTS_PKG", "oUpdate", obj);
            return (Int64)u;
        }
        public Int64 Insert(FinishedFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FINISH_FLIGHTS_PKG", "oInsert", obj);
            return (Int64)u;
        }

        public DataTable GetHisById(string id)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetHisBy", new OracleParameter("p_value", id)).Tables[0];
        }
        public bool RetoreHis(Int64 id, Int64 version, string idUser)
        {
            return new oDataProvider().ExecuteNonQuery("FINISH_FLIGHTS_PKG", "ReStoreById"
                    , new OracleParameter("p_Id", id)
                    , new OracleParameter("p_Version", version)
                    , new OracleParameter("p_UserId", idUser)) == -1 ? false : true;
        }
        public DataTable GetTableDelete(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "GetPageDeleteBySearch", obj).Tables[0];
        }
    }
}

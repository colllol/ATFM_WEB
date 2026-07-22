using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using QLB.BusinessLogic;
namespace QLB.API
{
    public class FinishedFlightsController:ApiController
    {
        #region MIEN BAC

        /*
         * clsDayFlightValueSearch : đối tượng tìm kiếm        
         * Lấy danh sách các chuyến bay đã kết thúc              
        */

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch(clsFinishedFlightSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage(obj));
        }

        //Phan Moi

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_T_FINISHED_FLIGHTS_BY_TIME(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_T_FINISHED_FLIGHTS_BY_TIME(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_KHUNGTIME(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_KHUNGTIME(_obj));
        }


        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_HANG_QNOI(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_HANG_QNOI(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_QTVE_MOVE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_QTVE_MOVE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_CHOTSL_MOVE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_CHOTSL_MOVE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_HANG_QTE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_HANG_QTE(_obj));
        }


        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_BAY_QNOI(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QNOI(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_FLIGHTS_BAY_QTE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QTE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_HQN_BAY_QNOI(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQN_BAY_QNOI(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_HQN_BAY_QTE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQN_BAY_QTE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_HQT_BAY_QNOI(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQT_BAY_QNOI(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_HQT_BAY_QTE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQT_BAY_QTE(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_BAY_FIRHN(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_BAY_FIRHN(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FIN_FLIGHTS_BAY_FIRHCM(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FIN_FLIGHTS_BAY_FIRHCM(_obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GET_FINISHED_F_NOTCOMPLATE(clsFinishedFlightSearch _obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GET_FINISHED_F_NOTCOMPLATE(_obj));
        }



        [AcceptVerbs("Delete")]        
        public ReponseEntity Delete(Int64 Id)
        {
            return new FinishedFlightsRepository().Delete(Id);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateMoveQNDI(clsFinishedFlightSearch obj)
        {
            return new ReponseEntityHelper().Update<clsFinishedFlightSearch, FinishedFlightsDAL>(obj, "UpdateMoveQNDI");
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateMoveQTVE(clsFinishedFlightSearch obj)
        {
            return new ReponseEntityHelper().Update<clsFinishedFlightSearch, FinishedFlightsDAL>(obj, "UpdateMoveQTVE");
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateMoveCHOTSL(clsFinishedFlightSearch obj)
        {
            return new ReponseEntityHelper().Update<clsFinishedFlightSearch, FinishedFlightsDAL>(obj, "UpdateMoveCHOTSL");
        }

        [AcceptVerbs("Put")]
        public ReponseEntity Make_Finished()
        {
            return new FinishedFlightsRepository().Make_Finished_Response();
        }
        /*
         * clsDayFlightValueSearch : đối tượng tìm kiếm        
         * Lấy danh sách các chuyến bay nối chuyến đã kết thúc              
        */

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBNC(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBNC(obj));
        }
        /*
         * clsDayFlightValueSearch : đối tượng tìm kiếm        
         * Lấy danh sách các chuyến bay chậm đã kết thúc              
        */
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBATA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBATA(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBVIA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBVIA(obj));
        }
        /*
        * clsDayFlightValueSearch : đối tượng tìm kiếm        
        * Lấy danh sách các chuyến bay chuyển sân đã kết thúc              
        */
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBBCS(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBBCS(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBSAMECAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBSAMECAL(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMBTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMBTRUNGCAL(obj));
        }


        #endregion MIEN BAC

        #region MIEN NAM

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchMN(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageMN(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchNC(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageNC(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchATA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageATA(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchVIA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageVIA(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchBCS(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageBCS(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchSAMECAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageSAMECAL(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch7(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage7(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch8(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage8(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch9(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage9(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch10(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage10(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch11(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage11(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch12(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage12(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch13(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage13(obj));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearch14(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPage14(obj));
        }


        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageTRUNGCAL(obj));
        }

        #endregion MIEN NAM

        #region MIEN TRUNG

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDN(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDN(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNNC(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNNC(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNATA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNATA(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNVIA(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNVIA(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNBCS(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNBCS(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNSAMECAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNSAMECAL(obj));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity GetTableBySearchDNTRUNGCAL(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetPageDNTRUNGCAL(obj));
        }


        #endregion MIEN TRUNG


        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity ExportBySearch(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetExportBySearch(obj));
        }
        
        [AcceptVerbs("Get")]
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate)
        {
            return new FinishedFlightsRepository().GetPage(page_size, page_index, where, Fromdate, Todate);
        }

       

        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 Id)
        {
            return new FinishedFlightsRepository().GetByID(Id);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity Update(FinishedFlights obj)
        {
            //new FinishedFlightsDAL().Update(obj);
            return new ReponseEntityHelper().Update<FinishedFlights, FinishedFlightsDAL>(obj, "Update");
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseEntity Insert(FinishedFlights obj)
        {
            return new ReponseEntityHelper().Update<FinishedFlights, FinishedFlightsDAL>(obj, "Insert");
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetHistoryById(string id)
        {
            return new ReponseEntityHelper().GetHistoryByID<FinishedFlights_BAK>(new FinishedFlightsDAL().GetHisById(id));
        }
        [AcceptVerbs("Get")]
        public ReponseEntity RestoreHis(Int64 id, int version, string iduser)
        {
            return new ReponseEntityHelper().RestoreHis<FinishedFlights_BAK, FinishedFlightsDAL>(id, version, iduser, "RetoreHis");
        }
        [AcceptVerbs("Put")]
        public ReponseReportEntity GetTableDelete(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new FinishedFlightsDAL().GetTableDelete(obj));
        }
    }
}
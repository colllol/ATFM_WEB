using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class FinishedFlightsRepository
    {
        /*
         * page_size :số bản ghi hiển thị trên trang
         * page_index : index của trang
         * where : Điều kiện lọc
         * FromDate : Từ ngày
         * ToDate : Đến ngày
         * Lấy danh sách các chuyến bay đã kết thúc              
        */
        public ReponseEntity GetPage (int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetPage<FinishedFlights>(new FinishedFlightsDAL().GetPage(page_size, page_index, where, FromDate, ToDate));
        }

        public ReponseEntity GET_T_FINISHED_FLIGHTS_BY_TIME(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_T_FINISHED_FLIGHTS_BY_TIME(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_KHUNGTIME(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_KHUNGTIME(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_HANG_QNOI(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_HANG_QNOI(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_HANG_QTE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_HANG_QTE(finished_flight));
        }


        public ReponseEntity GET_FINISHED_FLIGHTS_BAY_QNOI(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QNOI(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QNOI_MOVE(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_BAY_QTE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_BAY_QTE(finished_flight));
        }

        public ReponseEntity GET_FIN_FLIGHTS_HQN_BAY_QNOI(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQN_BAY_QNOI(finished_flight));
        }

        public ReponseEntity GET_FIN_FLIGHTS_HQN_BAY_QTE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQN_BAY_QTE(finished_flight));
        }


        public ReponseEntity GET_FINISHED_FLIGHTS_QTVE_MOVE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_QTVE_MOVE(finished_flight));
        }

        public ReponseEntity GET_FINISHED_FLIGHTS_CHOTSL_MOVE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_FLIGHTS_CHOTSL_MOVE(finished_flight));
        }


        public ReponseEntity GET_FIN_FLIGHTS_HQT_BAY_QNOI(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQT_BAY_QNOI(finished_flight));
        }

        public ReponseEntity GET_FIN_FLIGHTS_HQT_BAY_QTE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FIN_FLIGHTS_HQT_BAY_QTE(finished_flight));
        }

        public ReponseEntity GET_FIN_FLIGHTS_BAY_FIRHN(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FIN_FLIGHTS_BAY_FIRHN(finished_flight));
        }


        public ReponseEntity GET_FINISHED_F_NOTCOMPLATE(clsFinishedFlightSearch finished_flight)
        {
            return new ReponseEntityHelper().GetPage<clsFinishedFlightSearch>(new FinishedFlightsDAL().GET_FINISHED_F_NOTCOMPLATE(finished_flight));
        }

        public ReponseEntity Make_Finished_Response()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FinishedFlightsDAL objDAL = new FinishedFlightsDAL();
                objDAL.Make_FinishedDAL();
                oResponse.Code = "1";
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity Delete(Int64 Id)
        {
            return new ReponseEntityHelper().Delete<clsFinishedFlightSearch, FinishedFlightsDAL>(Id, "Delete");
        }
        /*
         * Id : Mã của chuyến bay         
         * Lấy thông tin chuyến bay đã kết thúc             
        */
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<FinishedFlights>(new FinishedFlightsDAL().GetById(Id));
        }
    }
}
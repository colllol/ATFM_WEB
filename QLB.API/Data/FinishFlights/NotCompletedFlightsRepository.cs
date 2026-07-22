using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class NotCompletedFlightsRepository
    {
        /*
         * page_size :số bản ghi hiển thị trên trang
         * page_index : index của trang
         * where : Điều kiện lọc
         * FromDate : Từ ngày
         * ToDate : Đến ngày
         * Lấy danh sách các chuyến bay không hoàn thành              
        */
        public ReponseEntity GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            return new ReponseEntityHelper().GetPage<NotCompletedFlights>(new NotCompletedFlightsDAL().GetPage(page_size, page_index, where, FromDate, ToDate));
        }
        /*
        * Id : Mã của chuyến bay         
        * Lấy thông tin chuyến bay không hoàn thành             
       */
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<NotCompletedFlights>(new NotCompletedFlightsDAL().GetById(Id));
        }
    }
}
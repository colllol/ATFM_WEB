using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class FlightFixesRepository
    {
        /*
         * page_size :số bản ghi hiển thị trên trang
         * page_index : index của trang
         * where : Điều kiện lọc
         * FromDate : Từ ngày
         * ToDate : Đến ngày
         * Lấy danh sách các chuyến bay đã kết thúc              
        */
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<clsFLIGHT_FIXES_Search>(new Flight_FixesDAL().GetPageFLIGHT_FIXES(page_size, page_index, where));
        }
    }
}
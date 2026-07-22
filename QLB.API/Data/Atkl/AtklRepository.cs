using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using QLB.Info;
using QLB.BusinessLogic;
using QLB.API.Common;

namespace QLB.API.Data
{
    public class AtklRepository
    {
        //public ReponseEntity GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        //{
        //    return new ReponseEntityHelper().GetPage<Atkl>(new AtklDAL().GetPage(page_size, page_index, where, FromDate, ToDate));
        //}
        public ReponseEntity GetPage(clsSearchAtkl obj)
        {
            return new ReponseEntityHelper().GetPage<Atkl>(new AtklDAL().GetPage(obj));
        }
        public ReponseEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<Atkl>(new AtklDAL().GetById(Id).Tables[0]);
        }
        public ReponseEntity UpdateAtkl(Atkl oAtkl)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AtklDAL objDAL = new AtklDAL();
                Int64 Id = objDAL.UpdateAtkl(oAtkl);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.UpdatePermMasterSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.UpdatePermMasterSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

    }
}
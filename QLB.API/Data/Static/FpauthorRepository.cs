using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class FpauthorRepository
    {
        public ReponseEntity GetPageFpauthor(int page_size, int page_index,string where)
        {
            return new ReponseEntityHelper().GetPage<Fpauthor>(new FpauthorDAL().GetPageFpauthor(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageFpauthorExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Fpauthor>(new FpauthorDAL().GetPageFpauthorExport(where).Tables[0]);
        }

        public ReponseEntity GetAllFpauthor()
        {
            return new ReponseEntityHelper().GetAll<Fpauthor>(new FpauthorDAL().GetAllFpauthor().Tables[0]);
        }

        public ReponseEntity GetByIdFpauthor(int Id)
        {
            return new ReponseEntityHelper().GetByID<Fpauthor>(new FpauthorDAL().GetByIdFpauthor(Id).Tables[0]);
        }

        public ReponseEntity CreateFpauthor(Fpauthor oFPAUTHOR)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FpauthorDAL objDAL = new FpauthorDAL();
                Int32 Id = objDAL.CreateFpauthor(oFPAUTHOR);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "FpauthorRepository.CreateFpauthor:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FpauthorRepository.CreateFpauthor: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateFpauthor(Fpauthor oFPAUTHOR)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FpauthorDAL objDAL = new FpauthorDAL();
                Int32 Id = objDAL.UpdateFpauthor(oFPAUTHOR);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "FpauthorRepository.UpdateFpauthor:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FpauthorRepository.UpdateFpauthor: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteFpauthor(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FpauthorDAL objDAL = new FpauthorDAL();
                objDAL.DeleteFpauthor(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FpauthorRepository.DeleteFpauthor: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
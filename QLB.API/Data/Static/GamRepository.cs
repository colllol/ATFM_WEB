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
    public class GamRepository
    {
        public ReponseEntity GetPageGam(int page_size, int page_index,string where)
        {
            return new ReponseEntityHelper().GetPage<Gam>(new GamDAL().GetPageGam(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageGamExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Gam>(new GamDAL().GetPageGamExport(where).Tables[0]);
        }

        public ReponseEntity GetAllGam()
        {
            return new ReponseEntityHelper().GetAll<Gam>(new GamDAL().GetAllGam().Tables[0]);
        }

        public ReponseEntity GetByIdGam(int Id)
        {
            return new ReponseEntityHelper().GetByID<Gam>(new GamDAL().GetByIdGam(Id).Tables[0]);
        }

        public ReponseEntity CreateGam(Gam oGAM)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GamDAL objDAL = new GamDAL();
                Int32 Id = objDAL.CreateGam(oGAM);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GamRepository.CreateGam:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GamRepository.CreateGam: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateGam(Gam oGAM)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GamDAL objDAL = new GamDAL();
                Int32 Id = objDAL.UpdateGam(oGAM);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GamRepository.UpdateGam:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GamRepository.UpdateGam: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteGam(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GamDAL objDAL = new GamDAL();
                objDAL.DeleteGam(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GamRepository.DeleteGam: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
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
    public class FlyPurposeRepository
    {
        public ReponseEntity GetPageFlyPurpose(int page_size, int page_index,string where)
        {
            return new ReponseEntityHelper().GetPage<FlyPurpose>(new FlyPurposeDAL().GetPageFlyPurpose(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageFlyPurposeExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<FlyPurpose>(new FlyPurposeDAL().GetPageFlyPurposeExport(where).Tables[0]);
        }

        public ReponseEntity GetAllFlyPurpose()
        {
            return new ReponseEntityHelper().GetAll<FlyPurpose>(new FlyPurposeDAL().GetAllFlyPurpose().Tables[0]);
        }

        public ReponseEntity GetByIdFlyPurpose(int Id)
        {
            return new ReponseEntityHelper().GetByID<FlyPurpose>(new FlyPurposeDAL().GetByIdFlyPurpose(Id).Tables[0]);
        }

        public ReponseEntity CreateFlyPurpose(FlyPurpose oFLYPURPOSE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FlyPurposeDAL objDAL = new FlyPurposeDAL();
                Int32 Id = objDAL.CreateFlyPurpose(oFLYPURPOSE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "FlyPurposeRepository.CreateFlyPurpose:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FlyPurposeRepository.CreateFlyPurpose: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateFlyPurpose(FlyPurpose oFLYPURPOSE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FlyPurposeDAL objDAL = new FlyPurposeDAL();
                Int32 Id = objDAL.UpdateFlyPurpose(oFLYPURPOSE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "FlyPurposeRepository.UpdateFlyPurpose:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FlyPurposeRepository.UpdateFlyPurpose: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteFlyPurpose(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                FlyPurposeDAL objDAL = new FlyPurposeDAL();
                objDAL.DeleteFlyPurpose(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "FlyPurposeRepository.DeleteFlyPurpose: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
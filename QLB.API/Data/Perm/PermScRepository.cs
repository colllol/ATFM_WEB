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
    public class PermScRepository
    {
        public ReponseEntity GetPagePermScIMP(int page_size, int page_index, string where)
        {
           
            return new ReponseEntityHelper().GetPage<PermScIMP>(new PermScImpDAL().GetPagePermScIMP(page_size, page_index, where).Tables[0]);

        }

        public ReponseEntity GetPagePermScIMPExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<PermScIMP>(new PermScImpDAL().GetPagePermScIMPExport(where).Tables[0]);
        }

        public ReponseEntity GetAllPermSCIMP()
        {
            return new ReponseEntityHelper().GetAll<PermScIMP>(new PermScImpDAL().GetAllPermSCIMP().Tables[0]);
        }

        public ReponseEntity GetByIdPermSCIMP(int Id)
        {
            return new ReponseEntityHelper().GetByID<PermScIMP>(new PermScImpDAL().GetByIdPERMSC_IMP(Id).Tables[0]);
        }
        
        public ReponseEntity UpdatePermSCIMP(PermScIMP oPermScIMP)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermScImpDAL objDAL = new PermScImpDAL();
                Int32 Id = objDAL.UpdatePERMSC_IMP(oPermScIMP);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermScRepository.UpdatePermSCIMP:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermScRepository.UpdatePermSCIMP: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeletePermSCIMP(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermScImpDAL objDAL = new PermScImpDAL();
                objDAL.DeletePERMSC_IMP(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermScRepository.DeletePermSCIMP: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
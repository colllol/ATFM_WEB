using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
//'using Oracle.ManagedDataAccess;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class PermNoRepository
    {
        public ReponseEntity GetPagePermNoIMP(int page_size, int page_index, string where)
        {

            return new ReponseEntityHelper().GetPage<PermNoIMP>(new PermNoImpDAL().GetPagePermNoIMP(page_size, page_index, where).Tables[0]);

        }

        public ReponseEntity GetPagePermNoIMPExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<PermNoIMP>(new PermNoImpDAL().GetPagePermNoIMPExport(where).Tables[0]);
        }

        public ReponseEntity GetAllPermNOIMP()
        {
            return new ReponseEntityHelper().GetAll<PermNoIMP>(new PermNoImpDAL().GetAllPermNOIMP().Tables[0]);
        }

        public ReponseEntity GetByIdPermNOIMP(int Id)
        {
            return new ReponseEntityHelper().GetByID<PermNoIMP>(new PermNoImpDAL().GetByIdPERMNO_IMP(Id).Tables[0]);
        }

        public ReponseEntity UpdatePermNOIMP(PermNoIMP oPermNoIMP)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermNoImpDAL objDAL = new PermNoImpDAL();
                Int32 Id = objDAL.UpdatePERMNO_IMP(oPermNoIMP);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermNoRepository.UpdatePermNOIMP:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermNoRepository.UpdatePermNOIMP: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeletePermNOIMP(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermNoImpDAL objDAL = new PermNoImpDAL();
                objDAL.DeletePERMNO_IMP(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermNoRepository.DeletePermNOIMP: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
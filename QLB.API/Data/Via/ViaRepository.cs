using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
using QLB.BusinessLogic;


namespace QLB.API.Data
{
    public class ViaRepository
    {
        public ReponseEntity GetPageVia(int page_size, int page_index, string where)
        {            
            return new ReponseEntityHelper().GetPage<Via>(new ViaDAL().GetPageVia(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageViaExport(string where)
        {
           
            return new ReponseEntityHelper().GetPageExport<Via>(new ViaDAL().GetPageViaExport(where).Tables[0]);
        }

        public ReponseEntity GetAllVia()
        {
            return new ReponseEntityHelper().GetAll<Via>(new ViaDAL().GetAllVia().Tables[0]);
        }

        public ReponseEntity GetByIdVia(int Id)
        {
             return new ReponseEntityHelper().GetByID<Via>(new ViaDAL().GetByIdVia(Id).Tables[0]);
        }

        public ReponseEntity CreateVia(Via oVia)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                Int64 Id = objDAL.CreateVia(oVia);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.CreateAero:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.CreateVia: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
            
        }

        public ReponseEntity UpdateVia(Via oVia)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                Int64 Id = objDAL.UpdateVia(oVia);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.UpdateVia:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.UpdateVia: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteVia(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                objDAL.DeleteVia(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.DeleteVia: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity DeleteViaImport(ViaSearch obj)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                oResponse.Code = objDAL.DeleteViaBy(obj).ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.DeleteVia: " + ex.Message);
                oResponse.Code = "-1";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        ////////////////////////////////////////////////////////////////

        public ReponseEntity GetPageM_VIA_ARIPORT(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<M_VIA_ARIPORT>(new ViaDAL().GetPageM_VIA_ARIPORT(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageM_VIA_ARIPORTExport(string where)
        {

            return new ReponseEntityHelper().GetPageExport<M_VIA_ARIPORT>(new ViaDAL().GetPageM_VIA_ARIPORTExport(where).Tables[0]);
        }

       
        public ReponseEntity GetByIdM_VIA_ARIPORT(int Id)
        {
            return new ReponseEntityHelper().GetByID<M_VIA_ARIPORT>(new ViaDAL().GetByIdM_VIA_ARIPORT(Id).Tables[0]);
        }

        public ReponseEntity CreateM_VIA_ARIPORT(M_VIA_ARIPORT oVia)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                Int64 Id = objDAL.CreateM_VIA_ARIPORT(oVia);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.CreateAero:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.CreateM_VIA_ARIPORT: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;

        }

        public ReponseEntity UpdateM_VIA_ARIPORT(M_VIA_ARIPORT oVia)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                Int64 Id = objDAL.UpdateM_VIA_ARIPORT(oVia);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.UpdateVia:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.UpdateM_VIA_ARIPORT: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteM_VIA_ARIPORT(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ViaDAL objDAL = new ViaDAL();
                objDAL.DeleteM_VIA_ARIPORT(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ViaRepository.DeleteM_VIA_ARIPORT: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }




    }
}
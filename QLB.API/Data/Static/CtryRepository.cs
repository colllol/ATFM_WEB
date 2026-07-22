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
    public class CtryRepository
    {      
        public ReponseEntity GetPageCtry(int page_size, int page_index,string where)
        {
            return new ReponseEntityHelper().GetPage<Ctry>(new CtryDAL().GetPageCtry(page_size, page_index, where).Tables[0]);
        }    
        public ReponseEntity GetPageCtryExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Ctry>(new CtryDAL().GetPageCtryExport(where).Tables[0]);
        }        
        public ReponseEntity GetAllCtry()
        {
            return new ReponseEntityHelper().GetAll<Ctry>(new CtryDAL().GetAllCtry().Tables[0]);
        }

        public ReponseEntity GetByIdCtry(int Id)
        {
            return new ReponseEntityHelper().GetByID<Ctry>(new CtryDAL().GetByIdCtry(Id).Tables[0]);
        }

        public ReponseEntity CreateCtry(Ctry oCTRY)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CtryDAL objDAL = new CtryDAL();
                Int32 Id =  objDAL.CreateCtry(oCTRY);
                oResponse.Code = Id.ToString();
                if (Id==-1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CtryRepository.CreateCtry:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }
                  
            }
            catch(Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CtryRepository.CreateCtry: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        
        public ReponseEntity UpdateCtry(Ctry oCTRY)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CtryDAL objDAL = new CtryDAL();
                Int32 Id = objDAL.UpdateCtry(oCTRY);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CtryRepository.UpdateCtry:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CtryRepository.UpdateCtry: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteCtry(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CtryDAL objDAL = new CtryDAL();
                objDAL.DeleteCtry(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CtryRepository.DeleteCtry: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
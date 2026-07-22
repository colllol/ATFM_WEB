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
    public class AirlinesRepository
    {
        public ReponseEntity GetPageAirlines(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<Airlines>(new AirlinesDAL().GetPageAirlines(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageAirlinesExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Airlines>(new AirlinesDAL().GetPageAirlinesExport(where).Tables[0]);
        }        

        public ReponseEntity GetAllAirlines()
        {
            return new ReponseEntityHelper().GetAll<Airlines>(new AirlinesDAL().GetAllAirlines().Tables[0]);
        }

        public ReponseEntity GetByIdAirlines(int Id)
        {
            return new ReponseEntityHelper().GetByID<Airlines>(new AirlinesDAL().GetByIdAirlines(Id).Tables[0]);
        }

        public ReponseEntity CreateAirlines(Airlines oAIRLINES)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirlinesDAL objDAL = new AirlinesDAL();
                Int32 Id = objDAL.CreateAirlines(oAIRLINES);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AirlinesRepository.CreateAirlines:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirlinesRepository.CreateAirlines: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateAirlines(Airlines oAIRLINES)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirlinesDAL objDAL = new AirlinesDAL();
                Int32 Id = objDAL.UpdateAirlines(oAIRLINES);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AirlinesRepository.UpdateAirlines:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirlinesRepository.UpdateAirlines: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteAirlines(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirlinesDAL objDAL = new AirlinesDAL();
                objDAL.DeleteAirlines(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirlinesRepository.DeleteAirlines: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
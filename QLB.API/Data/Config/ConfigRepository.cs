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
    public class ConfigRepository
    {
        public ReponseEntity GetPageConfig(int page_size, int page_index, string where)
        {            
            return new ReponseEntityHelper().GetPage<Config>(new ConfigDAL().GetPageConfig(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageConfigExport(string where)
        {
           
            return new ReponseEntityHelper().GetPageExport<Config>(new ConfigDAL().GetPageConfigExport(where).Tables[0]);
        }

        public ReponseEntity GetAllConfig()
        {
            return new ReponseEntityHelper().GetAll<Config>(new ConfigDAL().GetAllConfig().Tables[0]);
        }

        public ReponseEntity GetByIdConfig(int Id)
        {
             return new ReponseEntityHelper().GetByID<Config>(new ConfigDAL().GetByIdConfig(Id).Tables[0]);
        }

        public ReponseEntity CreateConfig(Config oConfig)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ConfigDAL objDAL = new ConfigDAL();
                Int64 Id = objDAL.CreateConfig(oConfig);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ConfigRepository.CreateConfig:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ConfigRepository.CreateConfig: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
            
        }

        public ReponseEntity UpdateConfig(Config oConfig)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ConfigDAL objDAL = new ConfigDAL();
                Int64 Id = objDAL.UpdateConfig(oConfig);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ConfigRepository.UpdateConfig:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ConfigRepository.UpdateConfig: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteConfig(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ConfigDAL objDAL = new ConfigDAL();
                objDAL.DeleteConfig(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ConfigRepository.DeleteConfig: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
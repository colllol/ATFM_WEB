using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using QLB.BusinessLogic;
using System.Data;
using QLB.Info.System;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic.Perm;

namespace QLB.API.Data.System
{
    public class ActionHistoryRepository
    {
        public ReponseEntity GetPage(int page_size, int page_index, string where, string Fromdate, string Todate, string FullName)
        {
            return new ReponseEntityHelper().GetPage<ActionHistory>(new ActionHistoryDAL().GetPage(page_size, page_index, where, Fromdate, Todate, FullName));
        }
        public ReponseEntity GetPageActionHistory(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<ActionHistory>(new ActionHistoryDAL().GetPageActionHistory(page_size,page_index,where).Tables[0]);
        }
        public ReponseEntity GetAllActionHistory()
        {
            return new ReponseEntityHelper().GetAll<ActionHistory>(new ActionHistoryDAL().GetAllActionHistory().Tables[0]);
        }
        //Create  
        public ReponseEntity CreateActionHistory(ActionHistory oACTIONHISTORY)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ActionHistoryDAL objDAL = new ActionHistoryDAL();
                Int64 Id = objDAL.CreateActionHistory(oACTIONHISTORY);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ActionHistoryRepository.CreateActionHistory:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ActionHistoryRepository.CreateActionHistory: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateActionHistory(ActionHistory oACTIONHISTORY)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ActionHistoryDAL objDAL = new ActionHistoryDAL();
                Int64 Id = objDAL.UpdateActionHistory(oACTIONHISTORY);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ActionHistoryRepository.UpdateActionHistory:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ActionHistoryRepository.UpdateActionHistory: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteActionHistory(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ActionHistoryDAL objDAL = new ActionHistoryDAL();
                objDAL.DeleteActionHistory(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ActionHistoryRepository.DeleteActionHistory: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
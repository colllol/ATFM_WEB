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
    public class AlarmRepository
    {
        public ReponseEntity GetPageAlarm(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<Alarm>(new AlarmDAL().GetPageAlarm(page_size, page_index, where).Tables[0]);
        }
        public ReponseEntity GetPageAlarmExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Alarm>(new AlarmDAL().GetPageAlarmExport(where).Tables[0]);
        }
        public ReponseEntity GetAllAlarm()
        {
            return new ReponseEntityHelper().GetAll<Alarm>(new AlarmDAL().GetAllAlarm().Tables[0]);
        }

        public ReponseEntity GetByIdAlarm(int Id)
        {
            return new ReponseEntityHelper().GetByID<Alarm>(new AlarmDAL().GetByIdAlarm(Id).Tables[0]);
        }

        public ReponseEntity CreateAlarm(Alarm oAlarm)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AlarmDAL objDAL = new AlarmDAL();
                Int32 Id = objDAL.CreateAlarm(oAlarm);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AlarmRepository.CreateAlarm:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AlarmRepository.CreateAlarm: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateAlarm(Alarm oAlarm)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AlarmDAL objDAL = new AlarmDAL();
                Int32 Id = objDAL.UpdateAlarm(oAlarm);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AlarmRepository.UpdateAlarm:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AlarmRepository.UpdateAlarm: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteAlarm(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AlarmDAL objDAL = new AlarmDAL();
                objDAL.DeleteAlarm(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AlarmRepository.DeleteAlarm: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }        
        public ReponseReportEntity GetCountAlarmByDate()
        {
            return new ReponseEntityHelper().GetTable(new AlarmDAL().GetCountAlarmBySystemDate());
        }
    }
}
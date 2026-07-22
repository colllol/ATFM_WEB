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
    public class CalendarBackupRepository
    {      
        //Search by ID
        public ReponseEntity GetByIdCalendarBackup(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            CalendarBackupDAL objDAL = new CalendarBackupDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetByIdCalendarBackup(Id).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<CalendarBackup> lstCALENDARBACKUP = new List<CalendarBackup>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        CalendarBackup obj = new CalendarBackup();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = int.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = int.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = int.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
                        if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
                        if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
                        if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
                        if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
                        if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
                        if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
                        if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
                        if (_dt.Rows[i]["LOG_HISTORY"] != DBNull.Value) obj.LOG_HISTORY = int.Parse(_dt.Rows[i]["LOG_HISTORY"].ToString());
                        if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
                        if (_dt.Rows[i]["LAST_USER"] != DBNull.Value) obj.LAST_USER = _dt.Rows[i]["LAST_USER"].ToString();
                        if (_dt.Rows[i]["TIME_UPDATE"] != DBNull.Value) obj.TIME_UPDATE = DateTime.Parse(_dt.Rows[i]["TIME_UPDATE"].ToString());
                        if (_dt.Rows[i]["STATUS_DEL"] != DBNull.Value) obj.STATUS_DEL = _dt.Rows[i]["STATUS_DEL"].ToString();
                        lstCALENDARBACKUP.Add(obj);
                    }
                    if (lstCALENDARBACKUP != null || lstCALENDARBACKUP.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstCALENDARBACKUP.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarBackupRepository.GetByIdCalendarBackup:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;
        }
        //Create  
        public ReponseEntity CreateCalendarBackup(CalendarBackup oCALENDARBACKUP)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarBackupDAL objDAL = new CalendarBackupDAL();
                Int32 Id = objDAL.CreateCalendarBackup(oCALENDARBACKUP);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarBackupRepository.CreateCalendarBackup: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        //Restore
        public ReponseEntity RestoreCalendarBackup(CalendarBackup oCALENDARBACKUP)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarBackupDAL objDAL = new CalendarBackupDAL();
                Int32 Id = objDAL.RestoreCalendarBackup(oCALENDARBACKUP);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarBackupRepository.RestoreCalendarBackup: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        //Update
        public ReponseEntity UpdateCalendarBackup(CalendarBackup oCALENDARBACKUP)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                CalendarBackupDAL objDAL = new CalendarBackupDAL();
                Int32 Id = objDAL.UpdateCalendarBackup(oCALENDARBACKUP);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarBackupRepository.UpdateCalendarBackup: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteCalendarBackup(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarBackupDAL objDAL = new CalendarBackupDAL();
                objDAL.DeleteCalendarBackup(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarBackupRepository.DeleteCalendarBackup: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
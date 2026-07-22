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
    public class CalendarDetailRepository
    {
        //Get Page

        public ReponseEntity GetPageCalendarDetail(int page_size, int page_index, string where)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //CalendarDetailDAL objDAL = new CalendarDetailDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageCalendarDetail(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountCalendarDetail(where).Tables[0];
            //    //DataTable dtCount = new DataTable();

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<CalendarDetail> lstCALENDARDETAIL = new List<CalendarDetail>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            CalendarDetail obj = new CalendarDetail();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = int.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
            //            if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
            //            if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = int.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CARAFT_ID = int.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
            //            if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
            //            if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
            //            if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
            //            if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
            //            if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
            //            if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
            //            if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
            //            if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
            //            if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
            //            if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["LAST_USER"] != DBNull.Value) obj.LAST_USER = _dt.Rows[i]["LAST_USER"].ToString();
            //            if (_dt.Rows[i]["TIME_UPDATE"] != DBNull.Value) obj.TIME_UPDATE = DateTime.Parse(_dt.Rows[i]["TIME_UPDATE"].ToString());

            //            lstCALENDARDETAIL.Add(obj);

            //        }
            //        if (lstCALENDARDETAIL != null || lstCALENDARDETAIL.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDARDETAIL.ToList<object>();
            //            oResponse.Value = _dt1.Rows[0][0];
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetPageCalendarDetail:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetPageCalendarDetail:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<CalendarDetail>(new CalendarDetailDAL().GetPageCalendarDetail(page_size, page_index, where).Tables[0]);
        }

        //GetCount
        public ReponseEntity GetCountCalendarDetail(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            CalendarDetailDAL objDAL = new CalendarDetailDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetCountCalendarDetail(where).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<CalendarDetail> lstCALENDARDETAIL = new List<CalendarDetail>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        CalendarDetail obj = new CalendarDetail();

                        if (_dt.Rows[i][0] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i][0].ToString());


                        lstCALENDARDETAIL.Add(obj);
                    }
                    if (lstCALENDARDETAIL != null || lstCALENDARDETAIL.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstCALENDARDETAIL.ToList<object>();

                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetCountCalendarDetail:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetCountCalendarDetail:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }


        //Get ALL
        public ReponseEntity GetAllCalendarDetail()
        {
            #region old code
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //CalendarDetailDAL objDAL = new CalendarDetailDAL();
            //DataTable _dt = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetAllCalendarDetail().Tables[0];

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<CalendarDetail> lstCALENDARDETAIL = new List<CalendarDetail>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            CalendarDetail obj = new CalendarDetail();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = int.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
            //            if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
            //            if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = int.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CARAFT_ID = int.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
            //            if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
            //            if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
            //            if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
            //            if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
            //            if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
            //            if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
            //            if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
            //            if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
            //            if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
            //            if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["LAST_USER"] != DBNull.Value) obj.LAST_USER = _dt.Rows[i]["LAST_USER"].ToString();
            //            if (_dt.Rows[i]["TIME_UPDATE"] != DBNull.Value) obj.TIME_UPDATE = DateTime.Parse(_dt.Rows[i]["TIME_UPDATE"].ToString());

            //            lstCALENDARDETAIL.Add(obj);
            //        }
            //        if (lstCALENDARDETAIL != null || lstCALENDARDETAIL.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDARDETAIL.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetAllCalendarDetail:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetAllCalendarDetail:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetAll<CalendarDetail>(new CalendarDetailDAL().GetAllCalendarDetail().Tables[0]);
        }

        //Search by ID
        public ReponseEntity GetByIdCalendarDetail(Int64 Id)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //CalendarDetailDAL objDAL = new CalendarDetailDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = objDAL.GetByIdCalendarDetail(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<CalendarDetail> lstCALENDARDETAIL = new List<CalendarDetail>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            CalendarDetail obj = new CalendarDetail();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = int.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
            //            if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
            //            if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = int.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CARAFT_ID = int.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
            //            if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
            //            if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
            //            if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
            //            if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
            //            if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
            //            if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
            //            if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
            //            if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
            //            if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
            //            if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["LAST_USER"] != DBNull.Value) obj.LAST_USER = _dt.Rows[i]["LAST_USER"].ToString();
            //            if (_dt.Rows[i]["TIME_UPDATE"] != DBNull.Value) obj.TIME_UPDATE = DateTime.Parse(_dt.Rows[i]["TIME_UPDATE"].ToString());

            //            lstCALENDARDETAIL.Add(obj);
            //        }
            //        if (lstCALENDARDETAIL != null || lstCALENDARDETAIL.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            //oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDARDETAIL.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.GetByIdCalendarDetail:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<CalendarDetail>(new CalendarDetailDAL().GetByIdCalendarDetail(Id).Tables[0]);
        }

        //Create  
        public ReponseEntity CreateCalendarDetail(CalendarDetail oCALENDARDETAIL)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDetailDAL objDAL = new CalendarDetailDAL();
                Int64 Id = objDAL.InsertObject(oCALENDARDETAIL);
                oResponse.Code = Id.ToString();
                //lngResult = long.Parse(GetStoreDataSet_Oracle.Parameters["P_RETURN_CODE"].Value.ToString());
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.CreateCalendarDetail:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                {
                    oResponse.Message = "Cập nhật dữ liệu thành công";
                    oResponse.Code = Id.ToString();
                }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.CreateCalendarDetail: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateCalendarDetail(CalendarDetail oCALENDARDETAIL)
        {
             string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDetailDAL objDAL = new CalendarDetailDAL();
                Int64 Id = objDAL.UpdateObject(oCALENDARDETAIL);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.UpdateCalendarDetail:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                {
                    oResponse.Code = Id.ToString();
                    oResponse.Message = "Cập nhật dữ liệu thành công";
                }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.UpdateCalendarDetail: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteCalendarDetail(Int64 Id)
        {
            #region old code
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();

            //try
            //{
            //    CalendarDetailDAL objDAL = new CalendarDetailDAL();
            //    objDAL.DeleteCalendarDetail(Id);
            //    oResponse.Code = Id.ToString();
            //    oResponse.Message = "Cập nhật dữ liệu thành công";
            //}

            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarDetailRepository.DeleteCalendarDetail: " + ex.Message);
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //}
            //return oResponse;
            #endregion
            return new ReponseEntityHelper().Delete<CalendarDetail, CalendarDetailDAL>(Id, "DeleteCalendarDetail");
        }
        public ReponseEntity RestoreHis(Int64 id, string iduser, int version)
        {
            return new ReponseEntityHelper().RestoreHis<CalendarDetail, CalendarDetailDAL>(id, version, iduser, "RestoreHis");
        }
        public ReponseEntity GetHis(string id)
        {
            return new ReponseEntityHelper().GetHistoryByID<CalendarDetail>(new CalendarDetailDAL().GetHistoryById(id));
        }
    }
}
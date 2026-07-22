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
    public class CalendarRepository
    {
        //Get Page

        public ReponseEntity GetPageCalendar(int page_size, int page_index, string where)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //CalendarDAL objDAL = new CalendarDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageCalendar(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountCalendar(where).Tables[0];
            //    //DataTable dtCount = new DataTable();

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Calendar> lstCALENDAR = new List<Calendar>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Calendar obj = new Calendar();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_NAME"] != DBNull.Value) obj.CALENDAR_NAME = _dt.Rows[i]["CALENDAR_NAME"].ToString();
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["MAKING_DATE"] != DBNull.Value) obj.MAKING_DATE = DateTime.Parse(_dt.Rows[i]["MAKING_DATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["BEGIN_DATE"] != DBNull.Value) obj.BEGIN_DATE = DateTime.Parse(_dt.Rows[i]["BEGIN_DATE"].ToString());
            //            if (_dt.Rows[i]["FINISH_DATE"] != DBNull.Value) obj.FINISH_DATE = DateTime.Parse(_dt.Rows[i]["FINISH_DATE"].ToString());
            //            if (_dt.Rows[i]["SEASON"] != DBNull.Value) obj.SEASON = _dt.Rows[i]["SEASON"].ToString();
            //            if (_dt.Rows[i]["YEAR"] != DBNull.Value) obj.YEAR = _dt.Rows[i]["YEAR"].ToString();
            //            if (_dt.Rows[i]["TYPE"] != DBNull.Value) obj.TYPE = _dt.Rows[i]["TYPE"].ToString();

            //            lstCALENDAR.Add(obj);

            //        }
            //        if (lstCALENDAR != null || lstCALENDAR.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDAR.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetPageCalendar:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetPageCalendar:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<Calendar>(new CalendarDAL().GetPageCalendar(page_size, page_index, where).Tables[0]);
        }

        //GetCount
        public ReponseEntity GetCountCalendar(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            CalendarDAL objDAL = new CalendarDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetCountCalendar(where).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Calendar> lstCALENDAR = new List<Calendar>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Calendar obj = new Calendar();

                        if (_dt.Rows[i][0] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i][0].ToString());


                        lstCALENDAR.Add(obj);
                    }
                    if (lstCALENDAR != null || lstCALENDAR.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstCALENDAR.ToList<object>();

                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetCountCalendar:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetCountCalendar:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }


        //Get ALL
        public ReponseEntity GetAllCalendar()
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //CalendarDAL objDAL = new CalendarDAL();
            //DataTable _dt = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetAllCalendar().Tables[0];

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Calendar> lstCALENDAR = new List<Calendar>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Calendar obj = new Calendar();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_NAME"] != DBNull.Value) obj.CALENDAR_NAME = _dt.Rows[i]["CALENDAR_NAME"].ToString();
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["MAKING_DATE"] != DBNull.Value) obj.MAKING_DATE = DateTime.Parse(_dt.Rows[i]["MAKING_DATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["BEGIN_DATE"] != DBNull.Value) obj.BEGIN_DATE = DateTime.Parse(_dt.Rows[i]["BEGIN_DATE"].ToString());
            //            if (_dt.Rows[i]["FINISH_DATE"] != DBNull.Value) obj.FINISH_DATE = DateTime.Parse(_dt.Rows[i]["FINISH_DATE"].ToString());
            //            if (_dt.Rows[i]["SEASON"] != DBNull.Value) obj.SEASON = _dt.Rows[i]["SEASON"].ToString();
            //            if (_dt.Rows[i]["YEAR"] != DBNull.Value) obj.YEAR = _dt.Rows[i]["YEAR"].ToString();
            //            if (_dt.Rows[i]["TYPE"] != DBNull.Value) obj.TYPE = _dt.Rows[i]["TYPE"].ToString();

            //            lstCALENDAR.Add(obj);
            //        }
            //        if (lstCALENDAR != null || lstCALENDAR.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDAR.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetAllCalendar:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetAllCalendar:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetAll<Calendar>(new CalendarDAL().GetAllCalendar().Tables[0]);
        }

        //Search by ID
        public ReponseEntity GetByIdCalendar(Int64 Id)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //CalendarDAL objDAL = new CalendarDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = objDAL.GetByIdCalendar(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Calendar> lstCALENDAR = new List<Calendar>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Calendar obj = new Calendar();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_ID"] != DBNull.Value) obj.CALENDAR_ID = int.Parse(_dt.Rows[i]["CALENDAR_ID"].ToString());
            //            if (_dt.Rows[i]["CALENDAR_NAME"] != DBNull.Value) obj.CALENDAR_NAME = _dt.Rows[i]["CALENDAR_NAME"].ToString();
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["MAKING_DATE"] != DBNull.Value) obj.MAKING_DATE = DateTime.Parse(_dt.Rows[i]["MAKING_DATE"].ToString());
            //            if (_dt.Rows[i]["USER_NAME"] != DBNull.Value) obj.USER_NAME = _dt.Rows[i]["USER_NAME"].ToString();
            //            if (_dt.Rows[i]["BEGIN_DATE"] != DBNull.Value) obj.BEGIN_DATE = DateTime.Parse(_dt.Rows[i]["BEGIN_DATE"].ToString());
            //            if (_dt.Rows[i]["FINISH_DATE"] != DBNull.Value) obj.FINISH_DATE = DateTime.Parse(_dt.Rows[i]["FINISH_DATE"].ToString());
            //            if (_dt.Rows[i]["SEASON"] != DBNull.Value) obj.SEASON = _dt.Rows[i]["SEASON"].ToString();
            //            if (_dt.Rows[i]["YEAR"] != DBNull.Value) obj.YEAR = _dt.Rows[i]["YEAR"].ToString();
            //            if (_dt.Rows[i]["TYPE"] != DBNull.Value) obj.TYPE = _dt.Rows[i]["TYPE"].ToString();

            //            lstCALENDAR.Add(obj);
            //        }
            //        if (lstCALENDAR != null || lstCALENDAR.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            //oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCALENDAR.ToList<object>();
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
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.GetByIdCalendar:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<Calendar>(new CalendarDAL().GetByIdCalendar(Id).Tables[0]);
        }

        //Create  
        public ReponseEntity CreateCalendar(Calendar oCALENDAR)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDAL objDAL = new CalendarDAL();
                var Id = objDAL.CreateCalendarReturnStringID(oCALENDAR);
                oResponse.Code = Id.ToString();
                if (Id == "-1")
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == "-99")
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.CreateCalendar:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.CreateCalendar: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateCalendar(Calendar oCALENDAR)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDAL objDAL = new CalendarDAL();
                Int64 Id = objDAL.UpdateObject(oCALENDAR);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.UpdateCalendar:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.UpdateCalendar: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteCalendar(Int64 Id)
        {
            #region old code
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();

            //try
            //{
            //    CalendarDAL objDAL = new CalendarDAL();
            //    objDAL.DeleteCalendar(Id);
            //    oResponse.Code = Id.ToString();
            //    oResponse.Message = "Cập nhật dữ liệu thành công";
            //}

            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.DeleteCalendar: " + ex.Message);
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //}
            //return oResponse;
            #endregion
            return new ReponseEntityHelper().Delete<Calendar, CalendarDAL>(Id, "DeleteCalendar");
        }

        public ReponseEntity InsertObject(Calendar obj)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDAL objDAL = new CalendarDAL();
                var Id = objDAL.InsertObject(obj);
                oResponse.Code = Id.ToString();
                if (Id.ToString() == "-1")
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id.ToString() == "-99")
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.CreateCalendar:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.CreateCalendar: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity UpdateObject(Calendar oCALENDAR)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CalendarDAL objDAL = new CalendarDAL();
                Int64 Id = objDAL.UpdateObject(oCALENDAR);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.UpdateCalendar:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CalendarRepository.UpdateCalendar: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

    }
}
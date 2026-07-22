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
using System.Reflection;

namespace QLB.API.Data
{
    public class DayFlightsRepository
    {
        public ReponseEntity GetPage(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<DayFlights>(new DayFlightsDAL().GetPage(page_size, page_index, where));
        }
        public ReponseEntity GetPageTrungLap(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<DayFlights>(new DayFlightsDAL().GetPageTrungLap(page_size, page_index, where));
        }
        public ReponseReportEntity GetAll()
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetAll());
        }
        public ReponseReportEntity GetPermById(Int64 id)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetPermById(id));
        }

        public ReponseReportEntity GetPerm_GoingOnById(Int64 id)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetPerm_GoingOnById(id));
        }

        
        public ReponseReportEntity GetLinkFile(Int64 id, string permtype)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetLinkFile(id, permtype));
        }
        public ReponseReportEntity GetByID(Int64 Id)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetById(Id));
        }
        public ReponseEntity Insert(DayFlights obj)
        {
            return new ReponseEntityHelper().Insert<DayFlights, DayFlightsDAL>(obj, "Insert");
        }

        public ReponseEntity Insert_Manual(DayFlights obj)
        {
            return new ReponseEntityHelper().Insert<DayFlights, DayFlightsDAL>(obj, "Insert_Manual");
        }

        public ReponseEntity Insert_Manual_GoingOn(DayFlights_GoingOn obj)
        {
            return new ReponseEntityHelper().Insert<DayFlights_GoingOn, DayFlightsDAL>(obj, "Insert_Manual_GoingOn");

        }
        public ReponseEntity Update_Manual_GoingOn(DayFlights_GoingOn obj)
        {
            return new ReponseEntityHelper().Update<DayFlights_GoingOn, DayFlightsDAL>(obj, "Update_Manual_GoingOn");
        }
        public ReponseEntity Update_Manual_GoingOn_DB(DayFlights_GoingOn obj)
        {
            return new ReponseEntityHelper().Update<DayFlights_GoingOn, DayFlightsDAL>(obj, "Update_Manual_GoingOn_DB");
        }

        public ReponseEntity Update(DayFlights obj)
        {
            return new ReponseEntityHelper().Update<DayFlights, DayFlightsDAL>(obj, "Update");
        }
        public ReponseEntity Delete(Int64 Id)
        {
            return new ReponseEntityHelper().Delete<DayFlights, DayFlightsDAL>(Id, "Delete");
        }

        public ReponseEntity WaitDelete(Int64 Id, Int64 Status)
        {
            return new ReponseEntityHelper().WaitDelete<DayFlights, DayFlightsDAL>(Id, Status, "WaitDelete");
        }

        public ReponseEntity MoveDate(Int64 Id)
        {
            return new ReponseEntityHelper().Movedate<DayFlights, DayFlightsDAL>(Id, "MoveDate");
        }
        public ReponseEntity MoveDate(DayFlights obj)
        {
            return new ReponseEntityHelper().Update<DayFlights, DayFlightsDAL>(obj, "MoveDate");
        }

        public ReponseEntity GetHistoryByID(Int64 id)
        {
            return new ReponseEntityHelper().GetHistoryByID<DayFlights_BAK>(new DayFlightsDAL().GetHistoryById(id));
        }
        public ReponseEntity RestoreHis(Int64 id, int version, string iduser)
        {
            return new ReponseEntityHelper().RestoreHis<DayFlights, DayFlightsDAL>(id, version, iduser, "RestoreHis");
        }
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new ReponseEntityHelper().GetRecordDeleted<DayFlights_BAK>(new DayFlightsDAL().GetDeleted(where));
        }
        public ReponseEntity GetDaylyFlightInfo()
        {
            return new ReponseEntityHelper().GetAll<DaylyFlightTotalInfo>(new DaylyFlightTotalInfoDAL().GetAll());
        }
        public ReponseReportEntity GetDayFlightByValueSearch(clsDayFlightValueSearch obj)
        {
            
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetAllForValueSearch(obj));
        }

        public ReponseReportEntity GetDayFlightByValueSearch_New(clsDayFlightValueSearch obj)
        {

            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetAllForValueSearch_New(obj));
        }
        public ReponseReportEntity GetDayFlightByNgayNtru1(clsDayFlightValueSearch obj)
        {

            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetNgayNTru1(obj));
        }
        public ReponseReportEntity GetDayFlightByNgayNCong1(clsDayFlightValueSearch obj)
        {

            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetNgayNCong1(obj));
        }
        public ReponseReportEntity GetDayFlightByValueSearch_HCM(clsDayFlightValueSearch obj)
        {

            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetAllForValueSearch_HCM(obj));
        }
        public ReponseReportEntity GetDayFlightByValueSearch_DNG(clsDayFlightValueSearch obj)
        {

            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().GetAllForValueSearch_DNG(obj));
        }
        public ReponseReportEntity GetMessage(DateTime date, string FlighNBR)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().FlightInfoExtension_DienVan(date, FlighNBR));
        }
        public ReponseEntity GetDaylyFlightInfo_Hcm(string lis)
        {
            return new ReponseEntityHelper().GetAll<DaylyFlightTotalInfo>(new DaylyFlightTotalInfoDAL().GetDivInfo_HCM(lis));
        }
        public ReponseEntity GetDaylyFlightInfo_Dng(string lis)
        {
            return new ReponseEntityHelper().GetAll<DaylyFlightTotalInfo>(new DaylyFlightTotalInfoDAL().GetDivInfo_Dng(lis));
        }
        #region Draft
        public ReponseEntity GetPageDraft(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<DayFlights>(new DayFlightsDAL().GetPageDraft(page_size, page_index, where));
        }
        public ReponseEntity GetPageDraftTrungLap(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<DayFlights>(new DayFlightsDAL().GetPageDraftTrungLap(page_size, page_index, where));
        }
        public ReponseEntity GetByIdDraft(Int64 id)
        {
            return new ReponseEntityHelper().GetByID<DayFlights>(new DayFlightsDAL().GetByIdDraft(id));
        }
        public ReponseEntity DeleteDraft(Int64 id, string user)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                int Id = new DayFlightsDAL().DeleteDraft(id,user);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"DeleteAllDraft");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"DeleteAllDraft: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity InsertDraft(DayFlights obj)
        {
            return new ReponseEntityHelper().Insert<DayFlights, DayFlightsDAL>(obj, "InsertDraft");
        }
        public ReponseEntity UpdateDraft(DayFlights obj)
        {
            return new ReponseEntityHelper().Update<DayFlights, DayFlightsDAL>(obj, "UpdateDraft");
        }
        public ReponseEntity DeleteAllDraft(string user)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                int Id = new DayFlightsDAL().DeleteAllDraft(user);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"DeleteAllDraft");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"DeleteAllDraft: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity GenToDraftByDate(DateTime date,string user)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                int Id = new DayFlightsDAL().GenToDraftByDate(date,user);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"GenToDraftByDate");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"GenToDraftByDate: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity AccessDraft(DateTime date, string user)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().AccessDraft(user, date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        #endregion

        #region ke hoach bay ngay
        public ReponseReportEntity LayKeHoachBayNgay(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachBayNgay(obj));
        }
        public ReponseReportEntity LayKeHoachBayNgayFull(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachBayNgayFull(obj));
        }
        public ReponseReportEntity LayKeHoachBayNgayOrigin(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachBayOrigin(obj));
        }
        public ReponseReportEntity LayKeHoachBayNgayTrungLap(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachBayNgay_TrungLap(obj));
        }
        public ReponseEntity AccessKeHoachBayNgay(string user, DateTime date)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().AccessKeHoachBay(user, date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }

        public ReponseEntity AccessKeHoachBayNgayNew(string user, DateTime date)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().AccessKeHoachBayNew(user, date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"AccessDraft: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }


        public ReponseEntity KeHoachBayToMessage(DateTime date)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().KhbToMessage(date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity KeHoachBayToMessageExt(DateTime date)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().KhbToMessageExt(date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }

        public ReponseEntity KeHoachBayToMessageByAirport(DateTime date)
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = new DayFlightsDAL().KhbToMessageByAirport(date);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"KhbToMessage: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseReportEntity LayKeHoachBayNgayDelete(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachBayNgay_Delete(obj));
        }
        #endregion

        #region Ke hoach bay venh
        public ReponseReportEntity LayKeHoachVenhTheoMua(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachVenhTheoMua(obj));
        }
        public ReponseReportEntity LayKeHoachVenhTheoThang(clsDayFlightValueSearch obj)
        {
            return new ReponseEntityHelper().GetTable(new DayFlightsDAL().LayKeHoachVenhTheoThang(obj));
        }
        #endregion

    }
}
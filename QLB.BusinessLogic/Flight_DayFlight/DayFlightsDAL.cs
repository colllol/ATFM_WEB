using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
namespace QLB.BusinessLogic
{
    public class DayFlightsDAL
    {
        public DayFlightsDAL()
        {

        }
        public DataTable GetAll()
        {
            try
            {
                return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GETALL", new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPage(int page_size, int page_index, string where)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GETPAGE"
                            , new OracleParameter("P_PAGE_SIZE", page_size)
                            , new OracleParameter("P_PAGE_INDEX", page_index)
                            , new OracleParameter("P_WHERE", where)).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPageTrungLap(int page_size, int page_index, string where)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetPageTrungLap"
                            , new OracleParameter("P_PAGE_SIZE", page_size)
                            , new OracleParameter("P_PAGE_INDEX", page_index)
                            , new OracleParameter("P_WHERE", where)).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPermById(Int64 id)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetPermById"
                            , new OracleParameter("P_ID", id)).Tables[0];
        }

        public DataTable GetPerm_GoingOnById(Int64 id)
        {
            return new oDataProvider().ExecuteDatase("A_TEST_SEARCH", "GetPermById_GoingOn"
                            , new OracleParameter("P_ID", id)).Tables[0];
        }

        public DataTable GetLinkFile(Int64 id, string permtype)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "fileUpload_Select"
                            , new OracleParameter("p_permId", id)
                            , new OracleParameter("p_permType", permtype)).Tables[0];
        }
        
        public DataTable GetHistoryById(Int64 id)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetHistoryById"
                , new OracleParameter("P_FLIGHT_ID", id)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
        public bool RestoreHis(Int64 id, int version, string idUser)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_FLIGHT_ID", id));
            lis.Add(new OracleParameter("P_Version", version));
            lis.Add(new OracleParameter("P_IdUser", idUser));
            lis.Add(new OracleParameter("P_Out", OracleDbType.Int64, ParameterDirection.Output));
            Int64 ax = new oDataProvider().ExecuteNonQuery("FLIGHT_DAYFLIGHT", "RestoreRecord", lis.ToArray());
            if (ax < 0) return false;
            return true;
        }
        public DataTable GetById(Int64 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("FLIGHT_DAYFLIGHT.GETBYID", new string[] { "p_ID" }, new object[] { Id }).Tables[0];
                return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GETBYID", new OracleParameter("p_ID", Id)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(Int64 ID)
        {
            try
            {
                new oDataProvider().ExecuteNonQuery("FLIGHT_DAYFLIGHT", "ODELETE", new OracleParameter("P_ID", ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WaitDelete(Int64 ID, Int64 Status)
        {
            try
            {
               // new oDataProvider().ExecuteNonQuery("FLIGHT_DAYFLIGHT", "WAITDELETE", new ///OracleParameter("P_ID", ID), new OracleParameter("p_WAITDEL", Status));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MoveDate(Int64 ID)
        {
            try
            {
                new oDataProvider().ExecuteNonQuery("FLIGHT_DAYFLIGHT", "OMOVEDATE", new OracleParameter("P_ID", ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 MoveDate(DayFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FLIGHT_DAYFLIGHT", "OMOVEDATE", obj);
            return (Int64)u;
        }
        public Int64 Insert(DayFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FLIGHT_DAYFLIGHT", "OINSERT", obj);
            return (Int64)u;
        }

        public Int64 Insert_Manual(DayFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FLIGHT_DAYFLIGHT", "OINSERT_MANUAL", obj);
            return (Int64)u;
        }

        public Int64 Insert_Manual_GoingOn(DayFlights_GoingOn obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FLIGHT_DAYFLIGHT", "OINSERT_MANUAL_GOINGON", obj);
            return (Int64)u;
        }

        public Int64 Update_Manual_GoingOn(DayFlights_GoingOn obj)
        {            
            var u = new oDataProvider().ExecuteReturnID("A_TEST_SEARCH", "OUPDATE_MANUAL_GOINGON", obj);
            return (Int64)u;
        }

        public Int64 Update_Manual_GoingOn_DB(DayFlights_GoingOn obj)
        {
            var u = new oDataProvider().ExecuteReturnID("A_TEST_SEARCH", "OUPDATE_MANUAL_GOINGON_DB", obj);
            return (Int64)u;
        }

        public Int64 Update(DayFlights obj)
        {
            var u = new oDataProvider().ExecuteReturnID("FLIGHT_DAYFLIGHT", "OUPDATE", obj);
            return (Int64)u;
        }
        public DataTable GetDeleted(string where)
        {
            if (string.IsNullOrEmpty(where)) where = " 1=1";
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetRecordDeleted"
                , new OracleParameter("P_WHERE", where)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
        public DataTable GetAllForValueSearch(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_GetAll", objSearch).Tables[0];
        }

        public DataTable GetAllForValueSearch_New(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("A_TEST_SEARCH", "GetDaylyFlight_GetAll", objSearch).Tables[0];
        }

        public DataTable GetAllForValueSearch_HCM(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetAll_HCM", objSearch).Tables[0];
        }
        public DataTable GetAllForValueSearch_DNG(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetAll_DNG", objSearch).Tables[0];
        }
        public DataTable FlightInfoExtension_DienVan(DateTime datePK, string FlightNbr)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "FlightInfoExtension_Message", new OracleParameter("p_datePK", datePK), new OracleParameter("p_FlightNbr", FlightNbr)).Tables[0];
        }

        #region flight change
        public DataTable GetFlightChangeBySearch(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_HasChange", obj).Tables[0];
        }
        #endregion
        #region flight not perm
        public DataTable GetFlightNotPermBySearch(clsDayFlightValueSearch obj)
        {
            //return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "DayFlightNotPerm_GetBySearch", obj).Tables[0];
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_GetAll_NoPerm", obj).Tables[0];
        }
        #endregion

        #region flight ATD not null
        public DataTable GetFlightAtdNotNullBySearch(clsDayFlightValueSearch obj)
        {
            //return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "DayFlightAtdNull_GetBySearch", obj).Tables[0];
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_GetAll_AtdNull", obj).Tables[0];
        }
        #endregion


        #region flight not in route
        public DataTable GetFlightNotRouteBySearch(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_NotRoute", obj).Tables[0];
        }
        #endregion
        #region flight search all by date
        public DataTable GetFlightBySearch(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_Search", obj).Tables[0];
        }
        public DataTable GetNgayNTru1(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_nTru", obj).Tables[0];
        }
        public DataTable GetNgayNCong1(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlight_nCong", obj).Tables[0];
        }
        #endregion
        #region Draft
        public DataTable GetPageDraft(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("CALENDARDRAFT", "GetPage"
                        , new OracleParameter("P_PAGE_SIZE", page_size)
                        , new OracleParameter("P_PAGE_INDEX", page_index)
                        , new OracleParameter("P_WHERE", where)).Tables[0];
        }
        public DataTable GetPageDraftTrungLap(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("CALENDARDRAFT", "GetPageDraftTrungLap"
                        , new OracleParameter("P_PAGE_SIZE", page_size)
                        , new OracleParameter("P_PAGE_INDEX", page_index)
                        , new OracleParameter("P_WHERE", where)).Tables[0];
        }
        public DataTable GetByIdDraft(Int64 id)
        {
            return new oDataProvider().ExecuteDatase("CALENDARDRAFT", "GetById", new OracleParameter("p_ID", id)).Tables[0];
        }
        public int DeleteDraft(Int64 id, string user)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "oDelete", new OracleParameter("p_ID", id));
        }
        public int UpdateDraft(DayFlights obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "oUpdate", obj);
        }
        public int InsertDraft(DayFlights obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "oInsert", obj);
        }
        public int GenToDraftByDate(DateTime date, string user)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "GenToDraftByDate",
                    new OracleParameter("p_User", user)
                    , new OracleParameter("p_Date", date));
        }
        public int DeleteAllDraft(string user)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "GenToDraftByDate",
                    new OracleParameter("p_User", user));
        }
        public Int64 AccessDraft(string user, DateTime date)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDARDRAFT", "GenToDraftByDate",
                    new OracleParameter("p_User", user)
                    , new OracleParameter("p_Date", date));
        }
        #endregion


        #region ke hoach bay ngay
        public DataTable LayKeHoachBayNgay(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKeHoachBay", obj).Tables[0];
        }
        public DataTable LayKeHoachBayNgayFull(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKeHoachBayFull", obj).Tables[0];
        }
        public DataTable LayKeHoachBayOrigin(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKeHoachBayOrigin", obj).Tables[0];
        }

        public DataTable LayKeHoachBayNgay_TrungLap(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKeHoachBay_TrungLap", obj).Tables[0];
            //return new oDataProvider().ExecuteDatase("A_TEST_SEARCH", "LayKeHoachBay_TrungLap", obj).Tables[0];
        }
        public Int64 AccessKeHoachBay(string user, DateTime date)
        {
            return new oDataProvider().ExecuteNonQuery("FLIGHT_DAYFLIGHT", "AccessCalendar", new OracleParameter("p_User", user), new OracleParameter("p_date", date));
        }

        public Int64 AccessKeHoachBayNew(string user, DateTime date)
        {
            return new oDataProvider().ExecuteNonQuery("A_TEST_SEARCH", "AccessCalendar", new OracleParameter("p_User", user), new OracleParameter("p_date", date));
        }

        public Int64 KhbToMessage(DateTime date)
        {
            return new oDataProvider().ExecuteNonQuery("PERMISSION2TEXT", "RenderKhb", new OracleParameter("DATE_FLY", date));
        }
        public Int64 KhbToMessageExt(DateTime date)
        {
            return new oDataProvider().ExecuteNonQuery("PERMISSION2TEXT", "RenderKhb_Ext", new OracleParameter("DATE_FLY", date));
        }
        public Int64 KhbToMessageByAirport(DateTime date)
        {
            return new oDataProvider().ExecuteNonQuery("PERMISSION2TEXT", "RenderKhb_airport", new OracleParameter("DATE_FLY", date));
        }
        public DataTable LayKeHoachBayNgay_Delete(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKeHoachBay_Delete", obj).Tables[0];
        }
        #endregion
        #region ke hoach bay venh 
        public DataTable LayKeHoachVenhTheoMua(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKhb_Venh_So_LichMua", obj).Tables[0];
        }
        public DataTable LayKeHoachVenhTheoThang(clsDayFlightValueSearch obj)
        {
            return new oDataProvider().ExecuteDatase("KEHOACHBAY", "LayKhb_Venh_So_LichThang", obj).Tables[0];
        }
        #endregion
    }
}

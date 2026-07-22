using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
namespace QLB.BusinessLogic
{
    public class CalendarDetailDAL
    {
        public DataSet GetAllCalendarDetail()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CalendarDetail_GetAll", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageCalendarDetail(int page_size, int page_index, string where)
        {
            try
            {
                //DataTable dt1 = DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CALENDAR_DETAIL_GET_COUNT", new string[] { "P_WHERE" }, new object[] //{ where }).Tables[0];
                // DataTable dt2 =               
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CalendarDetail_GetPage", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE" }, new object[] { page_size, page_index, where });
                return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "CalendarDetail_GetPage", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCountCalendarDetail(string where)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CALENDAR_DETAIL_GET_COUNT", new string[] { "P_WHERE" }, new object[] { where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByIdCalendarDetail(Int64 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CalendarDetail_GetOne", new string[] { "p_ID" }, new object[] { Id });
                return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "CalendarDetail_GetOne", new OracleParameter("p_ID", Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteCalendarDetail(Int64 ID)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("CALENDAR_PKG.CalendarDetail_Delete", new string[] { "p_ID" }, new object[] { ID });
                return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CalendarDetail_Delete", new OracleParameter("P_ID", ID)) == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 CreateCalendarDetail(CalendarDetail obj)
        {
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CalendarDetail_Insert", obj);
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "CALENDAR_PKG.CalendarDetail_Insert");
        }
        public Int64 UpdateCalendarDetail(CalendarDetail obj)
        {
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CalendarDetail_Update", obj);
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "CALENDAR_PKG.CalendarDetail_Update");
        }

        public Int64 InsertObject(CalendarDetail obj)
        {
            //List<OracleParameter> lis = new List<OracleParameter>();
            //lis.Add(new OracleParameter("p_CALENDAR_ID", obj.CALENDAR_ID));
            //lis.Add(new OracleParameter("p_PERM_ID", obj.PERM_ID));
            //lis.Add(new OracleParameter("p_ETA", obj.ETA));
            //lis.Add(new OracleParameter("p_ETD", obj.ETD));
            //lis.Add(new OracleParameter("p_FLIGHT_PK", obj.FLIGHT_PK));
            //lis.Add(new OracleParameter("p_CRAFT_ID", obj.CARAFT_ID));
            //lis.Add(new OracleParameter("p_FLIGHTNBR", obj.FLIGHTNBR));
            //lis.Add(new OracleParameter("p_PURPOSE_ID", obj.PURPOSE_ID));
            //lis.Add(new OracleParameter("p_DAY1", obj.DAY1));
            //lis.Add(new OracleParameter("p_DAY2", obj.DAY2));
            //lis.Add(new OracleParameter("p_DAY3", obj.DAY3));
            //lis.Add(new OracleParameter("p_DAY4", obj.DAY4));
            //lis.Add(new OracleParameter("p_DAY5", obj.DAY5));
            //lis.Add(new OracleParameter("p_DAY6", obj.DAY6));
            //lis.Add(new OracleParameter("p_DAY7", obj.DAY7));
            //lis.Add(new OracleParameter("p_FROM_AIRP", obj.FROM_AIRP));
            //lis.Add(new OracleParameter("p_TO_AIRP", obj.TO_AIRP));
            //lis.Add(new OracleParameter("p_MTOW", obj.MTOW));
            //lis.Add(new OracleParameter("p_REGISTRATION", obj.REGISTRATION));
            //lis.Add(new OracleParameter("p_VIA", obj.VIA));
            //lis.Add(new OracleParameter("p_REMARK", obj.REMARK));
            //lis.Add(new OracleParameter("p_BEGINDATE", obj.BEGINDATE));
            //lis.Add(new OracleParameter("p_ENDDATE", obj.ENDDATE));            
            //lis.Add(new OracleParameter("p_USER_NAME", obj.USER_NAME));
            //lis.Add(new OracleParameter("p_LAST_USER", obj.LAST_USER));
            //lis.Add(new OracleParameter("p_TIME_UPDATE", DateTime.Now));
            //lis.Add(new OracleParameter("P_RETURN_CODE", OracleDbType.Int64) { Direction = ParameterDirection.Output });
            //clsData.ExecuteNonQuery("CALENDAR_PKG.CALENDAR_DETAIL_INSERT", lis.ToArray());
            //return Int64.Parse(lis[lis.Count - 1].Value.ToString());
            return new oDataProvider().ExecuteReturnID("CALENDAR_PKG", "CalendarDetail_Insert", obj);
        }
        public Int64 UpdateObject(CalendarDetail obj)
        {
            //List<OracleParameter> lis = new List<OracleParameter>();
            //lis.Add(new OracleParameter("p_ID", obj.ID));
            //lis.Add(new OracleParameter("p_CALENDAR_ID", obj.CALENDAR_ID));
            //lis.Add(new OracleParameter("p_PERM_ID", obj.PERM_ID));
            //lis.Add(new OracleParameter("p_ETA", obj.ETA));
            //lis.Add(new OracleParameter("p_ETD", obj.ETD));
            //lis.Add(new OracleParameter("p_FLIGHT_PK", obj.FLIGHT_PK));
            //lis.Add(new OracleParameter("p_CRAFT_ID", obj.CARAFT_ID));
            //lis.Add(new OracleParameter("p_FLIGHTNBR", obj.FLIGHTNBR));
            //lis.Add(new OracleParameter("p_PURPOSE_ID", obj.PURPOSE_ID));
            //lis.Add(new OracleParameter("p_DAY1", obj.DAY1));
            //lis.Add(new OracleParameter("p_DAY2", obj.DAY2));
            //lis.Add(new OracleParameter("p_DAY3", obj.DAY3));
            //lis.Add(new OracleParameter("p_DAY4", obj.DAY4));
            //lis.Add(new OracleParameter("p_DAY5", obj.DAY5));
            //lis.Add(new OracleParameter("p_DAY6", obj.DAY6));
            //lis.Add(new OracleParameter("p_DAY7", obj.DAY7));
            //lis.Add(new OracleParameter("p_FROM_AIRP", obj.FROM_AIRP));
            //lis.Add(new OracleParameter("p_TO_AIRP", obj.TO_AIRP));
            //lis.Add(new OracleParameter("p_MTOW", obj.MTOW));
            //lis.Add(new OracleParameter("p_REGISTRATION", obj.REGISTRATION));
            //lis.Add(new OracleParameter("p_VIA", obj.VIA));
            //lis.Add(new OracleParameter("p_REMARK", obj.REMARK));
            //lis.Add(new OracleParameter("p_BEGINDATE", obj.BEGINDATE));
            //lis.Add(new OracleParameter("p_ENDDATE", obj.ENDDATE));            
            //lis.Add(new OracleParameter("p_LAST_USER", obj.LAST_USER));
            //lis.Add(new OracleParameter("p_TIME_UPDATE", DateTime.Now));
            //lis.Add(new OracleParameter("P_RETURN_CODE", OracleDbType.Int64) { Direction = ParameterDirection.Output });
            //clsData.ExecuteNonQuery("CALENDAR_PKG.CALENDAR_DETAIL_UPDATE", lis.ToArray());
            //return Int64.Parse(lis[lis.Count - 1].Value.ToString());
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CalendarDetail_Update", obj);
        }
        public DataTable GetHistoryById(string id)
        {
            return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "CalendarDetail_GetHistoryById", new OracleParameter("P_FLIGHT_ID", id)).Tables[0];
        }
        public bool RestoreHis(Int64 id, int version, string idUser)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_ID", id));
            lis.Add(new OracleParameter("P_Version", version));
            lis.Add(new OracleParameter("P_IdUser", idUser));            
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CalendarDetail_RestoreRecord", lis.ToArray())==1?true:false;
        }
    }
}

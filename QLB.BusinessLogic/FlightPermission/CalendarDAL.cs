using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public class CalendarDAL
    {
        #region code old
        public DataSet GetAllCalendar()
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.Calendar_GetAll", new string[] { }, new object[] { });
                return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "Calendar_GetAll");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageCalendar(int page_size, int page_index, string where)
        {
            try
            {
                //DataTable dt1 = DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CALENDAR_GET_COUNT", new string[] { "P_WHERE" }, new object[] //{ where }).Tables[0];
                // DataTable dt2 = 
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.Calendar_GetPage", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE" }, new object[] { page_size, page_index, where });
                return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "Calendar_GetPage", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCountCalendar(string where)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.CALENDAR_GET_COUNT", new string[] { "P_WHERE" }, new object[] { where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByIdCalendar(Int64 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CALENDAR_PKG.Calendar_GetOne", new string[] { "p_ID" }, new object[] { Id });
                return new oDataProvider().ExecuteDatase("CALENDAR_PKG", "Calendar_GetOne", new OracleParameter("p_ID", Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteCalendar(Int64 ID)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("CALENDAR_PKG.Calendar_Delete", new string[] { "p_ID" }, new object[] { ID });
                return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "Calendar_Delete", new OracleParameter("P_ID", ID))==-1?false:true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateCalendar(Calendar obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "CALENDAR_PKG.CALENDAR_INSERT");
            return (int)new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CALENDAR_INSERT", obj);
        }
        public string CreateCalendarReturnStringID(Calendar obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "CALENDAR_PKG.CALENDAR_INSERT").ToString();
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CALENDAR_INSERT", obj).ToString();
        }
        public int UpdateCalendar(Calendar obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "CALENDAR_UPDATE", obj);
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "CALENDAR_PKG.CALENDAR_UPDATE");
        }
        #endregion

        #region code new
        public Int64 InsertObject(Calendar obj)
        {
            //List<OracleParameter> lis = new List<OracleParameter>();
            //lis.Add(new OracleParameter("p_CALENDAR_ID", obj.CALENDAR_ID));
            //lis.Add(new OracleParameter("p_CALENDAR_NAME", obj.CALENDAR_NAME));
            //lis.Add(new OracleParameter("p_LASTUSER", obj.LASTUSER));
            //lis.Add(new OracleParameter("p_USER_NAME", obj.USER_NAME));
            //lis.Add(new OracleParameter("p_BEGIN_DATE", obj.BEGIN_DATE));
            //lis.Add(new OracleParameter("p_FINISH_DATE", obj.FINISH_DATE));
            //lis.Add(new OracleParameter("p_SEASON", obj.SEASON));
            //lis.Add(new OracleParameter("p_YEAR", obj.YEAR));
            //lis.Add(new OracleParameter("p_TYPE", obj.TYPE));
            //lis.Add(new OracleParameter("P_RETURN_CODE", OracleDbType.Int64) { Direction= ParameterDirection.Output});
            //clsData.ExecuteNonQuery("CALENDAR_PKG.CALENDAR_INSERT", lis.ToArray());
            //return Int64.Parse(lis[lis.Count - 1].Value.ToString());
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "Calendar_Insert", obj);
        }
        public Int64 UpdateObject(Calendar obj)
        {
            //List<OracleParameter> lis = new List<OracleParameter>();
            //lis.Add(new OracleParameter("p_ID", obj.ID));
            //lis.Add(new OracleParameter("p_CALENDAR_ID", obj.CALENDAR_ID));
            //lis.Add(new OracleParameter("p_CALENDAR_NAME", obj.CALENDAR_NAME));
            //lis.Add(new OracleParameter("p_LASTUSER", obj.LASTUSER));
            //lis.Add(new OracleParameter("p_MAKING_DATE", obj.MAKING_DATE));
            //lis.Add(new OracleParameter("p_USER_NAME", obj.USER_NAME));
            //lis.Add(new OracleParameter("p_BEGIN_DATE", obj.BEGIN_DATE));
            //lis.Add(new OracleParameter("p_FINISH_DATE", obj.FINISH_DATE));
            //lis.Add(new OracleParameter("p_SEASON", obj.SEASON));
            //lis.Add(new OracleParameter("p_YEAR", obj.YEAR));
            //lis.Add(new OracleParameter("p_TYPE", obj.TYPE));
            //lis.Add(new OracleParameter("P_RETURN_CODE", OracleDbType.Int64) { Direction = ParameterDirection.Output });
            //clsData.ExecuteNonQuery("CALENDAR_PKG.CALENDAR_UPDATE", lis.ToArray());
            //return Int64.Parse(lis[lis.Count - 1].Value.ToString());
            return new oDataProvider().ExecuteNonQuery("CALENDAR_PKG", "Calendar_Update", obj);
        }
        public DataTable GetPageListObject(int pageSize, int pageIndex, string where)
        {
            return clsData.ExecuteDataset("CALENDAR_PKG.CALENDAR_GET_PAGE",
                new OracleParameter("P_PAGE_SIZE", pageSize),
                new OracleParameter("P_PAGE_INDEX", pageIndex),
                new OracleParameter("P_WHERE", where)).Tables[0];
        }
        #endregion
    }
}

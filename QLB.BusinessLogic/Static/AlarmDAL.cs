using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class AlarmDAL
    {
        public DataSet GetAllAlarm()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ALARM_GET_ALL");
        }
        public DataSet GetPageAlarm(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ALARM_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageAlarmExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ALARM_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdAlarm(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ALARM_GET_ID"
                , new OracleParameter("p_ID", Id));
        }
        public void DeleteAlarm(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ALARM_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        public int CreateAlarm(Alarm obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "ALARM_INSERT", obj);
        }
        public int UpdateAlarm(Alarm obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "ALARM_UPDATE", obj);
        }       
        public DataTable GetCountAlarmBySystemDate()
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();               
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("DIC_PKG", "ALARM_GET_COUNT_BYDATE", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public class CalendarBackupDAL
    {              
        public DataSet GetByIdCalendarBackup(Int32 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("FLY_PERMISSION_PKG.CALENDAR_BACKUP_GET_ID", new string[] { "p_ID" }, new object[] { Id });
                return new oDataProvider().ExecuteDatase("FLY_PERMISSION_PKG", "CALENDAR_BACKUP_GET_ID", new OracleParameter("p_ID", Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCalendarBackup(Int32 ID)
        {
            try
            {
                DataProvider.Instance().ExecStore_Oracle("FLY_PERMISSION_PKG.CALENDAR_BACKUP_DELETE", new string[] { "p_ID" }, new object[] { ID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CreateCalendarBackup(CalendarBackup obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "FLY_PERMISSION_PKG.CALENDAR_BACKUP_INSERT");
        }
        public int UpdateCalendarBackup(CalendarBackup obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "FLY_PERMISSION_PKG.CALENDAR_BACKUP_UPDATE");
        }
        public int RestoreCalendarBackup(CalendarBackup obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "FLY_PERMISSION_PKG.CALENDAR_BACKUP_RESTORE");
        }
    }
}

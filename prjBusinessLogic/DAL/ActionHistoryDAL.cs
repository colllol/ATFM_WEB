using System;
using prjInfo;
using System.Collections.Generic;
using System.Data;
using HPCShareDLL;

namespace prjBusinessLogic
{
    public class ActionHistoryDAL
    {
        #region ActionHistory Old
        public string InserT_ActionOrc(T_ActionHistory obj)
        {
            return new clsResuftAPI<T_ActionHistory>().InsertReturnId(obj, "api/ActionHistory/CreateActionHistory");
        }
        public void InserT_Action(T_ActionHistory objActionHistory)
        {
            HPCDataProvider.Instance().InsertObject(objActionHistory, "[CMS_InsertT_ActionHistory]");
        }
        public DataSet BindGridT_ActionHistory(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("CMS_ListT_ActionHistoryDynamic", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BindGridT_ActionHistoryTable(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                //return HPCDataProvider.Instance().GetStoreDataSet("CMS_ListT_ActionHistoryDynamic", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "T_ACTIONHISTORY_GET_PAGE".ToUpper(), new { P_PAGE_SIZE = PageSize, P_PAGE_INDEX = PageIndex, P_WHERE = WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public List<ActionHistory> GetAllActionHistory()
        {
            return new clsResuftAPI<ActionHistory>().GetListObj("api/ActionHistory/GetAllActionHistory");
        }
        public List<ActionHistory> GetExportActionHistory(string where)
        {
            return new clsResuftAPI<ActionHistory>().GetListObjExportData("api/ActionHistory/GetExportActionHistory",where);
        }
        public List<ActionHistory> GetPageActionHistory(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<ActionHistory>().GetListObj("api/ActionHistory/GetPageActionHistory", pageSize, pageIndex, where);
        }
        public void InsertLogView(Int64 UserID, string UserName, Int64 ISLOGIN, string TimeLogin, string IPADDRESS, string NOTE)
        {
            try
            {
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "T_INSERT_VIEWLOG".ToUpper(), new {P_USERID= UserID, P_USERNAME = UserName, P_ISLOGIN= ISLOGIN, P_TIMELOGIN= TimeLogin , P_IPADDRESS = IPADDRESS , P_NOTE = NOTE });
                /*T_ActionHistory objActionHistory = new T_ActionHistory();
                objActionHistory.UserID = UserID;
                objActionHistory.FullName = UserName;
                objActionHistory.DateModify = Convert.ToDateTime(TimeLogin);

                objActionHistory.HostIP = IPADDRESS;
                objActionHistory.Notes = NOTE;
                HPCDataProvider.Instance().InsertObject(objActionHistory, "[CMS_InsertT_ActionHistory]");*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertLog(string Name,string Time,string Err)
        {
            try
            {
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "T_INSERT_LOG".ToUpper(), new { P_NAME = Name, P_SQLCODE = Time, P_ERR = Err});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
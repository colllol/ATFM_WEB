using System;
using System.Collections.Generic;
using System.Linq;
using ShareDLL;
using System.Data;
using QLB.Info;
using System.Text;
using System.Threading.Tasks;

namespace QLB.BusinessLogic
{
    public class GroupMenuDAL
    {
        public DataSet GetAllGroupMenu()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPMENU_GET_ALL", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageGroupMenu(int page_size, int page_index)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPMENU_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetByIdGroupMenu(Int32 GROUPMENU_ID)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPMENU_GET_ID", new string[] { "p_GROUPMENU_ID" }, new object[] { GROUPMENU_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteGroupMenu(Int32 GROUPMENU_ID)
        {
            try
            {
                DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_GROUPMENU_DELETE", new string[] { "p_GROUPMENU_ID" }, new object[] { GROUPMENU_ID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateGroupMenu(GroupMenu obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_GROUPMENU_INSERT");
        }
        public int UpdateGroupMenu(GroupMenu obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_GROUPMENU_UPDATE");
        }
    }
}

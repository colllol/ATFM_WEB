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
   public class UserGroupsDAL
    {
        public DataSet GetAllUserGroups()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERGROUPS_GET_ALL", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageUserGroups(int page_size, int page_index)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERGROUPS_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetByIdUserGroups(Int32 ID)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERGROUPS_GET_ID", new string[] { "p_ID" }, new object[] { ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteUserGroups(Int32 ID)
        {
            try
            {
                DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_USERGROUPS_DELETE", new string[] { "p_ID" }, new object[] { ID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateUserGroups(UserGroups obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERGROUPS_INSERT");
        }
        public int UpdateUserGroups(UserGroups obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERGROUPS_UPDATE");
        }
    }
}

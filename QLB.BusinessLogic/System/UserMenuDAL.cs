using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShareDLL;
using QLB.Info;
using System.Text;
using System.Threading.Tasks;

namespace QLB.BusinessLogic
{
    public class UserMenuDAL
    {
        public DataSet GetAllUserMenu()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERMENU_GET_ALL", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageUserMenu(int page_size, int page_index)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERMENU_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetByIdUserMenu(Int32 ID)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERMENU_GET_ID", new string[] { "p_ID" }, new object[] { ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteUserMenu(Int32 ID)
        {
            try
            {
                DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_USERMENU_DELETE", new string[] { "p_ID" }, new object[] { ID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateUserMenu(UserMenu obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERMENU_INSERT");
        }
        public int UpdateUserMenu(UserMenu obj)
        {
            return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERMENU_UPDATE");
        }
    }
}

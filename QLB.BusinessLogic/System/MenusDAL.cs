using System;
using System.Collections.Generic;
using System.Linq;
using ShareDLL;
using System.Data;
using QLB.Info;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
   public class MenusDAL
    {
        public DataSet GetAllMenus()
        {
            try
            {
                // return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_MENUS_GET_ALL", new string[] { }, new object[] { });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_MENUS_GET_ALL");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageMenus(int page_size, int page_index, string where)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_MENUS_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_MENUS_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetByIdMenus(Int32 ID)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_MENUS_GET_ID", new string[] { "p_ID" }, new object[] { ID });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_MENUS_GET_ID", new OracleParameter("p_ID", ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteMenus(Int32 ID)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_MENUS_DELETE", new string[] { "p_ID" }, new object[] { ID });
                new oDataProvider().ExecuteNonQuery("SYS_PKG", "T_MENUS_DELETE"
               , new OracleParameter("p_ID", ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateMenus(Menus obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_MENUS_INSERT");
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_MENUS_INSERT", obj);
        }
        public int UpdateMenus(Menus obj)
        {
            // return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_MENUS_UPDATE");
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_MENUS_UPDATE", obj);
        }
    }
}

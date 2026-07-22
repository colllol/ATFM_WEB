using System;
using System.Collections.Generic;
using System.Linq;
using ShareDLL;
using QLB.Info;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public class GroupsDAL
    {
        public DataSet BindGridMenuByGroup(int Parrent_ID, int GroupId)
        {
            try
            {

                DataSet ax = new DataSet();
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Parrent_ID", Parrent_ID));
                lis.Add(new OracleParameter("P_GroupId", GroupId));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_BindGridMenuByGroup", lis.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllGroups()
        {
            try
            {
                //  return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPS_GET_ALL", new string[] { }, new object[] { });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_GROUPS_GET_ALL");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageGroupExport(string where)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.GROUP_GET_PAGE_EXPORT", new string[] { "P_WHERE" }, new object[] { where });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "GROUP_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPageGroups(int page_size, int page_index)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPS_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_GROUPS_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetByIdGroups(Int32 GROUP_ID)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_GROUPS_GET_ID", new string[] { "p_GROUP_ID" }, new object[] { GROUP_ID });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_GROUPS_GET_ID", new OracleParameter("p_GROUP_ID", GROUP_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteGroups(Int32 GROUP_ID)
        {
            try
            {
                // DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_GROUPS_DELETE", new string[] { "p_GROUP_ID" }, new object[] { GROUP_ID });
                new oDataProvider().ExecuteNonQuery("SYS_PKG", "T_GROUPS_DELETE"
             , new OracleParameter("p_GROUP_ID", GROUP_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateGroups(Groups obj)
        {
            // return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_GROUPS_INSERT");
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_GROUPS_INSERT", obj);
        }
        public int UpdateGroups(Groups obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_GROUPS_UPDATE");
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_GROUPS_UPDATE", obj);
        }
    }
}

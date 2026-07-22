using System;
using System.Collections.Generic;
using System.Linq;
using QLB.Info;
using System.Data;
using ShareDLL;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class UsersDAL
    {
        public DataSet GetAllUsers()
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERS_GET_ALL", new string[] { }, new object[] { });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_USERS_GET_ALL");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageUsers(int page_size, int page_index)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERS_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX" }, new object[] { page_size, page_index });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPageUsers_New(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("SYS_PKG", "T_USERS_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
       
        public DataSet GetByIdUsers(Int32 UserId)
        {
            try
            {
                // return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.T_USERS_GET_ID", new string[] { "p_USERID" }, new object[] { UserId });
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_USERS_GET_ID", new OracleParameter("p_USERID", UserId));
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        public void DeleteUsers(Int32 UserId)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_USERS_DELETE", new string[] { "p_USERID" }, new object[] { UserId });
                new oDataProvider().ExecuteNonQuery("SYS_PKG", "T_USERS_DELETE"
              , new OracleParameter("p_USERID", UserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMenu4User(Int64 UserId)
        {
            try
            {

                DataSet ax = new DataSet();
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CANT_PKG.CANT_REALIZE_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE", "P_FROM_DATE ", "P_TO_DATE" }, new object[] { page_size, page_index, where, FromDate, ToDate }).Tables[0];
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_UserId", UserId));
                lis.Add(new OracleParameter("P_RETURN_CODE", OracleDbType.Int64, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_GetMenu4User", lis.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CreateUsers(Users obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERS_INSERT");
            //return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_USERS_INSERT", obj);
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_USERS_INSERT", obj);
        }
        public int UpdateUsers(Users obj)
        {
            // return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "SYS_PKG.T_USERS_UPDATE");
            return (int)new oDataProvider().ExecuteReturnID("SYS_PKG", "T_USERS_UPDATE", obj);
        }

        //public DataSet GetUserByUserPass(string Username, string Password)
        //{
        //    try
        //    {
        //        // return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.CMS_GetUserByUserPass", new string[] { "p_USERNAME", "p_USERPASS" }, new object[] { Username,Password });
        //        return (DataSet)new oDataProvider().ExecuteDatase("SYS_PKG", "T_USERS_INSERT", obj);
        //        //return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_GetUserByUserPass",
        //        //   new Oracle.DataAccess.Client.OracleParameter("p_USERNAME", Username)
        //        //   , new Oracle.DataAccess.Client.OracleParameter("p_USERPASS", Password)
        //        //   , new Oracle.DataAccess.Client.OracleParameter("P_OUT_CURSOR", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        // }

        public DataSet GetUserByUserPass(string Username, string Password)
        {
            try
            {

                DataSet ax = new DataSet();
                //return DataProvider.Instance().GetStoreDataSet_Oracle("CANT_PKG.CANT_REALIZE_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE", "P_FROM_DATE ", "P_TO_DATE" }, new object[] { page_size, page_index, where, FromDate, ToDate }).Tables[0];
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("p_USERNAME", Username));
                lis.Add(new OracleParameter("p_USERPASS", Password));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_GetUserByPass", lis.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BindGridMenuByUser(int Parrent_ID, int UserId)
        {
            try
            {

                DataSet ax = new DataSet();
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Parrent_ID", Parrent_ID));
                lis.Add(new OracleParameter("P_UserId", UserId));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_BindGridMenuByUser", lis.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet isParrentMenu(int Menu_ID)
        {
            try
            {

                DataSet ax = new DataSet();
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_Menu_ID", Menu_ID));
                lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_isParrentMenu", lis.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetUserByUserName(string Username)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.CMS_GetUserByName", new string[] { "p_USERNAME" }, new object[] { Username });           
                return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_GetUserByName", new OracleParameter("p_USERNAME", Username));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUSERNAME(string Username, int UserID)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.GetUSERNAME", new string[] { "p_USERNAME", "p_UserID" }, new object[] { Username, UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRole4UserMenu(int UserID, int MenuId)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("SYS_PKG.GetUSERNAME", new string[] { "p_USERNAME", "p_UserID" }, new object[] { Username , UserID });
                try
                {

                    DataSet ax = new DataSet();
                    List<OracleParameter> lis = new List<OracleParameter>();
                    lis.Add(new OracleParameter("P_UserId", UserID));
                    lis.Add(new OracleParameter("P_MenuId", MenuId));
                    lis.Add(new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                    return new oDataProvider().ExecuteDatase("SYS_PKG", "CMS_GetRoleFourUserMenu", lis.ToArray());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

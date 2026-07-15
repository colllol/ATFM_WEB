using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using prjInfo;
using System.Reflection;
using System.Web.Caching;
using System.Data.SqlClient;
namespace HPCShareDLL
{
    public abstract class HPCDataProvider
    {
        public static HPCDataProvider Instance()
        {
            return Instance(HttpContext.Current, null, null, null);
        }
        public static HPCDataProvider Instance(HttpContext context, string providerTypeName, string databaseOwner, string connectionString)
        {
            Cache cache = HttpRuntime.Cache;
            Type type = null;

            // Get the names of the providers
            //
            Configuration config = Configuration.GetConfig();

            // Read the configuration specific information
            // for this provider
            //
            Provider sqlForumsProvider = (Provider)config.Providers[config.DefaultProvider];

            // Read the connection string for this provider
            //
            if (connectionString == null)
                connectionString = sqlForumsProvider.Attributes["connectionString"];

            // Read the database owner name for this provider
            //
            if (databaseOwner == null)
                databaseOwner = sqlForumsProvider.Attributes["databaseOwner"];

            if (providerTypeName == null)
                providerTypeName = ((Provider)config.Providers[config.DefaultProvider]).Type;

            if (cache["DataProvider"] == null)
            {

                // The assembly should be in \bin or GAC, so we simply need
                // to get an instance of the type
                //
                try
                {


                    type = Type.GetType(providerTypeName);

                    // Insert the type into the cache
                    //
                    Type[] paramTypes = new Type[2];
                    paramTypes[0] = typeof(string);
                    paramTypes[1] = typeof(string);
                    cache.Insert("DataProvider", type.GetConstructor(paramTypes));
                }
                catch
                {

                    //if (context != null)
                    //{
                    //    // We can't load the dataprovider
                    //    //
                    //    StreamReader reader = new StreamReader(context.Server.MapPath("~/Languages/" + config.DefaultLanguage + "/errors/DataProvider.htm"));
                    //    string html = reader.ReadToEnd();
                    //    reader.Close();

                    //    html = html.Replace("[DATAPROVIDERCLASS]", config.DefaultProvider);
                    //    html = html.Replace("[DATAPROVIDERASSEMBLY]", config.DefaultProvider);
                    //    context.Response.Write(html);
                    //    context.Response.End();
                    //}
                    //else
                    //{
                    //    throw new HPCException(ExceptionType.DataProvider, "Unable to load " + config.DefaultProvider);
                    //}

                }

            }

            object[] paramArray = new object[2];
            paramArray[0] = databaseOwner;
            paramArray[1] = connectionString;

            return (HPCDataProvider)(((ConstructorInfo)cache["DataProvider"]).Invoke(paramArray));
        }
        public abstract DataSet GetDataSet(string TableName, string ColumnList, string Where);
        public abstract string GetColumnValues(string TableName, string ColumnName, string Where);
        public abstract DataSet GetStoreDataSet(string StoreName, string[] param1, object[] value);
        public abstract void ExecStore(string StoreName, string[] param1, object[] value);
        public abstract double ExecStoreReturn(string StoreName, string[] param1, object[] value);
        public abstract void ExecStore(string StoreName);
        public abstract DataSet ExecStoreDataSet(string StoreName);
        public abstract void ExecSql(string sql);
        public abstract DataSet ExecSqlDataSet(string sql);
        public abstract int ExecSqlReturnVal(string sql);
        public abstract void InsertObject(object obj);
        public abstract void InsertObject(object obj, string spName);
        public abstract int InsertObjectReturn(object obj, string spName);
        public abstract void Insert_T_NewsEvent_Content_From_T_NewsEvent_Content(int news_id, int _CopyFrom);
        public abstract void Insert_T_Event_Category_From_T_Event_Category(int news_id);
        public abstract void Insert_T_CategoryChuyenDe_From_T_CategoryChuyenDe(int news_id,int copyfrom);
        public abstract void Insert_T_NewsChuyenDe_From_T_NewsChuyenDe(int news_id, int copyfrom,int Lang_id,int status,int nguoisua,DateTime ngaysua);
        public abstract void UpdateObject(object obj);
        public abstract void UpdateObject(object obj, string spName);
        public abstract bool Check_Nhanban_T_Categorys_Chuyende(int id);
        public abstract bool LanquageExitsTranlate(int id,int langID);
        public abstract bool LanquageExitsTranlatein_T_Album_Categories(int id, int langID);
        public abstract bool ExitsTranlate_T_Multimedia(int id, int langId);
        public abstract bool ExitsTranlate_T_Album_Photo(int id, int langID);
        public abstract bool ExitsTranlate_T_Photo_Even(int id, int langId);
        public abstract object GetObjectByID(string Id, string TypeName, string FieldIDName);
        public abstract object GetObjectByID(string sp_name, string Id, string TypeName, string FieldIDName);
        
        public abstract object GetObjectByCondition(string TypeName, string whereCondition);
        public abstract T_Users GetUserByUserPass(string userName, string passWord);
        public abstract bool Login(string userName, string passWord);
        public abstract T_Users GetUserByUserName(string userName);
        public abstract T_Users GetUserByUserName_ID(int userID);
        public abstract T_Languages GetLanguagesNameBy_ID(int ID);
        //public abstract T_Categorys GetCategoryNameBy_ID(int ID);
        public abstract T_Groups GetGroupByGroupName_ID(int userID);
        //public abstract T_RolePermission GetRole4User(int User_ID, string MenuName);
        public abstract T_RolePermission GetRole4UserMenu(int User_ID, int Menu_ID);
        
    }
}

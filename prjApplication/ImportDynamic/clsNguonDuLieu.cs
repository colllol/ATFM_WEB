using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace prjApplication.ImportData
{
    public class clsNguonDuLieu
    {
        #region properties
        private string _ID;
        private string _SOURCENAME;

        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        public string SOURCENAME
        {
            get
            {
                return _SOURCENAME;
            }

            set
            {
                _SOURCENAME = value;
            }
        }
        #endregion
        #region function
        public DataTable GetAll()
        {
            return new oDataProvider().ExecuteDataseProcduce("SuorceData_GetALL"
                , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
        }
        public bool Insert(string sourceName)
        {
            var ax = new oDataProvider().ExecuteNonQuery("IMPORT_PKG", "SuorceData_Insert"
                , new Oracle.DataAccess.Client.OracleParameter("vSourceName", sourceName)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int64, ParameterDirection.Output));
            return ax == -1 ? false : true;
        }
        public bool Update(string id, string sourceName)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG", "SuorceData_Update"
                , new Oracle.DataAccess.Client.OracleParameter("nId", id)
                , new Oracle.DataAccess.Client.OracleParameter("vSourceName", sourceName)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int64, ParameterDirection.Output)) == -1 ? false : true;
        }
        public bool Delete(string id)
        {
            return new oDataProvider().ExecuteNonQueryProduce("IMPORT_PKG.SuorceData_Delete"
                , new Oracle.DataAccess.Client.OracleParameter("nId", id)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int64, ParameterDirection.Output)) == -1 ? false : true;
        }
        public clsNguonDuLieu GetById(string id)
        {
            DataTable dt = new oDataProvider().ExecuteDataseProcduce("IMPORT_PKG.SuorceData_GetById"
                , new Oracle.DataAccess.Client.OracleParameter("nId", id)
                , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            var ax = new clsConvertTableToObject().ConvertTo<clsNguonDuLieu>(dt);
            if (ax.Count > 0)
                return ax[0];
            return new clsNguonDuLieu();
        }
        #endregion
    }
}
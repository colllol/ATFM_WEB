using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace prjApplication.ImportData
{
    public class clsDanhMucTable
    {
        #region properties and filed
        private string _ID;
        private string _VTABLENAME;
        private string _VTABLENAMEALIAS;
        private string _NSOURCEDATA;

        public string NSOURCEDATA
        {
            get
            {
                return _NSOURCEDATA;
            }

            set
            {
                _NSOURCEDATA = value;
            }
        }

        public string VTABLENAMEALIAS
        {
            get
            {
                return _VTABLENAMEALIAS;
            }

            set
            {
                _VTABLENAMEALIAS = value;
            }
        }

        public string VTABLENAME
        {
            get
            {
                return _VTABLENAME;
            }

            set
            {
                _VTABLENAME = value;
            }
        }

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
        #endregion

        #region function
        public DataTable GetAll()
        {
            DataTable dt = new oDataProvider().ExecuteDatase("IMPORT_PKG", "danhmuctable_getall"
                , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
            return dt;
        }
        public bool Insert(clsDanhMucTable obj)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG","danhmuctable_INSERT"
                , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAME", obj.VTABLENAME)
                , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAMEALIAS", obj.VTABLENAMEALIAS)
                , new Oracle.DataAccess.Client.OracleParameter("P_NSOURCEDATA", obj.NSOURCEDATA)
                , new Oracle.DataAccess.Client.OracleParameter("nResult",Oracle.DataAccess.Client.OracleDbType.Int32, ParameterDirection.Output)) == -1 ? false : true;
        }
        public bool Update(clsDanhMucTable obj)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG", "danhmuctable_UPDATE"
                , new Oracle.DataAccess.Client.OracleParameter("P_ID", obj.ID)
                , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAME", obj.VTABLENAME)
                , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAMEALIAS", obj.VTABLENAMEALIAS)
                , new Oracle.DataAccess.Client.OracleParameter("P_NSOURCEDATA", obj.NSOURCEDATA)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int32, ParameterDirection.Output)) == -1 ? false : true;
        }
        public bool Delete(string id)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG","danhmuctable_DELETE"
                , new Oracle.DataAccess.Client.OracleParameter("P_ID", id)) == -1 ? false : true;
        }
        public clsDanhMucTable GetById(string id)
        {
            DataTable dt = new oDataProvider().ExecuteDatase("IMPORT_PKG", "DanhMucTable_GetById"
                , new Oracle.DataAccess.Client.OracleParameter("nId", id)
                , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
            var ax = new clsConvertTableToObject().ConvertTo<clsDanhMucTable>(dt);
            if (ax.Count > 0)
                return ax[0];
            return new clsDanhMucTable();
        }
        #endregion
    }
}
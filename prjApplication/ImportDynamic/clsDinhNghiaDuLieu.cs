using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace prjApplication.ImportData
{
    public class clsDinhNghiaDuLieu
    {
        #region properties
        private string _ID;
        private string _NIDTABLE;
        private string _VCOLNAME;
        private string _VCOLNAMEALIAS;
        private string _VCOLDATATYPE;
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

        public string VCOLDATATYPE
        {
            get
            {
                return _VCOLDATATYPE;
            }

            set
            {
                _VCOLDATATYPE = value;
            }
        }

        public string VCOLNAMEALIAS
        {
            get
            {
                return _VCOLNAMEALIAS;
            }

            set
            {
                _VCOLNAMEALIAS = value;
            }
        }

        public string VCOLNAME
        {
            get
            {
                return _VCOLNAME;
            }

            set
            {
                _VCOLNAME = value;
            }
        }

        public string NIDTABLE
        {
            get
            {
                return _NIDTABLE;
            }

            set
            {
                _NIDTABLE = value;
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
        public bool Delete(string tableName, string idSource)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG","DinhNghiaDuLieu_DeleteGroup"
                , new Oracle.DataAccess.Client.OracleParameter("P_IdTable", tableName)
                , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", idSource)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int64, ParameterDirection.Output)) == -1 ? false : true;
        }
        public bool Insert(clsDinhNghiaDuLieu obj)
        {
            return new oDataProvider().ExecuteNonQuery("IMPORT_PKG", "DinhNghiaDuLieu_Insert"
                , new Oracle.DataAccess.Client.OracleParameter("P_IdTable", obj.NIDTABLE)
                , new Oracle.DataAccess.Client.OracleParameter("P_ColName", obj.VCOLNAME)
                , new Oracle.DataAccess.Client.OracleParameter("P_ColNameAlias", obj.VCOLNAMEALIAS)
                , new Oracle.DataAccess.Client.OracleParameter("P_ColDataType", obj._VCOLDATATYPE)
                , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", obj.NSOURCEDATA)
                , new Oracle.DataAccess.Client.OracleParameter("nResult", Oracle.DataAccess.Client.OracleDbType.Int64, ParameterDirection.Output)) == -1 ? false : true;

        }
        public DataTable GetBy(string tableName, string idSource)
        {
            return new oDataProvider().ExecuteDatase("IMPORT_PKG", "DinhNghiaDuLieu_GetBy"
                , new Oracle.DataAccess.Client.OracleParameter("P_TableName", tableName)
                , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", idSource)
                , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
        #endregion
    }
}
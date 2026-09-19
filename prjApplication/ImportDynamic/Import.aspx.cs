using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;
using System.Web.Script.Serialization;
using System.Data;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Data.OleDb;
using Oracle.DataAccess.Client;
using System.Globalization;

namespace prjApplication.ImportData
{
    public partial class Import : PageBaseCallBack
    {
        public string _PhanCach = "::::";
        public string _PhanCachArg = "_____";
        public const string _pathImport = "/UploadMulti/";
        public string _objTable
        {
            get
            {
                clsDanhMucTable obj = new clsDanhMucTable();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        public string _objNguonDuLieu
        {
            get
            {
                clsNguonDuLieu obj = new clsNguonDuLieu();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        public string _objDinhNghia
        {
            get
            {
                clsDinhNghiaDuLieu obj = new clsDinhNghiaDuLieu();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        private DataTable dtDinhNghia;
        private DataTable _dtDuLieuTrung;
        #region form event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTableName_Load();
                ddlSourceData_Load();
                a2grdSourceData_Load();

                a3SourceData_Load();
                a3TableName_Load();
                a3grd_Load();
                a4TableName_Load();
                a4SourceName_Load(a4TableName.SelectedItem.Text);
                a4grd_Load(a4TableName.SelectedItem.Text, a4SourceName.SelectedValue);

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

        #region call back
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _PhanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _PhanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                #region #3
                case "SelectEdit_a3":
                    kq = DanhMucTable_GetByID(_arg[0]);
                    break;
                case "a3AddSource_Onclick":
                    kq = DanhMucTable_Insert(_arg[0]);
                    break;
                case "a3UpdateSource_Onclick":
                    kq = DanhMucTable_Update(_arg[0]);
                    break;
                case "a3DeleteOnclick":
                    kq = DanhMucTable_Delete(_arg[0]);
                    break;
                case "a3grdSourceData_Load":
                    kq = a3grd_Load();
                    break;
                case "a3TableName_Load":
                    kq = a3TableName_Load();
                    break;
                case "a3SourceData_Load":
                    kq = a3SourceData_Load();
                    break;
                #endregion
                #region #2
                case "SelectEdit_a2":
                    kq = NguonDuLieu_GetByID(_arg[0]);
                    break;
                case "a2AddSource_Onclick":
                    kq = NguonDuLieu_Insert(_arg[0]);
                    break;
                case "a2UpdateSource_Onclick":
                    kq = NguonDuLieu_Update(_arg[0]);
                    break;
                case "a2DeleteOnclick":
                    kq = NguonDuLieu_Delete(_arg[0]);
                    break;
                case "a2grdSourceData_Load":
                    kq = a2grdSourceData_Load();
                    break;
                #endregion
                #region #4
                case "objDinhNghia_Delete":
                    kq = objDinhNghia_Delete(ThamSo[0], ThamSo[1]);
                    break;
                case "a4btnAddSource_Onclick":
                    kq = a4btnAddSource_Onclick(_arg[0]);
                    break;
                case "a4SourceName_Load":
                    kq = a4SourceName_Load(_arg[0]);
                    break;
                case "a4grd_Load":
                    kq = a4grd_Load(ThamSo[0], ThamSo[1]);
                    break;
                    #endregion
            }
            return kq;
        }
        #endregion

        #region function
        private static DateTime cToDate(string value)
        {
            //if (string.IsNullOrEmpty(value)) return new DateTime();
            //if (value.IndexOf("-") > 0)
            //{
            //    value = value.Replace("-", "");
            //}
            //else if (value.IndexOf("/") > 0) value = value.Replace("/", "");
            //return DateTime.ParseExact(value, "ddMMyyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return DateTime.Parse(value);
        }
        #region tab import #1
        protected void ddlTableName_Load()
        {
            try
            {
                ddlTableName.DataTextField = "VTABLENAME";
                ddlTableName.DataValueField = "VTABLENAME";
                ddlTableName.DataSource = new ImportDAL().DinhNghia_ddlTableName1();
                ddlTableName.DataBind();
                ddlTableName.SelectedIndex = 0;
            }
            catch
            {
            }

        }
        protected void ddlSourceData_Load()
        {
            try
            {
                ddlSourceData.DataTextField = "SOURCENAME";
                ddlSourceData.DataValueField = "ID";
                ddlSourceData.DataSource = new ImportDAL().DinhNghia_GetSourceData(ddlTableName.SelectedItem.Text);
                ddlSourceData.DataBind();
            }
            catch { }
        }
        private DataTable ReadDataFromExcel()
        {
            //Coneection String by default empty  
            string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            string ext = Path.GetExtension(fileUpload.FileName).ToLower();
            //getting the path of the file   
            string _fileName = DateTime.Now.ToString("ddMMyyyy") + fileUpload.FileName;
            string path = Server.MapPath("~/UploadMulti/" + _fileName);
            //saving the file inside the MyFolder of the server  
            fileUpload.SaveAs(path);
            //checking that extantion is .xls or .xlsx  
            if (ext.Trim() == ".xls")
            {
                //connection string for that file which extantion is .xls  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext.Trim() == ".xlsx")
            {
                //connection string for that file which extantion is .xlsx  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            //making query  
            string query = "SELECT * FROM [Sheet1$]";
            //Providing connection  
            OleDbConnection conn = new OleDbConnection(ConStr);
            //checking that connection state is closed or not if closed the   
            //open the connection  
            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    System.IO.File.Delete(path);
                }

            }
            //create command object  
            OleDbCommand cmd = new OleDbCommand(query, conn);
            // create a data adapter and get the data into dataadapter  
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            //fill the Excel data to data set  
            da.Fill(ds);
            //set data source of the grid view  

            //close the connection  
            conn.Close();
            System.IO.File.Delete(path);
            return ds.Tables[0];
        }
        private void XoaColumn(DataTable a, DataTable _dinhnghia)
        {
            List<DataColumn> lis = new List<DataColumn>();
            foreach (DataColumn cl in a.Columns)
            {
                int Nxoa = 0;
                foreach (DataRow r in _dinhnghia.Rows)
                {
                    if (cl.ColumnName == r["VCOLNAME"].ToString())
                    {
                        Nxoa++;
                    }
                }
                if (Nxoa == 0)
                {
                    lis.Add(cl);
                }
            }
            foreach (DataColumn cl in lis)
            {
                a.Columns.Remove(cl.ColumnName);
            }
        }
        public string CreateStore(DataTable _dinhNghia, string _tableName)
        {
            string kq = "";
            string tab = "\t";
            string xuongdong = "\r\n";
            string tenStoreTemple = "sp" + _tableName + "_InsertFromToll";
            kq += "create or replace PROCEDURE " + tenStoreTemple + xuongdong;
            kq += "(" + xuongdong;
            foreach (DataRow r in _dinhNghia.Rows)
            {
                //if (!r["ColType"].ToString().Contains("varchar"))
                //{
                kq += "P_" + r["VCOLNAME"].ToString() + tab + "IN" + tab + r["VCOLDATATYPE"].ToString() + "," + xuongdong;
                //}
                //else
                //{
                //    if (r["ColType"].ToString() != "-1")
                //    {
                //        kq += "P_" + r["ColName"].ToString() + tab + r["ColType"].ToString() + "(" + r["MaxType"].ToString() + ")" + "," + xuongdong;
                //    }
                //    else kq += "P_" + r["ColName"].ToString() + tab + r["ColType"].ToString() + "(max)" + "," + xuongdong;
                //}
            }
            kq = kq.Substring(0, kq.Length - 1 - xuongdong.Length) + xuongdong;
            kq += ")" + xuongdong;

            kq += "AS" + xuongdong;
            kq += "BEGIN" + xuongdong;
            kq += "INSERT INTO " + _tableName + "(" + xuongdong;
            foreach (DataRow r in _dinhNghia.Rows)
            {
                kq += r["VCOLNAME"].ToString() + "," + xuongdong;
            }
            kq = kq.Substring(0, kq.Length - 1 - xuongdong.Length);
            kq += ")" + xuongdong;
            kq += tab + "VALUES" + xuongdong;
            kq += tab + tab + "(" + xuongdong;
            foreach (DataRow dr in _dinhNghia.Rows)
            {
                //if(dr["VCOLDATATYPE"].ToString().Contains("VARCHAR"))     
                kq += "P_" + dr["VCOLNAME"].ToString() + "," + xuongdong;
            }
            kq = kq.Substring(0, kq.Length - 1 - xuongdong.Length);
            kq += ");" + xuongdong;
            kq += "END;" + xuongdong;
            return kq;
        }
        public void Insert(DataTable dt, DataTable _dt, string _tableName, DataTable _dinhNghia)
        {

            string ax = CreateStore(dtDinhNghia, _tableName);
            try
            {
                new oDataProvider().ExecuteNonQueryProduce(ax, CommandType.Text);
                //SqlHelper.ExecuteNonQuery(ConnApp, CommandType.Text, CreateStore(_dinhNghia, _tableName));
            }
            catch { }
            string[] colNRemove = new string[_dt.Columns.Count];
            int dem = 0;
            foreach (DataColumn cl in _dt.Columns)
            {
                foreach (DataRow r2 in _dinhNghia.Rows)
                {
                    if (r2["VCOLNAMEALIAS"].ToString() == cl.ColumnName)
                    {
                        cl.ColumnName = r2["VCOLNAME"].ToString();
                        colNRemove[dem] = cl.ColumnName;
                        dem++;
                    }
                }
            }
            XoaColumn(_dt, _dinhNghia);
            _dtDuLieuTrung = dt.Clone();
            _dtDuLieuTrung.Columns.Add("Error");
            _dtDuLieuTrung.Columns.Add("STT");
            Int64 demDong = 0;
            int er = 0;
            int rIndex = 0;

            List<OracleParameter> lis = new List<OracleParameter>();

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                int checkC = 0;
                lis = new List<OracleParameter>();
                try
                {
                    rIndex = i;
                    for (int j = 0; j < _dinhNghia.Rows.Count; j++)
                    {

                        OracleParameter pa = new OracleParameter();
                        pa.Value = _dt.Rows[i][j].ToString();
                        pa.ParameterName = "P_" + _dinhNghia.Rows[j]["VCOLNAME"].ToString();
                        if (_dinhNghia.Rows[j]["VCOLDATATYPE"].ToString().Equals("NVARCHAR2"))
                            pa.OracleDbType = OracleDbType.NVarchar2;
                        if (_dinhNghia.Rows[j]["VCOLDATATYPE"].ToString().Equals("VARCHAR2"))
                            pa.OracleDbType = OracleDbType.Varchar2;
                        if (_dinhNghia.Rows[j]["VCOLDATATYPE"].ToString() == "DATE")
                        {
                            pa.Value = cToDate(_dt.Rows[i][j].ToString());
                            pa.OracleDbType = OracleDbType.Date;
                        }
                        lis.Add(pa);
                        if (string.IsNullOrEmpty(_dt.Rows[i][j].ToString())) checkC++;
                    }
                    if (checkC == 0)
                    {
                        int df = new oDataProvider().ExecuteNonQueryProduce("sp" + _tableName + "_InsertFromToll", lis.ToArray());
                        if (df == 1)
                            demDong++;
                        else
                        {
                            _dtDuLieuTrung.ImportRow(dt.Rows[rIndex]);
                            _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["Error"] = "Import error!";
                            _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["STT"] = rIndex.ToString();
                        }
                    }
                    else
                    {
                        _dtDuLieuTrung.ImportRow(dt.Rows[rIndex]);
                        _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["Error"] = "No data!";
                        _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["STT"] = rIndex.ToString();
                    }
                }
                catch (Exception ex)
                {
                    _dtDuLieuTrung.ImportRow(dt.Rows[rIndex]);
                    _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["Error"] = ex.Message;
                    _dtDuLieuTrung.Rows[_dtDuLieuTrung.Rows.Count - 1]["STT"] = rIndex.ToString();
                    er++; demDong--;
                }
            }
            //SqlHelper.ExecuteNonQuery(ConnApp, CommandType.Text, "DROP PROCEDURE sp" + _tableName + "_InsertFromToll");
            new oDataProvider().ExecuteNonQueryProduce("DROP PROCEDURE sp" + _tableName + "_InsertFromToll", CommandType.Text);
            if (_dtDuLieuTrung.Rows.Count > 0)
            {
                grdNoImport.DataSource = _dtDuLieuTrung;
                grdNoImport.DataBind();
            }

            this.AlertMessage("Insert sucess: " + (demDong > 0 ? demDong.ToString() : "0") + "/" + _dt.Rows.Count.ToString());

            //frmDuLieuTrungLap f = new frmDuLieuTrungLap();
            //f.dtDuLieuTrung = _dtDuLieuTrung;
            //f.Show();

        }
        private void GetTableDinhNghia()
        {
            try
            {
                dtDinhNghia = new DataTable();
                dtDinhNghia = new ImportDAL().DinhNghiaDuLieu_dtDinhNghia(ddlTableName.SelectedValue, ddlSourceData.SelectedValue);
            }
            catch { }
        }
        #endregion

        #region tab list source #2
        protected string a2grdSourceData_Load()
        {
            try
            {
                a2grdSourceData.DataSource = new clsNguonDuLieu().GetAll();
                a2grdSourceData.DataBind();
                return this.RenderToHTML(a2grdSourceData);
            }
            catch { return ""; }
        }
        protected string NguonDuLieu_GetByID(string id)
        {
            clsNguonDuLieu obj = new clsNguonDuLieu().GetById(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        protected string NguonDuLieu_Insert(string sourceName)
        {
            return new clsNguonDuLieu().Insert(sourceName) ? "OK" : "NOK";
        }
        protected string NguonDuLieu_Update(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsNguonDuLieu>(thamso, dateTimeConverter);
            return new clsNguonDuLieu().Update(obj.ID.ToString(), obj.SOURCENAME) ? "OK" : "NOK";
        }
        protected string NguonDuLieu_Delete(string id)
        {
            return new clsNguonDuLieu().Delete(id) ? "OK" : "NOK";
        }
        #endregion

        #region tab attach #3
        protected string a3TableName_Load()
        {
            a3TableName.DataTextField = "TABLE_NAME";
            a3TableName.DataValueField = "TABLE_NAME";
            a3TableName.DataSource = new ImportDAL().GetAllTableInDB();
            a3TableName.DataBind();
            return this.RenderToHTML(a3TableName);
        }
        protected string a3SourceData_Load()
        {
            try { 
            a3SourceData.DataTextField = "SOURCENAME";
            a3SourceData.DataValueField = "ID";
            a3SourceData.DataSource = new clsNguonDuLieu().GetAll();
            a3SourceData.DataBind();
            return this.RenderToHTML(a3SourceData);
            }
            catch { return ""; }
        }
        protected string a3grd_Load()
        {
            try { 
            a3grd.DataSource = new clsDanhMucTable().GetAll();
            a3grd.DataBind();
            return this.RenderToHTML(a3grd);
            }
            catch { return ""; }
        }
        protected string DanhMucTable_GetByID(string thamso)
        {
            clsDanhMucTable obj = new clsDanhMucTable().GetById(thamso);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        protected string DanhMucTable_Delete(string thamso)
        {
            return new clsDanhMucTable().Delete(thamso) ? "OK" : "NOK";
        }
        protected string DanhMucTable_Insert(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDanhMucTable>(thamso, dateTimeConverter);
            return new clsDanhMucTable().Insert(obj) ? "OK" : "NOK";
        }
        protected string DanhMucTable_Update(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDanhMucTable>(thamso, dateTimeConverter);
            if (obj.ID == "0") return "NOK";
            return new clsDanhMucTable().Update(obj) ? "OK" : "NOK";
        }
        #endregion

        #region tab Define #4
        protected string a4TableName_Load()
        {
            a4TableName.DataTextField = "VTABLENAME";
            a4TableName.DataValueField = "VTABLENAME";
            a4TableName.DataSource = new ImportDAL().DinhNghia_GetTableDinhNghia();
            a4TableName.DataBind();
            return this.RenderToHTML(a4TableName);
        }
        protected string a4SourceName_Load(string tableName)
        {
            try
            {
                a4SourceName.DataTextField = "SOURCENAME";
                a4SourceName.DataValueField = "ID";
                a4SourceName.DataSource = new ImportDAL().a4SourceName_Load(tableName);
                a4SourceName.DataBind();
                return this.RenderToHTML(a4SourceName);
            }
            catch { return ""; }
        }
        protected string a4grd_Load(string tableName, string idSourceName)
        {
            try
            {
                a4grd.DataSource = new ImportDAL().DinhNghiaDuLieu_GetBy(tableName, idSourceName);
                a4grd.DataBind();
                return this.RenderToHTML(a4grd);
            }
            catch { return ""; }
        }
        protected string a4btnAddSource_Onclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDinhNghiaDuLieu>(thamso, dateTimeConverter);
            return new clsDinhNghiaDuLieu().Insert(obj) ? "OK" : "NOK";
        }
        protected string objDinhNghia_Delete(string tableName, string idsource)
        {
            return new clsDinhNghiaDuLieu().Delete(tableName, idsource) ? "OK" : "NOK";
        }
        #endregion
        #endregion

        #region grd event

        #endregion

        #region function class
        protected class ImportDAL
        {
            public bool GanTable4Source(clsDanhMucTable objTable, clsNguonDuLieu objSource)
            {
                return new oDataProvider().ExecuteNonQuery("danhmuctable_INSERT"
                    , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAME", objTable.VTABLENAME)
                    , new Oracle.DataAccess.Client.OracleParameter("P_VTABLENAMEALIAS", objTable.VTABLENAMEALIAS)
                    , new Oracle.DataAccess.Client.OracleParameter("P_NSOURCEDATA", objSource.ID)) == -1 ? false : true;
            }
            public DataTable GetAllTableInDB()
            {
                return new oDataProvider().ExecuteDataseProcduce("GetAllTable"
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable GetColumnTable(string tableName)
            {
                return new oDataProvider().ExecuteDatase($"SELECT COLUMN_NAME, DATA_TYPE from USER_TAB_COLUMNS where table_name={tableName.ToUpper()} order by COLUMN_ID ").Tables[0];
            }
            public bool DinhNghiaDuLieu_Insert(string idTableName, string idSourceName, string colName, string colAlias, string dataType)
            {
                return new oDataProvider().ExecuteNonQueryProduce("DinhNghiaDuLieu_Insert"
                    , new Oracle.DataAccess.Client.OracleParameter("P_IdTable", idTableName)
                    , new Oracle.DataAccess.Client.OracleParameter("P_ColName", colName)
                    , new Oracle.DataAccess.Client.OracleParameter("P_ColNameAlias", colAlias)
                    , new Oracle.DataAccess.Client.OracleParameter("P_ColDataType", dataType)
                    , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", idSourceName)) == -1 ? false : true;
            }
            public bool DinhNghiaDuLieu_DeleteByGroup(string idTableName, string idSourceName)
            {
                return new oDataProvider().ExecuteNonQueryProduce("DinhNghiaDuLieu_DeleteGroup"
                    , new Oracle.DataAccess.Client.OracleParameter("P_IdTable", idTableName)
                    , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", idSourceName)) == -1 ? false : true;
            }
            public DataTable DinhNghia_ddlTableName1()
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghia_ddlTableName1"
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable DinhNghiaDuLieu_GetBy(string idTable, string idSourceData)
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghiaDuLieu_GetBy"
                    , new Oracle.DataAccess.Client.OracleParameter("P_TableName", idTable)
                    , new Oracle.DataAccess.Client.OracleParameter("P_SourceData", idSourceData)
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable DinhNghiaDuLieu_dtDinhNghia(string idtable, string idsource)
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghiaDuLieu_dtDinhNghia"
                    , new Oracle.DataAccess.Client.OracleParameter("P_IdTable", idtable)
                    , new Oracle.DataAccess.Client.OracleParameter("P_IdSourceData", idsource)
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable DinhNghia_GetTableName(string tableName)
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghia_GetTableName"
                    , new Oracle.DataAccess.Client.OracleParameter("P_TableName", tableName)
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable DinhNghia_GetTableDinhNghia()
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghia_GetTableDinhNghia"
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable DinhNghia_GetSourceData(string tableName)
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghia_GetSourceData"
                    , new Oracle.DataAccess.Client.OracleParameter("P_TableName", tableName)
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
            public DataTable a4SourceName_Load(string tableName)
            {
                return new oDataProvider().ExecuteDataseProcduce("DinhNghia_a4SourceName_Load"
                    , new Oracle.DataAccess.Client.OracleParameter("P_TableName", tableName)
                    , new Oracle.DataAccess.Client.OracleParameter("mycursor", Oracle.DataAccess.Client.OracleDbType.RefCursor, ParameterDirection.Output));
            }
        }
        #endregion

        #region control event
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                GetTableDinhNghia();
                DataTable dtExcel = ReadDataFromExcel();
                //Insert(dtExcel, dtExcel, (cbbTenBang.SelectedItem as CustomItem)._Value, dtDinhNghia);

                foreach (DataRow r in dtExcel.Rows)
                {
                    string From_AIRP = r["From_To"].ToString().Length > 5 ? r["From_To"].ToString().Substring(0, 4): r["From_To"].ToString();
                    string TO_AIRP = r["From_To"].ToString().Length > 5 ? r["From_To"].ToString().Substring(5, 4) : r["From_To"].ToString();
                    var ax = new clsResuftAPI().GetValueApiExtension("IMPORT_PKG", "spImportVia", new
                    {
                        P_CRAFT_TYPE = r["Craft"].ToString(),
                        P_VIA = r["Via"].ToString()
                        ,
                        P_FROM_AIRP = From_AIRP
                        ,
                        P_TO_AIRP = TO_AIRP
                        ,
                        P_OPER = r["Oper"].ToString()
                    });
                }

                /*
                 * 16042018 update for insert onnly M_VIA
                 * 
                Insert(dtExcel, dtExcel, ddlTableName.SelectedItem.Text, dtDinhNghia);
                */
            }
            catch (Exception ex)
            {

                this.AlertMessage(ex.Message);
            }
        }
        protected void ddlTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSourceData_Load();
            ddlSourceData_SelectedIndexChanged(sender, e);
        }
        protected void ddlSourceData_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetTableDinhNghia();
        }
        #endregion


    }


}
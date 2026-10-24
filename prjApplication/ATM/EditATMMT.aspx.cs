using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;
using System.Data;

namespace prjApplication.ATM
{
    public partial class EditATMMT : PageBaseCallBack
    {
        private const string _AliasSession = "EditATMMT";
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
                txtBEGINDATE.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            Upload();
        }
        public void Upload()
        {
            
            string _date = txtBEGINDATE.Value;
            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            string _pathdate = _y + "\\" + _m + "\\" + _d;

            //Server.MapPath("http:\\123.34.212.77\\WebApp\\upload\\" + X);
            string _r = System.Configuration.ConfigurationManager.AppSettings["FileUploadPathMT"];

            //string root = Server.MapPath("~/upload/ShareATM/MB/" + _pathdate + "/");
            string root = _r + _pathdate + "\\";
            //lit.Text = root;
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            if (FileUpload1.PostedFile != null)
            {
                if (FileUpload1.PostedFile.FileName.Length > 0)
                {
                    DataTable dtFmt = new DataTable();
                    DataTable dt = CreateDataTableFormat();
                    int i = 0;
                    foreach (var file in FileUpload1.PostedFiles)
                    {
                        string filename = file.FileName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
                        string filePath = _r + _pathdate + "\\" + System.IO.Path.GetFileName(file.FileName);
                        file.SaveAs(filePath);

                        DataRow dR = dt.NewRow();
                        dR["SlNo"] = i + 1;
                        dR["FileName"] = System.IO.Path.GetFileName(file.FileName.Trim());
                        dt.Rows.Add(dR);
                        i = i + 1;
                    }
                    grdS.DataSource = dt;
                    grdS.DataBind();
                }
            }

        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _phanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[0])
            {
                case "Upload":
                    Upload();
                    kq = "Upload";
                    break;
            }
            return kq;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            btnCapNhat.Visible = true;
            FileUpload1.Visible = true;
            btnNew.Visible = false;

            grdSource.DataSource = null;
            grdSource.DataBind();

            grdS.DataSource = null;
            grdS.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
            {

            UserDAL _obj = new UserDAL();
            string _r = System.Configuration.ConfigurationManager.AppSettings["FileUploadPathMT"];

            string _date = txtBEGINDATE.Value;
            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            string _pathdate = _y + "\\" + _m + "\\" + _d;
            string _pathdatesave = _y + "_" + _m + "_" + _d;
            string Pathi = _r + _pathdate + "\\";
            string sFileExt = "*.*";
            LoadFilesFromPath(Pathi, sFileExt, _pathdatesave);

            btnCapNhat.Visible = false;
            btnNew.Visible = true;
            FileUpload1.Visible = false;
        }
        #region Load FileName in Grid
        private void LoadFilesFromPath(string Path, string sFileExt,string _pathdate)
        {
            UserDAL _obj = new UserDAL();
            grdSource.Visible = true;
            try
            {
                DataTable dtFmt = new DataTable();
                DataTable dt = CreateDataTableFormat();
                if (System.IO.Directory.Exists(Path))
                {
                    String[] arFileList;
                    arFileList = Directory.GetFiles(Path, sFileExt);
                    for (int RowNo = 0; RowNo <= arFileList.Length - 1; RowNo++)
                    {
                       

                        string[] strFolder = arFileList[RowNo].Split('\\');
                        string lstFileName = strFolder[strFolder.Length - 1];
                        DataRow dR = dt.NewRow();
                        dR["SlNo"] = RowNo + 1;
                        dR["FileName"] = lstFileName.Trim();
                        dt.Rows.Add(dR);

                        _obj.InsertATMExp(lstFileName.Trim(), "", "MT", _pathdate);
                    }
                }
                grdSource.DataSource = dt;
                grdSource.DataBind();
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
        private void LoadFilesFromPath(string Path, string sFileExt)
        {
            UserDAL _obj = new UserDAL();
            grdSource.Visible = true;
            try
            {
                DataTable dtFmt = new DataTable();
                DataTable dt = CreateDataTableFormat();
                if (System.IO.Directory.Exists(Path))
                {
                    String[] arFileList;
                    arFileList = Directory.GetFiles(Path, sFileExt);
                    for (int RowNo = 0; RowNo <= arFileList.Length - 1; RowNo++)
                    {


                        string[] strFolder = arFileList[RowNo].Split('\\');
                        string lstFileName = strFolder[strFolder.Length - 1];
                        DataRow dR = dt.NewRow();
                        dR["SlNo"] = RowNo + 1;
                        dR["FileName"] = lstFileName.Trim();
                        dt.Rows.Add(dR);

                        
                    }
                }
                grdSource.DataSource = dt;
                grdSource.DataBind();
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
        #endregion

        #region Datatable Creation
        private DataTable CreateDataTableFormat()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SlNo");
                dt.Columns.Add("FileName");
                return (dt);

            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region function
        public void LoadData()
        {          
            UserDAL _obj = new UserDAL();
            DataTable t = _obj.getATMExp("1");
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        private void PutclsSearchDetail()
        {
          
        }


        #endregion

        #region control-event
        protected void pages_IndexChanged(object sender, EventArgs e)
        {

            LoadData();
        }

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            
        }

       

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Document/EditDocument.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #endregion

        #region grd event

        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            UserDAL _obj = new UserDAL();
            string _date = txtBEGINDATE.Value;
            //'2017_01_20'
            //17/05/2018
            string path = System.Configuration.ConfigurationManager.AppSettings["ATMPATH"];
            string _d = _date.Substring(0, 2);
            string _m = _date.Substring(3, 2);
            string _y = _date.Substring(6, 4);
            string _pathdate = _y + "\\" + _m + "\\" + _d;
            string _pathdatesave = _y + "_" + _m + "_" + _d;
            string Name = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "DeleteByID":
                    //Response.Redirect(path + "MB/" + _pathdate + "/" + Name.ToString() + ".xls");                   
                    string _r = System.Configuration.ConfigurationManager.AppSettings["FileUploadPathMT"];
                    string filePath = _r + _pathdate + "\\" + Name.ToString();
                    File.Delete(filePath);
                    string sFileExt = "*.*";
                    
                    _obj.DeleATMExp(Name.ToString(),"MT", _pathdatesave);
                    LoadFilesFromPath(_r + _pathdate + "\\", sFileExt);

                    break;
                case "View":
                    Response.Redirect(path + "MT/" + _pathdate + "/" + Name.ToString() );       
                    break;
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
       
        #endregion

        #region pdf excel 

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
           
        }

        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion
    }
}


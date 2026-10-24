using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using System.Data;
using System.IO;

namespace prjApplication.Until
{
    public partial class LoadImg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                int _CATID  = Convert.ToInt32(Request["catid"]);
                string _tenfile  = Convert.ToString(Request["tenfile"]);
                int _UserID  = Convert.ToInt32(Request["UserID"]);
                string txtFromDate  = Convert.ToString(Request["txtFromDate"]);
                string txtTodate  = Convert.ToString(Request["txtTodate"]);
                ListImages(_CATID, _tenfile, _UserID, txtFromDate, txtTodate);
            }
        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }
        public string Cut_Filename(object filename)
        {
            string _Name = "";
            try
            {
                _Name = filename.ToString();
                if (_Name.Length > 13)
                {
                    _Name = _Name.Substring(0, 11) + "...";
                }
            }
            catch { ;}
            return _Name;
        }
        public string GetFileURL(object Url)
        {
            string _url = "";
            try { _url = Url.ToString(); }
            catch { ;}
            if (!string.IsNullOrEmpty(_url))
            {
                _url = prjComponents.Global.TinPath + _url;
                string ex = Path.GetExtension(_url);
                if (!string.IsNullOrEmpty(ex))
                {
                    int type = GetFileType(ex);
                    if (type == 2)
                        _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/Video-icon.png";
                    else if (type == 3)
                        _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/AnotherFile.bmp";
                }
            }
            else
            {
                _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/NoImage.png";
            }
            return _url;
        }
        public int GetFileType(string FileExtend)
        {
            if (FileExtend.Trim().ToLower() == ".jpg" || FileExtend.Trim().ToLower() == ".png"
                || FileExtend.Trim().ToLower() == ".gif" || FileExtend.Trim().ToLower() == ".tif"
                || FileExtend.Trim().ToLower() == ".bmp" || FileExtend.Trim().ToLower() == ".jpeg"
                )
                return 1;
            else if (FileExtend.Trim().ToLower() == ".avi" || FileExtend.Trim().ToLower() == ".flv"
                || FileExtend.Trim().ToLower() == ".mp4" || FileExtend.Trim().ToLower() == ".wmv"
                || FileExtend.Trim().ToLower() == ".mpg" || FileExtend.Trim().ToLower() == ".3gp"
                || FileExtend.Trim().ToLower() == ".wma" || FileExtend.Trim().ToLower() == ".mpeg"
                || FileExtend.Trim().ToLower() == ".swf"
                )
                return 2;
            else
                return 3;
        }
        private void ListImages(int _CATID, string _tenfile, int _UserID, string txtFromDate, string txtTodate)
        {
            ImageFilesDAL _untilDAL = new ImageFilesDAL();
            DataSet _ds = null;
            string where = " UserCreated =" + _UserID;
            if (_tenfile.Length > 0)
                where += " AND ImageFileName like N'%" + _tenfile + "%'";
            if (_CATID > 0)
                where += string.Format(" AND Categorys_ID IN (SELECT * FROM [fn_Return_Category_Tree] ({0}))", _CATID);
            if (txtFromDate.Length > 0)
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)>=0)", UltilFunc.ToDate(txtFromDate.Trim(), "MM/dd/yyyy"));
            if (txtTodate.Length > 0)
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)<=0) ", UltilFunc.ToDate(txtTodate.Trim(), "MM/dd/yyyy"));
            else where += " AND " + string.Format(" DATEDIFF(DAY,DateCreated,'{0}')=0 ", DateTime.Now.ToString("MM/dd/yyyy"));
            where += " ORDER BY DateCreated DESC";
            _ds = _untilDAL.BindGridT_ImageFiles(0, 10, where);
            if (_ds != null)
            {
                this.dlImages.DataSource = _ds.Tables[0];
                this.dlImages.DataBind();
            }
        }
    }
}

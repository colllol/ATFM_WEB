using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using HPCServerDataAccess;

namespace prjApplication.UploadMulti
{
    public partial class Upload : System.Web.UI.Page
    {
        protected string strNumberArg = "1";
        protected string vType = "1";
        T_Users _user = new T_Users();
        UserDAL _userDAL = new UserDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["vType"] != null)
                vType = Request.QueryString["vType"].ToString();
            if (Request.QueryString["vKey"] != null)
                strNumberArg = Request.QueryString["vKey"].ToString();
            _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user == null)
            {
                Response.Redirect(Global.ApplicationPath + "/Login.aspx", true);
            }
            if (!this.IsPostBack)
            {
                if (_user != null)
                {
                    txt_FromDate.Text = DateTime.Now.AddDays(-30).ToString("dd'/'MM'/'yyyy");
                    txt_ToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    ListImages();
                }
            }
        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }
        protected string getFileName(object objFileType, object objFileName)
        {
            string strFilename = "";
            if (objFileName.ToString() != "")
            {
                if (objFileType.ToString() == "1")
                    strFilename = objFileName.ToString().Substring(4);
                else
                    strFilename = objFileName.ToString();
            }
            return strFilename;
        }
        private void ListImages()
        {
            ImageFilesDAL obj = new ImageFilesDAL();
            DataSet objDataset = new DataSet();
            //string where = " UserCreated =" + _user.UserID;
            string where = " 1=1 ";
            if(vType=="1")
                where += " AND ImageType = 1 ";
            if (vType == "2")
                where += " AND ImageType IN(2,3,4) ";
            if (!String.IsNullOrEmpty(fileName.Text.Trim()))
                where += " AND " + string.Format(" ImageFileName like N'%{0}%'", UltilFunc.SqlFormatText(this.fileName.Text.Trim()));
            if (!String.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)>=0) ", UltilFunc.ToDate(this.txt_FromDate.Value.ToString().Trim(), "MM/dd/yyyy"));
            if (!String.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)<=0) ", UltilFunc.ToDate(this.txt_ToDate.Value.ToString().Trim(), "MM/dd/yyyy"));
            else where += " AND " + string.Format(" DATEDIFF(DAY,DateCreated,'{0}')=0 ", DateTime.Now.ToString("MM/dd/yyyy"));
            where += " ORDER BY DateCreated DESC";
            objDataset = obj.ListAllImages(where);
            if (objDataset != null)
            {
                DataView _dv = obj.BindGridListImages(objDataset.Tables[0]);
                dlImages.DataSource = _dv;
                dlImages.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ListImages();
        }

        protected Int16 getFileType(string FileExtension)
        {
            Int16 intFileType = 0;
            if (FileExtension.Trim() != null)
            {
                if (FileExtension.ToLower() == "png" || FileExtension.ToLower() == "jpeg" || FileExtension.ToLower() == "jpg" || FileExtension.ToLower() == "bmp" || FileExtension.ToLower() == "gif" || FileExtension.ToLower() == "ico")
                    intFileType = 1;
                else if (FileExtension.ToLower() == "wmv" || FileExtension.ToLower() == "swf" || FileExtension.ToLower() == "flv" || FileExtension.ToLower() == "mpeg" || FileExtension.ToLower() == "avi")
                    intFileType = 2;
                else if (FileExtension.ToLower() == "mp3" || FileExtension.ToLower() == "wma")
                    intFileType = 3;
                else if (FileExtension.ToLower() == "doc" || FileExtension.ToLower() == "docx" || FileExtension.ToLower() == "txt" || FileExtension.ToLower() == "xls" || FileExtension.ToLower() == "xlsx" || FileExtension.ToLower() == "pdf")
                    intFileType = 4;
                else
                    intFileType =5;
            }

            return intFileType;
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
        protected void dlImages_EditCommand(object source, DataListCommandEventArgs e)
        {
            // xóa ảnh
            Label lblid = (Label)e.Item.FindControl("lbl_ID");
            Label lblURL = (Label)e.Item.FindControl("lbl_URL");
            int ImageID = int.Parse(lblid.Text);
            ImageFilesDAL obj = new ImageFilesDAL();
            try
            {
                string strRootPathVirtual = System.Configuration.ConfigurationManager.AppSettings["UploadPath"] + lblURL.Text;
                string savepath = Server.MapPath(strRootPathVirtual);
                if (File.Exists(savepath))
                    File.Delete(savepath);
            }
            catch { ;}
            obj.Delete_Image(ImageID);
            ListImages();
        }


        public string GetFileURL(object Url)
        {
            string _url = "";
            try { _url = Url.ToString(); }
            catch { ;}
            if (!string.IsNullOrEmpty(_url))
            {
                _url = Global.TinPath + Global.UploadPath + _url;
                string ex = Path.GetExtension(_url);
                if (!string.IsNullOrEmpty(ex))
                {
                    int type = GetFileType(ex);
                    if (type == 2)
                        _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/ico_video.png";
                    if (type == 3)
                        _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/audio_itunes.png";
                    if (type == 4)
                        _url = System.Configuration.ConfigurationManager.AppSettings["ApplicationPath"].ToString() + "/Images/doc_tests.png";
                    else if (type == 5)
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
                || FileExtend.Trim().ToLower() == ".bmp" || FileExtend.Trim().ToLower() == ".jpeg"||FileExtend.Trim().ToLower() == ".ico"
                )
                return 1;
            else if (FileExtend.Trim().ToLower() == ".avi" || FileExtend.Trim().ToLower() == ".flv"
                || FileExtend.Trim().ToLower() == ".mp4" || FileExtend.Trim().ToLower() == ".wmv"
                || FileExtend.Trim().ToLower() == ".mpg" || FileExtend.Trim().ToLower() == ".3gp"
                || FileExtend.Trim().ToLower() == ".mpeg"
                )
                return 2;
            else if (FileExtend.Trim().ToLower() == ".mp3" || FileExtend.Trim().ToLower() == ".wma"           
                )
                return 3;
            else if (FileExtend.Trim().ToLower() == ".doc" || FileExtend.Trim().ToLower() == ".docx"
                || FileExtend.Trim().ToLower() == ".xls" || FileExtend.Trim().ToLower() == ".xlsx"
                || FileExtend.Trim().ToLower() == ".txt" || FileExtend.Trim().ToLower() == ".pdf"            
                 )
                return 4;
            else
                return 5;
        }
        public string GetUserName()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString() + "," + GetvType() + "," + HPCSecurity.CurrentUser.Identity.ID.ToString() + "," + GetChuyenmuc();
            return strTemp;
        }
        public string GetvType()
        {
            string strTemp = HttpContext.Current.Request.QueryString["vType"].ToString();
            return strTemp;
        }
        public string GetvKey()
        {
            string strTemp = HttpContext.Current.Request.QueryString["vKey"].ToString();
            return strTemp;
        }
        public string GetChuyenmuc()
        {
            string strTemp = "0";
            return strTemp;
        }
    }
}

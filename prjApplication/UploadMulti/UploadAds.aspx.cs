using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using prjInfo;
using prjBusinessLogic;
using prjComponents;

namespace prjApplication.UploadMulti
{
    public partial class UploadAds : System.Web.UI.Page
    {
       
        T_Users _user = new T_Users();
        UserDAL _DAL = new UserDAL();
        protected string strNumberArg = "1";
        protected string strVtypeArg = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString["vType"] != null)
                strVtypeArg = HttpContext.Current.Request.QueryString["vType"].ToString();
            if (Request.QueryString["vKey"] != null)
                strNumberArg = Request.QueryString["vKey"].ToString();
            _user = _DAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user == null)
            {
                Response.Redirect(Global.ApplicationPath + "/Login.aspx");
            }
            if (!this.IsPostBack)
            {
                if (_user != null)
                {
                    LoadCM();
                    txt_FromDate.Text = DateTime.Now.AddDays(-15).ToString("dd'/'MM'/'yyyy");
                    txt_ToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    ListImages();
                }
            }
            else
            {
                string EventName = Request.Form["__EVENTTARGET"].ToString();
                if (EventName == "UploadSuccess")
                {
                    ListImages();
                }
            }

        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }
        private void ListImages()
        {
            ImageFilesDAL obj = new ImageFilesDAL();
            DataSet objDataset = new DataSet();
            try
            {
                //string where = " UserCreated =" + _user.UserID;
                string where = "";
                where += " 1=1 And (( AuthorID =0 ) OR (AuthorID is null ))";
                if (!string.IsNullOrEmpty(fileName.Text))
                    where += " AND ImageFileName like N'%" + fileName.Text + "%'";
                if (Drop_Chuyenmuc.SelectedValue.Trim() != "0")
                    where += string.Format(" AND Categorys_ID IN (SELECT * FROM [fn_Return_Category_Tree] ({0}))", this.Drop_Chuyenmuc.SelectedValue);
                if (!String.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                    where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)>=0) ", UltilFunc.ToDate(this.txt_FromDate.Value.ToString().Trim(), "MM/dd/yyyy"));
                if (!String.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                    where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)<=0)", UltilFunc.ToDate(this.txt_ToDate.Value.ToString().Trim(), "MM/dd/yyyy"));
                else
                {
                    where += " AND " + string.Format(" DATEDIFF(DAY,DateCreated,'{0}')=0 ", DateTime.Now.ToString("MM/dd/yyyy"));
                }
                if (strVtypeArg == "3")
                    where += " AND ImageType IN (3) ";
                where += " ORDER BY DateCreated DESC";
                objDataset = obj.ListAllImages(where);
                if (objDataset != null)
                {
                    DataView _dv = obj.BindGridListImages(objDataset.Tables[0]);
                    dlImages.DataSource = _dv;
                    dlImages.DataBind();
                }
                objDataset.Clear();
            }
            catch
            {
                dlImages.DataSource = null;
                dlImages.DataBind();
            }
        }

        public string GetUserName()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString() + "," + GetvType() + "," + HPCSecurity.CurrentUser.Identity.ID.ToString() + "," + GetChuyenmuc();
            return strTemp;

        }
        public string GetvType()
        {
            string strTemp = "1";
            if (HttpContext.Current.Request.QueryString["vType"] != null)
                strTemp = HttpContext.Current.Request.QueryString["vType"].ToString();
            return strTemp;
        }
        public string GetChuyenmuc()
        {
            string strTemp = Drop_Chuyenmuc.SelectedValue.Trim();
            return strTemp;
        }
        public void LoadCM()
        {
            UltilFunc.BindCombox(Drop_Lang, "Languages_ID", "Languages_Name", "T_Languages", string.Format(" 1=1 AND Languages_ID IN ({0}) Order by Languages_Name ", UltilFunc.GetLanguagesByUser(_user.UserID)), "---Tất cả---");
            if (Drop_Lang.Items.Count >= 3)
                Drop_Lang.SelectedIndex = prjComponents.Global.DefaultLangID;
            else Drop_Lang.SelectedIndex = UltilFunc.GetIndexControl(Drop_Lang, prjComponents.Global.DefaultCombobox);
            if (Drop_Lang.SelectedIndex != 0)
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID= " + this.Drop_Lang.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ListImages();
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
        protected void Drop_Lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drop_Chuyenmuc.Items.Clear();
            if (Drop_Lang.SelectedIndex >= 0)
            {
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID= " + this.Drop_Lang.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
                //Drop_Chuyenmuc.UpdateAfterCallBack = true;
            }
            else
            {
                this.Drop_Chuyenmuc.DataSource = null;
                this.Drop_Chuyenmuc.DataBind();
                //this.Drop_Chuyenmuc.UpdateAfterCallBack = true;
            }
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
                {
                    File.Delete(savepath);
                    WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Xóa ảnh]", Convert.ToInt32(Request["Menu_ID"]).ToString(), "[Xóa ảnh] [Thao tác xóa ảnh trên server: " + lblURL.Text + "]", 0);
                }
            }
            catch { ;}
            obj.Delete_Image(ImageID);
            ListImages();
        }

        protected void Drop_Chuyenmuc_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                || FileExtend.Trim().ToLower() == ".bmp" || FileExtend.Trim().ToLower() == ".jpeg" || FileExtend.Trim().ToLower() == ".ico"
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

        protected void DropStyle_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public string GetWidthImage(object _Path)
        {
            string _return = "0";
            try
            {
                string sourcepath = ConfigurationManager.AppSettings["ServerPathDis"] + _Path.ToString();
                System.Drawing.Image objImage = System.Drawing.Image.FromFile(sourcepath);
                double width = objImage.Width;
                if (width.ToString() != "0")
                    _return = width.ToString();
            }
            catch
            {
                _return = "0";
            }
            return _return;
        }
        public string GetHeightImage(object _Path)
        {
            string _return = "0";
            try
            {
                string sourcepath = ConfigurationManager.AppSettings["ServerPathDis"] + _Path.ToString();
                System.Drawing.Image objImage = System.Drawing.Image.FromFile(sourcepath);
                double height = objImage.Height;
                if (height.ToString() != "0")
                    _return = height.ToString();
            }
            catch
            {
                _return = "0";
            }
            return _return;
        }
        
    }
}
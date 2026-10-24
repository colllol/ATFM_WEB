using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using prjComponents;
using prjBusinessLogic;
using prjInfo;

namespace prjApplication.Until
{
    public partial class LoadImagAjax : System.Web.UI.Page
    {
        public string strNumberArg = "1";
        T_Users _user = new T_Users();
        UserDAL _DAL = new UserDAL();
        public int CurrentPage
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_CurrentPage"];
                if (o == null)
                    return 0;	// default to showing the first page
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_CurrentPage"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            strNumberArg = HttpContext.Current.Request.QueryString["vType"].ToString();
            _user = _DAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user == null)
            {
                Response.Redirect(Global.ApplicationPath + "/Login.aspx", true);
            }
            if (!this.IsPostBack)
            {                
                if (_user != null)
                {                   
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
            try
            {
                ImageFilesDAL obj = new ImageFilesDAL();
                DataSet objDataset = new DataSet();

                // Populate the repeater control with the Items DataSet
                PagedDataSource objPds = new PagedDataSource();
                string where = " UserCreated =" + _user.UserID;
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)>=0)", UltilFunc.ToDate(DateTime.Now.AddDays(-7).ToString(), "MM/dd/yyyy"));
                where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)<=0) ", UltilFunc.ToDate(DateTime.Now.ToString(), "MM/dd/yyyy"));                
                where += " ORDER BY DateCreated DESC";
                objDataset = obj.ListAllImages(where);
                if (objDataset != null)
                {
                    DataTable _dv = obj.BindGridListImages2Table(objDataset.Tables[0]);
                    objPds.DataSource = _dv.DefaultView;
                    objPds.AllowPaging = true;
                    objPds.PageSize = 12;

                    objPds.CurrentPageIndex = CurrentPage;

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " / " + objPds.PageCount.ToString();

                    // Disable Prev or Next buttons if necessary

                    cmdPrev.Enabled = !objPds.IsFirstPage;
                    cmdNext.Enabled = !objPds.IsLastPage;

                    //dlImages.DataSource = _dv;
                    dlImages.DataSource = objPds;
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
        public string GetvType()
        {
            string strTemp = HttpContext.Current.Request.QueryString["vType"].ToString();
            return strTemp;
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
                //if (_Name.Length > 13)
                //{
                //    _Name = _Name.Substring(0, 11) + "...";
                //}
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
                {
                    File.Delete(savepath);
                    WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Xóa ảnh]", Convert.ToInt32(Request["Menu_ID"]).ToString(), "[Xóa ảnh] [Thao tác xóa ảnh trên server: " + lblURL.Text + "]", 0);
                }
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
                _url = Global.TinPath + _url;
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

        protected void cmdPrev_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPage -= 1;

            // Reload control
            ListImages();
        }

        protected void cmdNext_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage += 1;

            // Reload control
            ListImages();
        }
    }
}

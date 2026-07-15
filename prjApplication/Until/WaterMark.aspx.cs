using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using prjComponents;
using prjInfo;
using prjBusinessLogic;
using System.IO;
using System.Drawing;
using HPCServerDataAccess;

namespace prjApplication.Until
{
    public partial class WaterMark : System.Web.UI.Page
    {
        public string strNumberArg = "1";
        public string strPathImage = "";
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
            // strNumberArg = HttpContext.Current.Request.QueryString["vType"].ToString();

            _user = _DAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user == null)
            {
                Response.Redirect(Global.ApplicationPath + "/Login.aspx", true);
            }
            if (!this.IsPostBack)
            {                
                if (_user != null)
                {
                   
                }
            }
            

        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }
        
        public string GetUserName()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString() + "," + GetvType() + "," + HPCSecurity.CurrentUser.Identity.ID.ToString() + "," + "1";
            return strTemp;
        }
        public string GetUserID()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString();
            return strTemp;
        }
        public string GetvType()
        {
            string strTemp = "1";//HttpContext.Current.Request.QueryString["vType"].ToString();
            return strTemp;
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
            txt_UrlImage.Text = lblURL.Text;
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
            
        }
        public string GetFileURL(object Url)
        {
            string _url = "";
            try { _url = Url.ToString(); }
            catch { ;}
            if (!string.IsNullOrEmpty(_url))
            {
                //_url = Global.TinPath + _url;
                _url = Global.TinPath + Global.UploadPath + _url; //_url;
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

       
        protected void cmd_watermark_Click(object sender, EventArgs e)
        {

            string url1 = txt_UrlImage.Text.Trim();
            if (!string.IsNullOrEmpty(url1))
            {
                string url = ConfigurationManager.AppSettings["ServerPathDis"].ToString() + url1;
                if (File.Exists(url))
                {
                    string strPhysLocal = "";
                    if (strNumberArg == "1")
                        strPhysLocal = "/" + "Article/Thumnail/";
                    if (strNumberArg == "2")
                        strPhysLocal = "/" + Global.UploadPhotoAlbum + "/";
                    if (strNumberArg == "3")
                        strPhysLocal = "/" + Global.UploadPhotoEvent + "/";

                    //string width = DropSize.SelectedValue;
                    System.Drawing.Bitmap sourceImage = new System.Drawing.Bitmap(url);


                    //Double image_W = sourceImage.Width;
                    //Double image_H = sourceImage.Height;
                    //Double tyle = image_W / image_H;
                    //Double save_w = double.Parse(width);
                    //Double save_h = save_w / tyle;
                    //int int_save_w = Convert.ToInt32(save_w);
                    //int int_save_h = Convert.ToInt32(save_h);
                    //if (image_W <= save_w)
                    //{
                    //    int_save_w = Convert.ToInt32(image_W);
                    //    int_save_h = Convert.ToInt32(image_H);
                    //}
                    string _extension = Path.GetExtension(url);

                    string filename = "WaterMark_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + DateTime.Now.Millisecond.ToString() + Path.GetExtension(url);
                    Bitmap imgsave = null;
                    string newfolder = strPhysLocal + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                    string pathsave = ConfigurationManager.AppSettings["ServerPathDis"].ToString() + newfolder;
                    string _logo = Server.MapPath("../Images/IconHPC/LoGoBaoAnhDong.png");
                    Bitmap Imagemark = new Bitmap(_logo);
                    int spacevalues = 0;
                    try { spacevalues = int.Parse(ConfigurationManager.AppSettings["SpaceValue"].ToString()); }
                    catch { ;}

                    imgsave = HPCImages.WatermarkImages(sourceImage, Imagemark, int.Parse(DropStyle.SelectedValue), spacevalues);

                    if (Directory.Exists(pathsave) == false) Directory.CreateDirectory(pathsave);

                    imgsave.Save(pathsave + @"\" + filename);
                    imgsave.Dispose();
                    Imagemark.Dispose();
                    sourceImage.Dispose();

                    txt_UrlImage.Text = newfolder + "/" + filename;
                   
                    T_ImageFiles _obj = new T_ImageFiles();
                    ImageFilesDAL _DAL = new ImageFilesDAL();
                    _obj = SetItem(filename, 0, txt_UrlImage.Text, _extension, _user.UserID, Convert.ToInt16(strNumberArg), 1);
                    _DAL.InsertT_ImageFiles(_obj);
                   
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "PreviewImage('" + newfolder + "/" + filename + "','" + strNumberArg + "','0','" + _extension + "');", true);
                
                }

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Hãy chọn ảnh để đóng dấu ');", true);
            }
        }
        protected T_ImageFiles SetItem(string _tenFile, double _size, string _pathfile, string _extenfile, int _UserID, Int16 vType, double chuyenmuc)
        {
            T_ImageFiles _obj = new T_ImageFiles();
            _obj.ImageFileName = _tenFile.ToString();
            _obj.ImageFileSize = _size;
            _obj.ImageFileExtension = _extenfile.ToString();
            _obj.ImageType = vType;
            _obj.ImgeFilePath = _pathfile.ToString();
            _obj.Status = 0;
            _obj.UserCreated = _UserID;
            _obj.DateCreated = DateTime.Now;
            _obj.Categorys_ID = chuyenmuc;

            return _obj;
        }

    }
}

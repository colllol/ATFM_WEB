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
using HPCServerDataAccess;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Net;

namespace prjApplication.Until
{
    public partial class FileManager : System.Web.UI.Page
    {
        
        public string strNumberArg = "1";
        public string strPathImage = "";
        T_Users _user = new T_Users();
        UserDAL _DAL = new UserDAL();
        FTPClass _ftphelp = new FTPClass();
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
                if (HttpContext.Current.Request.QueryString["imgPath"] != null && HttpContext.Current.Request.QueryString["imgPath"].ToString() != "")
                    strPathImage = HttpContext.Current.Request.QueryString["imgPath"].ToString();
                if (strPathImage != "")
                {
                    lblFilename.InnerHtml = "Thư mục:" + strPathImage;
                    string v_path = strPathImage.Replace("/upload/", "");
                    txt_UrlImage.Text = v_path;
                    ContainerCrop.InnerHtml = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + strPathImage + "','','','');\"  src=\"" + Global.TinPath_CMS()+ strPathImage + "\" />";
                }                
                txt_FromDate.Text = DateTime.Now.AddDays(-15).ToString("dd'/'MM'/'yyyy");
                txt_ToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                if (_user != null)
                {

                    LoadCM();
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
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(),"");            
        }
        private void ListImages()
        {
            try
            {
                ImageFilesDAL obj = new ImageFilesDAL();
                DataSet objDataset = new DataSet();

                // Populate the repeater control with the Items DataSet
                PagedDataSource objPds = new PagedDataSource();
                //string where = " UserCreated =" + _user.UserID;
                string where = "";
                if (ddlStock.SelectedValue.Trim() != "0")
                    where += " 1=1 And AuthorID =1 ";
                else
                    where += " 1=1 And (( AuthorID =0 ) OR (AuthorID is null ))";
                if (!string.IsNullOrEmpty(txtTenfile.Text))
                    where += " AND ImageFileName like N'%" + txtTenfile.Text + "%'";
                if (Drop_Chuyenmuc.SelectedValue.Trim() != "0")
                    where += string.Format(" AND Categorys_ID IN (SELECT * FROM [fn_Return_Category_Tree] ({0}))", this.Drop_Chuyenmuc.SelectedValue);
                if (!String.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                    where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)>=0)", UltilFunc.ToDate(this.txt_FromDate.Value.ToString().Trim(), "MM/dd/yyyy"));
                if (!String.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                    where += " AND " + string.Format(" (Datediff(DAY,'{0}',DateCreated)<=0) ", UltilFunc.ToDate(this.txt_ToDate.Value.ToString().Trim(), "MM/dd/yyyy"));
                else
                {
                    if (ddlStock.SelectedValue.Trim() == "0")
                        where += " AND " + string.Format(" DATEDIFF(DAY,DateCreated,'{0}')=0 ", DateTime.Now.ToString("MM/dd/yyyy"));
                }
                if (strNumberArg == "2")
                    where += " AND ImageType <> 1 ";
                where += " ORDER BY DateCreated DESC";
                objDataset = obj.ListAllImages(where);
                if (objDataset != null && objDataset.Tables[0].Rows.Count >0)
                {
                    DataTable _dv = obj.BindGridListImages2Table(objDataset.Tables[0]);
                    objPds.DataSource = _dv.DefaultView;
                    objPds.AllowPaging = true;
                    if (ddlStock.SelectedValue.Trim() != "0")
                        objPds.PageSize = 30;
                    else
                        objPds.PageSize = 12;                   

                    objPds.CurrentPageIndex = CurrentPage;

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " / " + objPds.PageCount.ToString();

                    // Disable Prev or Next buttons if necessary
                    cmdNext.Visible = true;
                    cmdPrev.Visible = true;
                    cmdPrev.Enabled = !objPds.IsFirstPage;
                    cmdNext.Enabled = !objPds.IsLastPage;

                    //dlImages.DataSource = _dv;
                    dlImages.DataSource = objPds;
                    dlImages.DataBind();
                }
                else
                {   
                    lblCurrentPage.Text = "";
                    cmdNext.Visible = false;
                    cmdPrev.Visible = false;
                    dlImages.DataSource = null;
                    dlImages.DataBind();
                }
                objDataset.Dispose();
            }
            catch
            {
                dlImages.DataSource = null;
                dlImages.DataBind();
            }
        }
        public  string GetUserName()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString() + "," + GetvType() + "," + HPCSecurity.CurrentUser.Identity.ID.ToString() + "," + GetChuyenmuc();
            return strTemp;
        }
        public string GetUserID()
        {
            string strTemp = HPCSecurity.CurrentUser.Identity.Name.ToString();
            return strTemp;
        }
        public  string GetvType()
        {
            string strTemp = HttpContext.Current.Request.QueryString["vType"].ToString() ;
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
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID=" + this.Drop_Lang.SelectedValue.ToString() + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
            else
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID in (" + UltilFunc.GetLanguagesByUser(_user.UserID) + ") AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
       
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ViewState["_CurrentPage"] = 0; 
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
                    _Name = _Name.Substring(0, 11)+"...";
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
            ListImages();
        }
        public string GetFileURL(object Url)
        {
            string _url = "";
            try { _url = Url.ToString(); }
            catch { ;}
            if (!string.IsNullOrEmpty(_url))
            {

                _url = Global.UploadPath + _url;
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

        public int GetFileType(string FileExtension)
        {
            Int16 intFileType = 0;
            if (FileExtension.Trim() != null)
            {
                if (FileExtension.ToLower() == ".png" || FileExtension.ToLower() == ".jpeg" || FileExtension.ToLower() == ".jpg" || FileExtension.ToLower() == ".bmp" || FileExtension.ToLower() == ".gif" || FileExtension.ToLower() == ".ico")
                    intFileType = 1;
                else if (FileExtension.ToLower() == ".wmv" || FileExtension.ToLower() == ".swf" || FileExtension.ToLower() == ".flv" || FileExtension.ToLower() == ".mpeg" || FileExtension.ToLower() == ".avi")
                    intFileType = 2;
                else if (FileExtension.ToLower() == "mp3" || FileExtension.ToLower() == "wma")
                    intFileType = 3;
                else if (FileExtension.ToLower() == ".doc" || FileExtension.ToLower() == ".docx" || FileExtension.ToLower() == ".txt" || FileExtension.ToLower() == ".xls" || FileExtension.ToLower() == ".xlsx" || FileExtension.ToLower() == ".pdf")
                    intFileType = 4;
                else
                    intFileType = 5;
            }

            return intFileType;
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
        protected void cmd_watermark_Click(object sender, EventArgs e)
        {
                        
            string url1 = txt_UrlImage.Text.Trim();
            if (!string.IsNullOrEmpty(url1))
            {
                string url = ConfigurationManager.AppSettings["ServerPathDis"].ToString() + url1;
                
                T_ImageFiles _obj = new T_ImageFiles();
                ImageFilesDAL _DAL = new ImageFilesDAL();

                int _instock = Convert.ToInt32(ddlStock.SelectedValue.Trim());
                string strPhysLocal = "";
                if (strNumberArg == "2")
                {
                    if (_instock != 0)
                        strPhysLocal = "/" + "InStockSNS/Thumnail/";
                    else
                        strPhysLocal = "/" + "ArticleSNS/Thumnail/";
                }                   

                if (Global.UseUpload_FTP == 1)
                {

                    string username = ConfigurationManager.AppSettings["Username_FTP"].ToString();
                    string password = ConfigurationManager.AppSettings["Password_FTP"].ToString();
                    string ftpServer = ConfigurationManager.AppSettings["FTP_NameServer"].ToString();

                       
                    byte[] binData = null;

                    url = Global.TinPath_CMS() + url1;
                    System.Drawing.Bitmap sourceImage = getBitmapFromURL(url, username, password);                    

                    string _extension = Path.GetExtension(url);

                    string filename = "WaterMark_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + DateTime.Now.Millisecond.ToString() + Path.GetExtension(url);
                    Bitmap imgsave = null;

                    string strYear = DateTime.Now.ToString("yyyy");
                    string strMonth = DateTime.Now.ToString("MM");
                    string strDay = DateTime.Now.ToString("dd");
                       

                    string _logo = Server.MapPath("../Images/IconHPC/LoGoBaoAnhDong.png");
                    Bitmap Imagemark = new Bitmap(_logo);
                    int spacevalues = 0;
                    try { spacevalues = int.Parse(ConfigurationManager.AppSettings["SpaceValue"].ToString()); }
                    catch { ;}

                    imgsave = HPCImages.WatermarkImages(sourceImage, Imagemark, int.Parse(X11.Value), int.Parse(Y11.Value));

                    MemoryStream ms = new MemoryStream();
                    imgsave.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    binData = ms.ToArray();

                    UploadFTP(ftpServer + strPhysLocal, filename, username, password, binData, _user.UserName, strYear, strMonth, strDay);
                    //phan insert co so du lieu                    
                    string newfolder = strPhysLocal + _user.UserName +"/"+ strYear + "/" + strMonth + "/" + strDay + "/";
                    txt_UrlImage.Text = newfolder + filename;

                    _obj = SetItem(filename, 0, txt_UrlImage.Text, _extension, _user.UserID, Convert.ToInt16(strNumberArg), Convert.ToInt32(Drop_Chuyenmuc.SelectedValue));

                    int _idReturn = _DAL.InsertT_ImageFiles(_obj);
                    if (_instock != 0)
                        _DAL.UpdateStatusDataByID(" AuthorID =1 Where ID =" + _idReturn);
                    ListImages();
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "PreviewImage('" + newfolder + "/" + filename + "','" + strNumberArg + "','0','" + _extension + "');", true);

                }
                else
                {
                    if (File.Exists(url))
                    {

                        System.Drawing.Bitmap sourceImage = new System.Drawing.Bitmap(url);

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

                        imgsave = HPCImages.WatermarkImages(sourceImage, Imagemark, int.Parse(X11.Value), int.Parse(Y11.Value));

                        if (Directory.Exists(pathsave) == false) Directory.CreateDirectory(pathsave);

                        imgsave.Save(pathsave + @"\" + filename);
                        imgsave.Dispose();
                        Imagemark.Dispose();
                        sourceImage.Dispose();

                        txt_UrlImage.Text = newfolder + "/" + filename;


                        _obj = SetItem(filename, 0, txt_UrlImage.Text, _extension, _user.UserID, Convert.ToInt16(strNumberArg), Convert.ToInt32(Drop_Chuyenmuc.SelectedValue));

                        int _idReturn = _DAL.InsertT_ImageFiles(_obj);
                        if (_instock != 0)
                            _DAL.UpdateStatusDataByID(" AuthorID =1 Where ID =" + _idReturn);
                        ListImages();
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "PreviewImage('" + newfolder + "/" + filename + "','" + strNumberArg + "','0','" + _extension + "');", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Hãy chọn ảnh để đóng dấu ');", true);
                    }
                }
            }

           
           
        }

        public static Bitmap getBitmapFromURL(String src, string username, string password)
        {
            Bitmap _source = null;
            FtpWebRequest reqFTP;
            try
            {

                string filename = Path.GetFileName(src);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(src));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(username, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(ftpStream);
                _source = new Bitmap(originalImage);
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _source;
        }
        #region PHAN FTP

        private bool UploadFTP(string DestPath, string LocalFileName, string FtpUser, string FtpPass, byte[] FileInByte, string userName, string strYear, string strMonth, string strDay)
        {
            bool _return = false;
            try
            {
                string _spath = LocalFileName;
                //Dia chi ftp
                string _desdir = DestPath;
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + userName + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + strYear + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + strMonth + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + strDay + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + LocalFileName;
                if (!_ftphelp.ftpFileExist(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.AsynchronousUpload(_desdir, _spath, FtpUser, FtpPass, FileInByte);
                    _return = true;
                }
            }
            catch (Exception ex)
            {

            }
            return _return;
        }

        

        
        #endregion



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
        protected void ddlStock_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStock.SelectedIndex > 0)
            {
                txt_FromDate.Text = "";
                txt_ToDate.Text = "";
                ListImages();
            }
            else
            {
                txt_FromDate.Text = DateTime.Now.AddDays(-1).ToString("dd'/'MM'/'yyyy");
                txt_ToDate.Text = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                ListImages();
            }
        }
    }
}

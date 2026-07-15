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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using prjBusinessLogic;
using prjInfo;
using HPCServerDataAccess;
using prjComponents;
//using Webservice_test;
namespace prjApplication.Until
{
    public partial class DownloadImageFromWebsite : System.Web.UI.Page
    {
        T_Users _user = new T_Users();
        UserDAL _DAL = new UserDAL();

        protected void Page_Load(object sender, EventArgs e)
        { 
            _user = _DAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (_user == null)
            {
                Response.Redirect(Global.ApplicationPath + "/Login.aspx", true);
            }
            if (!this.IsPostBack)
            {
                if (_user != null)
                {
                    LoadCM();
                }
            }
        }

        protected void cmd_getImage_Click(object sender, EventArgs e)
        {
            if (Drop_nguon.SelectedValue == "1")
            {
                //    Webservice_test.WebService_test1 search_service = new prjApplication.Webservice_test.WebService_test1();
                //    string where = " 1 = 1   ";
                //    if (!string.IsNullOrEmpty(fileName.Text))
                //        where += " AND ImageFileName like N'%" + fileName.Text + "%'";
                //    if (!String.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                //        where += " AND " + string.Format(" DateCreated >='{0}'", UltilFunc.ToDate(this.txt_FromDate.Value.ToString().Trim(), "MM/dd/yyyy").ToShortDateString() + " 00:00:00 ");
                //    if (!String.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                //        where += " AND " + string.Format(" DateCreated <='{0}'", UltilFunc.ToDate(this.txt_ToDate.Value.ToString().Trim(), "MM/dd/yyyy").ToShortDateString() + " 23:59:59 ");
                //    where += " ORDER BY DateCreated DESC";
                //    DataTable dt = search_service.Search_Image(where);
                //    DataColumn datacol = new System.Data.DataColumn("FullImage", typeof(string));
                //    DataColumn datacol1 = new System.Data.DataColumn("URLImage", typeof(string));
                //    DataColumn datacol2 = new System.Data.DataColumn("Title", typeof(string));
                //    DataColumn datacol3 = new System.Data.DataColumn("Size", typeof(string));
                //    dt.Columns.Add(datacol);
                //    dt.Columns.Add(datacol1);
                //    dt.Columns.Add(datacol2);
                //    dt.Columns.Add(datacol3);
                //    if (dt != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                dr["URLImage"] = GetFileURL(dr["ImgeFilePath"]);
                //                dr["Size"] = "";
                //                dr["Title"] = "";
                //            }
                //        }
                //    }
                //    dlImages.DataSource = dt;
                //    dlImages.DataBind();
                //}
                //else
                //{
                if (fileWebURL.Text.ToLower().StartsWith("http://"))
                {
                    GetAllImages getImage = new GetAllImages();
                    getImage.URLSite = fileWebURL.Text;
                    DataTable dt = getImage.RetrieveUrls();
                    dlImages.DataSource = dt;
                    dlImages.DataBind();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('hãy nhập dúng địa chỉ trang web : http://abc.xyz!') ;", true);
                }
            }
        }

        protected void cmd_Download_Click(object sender, EventArgs e)
        {
            foreach (DataListItem item in dlImages.Items)
            {
                CheckBox check_download = (CheckBox)item.FindControl("checkbox_download");
                if (check_download.Checked)
                {
                    Label lbl_Warning = (Label)item.FindControl("lbl_Warning");
                    lbl_Warning.Visible = true;
                    lbl_Warning.Text = "Downloaded";
                    check_download.Checked = false;
                    check_download.Visible = false;
                    try
                    {
                        Label lbl_URL = (Label)item.FindControl("lbl_URL");

                        string FolderCat = "/Article/";
                        string savepath = "";
                        string tempPath = ""; string _urlSave = "";
                        string filename = "";
                        string strRootPathVirtual = "";
                        tempPath = FolderCat;
                        strRootPathVirtual = System.Configuration.ConfigurationManager.AppSettings["UploadPath"] + FolderCat + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                        savepath = Server.MapPath(strRootPathVirtual);
                        filename = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                            DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + System.IO.Path.GetFileName(lbl_URL.Text);
                        _urlSave = UrlPathImage_RemoveUpload(strRootPathVirtual + "rez_" + filename);
                        if (!Directory.Exists(savepath))
                            Directory.CreateDirectory(savepath);

                        //download Image to server
                        //DownloadImage download = new DownloadImage(lbl_URL.Text);
                        //download.Download();
                        //Bitmap _Image = download.GetImage();
                        //_Image.Save(savepath + "rez_" + filename, _Image.RawFormat);

                        save_file_from_url(savepath + "rez_" + filename, lbl_URL.Text);
                        FileInfo _file = new FileInfo(savepath + "rez_" + filename);


                        //phan insert co so du lieu
                        T_ImageFiles _obj = new T_ImageFiles();
                        ImageFilesDAL _DAL = new ImageFilesDAL();
                        int cm = 0;
                        try { cm = int.Parse(Drop_Chuyenmuc.SelectedValue); }
                        catch { ;}
                        _obj = SetItem(filename, _file.Length, _urlSave, System.IO.Path.GetExtension(filename), 1, 1, cm);
                        _DAL.InsertT_ImageFiles(_obj);
                    }
                    catch { lbl_Warning.Text = "Downloading Error !"; }
                }
            }
        }

        protected void Drop_Lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Drop_Chuyenmuc.Items.Clear();
            if (Drop_Lang.SelectedIndex >= 0)
            {
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID= " + this.Drop_Lang.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
            }
            else
            {
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", " 1=1 AND Languages_ID in( " + UltilFunc.GetLanguagesByUser(_user.UserID) + ")", "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
            }
        }

        public void LoadCM()
        {
            UltilFunc.BindCombox(Drop_Lang, "Languages_ID", "Languages_Name", "T_Languages", string.Format(" 1=1 AND Languages_ID IN ({0}) Order by Languages_Name ", UltilFunc.GetLanguagesByUser(_user.UserID)), "---Tất cả---");
            if (Drop_Lang.Items.Count == 2)
            {
               Drop_Lang.SelectedIndex = 1; 
            }
            else
                Drop_Lang.SelectedIndex = UltilFunc.GetIndexControl(Drop_Lang, prjComponents.Global.DefaultCombobox);
            if (Drop_Lang.SelectedIndex != 0)
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", string.Format(" 1=1 AND Languages_ID= " + this.Drop_Lang.SelectedValue + " AND Categorys_ID IN ({0})", UltilFunc.GetCategory4User(_user.UserID)), "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");
            else
                UltilFunc.BindCombox(Drop_Chuyenmuc, "Categorys_ID", "Category_Name", "T_Categorys", " 1=1 AND Languages_ID in( " + UltilFunc.GetLanguagesByUser(_user.UserID) + ")", "---Tất cả---", "Category_ParrentID", " Order by Category_Order ASC");

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

        protected T_ImageFiles SetItem(string _tenFile, double _size, string _pathfile, string _extenfile, int _UserID, Int16 vType, double Chuyenmuc)
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
            _obj.Categorys_ID = Chuyenmuc;

            return _obj;
        }

        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }

        public void save_file_from_url(string file_name, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                content = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();

            FileStream fs = new FileStream(file_name, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
        }

        protected void Drop_nguon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Drop_nguon.SelectedIndex == 0)
            {
                panel_website.Visible = true;
                Panel_webservice.Visible = false;
            }
            else
            {
                panel_website.Visible = false;
                Panel_webservice.Visible = true;
            }
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

    }

    public class GetAllImages
    {
        private string _UrlWebsite;

        public string URLSite
        {
            get { return _UrlWebsite; }
            set { _UrlWebsite = value; }
        }
        
        public DataTable RetrieveUrls()
        {
            DataTable dt = new DataTable();
            DataColumn datacol = new System.Data.DataColumn("FullImage", typeof(string));
            DataColumn datacol1 = new System.Data.DataColumn("URLImage", typeof(string));
            DataColumn datacol2 = new System.Data.DataColumn("Title", typeof(string)); 
            DataColumn datacol3 = new System.Data.DataColumn("Size", typeof(string));
            dt.Columns.Add(datacol);
            dt.Columns.Add(datacol1);
            dt.Columns.Add(datacol2);
            dt.Columns.Add(datacol3);
            try
            {
                string content =  RetrieveContent(_UrlWebsite);//"<tr><td align=\"center\"><img alt=\"Tàu tuần tiễu cao tốc TT 400 là tàu cao tốc vỏ thép, đây là dạng tàu có tính năng kỹ thuật cao, trang thiết bị hiện đại. Khả năng tự động hoá, tích hợp các thiết bị trên tàu đồng bộ và chịu được sóng đến cấp 10.\" style=\"border: 0px; Display:yes;\"src=\"http://image.qdnd.vn/ImageHandler/Upload//vanphong/2012/5/8/7795243320120508090006984.jpg\" /></td></tr>";
                //content = 
                GetAllUrls(content, dt);
            }
            catch { ;}
            return dt;
        }

        private string RetrieveContent(string webPage)
        {
            HttpWebResponse response = null;//used to get response
            StreamReader respStream = null;//used to read response into string
            string content = "";
            try
            {
                //create a request object using the url passed in
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webPage);
                request.Timeout = 100000;

                //go get a response from the page
                response = (HttpWebResponse)request.GetResponse();

                //create a streamreader object from the response
                respStream = new StreamReader(response.GetResponseStream());

                //get the contents of the page as a string and return it
                content= respStream.ReadToEnd();
            }
            catch (Exception ex)//houston we have a problem!
            {
                //throw ex;
            }
            finally
            {
                //close it down, we're going home!
                response.Close();
                respStream.Close();
            }
            return content;
        }

        private void GetAllUrls(string content, DataTable dt)
        {
            int i = 1;
            string test = GetRoot(URLSite);
            int startImg = 0;
            int EndImg = 0;
            string[] ArrContent = content.Split('<');
            for (i = 1; i < ArrContent.Length; i++)
            {
                string subcontent = ArrContent[i];
                if (subcontent.Trim().Length > 9)
                {
                    string start_string = subcontent.Substring(0, 3);
                    if (start_string.ToLower() == "img")
                    {
                        EndImg = subcontent.IndexOf(">");
                        string Image = subcontent.Substring(startImg, EndImg);
                        DataRow dr = dt.NewRow();
                        dr["FullImage"] = Image;
                        dr["Title"] = GetTitle(Image);
                        try
                        {
                            int last = Image.IndexOf("src=")+5;
                            int lenght = Image.Length - last;
                            string _src = Image.Substring(last, lenght);
                            int _single = 0, _double = 0;
                            _single = _src.IndexOf('\'');
                            _double = _src.IndexOf("\"");
                            string imageurl = "";
                            if (_single > 0 && _double > 0)
                            {
                                if (_single < _double)
                                    imageurl = _src.Substring(0,_single);
                                else
                                    imageurl = _src.Substring(0, _double);
                            }
                            else if (_single > 0 && _double < 1)
                            {
                                imageurl = _src.Substring(0, _single);
                            }
                            else
                            {
                                imageurl = _src.Substring(0, _double);
                            }
                            if (!imageurl.StartsWith("http://"))
                            {
                                imageurl = test + imageurl;
                            }
                            dr["URLImage"] = imageurl;
                            string size = "";
                            try
                            {
                                size = GetSize(imageurl);

                            }
                            catch { ;}
                            if (size != "0")
                            {
                                dr["Size"] = size;
                                dt.Rows.Add(dr);
                            }
                        }
                        catch { ;}
                    }
                }
                
            }
        }

        public string GetRoot(string Url)
        {
            string root = ""; 
            Url = Url.ToLower();
            if (Url.ToLower().StartsWith("http://"))
            {
                string child = Url.Substring(Url.LastIndexOf("http://")+7, Url.Length - Url.LastIndexOf("http://")-7);
                string []temp = child.Split('/');
                root = temp[0] ;
            }
            return "http://"+root;
        }

        public string GetTitle(string str_image)
        {
            string Title = "";
            int last = str_image.LastIndexOf("title=\"");
            if (last > 0)
            {
                int lenght = str_image.Length - str_image.LastIndexOf("title=\"") - 7;
                string _tile = str_image.Substring(last + 7, lenght);
                Title = _tile.Split('\"')[0];
            }
            return Title;
        }

        string GetSize(string Url)
        {
            DownloadImage download = new DownloadImage(Url);
            download.Download();
            string size = "";
            Bitmap image = download.GetImage();
            size = image.Size.Width.ToString()+ "x" + image.Size.Height.ToString() ;
            int minwith = 0, minheight = 0;
            try { minwith = int.Parse(ConfigurationManager.AppSettings["Min_With"].ToString());
            minheight = int.Parse(ConfigurationManager.AppSettings["Min_height"].ToString());
            }
            catch { ;}
            if (image.Size.Width < minwith && image.Size.Height < minheight)
                size = "0";
            return size;
        }

    }

    public class DownloadImage
    {
        private string imageUrl;

        private Bitmap bitmap;

        public DownloadImage(string imageUrl)
        {
            this.imageUrl = imageUrl;
        }

        public void Download()
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(imageUrl);
                bitmap = new Bitmap(stream);
                stream.Flush();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Bitmap GetImage()
        {
            return bitmap;
        }

        public void SaveImage(string filename, ImageFormat format)
        {
            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }
        }
    }

}

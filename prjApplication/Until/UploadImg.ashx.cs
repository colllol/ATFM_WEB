using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using prjBusinessLogic;
using prjInfo;
using System.IO;
using System.Configuration;
using HPCServerDataAccess;
using prjComponents;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace prjApplication.Until
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class UploadImg : IHttpHandler
    {
        T_Users _user = new T_Users();
        UserDAL _userDAL = new UserDAL();
        FTPClass _ftphelp = new FTPClass();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                HttpPostedFile postedFile = context.Request.Files["Filedata"];

                int inStock = 0;
                try
                {
                    inStock = int.Parse(context.Request.QueryString["inStock"].ToString().Trim());
                }
                catch { ;}

                string[] sArrProdID = null;
                char[] sep = { '?' };
                string[] sArrVkey = null;
                string strUserID = "";
                char[] sep2 = { ',' };
                sArrProdID = context.Request.QueryString["user"].ToString().Trim().Split(sep);
                sArrVkey = sArrProdID[0].ToString().Trim().Split(sep2);
                string chuyenmuc = sArrVkey[sArrVkey.Length - 1];
                string _urlSave = "";
                string FolderCat = "";
                string savepath = "";
                string tempPath = "";
                string strRootPathVirtual = "";

                string filename = "";
                string _extenfile = "";
                Int16 intFileType = 0;
                Bitmap imgOut = null;

                T_ImageFiles _obj = new T_ImageFiles();
                ImageFilesDAL _DAL = new ImageFilesDAL();


                _user = _userDAL.GetUserByUserName(sArrVkey[0].ToString());
                strUserID = _user.UserID.ToString();
                string vType = sArrVkey[1].ToString();

                if (vType == "1")
                    FolderCat = "/IconsSNS/";
                else if (vType == "2")
                {
                    if (inStock != 0)
                        FolderCat = "/InStockSNS/";
                    else
                        FolderCat = "/ArticleSNS/";
                }
                else if (vType == "3")
                    FolderCat = "/AdsSNS/";
                else if (vType == "4")
                    FolderCat = "/VideoSNS/";
                else if (vType == "5")
                    FolderCat = "/Photo24SNS/";
                else if (vType == "6")
                    FolderCat = "/InStockSNS/";
                else
                    FolderCat = "";

                if (Global.UseUpload_FTP == 1)
                {
                     string username = ConfigurationManager.AppSettings["Username_FTP"].ToString();
                    string password = ConfigurationManager.AppSettings["Password_FTP"].ToString();
                    string ftpServer = ConfigurationManager.AppSettings["FTP_NameServer"].ToString();

                    filename = postedFile.FileName;
                    BinaryReader b = new BinaryReader(postedFile.InputStream);
                    byte[] binData = b.ReadBytes(postedFile.ContentLength);
                                      
                       
                    Image obj_InImage = null;
                    MemoryStream msroot = new MemoryStream(binData);
                    obj_InImage = Image.FromStream(msroot);
                    imgOut = new Bitmap(obj_InImage);

                    try { intFileType = getFileType(_extenfile); }
                    catch { ;}
                    int CATID = 0;
                    try { CATID = Int32.Parse(chuyenmuc); }
                    catch { ;}

                    string extenfile = Path.GetExtension(filename.ToString().Trim()).Replace(".", "");
                    string tenfile = UltilFunc.ReplaceCharsRewrite(Path.GetFileNameWithoutExtension(filename));
                    string tenfilegoc = tenfile + "." + extenfile.ToString();
                    string strYear = DateTime.Now.ToString("yyyy");
                    string strMonth = DateTime.Now.ToString("MM");
                    string strDay = DateTime.Now.ToString("dd");

                    string strmi = DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond.ToString();
                    int milisecom = 0; int.TryParse(strmi, out milisecom);
                    string strunique = milisecom.ToString("X");
                    string fileSave = tenfile.Replace(" ", "-") + strunique + "." + extenfile.ToString();
                    string fileURL = FolderCat + _user.UserName + "/" + strYear + "/" + strMonth + "/" + strDay + "/" + fileSave;

                    bool checkresize = false;

                    if (_extenfile.ToLower() != ".flv" && _extenfile.ToLower() != ".swf" && _extenfile.ToLower() != ".mp3" && _extenfile.ToLower() != ".mp4" && _extenfile.ToLower() != ".wmv" && _extenfile.ToLower() != ".doc" && _extenfile.ToLower() != ".docx" && _extenfile.ToLower() != ".xls" && _extenfile.ToLower() != ".rar" && _extenfile.ToLower() != ".txt" && _extenfile.ToLower() != ".pdf")
                    {
                        checkresize = true;
                    }

                    if (IsImage(postedFile))
                    {

                        MemoryStream ms = new MemoryStream();
                        imgOut.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                        binData = ms.ToArray();

                        string _paramWH = prjComponents.Global.VNPResizeImagesContent + "x" + prjComponents.Global.VNPResizeImagesContent;
                        if (checkresize)
                        {
                            binData = ResizeImage(_paramWH, binData);
                        }

                        UploadFTP(ftpServer + FolderCat, fileSave, username, password, binData, _user.UserName, strYear, strMonth, strDay);
                        //phan insert co so du lieu   
                        _obj = SetItem(fileSave, postedFile.ContentLength, fileURL, _extenfile, Convert.ToInt16(strUserID), Convert.ToInt16(vType), CATID, inStock);                        
                        int _idReturn = _DAL.InsertT_ImageFiles(_obj);
                    }
                    else
                    {
                        UploadFTP(ftpServer + FolderCat, fileSave, username, password, binData, _user.UserName, strYear, strMonth, strDay);
                        //phan insert co so du lieu                    
                        _obj = SetItem(fileSave, postedFile.ContentLength, fileURL, _extenfile, Convert.ToInt16(strUserID), Convert.ToInt16(vType), CATID, inStock); 
                        int _idReturn = _DAL.InsertT_ImageFiles(_obj);
                    }
                    context.Response.Write(fileURL);
                    context.Response.StatusCode = 200;
                }
                else
                {

                    tempPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"] + FolderCat + sArrVkey[0].ToString() + "/";
                    strRootPathVirtual = tempPath + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                    savepath = context.Server.MapPath(strRootPathVirtual);

                    filename = getFileNameUnique(savepath + @"\", postedFile.FileName, Path.GetFileNameWithoutExtension(postedFile.FileName), Path.GetExtension(postedFile.FileName.ToString()));
                    _extenfile = Path.GetExtension(filename.ToString());
                    string strFileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName).Trim();
                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);
                    if (_extenfile.ToLower().Contains(".jpg")
                       || _extenfile.ToLower().Contains(".gif")
                       || _extenfile.ToLower().Contains(".png")
                       || _extenfile.ToLower().Contains(".bmp")
                       || _extenfile.ToLower().Contains(".ico")
                       || _extenfile.ToLower().Contains(".jpeg"))
                        postedFile.SaveAs(savepath + @"\" + filename);


                    double CATID = 0;
                    try { CATID = double.Parse(chuyenmuc); }
                    catch { ;}
                    string _logo = context.Server.MapPath("../Images/IconHPC/LoGoBaoNongNghiep.png");

                    string _imagesEndWatermark = getFileNameUnique(savepath + @"\", postedFile.FileName, Path.GetFileNameWithoutExtension(filename), _extenfile);
                    if (_extenfile.ToLower() != ".flv" && _extenfile.ToLower() != ".swf" && _extenfile.ToLower() != ".mp3" && _extenfile.ToLower() != ".mp4" && _extenfile.ToLower() != ".wmv" && _extenfile.ToLower() != ".doc" && _extenfile.ToLower() != ".docx" && _extenfile.ToLower() != ".xls" && _extenfile.ToLower() != ".rar" && _extenfile.ToLower() != ".txt" && _extenfile.ToLower() != ".pdf")
                    {
                        if (Convert.ToBoolean(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WatermarkImages"])))
                        {
                            // Begin BO CT EDIT Đóng dấu ảnh
                            _imagesEndWatermark = "W_" + filename;
                            HPCImageResize.SaveImage2Server(savepath, filename, "rez_" + filename, _imagesEndWatermark, _logo, Convert.ToInt32(prjComponents.Global.VNPResizeImages), Convert.ToInt32(prjComponents.Global.VNPResizeImages));
                        }
                        else if (Convert.ToBoolean(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoProcessReszie"])))// Không đóng dấu ảnh //END
                        {
                            HPCImageResize.SaveImage2Server(savepath, filename, _imagesEndWatermark, Convert.ToInt32(prjComponents.Global.VNPResizeImages), Convert.ToInt32(prjComponents.Global.VNPResizeImages));
                        }
                    }
                    else
                    {
                        postedFile.SaveAs(Path.Combine(savepath, _imagesEndWatermark));
                    }
                    _urlSave = UrlPathImage_RemoveUpload(strRootPathVirtual + _imagesEndWatermark);

                    //phan insert co so du lieu

                    _obj = SetItem(_imagesEndWatermark, postedFile.ContentLength, _urlSave, _extenfile, Convert.ToInt16(strUserID), Convert.ToInt16(vType), CATID, inStock);

                    int _idReturn = _DAL.InsertT_ImageFiles(_obj);
                    //if (inStock != 0)
                    //    _DAL.UpdateStatusDataByID(" AuthorID =1 Where ID =" + _idReturn);
                    context.Response.Write(savepath + "/" + _imagesEndWatermark);
                    context.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
            }

        }

        protected string getFileNameUnique(string ImageDirectory, string strFileName, string fileNameWithoutExtension,string FileExtension)
        {
            int count = 1;
            string strFileNameUnique = UltilFunc.ReplaceCharsRewrite(fileNameWithoutExtension) + FileExtension;
            string ImagePath = ImageDirectory + strFileNameUnique;
            while (System.IO.File.Exists(ImagePath))
            {

                ImagePath = string.Concat(ImageDirectory, UltilFunc.ReplaceCharsRewrite(fileNameWithoutExtension), "-", count.ToString(), FileExtension);
                strFileNameUnique = UltilFunc.ReplaceCharsRewrite(fileNameWithoutExtension) + "-" + count.ToString() + FileExtension;
                count++;

            }

            return strFileNameUnique;
        }



        protected T_ImageFiles SetItem(string _tenFile, double _size, string _pathfile, string _extenfile, int _UserID, Int16 vType, double chuyenmuc, int _instock)
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
            _obj.AuthorID = _instock;
            return _obj;
        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
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

        public bool IsImage(HttpPostedFile filePosted)
        {
            bool isImage = false;
            try
            {
                using (Bitmap img = new Bitmap(filePosted.InputStream))
                {
                    if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg)
                        || img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp)
                        || img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png)
                        || img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif)
                        || img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                    {
                        isImage = true;
                    }
                }
            }
            catch
            { }
            return isImage;
        }

        public byte[] ResizeImage(string Resize, byte[] imgPhoto)
        {
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(imgPhoto);
            Bitmap sourceimage = new Bitmap(img);
            HPCImages objResize = new HPCImages();
            Bitmap imageResize = null;
            string[] wh = Resize.Split('x');
            int width = 0, height = 0;
            if (wh.Length == 2)
            {
                int.TryParse(wh[0], out width);
                int.TryParse(wh[1], out height);
            }
            if (width > 0 && height > 0)
            {
                imageResize = GetResizedImage(sourceimage, width, height);
                if (imageResize != null)
                {
                    MemoryStream ms = new MemoryStream();
                    imageResize.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
                return imgPhoto;
            }
            else
            {
                return imgPhoto;
            }
        }

        public Bitmap GetResizedImage(Bitmap source, int maxWidth, int maxHeight)
        {
            int width = 0, height = 0;
            float aspectRatio = (float)source.Width / (float)source.Height;

            if ((maxHeight > 0) && (maxWidth > 0))
            {
                if ((source.Width < maxWidth) && (source.Height < maxHeight))
                {
                    return source;
                }
                else if (aspectRatio > 1)
                {
                    width = maxWidth;
                    height = (int)(width / aspectRatio);
                    if (height > maxHeight)
                    {
                        height = maxHeight;
                        width = (int)(height * aspectRatio);
                    }
                }
                else
                {
                    height = maxHeight;
                    width = (int)(height * aspectRatio);
                    if (width > maxWidth)
                    {
                        width = maxWidth;
                        height = (int)(width / aspectRatio);
                    }
                }
            }
            else if ((maxHeight == 0) && (maxWidth > 0))
            {
                width = maxWidth;
                height = (int)(width / aspectRatio);
            }
            else if ((maxWidth == 0) && (maxHeight > 0))
            {
                height = maxHeight;
                width = (int)(height * aspectRatio);
            }
            else if ((maxWidth == 0) && (maxHeight == 0))
            {
                return source;
            }
            //This function creates the thumbnail image.
            //The logic is to create a blank image and to 
            // draw the source image onto it
            Bitmap thumb = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(thumb);
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            Rectangle rectDestination = new Rectangle(0, 0, width, height);
            gr.DrawImage(source, rectDestination, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
            gr.Dispose();
            return thumb;
        }
        #endregion

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
                    intFileType = 5;
            }

            return intFileType;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

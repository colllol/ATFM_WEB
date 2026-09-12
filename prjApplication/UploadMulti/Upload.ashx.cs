using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using prjBusinessLogic;
using prjInfo;
using System.IO;
using System.Configuration;
using HPCServerDataAccess;
using System.Drawing;
using prjComponents;

namespace prjApplication.UploadMulti
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Upload1 : IHttpHandler
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
                T_ImageFiles _obj = new T_ImageFiles();
                ImageFilesDAL _DAL = new ImageFilesDAL();

                HttpPostedFile postedFile = context.Request.Files["Filedata"];

                
                int spacevalue = 0;
                try { spacevalue = int.Parse(ConfigurationManager.AppSettings["SpaceValue"].ToString()); }
                catch { ;}
                string _logo = context.Server.MapPath("../Images/IconHPC/LoGoBaoNongNghiep.png");

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

                _user = _userDAL.GetUserByUserName(sArrVkey[0].ToString());
                strUserID = _user.UserID.ToString();
                string vType = sArrVkey[1].ToString();
                if (vType == "1")
                    FolderCat = "/IconsSNS/";
                else if (vType == "2")
                    FolderCat = "/ArticleSNS/";
                else if (vType == "3")
                    FolderCat = "/AdsSNS/";
                else if (vType == "4")
                    FolderCat = "/VideoSNS/";
                else if (vType == "5")
                    FolderCat = "/Photo24SNS/";
                else if (vType == "9")
                    FolderCat = "/ProductSNS/";
                else
                    FolderCat = "";

                if (Global.UseUpload_FTP == 1)
                {
                    string username = ConfigurationManager.AppSettings["Username_FTP"].ToString();
                    string password = ConfigurationManager.AppSettings["Password_FTP"].ToString();
                    string FtpServer = ConfigurationManager.AppSettings["FTP_NameServer"].ToString();

                    BinaryReader b = new BinaryReader(postedFile.InputStream);
                    byte[] binData = b.ReadBytes(postedFile.ContentLength);

                    string _DateTime = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                    filename = postedFile.FileName;
                    _extenfile = Path.GetExtension(filename.ToString().Trim()).Replace(".", "");

                   
                    try { intFileType = getFileType(_extenfile); }
                    catch { ;}
                    int CATID = 0;
                    try { CATID = Int32.Parse(chuyenmuc); }
                    catch { ;}

                    string _tenfile = UltilFunc.ReplaceCharsRewrite(Path.GetFileNameWithoutExtension(filename));
                    string _tenfilegoc = _tenfile + "." + _extenfile.ToString();

                    string strmi = DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond.ToString();
                    int milisecom = 0; int.TryParse(strmi, out milisecom);
                    string strunique = milisecom.ToString("X");
                    string fileSave = _tenfile.Replace(" ", "-") + strunique + "." + _extenfile.ToString();

                    _urlSave = FolderCat + "" + _user.UserName + "/" + _DateTime + "" + fileSave;

                    UploadFTP(FtpServer + FolderCat, _user.UserName, fileSave, username, password, binData);

                    //phan insert co so du lieu
                    _obj = SetItem(fileSave, postedFile.ContentLength, _urlSave, _extenfile, Convert.ToInt16(strUserID), Convert.ToInt16(vType), 0, intFileType);
                    _DAL.InsertT_ImageFiles(_obj);                    
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

                   
                    try { intFileType = getFileType(_extenfile); }
                    catch { ;}
                    int CATID = 0;
                    try { CATID = Int32.Parse(chuyenmuc); }
                    catch { ;}


                    _urlSave = UrlPathImage_RemoveUpload(strRootPathVirtual + filename);
                    if (_extenfile.ToLower() == ".flv" || _extenfile.ToLower() == ".swf" || _extenfile.ToLower() == ".mp3" || _extenfile.ToLower() == ".mp4" || _extenfile.ToLower() == ".wmv")
                    {
                        postedFile.SaveAs(Path.Combine(savepath, filename));

                    }
                    //phan insert co so du lieu
                    _obj = SetItem(filename, postedFile.ContentLength, _urlSave, _extenfile, Convert.ToInt16(strUserID), Convert.ToInt16(vType), 0, intFileType);
                    _DAL.InsertT_ImageFiles(_obj);
                    context.Response.Write(savepath + "/" + filename);
                    context.Response.StatusCode = 200;                    
                }                            
              

            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
            }

        }

        protected string getFileNameUnique(string ImageDirectory, string strFileName, string fileNameWithoutExtension, string FileExtension)
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



        protected T_ImageFiles SetItem(string _tenFile, double _size, string _pathfile, string _extenfile, int _UserID, Int16 vType, double chuyenmuc, Int16 intFileType)
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
            _obj.FileType = intFileType;            
            _obj.AuthorID = 0;
            return _obj;
        }
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }

        private bool UploadFTP(string DestPath, string _userName, string LocalFileName, string FtpUser, string FtpPass, byte[] FileInByte)
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
                _desdir += "/" + _userName.ToString()+ "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + DateTime.Now.Year.ToString() + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + DateTime.Now.Month.ToString() + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + DateTime.Now.Day.ToString() + "/";
                if (!_ftphelp.FtpDirectoryExists(_desdir, FtpUser, FtpPass))
                {
                    _ftphelp.CreateFTPDirectory(_desdir, FtpUser, FtpPass);
                }
                _desdir += "/" + LocalFileName;
                bool _exist = _ftphelp.ftpFileExist(_desdir, FtpUser, FtpPass);
                if (_exist == false)
                {
                    _ftphelp.AsynchronousUpload(_desdir, _spath, FtpUser, FtpPass, FileInByte);
                    _return = true;
                }

            }
            catch { }
            return _return;
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

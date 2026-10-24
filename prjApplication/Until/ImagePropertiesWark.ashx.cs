using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjApplication.Service;
using System.Web.Services;
using prjInfo;
using prjBusinessLogic;
using prjComponents;
using HPCServerDataAccess;

namespace prjApplication.Until
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImagePropertiesWark : IHttpHandler
    {
        T_Users _user = new T_Users();
        UserDAL _userDAL = new UserDAL();
        public void ProcessRequest(HttpContext context)
        {
            
            try
            {
                string _pathGoc = context.Request["img"].ToString();
                string _replace = "http://" + HttpContext.Current.Request.Url.Host + "/upload/";
                string url1 = _pathGoc.Replace(_replace, "");
                
                string strNumberArg = context.Request["vType"].ToString();
                string _vpos = context.Request["vposX"].ToString();
                string _vposY = context.Request["vposY"].ToString();
                _user = _userDAL.GetUserByUserName(context.Request["user"].ToString());
                string url = System.Configuration.ConfigurationManager.AppSettings["ServerPathDis"].ToString() + url1;
                if (System.IO.File.Exists(url))
                {
                    string strPhysLocal = "";
                    if (strNumberArg == "1")
                        strPhysLocal = "/" + "Article/Thumnail/";
                    if (strNumberArg == "2")
                        strPhysLocal = "/" + Global.UploadPhotoAlbum + "/";
                    if (strNumberArg == "3")
                        strPhysLocal = "/" + Global.UploadPhotoEvent + "/";

                    
                    System.Drawing.Bitmap sourceImage = new System.Drawing.Bitmap(url);
                    
                    string _extension = System.IO.Path.GetExtension(url);

                    string filename = "WaterMark_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + DateTime.Now.Millisecond.ToString() + System.IO.Path.GetExtension(url);
                    System.Drawing.Bitmap imgsave = null;
                    string newfolder = strPhysLocal + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                    string pathsave = System.Configuration.ConfigurationManager.AppSettings["ServerPathDis"].ToString() + newfolder;
                    string _logo = context.Server.MapPath("../Images/IconHPC/LoGoBaoAnhDong.png");
                    System.Drawing.Bitmap Imagemark = new System.Drawing.Bitmap(_logo);
                    int spacevalues = 0;
                    try { spacevalues = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpaceValue"].ToString()); }
                    catch { ;}

                    //imgsave = HPCServerDataAccess.HPCImages.WatermarkImages(sourceImage, Imagemark, int.Parse(_vpos.ToString()), spacevalues);
                    double _X11 = Double.Parse(_vpos.ToString());
                    _X11 = Math.Round(_X11);
                    double _Y11 = Double.Parse(_vposY.ToString());
                    _Y11 = Math.Round(_Y11);
                    int _vposX11 = Convert.ToInt32(_X11.ToString());
                    int _vposY11 = Convert.ToInt32(_Y11.ToString());
                    imgsave = HPCImages.WatermarkImages(sourceImage, Imagemark, _vposX11, _vposY11);

                    if (System.IO.Directory.Exists(pathsave) == false) System.IO.Directory.CreateDirectory(pathsave);

                    imgsave.Save(pathsave + @"\" + filename);
                    imgsave.Dispose();
                    Imagemark.Dispose();
                    sourceImage.Dispose();

                    string _pathNew = newfolder + "/" + filename;

                    T_ImageFiles _obj = new T_ImageFiles();
                    ImageFilesDAL _DAL = new ImageFilesDAL();
                    _obj = SetItem(filename, 0, _pathNew, _extension, _user.UserID, Convert.ToInt16(strNumberArg), 1);
                    _DAL.InsertT_ImageFiles(_obj);

                    context.Response.Write(_pathNew);
                    context.Response.StatusCode = 200;

                }
            }
            catch { context.Response.End(); }
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

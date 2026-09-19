using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Drawing.Drawing2D;
using SD = System.Drawing;
using System.IO;
using prjInfo;
using prjBusinessLogic;
namespace prjApplication.Until
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CropImage : IHttpHandler
    {
        T_Users _user = new T_Users();
        UserDAL _userDAL = new UserDAL();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int w = Convert.ToInt32(context.Request["w"]);
                int h = Convert.ToInt32(context.Request["h"]);
                int x = Convert.ToInt32(context.Request["x"]);
                int y = Convert.ToInt32(context.Request["y"]);
                string _imageUrl = context.Request["img"];
                string strUserID = "";
                _user = _userDAL.GetUserByUserName(context.Request["user"].ToString());
                strUserID = _user.UserID.ToString();
                string vType = context.Request["vType"];
                string ImageName = string.Empty;
                string PathUrlImage = "";
                string PathUrlDest = "";
               
                string RootPath = ""; 
                _imageUrl=_imageUrl.Replace("http://" + HttpContext.Current.Request.Url.Host,"");
                
                for (int i = 0; i < _imageUrl.Split('/').Length; i++)
                {
                    if (_imageUrl.Split('/')[i].ToString().Trim() != "" && i < _imageUrl.Split('/').Length - 1)
                    {
                        if (PathUrlImage =="")
                            PathUrlImage = "/" +_imageUrl.Split('/')[i].ToString();
                        else
                            PathUrlImage = PathUrlImage + "/" + _imageUrl.Split('/')[i].ToString();
                    }
                    else
                        ImageName = _imageUrl.Split('/')[i].ToString();

                }
                _imageUrl = RootPath + _imageUrl;
                RootPath = RootPath + PathUrlImage;
                ImageName = w.ToString() + "x" + h.ToString() + "_" + ImageName.Trim();
                PathUrlDest = RootPath + "/" + ImageName;                
                string PhysicalPathDest = context.Server.MapPath(PathUrlDest);

                PathUrlDest = "/" + PathUrlDest.Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"], "");


                _imageUrl = context.Server.MapPath(_imageUrl);
                
                HPCImageCrop.Crop(_imageUrl, PhysicalPathDest, w, h, x, y);                


                //phan insert co so du lieu
                T_ImageFiles _obj = new T_ImageFiles();
                ImageFilesDAL _DAL = new ImageFilesDAL();
                double fileSizeTotal = new FileInfo(PhysicalPathDest).Length;
                _obj = SetItem(ImageName, fileSizeTotal, PathUrlDest, Path.GetExtension(ImageName), Convert.ToInt16(strUserID), Convert.ToInt16(vType), 0);
                _DAL.InsertT_ImageFiles(_obj);


                context.Response.Write(PathUrlDest);
                context.Response.StatusCode = 200;
            }
            catch (Exception e)
            { 
                context.Response.End(); 
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}

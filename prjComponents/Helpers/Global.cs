using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Collections;
namespace prjComponents
{
    public class Global : System.Web.HttpApplication
    {
        public static string GetAppPath(HttpRequest request)
        {
            string path = string.Empty;
            try
            {
                if (request.ApplicationPath != "/")
                {
                    path = request.ApplicationPath + "/";
                }
                else
                {
                    path = request.ApplicationPath;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return path;
        }
        
        public static string ApplicationPath = ConfigurationManager.AppSettings["ApplicationPath"];
        public static string MultimediaPath = ConfigurationManager.AppSettings["MultimediaPath"];
        public static string VNPResizeImages = ConfigurationManager.AppSettings["VNPResizeImages"];
        public static string VNPResizeImagesContent = ConfigurationManager.AppSettings["VNPResizeContent"];
        public static string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
        public static string UploadPhotoAlbum = ConfigurationManager.AppSettings["UploadPhotoAlbum"];
        public static string DefaultCombobox = ConfigurationManager.AppSettings["DefaultCombobox"];
        public static int DefaultLangID =Convert.ToInt32(ConfigurationManager.AppSettings["DefaultLangID"].ToString());
        public static int DefaultSourceRss = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultSourceRss"].ToString());
        public static string UploadPhotoEvent = ConfigurationManager.AppSettings["UploadPhotoEvent"];
        public static string TinPath = ConfigurationManager.AppSettings["tinpath"];
        public static string TinPathFTP = ConfigurationManager.AppSettings["tinpathFTP"];
        public static int PublishID = Convert.ToInt32(ConfigurationManager.AppSettings["PublishingID"]);
        public static int ListID_News = Convert.ToInt32(ConfigurationManager.AppSettings["ListNewsID"]);
        public static int ListID_ChoXB = Convert.ToInt32(ConfigurationManager.AppSettings["ListNewsIDChoXB"]);
        public static string TagHtmlNotRemove = ConfigurationManager.AppSettings["TagHtmlNotRemove"].ToString();
        public static int UseUpload_FTP = Convert.ToInt32(ConfigurationManager.AppSettings["UseUpload_FTP"]);

        #region ADD BY NVTHAI
        public static string TinPath_CMS()
        {
            string _url = "";
            if (UseUpload_FTP == 1)
                _url = TinPathFTP;
            else
                _url = TinPath + UploadPath;
            return _url;
        }
        public static string TinPath_Content()
        {
            string _url = "";
            if (UseUpload_FTP == 1)
                _url = TinPathFTP;
            else
                _url = UploadPath;
            return _url;
        }
        #endregion

        #region BEGIN SYNC
        public static ArrayList Path_Service
        {
            get
            {
                ArrayList _arr = new ArrayList();

                string _str = System.Configuration.ConfigurationSettings.AppSettings["PuDataService"];
                char[] ch = { ';' };
                string[] arr = _str.Split(ch);
                for (int i = 0; i < arr.Length; i++)
                {
                    _arr.Add(arr[i]);
                }
                return _arr;
            }
        }
        public static ArrayList ImagesService
        {
            get
            {
                ArrayList _arr = new ArrayList();

                string _str = System.Configuration.ConfigurationSettings.AppSettings["ImagesService"];
                char[] ch = { ';' };
                string[] arr = _str.Split(ch);
                for (int i = 0; i < arr.Length; i++)
                {
                    _arr.Add(arr[i]);
                }
                return _arr;
            }
        }
        public static ArrayList Path_Replate
        {
            get
            {
                ArrayList _arr = new ArrayList();

                string _str = System.Configuration.ConfigurationSettings.AppSettings["Path_Replate"];
                char[] ch = { ';' };
                string[] arr = _str.Split(ch);
                for (int i = 0; i < arr.Length; i++)
                {
                    _arr.Add(arr[i]);
                }
                return _arr;

            }
        }
        #endregion END
        public static ResourceManager RM = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        public static int MembersPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["MembersPerPage"]);
        public static int MPerPageLienQuan = Convert.ToInt32(ConfigurationManager.AppSettings["MPerPageLienQuan"]);
        //public static string GetStatusT_NewsFrom_T_version(Object ID)
        //{
        //    string str = "";
        //    int status = 0;
        //    prjBusinessLogic.DAL.T_NewsDAL Dal = new prjBusinessLogic.DAL.T_NewsDAL();
        //    if (Dal.load_T_news(Convert.ToInt32(ID)) == null)
        //        status = 0;
        //    else
        //        status = (int)Dal.load_T_news(Convert.ToInt32(ID)).News_Status;
        //    switch (status)
        //    {
        //        case 12:
        //            str = Global.RM.GetString("System_StatusDocumentsALL1");//Bài đang xử lý tại PV
        //            break;
        //        case 13:
        //            str = Global.RM.GetString("System_StatusDocumentsALL2");//Trả lại bài cho PV
        //            break;
        //        case 22:
        //            str = Global.RM.GetString("System_StatusDocumentsALL3");//TKTS đang xử lý
        //            break;

        //        case 23:
        //            str = Global.RM.GetString("System_StatusDocumentsALL4");//Trả lại bài cho TKTS
        //            break;

        //        case 32:
        //            str = Global.RM.GetString("System_StatusDocumentsALL5");//"Họa sĩ trình bày đang xử lý"; ;
        //            break;
        //        case 33:
        //            str = Global.RM.GetString("System_StatusDocumentsALL6");//"Trả lại bài cho Họa sĩ trình bầy"; ;
        //            break;

        //        case 42:
        //            str = Global.RM.GetString("System_StatusDocumentsALL7");//"Biên dịch ngữ đang xử lý"; ;
        //            break;
        //        case 43:
        //            str = Global.RM.GetString("System_StatusDocumentsALL8");//"Trả lại biên dịch ngữ"; ;
        //            break;

        //        case 52:
        //            str = Global.RM.GetString("System_StatusDocumentsALL9");//"Hiệu đính đang xử lý"; ;
        //            break;
        //        case 53:
        //            str = Global.RM.GetString("System_StatusDocumentsALL10");//"Trả lại bài cho hiệu đính "; ;
        //            break;
        //        case 55:
        //            str = Global.RM.GetString("System_StatusDocumentsALL11");//"Bài đã bị xóa "; ;
        //            break;

        //        case 62:
        //            str = Global.RM.GetString("System_StatusDocumentsALL12");//"Hoàn chỉnh ngữ đang xử lý "; ;
        //            break;
        //        case 63:
        //            str = Global.RM.GetString("System_StatusDocumentsALL13");//"Trả lại bài cho Hoàn chỉnh ngữ "; ;
        //            break;

        //        case 72:
        //            str = Global.RM.GetString("System_StatusDocumentsALL14");//"DXB đang xử lý "; ;
        //            break;
        //        case 73:
        //            str = Global.RM.GetString("System_StatusDocumentsALL15");//"Trả lại bài cho DXB "; ;
        //            break;

        //        case 82:
        //            str = Global.RM.GetString("System_StatusDocumentsALL16");//"Chờ DXB đang xử lý "; ;
        //            break;
        //        case 92:
        //            str = Global.RM.GetString("System_StatusDocumentsALL70");//"TBT đang xử lý "; ;
        //            break;
        //        case 83:
        //            str = Global.RM.GetString("System_StatusDocumentsALL71");//"TBT đang xử lý "; ;
        //            break;
        //        case 6:
        //            str = Global.RM.GetString("System_StatusDocumentsALL17");//" Bài đã được xuất bản ";
        //            break;
        //        case 4:
        //            str = "Bài ngừng đăng";//" Bài đã được xuất bản ";
        //            break;
        //        default:
        //            str = "";
        //            break;
        //    }
        //    return str;
        //}

        public static string GetActionName(Object ID)
        {
            string str = "";
            switch (Convert.ToInt32(ID.ToString()))
            {
                case 12:
                    str = Global.RM.GetString("System_StatusDocumentsALL20"); //"Bài đang xử lý tại PV"; ;
                    break;
                case 13:
                    str = Global.RM.GetString("System_StatusDocumentsALL21");//" Gửi trả lại bài cho PV"; ;
                    break;
                case 22:
                    str = Global.RM.GetString("System_StatusDocumentsALL22");//"Gửi TKTS"; ;
                    break;

                case 23:
                    str = Global.RM.GetString("System_StatusDocumentsALL23");//"Gửi trả lại bài cho TKTS"; ;
                    break;

                case 32:
                    str = Global.RM.GetString("System_StatusDocumentsALL24");//"Gửi Họa sĩ trình bày"; ;
                    break;
                case 33:
                    str = Global.RM.GetString("System_StatusDocumentsALL25");//"Gửi trả lại bài cho Họa sĩ trình bầy"; ;
                    break;

                case 42:
                    str = Global.RM.GetString("System_StatusDocumentsALL26");//"Gửi Biên dịch ngữ"; ;
                    break;
                case 43:
                    str = Global.RM.GetString("System_StatusDocumentsALL27");//"Gửi trả lại biên dịch ngữ"; ;
                    break;

                case 52:
                    str = Global.RM.GetString("System_StatusDocumentsALL28");//"Gửi Hiệu đính"; ;
                    break;
                case 53:
                    str = Global.RM.GetString("System_StatusDocumentsALL29");//"Gửi trả lại bài cho hiệu đính "; ;
                    break;

                case 62:
                    str = Global.RM.GetString("System_StatusDocumentsALL30");//"Gửi Hoàn chỉnh ngữ"; ;
                    break;
                case 63:
                    str = Global.RM.GetString("System_StatusDocumentsALL31"); //"Gửi trả lại bài cho Hoàn chỉnh ngữ "; ;
                    break;

                case 72:
                    str = Global.RM.GetString("System_StatusDocumentsALL32"); //"Gửi DXB"; ;
                    break;
                case 73:
                    str = Global.RM.GetString("System_StatusDocumentsALL33");//" Gửi trả lại bài cho DXB "; ;
                    break;

                case 82:
                    str = Global.RM.GetString("System_StatusDocumentsALL34");//"Gửi Chờ DXB"; ;
                    break;
                case 6:
                    str = Global.RM.GetString("System_StatusDocumentsALL35");//"Gửi Bài đã được xuất bản "; ;
                    break;
                case 4:
                    str = "Bài hủy đang hủy đăng";//"Gửi Bài đã được xuất bản ";
                    break;
                default:
                    str = "";
                    break;
            }
            return str;
        }

        
    }
}

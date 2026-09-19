using System;
using System.Collections.Generic;
using System.Web;
using prjComponents;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace HPCSYNCUploadFiles
{
    public class SyncUpload
    {
        //public static void SynData_UploadImgOne(string path, ArrayList _arr)
        //{
        //    if (_arr.Count > 0)
        //    {
        //        for (int i = 0; i < _arr.Count; i++)
        //        {
        //            SynData_UploadImg(path, _arr[i].ToString());
        //        }
        //    }
        //}
        //private static void SynData_UploadImg(string path, string urlService)
        //{
        //    ServicesPutDataBusines.UltilFunc _untilDAL = new ServicesPutDataBusines.UltilFunc(urlService);
        //    string fileName = string.Empty;
        //    string fullPath = string.Empty;
        //    string Upload = Global.UploadPath;
        //    fullPath = HttpContext.Current.Server.MapPath("../../" + Upload) + "\\" + path.Replace("/", "\\");
        //    char[] ch = { '\\' };
        //    string[] arr = fullPath.Split(ch);
        //    fileName = arr[arr.Length - 1];
        //    string path_Directory = path.Replace("/" + fileName, "");
        //    path_Directory = "/" + path_Directory;
        //    if (File.Exists(fullPath))
        //    {
        //        try
        //        {
        //            FileInfo fInfo = new FileInfo(fullPath);
        //            long numBytes = fInfo.Length;
        //            double dLen = Convert.ToDouble(fInfo.Length / 100000000);
        //            if (dLen < 4)
        //            {
        //                // set up a file stream and binary reader for the
        //                // selected file
        //                FileStream fStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        //                BinaryReader br = new BinaryReader(fStream);
        //                // convert the file to a byte array
        //                byte[] data = br.ReadBytes((int)numBytes);
        //                br.Close();
        //                //_untilDAL.UploadFile(data, fileName, path_Directory);
        //                _untilDAL.UploadFile(data, fileName, path_Directory.ToLower().Replace("/" + prjComponents.Global.UploadPath, ""));
        //                fStream.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}
        public static bool SearchImgTag(string str)
        {
            bool _return = false;
            try
            {
                Regex regex = new Regex(
                    @"(?<=<img[^<]+?src=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    //"/Upload/"
                    string _url = matchCollect[i].Value.Trim();

                    if (_url.IndexOf("http://") <= 0 && _url.Length > 0)
                    {
                        _url = _url.Replace("http://localhost/", "").Trim();
                        string Upload = prjComponents.Global.UploadPath;
                        //string fullPath = HttpContext.Current.Server.MapPath(Upload) + _url.Replace("/", "\\");
                        //string fullPath = HttpContext.Current.Server.MapPath(Upload + "../../../") + _url.Replace("/", "\\");
                        string fullPath = HttpContext.Current.Server.MapPath("../../" + Upload) + _url.Replace("/" + prjComponents.Global.UploadPath, "").Replace("/", "\\");
                       // if (File.Exists(fullPath))
                           // SynData_UploadImgOne(_url.Replace("/" + prjComponents.Global.UploadPath, ""), prjComponents.Global.ImagesService);
                    }
                }
                _return = true;
            }catch{_return = false;}
            return _return;
        }

        public static bool SearchTagSwf(string str)
        {
            bool _return = false;
            try
            {
                Regex regex = new Regex(
                    @"(?<=<embed[^<]+?src=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    //"/Upload/"
                    string _url = matchCollect[i].Value.Trim();

                    if (_url.IndexOf("http://") <= 0 && _url.Length > 0)
                    {
                        _url = _url.Replace("http://localhost/", "").Trim();
                        string Upload = prjComponents.Global.UploadPath;
                        //string fullPath = HttpContext.Current.Server.MapPath(Upload) + _url.Replace("/", "\\");
                        string fullPath = HttpContext.Current.Server.MapPath("../../" + Upload) + _url.Replace("/" + prjComponents.Global.UploadPath, "").Replace("/", "\\");
                        //if (File.Exists(fullPath))
                          //  SynData_UploadImgOne(_url.Replace("/" + prjComponents.Global.UploadPath, ""), prjComponents.Global.ImagesService);
                    }
                }
                _return = true;
            }
            catch
            {
                _return = false;
            }
            return _return;
        }


        public static bool SearchTagFLV(string str)
        {
            bool _return = false;
            try
            {
                Regex regex = new Regex(
                    @"(?<=<embed[^<]+?flashvars=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    //"/Upload/"
                    string _url = matchCollect[i].Value.Trim().Replace("file=", "");

                    if (_url.IndexOf("http://") <= 0 && _url.Length > 0)
                    {
                        _url = _url.Replace("http://localhost/", "").Trim();
                        string Upload = prjComponents.Global.UploadPath;
                        //string fullPath = HttpContext.Current.Server.MapPath(Upload + "../../../") + _url.Replace("/", "\\");
                        string fullPath = HttpContext.Current.Server.MapPath("../../" + Upload) + _url.Replace("/" + prjComponents.Global.UploadPath, "").Replace("/", "\\");
                        //if (File.Exists(fullPath))
                           // SynData_UploadImgOne(_url.Replace("/" + prjComponents.Global.UploadPath, ""), prjComponents.Global.ImagesService);
                    }
                }
                _return = true;
            }
            catch
            {
                _return = false;
            }
            return _return;
        }

    }
}

using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;
using System.Net.Http;
using QLB.BusinessLogic;
using System.Data;
using QLB.API.Common;
namespace QLB.API.Controllers.InterfacePublic
{
    public class ApiExtensionController : ApiController
    {

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity ExcuteReturnInt(string packageName, string storeName, object obj)
        {
            try
            {
                if (obj.ToString() == "System.Object")
                {
                    var json = Request.Content.ReadAsStreamAsync().Result;
                    Stream stream = new MemoryStream();
                    json.Seek(0, SeekOrigin.Begin);
                    StreamReader rd = new StreamReader(json);
                    var ax = rd.ReadToEnd();
                    var _obj = JsonConvert.DeserializeObject<object>(ax, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddThh:mm:ss" });
                    return new Data.AipHelper.ApiHelperRepository().ExecuteScalar(packageName, storeName, _obj);
                }
            }
            catch(Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName + ex.ToString());
            }
            return new Data.AipHelper.ApiHelperRepository().ExecuteScalar(packageName, storeName, obj);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity ExcuteTable(string packageName, string storeName, object obj)
        {
            try
            {
                if (obj.ToString() == "System.Object")
                {
                    var json = Request.Content.ReadAsStreamAsync().Result;
                    Stream stream = new MemoryStream();
                    json.Seek(0, SeekOrigin.Begin);
                    StreamReader rd = new StreamReader(json);
                    var ax = rd.ReadToEnd();
                    var _obj = JsonConvert.DeserializeObject<object>(ax, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddThh:mm:ss" });
                    return new Data.AipHelper.ApiHelperRepository().ExcuteDataTable(packageName, storeName, _obj);
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName + ex.ToString());
            }
            return new Data.AipHelper.ApiHelperRepository().ExcuteDataTable(packageName, storeName, obj);
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity ExcutePostReturnInt(string packageName, string storeName, object obj)
        {
            ReponseReportEntity objRes = new ReponseReportEntity();
            try
            {

                objRes = new Data.AipHelper.ApiHelperRepository().ExecuteScalar(packageName, storeName, obj);
            }
            
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName + ex.ToString());
            }
            return objRes;
                
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseReportEntity ExcutePostTable(string packageName, string storeName, object obj)
        {
            ReponseReportEntity objRes = new ReponseReportEntity();
            try
            {

                return new Data.AipHelper.ApiHelperRepository().ExcuteDataTable(packageName, storeName, obj);
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName + ex.ToString());
            }
            return objRes;

        }

    }
}

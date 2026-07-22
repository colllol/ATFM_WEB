using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic;
using System.Reflection;

namespace QLB.API.Data.AipHelper
{
    public class ApiHelperRepository
    {
        public ReponseReportEntity ExcuteDataTable(string packageName, string storeName, object obj)
        {
            ReponseReportEntity oResponse = new ReponseReportEntity();

            try
            {

                oResponse= new ReponseEntityHelper().AipHelper(new oDataProvider().ExecuteDataseForApiExtension(packageName, storeName, obj).Tables[0]);
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName + ex.ToString());
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseReportEntity ExecuteScalar(string packageName, string storeName, object obj)
        {
            ReponseReportEntity oResponse = new ReponseReportEntity();
            try
            {
                oResponse = new ReponseEntityHelper().AipHelper(new oDataProvider().ExecuteNonQueryForApiExtension(packageName, storeName, obj));
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, packageName + "--" + storeName  + ex.ToString());
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
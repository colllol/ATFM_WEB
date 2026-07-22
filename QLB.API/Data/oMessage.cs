using QLB.API.Common;
using QLB.API.Models;
using QLB.BusinessLogic;
using QLB.Info;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace QLB.API
{
    public static class oMessage
    {
        public const string CodeSussess = "00";
        public const string CodeError = "-1";
        public const string NoData = "No data!";
        public const string GetDataSussess = "Get Data Sussess!";
        public const string GetDataError = "Get Data Error!";
        public const string InsertSussess = "Insert Sussess!";
        public const string InsertError = "Insert Error!";
        public const string UpdateSussess = "Update Sussess!";
        public const string UpdateError = "Update Error!";
        public const string DeleteSussess = "Delete Sussess!";
        public const string DeleteError = "Delete Error!";
        public const string RestoreSussess = "Restore Error!";
        public const string RestoreError = "Restore Error!";
        public const string ExceptionCode = "-99";
        public const string ExceptionMessage = "Has Exception!";
    }
    public class ReponseEntityHelper
    {

        public static string _Path = HttpContext.Current.Server.MapPath(@"~/bin/QLB.BusinessLogic.dll");

        //public ReponseReportEntity SumPur<T>(DataTable dt) where T : class, new()
        //{
        //    string sReturn = string.Empty;
        //    string sOutPut = string.Empty;
        //    ReponseReportEntity oResponse = new ReponseReportEntity();
        //    try
        //    {
        //       if (dt.Rows.Count > 0)
        //        {
        //            var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
        //            oResponse.Code = oMessage.CodeSussess;
        //            oResponse.Message = oMessage.GetDataSussess;
        //            oResponse.ListValue = 
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.SumPur: " + ex.Message);
        //        oResponse.Code = oMessage.ExceptionCode;
        //        oResponse.Message = oMessage.ExceptionMessage;
        //    }
        //    return oResponse;
        //}

        public ReponseEntity GetPage<T>(DataTable dt) where T : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.SumRecord = dt.Rows[0]["Record_Sum"].ToString();
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = lis.ToList<object>();
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.NoData;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetPage: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity GetPageExport<T>(DataTable dt) where T : class,new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
                    oResponse.Code = oMessage.CodeSussess;                    
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = lis.ToList<object>();
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.NoData;
                }
            }
            catch(Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetPageExport: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity GetAll<T>(DataTable dt) where T : class, new()
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);

                    if (lis != null || lis.Count > 0)
                    {
                        oResponse.Code = oMessage.CodeSussess;
                        oResponse.Message = oMessage.GetDataSussess;
                        oResponse.ListValue = lis.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = oMessage.CodeError;
                        oResponse.Message = oMessage.NoData;
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetAll");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetAll: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }


            return oResponse;
        }
        public ReponseEntity GetByID<T>(DataTable dt) where T : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
                    if (lis != null || lis.Count > 0)
                    {
                        oResponse.Code = oMessage.CodeSussess;
                        oResponse.Message = oMessage.GetDataSussess;
                        oResponse.ListValue = lis.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = oMessage.CodeError;
                        oResponse.Message = oMessage.NoData;
                    }
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.GetDataError;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetByID: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }

            return oResponse;
        }
        /// <summary>
        /// Return data table histori by id
        /// </summary>
        /// <typeparam name="T">Class back up</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ReponseEntity GetHistoryByID<T>(DataTable dt) where T : class, new()
        {
            var oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = lis.ToList<object>();
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.GetDataError;
                }
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetHis: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity GetReport<T>(DataTable dt) where T : class, new()
        {
            var oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    var lis = new clsConvertTableToObject().ConvertTo<T>(dt);
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = lis.ToList<object>();
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.GetDataError;
                }
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetReport: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseEntity GetRecordDeleted<T>(DataTable dt) where T : class, new()
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = new clsConvertTableToObject().ConvertTo<T>(dt).ToList<object>();
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.GetDataError;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, new T().ToString() + "Repository.GetDelete: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }

            return oResponse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="obj"></param>
        /// <param name="methobName">Name Methob insert object DAL</param>
        /// <returns></returns>
        public ReponseEntity Insert<T, DAL>(T obj, string methobName) where T : class, new() where DAL : class, new()
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {                       
                Int64 Id = (Int64)Invoker.CreateAndInvoke(new DAL().ToString(), methobName, new object[] { obj });
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.InsertError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Insert");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.InsertSussess; oResponse.Code = Id.ToString(); }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new T().ToString()}Repository.Insert: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="Id"></param>
        /// <param name="methobName">Name methob delete object DAL</param>
        /// <returns></returns>
        public ReponseEntity Delete<T, DAL>(Int64 Id, string methobName) where T : class, new() where DAL : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                Invoker.CreateAndInvoke($"{new DAL().ToString()}", methobName, new object[] { Id });
                oResponse.Code = Id.ToString();
                oResponse.Message = oMessage.DeleteSussess;
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Delete: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }

        public ReponseEntity WaitDelete<T, DAL>(Int64 Id, Int64 Status, string methobName) where T : class, new() where DAL : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                Invoker.CreateAndInvoke($"{new DAL().ToString()}", methobName, new object[] { Id, Status });
                oResponse.Code = Id.ToString();
                oResponse.Message = oMessage.DeleteSussess;
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Delete: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="Id"></param>
        /// <param name="methobName">Name methob delete object DAL</param>
        /// <returns></returns>
        public ReponseEntity Movedate<T, DAL>(Int64 Id, string methobName) where T : class, new() where DAL : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                Invoker.CreateAndInvoke($"{new DAL().ToString()}", methobName, new object[] { Id });
                oResponse.Code = Id.ToString();
                oResponse.Message = oMessage.UpdateSussess;
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Movedate: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }



        public ReponseEntity DeleteReturnParamOut<T, DAL>(Int64 Id, string methobName) where T : class, new() where DAL : class, new()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                Int64 kq = (Int64)Invoker.CreateAndInvoke($"{new DAL().ToString()}", methobName, new object[] { Id });
                if (kq != -1) { 
                oResponse.Code = kq.ToString();
                oResponse.Message = oMessage.DeleteSussess;
                }
                else
                {
                    oResponse.Code = kq.ToString();
                    oResponse.Message = oMessage.DeleteError;
                }
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Delete: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="DAL"></typeparam>
        /// <param name="obj"></param>
        /// <param name="methobName">Name methob update object DAL</param>
        /// <returns></returns>
        public ReponseEntity Update<T, DAL>(T obj, string methobName) where T : class, new() where DAL : class, new()
        {
            ReponseEntity oResponse = new ReponseEntity();
            try
            {
                Int64 Id = (Int64)Invoker.CreateAndInvoke($"{new DAL().ToString()}", methobName, new object[] { obj });
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.UpdateError;
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Update");
                    oResponse.Code = oMessage.ExceptionCode;
                    oResponse.Message = oMessage.ExceptionMessage;
                }
                else
                { oResponse.Message = oMessage.UpdateSussess; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, $"{new DAL().ToString()}Repository.Update: {ex.Message}");
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Class non backup</typeparam>
        /// <typeparam name="DAL">DAL call funciton</typeparam>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="iduser"></param>
        /// <param name="methobName">Name Methob retore history DAL: Type (bool)</param>
        /// <returns></returns>
        public ReponseEntity RestoreHis<T, DAL>(Int64 id, int version, string iduser, string methobName) where T : class, new() where DAL : class, new()
        {
            ReponseEntity oResponse = new ReponseEntity();

            var ax = bool.Parse(Invoker.CreateAndInvoke(new DAL().ToString(), methobName, new object[] { id, version, iduser }).ToString());
            oResponse.Code = ax ? oMessage.CodeSussess : oMessage.CodeError;
            oResponse.Message = ax ? oMessage.RestoreSussess : oMessage.RestoreError;
            return oResponse;
        }
        public static class Invoker
        {
            public static object CreateAndInvoke(string typeName, string methodName, object[] methodArgs)
            {
                Assembly a = Assembly.LoadFile(_Path);
                Type type = a.GetType(typeName);
                object instance = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                return method.Invoke(instance, methodArgs);
            }
        }

        #region return respon for report
        public ReponseReportEntity GetTable(DataTable dt)
        {
            var oResponse = new ReponseReportEntity();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.SumRecord = dt.Rows.Count.ToString();
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = dt;
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.NoData;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "Data for report: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseReportEntity AipHelper(object value)
        {
            var oResponse = new ReponseReportEntity();
            try
            {
                if (value != null)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = value;
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.NoData;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "Get data helper: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        public ReponseReportEntity GetString(string _value)
        {
            var oResponse = new ReponseReportEntity();
            try
            {
                if (_value.Length > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.SumRecord = "";
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = _value.ToString();
                    
                }
                else
                {
                    oResponse.Code = oMessage.CodeError;
                    oResponse.Message = oMessage.NoData;
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "Data for report: " + ex.Message);
                oResponse.Code = oMessage.ExceptionCode;
                oResponse.Message = oMessage.ExceptionMessage;
            }
            return oResponse;
        }
        #endregion
    }
    public static class DateTimeHelper
    {
        public static DateTime ConvertToDateTime(string value, string formatType)
        {
            if (value.IndexOf("-") > 0)
            {
                value = value.Replace("-", "");
            }
            else if(value.IndexOf("/") > 0) value = value.Replace("/", "");
            return DateTime.ParseExact(value, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

    }
}
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

namespace QLB.API.Data
{
    public class ReportingRouteRepository
    {
        public ReponseEntity GetPageReportingRoute(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //ReportingRouteDAL objDAL = new ReportingRouteDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{
            //    _dt = objDAL.GetPageReportingRoute(page_size, page_index, where).Tables[0];

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<ReportingRoute> lstREPORTINGROUTE = new List<ReportingRoute>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            ReportingRoute obj = new ReportingRoute();
            //            if (_dt.Rows[i]["POINT_NAME"] != DBNull.Value) obj.POINT_NAME = _dt.Rows[i]["POINT_NAME"].ToString();
            //            if (_dt.Rows[i]["ROUTE_NAME"] != DBNull.Value) obj.ROUTE_NAME = _dt.Rows[i]["ROUTE_NAME"].ToString();
            //            if (_dt.Rows[i]["ROUTE_ID"] != DBNull.Value) obj.ROUTE_ID = int.Parse(_dt.Rows[i]["ROUTE_ID"].ToString());

            //            lstREPORTINGROUTE.Add(obj);
            //        }
            //        if (lstREPORTINGROUTE != null || lstREPORTINGROUTE.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstREPORTINGROUTE.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.GetPageReportingRoute:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.GetPageReportingRoute:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<ReportingRoute>(new ReportingRouteDAL().GetPageReportingRoute(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageReportingRouteExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<ReportingRoute>(new ReportingRouteDAL().GetPageReportingRouteExport(where).Tables[0]);
        }
      

        public ReponseEntity GetAllReportingRoute()
        {
            return new ReponseEntityHelper().GetAll<ReportingRoute>(new ReportingRouteDAL().GetAllReportingRoute().Tables[0]);
        }

        public ReponseEntity GetByIdReportingRoute(int Id)
        {
            return new ReponseEntityHelper().GetByID<ReportingRoute>(new ReportingRouteDAL().GetByIdReportingRoute(Id).Tables[0]);
        }

        public ReponseEntity CreateReportingRoute(ReportingRoute oREPORTINGROUTE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingRouteDAL objDAL = new ReportingRouteDAL();
                Int32 Id = objDAL.CreateReportingRoute(oREPORTINGROUTE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.CreateReportingRoute:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.CreateReportingRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateReportingRoute(ReportingRoute oREPORTINGROUTE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingRouteDAL objDAL = new ReportingRouteDAL();
                Int32 Id = objDAL.UpdateReportingRoute(oREPORTINGROUTE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.UpdateReportingRoute:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.UpdateReportingRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteReportingRoute(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingRouteDAL objDAL = new ReportingRouteDAL();
                objDAL.DeleteReportingRoute(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.DeleteReportingRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        #region ROUTE_SECTOR
        public ReponseEntity GetPageReportingSector(int page_size, int page_index, string where)
        {
           
            return new ReponseEntityHelper().GetPage<SectorRoute>(new ReportingRouteDAL().GetPageReportingSector(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity DeleteReportingSector(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingRouteDAL objDAL = new ReportingRouteDAL();
                objDAL.DeleteReportingSector(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingRouteRepository.DeleteReportingSector: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        #endregion
    }
}
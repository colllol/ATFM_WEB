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
    public class ReportStaticRepository
    {
        public ReponseEntity GetPageReportStatic(int page_size, int page_index, string where)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //ReportStaticDAL objDAL = new ReportStaticDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageReportStatic(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountReportStatic(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<ReportStatic> lstAERO = new List<ReportStatic>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            ReportStatic obj = new ReportStatic();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["NAME_REPORT"] != DBNull.Value) obj.NAME_REPORT = _dt.Rows[i]["NAME_REPORT"].ToString();
            //            if (_dt.Rows[i]["ADDRESS_FILE"] != DBNull.Value) obj.ADDRESS_FILE = _dt.Rows[i]["ADDRESS_FILE"].ToString();
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            lstAERO.Add(obj);
            //        }
            //        if (lstAERO != null || lstAERO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstAERO.ToList<object>();
            //            oResponse.Value = _dt1.Rows[0][0];
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.GetPageReportStatic:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.GetPageReportStatic:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<ReportStatic>(new ReportStaticDAL().GetPageReportStatic(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageReportStaticExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<ReportStatic>(new ReportStaticDAL().GetPageReportStaticExport(where).Tables[0]);
        }

        public ReponseEntity GetAllReportStatic()
        {
            return new ReponseEntityHelper().GetAll<ReportStatic>(new ReportStaticDAL().GetAllReportStatic().Tables[0]);
        }
        public ReponseEntity GetAllReportNameStatic()
        {
            return new ReponseEntityHelper().GetAll<ReportNameStatic>(new ReportStaticDAL().GetAllReportNameStatic().Tables[0]);
        }

        public ReponseEntity GetByIdReportStatic(int Id)
        {
            return new ReponseEntityHelper().GetByID<ReportStatic>(new ReportStaticDAL().GetByIdReportStatic(Id).Tables[0]);
        }

        public ReponseEntity CreateReportStatic(ReportStatic oREPORT)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportStaticDAL objDAL = new ReportStaticDAL();
                Int32 Id = objDAL.CreateReportStatic(oREPORT);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.CreateReportStatic:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.CreateReportStatic: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateReportStatic(ReportStatic oREPORT)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportStaticDAL objDAL = new ReportStaticDAL();
                Int32 Id = objDAL.UpdateReportStatic(oREPORT);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.UpdateReportStatic:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.UpdateReportStatic: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteReportStatic(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportStaticDAL objDAL = new ReportStaticDAL();
                objDAL.DeleteReportStatic(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportStaticRepository.DeleteReportStatic: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
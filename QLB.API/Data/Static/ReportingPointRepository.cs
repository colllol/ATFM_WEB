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
    public class ReportingPointRepository
    {
        public ReponseEntity GetPageReportingPoint(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //ReportingPointDAL objDAL = new ReportingPointDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageReportingPoint(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountReportingPoint(where).Tables[0];
            //    //DataTable dtCount = new DataTable();

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<ReportingPoint> lstREPORTINGPOINT = new List<ReportingPoint>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            ReportingPoint obj = new ReportingPoint();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["POINT_NAME"] != DBNull.Value) obj.POINT_NAME = _dt.Rows[i]["POINT_NAME"].ToString();
            //            if (_dt.Rows[i]["COMPULSORY_ON_REQUEST"] != DBNull.Value) obj.COMPULSORY_ON_REQUEST = _dt.Rows[i]["COMPULSORY_ON_REQUEST"].ToString();
            //            if (_dt.Rows[i]["DESCRIPTION"] != DBNull.Value) obj.DESCRIPTION = _dt.Rows[i]["DESCRIPTION"].ToString();
            //            if (_dt.Rows[i]["IN_FIR_VN"] != DBNull.Value) obj.IN_FIR_VN = _dt.Rows[i]["IN_FIR_VN"].ToString();
            //            if (_dt.Rows[i]["FIR_HN"] != DBNull.Value) obj.FIR_HN = _dt.Rows[i]["FIR_HN"].ToString();
            //            if (_dt.Rows[i]["FIR_HCM"] != DBNull.Value) obj.FIR_HCM = _dt.Rows[i]["FIR_HCM"].ToString();
            //            if (_dt.Rows[i]["POINT_ID"] != DBNull.Value) obj.POINT_ID = int.Parse(_dt.Rows[i]["POINT_ID"].ToString());

            //            lstREPORTINGPOINT.Add(obj);

            //        }
            //        if (lstREPORTINGPOINT != null || lstREPORTINGPOINT.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstREPORTINGPOINT.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.GetPageReportingPoint:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.GetPageReportingPoint:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<ReportingPoint>(new ReportingPointDAL().GetPageReportingPoint(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageReportingPointExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<ReportingPoint>(new ReportingPointDAL().GetPageReportingPointExport(where).Tables[0]);
        }

        public ReponseEntity GetAllReportingPoint()
        {
            return new ReponseEntityHelper().GetAll<ReportingPoint>(new ReportingPointDAL().GetAllReportingPoint().Tables[0]);
        }
        
        public ReponseEntity GetByIdReportingPoint(int Id)
        {
            return new ReponseEntityHelper().GetByID<ReportingPoint>(new ReportingPointDAL().GetByIdReportingPoint(Id).Tables[0]);
        }

        public ReponseEntity GetAddReportingPoint(int Route_Id)
        {
            return new ReponseEntityHelper().GetByID<ReportingPoint>(new ReportingPointDAL().GetAddReportingPoint(Route_Id).Tables[0]);
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //ReportingPointDAL objDAL = new ReportingPointDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = objDAL.GetAddReportingPoint(Route_Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<ReportingPoint> lstREPORTINGPOINT = new List<ReportingPoint>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            ReportingPoint obj = new ReportingPoint();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["POINT_NAME"] != DBNull.Value) obj.POINT_NAME = _dt.Rows[i]["POINT_NAME"].ToString();
            //            if (_dt.Rows[i]["COMPULSORY_ON_REQUEST"] != DBNull.Value) obj.COMPULSORY_ON_REQUEST = _dt.Rows[i]["COMPULSORY_ON_REQUEST"].ToString();
            //            //if (_dt.Rows[i]["DESCRIPTION"] != DBNull.Value) obj.DESCRIPTION = _dt.Rows[i]["DESCRIPTION"].ToString();
            //            if (_dt.Rows[i]["IN_FIR_VN"] != DBNull.Value) obj.IN_FIR_VN = _dt.Rows[i]["IN_FIR_VN"].ToString();
            //            if (_dt.Rows[i]["FIR_HN"] != DBNull.Value) obj.FIR_HN = _dt.Rows[i]["FIR_HN"].ToString();
            //            if (_dt.Rows[i]["FIR_HCM"] != DBNull.Value) obj.FIR_HCM = _dt.Rows[i]["FIR_HCM"].ToString();
            //            if (_dt.Rows[i]["POINT_ID"] != DBNull.Value) obj.POINT_ID = int.Parse(_dt.Rows[i]["POINT_ID"].ToString());
            //            if (_dt.Rows[i]["MARK"] != DBNull.Value) obj.MARK = _dt.Rows[i]["MARK"].ToString();

            //            lstREPORTINGPOINT.Add(obj);
            //        }
            //        if (lstREPORTINGPOINT != null || lstREPORTINGPOINT.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            //oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstREPORTINGPOINT.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.GetByIdReportingPoint:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
        }

        public ReponseEntity GetAddSectorPoint(int Route_Id)
        {
            return new ReponseEntityHelper().GetByID<Sectors>(new ReportingPointDAL().GetAddSectorPoint(Route_Id).Tables[0]);
            
        }


        public ReponseEntity CreateReportingPoint(ReportingPoint oREPORTINGPOINT)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingPointDAL objDAL = new ReportingPointDAL();
                Int32 Id = objDAL.CreateReportingPoint(oREPORTINGPOINT);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.CreateReportingPoint:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.CreateReportingPoint: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateReportingPoint(ReportingPoint oREPORTINGPOINT)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingPointDAL objDAL = new ReportingPointDAL();
                Int32 Id = objDAL.UpdateReportingPoint(oREPORTINGPOINT);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.UpdateReportingPoint:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.UpdateReportingPoint: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteReportingPoint(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                ReportingPointDAL objDAL = new ReportingPointDAL();
                objDAL.DeleteReportingPoint(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "ReportingPointRepository.DeleteReportingPoint: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
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
    public class SubdivisionRepository
    {
        public ReponseEntity GetPageSubdivision(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //SubdivisionDAL objDAL = new SubdivisionDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageSubdivision(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountSubdivision(where).Tables[0];
            //    //DataTable dtCount = new DataTable();

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Subdivision> lstSUBDIVISION = new List<Subdivision>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Subdivision obj = new Subdivision();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["SUBDIVISION_NAME"] != DBNull.Value) obj.SUBDIVISION_NAME = _dt.Rows[i]["SUBDIVISION_NAME"].ToString();
            //            if (_dt.Rows[i]["DESCRIPTION"] != DBNull.Value) obj.DESCRIPTION = _dt.Rows[i]["DESCRIPTION"].ToString();
            //            if (_dt.Rows[i]["FIR_HN"] != DBNull.Value) obj.FIR_HN = _dt.Rows[i]["FIR_HN"].ToString();
            //            if (_dt.Rows[i]["FIR_HCM"] != DBNull.Value) obj.FIR_HCM = _dt.Rows[i]["FIR_HCM"].ToString();

            //            lstSUBDIVISION.Add(obj);

            //        }
            //        if (lstSUBDIVISION != null || lstSUBDIVISION.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstSUBDIVISION.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.GetPageSubdivision:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.GetPageSubdivision:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<Subdivision>(new SubdivisionDAL().GetPageSubdivision(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageSubdivisionExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Subdivision>(new SubdivisionDAL().GetPageSubdivisionExport(where).Tables[0]);
        }

        public ReponseEntity GetAllSubdivision()
        {
            return new ReponseEntityHelper().GetAll<Subdivision>(new SubdivisionDAL().GetAllSubdivision().Tables[0]);
        }

        public ReponseEntity GetByIdSubdivision(int Id)
        {
            return new ReponseEntityHelper().GetByID<Subdivision>(new SubdivisionDAL().GetByIdSubdivision(Id).Tables[0]);
        }

        public ReponseEntity CreateSubdivision(Subdivision oSUBDIVISION)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SubdivisionDAL objDAL = new SubdivisionDAL();
                Int32 Id = objDAL.CreateSubdivision(oSUBDIVISION);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.CreateSubdivision:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.CreateSubdivision: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateSubdivision(Subdivision oSUBDIVISION)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SubdivisionDAL objDAL = new SubdivisionDAL();
                Int32 Id = objDAL.UpdateSubdivision(oSUBDIVISION);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.UpdateSubdivision:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.UpdateSubdivision: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteSubdivision(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SubdivisionDAL objDAL = new SubdivisionDAL();
                objDAL.DeleteSubdivision(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SubdivisionRepository.DeleteSubdivision: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
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
    public class AeroRepository
    {
        public ReponseEntity GetPageAero(int page_size, int page_index,string where)
        {
            #region Code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //AeroDAL objDAL = new AeroDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageAero(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountAero(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Aero> lstAERO = new List<Aero>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Aero obj = new Aero();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["AE_ID"] != DBNull.Value) obj.AE_ID = int.Parse(_dt.Rows[i]["AE_ID"].ToString());
            //            if (_dt.Rows[i]["CTR_CODE"] != DBNull.Value) obj.CTR_CODE = _dt.Rows[i]["CTR_CODE"].ToString();
            //            if (_dt.Rows[i]["FR_ID"] != DBNull.Value) obj.FR_ID = int.Parse(_dt.Rows[i]["FR_ID"].ToString());
            //            if (_dt.Rows[i]["AE_CODE"] != DBNull.Value) obj.AE_CODE = _dt.Rows[i]["AE_CODE"].ToString();
            //            if (_dt.Rows[i]["AE_NAME"] != DBNull.Value) obj.AE_NAME = _dt.Rows[i]["AE_NAME"].ToString();
            //            if (_dt.Rows[i]["AE_ZONE"] != DBNull.Value) obj.AE_ZONE = _dt.Rows[i]["AE_ZONE"].ToString();
            //            if (_dt.Rows[i]["AE_INTER"] != DBNull.Value) obj.AE_INTER = _dt.Rows[i]["AE_INTER"].ToString();
            //            if (_dt.Rows[i]["AE_IATA"] != DBNull.Value) obj.AE_IATA = _dt.Rows[i]["AE_IATA"].ToString();
            //            if (_dt.Rows[i]["AE_LAT"] != DBNull.Value) obj.AE_LAT = _dt.Rows[i]["AE_LAT"].ToString();
            //            if (_dt.Rows[i]["AE_LONG"] != DBNull.Value) obj.AE_LONG = _dt.Rows[i]["AE_LONG"].ToString();
            //            if (_dt.Rows[i]["AD_ISOVERSEA"] != DBNull.Value) obj.AD_ISOVERSEA = _dt.Rows[i]["AD_ISOVERSEA"].ToString();
            //            if (_dt.Rows[i]["SUMMER_TIME"] != DBNull.Value) obj.SUMMER_TIME = _dt.Rows[i]["SUMMER_TIME"].ToString();
            //            if (_dt.Rows[i]["WINTER_TIME"] != DBNull.Value) obj.WINTER_TIME = _dt.Rows[i]["WINTER_TIME"].ToString();
            //            if (_dt.Rows[i]["TM"] != DBNull.Value) obj.TM = _dt.Rows[i]["TM"].ToString();
            //            if (_dt.Rows[i]["TN"] != DBNull.Value) obj.TN = _dt.Rows[i]["TN"].ToString();

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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetPageAero:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetPageAero:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<Aero>(new AeroDAL().GetPageAero(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageAeroExport(string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //AeroDAL objDAL = new AeroDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageAeroExport(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Aero> lstAERO = new List<Aero>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Aero obj = new Aero();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["AE_ID"] != DBNull.Value) obj.AE_ID = int.Parse(_dt.Rows[i]["AE_ID"].ToString());
            //            if (_dt.Rows[i]["CTR_CODE"] != DBNull.Value) obj.CTR_CODE = _dt.Rows[i]["CTR_CODE"].ToString();
            //            if (_dt.Rows[i]["FR_ID"] != DBNull.Value) obj.FR_ID = int.Parse(_dt.Rows[i]["FR_ID"].ToString());
            //            if (_dt.Rows[i]["AE_CODE"] != DBNull.Value) obj.AE_CODE = _dt.Rows[i]["AE_CODE"].ToString();
            //            if (_dt.Rows[i]["AE_NAME"] != DBNull.Value) obj.AE_NAME = _dt.Rows[i]["AE_NAME"].ToString();
            //            if (_dt.Rows[i]["AE_ZONE"] != DBNull.Value) obj.AE_ZONE = _dt.Rows[i]["AE_ZONE"].ToString();
            //            if (_dt.Rows[i]["AE_INTER"] != DBNull.Value) obj.AE_INTER = _dt.Rows[i]["AE_INTER"].ToString();
            //            if (_dt.Rows[i]["AE_IATA"] != DBNull.Value) obj.AE_IATA = _dt.Rows[i]["AE_IATA"].ToString();
            //            if (_dt.Rows[i]["AE_LAT"] != DBNull.Value) obj.AE_LAT = _dt.Rows[i]["AE_LAT"].ToString();
            //            if (_dt.Rows[i]["AE_LONG"] != DBNull.Value) obj.AE_LONG = _dt.Rows[i]["AE_LONG"].ToString();
            //            if (_dt.Rows[i]["AD_ISOVERSEA"] != DBNull.Value) obj.AD_ISOVERSEA = _dt.Rows[i]["AD_ISOVERSEA"].ToString();
            //            if (_dt.Rows[i]["SUMMER_TIME"] != DBNull.Value) obj.SUMMER_TIME = _dt.Rows[i]["SUMMER_TIME"].ToString();
            //            if (_dt.Rows[i]["WINTER_TIME"] != DBNull.Value) obj.WINTER_TIME = _dt.Rows[i]["WINTER_TIME"].ToString();
            //            if (_dt.Rows[i]["TM"] != DBNull.Value) obj.TM = _dt.Rows[i]["TM"].ToString();
            //            if (_dt.Rows[i]["TN"] != DBNull.Value) obj.TN = _dt.Rows[i]["TN"].ToString();

            //            lstAERO.Add(obj);
            //        }
            //        if (lstAERO != null || lstAERO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            //oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstAERO.ToList<object>();
            //            //oResponse.Value = _dt1.Rows[0][0];
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetPageAeroExport:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetPageAeroExport:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPageExport<Aero>(new AeroDAL().GetPageAeroExport(where).Tables[0]);
        }

        public ReponseEntity GetAllAero()
        {
            return new ReponseEntityHelper().GetAll<Aero>(new AeroDAL().GetAllAero().Tables[0]);
        }
        public ReponseEntity GetAeroVV()
        {
            return new ReponseEntityHelper().GetAll<Aero>(new AeroDAL().GetAeroVV().Tables[0]);
        }
        public ReponseEntity GetByIdAero(int Id)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //AeroDAL objDAL = new AeroDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = objDAL.GetByIdAero(Id).Tables[0];

            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Aero> lstAERO = new List<Aero>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Aero obj = new Aero();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["AE_ID"] != DBNull.Value) obj.AE_ID = int.Parse(_dt.Rows[i]["AE_ID"].ToString());
            //            if (_dt.Rows[i]["CTR_CODE"] != DBNull.Value) obj.CTR_CODE = _dt.Rows[i]["CTR_CODE"].ToString();
            //            if (_dt.Rows[i]["FR_ID"] != DBNull.Value) obj.FR_ID = int.Parse(_dt.Rows[i]["FR_ID"].ToString());
            //            if (_dt.Rows[i]["AE_CODE"] != DBNull.Value) obj.AE_CODE = _dt.Rows[i]["AE_CODE"].ToString();
            //            if (_dt.Rows[i]["AE_NAME"] != DBNull.Value) obj.AE_NAME = _dt.Rows[i]["AE_NAME"].ToString();
            //            if (_dt.Rows[i]["AE_ZONE"] != DBNull.Value) obj.AE_ZONE = _dt.Rows[i]["AE_ZONE"].ToString();
            //            if (_dt.Rows[i]["AE_INTER"] != DBNull.Value) obj.AE_INTER = _dt.Rows[i]["AE_INTER"].ToString();
            //            if (_dt.Rows[i]["AE_IATA"] != DBNull.Value) obj.AE_IATA = _dt.Rows[i]["AE_IATA"].ToString();
            //            if (_dt.Rows[i]["AE_LAT"] != DBNull.Value) obj.AE_LAT = _dt.Rows[i]["AE_LAT"].ToString();
            //            if (_dt.Rows[i]["AE_LONG"] != DBNull.Value) obj.AE_LONG = _dt.Rows[i]["AE_LONG"].ToString();
            //            if (_dt.Rows[i]["AD_ISOVERSEA"] != DBNull.Value) obj.AD_ISOVERSEA = _dt.Rows[i]["AD_ISOVERSEA"].ToString();
            //            if (_dt.Rows[i]["SUMMER_TIME"] != DBNull.Value) obj.SUMMER_TIME = _dt.Rows[i]["SUMMER_TIME"].ToString();
            //            if (_dt.Rows[i]["WINTER_TIME"] != DBNull.Value) obj.WINTER_TIME = _dt.Rows[i]["WINTER_TIME"].ToString();
            //            if (_dt.Rows[i]["TM"] != DBNull.Value) obj.TM = _dt.Rows[i]["TM"].ToString();
            //            if (_dt.Rows[i]["TN"] != DBNull.Value) obj.TN = _dt.Rows[i]["TN"].ToString();

            //            lstAERO.Add(obj);
            //        }
            //        if (lstAERO != null || lstAERO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstAERO.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetByIdAero:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.GetByIdAero:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<Aero>(new AeroDAL().GetByIdAero(Id).Tables[0]);
        }

        public ReponseEntity CreateAero(Aero oAERO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AeroDAL objDAL = new AeroDAL();
                Int64 Id = objDAL.CreateAero(oAERO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.CreateAero:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.CreateAero: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
            //return new ReponseEntityHelper().Insert<Aero, AeroDAL>(oAERO, "Insert");
        }

        public ReponseEntity UpdateAero(Aero oAERO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AeroDAL objDAL = new AeroDAL();
                Int64 Id = objDAL.UpdateAero(oAERO);    
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.UpdateAero:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.UpdateAero: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteAero(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AeroDAL objDAL = new AeroDAL();
                objDAL.DeleteAero(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AeroRepository.DeleteAero: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
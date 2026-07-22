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
    public class GadRepository
    {
        public ReponseEntity GetPageGad(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //GadDAL objDAL = new GadDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageGad(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountGad(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<Gad> lstGAM = new List<Gad>();

            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            Gad obj = new Gad();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["ADDRESS"] != DBNull.Value) obj.ADDRESS = _dt.Rows[i]["ADDRESS"].ToString();
            //            if (_dt.Rows[i]["DESCRIPTION"] != DBNull.Value) obj.DESCRIPTION = _dt.Rows[i]["DESCRIPTION"].ToString();
            //            if (_dt.Rows[i]["G_A_D_ID"] != DBNull.Value) obj.G_A_D_ID = int.Parse(_dt.Rows[i]["G_A_D_ID"].ToString());

            //            lstGAM.Add(obj);
            //        }
            //        if (lstGAM != null || lstGAM.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstGAM.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.GetPageGad:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.GetPageGad:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<Gad>(new GadDAL().GetPageGad(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageGadExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Gad>(new GadDAL().GetPageGadExport(where).Tables[0]);
        }

        public ReponseEntity GetAllGad()
        {
            return new ReponseEntityHelper().GetAll<Gad>(new GadDAL().GetAllGad().Tables[0]);
        }

        public ReponseEntity GetByIdGad(int Id)
        {
            return new ReponseEntityHelper().GetByID<Gad>(new GadDAL().GetByIdGad(Id).Tables[0]);
        }

        public ReponseEntity CreateGad(Gad oGAD)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GadDAL objDAL = new GadDAL();
                Int32 Id = objDAL.CreateGad(oGAD);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.CreateGad:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.CreateGad: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateGad(Gad oGAD)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GadDAL objDAL = new GadDAL();
                Int32 Id = objDAL.UpdateGad(oGAD);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.UpdateGad:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.UpdateGad: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteGad(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GadDAL objDAL = new GadDAL();
                objDAL.DeleteGad(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GadRepository.DeleteGad: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
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
    public class CraftTypeRepository
    {
        public ReponseEntity GetPageCraftType(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //CraftTypeDAL objDAL = new CraftTypeDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageCraftType(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountCraftType(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<CraftType> lstCRAFTTYPE = new List<CraftType>();

            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            CraftType obj = new CraftType();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = int.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["MA"] != DBNull.Value) obj.MA = _dt.Rows[i]["MA"].ToString();
            //            if (_dt.Rows[i]["SOHIEU"] != DBNull.Value) obj.SOHIEU = _dt.Rows[i]["SOHIEU"].ToString();
            //            if (_dt.Rows[i]["TAITRONG"] != DBNull.Value) obj.TAITRONG = decimal.Parse(_dt.Rows[i]["TAITRONG"].ToString());

            //            lstCRAFTTYPE.Add(obj);
            //        }
            //        if (lstCRAFTTYPE != null || lstCRAFTTYPE.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstCRAFTTYPE.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.GetPageCraftType:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.GetPageCraftType:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<CraftType>(new CraftTypeDAL().GetPageCraftType(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageCraftTypeExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<CraftType>(new CraftTypeDAL().GetPageCraftTypeExport(where).Tables[0]);
        }

        public ReponseEntity GetAllCraftType()
        {
            return new ReponseEntityHelper().GetAll<CraftType>(new CraftTypeDAL().GetAllCraftType().Tables[0]);
        }

        public ReponseEntity GetByIdCraftType(int Id)
        {
            return new ReponseEntityHelper().GetByID<CraftType>(new CraftTypeDAL().GetByIdCraftType(Id).Tables[0]);
        }

        public ReponseEntity CreateCraftType(CraftType oCRAFTTYPE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CraftTypeDAL objDAL = new CraftTypeDAL();
                Int64 Id = objDAL.CreateCraftType(oCRAFTTYPE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.CreateCraftType:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.CreateCraftType: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateCraftType(CraftType oCRAFTTYPE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CraftTypeDAL objDAL = new CraftTypeDAL();
                Int32 Id = objDAL.UpdateCraftType(oCRAFTTYPE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.UpdateCraftType:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.UpdateCraftType: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteCraftType(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                CraftTypeDAL objDAL = new CraftTypeDAL();
                objDAL.DeleteCraftType(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "CraftTypeRepository.DeleteCraftType: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
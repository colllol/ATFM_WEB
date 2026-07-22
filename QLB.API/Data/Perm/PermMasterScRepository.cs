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
using QLB.BusinessLogic.Perm;

namespace QLB.API.Data
{
    public class PermMasterScRepository
    {

        public ReponseEntity GetPagePermMasterSc(int page_size, int page_index, string where)
        {
            #region oldcode
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //PermMasterScDAL objDAL = new PermMasterScDAL();
            //DataTable _dt = new DataTable();
            //try
            //{
            //    _dt = objDAL.GetPagePermMasterSc(page_size, page_index, where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<PermMasterSc> lstPERMMASTER_SC = new List<PermMasterSc>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            PermMasterSc obj = new PermMasterSc();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = int.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
            //            if (_dt.Rows[i]["AUTHOR_ID"] != DBNull.Value) obj.AUTHOR_ID = _dt.Rows[i]["AUTHOR_ID"].ToString();
            //            if (_dt.Rows[i]["PERMTYPE"] != DBNull.Value) obj.PERMTYPE = _dt.Rows[i]["PERMTYPE"].ToString();
            //            if (_dt.Rows[i]["PERMNBR"] != DBNull.Value) obj.PERMNBR = _dt.Rows[i]["PERMNBR"].ToString();
            //            if (_dt.Rows[i]["VERSION"] != DBNull.Value) obj.VERSION = _dt.Rows[i]["VERSION"].ToString();
            //            if (_dt.Rows[i]["PERMDATE"] != DBNull.Value) obj.PERMDATE = DateTime.Parse(_dt.Rows[i]["PERMDATE"].ToString());
            //            if (_dt.Rows[i]["OPER_ID"] != DBNull.Value) obj.OPER_ID = _dt.Rows[i]["OPER_ID"].ToString();
            //            if (_dt.Rows[i]["REFERENCE"] != DBNull.Value) obj.REFERENCE = _dt.Rows[i]["REFERENCE"].ToString();
            //            if (_dt.Rows[i]["VALIDHOURS"] != DBNull.Value) obj.VALIDHOURS = int.Parse(_dt.Rows[i]["VALIDHOURS"].ToString());
            //            if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
            //            if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
            //            if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());       
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
            //            if (_dt.Rows[i]["AUTHOR_NAME"] != DBNull.Value) obj.AUTHOR_NAME = _dt.Rows[i]["AUTHOR_NAME"].ToString();
            //            if (_dt.Rows[i]["OPER_NAME"] != DBNull.Value) obj.OPER_NAME = _dt.Rows[i]["OPER_NAME"].ToString();

            //            lstPERMMASTER_SC.Add(obj);

            //        }
            //        if (lstPERMMASTER_SC != null || lstPERMMASTER_SC.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstPERMMASTER_SC.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.GetPagePermMasterSc:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.GetPagePermMasterSc:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<PermMasterSc>(new PermMasterScDAL().GetPagePermMasterSc(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPagePermMasterScExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<PermMasterSc>(new PermMasterScDAL().GetPagePermMasterScExport(where).Tables[0]);
        }

        public ReponseEntity GetAllPermMasterSc()
        {
            return new ReponseEntityHelper().GetAll<PermMasterSc>(new PermMasterScDAL().GetAllPermMasterSc().Tables[0]);
        }

        public ReponseEntity GetByIdPermMasterSc(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<PermMasterSc>(new PermMasterScDAL().GetByIdPermMasterSc(Id).Tables[0]);
        }

        public ReponseEntity CreatePermMasterSc(PermMasterSc oPERMMASTER_SC)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterScDAL objDAL = new PermMasterScDAL();
                Int64 Id = objDAL.CreatePermMasterSc(oPERMMASTER_SC);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.CreatePermMasterSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.CreatePermMasterSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdatePermMasterSc(PermMasterSc oPERMMASTER_SC)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterScDAL objDAL = new PermMasterScDAL();
                Int64 Id = objDAL.UpdatePermMasterSc(oPERMMASTER_SC);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.UpdatePermMasterSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.UpdatePermMasterSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeletePermMasterSc(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterScDAL objDAL = new PermMasterScDAL();
                objDAL.DeletePermMasterSc(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterScRepository.DeletePermMasterSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity GetDeleted(string where)
        {
            ReponseEntity oResponse = new ReponseEntity();
            DataTable dt = new PermMasterScDAL().GetDeleted(where);
            if (dt.Rows.Count > 0)
            {
                oResponse.Code = oMessage.CodeSussess;
                oResponse.Message = oMessage.GetDataSussess;
                oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermMasterSc>(dt).ToList<object>();
            }
            else
            {
                oResponse.Code = oMessage.CodeError;
                oResponse.Message = oMessage.GetDataError;
            }
            return oResponse;
        }
    }
}
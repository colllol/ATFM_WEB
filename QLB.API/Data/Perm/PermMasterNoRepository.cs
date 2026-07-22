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
    public class PermMasterNoRepository
    {

        public ReponseEntity GetPagePermMasterNo(int page_size, int page_index, string where)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //PermMasterNoDAL objDAL = new PermMasterNoDAL();
            //DataTable _dt = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPagePermMasterNo(page_size, page_index, where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<PermMasterNo> lstPERMMASTERNO = new List<PermMasterNo>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            PermMasterNo obj = new PermMasterNo();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
            //            if (_dt.Rows[i]["AUTHOR_ID"] != DBNull.Value) obj.AUTHOR_ID = _dt.Rows[i]["AUTHOR_ID"].ToString();
            //            if (_dt.Rows[i]["PERMTYPE"] != DBNull.Value) obj.PERMTYPE = _dt.Rows[i]["PERMTYPE"].ToString();
            //            if (_dt.Rows[i]["PERMNBR"] != DBNull.Value) obj.PERMNBR = _dt.Rows[i]["PERMNBR"].ToString();
            //            if (_dt.Rows[i]["VERSION"] != DBNull.Value) obj.VERSION = _dt.Rows[i]["VERSION"].ToString();
            //            if (_dt.Rows[i]["PERMDATE"] != DBNull.Value) obj.PERMDATE = DateTime.Parse(_dt.Rows[i]["PERMDATE"].ToString());
            //            if (_dt.Rows[i]["OPER_ID"] != DBNull.Value) obj.OPER_ID = _dt.Rows[i]["OPER_ID"].ToString();
            //            if (_dt.Rows[i]["REFERENCE"] != DBNull.Value) obj.REFERENCE = _dt.Rows[i]["REFERENCE"].ToString();
            //            if (_dt.Rows[i]["VALIDHOURS"] != DBNull.Value) obj.VALIDHOURS = Int64.Parse(_dt.Rows[i]["VALIDHOURS"].ToString());
            //            if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
            //            if (_dt.Rows[i]["AUTHOR_NAME"] != DBNull.Value) obj.AUTHOR_NAME = _dt.Rows[i]["AUTHOR_NAME"].ToString();
            //            if (_dt.Rows[i]["OPER_NAME"] != DBNull.Value) obj.OPER_NAME = _dt.Rows[i]["OPER_NAME"].ToString();

            //            lstPERMMASTERNO.Add(obj);

            //        }
            //        if (lstPERMMASTERNO != null || lstPERMMASTERNO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstPERMMASTERNO.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.GetPagePermMasterNo:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.GetPagePermMasterNo:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<PermMasterNo>(new PermMasterNoDAL().GetPagePermMasterNo(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPagePermMasterNoExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<PermMasterNo>(new PermMasterNoDAL().GetPagePermMasterNoExport(where).Tables[0]);
        }

        public ReponseEntity GetAllPermMasterNo()
        {
            return new ReponseEntityHelper().GetAll<PermMasterNo>(new PermMasterNoDAL().GetAllPermMasterNo().Tables[0]);
        }

        public ReponseEntity GetByIdPermMasterNo(Int64 Id)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //PermMasterNoDAL objDAL = new PermMasterNoDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = objDAL.GetByIdPermMasterNo(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<PermMasterNo> lstPERMMASTERNO = new List<PermMasterNo>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            PermMasterNo obj = new PermMasterNo();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
            //            if (_dt.Rows[i]["AUTHOR_ID"] != DBNull.Value) obj.AUTHOR_ID = _dt.Rows[i]["AUTHOR_ID"].ToString();
            //            if (_dt.Rows[i]["PERMTYPE"] != DBNull.Value) obj.PERMTYPE = _dt.Rows[i]["PERMTYPE"].ToString();
            //            if (_dt.Rows[i]["PERMNBR"] != DBNull.Value) obj.PERMNBR = _dt.Rows[i]["PERMNBR"].ToString();
            //            if (_dt.Rows[i]["VERSION"] != DBNull.Value) obj.VERSION = _dt.Rows[i]["VERSION"].ToString();
            //            if (_dt.Rows[i]["PERMDATE"] != DBNull.Value) obj.PERMDATE = DateTime.Parse(_dt.Rows[i]["PERMDATE"].ToString());
            //            if (_dt.Rows[i]["OPER_ID"] != DBNull.Value) obj.OPER_ID = _dt.Rows[i]["OPER_ID"].ToString();
            //            if (_dt.Rows[i]["REFERENCE"] != DBNull.Value) obj.REFERENCE = _dt.Rows[i]["REFERENCE"].ToString();
            //            if (_dt.Rows[i]["VALIDHOURS"] != DBNull.Value) obj.VALIDHOURS = Int64.Parse(_dt.Rows[i]["VALIDHOURS"].ToString());
            //            if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
            //            if (_dt.Rows[i]["AUTHOR_NAME"] != DBNull.Value) obj.AUTHOR_NAME = (_dt.Rows[i]["AUTHOR_NAME"].ToString());
            //            if (_dt.Rows[i]["OPER_NAME"] != DBNull.Value) obj.OPER_NAME = (_dt.Rows[i]["OPER_NAME"].ToString());

            //            lstPERMMASTERNO.Add(obj);
            //        }
            //        if (lstPERMMASTERNO != null || lstPERMMASTERNO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            //oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstPERMMASTERNO.ToList<object>();
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
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.GetByIdPermMasterNo:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<PermMasterNo>(new PermMasterNoDAL().GetByIdPermMasterNo(Id).Tables[0]);
        }

        //Create  
        public ReponseEntity CreatePermMasterNo(PermMasterNo oPERMMASTERNO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterNoDAL objDAL = new PermMasterNoDAL();
                var Id = objDAL.CreatePermMasterNo(oPERMMASTERNO);
                oResponse.Code = Id.ToString();

                if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.CreatePermMasterNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.CreatePermMasterNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdatePermMasterNo(PermMasterNo oPERMMASTERNO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterNoDAL objDAL = new PermMasterNoDAL();
                //Int32 Id = objDAL.UpdatePermMasterNo(oPERMMASTERNO);
                Int64 Id = objDAL.UpdatePermMasterNo(oPERMMASTERNO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.UpdatePermMasterNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.UpdatePermMasterNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeletePermMasterNo(Int64 Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermMasterNoDAL objDAL = new PermMasterNoDAL();
                objDAL.DeletePermMasterNo(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermMasterNoRepository.DeletePermMasterNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity GetDeleted(string where)
        {
            ReponseEntity oResponse = new ReponseEntity();
            DataTable dt = new PermMasterNoDAL().GetDeleted(where);
            if (dt.Rows.Count > 0)
            {
                oResponse.Code = oMessage.CodeSussess;
                oResponse.Message = oMessage.GetDataSussess;
                oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermMasterNo>(dt).ToList<object>();
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
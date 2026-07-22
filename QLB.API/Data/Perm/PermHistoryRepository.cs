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

namespace QLB.API.Data.Perm
{
    public class PermHistoryRepository
    {
        public ReponseEntity GetByIdPermMasterNoBk(Int64 Id)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //PermDetailScDAL objDAL = new PermDetailScDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = new PermHistoryDAL().GetByIdPermMasterNoBk(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        oResponse.Code = "00";
            //        //oResponse.Message = "Lấy dữ liệu thành công";
            //        oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermMasterNo_HIS>(_dt).ToList<object>();
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetByIdPermMasterNoBk:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<PermMasterNo_HIS>(new PermHistoryDAL().GetByIdPermMasterNoBk(Id).Tables[0]);
        }

        public ReponseEntity GetByIdPermMasterScBk(Int64 Id)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //PermDetailScDAL objDAL = new PermDetailScDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = new PermHistoryDAL().GetByIdPermMasterScBk(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        oResponse.Code = "00";
            //        //oResponse.Message = "Lấy dữ liệu thành công";
            //        oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermMasterSc_HIS>(_dt).ToList<object>();
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetByIdPermMasterNoBk:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<PermMasterSc_HIS>(new PermHistoryDAL().GetByIdPermMasterScBk(Id).Tables[0]);
        }
        public ReponseEntity GetByIdPermDetailNoBk(Int64 Id)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //PermDetailScDAL objDAL = new PermDetailScDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = new PermHistoryDAL().GetByIdPermDetailNoBk(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        oResponse.Code = "00";
            //        //oResponse.Message = "Lấy dữ liệu thành công";                    
            //        oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermDetailNo_HIS>(_dt).ToList<object>();
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetByIdPermMasterNoBk:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<PermDetailNo_HIS>(new PermHistoryDAL().GetByIdPermDetailNoBk(Id).Tables[0]);
        }
        public ReponseEntity GetByIdPermDetailScBk(Int64 Id)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //PermDetailScDAL objDAL = new PermDetailScDAL();

            //DataTable _dt = new DataTable();

            //try
            //{
            //    _dt = new PermHistoryDAL().GetByIdPermDetailScBk(Id).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        oResponse.Code = "00";
            //        //oResponse.Message = "Lấy dữ liệu thành công";
            //        oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermDetailSc_HIS>(_dt).ToList<object>();
            //    }
            //    else
            //    {
            //        oResponse.Code = "-01";
            //        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetByIdPermMasterNoBk:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}

            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetByID<PermDetailSc_HIS>(new PermHistoryDAL().GetByIdPermDetailScBk(Id).Tables[0]);
        }
        public ReponseEntity GetPagePermMasterScBk(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<PermMasterSc>(new PermHistoryDAL().GetPagePermMasterScBk(page_size, page_index, where).Tables[0]);
        }
        public ReponseEntity GetPagePermMasterNoBk(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<PermMasterNo>(new PermHistoryDAL().GetPagePermMasterNoBk(page_size, page_index, where).Tables[0]);
        }
        public ReponseEntity GetPagePermDetailScBk(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<PermDetailSc>(new PermHistoryDAL().GetPagePermDetailScBk(page_size, page_index, where).Tables[0]);
        }
        public ReponseEntity GetPagePermDetailNoBk(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<PermDetailNo>(new PermHistoryDAL().GetPagePermDetailNoBk(page_size, page_index, where).Tables[0]);
        }
    }
}
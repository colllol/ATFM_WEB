using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info.Perm;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic.Perm;

namespace QLB.API.Data.Perm
{
    public class OperRepository
    {
        public ReponseEntity GetPageOper(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<Oper>(new OperDAL().GetPageOper(page_size, page_index, where).Tables[0]);
        }
        public ReponseEntity GetPageOperExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<Oper>(new OperDAL().GetPageOperExport(where).Tables[0]);
        }
        public ReponseEntity GetAllOper()
        {
            return new ReponseEntityHelper().GetAll<Oper>(new OperDAL().GetAllOper().Tables[0]);
        }
        public ReponseEntity GetByIdOper(Int64 Id)
        {
            return new ReponseEntityHelper().GetByID<Oper>(new OperDAL().GetByIdOper(Id).Tables[0]);
        }

        public ReponseEntity CreateOper(Oper oOPER)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutput = string.Empty;

            //ReponseEntity oResponse = new ReponseEntity();

            //try
            //{
            //    OperDAL objDAL = new OperDAL();
            //    Int64 Id = objDAL.CreateOper(oOPER);
            //    oResponse.Code = Id.ToString();
            //    if (Id == -1)
            //    {
            //        oResponse.Code = "-1";
            //        oResponse.Message = "Dữ liệu đã tồn tại";
            //        oResponse.Value = -1;
            //    }
            //    else if (Id == -99)
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "OperRepository.CreateOper:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //    }
            //    else
            //    { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "OperRepository.CreateOper: " + ex.Message);
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //}
            //return oResponse;
            #endregion
            return new ReponseEntityHelper().Insert<Oper, OperDAL>(oOPER, "CreateOper");
        }

        public ReponseEntity UpdateOper(Oper oOPER)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutput = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();

            //try
            //{
            //    OperDAL objDAL = new OperDAL();
            //    Int64 Id = objDAL.UpdateOper(oOPER);
            //    oResponse.Code = Id.ToString();
            //    if (Id == -1)
            //    {
            //        oResponse.Code = "-1";
            //        oResponse.Message = "Dữ liệu đã tồn tại";
            //        oResponse.Value = -1;
            //    }
            //    else if (Id == -99)
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "OperRepository.UpdateOper:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //    }
            //    else
            //    { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "OperRepository.UpdateOper: " + ex.Message);
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //}
            //return oResponse;
            #endregion
            return new ReponseEntityHelper().Update<Oper, OperDAL>(oOPER, "UpdateOper");
        }

        public ReponseEntity DeleteOper(Int64 Id)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();

            //try
            //{
            //    OperDAL objDAL = new OperDAL();
            //    objDAL.DeleteOper(Id);
            //    oResponse.Code = Id.ToString();
            //    oResponse.Message = "Cập nhật dữ liệu thành công";
            //}

            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "OperRepository.DeleteOper: " + ex.Message);
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            //}
            //return oResponse;
            #endregion
            return new ReponseEntityHelper().Delete<Oper, OperDAL>(Id, "DeleteOper");
        }
    }
}
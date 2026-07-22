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
    public class RouteListRepository
    {
        public ReponseEntity GetPageRouteList(int page_size, int page_index, string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //RouteListDAL objDAL = new RouteListDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageRouteList(page_size, page_index, where).Tables[0];
            //    _dt1 = objDAL.GetCountRouteList(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<RouteList> lstROUTELIST = new List<RouteList>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            RouteList obj = new RouteList();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["ROUTE_ID"] != DBNull.Value) obj.ROUTE_ID = int.Parse(_dt.Rows[i]["ROUTE_ID"].ToString());
            //            if (_dt.Rows[i]["ROUTE_NAME"] != DBNull.Value) obj.ROUTE_NAME = _dt.Rows[i]["ROUTE_NAME"].ToString();
            //            if (_dt.Rows[i]["DESCRIPTION"] != DBNull.Value) obj.DESCRIPTION = _dt.Rows[i]["DESCRIPTION"].ToString();
            //            if (_dt.Rows[i]["IS_OVERSEA"] != DBNull.Value) obj.IS_OVERSEA = _dt.Rows[i]["IS_OVERSEA"].ToString();

            //            lstROUTELIST.Add(obj);

            //        }
            //        if (lstROUTELIST != null || lstROUTELIST.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstROUTELIST.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.GetPageRouteList:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.GetPageRouteList:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<RouteList>(new RouteListDAL().GetPageRouteList(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageRouteListExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<RouteList>(new RouteListDAL().GetPageRouteListExport(where).Tables[0]);
        }      

        public ReponseEntity GetAllRouteList()
        {
            return new ReponseEntityHelper().GetAll<RouteList>(new RouteListDAL().GetAllRouteList().Tables[0]);
        }

        public ReponseEntity GetByIdRouteList(int Id)
        {
            return new ReponseEntityHelper().GetByID<RouteList>(new RouteListDAL().GetByIdRouteList(Id).Tables[0]);
        }
        public ReponseEntity GetByIdRoutePoint(int Id)
        {
            return new ReponseEntityHelper().GetByID<RouteList>(new RouteListDAL().GetByIdRoutePoint(Id).Tables[0]);
        }
        public ReponseEntity CreateRouteList(RouteList oROUTELIST)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                RouteListDAL objDAL = new RouteListDAL();
                Int32 Id = objDAL.CreateRouteList(oROUTELIST);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.CreateRouteList:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.CreateRouteList: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateRouteList(RouteList oROUTELIST)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                RouteListDAL objDAL = new RouteListDAL();
                Int32 Id = objDAL.UpdateRouteList(oROUTELIST);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.UpdateRouteList:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.UpdateRouteList: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteRouteList(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                RouteListDAL objDAL = new RouteListDAL();
                objDAL.DeleteRouteList(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "RouteListRepository.DeleteRouteList: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
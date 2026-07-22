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
    public class AirportRouteRepository
    {
        public ReponseEntity GetPageAirportRoute(int page_size, int page_index,string where)
        {
            #region Code Old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //AirportRouteDAL objDAL = new AirportRouteDAL();
            //DataTable _dt = new DataTable();
            //DataTable _dt1 = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPageAirportRoute(page_size, page_index,where).Tables[0];
            //    _dt1 = objDAL.GetCountAirportRoute(where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<AirportRoute> lstAIRPORTROUTE = new List<AirportRoute>();

            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            AirportRoute obj = new AirportRoute();
            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["ROUTE"] != DBNull.Value) obj.ROUTE = _dt.Rows[i]["ROUTE"].ToString();
            //            if (_dt.Rows[i]["IS_OVERSEA"] != DBNull.Value) obj.IS_OVERSEA = _dt.Rows[i]["IS_OVERSEA"].ToString();
            //            if (_dt.Rows[i]["DOMESTIC"] != DBNull.Value) obj.DOMESTIC = int.Parse(_dt.Rows[i]["DOMESTIC"].ToString());
            //            if (_dt.Rows[i]["INTERNATIONAL"] != DBNull.Value) obj.INTERNATIONAL = int.Parse(_dt.Rows[i]["INTERNATIONAL"].ToString());
            //            if (_dt.Rows[i]["IS_DOMESTIC"] != DBNull.Value) obj.IS_DOMESTIC = _dt.Rows[i]["IS_DOMESTIC"].ToString();
            //            if (_dt.Rows[i]["SUMMARY"] != DBNull.Value) obj.SUMMARY = int.Parse(_dt.Rows[i]["SUMMARY"].ToString());

            //            lstAIRPORTROUTE.Add(obj);
            //        }
            //        if (lstAIRPORTROUTE != null || lstAIRPORTROUTE.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstAIRPORTROUTE.ToList<object>();
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
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.GetPageAirportRoute:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.GetPageAirportRoute:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<AirportRoute>(new AirportRouteDAL().GetPageAirportRoute(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageAirportRouteExport(string where)
        {
            return new ReponseEntityHelper().GetPageExport<AirportRoute>(new AirportRouteDAL().GetPageAirportRouteExport(where).Tables[0]);
        }

        public ReponseEntity GetAllAirportRoute()
        {
            return new ReponseEntityHelper().GetAll<AirportRoute>(new AirportRouteDAL().GetAllAirportRoute().Tables[0]);
        }

        public ReponseEntity GetByIdAirportRoute(int Id)
        {
            return new ReponseEntityHelper().GetByID<AirportRoute>(new AirportRouteDAL().GetByIdAirportRoute(Id).Tables[0]);
        }

        public ReponseEntity CreateAirportRoute(AirportRoute oAIRPORTROUTE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirportRouteDAL objDAL = new AirportRouteDAL();
                Int32 Id = objDAL.CreateAirportRoute(oAIRPORTROUTE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.CreateAirportRoute:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.CreateAirportRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity UpdateAirportRoute(AirportRoute oAIRPORTROUTE)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirportRouteDAL objDAL = new AirportRouteDAL();
                Int32 Id = objDAL.UpdateAirportRoute(oAIRPORTROUTE);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.UpdateAirportRoute:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.UpdateAirportRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteAirportRoute(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AirportRouteDAL objDAL = new AirportRouteDAL();
                objDAL.DeleteAirportRoute(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AirportRouteRepository.DeleteAirportRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
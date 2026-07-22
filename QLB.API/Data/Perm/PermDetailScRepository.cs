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
    public class PermDetailScRepository
    {
        //Get Page

        public ReponseEntity GetPagePermDetailSc(int page_size, int page_index, string where)
        {
            #region old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //PermDetailScDAL objDAL = new PermDetailScDAL();
            //DataTable _dt = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPagePermDetailSc(page_size, page_index, where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<PermDetailSc> lstPERMDETAILSC = new List<PermDetailSc>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            PermDetailSc obj = new PermDetailSc();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
            //            if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
            //            if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
            //            if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
            //            if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
            //            if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
            //            if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
            //            if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
            //            if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
            //            if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
            //            if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
            //            if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
            //            if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
            //            if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
            //            if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
            //            if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
            //            if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
            //            if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();

            //            lstPERMDETAILSC.Add(obj);

            //        }
            //        if (lstPERMDETAILSC != null || lstPERMDETAILSC.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstPERMDETAILSC.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetPagePermDetailSc:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetPagePermDetailSc:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            DataTable dt = new PermDetailScDAL().GetPagePermDetailSc(page_size, page_index, where).Tables[0];
            return new ReponseEntityHelper().GetPage<PermDetailSc>(dt);
        }
        //Get Page Export

        public ReponseEntity GetPagePermDetailScExport(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailScDAL objDAL = new PermDetailScDAL();
            DataTable _dt = new DataTable();
            DataTable _dt1 = new DataTable();
            try
            {

                _dt = objDAL.GetPagePermDetailScExport(where).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailSc> lstPERMDETAILSC = new List<PermDetailSc>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailSc obj = new PermDetailSc();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
                        if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
                        if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
                        if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
                        if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
                        if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
                        if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
                        if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        //if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();

                        lstPERMDETAILSC.Add(obj);

                    }
                    if (lstPERMDETAILSC != null || lstPERMDETAILSC.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILSC.ToList<object>();
                        //oResponse.Value = _dt1.Rows[0][0];
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetPagePermDetailSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetPagePermDetailSc:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }
        //GetCount
        public ReponseEntity GetCountPermDetailSc(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailScDAL objDAL = new PermDetailScDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetCountPermDetailSc(where).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailSc> lstPERMDETAILSC = new List<PermDetailSc>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailSc obj = new PermDetailSc();

                        if (_dt.Rows[i][0] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i][0].ToString());


                        lstPERMDETAILSC.Add(obj);
                    }
                    if (lstPERMDETAILSC != null || lstPERMDETAILSC.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILSC.ToList<object>();

                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetCountPermDetailSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetCountPermDetailSc:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }


        //Get ALL
        public ReponseEntity GetAllPermDetailSc()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailScDAL objDAL = new PermDetailScDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllPermDetailSc().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailSc> lstPERMDETAILSC = new List<PermDetailSc>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailSc obj = new PermDetailSc();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
                        if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
                        if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
                        if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
                        if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
                        if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
                        if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
                        if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();

                        lstPERMDETAILSC.Add(obj);
                    }
                    if (lstPERMDETAILSC != null || lstPERMDETAILSC.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILSC.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetAllPermDetailSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetAllPermDetailSc:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdPermDetailSc(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            PermDetailScDAL objDAL = new PermDetailScDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetByIdPermDetailSc(Id).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailSc> lstPERMDETAILSC = new List<PermDetailSc>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailSc obj = new PermDetailSc();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["DAY1"] != DBNull.Value) obj.DAY1 = _dt.Rows[i]["DAY1"].ToString();
                        if (_dt.Rows[i]["DAY2"] != DBNull.Value) obj.DAY2 = _dt.Rows[i]["DAY2"].ToString();
                        if (_dt.Rows[i]["DAY3"] != DBNull.Value) obj.DAY3 = _dt.Rows[i]["DAY3"].ToString();
                        if (_dt.Rows[i]["DAY4"] != DBNull.Value) obj.DAY4 = _dt.Rows[i]["DAY4"].ToString();
                        if (_dt.Rows[i]["DAY5"] != DBNull.Value) obj.DAY5 = _dt.Rows[i]["DAY5"].ToString();
                        if (_dt.Rows[i]["DAY6"] != DBNull.Value) obj.DAY6 = _dt.Rows[i]["DAY6"].ToString();
                        if (_dt.Rows[i]["DAY7"] != DBNull.Value) obj.DAY7 = _dt.Rows[i]["DAY7"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["BEGINDATE"] != DBNull.Value) obj.BEGINDATE = DateTime.Parse(_dt.Rows[i]["BEGINDATE"].ToString());
                        if (_dt.Rows[i]["ENDDATE"] != DBNull.Value) obj.ENDDATE = DateTime.Parse(_dt.Rows[i]["ENDDATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();

                        lstPERMDETAILSC.Add(obj);
                    }
                    if (lstPERMDETAILSC != null || lstPERMDETAILSC.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILSC.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.GetByIdPermDetailSc:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;
        }

        //Create  
        public ReponseEntity CreatePermDetailSc(PermDetailSc oPERMDETAILSC)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailScDAL objDAL = new PermDetailScDAL();
                Int64 Id = objDAL.CreatePermDetailSc(oPERMDETAILSC);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.CreatePermDetailSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.CreatePermDetailSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdatePermDetailSc(PermDetailSc oPERMDETAILSC)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailScDAL objDAL = new PermDetailScDAL();
                Int64 Id = objDAL.UpdatePermDetailSc(oPERMDETAILSC);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.UpdatePermDetailSc:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.UpdatePermDetailSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdatePermDetailSc_GenBack(PermDetailSc oPERMDETAILSC)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailScDAL objDAL = new PermDetailScDAL();
                Int64 Id = objDAL.UpdatePermDetailSc_GenBack(oPERMDETAILSC);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.UpdatePermDetailSc_GenBack:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.UpdatePermDetailSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }


        //Delete

        public ReponseEntity DeletePermDetailSc(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailScDAL objDAL = new PermDetailScDAL();
                objDAL.DeletePermDetailSc(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailScRepository.DeletePermDetailSc: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity GetDeleted(string where)
        {
            ReponseEntity oResponse = new ReponseEntity();
            DataTable dt = new PermDetailScDAL().GetDeleted(where);
            if (dt.Rows.Count > 0)
            {
                oResponse.Code = oMessage.CodeSussess;
                oResponse.Message = oMessage.GetDataSussess;
                oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermDetailSc>(dt).ToList<object>();
            }
            else
            {
                oResponse.Code = oMessage.CodeError;
                oResponse.Message = oMessage.GetDataError;
            }
            return oResponse;
        }
        //get by search class
        public ReponseEntity GetBySearch(PermDetailSc_Search obj)
        {
            return new ReponseEntityHelper().GetAll<PermDetailSc>(new PermDetailScDAL().GetBySearch(obj));
        }
    }
}
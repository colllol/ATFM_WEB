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
    public class PermDetailNoRepository
    {
        //Get Page

        public ReponseEntity GetPagePermDetailNo(int page_size, int page_index, string where)
        {
            #region code old
            //string sReturn = string.Empty;
            //string sOutPut = string.Empty;
            //ReponseEntity oResponse = new ReponseEntity();
            //PermDetailNoDAL objDAL = new PermDetailNoDAL();
            //DataTable _dt = new DataTable();
            //try
            //{

            //    _dt = objDAL.GetPagePermDetailNo(page_size, page_index, where).Tables[0];
            //    if (_dt.Rows.Count > 0)
            //    {
            //        List<PermDetailNo> lstPERMDETAILNO = new List<PermDetailNo>();
            //        for (int i = 0; i < _dt.Rows.Count; i++)
            //        {
            //            PermDetailNo obj = new PermDetailNo();

            //            if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
            //            if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
            //            if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
            //            if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
            //            if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
            //            if (_dt.Rows[i]["DAYSFLIGHT"] != DBNull.Value) obj.DAYSFLIGHT = _dt.Rows[i]["DAYSFLIGHT"].ToString();
            //            if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
            //            if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
            //            if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
            //            if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
            //            if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
            //            if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
            //            if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
            //            if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
            //            if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
            //            if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
            //            if (_dt.Rows[i]["MAX_DATE"] != DBNull.Value) obj.MAX_DATE = DateTime.Parse(_dt.Rows[i]["MAX_DATE"].ToString());
            //            if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
            //            if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
            //            if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
            //            if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
            //            if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();
            //            if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();

            //            lstPERMDETAILNO.Add(obj);

            //        }
            //        if (lstPERMDETAILNO != null || lstPERMDETAILNO.Count > 0)
            //        {
            //            oResponse.Code = "00";
            //            oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
            //            oResponse.Message = "Lấy dữ liệu thành công";
            //            oResponse.ListValue = lstPERMDETAILNO.ToList<object>();
            //        }
            //        else
            //        {
            //            oResponse.Code = "-01";
            //            oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
            //        }
            //    }
            //    else
            //    {
            //        LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetPagePermDetailNo:");
            //        oResponse.Code = "-99";
            //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetPagePermDetailNo:");
            //    oResponse.Code = "-99";
            //    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            //}


            //return oResponse;
            #endregion
            return new ReponseEntityHelper().GetPage<PermDetailNo>(new PermDetailNoDAL().GetPagePermDetailNo(page_size, page_index, where).Tables[0]);
        }
        //Get Page Export

        public ReponseEntity GetPagePermDetailNoExport(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailNoDAL objDAL = new PermDetailNoDAL();
            DataTable _dt = new DataTable();
            DataTable _dt1 = new DataTable();
            try
            {
                _dt = objDAL.GetPagePermDetailNoExport(where).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailNo> lstPERMDETAILNO = new List<PermDetailNo>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailNo obj = new PermDetailNo();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["DAYSFLIGHT"] != DBNull.Value) obj.DAYSFLIGHT = _dt.Rows[i]["DAYSFLIGHT"].ToString();
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["MAX_DATE"] != DBNull.Value) obj.MAX_DATE = DateTime.Parse(_dt.Rows[i]["MAX_DATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();
                        //if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();

                        lstPERMDETAILNO.Add(obj);

                    }
                    if (lstPERMDETAILNO != null || lstPERMDETAILNO.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILNO.ToList<object>();
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
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetPagePermDetailNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetPagePermDetailNo:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }
        //GetCount
        public ReponseEntity GetCountPermDetailNo(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailNoDAL objDAL = new PermDetailNoDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetCountPermDetailNo(where).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailNo> lstPERMDETAILNO = new List<PermDetailNo>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailNo obj = new PermDetailNo();

                        if (_dt.Rows[i][0] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i][0].ToString());


                        lstPERMDETAILNO.Add(obj);
                    }
                    if (lstPERMDETAILNO != null || lstPERMDETAILNO.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILNO.ToList<object>();

                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetCountPermDetailNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetCountPermDetailNo:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }


        //Get ALL
        public ReponseEntity GetAllPermDetailNo()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            PermDetailNoDAL objDAL = new PermDetailNoDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllPermDetailNo().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailNo> lstPERMDETAILNO = new List<PermDetailNo>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailNo obj = new PermDetailNo();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["DAYSFLIGHT"] != DBNull.Value) obj.DAYSFLIGHT = _dt.Rows[i]["DAYSFLIGHT"].ToString();
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["MAX_DATE"] != DBNull.Value) obj.MAX_DATE = DateTime.Parse(_dt.Rows[i]["MAX_DATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();
                        if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();

                        lstPERMDETAILNO.Add(obj);
                    }
                    if (lstPERMDETAILNO != null || lstPERMDETAILNO.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILNO.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetAllPermDetailNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetAllPermDetailNo:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdPermDetailNo(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            PermDetailNoDAL objDAL = new PermDetailNoDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetByIdPermDetailNo(Id).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<PermDetailNo> lstPERMDETAILNO = new List<PermDetailNo>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        PermDetailNo obj = new PermDetailNo();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = Int64.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["FLIGHT_PK"] != DBNull.Value) obj.FLIGHT_PK = Int64.Parse(_dt.Rows[i]["FLIGHT_PK"].ToString());
                        if (_dt.Rows[i]["PERM_ID"] != DBNull.Value) obj.PERM_ID = Int64.Parse(_dt.Rows[i]["PERM_ID"].ToString());
                        if (_dt.Rows[i]["CRAFT_ID"] != DBNull.Value) obj.CRAFT_ID = Int64.Parse(_dt.Rows[i]["CRAFT_ID"].ToString());
                        if (_dt.Rows[i]["MTOW"] != DBNull.Value) obj.MTOW = int.Parse(_dt.Rows[i]["MTOW"].ToString());
                        if (_dt.Rows[i]["DAYSFLIGHT"] != DBNull.Value) obj.DAYSFLIGHT = _dt.Rows[i]["DAYSFLIGHT"].ToString();
                        if (_dt.Rows[i]["FLIGHTNBR"] != DBNull.Value) obj.FLIGHTNBR = _dt.Rows[i]["FLIGHTNBR"].ToString();
                        if (_dt.Rows[i]["REGISTRATION"] != DBNull.Value) obj.REGISTRATION = _dt.Rows[i]["REGISTRATION"].ToString();
                        if (_dt.Rows[i]["FROM_AIRP"] != DBNull.Value) obj.FROM_AIRP = _dt.Rows[i]["FROM_AIRP"].ToString();
                        if (_dt.Rows[i]["TO_AIRP"] != DBNull.Value) obj.TO_AIRP = _dt.Rows[i]["TO_AIRP"].ToString();
                        if (_dt.Rows[i]["ETD"] != DBNull.Value) obj.ETD = _dt.Rows[i]["ETD"].ToString();
                        if (_dt.Rows[i]["ETA"] != DBNull.Value) obj.ETA = _dt.Rows[i]["ETA"].ToString();
                        if (_dt.Rows[i]["VIA"] != DBNull.Value) obj.VIA = _dt.Rows[i]["VIA"].ToString();
                        if (_dt.Rows[i]["STATUS"] != DBNull.Value) obj.STATUS = _dt.Rows[i]["STATUS"].ToString();
                        if (_dt.Rows[i]["LASTMODIFY"] != DBNull.Value) obj.LASTMODIFY = DateTime.Parse(_dt.Rows[i]["LASTMODIFY"].ToString());
                        if (_dt.Rows[i]["LASTUSER"] != DBNull.Value) obj.LASTUSER = _dt.Rows[i]["LASTUSER"].ToString();
                        if (_dt.Rows[i]["PURPOSE_ID"] != DBNull.Value) obj.PURPOSE_ID = _dt.Rows[i]["PURPOSE_ID"].ToString();
                        if (_dt.Rows[i]["MAX_DATE"] != DBNull.Value) obj.MAX_DATE = DateTime.Parse(_dt.Rows[i]["MAX_DATE"].ToString());
                        if (_dt.Rows[i]["REMARK"] != DBNull.Value) obj.REMARK = _dt.Rows[i]["REMARK"].ToString();
                        if (_dt.Rows[i]["PURPOSE_NAME"] != DBNull.Value) obj.PURPOSE_NAME = _dt.Rows[i]["PURPOSE_NAME"].ToString();
                        if (_dt.Rows[i]["CRAFT_NAME"] != DBNull.Value) obj.CRAFT_NAME = _dt.Rows[i]["CRAFT_NAME"].ToString();
                        if (_dt.Rows[i]["FROM_NAME"] != DBNull.Value) obj.FROM_NAME = _dt.Rows[i]["FROM_NAME"].ToString();
                        if (_dt.Rows[i]["TO_NAME"] != DBNull.Value) obj.TO_NAME = _dt.Rows[i]["TO_NAME"].ToString();
                        if (_dt.Rows[i]["PERMNBR_ID"] != DBNull.Value) obj.PERMNBR_ID = _dt.Rows[i]["PERMNBR_ID"].ToString();

                        lstPERMDETAILNO.Add(obj);
                    }
                    if (lstPERMDETAILNO != null || lstPERMDETAILNO.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstPERMDETAILNO.ToList<object>();
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
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.GetByIdPermDetailNo:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;
        }

        //Create  
        public ReponseEntity CreatePermDetailNo(PermDetailNo oPERMDETAILNO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailNoDAL objDAL = new PermDetailNoDAL();
                Int64 Id = objDAL.CreatePermDetailNo(oPERMDETAILNO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.CreatePermDetailNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.CreatePermDetailNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdatePermDetailNo(PermDetailNo oPERMDETAILNO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailNoDAL objDAL = new PermDetailNoDAL();
                Int64 Id = objDAL.UpdatePermDetailNo(oPERMDETAILNO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.UpdatePermDetailNo:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.UpdatePermDetailNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        //Update
        public ReponseEntity UpdatePermDetailNo_GenBack(PermDetailNo oPERMDETAILNO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailNoDAL objDAL = new PermDetailNoDAL();
                Int64 Id = objDAL.UpdatePermDetailNo_GenBack(oPERMDETAILNO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.UpdatePermDetailNo_GenBack:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.UpdatePermDetailNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        //Delete

        public ReponseEntity DeletePermDetailNo(Int64 Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                PermDetailNoDAL objDAL = new PermDetailNoDAL();
                objDAL.DeletePermDetailNo(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "PermDetailNoRepository.DeletePermDetailNo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        public ReponseEntity GetDeleted(string where)
        {
            ReponseEntity oResponse = new ReponseEntity();
            DataTable dt = new PermDetailNoDAL().GetDeleted(where);
            if (dt.Rows.Count > 0)
            {
                oResponse.Code = oMessage.CodeSussess;
                oResponse.Message = oMessage.GetDataSussess;
                oResponse.ListValue = new clsConvertTableToObject().ConvertTo<PermDetailNo>(dt).ToList<object>();
            }
            else
            {
                oResponse.Code = oMessage.CodeError;
                oResponse.Message = oMessage.GetDataError;
            }
            return oResponse;
        }
        public ReponseEntity GetBySearch(PermDetailNo_Search obj)
        {
            return new ReponseEntityHelper().GetPage<PermDetailNo>(new PermDetailNoDAL().GetBySearch(obj));
        }
    }
}
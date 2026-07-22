using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class MenusRepository
    {
        public ReponseEntity GetPageMenus(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<Menus>(new MenusDAL().GetPageMenus(page_size, page_index,where).Tables[0]);
        }
        #region Get Page Old
        //public ReponseEntity GetPageMenus(int page_size, int page_index)
        //{
        //    string sReturn = string.Empty;
        //    string sOutPut = string.Empty;
        //    ReponseEntity oResponse = new ReponseEntity();
        //    MenusDAL objDAL = new MenusDAL();
        //    DataTable _dt = new DataTable();
        //    try
        //    {

        //        _dt = objDAL.GetPageMenus(page_size, page_index).Tables[0];

        //        if (_dt.Rows.Count > 0)
        //        {
        //            List<Menus> lstMENUS = new List<Menus>();
        //            for (int i = 0; i < _dt.Rows.Count; i++)
        //            {
        //                Menus obj = new Menus();

        //                if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
        //                if (_dt.Rows[i]["MENUNAME"] != DBNull.Value) obj.MenuName = _dt.Rows[i]["MENUNAME"].ToString();
        //                if (_dt.Rows[i]["MENUDESC"] != DBNull.Value) obj.MenuDesc = _dt.Rows[i]["MENUDESC"].ToString();
        //                if (_dt.Rows[i]["MENUORDER"] != DBNull.Value) obj.MenuOrder = int.Parse(_dt.Rows[i]["MENUORDER"].ToString());
        //                if (_dt.Rows[i]["PARRENTID"] != DBNull.Value) obj.ParrentID = int.Parse(_dt.Rows[i]["PARRENTID"].ToString());
        //                if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
        //                if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
        //                if (_dt.Rows[i]["MENUURL"] != DBNull.Value) obj.MenuURL = _dt.Rows[i]["MENUURL"].ToString();
        //                if (_dt.Rows[i]["MENUICON"] != DBNull.Value) obj.MenuIcon = _dt.Rows[i]["MENUICON"].ToString();
        //                if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
        //                if (_dt.Rows[i]["USERMODIFY"] != DBNull.Value) obj.UserModify = int.Parse(_dt.Rows[i]["USERMODIFY"].ToString());
        //                if (_dt.Rows[i]["ISDISPLAY"] != DBNull.Value) obj.isDisplay = int.Parse(_dt.Rows[i]["ISDISPLAY"].ToString());
        //                if (_dt.Rows[i]["ACTIVESYNC"] != DBNull.Value) obj.ActiveSync = int.Parse(_dt.Rows[i]["ACTIVESYNC"].ToString());
        //                if (_dt.Rows[i]["ACTIVESYNCIMAGES"] != DBNull.Value) obj.ActiveSyncImages = int.Parse(_dt.Rows[i]["ACTIVESYNCIMAGES"].ToString());

        //                lstMENUS.Add(obj);
        //            }
        //            if (lstMENUS != null || lstMENUS.Count > 0)
        //            {
        //                oResponse.Code = "00";
        //                oResponse.Message = "Lấy dữ liệu thành công";
        //                oResponse.ListValue = lstMENUS.ToList<object>();
        //            }
        //            else
        //            {
        //                oResponse.Code = "-01";
        //                oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
        //            }
        //        }
        //        else
        //        {
        //            LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.GetPageMenus:");
        //            oResponse.Code = "-99";
        //            oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.GetPageMenus:");
        //        oResponse.Code = "-99";
        //        oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
        //    }


        //    return oResponse;
        //}

        #endregion

        //Get ALL
        public ReponseEntity GetAllMenus()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            MenusDAL objDAL = new MenusDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllMenus().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Menus> lstMENUS = new List<Menus>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Menus obj = new Menus();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["MENUNAME"] != DBNull.Value) obj.MenuName = _dt.Rows[i]["MENUNAME"].ToString();
                        if (_dt.Rows[i]["MENUDESC"] != DBNull.Value) obj.MenuDesc = _dt.Rows[i]["MENUDESC"].ToString();
                        if (_dt.Rows[i]["MENUORDER"] != DBNull.Value) obj.MenuOrder = int.Parse(_dt.Rows[i]["MENUORDER"].ToString());
                        if (_dt.Rows[i]["PARRENTID"] != DBNull.Value) obj.ParrentID = int.Parse(_dt.Rows[i]["PARRENTID"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["MENUURL"] != DBNull.Value) obj.MenuURL = _dt.Rows[i]["MENUURL"].ToString();
                        if (_dt.Rows[i]["MENUICON"] != DBNull.Value) obj.MenuIcon = _dt.Rows[i]["MENUICON"].ToString();
                        if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
                        if (_dt.Rows[i]["USERMODIFY"] != DBNull.Value) obj.UserModify = int.Parse(_dt.Rows[i]["USERMODIFY"].ToString());
                        if (_dt.Rows[i]["ISDISPLAY"] != DBNull.Value) obj.isDisplay = int.Parse(_dt.Rows[i]["ISDISPLAY"].ToString());
                        if (_dt.Rows[i]["ACTIVESYNC"] != DBNull.Value) obj.ActiveSync = int.Parse(_dt.Rows[i]["ACTIVESYNC"].ToString());
                        if (_dt.Rows[i]["ACTIVESYNCIMAGES"] != DBNull.Value) obj.ActiveSyncImages = int.Parse(_dt.Rows[i]["ACTIVESYNCIMAGES"].ToString());


                        lstMENUS.Add(obj);
                    }
                    if (lstMENUS != null || lstMENUS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstMENUS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.GetAllMenus:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, ex.ToString()+ "MenusRepository.GetAllMenus:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdMenus(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            MenusDAL objDAL = new MenusDAL();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDAL.GetByIdMenus(ID).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Menus> lstMENUS = new List<Menus>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Menus obj = new Menus();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["MENUNAME"] != DBNull.Value) obj.MenuName = _dt.Rows[i]["MENUNAME"].ToString();
                        if (_dt.Rows[i]["MENUDESC"] != DBNull.Value) obj.MenuDesc = _dt.Rows[i]["MENUDESC"].ToString();
                        if (_dt.Rows[i]["MENUORDER"] != DBNull.Value) obj.MenuOrder = int.Parse(_dt.Rows[i]["MENUORDER"].ToString());
                        if (_dt.Rows[i]["PARRENTID"] != DBNull.Value) obj.ParrentID = int.Parse(_dt.Rows[i]["PARRENTID"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["MENUURL"] != DBNull.Value) obj.MenuURL = _dt.Rows[i]["MENUURL"].ToString();
                        if (_dt.Rows[i]["MENUICON"] != DBNull.Value) obj.MenuIcon = _dt.Rows[i]["MENUICON"].ToString();
                        if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
                        if (_dt.Rows[i]["USERMODIFY"] != DBNull.Value) obj.UserModify = int.Parse(_dt.Rows[i]["USERMODIFY"].ToString());
                        if (_dt.Rows[i]["ISDISPLAY"] != DBNull.Value) obj.isDisplay = int.Parse(_dt.Rows[i]["ISDISPLAY"].ToString());
                        if (_dt.Rows[i]["ACTIVESYNC"] != DBNull.Value) obj.ActiveSync = int.Parse(_dt.Rows[i]["ACTIVESYNC"].ToString());
                        if (_dt.Rows[i]["ACTIVESYNCIMAGES"] != DBNull.Value) obj.ActiveSyncImages = int.Parse(_dt.Rows[i]["ACTIVESYNCIMAGES"].ToString());


                        lstMENUS.Add(obj);
                    }
                    if (lstMENUS != null || lstMENUS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstMENUS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.GetByIdMenus:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.GetByIdMenus:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Create  
        public ReponseEntity CreateMenus(Menus oMENUS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                MenusDAL objDAL = new MenusDAL();
                Int32 Menus = objDAL.CreateMenus(oMENUS);
                oResponse.Code = Menus.ToString();
                if (Menus == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Menus == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.CreateMenus:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.CreateMenus: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateMenus(Menus oMENUS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                MenusDAL objDAL = new MenusDAL();
                Int32 Menus = objDAL.UpdateMenus(oMENUS);
                oResponse.Code = Menus.ToString();
                if (Menus == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Menus == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.UpdateMenus:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.UpdateMenus: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteMenus(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                MenusDAL objDAL = new MenusDAL();
                objDAL.DeleteMenus(ID);
                oResponse.Code = ID.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "MenusRepository.DeleteMenus: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
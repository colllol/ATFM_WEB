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
    public class UserMenuRepository
    {
        //Get Page
        public ReponseEntity GetPageUserMenu(int page_size, int page_index)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserMenuDAL objDAL = new UserMenuDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetPageUserMenu(page_size, page_index).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<UserMenu> lstUSERMENU = new List<UserMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        UserMenu obj = new UserMenu();
                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());

                        lstUSERMENU.Add(obj);
                    }
                    if (lstUSERMENU != null || lstUSERMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetPageUserMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetPageUserMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Get ALL
        public ReponseEntity GetAllUserMenu()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserMenuDAL objDAL = new UserMenuDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllUserMenu().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<UserMenu> lstUSERMENU = new List<UserMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        UserMenu obj = new UserMenu();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());

                        lstUSERMENU.Add(obj);
                    }
                    if (lstUSERMENU != null || lstUSERMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetAllUserMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetAllUserMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdUserMenu(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserMenuDAL objDAL = new UserMenuDAL();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDAL.GetByIdUserMenu(ID).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<UserMenu> lstUSERMENU = new List<UserMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        UserMenu obj = new UserMenu();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());

                        lstUSERMENU.Add(obj);
                    }
                    if (lstUSERMENU != null || lstUSERMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetByIdUserMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.GetByIdUserMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Create  
        public ReponseEntity CreateUserMenu(UserMenu oUSERMENU)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserMenuDAL objDAL = new UserMenuDAL();
                Int32 UserMenu = objDAL.CreateUserMenu(oUSERMENU);
                oResponse.Code = UserMenu.ToString();
                if (UserMenu == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (UserMenu == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.CreateUserMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.CreateUserMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateUserMenu(UserMenu oUSERMENU)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserMenuDAL objDAL = new UserMenuDAL();
                Int32 UserMenu = objDAL.UpdateUserMenu(oUSERMENU);
                oResponse.Code = UserMenu.ToString();
                if (UserMenu == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (UserMenu == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.UpdateUserMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.UpdateUserMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteUserMenu(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserMenuDAL objDAL = new UserMenuDAL();
                objDAL.DeleteUserMenu(ID);
                oResponse.Code = ID.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserMenuRepository.DeleteUserMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
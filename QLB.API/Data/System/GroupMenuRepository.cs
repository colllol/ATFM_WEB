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
    public class GroupMenuRepository
    {
        //Get Page
        public ReponseEntity GetPageGroupMenu(int page_size, int page_index)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupMenuDAL objDAL = new GroupMenuDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetPageGroupMenu(page_size, page_index).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<GroupMenu> lstGROUPMENU = new List<GroupMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        GroupMenu obj = new GroupMenu();
                        //obj.Id = int.Parse(_dt.Rows[i]["Id"].ToString());
                        if (_dt.Rows[i]["GROUPMENU_ID"] != DBNull.Value) obj.GROUPMENU_ID = int.Parse(_dt.Rows[i]["GROUPMENU_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());   
                                           
                        lstGROUPMENU.Add(obj);
                    }
                    if (lstGROUPMENU != null || lstGROUPMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstGROUPMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetPageGroupMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetPageGroupMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Get ALL
        public ReponseEntity GetAllGroupMenu()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupMenuDAL objDAL = new GroupMenuDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllGroupMenu().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<GroupMenu> lstGROUPMENU = new List<GroupMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        GroupMenu obj = new GroupMenu();

                        if (_dt.Rows[i]["GROUPMENU_ID"] != DBNull.Value) obj.GROUPMENU_ID = int.Parse(_dt.Rows[i]["GROUPMENU_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());

                        lstGROUPMENU.Add(obj);
                    }
                    if (lstGROUPMENU != null || lstGROUPMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstGROUPMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetAllGroupMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetAllGroupMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdGroupMenu(int GROUPMENU_ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupMenuDAL objDAL = new GroupMenuDAL();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDAL.GetByIdGroupMenu(GROUPMENU_ID).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<GroupMenu> lstGROUPMENU = new List<GroupMenu>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        GroupMenu obj = new GroupMenu();

                        if (_dt.Rows[i]["GROUPMENU_ID"] != DBNull.Value) obj.GROUPMENU_ID = int.Parse(_dt.Rows[i]["GROUPMENU_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MENU_ID"] != DBNull.Value) obj.MENU_ID = int.Parse(_dt.Rows[i]["MENU_ID"].ToString());
                        if (_dt.Rows[i]["R_EDIT"] != DBNull.Value) obj.R_EDIT = int.Parse(_dt.Rows[i]["R_EDIT"].ToString());
                        if (_dt.Rows[i]["R_DEL"] != DBNull.Value) obj.R_DEL = int.Parse(_dt.Rows[i]["R_DEL"].ToString());
                        if (_dt.Rows[i]["R_ADD"] != DBNull.Value) obj.R_ADD = int.Parse(_dt.Rows[i]["R_ADD"].ToString());
                        if (_dt.Rows[i]["R_PUB"] != DBNull.Value) obj.R_PUB = int.Parse(_dt.Rows[i]["R_PUB"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());

                        lstGROUPMENU.Add(obj);
                    }
                    if (lstGROUPMENU != null || lstGROUPMENU.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstGROUPMENU.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetByIdGroupMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.GetByIdGroupMenu:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Create  
        public ReponseEntity CreateGroupMenu(GroupMenu oGROUPMENU)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupMenuDAL objDAL = new GroupMenuDAL();
                Int32 GroupMenu = objDAL.CreateGroupMenu(oGROUPMENU);
                oResponse.Code = GroupMenu.ToString();
                if (GroupMenu == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (GroupMenu == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.CreateGroupMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.CreateGroupMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateGroupMenu(GroupMenu oGROUPMENU)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupMenuDAL objDAL = new GroupMenuDAL();
                Int32 GroupMenu = objDAL.UpdateGroupMenu(oGROUPMENU);
                oResponse.Code = GroupMenu.ToString();
                if (GroupMenu == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (GroupMenu == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.UpdateGroupMenu:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.UpdateGroupMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteGroupMenu(int GROUPMENU_ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupMenuDAL objDAL = new GroupMenuDAL();
                objDAL.DeleteGroupMenu(GROUPMENU_ID);
                oResponse.Code = GROUPMENU_ID.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupMenuRepository.DeleteGroupMenu: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
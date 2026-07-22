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
    public class UserGroupsRepository
    {
        //Get Page
        public ReponseEntity GetPageUserGroups(int page_size, int page_index)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserGroupsDAL objDAL = new UserGroupsDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetPageUserGroups(page_size, page_index).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List< UserGroups> lstUSERGROUPS = new List< UserGroups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                         UserGroups obj = new  UserGroups();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MSREPL_TRAN_VERSION"] != DBNull.Value) obj.MSREPL_TRAN_VERSION = _dt.Rows[i]["MSREPL_TRAN_VERSION"].ToString();
                        if (_dt.Rows[i]["ROWGUID"] != DBNull.Value) obj.ROWGUID = _dt.Rows[i]["ROWGUID"].ToString();                       

                        lstUSERGROUPS.Add(obj);
                    }
                    if (lstUSERGROUPS != null || lstUSERGROUPS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERGROUPS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetPageUserGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetPageUserGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Get ALL
        public ReponseEntity GetAllUserGroups()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserGroupsDAL objDAL = new UserGroupsDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllUserGroups().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<UserGroups> lstUSERGROUPS = new List<UserGroups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        UserGroups obj = new UserGroups();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MSREPL_TRAN_VERSION"] != DBNull.Value) obj.MSREPL_TRAN_VERSION = _dt.Rows[i]["MSREPL_TRAN_VERSION"].ToString();
                        if (_dt.Rows[i]["ROWGUID"] != DBNull.Value) obj.ROWGUID = _dt.Rows[i]["ROWGUID"].ToString();


                        lstUSERGROUPS.Add(obj);
                    }
                    if (lstUSERGROUPS != null || lstUSERGROUPS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERGROUPS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetAllUserGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetAllUserGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdUserGroups(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UserGroupsDAL objDAL = new UserGroupsDAL();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDAL.GetByIdUserGroups(ID).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<UserGroups> lstUSERGROUPS = new List<UserGroups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        UserGroups obj = new UserGroups();

                        if (_dt.Rows[i]["ID"] != DBNull.Value) obj.ID = int.Parse(_dt.Rows[i]["ID"].ToString());
                        if (_dt.Rows[i]["USER_ID"] != DBNull.Value) obj.USER_ID = int.Parse(_dt.Rows[i]["USER_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["MSREPL_TRAN_VERSION"] != DBNull.Value) obj.MSREPL_TRAN_VERSION = _dt.Rows[i]["MSREPL_TRAN_VERSION"].ToString();
                        if (_dt.Rows[i]["ROWGUID"] != DBNull.Value) obj.ROWGUID = _dt.Rows[i]["ROWGUID"].ToString();


                        lstUSERGROUPS.Add(obj);
                    }
                    if (lstUSERGROUPS != null || lstUSERGROUPS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERGROUPS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetByIdUserGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.GetByIdUserGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Create  
        public ReponseEntity CreateUserGroups( UserGroups oUSERGROUPS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserGroupsDAL objDAL = new UserGroupsDAL();
                Int32  UserGroups = objDAL.CreateUserGroups(oUSERGROUPS);
                oResponse.Code =  UserGroups.ToString();
                if ( UserGroups == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if ( UserGroups == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.CreateUserGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.CreateUserGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateUserGroups( UserGroups oUSERGROUPS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserGroupsDAL objDAL = new UserGroupsDAL();
                Int32  UserGroups = objDAL.UpdateUserGroups(oUSERGROUPS);
                oResponse.Code =  UserGroups.ToString();
                if ( UserGroups == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if ( UserGroups == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.UpdateUserGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.UpdateUserGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteUserGroups(int ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UserGroupsDAL objDAL = new UserGroupsDAL();
                objDAL.DeleteUserGroups(ID);
                oResponse.Code = ID.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UserGroupsRepository.DeleteUserGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
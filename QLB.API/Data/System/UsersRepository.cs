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
    public class UsersRepository
    {
        //Get Page
        public ReponseEntity GetPageUsers(int page_size, int page_index)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UsersDAL objDAL = new UsersDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetPageUsers(page_size, page_index).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Users> lstUSERS = new List<Users>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Users obj = new Users();
                        if (_dt.Rows[i]["USERID"] != DBNull.Value) obj.UserID = int.Parse(_dt.Rows[i]["USERID"].ToString());
                        if (_dt.Rows[i]["USERNAME"] != DBNull.Value) obj.UserName = _dt.Rows[i]["USERNAME"].ToString();
                        if (_dt.Rows[i]["USERPASS"] != DBNull.Value) obj.UserPass = _dt.Rows[i]["USERPASS"].ToString();
                        if (_dt.Rows[i]["USERFULLNAME"] != DBNull.Value) obj.UserFullName = _dt.Rows[i]["USERFULLNAME"].ToString();
                        //if (_dt.Rows[i]["USEREMAIL"] != DBNull.Value) obj.UserFullName = _dt.Rows[i]["USEREMAIL"].ToString();
                        if (_dt.Rows[i]["USERMOBILE"] != DBNull.Value) obj.UserMobile = _dt.Rows[i]["USERMOBILE"].ToString();
                        //if (_dt.Rows[i]["USERADDRESS"] != DBNull.Value) obj.UserMobile = _dt.Rows[i]["USERADDRESS"].ToString();
                        if (_dt.Rows[i]["USERBIRTHDAY"] != DBNull.Value) obj.UserBirthday = DateTime.Parse(_dt.Rows[i]["USERBIRTHDAY"].ToString());
                        if (_dt.Rows[i]["USERACTIVE"] != DBNull.Value) obj.UserActive = int.Parse(_dt.Rows[i]["USERACTIVE"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.Group_Id = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["REDIRECTPAGES"] != DBNull.Value) obj.RedirectPages = _dt.Rows[i]["REDIRECTPAGES"].ToString();

                        lstUSERS.Add(obj);
                    }
                    if (lstUSERS != null || lstUSERS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetPageUsers:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetPageUsers:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }


       

        public ReponseEntity GetPageUsers_New(int page_size, int page_index, string where)
        {

            return new ReponseEntityHelper().GetPage<Users>(new UsersDAL().GetPageUsers_New(page_size, page_index, where).Tables[0]);
        }

        //Get ALL
        public ReponseEntity GetAllUsers()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            UsersDAL objDAL = new UsersDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllUsers().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Users> lstUSERS = new List<Users>();

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Users obj = new Users();

                        if (_dt.Rows[i]["USERID"] != DBNull.Value) obj.UserID = int.Parse(_dt.Rows[i]["USERID"].ToString());
                        if (_dt.Rows[i]["USERNAME"] != DBNull.Value) obj.UserName = _dt.Rows[i]["USERNAME"].ToString();
                        if (_dt.Rows[i]["USERPASS"] != DBNull.Value) obj.UserPass = _dt.Rows[i]["USERPASS"].ToString();
                        if (_dt.Rows[i]["USERFULLNAME"] != DBNull.Value) obj.UserFullName = _dt.Rows[i]["USERFULLNAME"].ToString();
                        //if (_dt.Rows[i]["USEREMAIL"] != DBNull.Value) obj.UserFullName = _dt.Rows[i]["USEREMAIL"].ToString();
                        if (_dt.Rows[i]["USERMOBILE"] != DBNull.Value) obj.UserMobile = _dt.Rows[i]["USERMOBILE"].ToString();
                        //if (_dt.Rows[i]["USERADDRESS"] != DBNull.Value) obj.UserMobile = _dt.Rows[i]["USERADDRESS"].ToString();
                        if (_dt.Rows[i]["USERBIRTHDAY"] != DBNull.Value) obj.UserBirthday = DateTime.Parse( _dt.Rows[i]["USERBIRTHDAY"].ToString());
                        if (_dt.Rows[i]["USERACTIVE"] != DBNull.Value) obj.UserActive = int.Parse( _dt.Rows[i]["USERACTIVE"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.Group_Id = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["REDIRECTPAGES"] != DBNull.Value) obj.RedirectPages = _dt.Rows[i]["REDIRECTPAGES"].ToString();

                        lstUSERS.Add(obj);
                    }
                    if (lstUSERS != null || lstUSERS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERS.ToList<object>();
                    }
                    else
                    {
                        oResponse.Code = "-01";
                        oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                    }
                }
                else
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetAllUsers:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, ex.ToString() + "UsersRepository.GetAllUsers:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Search by ID
        public ReponseEntity GetByIdUsers(int UserId)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            UsersDAL objDAL = new UsersDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetByIdUsers(UserId).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<Users> lstUSERS = new List<Users>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Users obj = new Users();
                        if (_dt.Rows[i]["USERID"] != DBNull.Value) obj.UserID = int.Parse(_dt.Rows[i]["USERID"].ToString());
                        if (_dt.Rows[i]["USERNAME"] != DBNull.Value) obj.UserName = _dt.Rows[i]["USERNAME"].ToString();
                        if (_dt.Rows[i]["USERPASS"] != DBNull.Value) obj.UserPass = _dt.Rows[i]["USERPASS"].ToString();
                        if (_dt.Rows[i]["USERFULLNAME"] != DBNull.Value) obj.UserFullName = _dt.Rows[i]["USERFULLNAME"].ToString();
                        //if (_dt.Rows[i]["USEREMAIL"] != DBNull.Value) obj.UserEmail = _dt.Rows[i]["USEREMAIL"].ToString();
                        if (_dt.Rows[i]["USERMOBILE"] != DBNull.Value) obj.UserMobile = _dt.Rows[i]["USERMOBILE"].ToString();
                        //if (_dt.Rows[i]["USERADDRESS"] != DBNull.Value) obj.UserAddress = _dt.Rows[i]["USERADDRESS"].ToString();
                        if (_dt.Rows[i]["USERBIRTHDAY"] != DBNull.Value) obj.UserBirthday = DateTime.Parse(_dt.Rows[i]["USERBIRTHDAY"].ToString());
                        if (_dt.Rows[i]["USERACTIVE"] != DBNull.Value) obj.UserActive = int.Parse(_dt.Rows[i]["USERACTIVE"].ToString());
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DateCreated = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DateModify = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());
                        if (_dt.Rows[i]["USERCREATE"] != DBNull.Value) obj.UserCreate = int.Parse(_dt.Rows[i]["USERCREATE"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.Group_Id = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["REDIRECTPAGES"] != DBNull.Value) obj.RedirectPages = _dt.Rows[i]["REDIRECTPAGES"].ToString();

                        lstUSERS.Add(obj);
                    }
                    if (lstUSERS != null || lstUSERS.Count > 0)
                    {
                        oResponse.Code = "00";
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstUSERS.ToList<object>();
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
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetByIdUsers:"  +ex.ToString());
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;
        }

        //Create  
        public ReponseEntity CreateUsers(Users oUSERS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UsersDAL objDAL = new UsersDAL();
                Int32 UserId = objDAL.CreateUsers(oUSERS);
                oResponse.Code = UserId.ToString();
                if (UserId == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (UserId == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.CreateUsers:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.CreateUsers: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateUsers(Users oUSERS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UsersDAL objDAL = new UsersDAL();
                Int32 UserId = objDAL.UpdateUsers(oUSERS);
                oResponse.Code = UserId.ToString();
                if (UserId == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (UserId == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.UpdateUsers:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.UpdateUsers: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteUsers(int UserId)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                UsersDAL objDAL = new UsersDAL();
                objDAL.DeleteUsers(UserId);
                oResponse.Code = UserId.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.DeleteUsers: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
        

        public ReponseEntity GetUserByUserName(string Username)
        {

            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            UsersDAL objDAL = new UsersDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetUserByUserName(Username).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = new clsConvertTableToObject().ConvertTo<Users>(_dt).ToList<object>();
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetByIdUsers:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;

        }

        public ReponseEntity GetUserByUserPass(string Username, string Password)
        {
           
            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            UsersDAL objDAL = new UsersDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetUserByUserPass(Username, Password).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = new clsConvertTableToObject().ConvertTo<Users>(_dt).ToList<object>();
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetUserByUserPass:" + ex.ToString());
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;
            
        }

        public ReponseEntity GetUSERNAME(string Username,int UserID)
        {

            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            UsersDAL objDAL = new UsersDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetUSERNAME(Username, UserID).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = new clsConvertTableToObject().ConvertTo<Users>(_dt).ToList<object>();
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetUSERNAME:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;

        }
        public ReponseEntity GetRole4UserMenu(int UserID,int MenuId)
        {

            string sReturn = string.Empty;
            string sOutPut = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            UsersDAL objDAL = new UsersDAL();

            DataTable _dt = new DataTable();

            try
            {
                _dt = objDAL.GetRole4UserMenu(UserID, MenuId).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    oResponse.Code = oMessage.CodeSussess;
                    oResponse.Message = oMessage.GetDataSussess;
                    oResponse.ListValue = new clsConvertTableToObject().ConvertTo<RolePermission>(_dt).ToList<object>();
                }
                else
                {
                    oResponse.Code = "-01";
                    oResponse.Message = "Không tìm thấy dữ liệu phù hợp";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetUSERNAME:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }

            return oResponse;

        }
        public ReponseReportEntity GetMenu4User(int UserID)
        {
            return new ReponseEntityHelper().GetTable(new UsersDAL().GetMenu4User(UserID).Tables[0]);
        }
        public ReponseReportEntity BindGridMenuByUser(int ParrentID,int UserID)
        {
            return new ReponseEntityHelper().GetTable(new UsersDAL().BindGridMenuByUser(ParrentID,UserID).Tables[0]);
        }
        public ReponseReportEntity isParrentMenu(int MenuID)
        {
            return new ReponseEntityHelper().GetTable(new UsersDAL().isParrentMenu(MenuID).Tables[0]);
        }
    }
}
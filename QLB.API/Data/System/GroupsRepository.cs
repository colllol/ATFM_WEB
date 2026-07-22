using System;
using System.Collections.Generic;
using System.Linq;
using QLB.API.Models;
using QLB.API.Common;
using System.Data;
using QLB.Info;
using QLB.BusinessLogic;


namespace QLB.API.Data
{
    public class GroupsRepository
    {
        public ReponseReportEntity BindGridMenuByGroup(int ParrentID, int GroupID)
        {
            return new ReponseEntityHelper().GetTable(new GroupsDAL().BindGridMenuByGroup(ParrentID, GroupID).Tables[0]);
        }

        //Get Page
        public ReponseEntity GetPageGroups(int page_size, int page_index)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupsDAL objDAL = new GroupsDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetPageGroups(page_size, page_index).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Groups> lstGROUPMENU = new List<Groups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Groups obj = new Groups();
                        //obj.Id = int.Parse(_dt.Rows[i]["Id"].ToString());
                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_NAME"] != DBNull.Value) obj.GROUP_NAME = _dt.Rows[i]["GROUP_NAME"].ToString();
                        if (_dt.Rows[i]["GROUP_DESCRIPTION"] != DBNull.Value) obj.GROUP_DESCRIPTION = _dt.Rows[i]["GROUP_DESCRIPTION"].ToString();
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
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetPageGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetPageGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Get ALL
        public ReponseEntity GetAllGroups()
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupsDAL objDAL = new GroupsDAL();
            DataTable _dt = new DataTable();
            try
            {

                _dt = objDAL.GetAllGroups().Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Groups> lstGROUPMENU = new List<Groups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Groups obj = new Groups();

                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_NAME"] != DBNull.Value) obj.GROUP_NAME = _dt.Rows[i]["GROUP_NAME"].ToString();
                        if (_dt.Rows[i]["GROUP_DESCRIPTION"] != DBNull.Value) obj.GROUP_DESCRIPTION = _dt.Rows[i]["GROUP_DESCRIPTION"].ToString();
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
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetAllGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetAllGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }
        public ReponseEntity GetPageGroupExport(string where)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupsDAL objDAL = new GroupsDAL();
            DataTable _dt = new DataTable();
            DataTable _dt1 = new DataTable();
            try
            {

                _dt = objDAL.GetPageGroupExport(where).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    List<Groups> lstGAM = new List<Groups>();

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Groups obj = new Groups();

                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_NAME"] != DBNull.Value) obj.GROUP_NAME = _dt.Rows[i]["GROUP_NAME"].ToString();
                        if (_dt.Rows[i]["GROUP_DESCRIPTION"] != DBNull.Value) obj.GROUP_DESCRIPTION = _dt.Rows[i]["GROUP_DESCRIPTION"].ToString();
                        if (_dt.Rows[i]["DATECREATED"] != DBNull.Value) obj.DATECREATED = DateTime.Parse(_dt.Rows[i]["DATECREATED"].ToString());
                        if (_dt.Rows[i]["DATEMODIFY"] != DBNull.Value) obj.DATEMODIFY = DateTime.Parse(_dt.Rows[i]["DATEMODIFY"].ToString());

                        lstGAM.Add(obj);
                    }
                    if (lstGAM != null || lstGAM.Count > 0)
                    {
                        oResponse.Code = "00";
                        //oResponse.SumRecord = _dt.Rows[0]["Record_Sum"].ToString();
                        oResponse.Message = "Lấy dữ liệu thành công";
                        oResponse.ListValue = lstGAM.ToList<object>();
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
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupRepository.Groups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupRepository.Groups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }
        //Search by ID
        public ReponseEntity GetByIdGroups(int GROUP_ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();
            GroupsDAL objDAL = new GroupsDAL();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDAL.GetByIdGroups(GROUP_ID).Tables[0];

                if (_dt.Rows.Count > 0)
                {
                    List<Groups> lstGROUPMENU = new List<Groups>();
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        Groups obj = new Groups();

                        if (_dt.Rows[i]["GROUP_ID"] != DBNull.Value) obj.GROUP_ID = int.Parse(_dt.Rows[i]["GROUP_ID"].ToString());
                        if (_dt.Rows[i]["GROUP_NAME"] != DBNull.Value) obj.GROUP_NAME = _dt.Rows[i]["GROUP_NAME"].ToString();
                        if (_dt.Rows[i]["GROUP_DESCRIPTION"] != DBNull.Value) obj.GROUP_DESCRIPTION = _dt.Rows[i]["GROUP_DESCRIPTION"].ToString();
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
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetByIdGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
                }
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.GetByIdGroups:");
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";
            }


            return oResponse;
        }

        //Create  
        public ReponseEntity CreateGroups(Groups oGROUPS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupsDAL objDAL = new GroupsDAL();
                Int32 Groups = objDAL.CreateGroups(oGROUPS);
                oResponse.Code = Groups.ToString();
                if (Groups == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Groups == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.CreateGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.CreateGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Update
        public ReponseEntity UpdateGroups(Groups oGROUPS)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupsDAL objDAL = new GroupsDAL();
                Int32 Groups = objDAL.UpdateGroups(oGROUPS);
                oResponse.Code = Groups.ToString();
                if (Groups == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Groups == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.UpdateGroups:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.UpdateGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        //Delete

        public ReponseEntity DeleteGroups(int GROUP_ID)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                GroupsDAL objDAL = new GroupsDAL();
                objDAL.DeleteGroups(GROUP_ID);
                oResponse.Code = GROUP_ID.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "GroupsRepository.DeleteGroups: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}
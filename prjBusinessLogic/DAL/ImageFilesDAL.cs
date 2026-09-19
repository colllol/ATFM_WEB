using System;
using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;

namespace prjBusinessLogic
{
    public class ImageFilesDAL
    {
        public int InsertT_ImageFiles(T_ImageFiles _obj)
        {
            int _insert = 0;
            try
            {
                _insert = HPCDataProvider.Instance().InsertObjectReturn(_obj, "[CMS_InsertUpdateT_ImageFiles]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _insert;
        }
        public DataSet ListAllImages(string _where)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListAllImages]", new string[] { "@WhereCondition" }, new object[] { _where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataView BindGridListImages(DataTable _dt)
        {

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                DataTable dt = new DataTable();
                DataRow dr;                
                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageType", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageFileName", typeof(string)));
                dt.Columns.Add(new DataColumn("ImgeFilePath", typeof(string)));                
                dt.Columns.Add(new DataColumn("ImageFileSize", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageFileExtension", typeof(string)));
                dt.Columns.Add(new DataColumn("DateCreated", typeof(string)));
                dt.Columns.Add(new DataColumn("UserCreated", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("ImgeFilePathOrizin", typeof(string)));
                dt.Columns.Add(new DataColumn("FileType", typeof(string)));

                if (_dt.Rows.Count > 0)
                {

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {                        
                           
                        dr = dt.NewRow();
                        dr[0] = _dt.Rows[i]["ID"].ToString();
                        dr[1] = _dt.Rows[i]["ImageType"].ToString();
                        dr[2] = _dt.Rows[i]["ImageFileName"].ToString();
                        dr[3] = _dt.Rows[i]["ImgeFilePath"].ToString();
                        dr[4] = _dt.Rows[i]["ImageFileSize"].ToString();
                        dr[5] = _dt.Rows[i]["ImageFileExtension"].ToString();
                        dr[6] = _dt.Rows[i]["DateCreated"].ToString();
                        dr[7] = _dt.Rows[i]["UserCreated"].ToString();
                        dr[8] = _dt.Rows[i]["Status"].ToString();
                        dr[9] = _dt.Rows[i]["ImgeFilePath"].ToString();
                        dr[10] = _dt.Rows[i]["FileType"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
                DataView dv = new DataView(dt);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BindGridListImages2Table(DataTable _dt)
        {

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageType", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageFileName", typeof(string)));
                dt.Columns.Add(new DataColumn("ImgeFilePath", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageFileSize", typeof(string)));
                dt.Columns.Add(new DataColumn("ImageFileExtension", typeof(string)));
                dt.Columns.Add(new DataColumn("DateCreated", typeof(string)));
                dt.Columns.Add(new DataColumn("UserCreated", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("ImgeFilePathOrizin", typeof(string)));
                dt.Columns.Add(new DataColumn("FileType", typeof(string)));

                if (_dt.Rows.Count > 0)
                {

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {

                        dr = dt.NewRow();
                        dr[0] = _dt.Rows[i]["ID"].ToString();
                        dr[1] = _dt.Rows[i]["ImageType"].ToString();
                        dr[2] = _dt.Rows[i]["ImageFileName"].ToString();
                        dr[3] = _dt.Rows[i]["ImgeFilePath"].ToString();
                        dr[4] = _dt.Rows[i]["ImageFileSize"].ToString();
                        dr[5] = _dt.Rows[i]["ImageFileExtension"].ToString();
                        dr[6] = _dt.Rows[i]["DateCreated"].ToString();
                        dr[7] = _dt.Rows[i]["UserCreated"].ToString();
                        dr[8] = _dt.Rows[i]["Status"].ToString();
                        dr[9] = _dt.Rows[i]["ImgeFilePath"].ToString();
                        dr[10] = _dt.Rows[i]["FileType"].ToString();
                        dt.Rows.Add(dr);
                    }
                }                
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete_Image(int ID)
        {
            bool Success = true;
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_Delete_Image]", new string[] { "@ImageID" }, new object[] { ID });
            }
            catch { Success = false; }
            return Success;
        }
        public DataSet ListAllImagesInNews(int News_ID, int Type)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[CMS_Search_ImageByNewsID]", new string[] { "@NewsID", "@Type" }, new object[] { News_ID, Type });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Chamnhanbutanh(int type, int ImageID,int NewsID, int tien, string tacgia, int AuthorID)
        {
            bool Success = true;
            try
            {
                HPCDataProvider.Instance().ExecStore("CMS_Channhanbutanh",
                    new string[] { "@ImageID", "@NewsID", "@AuthorID", "@Tacgia", "@Tongtien", "@Type" },
                    new object[] { ImageID, NewsID, AuthorID, tacgia, tien, type });
            }
            catch { Success = false; }
            return Success;
        }
        public void Insert_ImagesInNews(double News_ID, int Image_ID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_Insert_News_Image]", new string[] { "@NewsID", "@ImageID" }, new object[] { News_ID, Image_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete_Image_NewsID(double News_ID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_NewsImagesNews_ID]", new string[] { "@News_ID" }, new object[] { News_ID });
            }
            catch { }
        }
        public DataSet BindGridT_ImageFiles(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("CMS_ListT_ImageFilesDynamic", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region PHAN KHO ANH

        public DataSet BindGridT_ImageFilesDynamic(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[Sp_ListT_ImageFilesDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteDataByID(double _ID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_Delete_Image]", new string[] { "@ImageID" }, new object[] { _ID });
            }
            catch { }
        }
        public void UpdateStatusDataByID(string _Where)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_ImageFilesDynamic]", new string[] { "@WhereCondition" }, new object[] { _Where });
            }
            catch { }
        }
        #endregion
    }
}

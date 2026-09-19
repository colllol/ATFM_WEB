using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections;
using HPCServerDataAccess;
using prjInfo;

namespace prjApplication.Service
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod]
        public ArrayList AutoFindKeyword(string key)
        {
            SqlService _sqlservice = new SqlService();
            SqlDataReader _reader;
            string sql = @"CMS_AutoFindKeyword";
            try
            {
                ArrayList arr = new ArrayList();
                _sqlservice.AddParameter(new SqlParameter("@Keyword", key.ToString().Trim()));
                _reader = _sqlservice.ExecuteSPReader(sql);
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        arr.Add(_reader["Keyword"]);
                    }
                }
                return arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                _sqlservice.Disconnect();
            }
        }
        [WebMethod]//
        public string SaveData(object id_autosave, object UserID, object lang_id, object cat_id, object title, object sub_title, object images, object summary, object keywords, object author_name, object isCategorys, object isHomePages, object isCategoryParrent, object isImages, object isNewsIsFocus, object isVideo, object isHistorys, object body, object comment, object news_id, object status, object isNewsHot, object tienNB)//
        {
            string _return = "0";
            int ID_AutoSave;
            try
            {
                try
                {
                    ID_AutoSave = Convert.ToInt32(id_autosave);
                }
                catch { ID_AutoSave = 0; }
                //int UserID = Convert.ToInt32(UserID);
                //int CatID = Convert.ToInt32(cat_id);
                //int LangID = Convert.ToInt32(lang_id);
                ////int Priority = Convert.ToInt32(priority);
                ////int Is_Type = Convert.ToInt32(is_type);
                //int News_ID = Convert.ToInt32(news_id);
                //int Status = Convert.ToInt32(status);
                //string _titleInsert = "Chưa có tiêu đề";
                //if (title.ToString().Length > 3)
                //    _titleInsert = title.ToString();
                //T_AutoSaves _AutoSaves = new T_AutoSaves();
                //_AutoSaves.ID = ID_AutoSave;
                //_AutoSaves.UserID = UserID;
                //_AutoSaves.UserCreated = UserID;
                //_AutoSaves.UserModify = UserID;
                //_AutoSaves.CAT_ID = CatID;
                //_AutoSaves.Tittle = _titleInsert;
                //_AutoSaves.BodySaves = body.ToString();
                //_AutoSaves.Summary = summary.ToString();
                //_AutoSaves.Sub_Title = sub_title.ToString();
                //_AutoSaves.Images_Summary = images.ToString();
                //_AutoSaves.Keywords = keywords.ToString();
                //_AutoSaves.Lang_ID = LangID;
                //_AutoSaves.News_ID = News_ID;
                //_AutoSaves.Comment = comment.ToString();
                //_AutoSaves.Status = Status;
                //_AutoSaves.DateCreated = DateTime.Now;
                //_AutoSaves.DateModify = DateTime.Now;
                //_AutoSaves.News_AuthorName = author_name.ToString();
                //_AutoSaves.News_IsCategoryParrent = Convert.ToInt32(isCategoryParrent);
                //_AutoSaves.News_IsCategorys = Convert.ToInt32(isCategorys);
                //_AutoSaves.News_IsFocus = Convert.ToInt32(isNewsIsFocus);
                //_AutoSaves.News_IsHistory = Convert.ToInt32(isHistorys);
                //_AutoSaves.News_IsHomePages = Convert.ToInt32(isHomePages);
                //_AutoSaves.News_IsHot = Convert.ToInt32(isNewsHot);
                //_AutoSaves.News_IsImages = Convert.ToInt32(isImages);
                //_AutoSaves.News_IsVideo = Convert.ToInt32(isVideo);
                //if (tienNB.ToString().Trim().Length > 0) {
                //    if (tienNB.ToString() != "0")
                //    {
                //        tienNB = int.Parse(tienNB.ToString().Replace(",", ""));
                //        _AutoSaves.News_TienNB = Convert.ToInt32(tienNB);
                //    }
                //}                

                //int id = 0;
                ////prjBusinessLogic.AutoSavesDAL _untilDAL = new prjBusinessLogic.AutoSavesDAL();
                ////id = _untilDAL.InsertAutoSaves(_AutoSaves);
               
                //_return = Convert.ToString(id);
                return _return;
            }
            catch (Exception ex) { throw ex; }
            
        }
    }
}

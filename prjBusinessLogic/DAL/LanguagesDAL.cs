using System;
using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;
using System.Collections;
namespace prjBusinessLogic
{
    public class LanguagesDAL
    {
        #region Get T_Languages by Language_ID
        public T_Languages GetLanguageNameBy_ID(int ID)
        {
            try
            {
                return HPCDataProvider.Instance().GetLanguagesNameBy_ID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get all records from T_Languages table

        public DataSet BindGridT_Languages(int PageIndex, int PageSize, string WhereCondition)
        {
            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListT_LanguagesDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _ds;
        }
        public DataSet BindGridT_LanguagesTranslate(int PageIndex, int PageSize, string WhereCondition)
        {
            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListT_LanguagesDynamic_TranslateLang]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _ds;
        }
        #endregion

        #region Insert & Upadate into T_Languages table
        public int InsertT_Languages(T_Languages objLang)
        {
            int _insert = 0;
            try
            {
                _insert = HPCDataProvider.Instance().InsertObjectReturn(objLang, "[CMS_InsertUpdateT_Languages]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _insert;
        }
        public int InsertT_Banner(T_Languages objLang)
        {
            int _insert = 0;
            try
            {
                _insert = HPCDataProvider.Instance().InsertObjectReturn(objLang, "[CMS_InsertUpdateT_Banner]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _insert;
        }
        #endregion

        #region Delete datas by Language_ID
        public void DeleteByID(T_Languages objLang)
        {
            try
            {
                int _lang_id = objLang.Languages_ID;
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteOneFromT_Languages]", new string[] { "@Languages_ID" }, new object[] { _lang_id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Get a T_Languages object by Language_ID
        public T_Languages GetT_LanguagesByID(int lang_id)
        {
            T_Languages _objLang = null;
            try
            {
                _objLang = (T_Languages)HPCDataProvider.Instance().GetObjectByID("CMS_SelectOneFromT_Languages", lang_id.ToString(), "T_Languages", "Languages_ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _objLang;
        }
        #endregion

    }
}

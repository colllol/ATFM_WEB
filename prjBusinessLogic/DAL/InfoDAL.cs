using System;
using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;
using System.Collections;

namespace prjBusinessLogic
{
    public class InfoDAL
    {
       
        #region Get all records from T_Languages table

        public DataSet BindGridT_Info(int PageIndex, int PageSize, string WhereCondition)
        {
            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("[Sp_ListT_InfoDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _ds;
        }
        
        #endregion

        #region Insert & Upadate into T_Info table
        public int InsertT_Info(T_Info obj)
        {
            int _insert = 0;
            try
            {
                _insert = HPCDataProvider.Instance().InsertObjectReturn(obj, "[CMS_InsertUpdateT_Info]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _insert;
        }
       
        #endregion

        #region Delete datas by Language_ID
        public void DeleteT_InfoByID(int _ID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteOneFromT_Info]", new string[] { "@ID" }, new object[] { _ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Get a T_Info object by _ID
        public T_Info GetT_InfoByID(int _id)
        {
            T_Info _obj = null;
            try
            {
                _obj = (T_Info)HPCDataProvider.Instance().GetObjectByID("[CMS_SelectOneFrom_T_Info]", _id.ToString(), "T_Info", "ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _obj;
        }
        #endregion

    }
}

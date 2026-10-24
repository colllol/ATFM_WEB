using System;
using prjInfo;
using System.Data;
using HPCShareDLL;
namespace prjBusinessLogic
{

    public class UserMenuDAL
    {        
        public void InsertT_UserMenu(T_UserMenu _T_UserMenu)
        {
            try
            {
                HPCDataProvider.Instance().InsertObject(_T_UserMenu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_UserMenu GetOneFromT_UserMenuByID(Int32 UserMenu_ID)
        {
            try
            {
                return (T_UserMenu)HPCDataProvider.Instance().GetObjectByID(UserMenu_ID.ToString(), "T_UserMenu", "UserMenu_ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRow_T_UserMenu(T_UserMenu _T_UserMenu)
        {
            try
            {
                HPCDataProvider.Instance().UpdateObject(_T_UserMenu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_UserMenuDynamic(string WhereCondition)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

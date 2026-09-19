using System;
using prjInfo;
using System.Data;
using HPCShareDLL;
namespace prjBusinessLogic
{
 
     public class GroupMenu
     {
        public void InsertT_GroupMenu(T_GroupMenu _T_GroupMenu)
        {
            try
            {
                HPCDataProvider.Instance().InsertObject(_T_GroupMenu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_GroupMenu GetOneFromT_GroupMenuByID(double GroupMenu_ID)
        {
            try
            {
                return (T_GroupMenu)HPCDataProvider.Instance().GetObjectByID(GroupMenu_ID.ToString(), "T_GroupMenu", "GroupMenu_ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

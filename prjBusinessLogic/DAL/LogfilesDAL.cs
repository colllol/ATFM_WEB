using System;
using prjInfo;
using System.Data;
using HPCShareDLL;
namespace prjBusinessLogic
{
    public class Logfiles
    {
        protected readonly string[] _parameter = 
        {
            "@UserID", "@UserName", "@DateLogin", "@TimeLogin", "@LoginStatus",
            "@SessionID", "@DateLogout", "@TimeLogout", "@Remote_Host"            
        };

        protected object[] GetValueT_Logfiles(T_Logfiles _t_logfiles)
        {
            object[] _value = 
            {
               _t_logfiles.UserID,_t_logfiles.UserName,_t_logfiles.DateLogin,
               _t_logfiles.TimeLogin,_t_logfiles.LoginStatus,_t_logfiles.SessionID,
               _t_logfiles.DateLogout,_t_logfiles.TimeLogout,_t_logfiles.Remote_Host
            };            
            return _value;
        }
        public void InsertT_Logfiles(T_Logfiles _t_logfiles)
        {           
            try
            {                
                HPCDataProvider.Instance().InsertObject(_t_logfiles);
                //HPCDataProvider.Instance().ExecStore("Sp_InsertT_Logfiles",_parameter,GetValueT_Logfiles(_t_logfiles));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

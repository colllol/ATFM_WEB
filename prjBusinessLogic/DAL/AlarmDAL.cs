using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using prjInfo;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
//using System.Net.Http.Formatting;

namespace prjBusinessLogic
{
    public class AlarmDAL
    {
        #region 
        public bool DeleteAlarm(string id)
        {
            return new clsResuftAPI<Alarm>().DeleteObj(id, "api/Alarm/DeleteAlarm/");
        }

        public Alarm GetOneAlarm(int id)
        {
            return new clsResuftAPI<Alarm>().GetOneObj(id.ToString(), "api/Alarm/GetByIdAlarm/");
        }

        public List<Alarm> GetAllAlarm()
        {
            return new clsResuftAPI<Alarm>().GetListObj("api/Alarm/GetAllAlarm");
        }
        public List<Alarm> GetPageAlarm(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Alarm>().GetListObj("api/Alarm/GetPageAlarm", pageSize, pageIndex, where);
        }

        public List<Alarm> GetPageAlarmExport(string where)
        {
            return new clsResuftAPI<Alarm>().GetListObjExportData("api/Alarm/GetPageAlarmExport", where);
        }

        public bool UpdateAlarmAsync(Alarm obj)
        {
            return new clsResuftAPI<Alarm>().UpdateObj(obj, "api/Alarm/UpdateAlarm");
        }
        
        public bool InsertAlarm(Alarm obj)
        {
            return new clsResuftAPI<Alarm>().InsertObj(obj, "api/Alarm/CreateAlarm");
        }
        public Alarm InsertReturnAlarm(Alarm obj)
        {
            return new clsResuftAPI<Alarm>().InsertReturnObj(obj, "api/Alarm/CreateAlarm");
        }

        public System.Data.DataTable GetTableCountAlarm()
        {
            return new clsResuftAPI().GetTableObj("api/Alarm/GetCountAlarmByDate");
        }
        #endregion
    }
}

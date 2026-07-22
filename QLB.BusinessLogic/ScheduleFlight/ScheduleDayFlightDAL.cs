using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLB.Info;
using Oracle.DataAccess.Client;
using System.Data;

namespace QLB.BusinessLogic
{
    public class ScheduleDayFlightDAL
    {
        public Int64 Insert(ScheduleDayFlight obj)
        {
            return Convert.ToInt64(new oDataProvider().ExecuteNonQuery("SCHEDULEDAYFLIGHTS_2020_PKG", "oInsert", obj).ToString());
        }
        public Int64 Update(ScheduleDayFlight obj)
        {
            
            return Convert.ToInt64(new oDataProvider().ExecuteNonQuery("SCHEDULEDAYFLIGHTS_2020_PKG", "oUpdate", obj).ToString());
        }
        public Int64 Delete(Int64 id)
        {
            return Convert.ToInt64(new oDataProvider().ExecuteNonQuery("SCHEDULEDAYFLIGHTS_2020_PKG", "oDelete", new OracleParameter("p_ID", id)).ToString());
        }
        public DataTable GetBySearch(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("SCHEDULEDAYFLIGHTS_2020_PKG", "getBySearch", objSearch).Tables[0];
        }
        public DataTable GetById(Int64 id)
        {
            return new oDataProvider().ExecuteDatase("SCHEDULEDAYFLIGHTS_PKG", "GetById", new OracleParameter("p_ID", id)).Tables[0];
        }

        public DataTable GetBySearchAll(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("SCHEDULEDAYFLIGHTS_2020_PKG", "getBySearch", objSearch).Tables[0];
        }
        public DataTable GetBySearchAll_New(clsDayFlightValueSearch objSearch)
        {
            return new oDataProvider().ExecuteDatase("SCHEDULEDAYFLIGHTS_2020_PKG", "getBySearch_New", objSearch).Tables[0];
        }
    }
}

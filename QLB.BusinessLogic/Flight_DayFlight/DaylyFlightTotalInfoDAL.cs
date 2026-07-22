using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public class DaylyFlightTotalInfoDAL
    {
        public DataTable GetAll()
        {
            try
            {
                return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDaylyFlightInfo").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDivInfo_HCM(string listSanBayHcm)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDivInfo_Hcm", new OracleParameter("p_listHcm", listSanBayHcm)).Tables[0];
        }
        public DataTable GetDivInfo_Dng(string listSanBayHcm)
        {
            return new oDataProvider().ExecuteDatase("FLIGHT_DAYFLIGHT", "GetDivInfo_Dng", new OracleParameter("p_listDng", listSanBayHcm)).Tables[0];
        }
    }
}

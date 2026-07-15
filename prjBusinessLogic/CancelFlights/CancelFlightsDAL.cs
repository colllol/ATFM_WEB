using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using prjInfo;

namespace prjBusinessLogic
{
    public class CancelFlightsDAL
    {
        private HttpClient client;
        public CancelFlightsDAL()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<CancelFlights> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<CancelFlights>().GetListObj("api/CancelFlights/GetPage/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/CancelFlights/GetPage/", pageSize, pageIndex, where);
        }
    }
}

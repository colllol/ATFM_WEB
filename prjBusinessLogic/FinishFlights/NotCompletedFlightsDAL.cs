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
    public class NotCompletedFlightsDAL
    {
        private HttpClient client;
        public NotCompletedFlightsDAL()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public DataTable GetOne(string id)
        {
            DataTable dt;
            try
            {
                HttpResponseMessage response = client.GetAsync($"api/NotCompletedFlights/GetById/{id}").Result;
                var re = response.Content.ReadAsAsync<dynamic>().Result;
                if (re.Code == "00")
                {
                    dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
                }
                else
                    dt = null;
            }
            catch
            {
                dt = null;

            }
            return dt;
        }
        public List<NotCompletedFlights> GetPage(int pageSize, int pageIndex, string where, DateTime sDate, DateTime fDate)
        {
            List<NotCompletedFlights> lisObj = new List<NotCompletedFlights>();

            try
            {
                HttpResponseMessage response = client.GetAsync($"api/NotCompletedFlights/GetPage?page_size={pageSize}&page_index={pageIndex}&where={where}&Fromdate={System.Web.HttpUtility.UrlEncode(sDate.ToShortDateString())}&Todate={System.Web.HttpUtility.UrlEncode(fDate.ToShortDateString())}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<NotCompletedFlights>>(rs.ListValue.ToString());
                    try
                    {
                        int totalrecords = Convert.ToInt32(rs.SumRecord);
                        lisObj.SetTotalRecord<NotCompletedFlights>(totalrecords);
                    }
                    catch { lisObj.SetTotalRecord<NotCompletedFlights>(0); }

                }
                else
                    lisObj = null;
            }
            catch (Exception ex)
            {
                lisObj = null;
            }
            return lisObj;
        }
    }
}

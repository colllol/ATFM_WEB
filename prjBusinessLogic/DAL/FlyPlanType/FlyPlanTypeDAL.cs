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
    public  class FlyPlanTypeDAL
    {
        private HttpClient client;
        public FlyPlanTypeDAL()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public DataTable GetPage(clsSearchFlyplanType obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FlyPlanType/GetPage", obj);
        }
        public DataTable GetOne(string id)
        {
            DataTable dt;
            try
            {
                HttpResponseMessage response = client.GetAsync($"api/FlyPlanType/GetById/{id}").Result;
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
    }
}

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
    public class PlanMessageDAL
    {
        private HttpClient client;
        public PlanMessageDAL()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public DataTable GetPage(clsSearchPlanMessageNew obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/PlanMessage/GetPage", obj);
        }
        public DataTable GetOne(string date,string part,string mestype)
        {
            DataTable dt;
            try
            {
                DateTime _fr;
                string _frdate = "";
                if (!String.IsNullOrEmpty(date.ToString().Trim()))
                {
                    _fr = UltilFunc.ToDate(date.ToString().Trim(), "MM/dd/yyyy");
                    if (_fr != DateTime.MinValue)
                        _frdate = System.Web.HttpUtility.UrlEncode(_fr.Day + "-"+_fr.Month+"-"+_fr.Year);                        
                }
                HttpResponseMessage response = client.GetAsync($"api/PlanMessage/GetById?date={_frdate}&part={part}&mestype={mestype}").Result;
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

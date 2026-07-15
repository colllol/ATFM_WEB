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
    public class CantRealizeDAL
    {
        private HttpClient client;
        public CantRealizeDAL()
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
                HttpResponseMessage response = client.GetAsync($"api/CantRealize/GetById/{id}").Result;
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

        public DataTable GetMessage(string nbr)
        {
            DataTable dt;
            try
            {
                
                HttpResponseMessage response = client.GetAsync($"api/CantRealize/Get_Message_ByNbr/{nbr}").Result;
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

        public DataTable GetAll()
        {
            return new clsResuftAPI().GetTableObj("api/DayFlights/GetAll");
        }

        public List<CantRealize> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<CantRealize>().GetListObj("api/CantRealize/GetPage/", pageSize, pageIndex, where);
        }

        public List<CantRealize> GetPage(int pageSize, int pageIndex,string where, string sDate, string fDate)
        {
            List<CantRealize> lisObj = new List<CantRealize>();

            try
            {

                DateTime _fr; DateTime _to;
                string _frdate = ""; string _todate = "";
                if (!String.IsNullOrEmpty(sDate.ToString().Trim()))
                {
                    _fr = UltilFunc.ToDate(sDate.ToString().Trim(), "dd/MM/yyyy");
                    if (_fr != DateTime.MinValue)
                        _frdate = System.Web.HttpUtility.UrlEncode(sDate.Replace("/", "-"));
                }
                if (!String.IsNullOrEmpty(fDate.ToString().Trim()))
                {
                    _to = UltilFunc.ToDate(fDate.ToString().Trim(), "dd/MM/yyyy");
                    if (_to != DateTime.MinValue)
                        _todate = System.Web.HttpUtility.UrlEncode(fDate.Replace("/", "-"));
                }
                HttpResponseMessage response = client.GetAsync($"api/CantRealize/GetPage?page_size={pageSize}&page_index={pageIndex}&where={where}&Fromdate={_frdate}&Todate={_todate}").Result;                
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<CantRealize>>(rs.ListValue.ToString());
                    try
                    {
                        int totalrecords = Convert.ToInt32(rs.SumRecord);
                        lisObj.SetTotalRecord<CantRealize>(totalrecords);
                    }
                    catch { lisObj.SetTotalRecord<CantRealize>(0); }

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

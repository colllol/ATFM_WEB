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
    public class FinishedFlightsDAL
    {
        private HttpClient client;
        public FinishedFlightsDAL()
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
                HttpResponseMessage response = client.GetAsync($"api/FinishedFlights/GetById/{id}").Result;
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

        public DataTable GetTableBySearch(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch", obj);
        }

        public DataTable GET_T_FINISHED_FLIGHTS_BY_TIME(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GET_T_FINISHED_FLIGHTS_BY_TIME", obj);
        }
        public DataTable GetTableBySearchMBNC(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBNC", obj);
        }

        public DataTable GetTableBySearchMBATA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBATA", obj);
        }

        public DataTable GetTableBySearchMBVIA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBVIA", obj);
        }

        public DataTable GetTableBySearchMBBCS(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBBCS", obj);
        }

        public DataTable GetTableBySearchMBSAMECAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBSAMECAL", obj);
        }
        public DataTable GetTableBySearch7(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch7", obj);
        }
        public DataTable GetTableBySearch8(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch8", obj);
        }
        public DataTable GetTableBySearch9(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch9", obj);
        }
        public DataTable GetTableBySearch10(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch10", obj);
        }
        public DataTable GetTableBySearch11(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch11", obj);
        }
        public DataTable GetTableBySearch12(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch12", obj);
        }
        public DataTable GetTableBySearch13(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch13", obj);
        }
        public DataTable GetTableBySearch14(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearch14", obj);
        }

        public DataTable GetTableBySearchMBTRUNGCAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMBTRUNGCAL", obj);
        }

        



        public DataTable GetTableBySearchMN(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchMN", obj);
        }

        public DataTable GetTableBySearchNC(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchNC", obj);
        }

        public DataTable GetTableBySearchATA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchATA", obj);
        }

        public DataTable GetTableBySearchVIA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchVIA", obj);
        }

        public DataTable GetTableBySearchBCS(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchBCS", obj);
        }

        public DataTable GetTableBySearchSAMECAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchSAMECAL", obj);
        }
        public DataTable GetTableBySearchTRUNGCAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchTRUNGCAL", obj);
        }


        public DataTable GetTableBySearchDN(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDN", obj);
        }

        public DataTable GetTableBySearchDNNC(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNNC", obj);
        }

        public DataTable GetTableBySearchDNATA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNATA", obj);
        }

        public DataTable GetTableBySearchDNVIA(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNVIA", obj);
        }

        public DataTable GetTableBySearchDNBCS(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNBCS", obj);
        }

        public DataTable GetTableBySearchDNSAMECAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNSAMECAL", obj);
        }
        public DataTable GetTableBySearchDNTRUNGCAL(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableBySearchDNTRUNGCAL", obj);
        }



        public DataTable ExportBySearch(object obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/ExportBySearch", obj);
        }
        public List<FinishedFlights> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<FinishedFlights>().GetListObj("api/FinishedFlights/GetPage/", pageSize, pageIndex, where);
        }
        public List<FinishedFlights> GetPage(int pageSize, int pageIndex, string where, DateTime sDate, DateTime fDate)
        {
            List<FinishedFlights> lisObj = new List<FinishedFlights>();

            try
            {
                HttpResponseMessage response = client.GetAsync($"api/FinishedFlights/GetPage?page_size={pageSize}&page_index={pageIndex}&where={where}&Fromdate={System.Web.HttpUtility.UrlEncode(sDate.ToShortDateString())}&Todate={System.Web.HttpUtility.UrlEncode(fDate.ToShortDateString())}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<FinishedFlights>>(rs.ListValue.ToString());
                    try
                    {
                        int totalrecords = Convert.ToInt32(rs.SumRecord);
                        lisObj.SetTotalRecord<FinishedFlights>(totalrecords);
                    }
                    catch { lisObj.SetTotalRecord<FinishedFlights>(0); }

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
        public bool Insert(FinishedFlights obj)
        {
            return new clsResuftAPI<FinishedFlights>().InsertObj(obj, "api/FinishedFlights/Insert");
        }
        public bool Update(FinishedFlights obj)
        {
            
            return new clsResuftAPI<FinishedFlights>().UpdateObj(obj, "api/FinishedFlights/Update");
        }
        public bool Delete(string id)
        {
            return Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("FINISH_FLIGHTS_PKG", "oDelete", new { P_VALUE=id}).ToString()) == -1 ? false : true;
        }
        public DataTable GetHisById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/FinishedFlights/GetHistoryById", id);
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/FinishedFlights/RestoreHis", id, version, idUser);
        }
        public System.Data.DataTable GetTableDelete(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/FinishedFlights/GetTableDelete", obj);
        }
    }
}

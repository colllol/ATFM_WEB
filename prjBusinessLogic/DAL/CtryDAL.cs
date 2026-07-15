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

//using ShareDLL;
//using System.Data.OracleClient;
namespace prjBusinessLogic
{
    public class CtryDAL
    {
        #region code old
        //private HttpClient client;
        //public CtryDAL()
        //{
        //    client = new HttpClient();            
        //    client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}

        /// <summary>
        /// Get all list Country
        /// </summary>
        /// <param name="urlJson"></param>
        /// <returns></returns>
        //public List<Ctry> GetAllCountry()
        //{
        //    #region old code
        //    List<Ctry> lisCountry = new List<Ctry>();
        //    try
        //    {                
        //        HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] + "/api/Ctry/GetAllCtry").Result;
        //        var rs = response.Content.ReadAsAsync<dynamic>().Result;
        //        if (rs.Code == "00")
        //        {
        //            dynamic oValue = rs.ListValue;
        //            foreach (var item in oValue)
        //            {
        //                lisCountry.Add(new Ctry
        //                {
        //                    ID = int.Parse((item.ID ?? "0").ToString()),
        //                    CTR_CODE = (item.CTR_CODE ?? "0").ToString(),
        //                    CTR_ENAME = (item.CTR_ENAME ?? "0").ToString()
        //                });
        //            }
        //        }
        //        else
        //            lisCountry = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        lisCountry = null;
        //        throw ex;
        //    }
        //    return lisCountry;
        //    #endregion
        //}

        //public List<Ctry> GetAllCountryByPageOption(string pageSize, string pageIndex) 
        //{
        //    List<Ctry> lisCountry = new List<Ctry>();
        //    try
        //    {                
        //        HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] + $"/api/Ctry/GetPageCtry?page_size={pageSize}&page_index={pageIndex}").Result;
        //        var rs = response.Content.ReadAsAsync<dynamic>().Result;
        //        if (rs.Code == "00")
        //        {
        //            dynamic oValue = rs.ListValue;
        //            foreach (var item in oValue)
        //            {
        //                lisCountry.Add(new Ctry
        //                {
        //                    ID = int.Parse((item.ID ?? "0").ToString()),
        //                    CTR_CODE = (item.CTR_CODE ?? "0").ToString(),
        //                    CTR_ENAME = (item.CTR_ENAME ?? "0").ToString()
        //                });
        //            }
        //        }
        //        else
        //            lisCountry = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        lisCountry = null;
        //        throw ex;
        //    }
        //    return lisCountry;
        //}

        //public bool UpdateCountryAsync(Ctry obj)
        //{ 
        //    string serilized = JsonConvert.SerializeObject(obj);
        //    var inputMessage = new HttpRequestMessage
        //    {
        //        Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        //    };
        //    inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage message = client.PutAsync("api/Ctry/UpdateCtry", inputMessage.Content).Result;              
        //    if (message.IsSuccessStatusCode)
        //        return true;
        //    return false;            
        //}

        //public bool InsertCountry(Ctry obj)
        //{            
        //    string serilized = JsonConvert.SerializeObject(obj);
        //    var inputMessage = new HttpRequestMessage
        //    {
        //        Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        //    };
        //    inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage message = client.PostAsync("/api/Ctry/CreateCtry", inputMessage.Content).Result;
        //    if (message.IsSuccessStatusCode)
        //        return true;
        //    return false;
        //} 



        //public bool DeleteCountry(string id)
        //{            
        //    HttpResponseMessage message = client.DeleteAsync($"api/Ctry/DeleteCtry/{id}").Result;
        //    if (message.IsSuccessStatusCode)
        //        return true;
        //    return false;
        //}

        //public Ctry GetCountryByIdAsync(string id)
        //{
        //    #region code old
        //    Ctry country = null;
        //    HttpResponseMessage response = client.GetAsync($"/api/Ctry/GetByIdCtry/{id}").Result;
        //    if (!response.IsSuccessStatusCode)
        //        return country;
        //    var re = response.Content.ReadAsAsync<dynamic>().Result;            
        //    if (re.Code == "00")
        //    {
        //        dynamic oValue = re.ListValue;
        //        var ax = JsonConvert.DeserializeObject<List<Ctry>>(re.ListValue.ToString());
        //        country = ax[0];
        //    }
        //    return country;
        //    #endregion

        //}
        #endregion

        #region 
        public Ctry GetCountryByIdAsync(string id)
        {
            return new clsResuftAPI<Ctry>().GetOneObj(id, "api/Ctry/GetByIdCtry");

        }
        public bool DeleteCountry(string id)
        {
            return new clsResuftAPI<Ctry>().DeleteObj(id, "api/Ctry/DeleteCtry/");
        }

        public Ctry GetOneCtry(int id)
        {
            return new clsResuftAPI<Ctry>().GetOneObj(id.ToString(), "api/Ctry/DeleteCtry/");
        }

        public List<Ctry> GetAllCountry()
        {
            return new clsResuftAPI<Ctry>().GetListObj("api/Ctry/GetAllCtry");
        }
        public List<Ctry> GetPageCountry(int pageSize,int pageIndex, string where)
        {
            return new clsResuftAPI<Ctry>().GetListObj("api/Ctry/GetPageCtry", pageSize, pageIndex, where);
        }

        public List<Ctry> GetPageCountryExport(string where)
        {
            return new clsResuftAPI<Ctry>().GetListObjExportData("api/Ctry/GetPageCtryExport", where);
        }

        public bool UpdateCountryAsync(Ctry obj)
        {
            return new clsResuftAPI<Ctry>().UpdateObj(obj, "api/Ctry/UpdateCtry");
        }
        //public List<Ctry> GetAllCountryByPageOption(int pageSize, int pageIndex)
        //{
        //    return new clsResuftAPI<Ctry>().GetListObj("api/Ctry/UpdateCtry", pageSize, pageIndex);
        //}

        public bool InsertCountry(Ctry obj)
        {
            return new clsResuftAPI<Ctry>().InsertObj(obj, "api/Ctry/CreateCtry");
        }
        public Ctry InsertReturnCountry(Ctry obj)
        {
            return new clsResuftAPI<Ctry>().InsertReturnObj(obj, "api/Ctry/CreateCtry");
        }
        #endregion

    }
}

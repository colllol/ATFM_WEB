using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
//using System.Net.Http.Formatting;
using System.Text;
using System.Data;
using prjInfo;
using System.Configuration;

namespace prjBusinessLogic
{
    public class clsResuftAPI<T> where T : new()
    {
        #region properties
        private string _Code;
        private string _Message;
        private string _Value;
        private string _ListValue;

        public string ListValue
        {
            get
            {
                return _ListValue;
            }

            set
            {
                _ListValue = value;
            }
        }

        internal List<Aero> GetListObjExport(string v, string where)
        {
            throw new NotImplementedException();
        }

        public string Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }

        public string Message
        {
            get
            {
                return _Message;
            }

            set
            {
                _Message = value;
            }
        }

        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                _Code = value;
            }
        }

        public T ListObject
        {
            get
            {
                return listObject;
            }

            set
            {
                listObject = value;
            }
        }

        private T listObject;
        #endregion
        #region contrustor

        private HttpClient client;
        public clsResuftAPI()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Thêm X-API-Key
            client.DefaultRequestHeaders.Add(
                "X-API-Key",
                System.Configuration.ConfigurationManager.AppSettings["APIKey"]);

        }

        #endregion        
        public bool InsertObj(T obj, string urlPath)
        {
            string serilized = JsonConvert.SerializeObject(obj);
            var inputMessage = new HttpRequestMessage
            {
                Content = new StringContent(serilized, Encoding.UTF8, "application/json")
            };
            inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage message = client.PostAsync(urlPath, inputMessage.Content).Result;
            if (message.IsSuccessStatusCode)
            {
                var re = message.Content.ReadAsAsync<dynamic>().Result;
                if (re.Code != "-1" && re.Code != "-99")
                    return true;
                else return false;
            }
            return false;
        }

        /// <summary>
        /// Insert sussecc: return Id
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public T InsertReturnObj(T obj, string urlPath)
        {
            try
            {
                string serilized = JsonConvert.SerializeObject(obj);
                var inputMessage = new HttpRequestMessage
                {
                    Content = new StringContent(serilized, Encoding.UTF8, "application/json")
                };
                inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsJsonAsync(urlPath, inputMessage.Content).Result;
                if (message.IsSuccessStatusCode)
                {
                    var re = message.Content.ReadAsAsync<dynamic>().Result;
                    if (re.Code == "-1" && re.Code != "-99")
                        return new T();
                    else
                    {
                        return GetOneObj(re.Code, (urlPath.Replace("Create", "GetById")).Insert(urlPath.Replace("Create", "GetById").Length, "/"));
                    }
                }
            }
            catch { return new T(); }
            return new T();
        }
        public string InsertReturnId(T obj, string urlPath)
        {
            try
            {
                string serilized = JsonConvert.SerializeObject(obj);
                var inputMessage = new HttpRequestMessage
                {
                    Content = new StringContent(serilized, Encoding.UTF8, "application/json")
                };
                inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = client.PostAsync(urlPath, inputMessage.Content).Result;
                if (message.IsSuccessStatusCode)
                {
                    var re = message.Content.ReadAsAsync<dynamic>().Result;
                    if (re.Code != "-1" && re.Code != "-99")
                        return re.Code;
                    else
                    {
                        return "-1";
                    }
                }
            }
            catch { return "-1"; }
            return "-1";
        }
        public bool UpdateObj(T obj, string urlPath)
        {
            string serilized = JsonConvert.SerializeObject(obj);
            var inputMessage = new HttpRequestMessage
            {
                Content = new StringContent(serilized, Encoding.UTF8, "application/json")
            };
            inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage message = client.PutAsync(urlPath, inputMessage.Content).Result;
            if (message.IsSuccessStatusCode)
            {
                var re = message.Content.ReadAsAsync<dynamic>().Result;
                if (re.Code != "-1" && re.Code != "-99")
                    return true;
                else return false;
            }
            return false;
        }

        public bool DeleteObj(string id, string urlPath)
        {
            HttpResponseMessage message = client.DeleteAsync($"{urlPath}/{id}").Result;
            if (message.IsSuccessStatusCode)
            {
                var re = message.Content.ReadAsAsync<dynamic>().Result;
                if (re.Code != "-99" && re.Code != "-1")
                    return true;
                else return false;
            }
            return false;
        }
        public T GetOneObjSysbyUser(string urlPath, string pramater1, string pramater2)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Username={pramater1}&UserID={pramater2}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }
        public T GetOneObjSysbyUserMenu(string urlPath, string pramater1, string pramater2)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}?UserID={pramater1}&MenuID={pramater2}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }
        public T GetOneObjSysbyUserID(string urlPath, string pramater1)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}?UserID={pramater1}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }
        public T GetOneObjSys(string urlPath, string pramater1, string pramater2)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Username={pramater1}&Password={pramater2}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }
        public T GetOneObjSys(string urlPath, string pramater1)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Username={pramater1}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }

        public T GetOneObj(string id, string urlPath)
        {
            T obj = new T();
            HttpResponseMessage response = client.GetAsync($"{urlPath}/{id}").Result;
            if (!response.IsSuccessStatusCode)
                return obj;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                //dynamic oValue = re.ListValue;
                var ax = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                obj = ax[0];
            }
            return obj;
        }

        /// <summary>
        /// Get ALl
        /// </summary>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public List<T> GetListObj(string urlPath)
        {
            List<T> lisObj = new List<T>();
            try
            {
                HttpResponseMessage response = client.GetAsync($"{urlPath}").Result;
                var re = response.Content.ReadAsAsync<dynamic>().Result;
                if (re.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<T>>(re.ListValue.ToString());
                }
                else
                    lisObj = null;
            }
            catch (Exception ex)
            {
                lisObj = null;
                throw ex;
            }
            return lisObj;
        }
        public List<T> GetListObjAdd(string id, string urlPath)
        {
            List<T> lisObj = new List<T>();
            try
            {
                HttpResponseMessage response = client.GetAsync($"{urlPath}?Route_Id={id}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<T>>(rs.ListValue.ToString());
                    //int totalrecords = Convert.ToInt32(rs.Value);
                    //lisObj.SetTotalRecord<T>(totalrecords);
                }
                else
                    lisObj = null;
            }
            catch (Exception ex)
            {
                lisObj = null;
                throw ex;
            }
            return lisObj;
        }

        /// <summary>
        /// Get object list by page size page index
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<T> GetListObj(string urlPath, int pageSize, int pageIndex, string where)
        {
            List<T> lisObj = new List<T>();

            try
            {
                HttpResponseMessage response = client.GetAsync($"{urlPath}?page_size={pageSize}&page_index={pageIndex}&where={where}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<T>>(rs.ListValue.ToString());
                    try
                    {
                        int totalrecords = Convert.ToInt32(rs.SumRecord);
                        lisObj.SetTotalRecord<T>(totalrecords);
                    }
                    catch { lisObj.SetTotalRecord<T>(0); }

                }
                else
                    lisObj = new List<T>();
            }
            catch (Exception ex)
            {
                lisObj = new List<T>();
            }
            return lisObj;
        }

        public List<T> GetListObjNoWhere(string urlPath, int pageSize, int pageIndex)
        {
            List<T> lisObj = new List<T>();

            try
            {
                HttpResponseMessage response = client.GetAsync($"{urlPath}?page_size={pageSize}&page_index={pageIndex}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<T>>(rs.ListValue.ToString());
                    try
                    {
                        int totalrecords = Convert.ToInt32(rs.SumRecord);
                        lisObj.SetTotalRecord<T>(totalrecords);
                    }
                    catch { lisObj.SetTotalRecord<T>(0); }

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

        public List<T> GetListObjExportData(string urlPath, string where)
        {
            List<T> lisObj = new List<T>();

            try
            {
                HttpResponseMessage response = client.GetAsync($"{urlPath}?where={where}").Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    lisObj = JsonConvert.DeserializeObject<List<T>>(rs.ListValue.ToString());

                }
                else
                    lisObj = null;
            }
            catch (Exception ex)
            {
                lisObj = null;
                throw ex;
            }
            return lisObj;
        }

    }

}
public class clsResuftAPI
{
    #region contrustor

    private HttpClient client;
    public clsResuftAPI()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string apiKey = Environment.GetEnvironmentVariable("ATFM_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
            apiKey = ConfigurationManager.AppSettings["APIKey"];
        if (!string.IsNullOrWhiteSpace(apiKey))
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }

    #endregion
    public DataTable GetTableParamaterMenu(string urlPath, int MenuId)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?MenuId={MenuId}").Result;
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
    public DataTable GetTableParamater(string urlPath, int Parrent_ID, int UserID)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Parrent_ID={Parrent_ID}&GroupId={UserID}").Result;
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
    public DataTable GetTableUserParamater(string urlPath, int Parrent_ID, int UserID)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Parrent_ID={Parrent_ID}&UserId={UserID}").Result;
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
    public DataTable GetAllMenu4User(string urlPath, int userID)
    {
        HttpResponseMessage response = client.GetAsync($"{urlPath}?UserID={userID}").Result;
        if (!response.IsSuccessStatusCode)
            return null;

        var result = response.Content.ReadAsAsync<dynamic>().Result;
        if (result.Code != "00")
            return null;

        return JsonConvert.DeserializeObject<DataTable>(result.ListValue.ToString());
    }
    public DataTable GetTableObj(string urlPath)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
                dt = null;
        }
        catch (Exception ex)
        {
            dt = null;
            throw ex;
        }
        return dt;
    }

    public DataTable GetTableObjectById(string urlPath, string id)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}/{id}").Result;
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
    public DataTable GetTableObj(string urlPath, int pageSize, int pageIndex, string where)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?page_size={pageSize}&page_index={pageIndex}&where={where}").Result;
            var rs = response.Content.ReadAsAsync<dynamic>().Result;
            if (rs.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(rs.ListValue.ToString());
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
    public bool RestoreRecord(string urlPath, string id, string version, string idUser)
    {
        HttpResponseMessage message = client.GetAsync($"{urlPath}/{id}?version={version}&iduser={idUser}").Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            //if (re.Code != "-1" && re.Code != "-99")
            //    return true;
            //else return false;
            if (re = true) return true;
            else return false;
        }
        return false;
    }
    public DataTable GetTableObj(string urlPath, string where)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?where={where}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
                dt = null;
        }
        catch (Exception ex)
        {
            dt = null;
            throw ex;
        }
        return dt;
    }

    public DataTable GetTableHistory(string urlPath, string id)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?id={id}").Result;
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
    public DataTable GetPostTableWithObject(string urlPath, object obj)
    {
        DataTable dt = new DataTable();
        string serilized = JsonConvert.SerializeObject(obj);
        var inputMessage = new HttpRequestMessage
        {
            Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        };
        inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage message = client.PostAsync(urlPath, inputMessage.Content).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
                dt = null;
        }
        return dt;
    }
    public DataTable GetTableWithObject(string urlPath, object obj)
    {
        DataTable dt = new DataTable();
        string serilized = JsonConvert.SerializeObject(obj);
        var inputMessage = new HttpRequestMessage(HttpMethod.Put, urlPath)
        {
            Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        };
        inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // Gắn trực tiếp vào request để bảo đảm header không bị bỏ qua khi gửi PUT.
        inputMessage.Headers.TryAddWithoutValidation(
            "X-API-Key",
            ConfigurationManager.AppSettings["APIKey"]);
        HttpResponseMessage message = client.SendAsync(inputMessage).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
                dt = null;
        }
        return dt;
    }
    public object GetValueWithObject(string urlPath, object obj)
    {
        string serilized = JsonConvert.SerializeObject(obj);
        var inputMessage = new HttpRequestMessage
        {
            Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        };
        inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage message = client.PutAsync(urlPath, inputMessage.Content).Result;

        /*
         * hungtn change 20062018
         * 
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                return re.ListValue;
            }            
        }
        */
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            return re.Code;
        }
        return null;
    }
    #region suport report
    public DataTable GetReport(string urlPath, string FromDate, string ToDate)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?FromDate={FromDate}&ToDate={ToDate}").Result;
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
    public DataTable GetReportMien(string urlPath, string Mien)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Mien={Mien}").Result;
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

    public DataTable GetReportMien(string urlPath, string Mien,string Month)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Mien={Mien}&Month={Month}").Result;
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

    public DataTable GetReportMonth(string urlPath,string Month)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Month={Month}").Result;
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




    public string GetValueReport(string urlPath, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }
    public string GetValueReport(string urlPath, string Month, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?MONTH={Month}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }
    public string GetValueReport(string urlPath, string Month)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?MONTH={Month}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }


    public string GetValueReportByVia(string urlPath, string Month,string MonthPre)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Month={Month}&MonthPre{MonthPre}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }



    public DataTable GetReport(string urlPath,string Oper, string FromDate, string ToDate)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Oper={Oper}&FromDate={FromDate}&ToDate={ToDate}").Result;
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

    public DataTable GetReportByCraft(string urlPath, string Craft, string FromDate, string ToDate)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Craft={Craft}&FromDate={FromDate}&ToDate={ToDate}").Result;
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

    public string GetValueReport_ByCraft(string urlPath, string Craft, string Route, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Craft={Craft}&Route={Route}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }



    public string GetValueReport_FIR(string urlPath, string Hang, string Route, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?OPER={Hang}&Route={Route}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }


    public string GetValueReport(string urlPath, string Hang, string Month, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?OPER={Hang}&MONTH={Month}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }


    public string GetValueReport_Pupose(string urlPath, string Hang, string Pupose, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Oper={Hang}&Pupose={Pupose}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }
    public string GetValueReport_air(string urlPath,string air, string Hang, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?air={air}&oper={Hang}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }

    public string GetValueReport_Mien(string urlPath, string Mien, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Mien={Mien}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }



    public string GetValueReport_Month(string urlPath, string air, string Hang, string Month)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?air={air}&oper={Hang}&Month={Month}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }

    public string GetValueRp_BC3(string urlPath, string Hang, string Month)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?OPER={Hang}&MONTH={Month}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }

    public string GetValueR_DB(string urlPath, string DuongBay, string Month, string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?ROUTE_NAME={DuongBay}&MONTH={Month}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }

    public string GetValueR_DB(string urlPath, string DuongBay, string Month)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?ROUTE_NAME={DuongBay}&MONTH={Month}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }





    public DataTable GetValue(string urlPath, string RouteName)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Route_Name={RouteName}").Result;
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

    public string GetValueReport_SLB(string urlPath, string FromDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Date={FromDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }
    public string GetValueReport_SLB(string urlPath, string Hang, string FromDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?OPER={Hang}&&Date={FromDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }
    public DataTable GetReport_SLB(string urlPath, string FromDate)
    {
        DataTable dt;
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?Date={FromDate}").Result;
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

    //Bao cao so 10

    public string GetValueR_DB(string urlPath, string Hang,string FromDate, string ToDate)
    {
        string _return = "";
        try
        {
            HttpResponseMessage response = client.GetAsync($"{urlPath}?OPER={Hang}&FromDate={FromDate}&ToDate={ToDate}").Result;
            var re = response.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                _return = JsonConvert.DeserializeObject<String>(re.ListValue.ToString());
            }

        }
        catch
        {

        }
        return _return;
    }




    #endregion
    #region suport api extension
    public DataTable GetTableApiExtension(string packageName, string storeName, object obj)
    {
        DataTable dt = new DataTable();
        string serilized = JsonConvert.SerializeObject(obj);
        var inputMessage = new HttpRequestMessage
        {
            Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        };
        inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage message = client.PutAsync($"api/ApiExtension/ExcuteTable?packageName={packageName}&storeName={storeName}", inputMessage.Content).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
                dt = null;
        }
        return dt;
    }
    public DataTable GetPostTableApiExtension(string packageName, string storeName, object obj)
    {
        DataTable dt = new DataTable();
        string serilized = JsonConvert.SerializeObject(obj);
        var content = new StringContent(serilized, Encoding.UTF8, "application/json");
        HttpResponseMessage message = client.PostAsync($"api/ApiExtension/ExcutePostTable?packageName={packageName}&storeName={storeName}", content).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                dt = JsonConvert.DeserializeObject<DataTable>(re.ListValue.ToString());
            }
            else
            {
                dt = null;
            }
        }
        return dt;
    }
    public object GetValueApiExtension(string packageName, string storeName, object obj)
    {
        string serilized = JsonConvert.SerializeObject(obj);
        var inputMessage = new HttpRequestMessage
        {
            Content = new StringContent(serilized, Encoding.UTF8, "application/json")
        };
        inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage message = client.PutAsync($"api/ApiExtension/ExcuteReturnInt?packageName={packageName}&storeName={storeName}", inputMessage.Content).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                return re.ListValue.ToString();
            }
            else
                return null;
        }
        return null;
    }
    public object GetPostValueApiExtension(string packageName, string storeName, object obj)
    {
        string serilized = JsonConvert.SerializeObject(obj);
        var content = new StringContent(serilized, Encoding.UTF8, "application/json");
        HttpResponseMessage message = client.PostAsync($"api/ApiExtension/ExcutePostReturnInt?packageName={packageName}&storeName={storeName}", content).Result;
        if (message.IsSuccessStatusCode)
        {
            var re = message.Content.ReadAsAsync<dynamic>().Result;
            if (re.Code == "00")
            {
                return re.ListValue.ToString();
            }
        }

        return null;
    }
    #endregion
    #region suport dayflight

    #endregion
}

public static class clsRecordCount
{
    private static int _total;
    public static void SetTotalRecord<T>(this List<T> lis, int value)
    {
        _total = value;
    }
    public static int GetTotalRecord<T>(this List<T> lis)
    {
        if (lis == null)
            return 0;
        return _total;
    }
}



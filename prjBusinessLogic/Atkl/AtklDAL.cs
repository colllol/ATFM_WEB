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
    public class AtklDAL
    {
        private HttpClient client;
        public AtklDAL()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public System.Data.DataTable GetFPL(string date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("ATKL_PKG", "sp_FPL", new { P_DATE = date });
        }
        public System.Data.DataTable GetExpFPL(string date, string pdate)
        {
            return new clsResuftAPI().GetPostTableApiExtension("ATKL_PKG", "sp_ExpFPL", new { P_DATE = date, P_EDATE = pdate });
        }
        public DataTable GetPage(clsSearchAtkl obj)
        {
            return new clsResuftAPI().GetPostTableWithObject("api/Atkl/GetPage", obj);

        }
        public Atkl GetAtklById(string id)
        {
            return new clsResuftAPI<Atkl>().GetOneObj(id, "api/Atkl/GetByID/");
        }
        public bool UpdateAtkl(Atkl obj)
        {
            return new clsResuftAPI<Atkl>().UpdateObj(obj, "api/Atkl/UpdateAtkl");
        }
    }
}

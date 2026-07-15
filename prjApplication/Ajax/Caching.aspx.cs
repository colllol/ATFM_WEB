using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.IO;
namespace prjApplication.Ajax
{
    public partial class Caching : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public String code()
        {
           // DataSet _ds = new DataSet();
            string Url = "http://192.168.1.8/CacheHomePage/default.aspx";
            string result = string.Empty;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            //_ds.ReadXml(Url);
            //if (_ds != null)
            //{
            //    DataTable _dt = _ds.Tables["span"];
            //}
            return result;
        }
    }
}

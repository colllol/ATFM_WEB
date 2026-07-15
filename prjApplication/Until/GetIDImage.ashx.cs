using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjApplication.Service;
using System.Web.Services;

namespace prjApplication.Until
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetIDImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            try
            {
                string _ID = "66";
                context.Response.Write(_ID);
                context.Response.StatusCode = 200;
            }
            catch { context.Response.End(); }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

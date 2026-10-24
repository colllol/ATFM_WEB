using System;
using System.Web;

namespace prjComponents
{
    public class HttpHandler: IHttpModule
    {
        #region Member variables and inherited properties / methods
        public void Init(HttpApplication application)
        {
            // Wire-up application events            
            application.Error += new EventHandler(this.Application_OnError);
        }
        public void Dispose()
        {
        }
        #endregion
        #region Application OnError
        private void Application_OnError(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            //context.Response.Redirect(Global.ApplicationPath + "Error.aspx");
        }
        #endregion
    }
}

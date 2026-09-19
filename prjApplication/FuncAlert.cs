using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjApplication
{
    public class FuncAlert : System.Web.UI.Page
    {
        public static void AlertJS(System.Web.UI.Control _control, string Msg)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(_control, typeof(string), "Message", "alert('" + Msg + "');", true);

        }
    }
}

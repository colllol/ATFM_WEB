using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjApplication.Ajax
{
    public partial class PluginPlayAudio : System.Web.UI.Page
    {
        public string _urlFile = "";
        public string _urlImage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Page.Request["urlfile"] != null && Page.Request["urlimg"] != null)
                {
                    _urlFile = prjComponents.Global.UploadPath + Page.Request["urlfile"];
                }

            }
        }
    }
}

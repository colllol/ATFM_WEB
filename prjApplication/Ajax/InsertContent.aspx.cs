using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;

namespace prjApplication.Ajax
{
    public partial class InsertContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request["id"] != null && Convert.ToInt32(Request["id"]) > 0)
                //{
                //    prjBusinessLogic.DAL.T_NewsDAL obj = new prjBusinessLogic.DAL.T_NewsDAL();
                //    prjInfo.T_News objnew = new T_News();
                //    objnew = obj.GetOneFromT_NewsByID(Convert.ToInt32(Request["id"]));
                //    this.litContent.Text = objnew.News_Body; obj = null; objnew = null;
                //}
            }
        }
    }
}

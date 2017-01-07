using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.sys
{
    public partial class quit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["person"] != null)
            {
                Session.Clear();
                Response.Redirect("/login.aspx");
            }
        }
    }
}
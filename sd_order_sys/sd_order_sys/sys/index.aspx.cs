using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.sys
{
    public partial class index : System.Web.UI.Page
    {
        public string msg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["person"] == null)
                ResponseUrl();
            else
            {
                if (!IsPostBack)
                {
                    Label1.Text = Session["person"].ToString();
                    LoadTree("01");
                }
            }
        }
        private void LoadTree(string role)
        {
            string path = Server.MapPath(@"/me_xml/sitemap.xml");
            msg = SDorder.BLL.XmlHelper.GetText(path: path, key: role);
            if (msg == "")
                ResponseUrl();
        }
        private void ResponseUrl()
        {
            Response.Redirect("/login.aspx");
        }
    }
}
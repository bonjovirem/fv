using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.files
{
    public partial class setFloor : System.Web.UI.Page
    {
        public string proId = "";
        public string proName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            proId = Request.QueryString["projectId"];
            proName = Request.QueryString["projectName"];
            hidpro.Value = proId;
        }
    }
}
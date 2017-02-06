using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.files
{
    public partial class projectBrandType_query : System.Web.UI.Page
    {
        public string projectId = "";
        public string projectName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            projectId = Request.QueryString["projectId"];
            projectName = Request.QueryString["projectName"];
        }
    }
}
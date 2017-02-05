using SDorder.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sd_order_sys.files
{
    public partial class DrawPointClient : System.Web.UI.Page
    {
        public string floorId = "";
        public string projectId = "";
        public string floorLevel = "";
        public string clientId = "";
        public string strForShow = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            floorId = Request.QueryString["floorId"];
            projectId = Request.QueryString["projectId"];
            floorLevel = Request.QueryString["floorLevel"];
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = string.Format("select id,clientPoint from fv_client where floorId={0} and projectId={1} and floorLevel={2} limit 1 ", floorId, projectId, floorLevel);
            DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                strForShow = string.Format("drawPt3{0};", dt.Rows[0][1].ToString());
                clientId = dt.Rows[0][0].ToString();
            }
        }
    }
}
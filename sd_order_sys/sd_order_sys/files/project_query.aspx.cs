using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SDorder.BLL;
using System.Data;

namespace sd_order_sys.files
{
    public partial class project_query : System.Web.UI.Page
    {
        public string projectId = "";
        public string projectName = "";
        public string projectBtId = "";
        public string projectBtName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                projectId = Request.QueryString["projectId"];
                projectName = Request.QueryString["projectName"];
                projectBtId = Request.QueryString["projectBtId"];
                projectBtName = Request.QueryString["projectBtName"];
               // LoadControl();
            }
        }
        //private void LoadControl()
        //{
        //    Dictionary<string, object> sqlparams = new Dictionary<string, object>();
        //    DataTable dt = SqlManage.Query("select id,brandName from fv_sysbrand", sqlparams).Tables[0];
        //    brandTypeName.DataSource = dt;
        //    brandTypeName.DataValueField = "id";
        //    brandTypeName.DataTextField = "brandName";
        //    brandTypeName.DataBind();
        //    if (dt.Rows.Count > 0)
        //    {
        //        hidname.Value = dt.Rows[0]["brandName"].ToString();
        //    }

        //}
    }
}
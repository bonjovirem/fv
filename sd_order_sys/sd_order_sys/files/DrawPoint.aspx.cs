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
    public partial class DrawPoint : System.Web.UI.Page
    {
        public string projectId = "";
        public string strForShow = "";
        public string projectBrandId = "";
        public string floorLevelId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            projectId = Request.QueryString["projectId"];
            projectBrandId = Request.QueryString["projectBrandId"];
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = string.Format("select areapoints,floorlevel from fv_projectbrand where id={0}  limit 1 ", projectBrandId);
            DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                string spoints = dt.Rows[0][0].ToString();
                floorLevelId = dt.Rows[0][1].ToString();
                string[] arrayPoints = spoints.Split(';');

                if (spoints != "")
                {
                    foreach (string item in arrayPoints)
                    {
                        strForShow += string.Format("drawPt3{0};", item);
                    }
                }
                if (floorLevelId == "")
                {
                    floorLevelId = "1";
                }

            }
            if (!IsPostBack)
            {
                LoadControl();
            }
        }

        private void LoadControl()
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            DataTable dt = SqlManage.Query("select id,floorlevel from fv_floor where projectId=" + projectId, sqlparams).Tables[0];
            floorLevel.DataSource = dt;
            floorLevel.DataValueField = "id";
            floorLevel.DataTextField = "floorlevel";
            floorLevel.DataBind();
            if (dt.Rows.Count > 0)
            {
                floorLevel.Value = floorLevelId;
                hidFloorId.Value = floorLevelId;
            }
            else {
                Response.Write("<script>alert('请先设置该楼层信息！');window.close();</script>");
            }

        }
    }
}
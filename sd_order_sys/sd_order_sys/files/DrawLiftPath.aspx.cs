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
    public partial class DrawLiftPath : System.Web.UI.Page
    {
        public string projectId = "";
        public string strForShow = "";
        public string projectBrandId = "";
        public string floorLevel = "";
        public string clientId = "";
        public string strForClient = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            projectId = Request.QueryString["projectId"];
            projectBrandId = Request.QueryString["projectBrandId"];
            floorLevel = Request.QueryString["floorLevel"];
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();

            //电梯的点 b.isClient=0
            string sql = string.Format("select b.id,b.clientPoint,a.floorLevel from fv_projectbrand a,fv_client b " +
            "where a.projectId=b.projectId and a.floorLevel=b.floorLevel and b.isClient=0 and a.id={0}", projectBrandId);
            DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                clientId = dt.Rows[0][0].ToString();
                strForClient = string.Format("drawPt3{0};", dt.Rows[0][1].ToString());
                strForShow += strForClient;
                floorLevel = dt.Rows[0][2].ToString(); //重新获取一下，免得数据不准确
            }
            else
            {
                Response.Write("<script>alert('请先设置该楼层客户端位置！');window.close();</script>");
            }

            //路径的点
            sql = string.Format("select c.walkWay from fv_projectbrand a,fv_client b,fv_walkway c " +
           "where a.projectId=b.projectId and a.floorLevel=b.floorLevel and a.id=c.projectBrandId and b.id=c.fromClientId and b.isClient=0 and a.id={0} limit 1 ", projectBrandId);
            dt = SqlManage.Query(sql, sqlparams).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                strForShow = "";
                string paths = dt.Rows[0][0].ToString();
                string[] arrayPaths = paths.Split(';');
                foreach (string item in arrayPaths)
                {
                    strForShow += string.Format("drawPt3{0};", item);
                }
            }
        }

    }
}
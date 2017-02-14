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
    public partial class DrawCToLift : System.Web.UI.Page
    {

        public string floorId = "";
        public string projectId = "";
        public string strForShow = "";
        public string projectBrandId = "";
        public string floorLevel = "";
        public string clientId = "";
        public string strForClient = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            floorId = Request.QueryString["floorId"];
            projectId = Request.QueryString["projectId"];
            projectBrandId = Request.QueryString["projectBrandId"]; //无用
            floorLevel = Request.QueryString["floorLevel"];
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();

            //客户端的点 b.isClient=1  nextPointName对应C TO 电梯的路径
            string sql = string.Format("select id,clientPoint,nextPointName from fv_client where isClient=1 and floorId={0} and projectId={1} and floorLevel={2} limit 1 ",
                floorId, projectId, floorLevel);
            DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                clientId = dt.Rows[0][0].ToString();
                string ctoLPath = dt.Rows[0][2].ToString();
                if (!string.IsNullOrEmpty(ctoLPath))
                {
                    string[] arrS = ctoLPath.Split(';');
                    foreach (string item in arrS)
                    {
                        strForShow += string.Format("drawPt3{0};", item);
                    }
                    //strForClient = string.Format("drawPt3{0};", dt.Rows[0][1].ToString());
                    //strForShow += strForClient;
                }

                //floorLevel = dt.Rows[0][2].ToString();
            }


            // //路径的点
            // sql = string.Format("select c.walkWay from fv_projectbrand a,fv_client b,fv_walkway c " +
            //"where a.projectId=b.projectId and a.floorLevel=b.floorLevel and a.id=c.projectBrandId and b.id=c.fromClientId and b.isClient=1 and a.id={0}  limit 1 ", projectBrandId);
            // dt = SqlManage.Query(sql, sqlparams).Tables[0];
            // if (dt != null && dt.Rows.Count > 0)
            // {
            //     strForShow = "";
            //     string paths = dt.Rows[0][0].ToString();
            //     string[] arrayPaths = paths.Split(';');
            //     foreach (string item in arrayPaths)
            //     {
            //         strForShow += string.Format("drawPt3{0};", item);
            //     }
        }
    }



}
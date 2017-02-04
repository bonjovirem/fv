using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SDorder.BLL;

namespace sd_order_sys.files
{
    /// <summary>
    /// DrawPoint1 的摘要说明
    /// </summary>
    public class DrawPoint1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string user = "";
            context.Response.ContentType = "text/plain";
            if ("nouser".Equals(user))
                context.Response.Redirect("/login.aspx");
            else
            {
                string key = context.Request["action"] == null ? "" : context.Request["action"].ToString();
                switch (key)
                {

                    case "add":
                        RecordAdd(context);
                        break;
                }
            }
        }
        private void RecordAdd(HttpContext context)
        {
            string areaPoints = context.Request.Form["txtArea"].ToString();
            string hidFloorId = context.Request.Form["hidFloorId"].ToString();
            string projectId = context.Request.Form["projectId"].ToString();
              Dictionary<string, object> sqlparams = new Dictionary<string, object>();
              sqlparams.Add("@floorId", hidFloorId);
              sqlparams.Add("@areaPoints", areaPoints);
            string sql = "";
            sql = "update fv_projectbrand set floorId=@floorId,areaPoints=@areaPoints where id=" + projectId;
            bool w = SqlManage.OpRecord(sql, sqlparams);
            string msg = "";
            if (w)

                msg = "suc";
            else
                msg = "数据库连接超时或出现未知错误";
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            context.Response.Write(javascriptSerializer.Serialize(msg));

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
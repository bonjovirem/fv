using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SDorder.BLL;


namespace sd_order_sys.struts
{
    /// <summary>
    /// DrawLiftPath 的摘要说明
    /// </summary>
    public class DrawLiftPath : IHttpHandler
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

            string clientId = context.Request.Form["clientId"].ToString();
            string areaPoints = context.Request.Form["txtArea"].ToString();
            string floorLevel = context.Request.Form["floorLevel"].ToString();
            string projectBrandId = context.Request.Form["projectBrandId"].ToString();
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@walkWay", areaPoints);
            string sql = "";
            sql = string.Format("select count(*) from fv_walkway where projectBrandId={0} and fromClientId={1}", projectBrandId, clientId);
            int rtn = Convert.ToInt32(SqlManage.Exists(sql, sqlparams));
            if (rtn > 0)
            {
                sql = string.Format("update fv_walkway set walkWay=@walkWay where projectBrandId={0} and fromClientId={1}", projectBrandId, clientId);

            }
            else
            {
                sql = string.Format("insert into fv_walkway (projectBrandId,walkWay,fromClientId) values ({0},@walkWay,{1})", projectBrandId, clientId);

            }
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
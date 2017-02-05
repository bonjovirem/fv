using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SDorder.BLL;
using System.Data;

namespace sd_order_sys.struts
{
    /// <summary>
    /// DrawPointClient 的摘要说明
    /// </summary>
    public class DrawPointClient : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string key = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (key)
            {
                case "add":
                    RecordAdd(context);
                    break;
            }
        }

        private void RecordAdd(HttpContext context)
        {
            string floorLevel = context.Request.Form["floorLevel"].ToString();
            string projectId = context.Request.Form["projectId"].ToString();
            string floorId = context.Request.Form["floorId"].ToString();
            string clientPoint = context.Request.Form["clientPoint"].ToString();
            int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@floorLevel", floorLevel);
            sqlparams.Add("@projectId", projectId);
            sqlparams.Add("@floorId", floorId);
            sqlparams.Add("@clientPoint", clientPoint);
            string sql = "";
            if (id == 0)
                sql = "insert into fv_client (clientPoint,clientName,projectId,floorLevel,floorId,nextPointId,nextPointName) values(@clientPoint,@clientPoint,@projectId,@floorLevel,@floorId,0,'')";
            else
                sql = "update fv_client set clientPoint=@clientPoint,clientName=@clientPoint where id=" + id;
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
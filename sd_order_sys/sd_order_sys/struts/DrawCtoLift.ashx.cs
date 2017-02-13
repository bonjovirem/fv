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
    /// DrawCtoLift 的摘要说明
    /// </summary>
    public class DrawCtoLift : IHttpHandler
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
            string floorId = context.Request.Form["floorId"].ToString();
            string areaPoints = context.Request.Form["txtArea"].ToString();

            string[] arrS = areaPoints.Split(';');
            string clientPoint = arrS[0];  //第一个点是C
            string liftPoint = arrS[arrS.Length - 1]; // 第二个点是L
            string floorLevel = context.Request.Form["floorLevel"].ToString();
            string projectId = context.Request.Form["projectId"].ToString();
            int id = string.IsNullOrEmpty(context.Request.Form["clientId"].ToString()) ? 0 : int.Parse(context.Request.Form["clientId"].ToString());
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@floorLevel", floorLevel);
              sqlparams.Add("@liftPoint", liftPoint);
            sqlparams.Add("@projectId", projectId);
            sqlparams.Add("@floorId", floorId);
            sqlparams.Add("@clientPoint", clientPoint);
            sqlparams.Add("@nextPointName", areaPoints);

            string sql = "";
            if (id == 0)
            {
                //插入client
                sql = "insert into fv_client (clientPoint,clientName,projectId,floorLevel,floorId,nextPointId,nextPointName,isClient) values(@clientPoint,@clientPoint,@projectId,@floorLevel,@floorId,0,@nextPointName,1);";
               //插入lift
                sql += "insert into fv_client (clientPoint,clientName,projectId,floorLevel,floorId,nextPointId,nextPointName,isClient) values(@liftPoint,@liftPoint,@projectId,@floorLevel,@floorId,0,'',0);";
            }
            else
            {
                //c有nextName  l没有
                sql = "update fv_client set clientPoint=@clientPoint,clientName=@clientPoint,nextPointName=@nextPointName where id=" + id;
                sql += string.Format("; update fv_client set clientPoint=@liftPoint,clientName=@liftPoint where floorLevel={0} and projectId={1} and isClient=0",
                    floorLevel, projectId);
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
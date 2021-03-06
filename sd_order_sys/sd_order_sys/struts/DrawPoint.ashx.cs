﻿using System;
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
            string floorLevel = context.Request.Form["hidFloorId"].ToString();
            string projectBrandId = context.Request.Form["projectBrandId"].ToString();
              Dictionary<string, object> sqlparams = new Dictionary<string, object>();
              sqlparams.Add("@floorLevel", floorLevel);
              sqlparams.Add("@areaPoints", areaPoints);
            string sql = "";
            sql = "update fv_projectbrand set floorLevel=@floorLevel,areaPoints=@areaPoints,lastchangetime=now() where id=" + projectBrandId;
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
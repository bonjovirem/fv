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
    /// sbrand 的摘要说明
    /// </summary>
    public class sbrand : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string user = context.Session["person"] == null ? "nouser" : context.Session["person"].ToString();
            context.Response.ContentType = "text/plain";
            if ("nouser".Equals(user))

                context.Response.Redirect("/login.aspx");
            else
            {
                string key = context.Request["action"] == null ? "" : context.Request["action"].ToString();
                switch (key)
                {

                    case "query":
                        LoadMsg(context);
                        break;

                    //case "LoadMyRecivedNotice":
                    //    LoadMyRecivedNotice(context, opt);
                    //    break;
                    //case "LoadAllNotice":
                    //    LoadAllNotice(context, opt);
                    //    break;


                    //case "del":
                    //    DelRecord(context);
                    //    break;
                }
            }
        }
        /// <summary>
        /// 获得列表数据
        /// </summary>
        /// <param name="context"></param>
        private void LoadMsg(HttpContext context)
        {
            int page = context.Request["page"] != "" ? Convert.ToInt32(context.Request.Form["page"]) : 1;
            int size = context.Request["rows"] != "" ? Convert.ToInt32(context.Request.Form["rows"]) : 1;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int total = 0;
            builder.Append(@"SELECT * FROM fv_sysbrand");
            
            if (context.Request["cul"] == null && context.Request["where"] == null)
            {
                //return;
            }
            else
            {
                string where = context.Request["cul"].ToString() + " LIKE '%" + context.Request["where"].ToString() + "%'";
                builder.Append(" where " + where);
            }
            builder.Append(" LIMIT " + (page - 1) + "," + size);
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            DataTable dt = SqlManage.Query(builder.ToString(), sqlparams).Tables[0];
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //List<SOA.MODEL.DocumentModel> list = docmanage.DataTableToList(dt);

            dictionary.Add("total", dt.Rows.Count);
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)//每一行信息，新建一个Dictionary<string,object>,将该行的每列信息加入到字典
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            dictionary.Add("rows", list);
            dt.Dispose();
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            context.Response.Write(javascriptSerializer.Serialize(dictionary));
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
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
    /// SetFloor 的摘要说明
    /// </summary>
    public class SetFloor : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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

                    case "opt":
                        RecordAdd(context);
                        break;
                    case "rRecord":
                        DelRecord(context);
                        break;
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
            builder.Append(@"SELECT a.*,isnull(b.clientPoint) as hasClient FROM fv_floor a left join fv_client b on a.id=b.floorId ");

            if (context.Request["cul"] == null && context.Request["where"] == null && context.Request["projectid"] == null)
            {
                //return;
            }
            else
            {
               // string where = context.Request["cul"].ToString() + " LIKE '%" + context.Request["where"].ToString() + "%'";

                builder.Append(" where a.projectid=" + context.Request["projectid"]);
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
        /// <summary>
        /// 增更数据库
        /// </summary>
        /// <param name="context"></param>
        private void RecordAdd(HttpContext context)
        {
            string floorLevel = context.Request.Form["floorLevel"].ToString();
            string hidProId = context.Request.Form["hidProId"].ToString();
            int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@floorLevel", floorLevel);
            sqlparams.Add("@hidProId", hidProId);
            sqlparams.Add("@floorImg", "f" + floorLevel);
            string sql = "";
            if (id == 0)
                sql = "insert into fv_floor (projectId,floorLevel,floorImg,createTime,lastChangeTime) values(@hidProId,@floorLevel,@floorImg,now(),now())";
            else
                sql = "update fv_floor set projectId=@hidProId,floorLevel=@floorLevel,floorImg=@floorImg,lastChangeTime=now() where id=" + id;
            bool w = SqlManage.OpRecord(sql, sqlparams);
            string msg = "";
            if (w)

                msg = "suc";
            else
                msg = "数据库连接超时或出现未知错误";
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            context.Response.Write(javascriptSerializer.Serialize(msg));

        }
        /// <summary>
        /// 删除数据库记录
        /// </summary>
        /// <param name="context"></param>
        private void DelRecord(HttpContext context)
        {
            string where = context.Request["id"] == null ? "" : context.Request["id"].ToString();
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = "";
            string msg = "";
            bool w = false;
            if (where == "")
                msg = "数据库网络延迟";
            else
            {
                sql = "delete from fv_project where id in (" + where + ")";
                w = SqlManage.OpRecord(sql, sqlparams);
            }
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
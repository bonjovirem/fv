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
    /// projectBrandType_query 的摘要说明
    /// </summary>
    public class projectBrandType_query : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            string key = context.Request["action"] == null ? "" : context.Request["action"].ToString();
            switch (key)
            {

                case "query":
                    LoadMsg(context);
                    break;

                case "redo":
                    UpdateFromBase(context);
                    break;
                //case "add":
                //    RecordAdd(context);
                //    break;
                case "del":
                    DelRecord(context);
                    break;
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
            builder.Append(@"SELECT * FROM fv_projectbrandtype where projectId=" + context.Request["projectId"].ToString());
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
        /// 同步数据库
        /// </summary>
        /// <param name="context"></param>
        private void UpdateFromBase(HttpContext context)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string sql = "update fv_projectbrandtype p ,fv_sysbrand v set p.brandTypeImg=v.brandLogo where brandTypeName=v.brandName and p.projectid=" + int.Parse(context.Request["id"].ToString());
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
        /// 增更数据库
        /// </summary>
        /// <param name="context"></param>
        //private void RecordAdd(HttpContext context)
        //{
        //    string projectId = context.Request.Form["projectId"].ToString();
        //    string btName = context.Request.Form["btName"].ToString();
        //    string btOrder = context.Request.Form["btOrder"].ToString();
        //    string btImg = context.Request.Form["btImg"].ToString();
        //    string btBgcolor = context.Request.Form["btBgcolor"].ToString();
        //    string btIsShow = context.Request.Form["btIsShow"].ToString();
        //    int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
        //    Dictionary<string, object> sqlparams = new Dictionary<string, object>();
        //    sqlparams.Add("@projectId", projectId);
        //    sqlparams.Add("@brandTypeName", btName);
        //    sqlparams.Add("@brandTypeOrder", btOrder);
        //    sqlparams.Add("@brandTypeImg", btImg);
        //    sqlparams.Add("@brandTypeBackColor", btBgcolor);
        //    sqlparams.Add("@btIsShow", btIsShow);
        //    string sql = "";
        //    if (id == 0)
        //        sql = "insert into fv_projectbrandtype (projectId,brandTypeName,brandTypeOrder,brandTypeImg,isShow,brandTypeBackColor,createTime,lastChangeTime)" +
        //             "values(@projectId,@brandTypeName,@brandTypeOrder,@brandTypeImg,@btIsShow,@brandTypeBackColor,now(),now())";
        //    else
        //        sql = "update fv_projectbrandtype set brandTypeName=@brandTypeName,brandTypeOrder=@brandTypeOrder,brandTypeImg=@brandTypeImg,isShow=@btIsShow,brandTypeBackColor=@brandTypeBackColor,lastChangeTime=NOW() where id=" + id;
        //    bool w = SqlManage.OpRecord(sql, sqlparams);
        //    string msg = "";
        //    if (w)

        //        msg = "suc";
        //    else
        //        msg = "数据库连接超时或出现未知错误";
        //    JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
        //    context.Response.Write(javascriptSerializer.Serialize(msg));

        //}
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
                sql = "delete from fv_projectbrandtype where id in (" + where + ")";
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
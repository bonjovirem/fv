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
            //string user = context.Session["person"] == null ? "nouser" : context.Session["person"].ToString();
            //context.Response.ContentType = "text/plain";
            //if ("nouser".Equals(user))

            //    context.Response.Redirect("/login.aspx");
            //else
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
            builder.Append(@"SELECT a.id,a.projectId,a.floorLevel,a.floorImg,a.createTime,a.lastChangeTime,sum(case b.isClient when 1 then 2 when 0 then 1 else 0 end) as hasClient " +
            "FROM fv_floor a left join fv_client b on a.floorLevel=b.floorLevel and a.projectId=b.projectId ");


            builder.Append(" where a.projectid=" + context.Request["projectId"] + "  group by a.id,a.projectId,a.floorLevel,a.floorImg,a.createTime,a.lastChangeTime ");
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
<<<<<<< HEAD
            string img = "";
            string projectid = context.Request.Form["hidpro"].ToString();
            if (context.Request.Files["floorImg"] != null)
            {
                HttpPostedFile _upfile = context.Request.Files["floorImg"];
                if (!System.IO.Directory.Exists(context.Server.MapPath("~/" + projectid + "/floors")))//创建项目楼层图片目录
                {
                    System.IO.Directory.CreateDirectory(context.Server.MapPath("~/" + projectid + "/floors"));
                }
                _upfile.SaveAs(HttpContext.Current.Server.MapPath("~/images/logo.jpg")); //保存图片 
                img = context.Server.MapPath("~/" + projectid + "/floors") + "/" + _upfile.FileName;
            }
            else
                img = "";
            string floorLevel = context.Request.Form["floorLevel"].ToString();
            string hidProId = context.Request.Form["hidProId"].ToString();
=======
            HttpPostedFile _upfile = context.Request.Files["File1"];
            _upfile.SaveAs(HttpContext.Current.Server.MapPath("~/images/logo.jpg")); //保存图片       
            string floorLevel = context.Request.Form["floorLevel"].ToString();   //楼层是第几层
            string hidProId = context.Request.Form["hidProId"].ToString();   //隐藏的projectID 项目ID
>>>>>>> refs/remotes/wh344972164/master
            int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@floorLevel", floorLevel);
            sqlparams.Add("@hidProId", hidProId);
            sqlparams.Add("@floorImg", "f" + img);
            string sql = "";
            if (id == 0)  //用来标识是添加还是修改，如果有ID，则是修改（根据ID修改），如果为0则是添加
                sql = "insert into fv_floor (projectId,floorLevel,floorImg,createTime,lastChangeTime) values(@hidProId,@floorLevel,@floorImg,now(),now())";
            else
                if (img != "")
                    sql = "update fv_floor set projectId=@hidProId,floorLevel=@floorLevel,floorImg=@floorImg,lastChangeTime=now() where id=" + id;
                else
                {
                    sqlparams.Remove("@floorImg");
                    sql = "update fv_floor set projectId=@hidProId,floorLevel=@floorLevel,lastChangeTime=now() where id=" + id;
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
                sql = "delete from fv_floor where id in (" + where + ")";
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
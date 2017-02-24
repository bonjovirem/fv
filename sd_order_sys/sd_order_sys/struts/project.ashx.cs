using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SDorder.BLL;
using System.Data;
using System.IO;

namespace sd_order_sys.struts
{
    /// <summary>
    /// project 的摘要说明
    /// </summary>
    public class project : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    case "redo":
                        UpdateFromBase(context);
                        break;
                    //case "opt":
                    //    RecordAdd(context);
                    //    break;
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
            builder.Append(@"SELECT a.id,a.brandName ,a.brandImg,a.brandDesc ,a.brandLogo, a.brandVideo ,a.brandOrder, a.brandTypeId ,a.brandTypeName , a.projectId, a.isShow, a.isStar ,a.isShowWay ,a.fvUrl ,a.createTime, a.lastChangeTime,a.floorLevel,a.areaPoints,isnull(a.areaPoints) as hasArea,sum( case isnull(b.walkWay) when 0 then 1 else 0 end) as hasPath "
                + " FROM fv_projectBrand a left join fv_walkway b on a.id=b.projectBrandId where a.brandTypeId= " + context.Request["projectBtId"].ToString()
   + " GROUP BY a.id,a.brandName ,a.brandImg,a.brandDesc ,a.brandLogo, a.brandVideo ,a.brandOrder, a.brandTypeId ,a.brandTypeName , a.projectId, a.isShow, a.isStar ,a.isShowWay ,a.fvUrl ,a.createTime, a.lastChangeTime,a.floorLevel,a.areaPoints ");

            builder.Append(" order by lastChangeTime desc LIMIT " + (page - 1) + "," + size);
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
            string sql = "update fv_projectbrand p ,fv_sys_brand v set p.brandLogo=v.sys_logo where p.brandName=v.sys_nane and p.projectid=" + int.Parse(context.Request["id"].ToString());
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
        //    string bName = context.Request.Form["txtName"].ToString();
        //    string bImg = context.Request.Form["txtImg"].ToString();
        //    string bDesc = context.Request.Form["txtdsc"].ToString();
        //    string bLogo = context.Request.Form["txtlogo"].ToString();
        //    string bVideo = context.Request.Form["txtvideo"].ToString();
        //    string bOrder = context.Request.Form["brandOrder"].ToString();
        //    int brandTypeID = int.Parse(context.Request.Form["projectBtId"].ToString());
        //    string bTypeName = context.Request.Form["projectBtName"].ToString();
        //    int isShow = int.Parse(context.Request.Form["isShow"].ToString());
        //    int isStar = int.Parse(context.Request.Form["isStar"].ToString());
        //    int isShowWay = int.Parse(context.Request.Form["isShowWay"].ToString());
        //    string fvUrl = context.Request.Form["fvUrl"].ToString();
        //    int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
        //    Dictionary<string, object> sqlparams = new Dictionary<string, object>();
        //    sqlparams.Add("@projectId", projectId);
        //    sqlparams.Add("@brandName", bName);
        //    sqlparams.Add("@brandImg", bImg);
        //    sqlparams.Add("@brandDesc", bDesc);
        //    sqlparams.Add("@brandLogo", bLogo);
        //    sqlparams.Add("@brandVideo", bVideo);
        //    sqlparams.Add("@brandOrder", bOrder);
        //    sqlparams.Add("@brandTypeId", brandTypeID);
        //    sqlparams.Add("@brandTypeName", bTypeName);
        //    sqlparams.Add("@isShow", isShow);
        //    sqlparams.Add("@isStar", isStar);
        //    sqlparams.Add("@isShowWay", isShowWay);
        //    sqlparams.Add("@fvUrl", fvUrl);
        //    string sql = "";
        //    if (id == 0)
        //        sql = "insert into fv_projectBrand (brandName,brandImg,brandDesc,brandLogo,brandVideo,brandOrder,brandTypeId,brandTypeName,projectId,isShow,isStar,isShowWay,fvUrl,createTime,lastChangeTime) values(@brandName,@brandImg,@brandDesc,@brandLogo,@brandVideo,@brandOrder,@brandTypeId,@brandTypeName,@projectId,@isShow,@isStar,@isShowWay,@fvUrl,now(),now())";
        //    else
        //        sql = "update fv_projectBrand set brandName=@brandName,brandImg=@brandImg,brandDesc=@brandDesc,brandLogo=@brandLogo,brandVideo=@brandVideo,brandOrder=@brandOrder,brandTypeId=@brandTypeId,brandTypeName=@brandTypeName,isShow=@isShow,isStar=@isStar,isShowWay=@isShowWay,fvUrl=@fvUrl,lastChangeTime=NOW() where id=" + id;
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
                sql = "delete from fv_projectBrand where id in (" + where + ")";
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
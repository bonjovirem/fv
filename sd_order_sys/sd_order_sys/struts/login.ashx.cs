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
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)//登录验证方法
        {
            context.Response.ContentType = "text/plain";
            string uname = context.Request.Form["username"].ToString();
            string upwd = context.Request.Form["pwd"].ToString();
            //string code = context.Request.Form["yzm"].ToString();
            LoginContent login = new LoginContent();
            //if (code == (context.Session["randomcode"] == null ? "nulltext" : context.Session["randomcode"].ToString()))
            //{
            string sql = "select count(*) from fv_users where uname=@uname and upwd=@upwd";
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@uname", uname);
            sqlparams.Add("@upwd", upwd);
            if (Convert.ToInt32(SqlManage.Exists(sql, sqlparams)) > 0)
            {
                context.Session["person"] = uname;
                login.msg = "suc";
                login.url = "/sys/index.aspx";
            }
            else
            {
                login.msg = "用户不存在或用户名、密码错误";
                    login.url = "/login.aspx";
            }
            //SqlManage manage = new SqlManage();
            //DataTable dt = manage.GetDataSet(sql, parameter).Tables[0];
            //string type = dt.Rows[0]["visible"].ToString();
            //if (dt.Rows.Count > 0)
            //{
            //    CAS.Model.UserModel model = new CAS.Model.UserModel();
            //    model.uloginid = dt.Rows[0]["uloginid"].ToString();
            //    //model.office = dt.Rows[0]["office"].ToString();
            //    model.utype = dt.Rows[0]["utype"].ToString();
            //    context.Session["person"] = model;
            //    login.msg = "suc";
            //    login.url = "validateuser.aspx";

            //}
            //else
            //{
            //    login.msg = "用户不存在或用户名、密码错误";
            //    login.url = "/index.aspx";
            //}
            //}
            //else
            //{
            //  login.msg = "验证码错误";
            // login.url = "/index.aspx";
            // }
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            context.Response.Write(javascriptSerializer.Serialize(login));
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public class LoginContent
        {
            public string msg { get; set; }
            public string url { get; set; }
        }
    }
}
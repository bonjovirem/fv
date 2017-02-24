using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using SDorder.BLL;

namespace sd_order_sys.files
{
    public partial class editProjectType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] == null)
                    hidpro.Value = "0";//表示插入数据
                else
                {
                    hidpro.Value = Request["id"].ToString();

                }
                LoadInfo(id: int.Parse(hidpro.Value));
                Label2.Text = Request.QueryString["projectId"];
                Label1.Text = Request.QueryString["projectName"];
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(Server.MapPath(@"~/release" + txtName.Value)))
            //{
            //    Directory.CreateDirectory(Server.MapPath(@"~/release/" + txtName.Value));//创建项目根文件夹
            //    Directory.CreateDirectory(Server.MapPath(@"~/release/" + txtName.Value + "/images"));
            //}
            //Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            string logo = "";
            if (txtlogo.HasFile)
            {
                logo = @"/" + txtName.Value + "/images/" + txtlogo.FileName;
                txtlogo.SaveAs(Server.MapPath(@"~/release/" + txtName.Value + "/images/") + txtlogo.FileName);
            }
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@projectId", int.Parse(Label2.Text));
            sqlparams.Add("@brandTypeName", txtName.Value);
            sqlparams.Add("@brandTypeOrder", btOrder.Value);
            sqlparams.Add("@brandTypeImg", logo);
            sqlparams.Add("@brandTypeBackColor", "");
            sqlparams.Add("@btIsShow", "1");
            string sql = "";
            if (int.Parse(hidpro.Value) == 0)
                sql = "insert into fv_projectbrandtype (projectId,brandTypeName,brandTypeOrder,brandTypeImg,isShow,brandTypeBackColor,createTime,lastChangeTime)" +
                     "values(@projectId,@brandTypeName,@brandTypeOrder,@brandTypeImg,@btIsShow,@brandTypeBackColor,now(),now())";
            else
                if (logo == "")
                    sql = "update fv_projectbrandtype set brandTypeName=@brandTypeName,brandTypeOrder=@brandTypeOrder,isShow=@btIsShow,brandTypeBackColor=@brandTypeBackColor,lastChangeTime=NOW() where id=" + int.Parse(hidpro.Value);
                else
                    sql = "update fv_projectbrandtype set brandTypeName=@brandTypeName,brandTypeOrder=@brandTypeOrder,brandTypeImg=@brandTypeImg,isShow=@btIsShow,brandTypeBackColor=@brandTypeBackColor,lastChangeTime=NOW() where id=" + int.Parse(hidpro.Value);
            bool w = SqlManage.OpRecord(sql, sqlparams);
            if (w)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                    "alert('您操作成功！稍后自动跳转到列表页'); window.location='projectBrandType_query.aspx?projectId=" + Label2.Text + "&projectName=" + Label1.Text + "'", true);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "error",
     "alert('数据库连接异常！');", true);
        }
        private void LoadInfo(int id)
        {
            string sql = "select * from fv_projectbrandtype where id= " + id;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            if (table.Rows.Count > 0)
            {
                txtName.Value = table.Rows[0]["brandTypeName"].ToString();
                btOrder.Value = table.Rows[0]["brandTypeOrder"].ToString();

                // txtdesc.Value = table.Rows[0]["projectDesc"].ToString();
                // txtcity.Value = table.Rows[0]["projectCity"].ToString();
                // ddltype.SelectedValue = table.Rows[0]["sys_type"].ToString();
            }
        }
    }
}
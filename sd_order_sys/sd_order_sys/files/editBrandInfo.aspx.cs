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
    public partial class editBrandInfo : System.Web.UI.Page
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
                    LoadInfo(id: int.Parse(hidpro.Value));
                }
                //LoadControl();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Server.MapPath(@"~/brandTemplate")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/brandTemplate"));
            }
            string bName = txtName.Value;
            //string bImg = "";
            string bDesc = txtdesc.Value;
            string bLogo = txtlogo.FileName;
            string bVideo = txtvideo.FileName;
            int id = hidpro.Value == "0" ? 0 : int.Parse(hidpro.Value);
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@sys_nane", bName);
            //sqlparams.Add("@brandImg", bImg);
            sqlparams.Add("@sys_desc", bDesc);
            sqlparams.Add("@sys_logo", @"/brandTemplate/" + bLogo);
            sqlparams.Add("@sys_video", bVideo);
            sqlparams.Add("@sys_type", "");
            sqlparams.Add("@sys_typeName", "");
            if (txtlogo.HasFile)
            {
                txtlogo.SaveAs(Server.MapPath(@"~/brandTemplate/" + txtlogo.FileName));
            }
            //else if (txtvideo.HasFile)
            //{
            //    txtvideo.SaveAs(Server.MapPath(@"~/brandTemplate/" + txtvideo.FileName));
            //}
            string sql = "";
            if (id == 0)
                sql = "insert into fv_sys_brand (sys_nane,sys_desc,sys_logo,sys_video,sys_type,sys_typeName,createTime,lastChangeTime) values(@sys_nane,@sys_desc,@sys_logo,@sys_video,@sys_type,@sys_typeName,now(),now())";
            else
                sql = "update fv_sys_brand set sys_nane=@sys_nane,sys_desc=@sys_desc,sys_logo=@sys_logo,sys_video=@sys_video,sys_type=@sys_type,sys_typeName=@sys_typeName,lastChangeTime=NOW() where id=" + id;
            bool w = SqlManage.OpRecord(sql, sqlparams);
            if (w)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                    "alert('您操作成功！稍后自动跳转到列表页'); window.location='sysRecord.aspx'", true);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "error",
     "alert('数据库连接异常！');", true);
        }
        private void LoadInfo(int id)
        {
            string sql = "select * from fv_sys_brand where id= " + id;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            if (table.Rows.Count > 0)
            {
                txtName.Value = table.Rows[0]["sys_nane"].ToString();
                txtdesc.Value = table.Rows[0]["sys_desc"].ToString();
                //ddltype.SelectedValue = table.Rows[0]["sys_type"].ToString();
                logoImg.Src = table.Rows[0]["sys_logo"].ToString();
            }
        }
        //private void LoadControl()
        //{
        //    string sql = "select id,brandName from fv_sysbrand ";
        //    Dictionary<string, object> sqlparams = new Dictionary<string, object>();
        //    //sqlparams.Add("@id", id);
        //    DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
        //    ddltype.DataSource = table;
        //    ddltype.DataTextField = "brandName";
        //    ddltype.DataValueField = "id";
        //    ddltype.DataBind();
        //}
    }
}
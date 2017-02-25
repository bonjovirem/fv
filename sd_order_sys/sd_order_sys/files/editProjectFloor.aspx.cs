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
    public partial class editProjectFloor : System.Web.UI.Page
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
                if (Request["proId"] != null)
                {
                    ViewState["proId"] = Request["proId"].ToString();
                    //proId = Request.QueryString["projectId"];
                    Label1.Text = Request.QueryString["projectName"];
                    // LoadControl(int.Parse(Request["proId"].ToString()));
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Server.MapPath("~/release/" + ViewState["proId"].ToString() + "/images")))//创建项目楼层图片目录
            {
                Directory.CreateDirectory(Server.MapPath("~/release/" + ViewState["proId"].ToString() + "/images"));
            }
            string img = "";
            if (floorImg.HasFile)
            {
                img = @"/release/" + ViewState["proId"].ToString() + "/images/" + floorLevel.Value + "F" + GetExtension(floorImg.FileName);
                floorImg.SaveAs(Server.MapPath("~/release/" + ViewState["proId"].ToString() + "/images/" + floorLevel.Value + floorImg.FileName));
            }
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@floorLevel", floorLevel.Value);
            sqlparams.Add("@hidProId", ViewState["proId"]);
            sqlparams.Add("@floorImg", img);
            string sql = "";
            if (int.Parse(hidpro.Value) == 0)
                sql = "insert into fv_floor (projectId,floorLevel,floorImg,createTime,lastChangeTime) values(@hidProId,@floorLevel,@floorImg,now(),now())";
            else
                if (img != "")
                    sql = "update fv_floor set projectId=@hidProId,floorLevel=@floorLevel,floorImg=@floorImg,lastChangeTime=now() where id=" + int.Parse(hidpro.Value);
                else
                {
                    sqlparams.Remove("@floorImg");
                    sql = "update fv_floor set projectId=@hidProId,floorLevel=@floorLevel,lastChangeTime=now() where id=" + int.Parse(hidpro.Value);
                }
            bool w = SqlManage.OpRecord(sql, sqlparams);
            if (w)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                    "alert('您操作成功！稍后自动跳转到列表页'); window.location='setFloor.aspx?projectId=" + ViewState["proId"].ToString()
                    + "&projectName=" + Label1.Text + "'", true);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "error",
     "alert('数据库连接异常！');", true);
        }
        private void LoadInfo(int id)
        {
            string sql = "select * from fv_floor where id= " + id;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            if (table.Rows.Count > 0)
            {
                floorLevel.Value = table.Rows[0]["floorLevel"].ToString();
                //txtdesc.Value = table.Rows[0]["sys_desc"].ToString();
                //ddltype.SelectedValue = table.Rows[0]["sys_type"].ToString();
            }
        }
        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetExtension(string value)
        {
            return System.IO.Path.GetExtension(value).ToLower();
        }
    }
}
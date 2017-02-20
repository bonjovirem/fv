using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SDorder.BLL;
using System.IO;

namespace sd_order_sys.files
{
    public partial class editSysBrandInfo : System.Web.UI.Page
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
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Server.MapPath(@"~/brandTypeTemplate")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/brandTypeTemplate"));
            }
            string bName = txtName.Value;
            string bImg = "";
            string bDesc = txtdesc.Value;
            string bLogo = txtlogo.FileName;
            string bVideo = txtvideo.FileName;
            int id = hidpro.Value == "0" ? 0 : int.Parse(hidpro.Value);
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@brandName", bName);
            sqlparams.Add("@brandImg", bImg);
            sqlparams.Add("@brandDesc", bDesc);
            sqlparams.Add("@brandLogo", @"/brandTypeTemplate/" + bLogo);
            sqlparams.Add("@brandVideo", bVideo);
            if (txtlogo.HasFile)
            {
                txtlogo.SaveAs(Server.MapPath(@"~/brandTypeTemplate/" + txtlogo.FileName));
            }
            //else if (txtvideo.HasFile)
            //{
            //    txtvideo.SaveAs(Server.MapPath(@"~/brandTypeTemplate/" + txtvideo.FileName));
            //}
            string sql = "";
            if (id == 0)
                sql = "insert into fv_sysbrand (brandName,brandImg,brandDesc,brandLogo,brandVideo,createTime,lastChangeTime) values(@brandName,@brandImg,@brandDesc,@brandLogo,@brandVideo,now(),now())";
            else
                sql = "update fv_sysbrand set brandName=@brandName,brandImg=@brandImg,brandDesc=@brandDesc,brandLogo=@brandLogo,brandVideo=@brandVideo,lastChangeTime=NOW() where id=" + id;
            bool w = SqlManage.OpRecord(sql, sqlparams);
            if (w)
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                    "alert('您操作成功！稍后自动跳转到列表页'); window.location='sbrand_query.aspx'", true);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "error",
     "alert('数据库连接异常！');", true);
        }
        private void LoadInfo(int id)
        {
            string sql = "select * from fv_sysbrand where id= " + id;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            if (table.Rows.Count > 0)
            {
                txtName.Value = table.Rows[0]["brandName"].ToString();
                txtdesc.Value = table.Rows[0]["brandDesc"].ToString();
            }
        }
    }
}
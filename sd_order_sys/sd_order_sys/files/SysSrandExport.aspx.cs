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
    public partial class SysSrandExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                lblmsg.Text = "请选择模板";
                return;
            }
            lblmsg.Text = "";
            if (!Directory.Exists(Server.MapPath(@"~/brandTypeTemplate")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/brandTypeTemplate"));
            }
            FileUpload1.SaveAs(Server.MapPath(@"~/brandTypeTemplate/" + FileUpload1.FileName));
            string path = Server.MapPath(@"~/brandTypeTemplate/" + FileUpload1.FileName);
            string msg = ExportHelper.TypeExportDB(path: path);
            lblmsg.Text = msg;
            File.Delete(path);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SDorder.BLL;

namespace sd_order_sys.files
{
    public partial class projectTypeExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["projectId"] == null)
                    Response.Redirect("/login.aspx");
                else
                {
                    ViewState["projectId"] = Request["projectId"].ToString();
                    Label1.Text = Request["projectName"].ToString();
                    //LoadForm(id: int.Parse(Request["projectId"].ToString()));
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                lblmsg.Text = "请选择模板";
                return;
            }
            lblmsg.Text = "";
            if (!Directory.Exists(Server.MapPath(@"~/template")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/template"));
            }
            FileUpload1.SaveAs(Server.MapPath(@"~/template/" + FileUpload1.FileName));
            string path = Server.MapPath(@"~/template/" + FileUpload1.FileName);
            string msg = ExportHelper.TypeExportDB(path: path, projectId: int.Parse(ViewState["projectId"].ToString()));
            lblmsg.Text = msg;
            File.Delete(path);
        }
    }
}
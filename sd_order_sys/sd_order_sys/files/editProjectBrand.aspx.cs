using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using SDorder.BLL;
using ThoughtWorks.QRCode.Codec;

namespace sd_order_sys.files
{
    public partial class editProjectBrand : System.Web.UI.Page
    {
        public string projectId = "";
        public string projectName = "";
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
                if (Request["projectId"] != null)
                {
                    ViewState["proId"] = Request["projectId"].ToString();
                    LoadControl(int.Parse(Request["projectId"].ToString()));
                    lblpro.Text = Request.QueryString["projectName"];
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(Server.MapPath(@"~/brandTemplate")))
            //{
            //    Directory.CreateDirectory(Server.MapPath(@"~/" + ViewState["proId"].ToString() + @"/brandTemplate"));
            //}
            string bName = txtName.Value;
            //string bImg = "";
            string bDesc = txtdesc.Value;
            string bLogo = txtlogo.FileName;
            string bVideo = txtvideo.FileName;
            int isStar = int.Parse(ddlisStar.SelectedValue);
            int isShow = int.Parse(ddlisShow.SelectedValue);
            string url = fvUrl.Value;
            string tel = telephone.Value;
            string addr = address.Value;
            int id = hidpro.Value == "0" ? 0 : int.Parse(hidpro.Value);
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@projectId", int.Parse(ViewState["proId"].ToString()));
            sqlparams.Add("@brandName", bName);
            sqlparams.Add("@brandImg", "");
            sqlparams.Add("@brandDesc", bDesc);
            sqlparams.Add("@brandLogo", bLogo);
            sqlparams.Add("@brandVideo", bVideo);
            sqlparams.Add("@brandOrder", int.Parse(brandOrder.Value));
            sqlparams.Add("@brandTypeId", int.Parse(ddltype.SelectedValue));
            sqlparams.Add("@brandTypeName", ddltype.SelectedItem.Text);
            sqlparams.Add("@isShow", isShow);
            sqlparams.Add("@isStar", isStar);
            sqlparams.Add("@isShowWay", 0);
            sqlparams.Add("@fvUrl", url);
            sqlparams.Add("@telephone", tel);
            sqlparams.Add("@address", addr);
            if (txtlogo.HasFile)
            {
                txtlogo.SaveAs(Server.MapPath(@"~/release" + ViewState["ProId"].ToString() + "/images/" + txtlogo.FileName));
            }
            if (fvUrl.Value != "")
            {
                Build2DimensionalBarCode(fvUrl.Value);//生成二维码图片存放
            }
            //else if (txtvideo.HasFile)
            //{
            //    txtvideo.SaveAs(Server.MapPath(@"~/brandTemplate/" + txtvideo.FileName));
            //}
            string sql = "";
            if (id == 0)
                sql = "insert into fv_projectBrand (brandName,brandImg,brandDesc,brandLogo,brandVideo,brandOrder,brandTypeId,brandTypeName,projectId,isShow,isStar,isShowWay,fvUrl,createTime,lastChangeTime,telephone,address) values(@brandName,@brandImg,@brandDesc,@brandLogo,@brandVideo,@brandOrder,@brandTypeId,@brandTypeName,@projectId,@isShow,@isStar,@isShowWay,@fvUrl,now(),now(),@telephone,@address)";
            else
                sql = "update fv_projectBrand set brandName=@brandName,brandImg=@brandImg,brandDesc=@brandDesc,brandLogo=@brandLogo,brandVideo=@brandVideo,brandOrder=@brandOrder,brandTypeId=@brandTypeId,brandTypeName=@brandTypeName,isShow=@isShow,isStar=@isStar,fvUrl=@fvUrl,lastChangeTime=NOW(),telephone=@telephone,address=@address where id=" + id;
            bool w = SqlManage.OpRecord(sql, sqlparams);
            if (w)
                //                projectId = Request.QueryString["projectId"];
                // projectName = Request.QueryString["projectName"];
                // projectBtId = Request.QueryString["projectBtId"];
                //   projectBtName = Request.QueryString["projectBtName"];
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                    "alert('您操作成功！稍后自动跳转到列表页'); window.location='project_query.aspx?projectId=" +
                    ViewState["proId"].ToString() + "&projectName=" + lblpro.Text + "&projectBtId=" + ddltype.SelectedValue + "&projectBtName=" + ddltype.SelectedItem.Text + "'", true);
            else
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "error",
     "alert('数据库连接异常！');", true);
        }
        private void LoadInfo(int id)
        {
            string sql = "select * from fv_projectbrand where id= " + id;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            if (table.Rows.Count > 0)
            {
                txtName.Value = table.Rows[0]["brandName"].ToString();
                txtdesc.Value = table.Rows[0]["brandDesc"].ToString();
                ddltype.SelectedValue = table.Rows[0]["brandTypeId"].ToString();
            }
        }
        private void LoadControl(int pid)
        {
            string sql = "select id,brandTypeName from fv_projectbrandType where projectId=" + pid;
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            //sqlparams.Add("@id", id);
            DataTable table = SqlManage.Query(sql, sqlparams).Tables[0];
            ddltype.DataSource = table;
            ddltype.DataTextField = "brandTypeName";
            ddltype.DataValueField = "id";
            ddltype.DataBind();
            ddltype.SelectedValue = Request.QueryString["projectBtId"];
        }
        private void Build2DimensionalBarCode(string url)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            string encoding = "Byte";
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 0;

            string level = "H";

            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            //文字生成图片
            System.Drawing.Image image = qrCodeEncoder.Encode(url);

            image.Save(Server.MapPath(@"~/release" + ViewState["ProId"].ToString() + "/erweima/" + txtName.Value + ".png"), System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
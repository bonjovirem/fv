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

                if (Request["projectId"] != null)
                {
                    ViewState["proId"] = Request["projectId"].ToString();
                    LoadControl(int.Parse(Request["projectId"].ToString()));
                    lblpro.Text = Request.QueryString["projectName"];
                }
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
            if (!Directory.Exists(Server.MapPath(@"~/release/" + ViewState["proId"].ToString() + "/images")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/release/" + ViewState["proId"].ToString() + @"/images"));
            }
            string bName = txtName.Value;
            //string bImg = "";
            string timeSign = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
+ DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
+ DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
            string qrcode = "";
            if (fvUrl.Value != hidurl.Value && fvUrl.Value != "")
            {
                qrcode = BuildQrCode(fvUrl.Value, value: timeSign);//生成二维码图片存放
            }
            string phone = "";
            if (sphoneurl.Value != hidPhone.Value && sphoneurl.Value != "")
            {
                phone = BuildQrCode(sphoneurl.Value, value: timeSign);
            }
            string bDesc = txtdesc.Value;
            string bLogo = "";
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
            //sqlparams.Add("@brandLogo", bLogo);
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
            sqlparams.Add("@qrCode", qrcode == "" ? ImageQrcode.ImageUrl : qrcode);
            sqlparams.Add("@localvpath", localpath.Value);
            sqlparams.Add("sphone", phone == "" ? hidPhone.Value : phone);
            sqlparams.Add("@sphoneurl", sphoneurl.Value);
            if (txtlogo.HasFile)
            {
                bLogo = "../images/" + timeSign + txtlogo.FileName;
                sqlparams.Add("@brandLogo", bLogo);
                txtlogo.SaveAs(Server.MapPath(@"~/release/" + ViewState["proId"].ToString() + "/images/" + timeSign + txtlogo.FileName));
            }

            //else if (txtvideo.HasFile)
            //{
            //    txtvideo.SaveAs(Server.MapPath(@"~/brandTemplate/" + txtvideo.FileName));
            //}
            string sql = "";
            if (id == 0)
                sql = "insert into fv_projectBrand (brandName,brandImg,brandDesc,brandLogo,brandVideo,brandOrder,brandTypeId,brandTypeName,projectId,isShow,isStar,isShowWay,fvUrl,createTime,lastChangeTime,telephone,address,localvpath,sphone,sphoneurl) values(@brandName,@brandImg,@brandDesc,@brandLogo,@brandVideo,@brandOrder,@brandTypeId,@brandTypeName,@projectId,@isShow,@isStar,@isShowWay,@fvUrl,now(),now(),@telephone,@address,@localvpath,@sphone,@sphoneurl)";
            else
                if (bLogo != "")
                    sql = "update fv_projectBrand set brandName=@brandName,brandImg=@brandImg,brandDesc=@brandDesc,brandLogo=@brandLogo,brandVideo=@brandVideo,brandOrder=@brandOrder,brandTypeId=@brandTypeId,brandTypeName=@brandTypeName,isShow=@isShow,isStar=@isStar,fvUrl=@fvUrl,lastChangeTime=NOW(),telephone=@telephone,address=@address,qrcode=@qrcode,localvpath=@localvpath,sphone=@sphone,sphoneurl=@sphoneurl where id=" + id;
                else
                    sql = "update fv_projectBrand set brandName=@brandName,brandImg=@brandImg,brandDesc=@brandDesc,brandVideo=@brandVideo,brandOrder=@brandOrder,brandTypeId=@brandTypeId,brandTypeName=@brandTypeName,isShow=@isShow,isStar=@isStar,fvUrl=@fvUrl,lastChangeTime=NOW(),telephone=@telephone,address=@address,qrcode=@qrcode,localvpath=@localvpath,sphone=@sphone,sphoneurl=@sphoneurl where id=" + id;
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
                hidurl.Value = table.Rows[0]["fvUrl"].ToString();
                fvUrl.Value = table.Rows[0]["fvUrl"].ToString();
                string imgUrl = table.Rows[0]["brandLogo"].ToString();
                if (imgUrl.Contains(".."))
                {
                    ImageLogo.ImageUrl = Server.MapPath(@"~/release/" + ViewState["proId"].ToString())
                        + @"/" + imgUrl.Substring(2);
                }
                else

                    ImageLogo.ImageUrl = table.Rows[0]["brandLogo"].ToString();
                string qrcodeUrl = table.Rows[0]["qrcode"].ToString();
                if (qrcodeUrl.Contains(".."))
                {

                    ImageQrcode.ImageUrl = @"~/release/" + ViewState["proId"].ToString()
                        + @"/" + qrcodeUrl.Substring(2);
                }
                else

                    ImageQrcode.ImageUrl = table.Rows[0]["qrcode"].ToString();
                string spurl = table.Rows[0]["sphone"].ToString();
                if (spurl.Contains(".."))
                {

                    Image1.ImageUrl = @"~/release/" + ViewState["proId"].ToString()
                        + @"/" + spurl.Substring(2);
                }
                else

                    Image1.ImageUrl = table.Rows[0]["qrcode"].ToString();
                hidPhone.Value = table.Rows[0]["sphone"].ToString();
                ddlisShow.SelectedValue = table.Rows[0]["isShow"].ToString();
                localpath.Value = table.Rows[0]["localvpath"].ToString();
                sphoneurl.Value = table.Rows[0]["sphoneurl"].ToString();
                Text1.Value = table.Rows[0]["floorlevel"].ToString();
                telephone.Value = table.Rows[0]["telephone"].ToString();
                address.Value = table.Rows[0]["address"].ToString();
               // Image1.ImageUrl = table.Rows[0]["sphone"].ToString();
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
        /// <summary>
        /// 二维码生成
        /// </summary>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildQrCode(string url, string value)
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
            string name = Guid.NewGuid().ToString();
            System.Drawing.Image image = qrCodeEncoder.Encode(url);
            string path = Server.MapPath(@"~/release/" + ViewState["proId"].ToString() + "/images") + @"/" + name + ".png";
            image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            return "../images" + @"/" + name + ".png";
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "success",
                   "window.location='project_query.aspx?projectId=" +
                   ViewState["proId"].ToString() + "&projectName=" + lblpro.Text + "&projectBtId=" + ddltype.SelectedValue + "&projectBtName=" + ddltype.SelectedItem.Text + "'", true);
        }
    }
}
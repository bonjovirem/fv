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
    /// city 的摘要说明
    /// </summary>
    public class city : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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

                    case "opt":
                        RecordAdd(context);
                        break;
                    case "rRecord":
                        DelRecord(context);
                        break;
                    case "build":
                        BuildHTML(context);
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
            builder.Append(@"SELECT * FROM fv_project");

            if (context.Request["cul"] == null && context.Request["where"] == null)
            {
                //return;
            }
            else
            {
                string where = context.Request["cul"].ToString() + " LIKE '%" + context.Request["where"].ToString() + "%'";
                builder.Append(" where " + where);
            }
            builder.Append(" LIMIT " + (page - 1) + "," + size);
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
        /// 增更数据库
        /// </summary>
        /// <param name="context"></param>
        private void RecordAdd(HttpContext context)
        {
            string bName = context.Request.Form["txtName"].ToString();
            string bImg = context.Request.Form["txtImg"].ToString();
            string bDesc = context.Request.Form["txtdsc"].ToString();
            string bLogo = context.Request.Form["txtlogo"].ToString();
            string bVideo = context.Request.Form["txtvideo"].ToString();
            string bCity = context.Request.Form["txtcity"].ToString();
            string bFirst = context.Request.Form["txtfirst"].ToString();
            int id = context.Request.Form["hid"].ToString() == "" ? 0 : int.Parse(context.Request.Form["hid"].ToString());
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@projectName", bName);
            sqlparams.Add("@projectLogo", bLogo);
            sqlparams.Add("@projectDesc", bDesc);
            sqlparams.Add("@projectImg", bImg);
            sqlparams.Add("@projectVideo", bVideo);
            sqlparams.Add("@projectCity", bCity);
            sqlparams.Add("@projectFirstShow", bFirst);
            string sql = "";
            if (id == 0)
                sql = "insert into fv_project (projectName,projectLogo,projectDesc,projectImg,projectVideo,projectCity,projectFirstShow,createTime,lastChangeTime) values(@projectName,@projectLogo,@projectDesc,@projectImg,@projectVideo,@projectCity,@projectFirstShow,now(),now())";
            else
                sql = "update fv_project set projectName=@projectName,projectLogo=@projectLogo,projectDesc=@projectDesc,projectImg=@projectImg,projectVideo=@projectVideo,projectCity=@projectCity,projectFirstShow=@projectFirstShow,lastChangeTime=NOW() where id=" + id;
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
                sql = "delete from fv_project where id in (" + where + ")";
                w = SqlManage.OpRecord(sql, sqlparams);
            }
            if (w)
                msg = "suc";
            else
                msg = "数据库连接超时或出现未知错误";
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            context.Response.Write(javascriptSerializer.Serialize(msg));

        }

        private void BuildHTML(HttpContext context)
        {
            string msg = "";
            string sql = "";
            int id = context.Request["id"].ToString() == "" ? 0 : int.Parse(context.Request["id"].ToString());
            string thisClientFloorLevel = context.Request["floorLevel"].ToString();
            if (id == 0 || string.IsNullOrEmpty(thisClientFloorLevel))
            {
                msg = "";
            }
            else
            {

                //第一步复制文件  *floorLevel 1,2,3,4当前的楼层  *f1,2,3,4 指左侧导航的标记
                string sourcePath = context.Server.MapPath("../WebTemp/");
                string toPath = context.Server.MapPath("../release/2/f" + thisClientFloorLevel + "/");
                CopyDirectory(sourcePath, toPath);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                sql = "select floorLevel from fv_floor where projectid=" + id + " order by floorLevel";
                DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    //查找需要展示的信息 for  floor.html   0
                    sql = "select a.id,a.fvUrl,a.areaPoints,a.floorLevel,a.brandDesc,a.brandName from fv_projectbrand a " +
                           " where a.projectid=" + id + " and a.floorLevel is not null;";
                    //品类展示   1
                    sql += "select id,brandTypeName from fv_projectbrandtype where projectId=" + id + " limit 24 ;";
                    //品牌展示   2
                    sql += "select id,brandName,fvUrl,brandDesc,floorlevel from fv_projectbrand where isShowWay=1 and projectId=" + id + "  limit 12 ;";
                    //取全景展示的品牌   3
                    sql += "select id,brandName,fvUrl,brandDesc,floorlevel from fv_projectbrand where fvUrl<>'' and fvUrl is not null and floorLevel is not null and projectId=" + id + "  limit 48 ;";
                    //取到品牌的路径 for f.html  4
                    sql += "select b.projectBrandId,b.walkWay,a.IsClient,a.floorLevel from fv_client a,fv_walkway b where a.id=b.fromClientId and a.projectId=" + id + "; ";
                    //取C to lift 路径 for f.html  5
                    sql += "select a.floorLevel,a.nextPointName from fv_client a where a.isClient=1 and a.projectId=" + id;

                    DataSet ds = SqlManage.Query(sql, sqlparams);
                    DataTable dt2 = ds.Tables[0];
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string floorLevel = dt.Rows[i][0].ToString();
                            #region 处理floor页面
                            string oldFile = toPath + "floor.html";
                            string newFile = toPath + "floor" + floorLevel + ".html";
                            File.Copy(oldFile, newFile, true);
                            string code = readFile(newFile);
                            //替换楼层
                            code = code.Replace("*floorLevel", floorLevel);
                            //替换左侧显示信息
                            for (int j = 1; j < 5; j++)
                            {
                                code = code.Replace("*f" + j, (j.ToString() == floorLevel ? "f" + j + "s" : "f" + j));
                            }
                            //替换全景路径
                            string fvString = "";
                            string descString = "";
                            string areaString = "";
                            DataRow[] dr = dt2.Select("floorLevel=" + floorLevel);
                            foreach (DataRow item in dr)
                            {
                                fvString += string.Format("Arrayfv['{0}'] = '{1}';", item["id"].ToString(), item["fvUrl"].ToString());
                                descString += string.Format("ArrayDesc['{0}'] = '{1}';", item["id"].ToString(), item["brandDesc"].ToString().Replace("*空格*", "&nbsp;").Replace("*换行*", "<br/>"));
                                string areaCodes = item["areaPoints"].ToString().Replace("(", "").Replace(")", "").Replace(";", ",").Trim();
                                if (!string.IsNullOrEmpty(areaCodes))
                                {
                                    areaString += string.Format(" <area onclick='loadPanelDesc({0});showPanel();' shape='polygon' coords='{1}' alt='{2}' title='{2}'>"
                                  , item["id"].ToString(), areaCodes, item["brandName"].ToString());
                                }

                            }
                            code = code.Replace("//*fvString", fvString);
                            code = code.Replace("//*descString", descString);
                            code = code.Replace("//*areaString", areaString);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);

                            writeFile(newFile, code);
                            #endregion

                            #region 处理f页面
                            string brandWalkway = "";
                            //*brandWalkway
                            oldFile = toPath + "f.html";
                            newFile = toPath + "f" + floorLevel + ".html";
                            File.Copy(oldFile, newFile, true);
                            code = readFile(newFile);
                            DataTable dtF = ds.Tables[4];
                            if (dtF != null && dtF.Rows.Count > 0)
                            {
                                dr = dtF.Select("floorLevel=" + floorLevel);
                                foreach (DataRow item in dr)
                                {
                                    if (!string.IsNullOrEmpty(item["walkWay"].ToString().Trim()))
                                    {
                                        brandWalkway += string.Format("ArrayBrand['{0}'] = '{1}';",
                                            item["projectBrandId"].ToString() + "_" + item["IsClient"].ToString(), item["walkWay"].ToString());
                                    }

                                }
                            }
                            DataTable dtF2 = ds.Tables[5];
                            if (dtF2 != null && dtF2.Rows.Count > 0)
                            {
                                dr = dtF2.Select("floorLevel=" + floorLevel);
                                if (dr.Length > 0)
                                {
                                    brandWalkway += string.Format("ArrayBrand['CTOL'] = '{0}';", dr[0]["nextPointName"].ToString());  //nextPointName
                                }
                            }
                            code = code.Replace("*floorLevel", floorLevel);
                            code = code.Replace("//*brandWalkway", brandWalkway);
                            writeFile(newFile, code);
                            #endregion

                            #region 处理brand.html页面

                            string brandGuide1_6 = "";
                            string brandGuide7_12 = "";
                            string brandTypeGuide1_12 = "";
                            string brandTypeGuide13_24 = "";
                            //*brandWalkway
                            oldFile = toPath + "brand.html";
                            code = readFile(oldFile);
                            DataTable dt3 = ds.Tables[1];
                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                if (dt3.Rows.Count < 12)
                                {
                                    brandTypeGuide13_24 = "<div class='col-md-1'><a class='btn' href='#' role='button'>&nbsp;</a></div>"; //加内容占一行
                                }
                                for (int k = 0; k < dt3.Rows.Count; k++)
                                {
                                    if (k < 12)
                                    {
                                        brandTypeGuide1_12 += string.Format("<div class='col-md-1'><a class='btn btn-success' href='#' role='button'>{0}</a></div>", dt3.Rows[k]["brandTypeName"].ToString());
                                    }
                                    else
                                    {
                                        brandTypeGuide13_24 += string.Format("<div class='col-md-1'><a class='btn btn-success' href='#' role='button'>{0}</a></div>", dt3.Rows[k]["brandTypeName"].ToString());
                                    }

                                }
                            }
                            DataTable dt4 = ds.Tables[2];
                            if (dt4 != null && dt4.Rows.Count > 0)
                            {

                                for (int k = 0; k < dt4.Rows.Count; k++)
                                {
                                    if (k < 6)
                                    {
                                        brandGuide1_6 += string.Format("<div class='col-md-2'><img onclick=\"window.location='floor{1}.html?projectId={0}';\"  style='width:190px;height:190px;' src='brandImg/logo{0}.jpg' alt='...' class='img-circle'></div>", dt4.Rows[k]["id"].ToString(), dt4.Rows[k]["floorlevel"].ToString());
                                    }
                                    else
                                    {
                                        brandGuide7_12 += string.Format("<div class='col-md-2'><img onclick=\"window.location='floor{1}.html?projectId={0}';\"  style='width:190px;height:190px;' src='brandImg/logo{0}.jpg' alt='...' class='img-circle'></div>", dt4.Rows[k]["id"].ToString(), dt4.Rows[k]["floorlevel"].ToString());
                                    }

                                }
                            }
                            code = code.Replace("//*brandGuide1_6", brandGuide1_6);
                            code = code.Replace("//*brandGuide7_12", brandGuide7_12);
                            code = code.Replace("//*brandTypeGuide1_12", brandTypeGuide1_12);
                            code = code.Replace("//*brandTypeGuide13_24", brandTypeGuide13_24);
                            writeFile(oldFile, code);

                            #endregion

                            #region 处理vr.html页面

                            string vrString = "";
                            //*brandWalkway
                            oldFile = toPath + "vr.html";
                            code = readFile(oldFile);
                            DataTable dt5 = ds.Tables[3];
                            if (dt5 != null && dt5.Rows.Count > 0)
                            {
                                for (int l = 0; l < dt5.Rows.Count; l++)
                                {
                                    if (l % 6 == 0 && l > 0)
                                    {
                                        //空一行
                                        vrString += "<div class='row'><div class='col-md-1'>&nbsp;</div><div class='col-md-1'>&nbsp;</div> <div class='col-md-1'>&nbsp;</div><div class='col-md-1'>&nbsp;</div><div class='col-md-1'>&nbsp;</div> <div class='col-md-1'>&nbsp;</div>  <div class='col-md-1'>&nbsp;</div>   <div class='col-md-1'>&nbsp;</div> <div class='col-md-1'>&nbsp;</div>  <div class='col-md-1'>&nbsp;</div>div class='col-md-1'>&nbsp;</div>   <div class='col-md-1'>&nbsp;</div>    </div>";
                                    }
                                    if (l % 6 == 0)
                                    {
                                        vrString += "<div class='row'>";
                                    }
                                    vrString += string.Format("<div class='col-md-2'><img onclick=\"window.location='floor{1}.html?projectId={0}';\"  style='width:190px;height:190px;' src='brandImg/logo{0}.jpg' alt='...' class='img-circle'></div>", dt5.Rows[l]["id"].ToString(), dt5.Rows[l]["floorlevel"].ToString());
                                    if (l % 6 == 5 || l == dt5.Rows.Count - 1)
                                    {
                                        vrString += "</div>";
                                    }

                                }
                            }
                            code = code.Replace("//*vrString", vrString);
                            writeFile(oldFile, code);
                            #endregion
                        }
                    }
                }
                if (true)

                    msg = "suc";
                else
                    msg = "数据库连接超时或出现未知错误";
                JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
                context.Response.Write(javascriptSerializer.Serialize(msg));
            }


        }


        public void CopyDirectory(string sourceDirName, string destDirName)
        {
            try
            {
                //Directory.Delete(destDirName,true);
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
                }

                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                    destDirName = destDirName + Path.DirectorySeparatorChar;

                string[] files = Directory.GetFiles(sourceDirName);
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }

                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                //StreamWriter sw = new StreamWriter(Application.StartupPath + "\\log.txt", true);
                //sw.Write(ex.Message + "     " + DateTime.Now + "\r\n");
                //sw.Close();
            }
        }

        public string readFile(string path)
        {
            string str = "";
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            str = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return str;
        }

        public void writeFile(string path, string txt)
        {
            string str = "";
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(txt);
            sw.Flush();
            sw.Close();
            fs.Close();

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
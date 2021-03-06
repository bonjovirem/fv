﻿using System;
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
                        BuildHTML2(context);
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
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            int total = 0;
            if (context.Request["cul"] == null && context.Request["where"] == null)
            {
                total = SqlManage.Query(@"SELECT * FROM fv_project", sqlparams).Tables[0].Rows.Count;
                //return;
            }
            else
            {
                string where = context.Request["cul"].ToString() + " LIKE '%" + context.Request["where"].ToString() + "%'";
                total = SqlManage.Query(@"SELECT * FROM fv_project where " + where, sqlparams).Tables[0].Rows.Count;
                builder.Append(" where " + where);
            }
            builder.Append(" order by lastChangeTime LIMIT " + (page - 1) * size + "," + size);

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
            int id = context.Request["id"].ToString() == "" ? 0 : int.Parse(context.Request["id"].ToString()); //projectId
            string thisClientFloorLevel = context.Request["floorLevel"].ToString();
            if (id == 0 || string.IsNullOrEmpty(thisClientFloorLevel))
            {
                msg = "";
            }
            else
            {

                //第一步复制文件  *floorLevel 1,2,3,4当前的楼层  *f1,2,3,4 指左侧导航的标记
                string sourcePath = context.Server.MapPath("../WebTemp/");
                string toPath = context.Server.MapPath("../release/" + id + "/f" + thisClientFloorLevel + "/");
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


                            oldFile = toPath + "PhonePath.html";
                            code = readFile(oldFile);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);
                            writeFile(oldFile, code);



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

        /// <summary>
        /// 用于第二版模板生成
        /// </summary>
        /// <param name="context"></param>
        private void BuildHTML2(HttpContext context)
        {
            string msg = "";
            string sql = "";
            int id = context.Request["id"].ToString() == "" ? 0 : int.Parse(context.Request["id"].ToString()); //projectId
            string thisClientFloorLevel = context.Request["floorLevel"].ToString();
            if (id == 0 || string.IsNullOrEmpty(thisClientFloorLevel))
            {
                msg = "";
            }
            else
            {

                //第一步复制文件  *floorLevel 1,2,3,4当前的楼层  *f1,2,3,4 指左侧导航的标记
                string sourcePath = context.Server.MapPath("../WebTemp2/");
                string toPath = context.Server.MapPath("../release/" + id + "/f" + thisClientFloorLevel + "/");
                if (Directory.Exists(toPath))
                {
                    Directory.Delete(toPath, true);
                }
                CopyDirectory(sourcePath, toPath);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                sql = "select floorLevel from fv_floor where projectid=" + id + " order by floorLevel";
                DataTable dt = SqlManage.Query(sql, sqlparams).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    //查找需要展示的信息 for  floor.html   0
                    sql = "select a.id,a.fvUrl,a.areaPoints,a.floorLevel,a.brandDesc,a.brandName,a.telephone,a.address,a.isShow,a.brandLogo,a.qrCode,a.localvpath,a.sphone from fv_projectbrand a " +
                           " where a.projectid=" + id + " ;";  //and a.floorLevel is not null
                    //品类展示   1
                    sql += "select id,brandTypeName,brandTypeImg from fv_projectbrandtype where projectId=" + id + " and isShow =1  ;";
                    //品牌展示   2
                    sql += "select id,brandName,fvUrl,brandDesc,floorlevel,brandLogo,brandTypeId from fv_projectbrand where projectId=" + id + " order by brandOrder ;";
                    //取全景展示的品牌   3
                    sql += "select id,brandName,fvUrl,brandDesc,floorlevel,brandLogo,brandTypeId from fv_projectbrand where fvUrl<>'' and fvUrl is not null and floorLevel is not null and projectId=" + id + ";";
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
                                code = code.Replace("*f" + j, (j.ToString() == floorLevel ? j + "_2" : j + "_1"));
                            }
                            //替换全景路径
                            string areaString = "";

                            DataRow[] dr = dt2.Select("floorLevel=" + floorLevel);
                            foreach (DataRow item in dr)
                            {
                                //fvString += string.Format("Arrayfv['{0}'] = '{1}';", item["id"].ToString(), item["fvUrl"].ToString());
                                //descString += string.Format("ArrayDesc['{0}'] = '{1}';", item["id"].ToString(), item["brandDesc"].ToString().Replace("*空格*", "&nbsp;").Replace("*换行*", "<br/>"));
                                //telephoneString += string.Format("ArrayTele['{0}'] = '{1}';", item["id"].ToString(), item["telephone"].ToString());
                                //addressString += string.Format("ArrayAddress['{0}'] = '{1}';", item["id"].ToString(), item["address"].ToString());
                                string areaCodes = item["areaPoints"].ToString().Replace("(", "").Replace(")", "").Replace(";", ",").Trim();
                                if (!string.IsNullOrEmpty(areaCodes))
                                {
                                    areaString += string.Format(" <area onclick='loadPanelDesc({0});showPanel();' shape='polygon' coords='{1}' alt='{2}' title='{2}'>"
                                  , item["id"].ToString(), areaCodes, item["brandName"].ToString());
                                }

                            }
                            DataRow[] drIsShow = dt2.Select("floorLevel=" + floorLevel + " and isShow=1");
                            string strIsShow = "";
                            for (int drIndex = 0; drIndex < 10; drIndex++)
                            {
                                if (drIndex % 2 == 0)
                                {
                                    strIsShow += @"<tr>";
                                }
                                if (drIndex >= drIsShow.Length)
                                {
                                    strIsShow += @" <td height='110' align='center' valign='middle'></td>";
                                }
                                else
                                {
                                    string fTemp = drIsShow[drIndex]["brandLogo"].ToString();
                                    int fIndex = fTemp.LastIndexOf('/');
                                    string fName = fTemp.Substring(fIndex + 1);
                                    strIsShow += @" <td height='110' align='center' valign='middle'><img src='../images/" +
                                        fName + "' onclick='loadPanelDesc(" + drIsShow[drIndex]["id"].ToString() + ");showPanel();' width='116' height='93' /></td>";
                                }
                                if (drIndex % 2 == 1)
                                {
                                    strIsShow += @"</tr>";
                                }
                            }

                            //code = code.Replace("//*fvString", fvString);
                            //code = code.Replace("//*descString", descString);
                            //code = code.Replace("//*telephoneString", fvString);
                            //code = code.Replace("//*addressString", descString);
                            code = code.Replace("//*isShow", strIsShow);
                            code = code.Replace("//*areaString", areaString);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);



                            writeFile(newFile, code);
                            #endregion

                            #region 处理data.js页面
                            oldFile = toPath + "/js/data.js";
                            code = readFile(oldFile);
                            //替换基础信息
                            string fvString = "";
                            string descString = "";
                            string telephoneString = "";
                            string addressString = "";
                            string brandLogo = "";
                            string brandQrCode = "";
                            string sendToPhone = "";
                            foreach (DataRow item in dt2.Rows)
                            {
                                fvString += string.Format("Arrayfv['{0}'] = '{1}';", item["id"].ToString(), item["localvpath"].ToString());
                                descString += string.Format("ArrayDesc['{0}'] = '{1}';", item["id"].ToString(), item["brandDesc"].ToString().Replace("*空格*", "&nbsp;").Replace("*换行*", "<br/>"));
                                telephoneString += string.Format("ArrayTele['{0}'] = '{1}';", item["id"].ToString(), item["telephone"].ToString());
                                addressString += string.Format("ArrayAddress['{0}'] = '{1}';", item["id"].ToString(), item["address"].ToString());
                                brandQrCode += string.Format("ArrayQrCode['{0}'] = '{1}';", item["id"].ToString(), item["qrCode"].ToString());
                                sendToPhone += string.Format("ArraySendToPhone['{0}'] = '{1}';", item["id"].ToString(), item["sPhone"].ToString());

                                //复制品牌图片文件
                                string fTemp = item["brandLogo"].ToString();
                                int fIndex = fTemp.LastIndexOf('/');
                                string fName = fTemp.Substring(fIndex + 1);

                                brandLogo += string.Format("ArrayLogo['{0}'] = '{1}';", item["id"].ToString(), "../images/" + fName);
                            }
                            code = code.Replace("//*fvString", fvString);
                            code = code.Replace("//*descString", descString);
                            code = code.Replace("//*telephoneString", telephoneString);
                            code = code.Replace("//*addressString", addressString);
                            code = code.Replace("//*brandLogo", brandLogo);
                            code = code.Replace("//*brandQrCode", brandQrCode);
                            code = code.Replace("//*sendToPhone", sendToPhone);
                            writeFile(oldFile, code);
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


                            oldFile = toPath + "PhonePath.html";
                            code = readFile(oldFile);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);
                            writeFile(oldFile, code);



                            #region 处理brand.html页面

                            string strBrandType = "";
                            string strBrand = "";
                            string btIndex = "";

                            oldFile = toPath + "brand.html";
                            code = readFile(oldFile);

                            DataTable dt3 = ds.Tables[1];
                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                int rowCount = 10;  //默认一行10列
                                int liCount = 3;
                                int btTemp = dt3.Rows.Count % rowCount;
                                int Count = btTemp == 0 ? dt3.Rows.Count / rowCount : (dt3.Rows.Count / rowCount) + 1;
                                while (Count % liCount != 0)
                                {
                                    Count++;
                                }
                                for (int j = 0; j < Count * rowCount; j++)
                                {

                                    //if (j % (liCount * rowCount) == 0)
                                    //{
                                    //    if (j == 0)
                                    //    {
                                    //        btIndex += " <li class='current'><em>1</em></li>";
                                    //    }
                                    //    else
                                    //    {
                                    //        btIndex += " <li><em>" + ((j / (liCount * rowCount)) + 1) + "</em></li>";
                                    //    }
                                    //}


                                    if (j % rowCount == 0)
                                    {
                                        if ((j / rowCount) % liCount == 0)
                                        {
                                            strBrandType += @"<table align='center'  width='95%' border='0' cellspacing='0' cellpadding='0'>";
                                        }
                                        strBrandType += @"<tr>";
                                    }
                                    if (j >= dt3.Rows.Count)
                                    {
                                        strBrandType += string.Format(@" <td width='90' height='90' align='center' valign='middle'></td>");
                                    }
                                    else
                                    {
                                        string fTemp = dt3.Rows[j]["brandTypeImg"].ToString();
                                        int fIndex = fTemp.LastIndexOf('/');
                                        string fName = fTemp.Substring(fIndex + 1);
                                        strBrandType += string.Format(@" <td width='90' height='90' align='center' valign='middle'><img src='{0}' width='100' height='45' onclick='showThis({1});' /></td>",
                                            "../images/" + fName, dt3.Rows[j]["id"].ToString());

                                    }
                                    if (j % rowCount == rowCount - 1)
                                    {
                                        strBrandType += @"</tr>";
                                        if ((j / rowCount) % liCount == liCount - 1)
                                        {
                                            strBrandType += @"</table>";
                                        }
                                    }
                                }
                            }
                            DataTable dt4 = ds.Tables[2];
                            if (dt4 != null && dt4.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt4.Rows.Count; j++)
                                {
                                    string fTemp = dt4.Rows[j]["brandLogo"].ToString();
                                    int fIndex = fTemp.LastIndexOf('/');
                                    string fName = fTemp.Substring(fIndex + 1);
                                    strBrand += string.Format(@"<div style='float:left' width='192' class='allType type{1}' height='192' align='center' valign='middle'><img src='{0}' onclick='loadPanelDescJump({2},{3});showPanel();' width='182' height='160' class='box-shadow' /></div>"
                                          , "../images/" + fName, dt4.Rows[j]["brandTypeId"].ToString(), dt4.Rows[j]["id"].ToString(), dt4.Rows[j]["floorLevel"].ToString());
                                }
                                //int rowCount = 7;  //默认一行10列
                                //int btTemp = dt4.Rows.Count % rowCount;
                                //int Count = btTemp == 0 ? (dt4.Rows.Count / rowCount) : (dt4.Rows.Count / rowCount) + 1;
                                //for (int j = 0; j < Count * rowCount; j++)
                                //{
                                //    if (j % rowCount == 0)
                                //    {
                                //        strBrand += @"<tr>";
                                //    }
                                //    if (j >= dt4.Rows.Count)
                                //    {
                                //        strBrand += string.Format(@"<td width='192' height='192' align='center' valign='middle'></td>");
                                //    }
                                //    else
                                //    {
                                //        string fTemp = dt4.Rows[j]["brandLogo"].ToString();
                                //        int fIndex = fTemp.LastIndexOf('/');
                                //        string fName = fTemp.Substring(fIndex + 1);
                                //        strBrand += string.Format(@"<td width='192' class='allType type{1}' height='192' align='center' valign='middle'><img src='{0}' onclick='loadPanelDesc({2});showPanel();' width='182' height='160' class='box-shadow' /></td>"
                                //             , "../images/" + fName, dt4.Rows[j]["brandTypeId"].ToString(), dt4.Rows[j]["id"].ToString());

                                //        //复制品牌图片文件,此处复制的全
                                //        string fOld = context.Server.MapPath(@"../brandTemplate/" + fName).Replace(@"\", "/");
                                //        string fNew = context.Server.MapPath(@"../release/" + id + "/images/" + fName).Replace(@"\", "/");
                                //        if (File.Exists(fOld))
                                //        {
                                //            File.Copy(fOld, fNew, true);
                                //        }

                                //    }
                                //    if (j % rowCount == rowCount - 1)
                                //    {
                                //        strBrand += @"</tr>";
                                //    }
                                //}

                            }
                            code = code.Replace("*btIndex", btIndex);
                            code = code.Replace("*floorLevel", floorLevel);
                            code = code.Replace("//*brandType", strBrandType);
                            code = code.Replace("//*brand", strBrand);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);
                            writeFile(oldFile, code);

                            #endregion

                            #region 处理vr.html页面

                            string vrString = "";
                            //*brandWalkway
                            oldFile = toPath + "vr.html";
                            code = readFile(oldFile);
                            strBrandType = "";
                            strBrand = "";


                            dt3 = ds.Tables[1];
                            if (dt3 != null && dt3.Rows.Count > 0)
                            {
                                int rowCount = 4;  //默认一行10列
                                int btTemp = dt3.Rows.Count % rowCount;
                                int Count = btTemp == 0 ? dt3.Rows.Count / rowCount : (dt3.Rows.Count / rowCount) + 1;
                                for (int j = 0; j < Count * rowCount; j++)
                                {
                                    if (j % rowCount == 0)
                                    {
                                        strBrandType += @"<tr>";
                                    }
                                    if (j >= dt3.Rows.Count)
                                    {
                                        strBrandType += string.Format(@" <td width='90' height='90' align='center' valign='middle'></td>");
                                    }
                                    else
                                    {
                                        string fTemp = dt3.Rows[j]["brandTypeImg"].ToString();
                                        int fIndex = fTemp.LastIndexOf('/');
                                        string fName = fTemp.Substring(fIndex + 1);
                                        strBrandType += string.Format(@" <td width='90' height='90' align='center' valign='middle'><img src='{0}' width='100' height='45' onclick='showThis({1});' /></td>",
                                             "../images/" + fName, dt3.Rows[j]["id"].ToString());
                                    }
                                    if (j % rowCount == rowCount - 1)
                                    {
                                        strBrandType += @"</tr>";
                                    }
                                }
                            }
                            DataTable dt5 = ds.Tables[3];
                            if (dt5 != null && dt5.Rows.Count > 0)
                            {

                                for (int j = 0; j < dt5.Rows.Count; j++)
                                {
                                    string fTemp = dt5.Rows[j]["brandLogo"].ToString();
                                    int fIndex = fTemp.LastIndexOf('/');
                                    string fName = fTemp.Substring(fIndex + 1);
                                    strBrand += string.Format(@"<div style='float:left' width='192' class='allType type{1}' height='192' align='center' valign='middle'><img src='{0}' onclick='loadPanelDescJump({2},{3});showPanel();' width='182' height='160' class='box-shadow' /></div>"
                                          , "../images/" + fName, dt5.Rows[j]["brandTypeId"].ToString(), dt5.Rows[j]["id"].ToString(), dt5.Rows[j]["floorLevel"].ToString());
                                }

                                //int rowCount = 5;  //默认一行10列
                                //int btTemp = dt5.Rows.Count % rowCount;
                                //int Count = btTemp == 0 ? (dt5.Rows.Count / rowCount) : (dt5.Rows.Count / rowCount) + 1;
                                //for (int j = 0; j < Count * rowCount; j++)
                                //{
                                //    if (j % rowCount == 0)
                                //    {
                                //        strBrand += @"<tr>";
                                //    }
                                //    if (j >= dt5.Rows.Count)
                                //    {
                                //        strBrand += string.Format(@"<td width='192' height='192' align='center' valign='middle'></td>");
                                //    }
                                //    else
                                //    {
                                //        string fTemp = dt5.Rows[j]["brandLogo"].ToString();
                                //        int fIndex = fTemp.LastIndexOf('/');
                                //        string fName = fTemp.Substring(fIndex + 1);
                                //        strBrand += string.Format(@"<td width='192' class='allType type{1}' height='192' align='center' valign='middle'><img src='{0}' onclick='loadPanelDesc({2});showPanel();' width='182' height='160' class='box-shadow' /></td>"
                                //             , "../images/" + fName, dt5.Rows[j]["brandTypeId"].ToString(), dt5.Rows[j]["id"].ToString());
                                //    }
                                //    if (j % rowCount == rowCount - 1)
                                //    {
                                //        strBrand += @"</tr>";
                                //    }
                                //}

                            }
                            code = code.Replace("*floorLevel", floorLevel);
                            code = code.Replace("//*brandType", strBrandType);
                            code = code.Replace("//*brand", strBrand);
                            code = code.Replace("//*thisClientFloorLevel", thisClientFloorLevel);
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
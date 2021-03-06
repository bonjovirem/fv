﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace SDorder.BLL
{
    public static class ExportHelper //导出工具类  
    {
        /// <summary>
        /// 数据写入模板类
        /// </summary>
        public static void WriteToTemplate()
        {
        }
        /// <summary>
        /// 创建项目目录
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <param name="projectName">项目名称</param>
        public static void CreateFile(string path, string projectName)
        {
            if (!Directory.Exists(path + @"/project/" + projectName))
            {
                Directory.CreateDirectory(path + @"/project/" + projectName);
            }
            //将模板复制到项目文件夹中
            List<string> templateFiles = new List<string>();//模板文件集合
            string templatePath = path + @"/template/";
            string projectPath = path + @"/project/" + projectName;
            foreach (string name in templateFiles)
            {
                File.Copy(templatePath + name, projectPath + name, true);
            }
        }
        /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="path">excel路径</param>  
        /// <param name="isColumnName">第一行是否是列名</param>  
        /// <returns>返回datatable</returns> 
        public static bool SwapValue()
        {
            bool w = true;
            return w;
        }
        /// <summary>
        /// 模板文件上传导入数据库
        /// </summary>
        public static DataTable UploadExcel(string path, bool isColumnName)
        {
            bool w = true;
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(path))
                {
                    // 2007版本  
                    if (path.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (path.IndexOf(".xls") > 0)
                        workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数  
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行  
                                int cellCount = firstRow.LastCellNum;//列数  

                                //构建datatable的列  
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行  
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return dataTable;
        }
        /// <summary>
        /// 项目品牌导入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static string BullToDB(string path, int projectId)
        {
            string msg = "";
            try
            {
                DataTable table = UploadExcel(path: path, isColumnName: true);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                //sqlparams.Add("@projectId", projectId);
                DataTable typeTable = SqlManage.Query("SELECT id,brandTypeName FROM fv_projectbrandtype where projectId=" + projectId, sqlparams).Tables[0];//拉取
                if (typeTable.Rows.Count == 0)
                {
                    msg = "该项目无品类，导入操作失败";
                }
                else
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        int order = int.Parse(dr[0].ToString());//品牌排序
                        string typeName = dr[1].ToString();//品类名称
                        string bName = dr[2].ToString();
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param.Add("@brandName", bName);
                        string validateSql = "select count(*) from fv_projectbrand where brandName='" + bName + "' and projectId="+projectId;
                        object o = SqlManage.Exists(validateSql, sqlparams);
                        if (int.Parse(o.ToString()) > 0)
                        //if(false)
                        {
                            msg += "品牌名称：" + bName + "的行写入失败，已有该品牌,";
                            continue;//已有该品牌则跳过
                        }
                        else //只加入存在于已导入的品类
                        {
                            var row = (from p in typeTable.AsEnumerable().Where(p => { return p.Field<string>("brandTypeName") == typeName; })
                                       select p).FirstOrDefault();

                            if (row != null)
                            {
                                DataTable dt = SqlManage.Query("select max(brandOrder) from fv_projectbrand where projectId=" + projectId + " and brandTypeId=" + int.Parse(row[0].ToString()), sqlparams).Tables[0];
                                int maxOrder = 0;
                                if (dt.Rows.Count == 0)
                                    maxOrder = 0;
                                else
                                    maxOrder = int.Parse(dt.Rows[0][0] is DBNull ? "0" : dt.Rows[0][0].ToString());
                                dt.Dispose();
                                param.Add("@brandOrder", order + maxOrder);
                                param.Add("@brandTypeId", row[0].ToString());
                                param.Add("@brandTypeName", typeName);
                                param.Add("@projectId", projectId);
                                string sql = "insert into fv_projectbrand (brandName,brandOrder,brandTypeId,brandTypeName,projectId,createTime,lastChangeTime) values(@brandName,@brandOrder,@brandTypeId,@brandTypeName,@projectId,now(),now()) ";
                                bool w = SqlManage.OpRecord(sql, param);
                                if (!w)
                                {
                                    msg += "品牌名称：" + bName + "的行写入失败，发生数据库阻塞,";
                                }
                            }
                            else
                            {
                                msg += "品牌名称：" + bName + "的行写入失败，无对应品类,";
                            }

                        }

                    }
                }
            }
            catch (SystemException e)
            {
                msg += e.Message + ",";

            }
            if (msg == "")
                msg = "品牌导入成功,";
            return msg.TrimEnd(',');
        }
        /// <summary>
        /// 项目品类导入数据库
        /// </summary>
        /// <param name="path"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static string TypeExportDB(string path, int projectId)
        {
            string msg = "";
            try
            {
                DataTable table = UploadExcel(path: path, isColumnName: true);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                //sqlparams.Add("@projectId", projectId);
                DataTable typeTable = SqlManage.Query("SELECT id,brandTypeName,brandTypeOrder FROM fv_projectbrandtype where projectId=" + projectId, sqlparams).Tables[0];//拉取
                int order = 0;
                //if (typeTable.Rows.Count != 0)
                //    order = (from p in typeTable.AsEnumerable().Select(p => p.Field<int>("brandTypeOrder")) select p).Max();
                foreach (DataRow dr in table.Rows)
                {
                    order = int.Parse(dr[0].ToString());//品类排序
                    string typeName = dr[1].ToString();//品类名称
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    var row = (from p in typeTable.AsEnumerable().Where(p => { return p.Field<string>("brandTypeName") == typeName; })
                               select p).FirstOrDefault();

                    if (row == null)
                    {
                        //typeTable.Rows.Add(row);
                        //param.Add("@brandTypeId", row[0].ToString());
                        param.Add("@projectId", projectId);
                        param.Add("@brandTypeName", typeName);
                        param.Add("@brandTypeOrder", order);
                        string sql = "insert into fv_projectbrandtype (projectId,brandTypeName,brandTypeOrder,createTime,lastChangeTime) values(@projectId,@brandTypeName,@brandTypeOrder,now(),now()) ";
                        bool w = SqlManage.OpRecord(sql, param);
                        if (!w)
                        {
                            msg += "品类名称：" + typeName + "的行写入失败，发生数据库阻塞,";
                        }
                    }
                    else
                    {
                        msg += "品牌名称：" + typeName + "的行添加失败，已有对应品类,";
                    }

                    // }

                }

            }
            catch (SystemException e)
            {
                msg += e.Message + ",";

            }
            if (msg == "")
                msg = "品类导入成功,";
            return msg.TrimEnd(',');
        }
        /// <summary>
        /// 系统品类导入数据库
        /// </summary>
        /// <param name="path"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static string TypeExportDB(string path)
        {
            string msg = "";
            try
            {
                DataTable table = UploadExcel(path: path, isColumnName: true);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                //sqlparams.Add("@projectId", projectId);
                DataTable typeTable = SqlManage.Query("SELECT id,brandName FROM fv_sysbrand ", sqlparams).Tables[0];//拉取

                //if (typeTable.Rows.Count != 0)
                //    order = (from p in typeTable.AsEnumerable().Select(p => p.Field<int>("brandTypeOrder")) select p).Max();
                foreach (DataRow dr in table.Rows)
                {
                    int order = int.Parse(dr[0].ToString());//品类排序
                    string typeName = dr[1].ToString();//品类名称
                    string tlogo = dr[2].ToString();//品类logo
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    var row = (from p in typeTable.AsEnumerable().Where(p => { return p.Field<string>("brandName") == typeName; })
                               select p).FirstOrDefault();

                    if (row == null)
                    {
                        //typeTable.Rows.Add(row);
                        //param.Add("@brandTypeId", row[0].ToString());
                        param.Add("@brandName", typeName);
                        param.Add("@brandLogo", @"/brandTypeTemplate/" + tlogo);
                        string sql = "insert into fv_sysbrand (brandName,brandDesc,brandLogo,createTime,lastChangeTime) values(@brandName,'',@brandLogo,now(),now())";
                        bool w = SqlManage.OpRecord(sql, param);
                        if (!w)
                        {
                            msg += "品类名称：" + typeName + "的行写入失败，发生数据库阻塞,";
                        }
                    }
                    else
                    {
                        msg += "品牌名称：" + typeName + "的行添加失败，已有对应品类,";
                    }

                    // }

                }

            }
            catch (SystemException e)
            {
                msg += e.Message + ",";

            }
            if (msg == "")
                msg = "品类导入成功,";
            return msg.TrimEnd(',');
        }

        /// <summary>
        /// 系统品牌导入
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string BullToDB(string path)
        {
            string msg = "";
            try
            {
                DataTable table = UploadExcel(path: path, isColumnName: true);
                Dictionary<string, object> sqlparams = new Dictionary<string, object>();
                //sqlparams.Add("@projectId", projectId);
                DataTable typeTable = SqlManage.Query("SELECT id,brandName FROM fv_sysbrand ", sqlparams).Tables[0];//拉取
                if (typeTable.Rows.Count == 0)
                {
                    msg = "该项目无品类，导入操作失败";
                }
                else
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        //int order = int.Parse(dr[0].ToString());//品类id
                        //string desc = dr[1].ToString();//品牌描述
                        string bName = dr[0].ToString();
                        string logo = dr[1].ToString();

                        string desc = dr[2].ToString();//品类名称
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        //param.Add("@brandName", bName);
                        string validateSql = "select count(*) from fv_sys_brand where sys_nane='" + bName + "'";
                        object o = SqlManage.Exists(validateSql, sqlparams);
                        if (int.Parse(o.ToString()) > 0)
                        //if(false)
                        {
                            msg += "品牌名称：" + bName + "的行写入失败，已有该品牌,";
                            continue;//已有该品牌则跳过
                        }
                        else //只加入存在于已导入的品类
                        {
                            //var row = (from p in typeTable.AsEnumerable().Where(p => { return p.Field<int>("id") == order; })
                            //       select p).FirstOrDefault();

                            // if (row != null)
                            //  {
                            param.Add("@sys_nane", bName);
                            param.Add("@sys_logo", @"/brandTemplate/" + logo);
                            //param.Add("@sys_type", order);
                            //param.Add("@sys_typeName", typeName);
                            param.Add("@sys_desc", desc);
                            string sql = "insert into fv_sys_brand (sys_nane,sys_logo,createTime,lastChangeTime,sys_desc) values(@sys_nane,@sys_logo,now(),now(),@sys_desc) ";
                            bool w = SqlManage.OpRecord(sql, param);
                            if (!w)
                            {
                                msg += "品牌名称：" + bName + "的行写入失败，发生数据库阻塞,";
                            }
                            // }
                            // else
                            // {
                            //      msg += "品牌名称：" + bName + "的行写入失败，无对应品类,";
                            //  }

                        }

                    }
                }
            }
            catch (SystemException e)
            {
                msg += e.Message + ",";

            }
            if (msg == "")
                msg = "品牌导入成功,";
            return msg.TrimEnd(',');
        }
    }
}

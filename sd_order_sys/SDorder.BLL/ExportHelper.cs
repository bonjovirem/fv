using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
    }
}

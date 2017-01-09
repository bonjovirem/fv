using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace SDorder.BLL
{
    public static class XmlHelper
    {
        public static string GetText(string path, string key)
        {
            string msg = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            if (doc.HasChildNodes)
            {
                XmlNodeList nodes = doc.GetElementsByTagName("global");
                XmlNode node = null;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].Attributes["value"].Value == key)
                    {
                        node = nodes[i];
                        break;
                    }
                }
                if (node.HasChildNodes && node != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("[{");
                    builder.Append("id: 'p1', homePage: 'welcome', menu: [");
                    foreach (XmlNode childnode in node)
                    {
                        builder.Append("{ text: '" + childnode.Attributes["value"].Value + "', items: [");
                        string temp = "";
                        foreach (XmlNode cnode in childnode.ChildNodes)
                        {
                           temp+="{ id: '" +cnode.Attributes["info"].Value + "', text: '" +cnode.Attributes["text"].Value 
                                + "', href: '" + cnode.Attributes["path"].Value + "', closeable: true },";

                        }
                        builder.Append(temp.TrimEnd(','));
                        builder.Append("] },");
                    }
                    builder.Append("]");
                    builder.Append("}];");
                    msg = builder.ToString();
                }

            }
            return msg;
        }
    }
}

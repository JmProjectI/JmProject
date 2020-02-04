using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Web;

namespace JMProject.Common
{
    public class NumHelper
    {
        public static string GetDH(string flag)
        {
            string result = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Temp.xml"));
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("DHS").ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("flag") == flag)
                {
                    result = xe.GetAttribute("num");
                }
            }
            return result;
        }

        public static bool UpdateDH(string countKey, string flag)
        {
            try
            {
                //获取模版数量
                string tempCount = ConfigurationManager.AppSettings[countKey].ToString();

                string result = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Temp.xml"));
                XmlNodeList nodeList = xmlDoc.SelectSingleNode("DHS").ChildNodes;
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.GetAttribute("flag") == flag)
                    {
                        result = xe.GetAttribute("num");

                        if (result == tempCount)
                        {
                            xe.SetAttribute("num", "1");
                        }
                        else
                        {
                            xe.SetAttribute("num", (int.Parse(xe.GetAttribute("num")) + 1).ToString());
                        }
                    }
                }
                xmlDoc.Save(HttpContext.Current.Server.MapPath("~/Temp.xml"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

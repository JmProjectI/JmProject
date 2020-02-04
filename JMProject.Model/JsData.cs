using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class JsData
    {
        public JsData()
        { }

        public String ID { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String Lxr { get; set; }
        public String JsYear { get; set; }
        public String JsKey { get; set; }
        public String Vlevel { get; set; }
        public String RegKey { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [JsData](");
            sb.Append("[ID]");
            sb.Append(",[Name]");
            sb.Append(",[Code]");
            sb.Append(",[Lxr]");
            sb.Append(",[JsYear]");
            sb.Append(",[JsKey]");
            sb.Append(",[Vlevel]");
            sb.Append(",[RegKey]");
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + Name + "'");
            sb.Append(",'" + Code + "'");
            sb.Append(",'" + Lxr + "'");
            sb.Append(",'" + JsYear + "'");
            sb.Append(",'" + JsKey + "'");
            sb.Append(",'" + Vlevel + "'");
            sb.Append(",'" + RegKey + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
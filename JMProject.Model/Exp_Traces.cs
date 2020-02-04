using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Exp_Traces
    {
        public Exp_Traces()
        { }

        public String LogisticCode { get; set; }
        public String AcceptTime { get; set; }
        public String AcceptStation { get; set; }
        public String Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Exp_Traces](");
            sb.Append("[LogisticCode]");
            sb.Append(",[AcceptTime]");
            sb.Append(",[AcceptStation]");
            sb.Append(",[Remark]");
            sb.Append(") VALUES (");
            sb.Append("'" + LogisticCode + "'");
            sb.Append(",'" + AcceptTime + "'");
            sb.Append(",'" + AcceptStation + "'");
            sb.Append(",'" + Remark + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SysNkReport
    {
        public SysNkReport()
        { }

        [PrimaryKey]
        public string Id { get; set; }
        public Guid Zid { get; set; }
        public string date { get; set; }
        public string flag { get; set; }
        public string czr { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO NkReport(");
            sb.Append("Id");
            sb.Append(",Zid");
            sb.Append(",date");
            sb.Append(",flag");
            sb.Append(",czr");
            sb.Append(") values(");
            sb.Append("'" + Id + "'");
            sb.Append(",'" + Zid + "'");
            sb.Append(",'" + date + "'");
            sb.Append(",'" + flag + "'");
            sb.Append(",'" + czr + "'");
            sb.Append(")");

            return sb.ToString();
        }
    }
}

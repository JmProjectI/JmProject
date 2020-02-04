using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class NkReport
    {
        public NkReport()
        {
            Id = Guid.NewGuid();
            Years = DateTime.Now.AddYears(-1).Year.ToString();
            Flag = "1";
        }

        public string OrderId { get; set; }
        public string CustomId { get; set; }
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Years { get; set; }
        public string Flag { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO NkReport(");
            sb.Append("OrderId");
            sb.Append(",CustomId");
            sb.Append(",Id");
            sb.Append(",Years");
            sb.Append(",Flag");
            sb.Append(") values(");
            sb.Append("'" + OrderId + "'");
            sb.Append(",'" + CustomId + "'");
            sb.Append(",'" + Id + "'");
            sb.Append(",'" + Years + "'");
            sb.Append(",'" + Flag + "'");
            sb.Append(")");

            return sb.ToString();
        }
    }
}

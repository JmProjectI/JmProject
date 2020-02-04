using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_inspect
    {
        public Nksc_inspect()
        {
        }

        [PrimaryKey]
        public string Id { get; set; }
        public string CustomerID { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Nksc_inspect](");
            sb.Append("[Id]");
            sb.Append(",[CustomerID]");
            sb.Append(",[Content]");
            sb.Append(") VALUES (");
            sb.Append("'" + Id + "'");
            sb.Append(",'" + CustomerID + "'");
            sb.Append(",'" + Content + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}

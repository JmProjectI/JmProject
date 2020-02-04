using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_Update
    {
        public Nksc_Update()
        {
            id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public String CustomerID { get; set; }
        public String NkscDate { get; set; }
        public String versionS { get; set; }
        public String versionE { get; set; }
        public String UpdateFlag { get; set; }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Nksc_Update](");
            sb.Append("[id]");
            sb.Append(",[CustomerID]");
            sb.Append(",[NkscDate]");
            sb.Append(",[versionS]");
            sb.Append(",[versionE]");
            sb.Append(",[UpdateFlag]");
            sb.Append(") VALUES (");
            sb.Append("'" + id + "'");
            sb.Append(",'" + CustomerID + "'");
            sb.Append(",'" + NkscDate + "'");
            sb.Append(",'" + versionS + "'");
            sb.Append(",'" + versionE + "'");
            sb.Append(",'" + UpdateFlag + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}

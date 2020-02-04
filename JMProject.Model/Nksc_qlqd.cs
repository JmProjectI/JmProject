using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_qlqd
    {
        public Nksc_qlqd()
        { }

        public Guid id { get; set; }
        public String qlsx { get; set; }
        public String qlsxname { get; set; }
        public String leder { get; set; }
        public String qltext { get; set; }
        public int qlsort { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Nksc_qlqd](");
            sb.Append("[id]");
            sb.Append(",[qlsx]");
            sb.Append(",[qlsxname]");
            sb.Append(",[leder]");
            sb.Append(",[qltext]");
            sb.Append(",[qlsort]");
            sb.Append(") VALUES (");
            sb.Append("'" + id + "'");
            sb.Append(",'" + qlsx + "'");
            sb.Append(",'" + qlsxname + "'");
            sb.Append(",'" + leder + "'");
            sb.Append(",'" + qltext + "'");
            sb.Append("," + qlsort + "");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
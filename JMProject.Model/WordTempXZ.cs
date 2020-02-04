using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WordTempXZ
    {
        public WordTempXZ()
        { }

        [PrimaryKey]
        public String ID { get; set; }
        public String dkey { get; set; }
        public String zz { get; set; }
        public String fz { get; set; }
        public String qtks { get; set; }
        public String cy { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [WordTempXZ](");
            sb.Append("[ID]");
            sb.Append(",[dkey]");
            sb.Append(",[zz]");
            sb.Append(",[fz]");
            sb.Append(",[qtks]");
            sb.Append(",[cy]");
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + dkey + "'");
            sb.Append(",'" + zz + "'");
            sb.Append(",'" + fz + "'");
            sb.Append(",'" + qtks + "'");
            sb.Append(",'" + cy + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }

}
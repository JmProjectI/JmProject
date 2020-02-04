using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class BasicSet
    {
        public BasicSet()
        { }

        [PrimaryKey]
        public String Userid { get; set; }
        public Decimal PercentZ { get; set; }
        public Decimal PercentY { get; set; }
        public Decimal PercentC { get; set; }
        public Decimal PercentN { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [BasicSet](");
            sb.Append("[Userid]");
            sb.Append(",[PercentZ]");
            sb.Append(",[PercentY]");
            sb.Append(",[PercentC]");
            sb.Append(",[PercentN]");
            sb.Append(") VALUES (");
            sb.Append("'" + Userid + "'");
            sb.Append(",'" + PercentZ + "'");
            sb.Append(",'" + PercentY + "'");
            sb.Append(",'" + PercentC + "'");
            sb.Append(",'" + PercentN + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
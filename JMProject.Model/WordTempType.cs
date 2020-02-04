using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WordTempType
    {
        public WordTempType()
        { }

        [PrimaryKey]
        public String ID { get; set; }
        public String Name { get; set; }
        public String Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [WordTempType](");
            sb.Append("[ID]");
            sb.Append(",[Name]");
            sb.Append(",[Remark]");
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + Name + "'");
            sb.Append(",'" + Remark + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
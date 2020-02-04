using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WordTempKey
    {
        public WordTempKey()
        {
            id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public String Zid { get; set; }
        public String WordKey { get; set; }
        public String DBKey { get; set; }
        public String KeyType { get; set; }
        public String Desc { get; set; }
        public String ywType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [WordTempKey](");
            sb.Append("[id]");
            sb.Append(",[Zid]");
            sb.Append(",[WordKey]");
            sb.Append(",[DBKey]");
            sb.Append(",[KeyType]");
            sb.Append(",[Desc]");
            sb.Append(",[ywType]");
            sb.Append(") VALUES (");
            sb.Append("'" + id + "'");
            sb.Append(",'" + Zid + "'");
            sb.Append(",'" + WordKey + "'");
            sb.Append(",'" + DBKey + "'");
            sb.Append(",'" + KeyType + "'");
            sb.Append(",'" + Desc + "'");
            sb.Append(",'" + ywType + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WordTempLCT
    {
        public WordTempLCT()
        {
            ID = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid ID { get; set; }
        public String wkey { get; set; }
        public String dkey { get; set; }
        public String formate { get; set; }
        public String fontName { get; set; }
        public Int32 fontSize { get; set; }
        public Int32 x { get; set; }
        public Int32 y { get; set; }
        public Int32 w { get; set; }
        public Int32 h { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [WordTempLCT](");
            sb.Append("[ID]");
            sb.Append(",[wkey]");
            sb.Append(",[dkey]");
            sb.Append(",[formate]");
            sb.Append(",[fontName]");
            sb.Append(",[fontSize]");
            sb.Append(",[x]");
            sb.Append(",[y]");
            sb.Append(",[w]");
            sb.Append(",[h]");
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + wkey + "'");
            sb.Append(",'" + dkey + "'");
            sb.Append(",'" + formate + "'");
            sb.Append(",'" + fontName + "'");
            sb.Append(",'" + fontSize + "'");
            sb.Append(",'" + x + "'");
            sb.Append(",'" + y + "'");
            sb.Append(",'" + w + "'");
            sb.Append(",'" + h + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WordTempFile
    {
        public WordTempFile()
        { }

        [PrimaryKey]
        public String ID { get; set; }
        public String Name { get; set; }
        public String WordFile { get; set; }
        public String NewPage { get; set; }
        public String ywKey { get; set; }
        public Int32 Sort { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [WordTempFile](");
            sb.Append("[ID]");
            sb.Append(",[Name]");
            sb.Append(",[WordFile]");
            sb.Append(",[NewPage]");
            sb.Append(",[ywKey]");
            sb.Append(",[Sort]");
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + Name + "'");
            sb.Append(",'" + WordFile + "'");
            sb.Append(",'" + NewPage + "'");
            sb.Append(",'" + ywKey + "'");
            sb.Append(",'" + Sort + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
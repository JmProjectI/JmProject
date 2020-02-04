using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class View_WordTempKey
    {
        public View_WordTempKey()
        { }

        public Guid id { get; set; }
        public String Zid { get; set; }
        public String WordKey { get; set; }
        public String DBKey { get; set; }
        public String KeyType { get; set; }
        public String Desc { get; set; }
        public String ywType { get; set; }
        public String KeyTypeName { get; set; }
        public String KeyTypeDesc { get; set; }
    }
}
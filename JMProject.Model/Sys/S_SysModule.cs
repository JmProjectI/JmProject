using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model.Sys
{
    public class S_SysModule
    {
        public S_SysModule()
        { }

        [PrimaryKey]
        public String id { get; set; }
        public String Name { get; set; }
        public String Icon { get; set; }
        public String Url { get; set; }
        public Int32 Sort { get; set; }
        public String _parentId { get; set; }
    }
}

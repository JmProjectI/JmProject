using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class SysModuleRole
    {
        public SysModuleRole()
        { }

        public String Id { get; set; }
        public String Name { get; set; }
        public String Icon { get; set; }
        public String Url { get; set; }
        public Int32 Sort { get; set; }
        public String _parentId { get; set; }
        public int HavRole { get; set; }
    }
}

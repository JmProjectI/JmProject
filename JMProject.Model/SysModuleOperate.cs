using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SysModuleOperate
    {
        public SysModuleOperate()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String ModuleId { get; set; }
        public String Name { get; set; }
        public String KeyCode { get; set; }
        public Boolean IsValid { get; set; }
        public Int32 Sort { get; set; }
    }
}


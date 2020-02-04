using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SysRole
    {
        public SysRole()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Remake { get; set; }
    }
}


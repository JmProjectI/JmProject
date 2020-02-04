using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class BasicProvince
    {
        public BasicProvince()
        {
        }

        [PrimaryKey]
        public string Pid { get; set; }
	    public string Name { get; set; }
        public string beizhu { get; set; }
    }
}

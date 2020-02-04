using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class BasicCity
    {
        public BasicCity()
        {
        }

        public string Pid { get; set; }
        [PrimaryKey]
	    public string ID { get; set; }
	    public string Name { get; set; }
	    public string Code { get; set; }
	    public string PostCode { get; set; }
        public string beizhu { get; set; }
    }
}

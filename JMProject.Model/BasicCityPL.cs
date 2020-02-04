using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class BasicCityPL
    {
        public BasicCityPL()
        {
        }

        public string Pid { get; set; }
        [PrimaryKey]
	    public string ID { get; set; }
	    public string Name { get; set; }
    }
}

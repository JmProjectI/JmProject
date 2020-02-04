using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOutStock
    {
        public FinOutStock()
        { }

        public String OrderId { get; set; }
        public String OSId { get; set; }
        public String OSdate { get; set; }
        public String Uid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class View_NkReport
    {
        public View_NkReport()
        {
        }

        public string OrderId { get; set; }
        public string CustomId { get; set; }
        public Guid Id { get; set; }
        public string Years { get; set; }
        public string Flag { get; set; }
        public string Name { get; set; }
        public string Invoice { get; set; }
    }
}

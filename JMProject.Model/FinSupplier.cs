using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinSupplier
    {
        public FinSupplier()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Linkman { get; set; }
        public String Phone { get; set; }
        public String Tel { get; set; }
        public String Address { get; set; }
        public String Remake { get; set; }
    }
}

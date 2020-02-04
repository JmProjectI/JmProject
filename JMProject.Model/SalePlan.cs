using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SalePlan
    {
        public SalePlan()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String Year { get; set; }
        public String Saler { get; set; }
        public Decimal YearTarget { get; set; }
        public Decimal MonthTarget { get; set; }
        public Int32 AddedTarget { get; set; }
        public Int32 AddedTarget1 { get; set; }
    }
}

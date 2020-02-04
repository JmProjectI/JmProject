using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_SalePlan
    {
        public String Id { get; set; }
        public String Year { get; set; }
        public String Saler { get; set; }
        public Decimal YearTarget { get; set; }
        public Decimal MonthTarget { get; set; }
        public Int32 AddedTarget { get; set; }
        public Int32 AddedTarget1 { get; set; }
        public String ZsName { get; set; }
    }
}

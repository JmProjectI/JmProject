using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinOutStock
    {
        public View_FinOutStock()
        { }

        public String OrderId { get; set; }
        public String OSId { get; set; }
        public String OSdate { get; set; }
        public Int32 OutStockCount { get; set; }
        public String Uid { get; set; }
        public String ZsName { get; set; }        
    }
}

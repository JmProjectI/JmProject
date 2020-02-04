using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinSalesCommission
    {
        public View_FinSalesCommission()
        { }

        public String _parentId { get; set; }
        public String TypeId { get; set; }
        public String Name { get; set; }
        public String Id { get; set; }
        public Decimal Unfinished { get; set; }
        public Decimal Finished { get; set; }
        public Decimal Nonsalesman { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinOrderInvoice
    {
        public View_FinOrderInvoice()
        { }

        public String OrderId { get; set; }
        public String Id { get; set; }
        public String AccountId { get; set; }
        public String Invoicedate { get; set; }
        public Decimal Invoicemoney { get; set; }
        public Decimal Receivablemoney { get; set; }
        public String Remark { get; set; }
        public String Name { get; set; }
        public String Key { get; set; }
    }
}

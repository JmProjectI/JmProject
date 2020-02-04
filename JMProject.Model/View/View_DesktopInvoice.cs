using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_DesktopInvoice
    {
        public View_DesktopInvoice()
        { }

        public string Id { get; set; }
        public string Paymentaccount { get; set; }
        public string Paymentdate { get; set; }
        public decimal Paymentmoney { get; set; }
        public string Remark { get; set; }
        public string Saler { get; set; }
        public string PaymentFlag { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string ZsName { get; set; }
        public string HkFlag { get; set; }
        public string CustomName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class S_SCustom
    {
        public S_SCustom() { }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Lxr { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal ItemMoney { get; set; }
        public decimal Invoicemoney { get; set; }
        public decimal Paymentmoney { get; set; }
    }
}

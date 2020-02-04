using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class S_CustomReprot
    {
        public Int64 Row { get; set; }
        public string SaleCustomId { get; set; }
        public string Name { get; set; }
        public decimal ItemMoney { get; set; }
        public decimal Invoicemoney { get; set; }
        public decimal Paymentmoney { get; set; }
        public string ItemNames { get; set; }
    }
}

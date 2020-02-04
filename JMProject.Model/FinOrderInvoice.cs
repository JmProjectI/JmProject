using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOrderInvoice
    {
        public FinOrderInvoice()
        { }

        public String OrderId { get; set; }
        [PrimaryKey]
        public String Id { get; set; }
        public String AccountId { get; set; }
        public String Invoicedate { get; set; }
        public Decimal Invoicemoney { get; set; }
        public Decimal Receivablemoney { get; set; }
        public String Remark { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOrderPayment
    {
        public FinOrderPayment()
        { }

        public String OrderId { get; set; }
        public String InvoiceId { get; set; }
        [PrimaryKey]
        public String Id { get; set; }
        public String Paymentaccount { get; set; }
        public String Paymentdate { get; set; }
        public Decimal Paymentmoney { get; set; }
        public String Remark { get; set; }
    }
}
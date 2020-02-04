using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinOrderPayment
    {
        public View_FinOrderPayment()
        { }

        public String OrderId { get; set; }
        public String Id { get; set; }
        public String Paymentaccount { get; set; }
        public String Paymentdate { get; set; }
        public Decimal Paymentmoney { get; set; }
        public String Remark { get; set; }
        public String Name { get; set; }
        public String Key { get; set; }

    }
}

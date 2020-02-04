using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Esayui
{
    public class EasyUIModel
    {
        public EasyUIModel() { }

        public string Id { get; set; }
        public string Key { get; set; }
        public string AccountId { get; set; }
        public decimal Invoicemoney { get; set; }
        public decimal Receivablemoney { get; set; }        
    }
}

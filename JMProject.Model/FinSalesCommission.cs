using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinSalesCommission
    {
        public FinSalesCommission()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String TypeId { get; set; }
        public Decimal Unfinished { get; set; }
        public Decimal Finished { get; set; }
        public Decimal Nonsalesman { get; set; }
    }
}
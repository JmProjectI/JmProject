using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOrderItem
    {
        public FinOrderItem()
        { }

        public String OrderId { get; set; }
        [PrimaryKey]
        public String ItemId { get; set; }
        public String ModularID { get; set; }
        public String TypeName { get; set; }
        public Decimal CbMoney { get; set; }
        public Decimal JcMoney { get; set; }
        public Int32 CsCount { get; set; }
        public Int32 AddCount { get; set; }
        public Decimal AddMoney { get; set; }
        public Int32 SumCount { get; set; }
        public Decimal sumMoney { get; set; }
    }
}


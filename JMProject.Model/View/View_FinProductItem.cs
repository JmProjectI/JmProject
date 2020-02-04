using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinProductItem
    {
        public View_FinProductItem()
        {}

        public String ItemID { get; set; }
        public String ProId { get; set; }
        public String ModularID { get; set; }
        public Decimal CbMoney { get; set; }
        public Decimal JcMoney { get; set; }
        public Int32 CsCount { get; set; }
        public Int32 AddCount { get; set; }
        public Decimal AddMoney { get; set; }
        public Int32 SumCount { get; set; }
        public Decimal sumMoney { get; set; }

        public String TypeName { get; set; }
    }
}

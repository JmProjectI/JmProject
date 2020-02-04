using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinOutStockItem
    {
        public View_FinOutStockItem()
        { }

        public String OutStockId { get; set; }
        public String SaleOrderItemId { get; set; }
        public String ProductId { get; set; }
        public Decimal Marketprice { get; set; }
        public Decimal Costprice { get; set; }
        public Int32 OutStockCount { get; set; }
        public String ProName { get; set; }
        public String ProSpec { get; set; }
        public Int32 ProUcount { get; set; }
        public String ProPkey { get; set; }
        public String ProductType { get; set; }
        public String TypeName { get; set; }

    }
}

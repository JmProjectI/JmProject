using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOutStockItem
    {
        public FinOutStockItem()
        { }

        public String OutStockId { get; set; }
        public String SaleOrderItemId { get; set; }
        public String ProductId { get; set; }
        public decimal Marketprice { get; set; }
        public decimal Costprice { get; set; }
        public Int32 OutStockCount { get; set; }
    }
}

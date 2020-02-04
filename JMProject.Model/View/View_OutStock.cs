using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_OutStock
    {
        public View_OutStock()
        { }

        public string OrderId { set; get; }
        public string OSId { set; get; }
        public string OSdate { set; get; }
        public int OutStockCount { set; get; }
        public string Uid { set; get; }
        public string ZsName { set; get; }
        public string SaleOrderItemId { set; get; }
        public string ProductId { set; get; }
        public Decimal Marketprice { set; get; }
        public Decimal Costprice { set; get; }
        public string ProName { set; get; }
        public string ProSpec { set; get; }
        public int ProUcount { set; get; }
        public string ProPkey { set; get; }
        public string ProductType { set; get; }
        public string TypeName { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model.View
{
    public class View_SaleOrderItem
    {
        public View_SaleOrderItem()
        { }

        public String OrderId { get; set; }
        public String ItemId { get; set; }
        public String ProdectDesc { get; set; }
        public Int32 ItemCount { get; set; }
        public Decimal ItemPrice { get; set; }
        public Decimal ItemMoney { get; set; }
        public Decimal TaxMoney { get; set; }
        public Decimal PresentMoney { get; set; }
        public Decimal OtherMoney { get; set; }
        public Decimal ValidMoney { get; set; }
        public String Service { get; set; }
        public String SerDateS { get; set; }
        public String SerDateE { get; set; }
        public Int32 ServiceMonth { get; set; }
        public String ProdectType { get; set; }
        public String TypeName { get; set; }
        public String TypeRemake { get; set; }
        public String _parentId { get; set; }
        public Int32 OSCount { get; set; }
        public Decimal CostMoney { get; set; }
        public String TcFlag { get; set; }
        public String TcName { get; set; }
        public String TcDate { get; set; }
        
    }
}



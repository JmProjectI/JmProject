using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model.Mobile
{
    public class View_SaleOrder_Mobile
    {
        public View_SaleOrder_Mobile()
        {
        }
        
        [PrimaryKey]
        public string Id { get; set; }
        public string OrderDate { get; set; }
        public string SaleCustomId { get; set; }
        public string Name { get; set; }
        public string Saler { get; set; }
        public string SalerName { get; set; }
        public string OrderType { get; set; }
        public string OrderTypeName { get; set; }
        public string ItemNames { get; set; }
        public string ItemCount { get; set; }
        public string ItemPrice { get; set; }
        public string PresentMoney { get; set; }
        public string OtherMoney { get; set; }
        public string Remake { get; set; }
    }
}

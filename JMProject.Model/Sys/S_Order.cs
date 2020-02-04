using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class S_Order
    {
        public S_Order()
        {
            OrderMain = new SaleOrder();
            OrderMain.OrderType = "000073";//合同类别
            OrderMain.InvoiceFlag = "000063";//未开票
            OrderMain.PaymentFlag = "000066";//未回款
            OrderMain.OutStockFlag = "000069";//未出库
            OrderMain.CheckFlag = "000071";//未审核
            OrderMain.CheckDate = "";//审核日期
            OrderMain.Finshed = false;//是否完成
            OrderMain.Remake = "";//备注
            OrderMain.Enclosure = "";//附件

            OrderItems = new List<SaleOrderItem>();
        }

        public string KhName { get; set; }
        public SaleOrder OrderMain;
        public List<SaleOrderItem> OrderItems;
    }
}

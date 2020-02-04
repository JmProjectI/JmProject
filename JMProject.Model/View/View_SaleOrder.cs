using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_SaleOrder
    {
        public View_SaleOrder()
        { }

        public String Id { get; set; }
        public String OrderDate { get; set; }
        public String SaleCustomId { get; set; }
        public String Saler { get; set; }
        public Boolean Finshed { get; set; }
        public String UserId { get; set; }
        public String Remake { get; set; }
        public String Enclosure { get; set; }
        public String AccountId { get; set; }
        public String AccountName { get; set; }
        public String Key { get; set; }
        public String OrderType { get; set; }
        public String InvoiceFlag { get; set; }
        public String PaymentFlag { get; set; }
        public String OutStockFlag { get; set; }
        public String CheckFlag { get; set; }
        public String CheckDate { get; set; }
        public String SalerName { get; set; }
        public String InvoiceFlagName { get; set; }
        public String PaymentFlagName { get; set; }
        public String OutStockFlagName { get; set; }
        public String CheckFlagName { get; set; }
        public String OrderTypeName { get; set; }
        public String Name { get; set; }
        public String Region { get; set; }
        public String CityName { get; set; }
        public String Lxr { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String Invoice { get; set; }
        public String Code { get; set; }
        public String Zyx { get; set; }
        public String ZyxName { get; set; }
        public String CustomerType { get; set; }
        public String TypeName { get; set; }
        public String Industry { get; set; }
        public String HyName { get; set; }
        public String ZsName { get; set; }
        //public Decimal Marketprice { get; set; }
        //public Decimal Costprice { get; set; }
        public Int32 ItemCount { get; set; }
        public Decimal ItemMoney { get; set; }
        public Decimal TaxMoney { get; set; }
        public Decimal PresentMoney { get; set; }
        public Decimal OtherMoney { get; set; }
        public Decimal ValidMoney { get; set; }

        public Decimal Invoicemoney { get; set; }
        public Decimal Receivablemoney { get; set; }
        public Decimal Paymentmoney { get; set; }
        public Int32 OSCount { get; set; }
        public String flagDown { get; set; }
        public String nkscflag { get; set; }
        public Guid nid { get; set; }
        public String ItemNames { get; set; }

        public Int32 unTc { get; set; }
        public Int32 Tc { get; set; }
        public String Flag { get; set; }
        public String Fp { get; set; }

        public String SuiH { get; set; }
        public String BankCard { get; set; }

        public String QtLxr { get; set; }
        public String QtTel { get; set; }
        public String CustomerGrade{ get; set; }
        public String DjName { get; set; }

        public String Paymentdate { get; set; }
    }
}
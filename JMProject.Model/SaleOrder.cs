using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SaleOrder
    {
        public SaleOrder()
        {
            Flag = "0";
        }

        [PrimaryKey]
        public String Id { get; set; }
        public String OrderDate { get; set; }
        public String SaleCustomId { get; set; }
        public String Saler { get; set; }
        public String AccountId { get; set; }
        public String OrderType { get; set; }
        public String InvoiceFlag { get; set; }
        public String PaymentFlag { get; set; }
        public String OutStockFlag { get; set; }
        public String CheckFlag { get; set; }
        public String CheckDate { get; set; }
        public Boolean Finshed { get; set; }
        public String Remake { get; set; }
        public String Enclosure { get; set; }
        public String UserId { get; set; }
        public String Flag { get; set; }
        public String Fp { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO SaleOrder(");
            sb.Append("id");
            sb.Append(",OrderDate");
            sb.Append(",SaleCustomId");
            sb.Append(",Saler");
            sb.Append(",InvoiceFlag");
            sb.Append(",PaymentFlag");
            sb.Append(",OutStockFlag");
            sb.Append(",CheckFlag");
            sb.Append(",CheckDate");
            sb.Append(",Finshed");
            sb.Append(",Remake");
            sb.Append(",UserId");
            if (!string.IsNullOrEmpty(AccountId))
            {
                sb.Append(",AccountId");
            }
            sb.Append(",OrderType");
            sb.Append(",Enclosure");
            sb.Append(",Flag");
            sb.Append(",Fp");
            sb.Append(") values(");
            sb.Append("'" + Id + "'");
            sb.Append(",'" + OrderDate + "'");
            sb.Append(",'" + SaleCustomId + "'");
            sb.Append(",'" + Saler + "'");
            sb.Append(",'" + InvoiceFlag + "'");
            sb.Append(",'" + PaymentFlag + "'");
            sb.Append(",'" + OutStockFlag + "'");
            sb.Append(",'" + CheckFlag + "'");
            sb.Append(",'" + CheckDate + "'");
            sb.Append(",'" + Finshed + "'");
            sb.Append(",'" + Remake + "'");
            sb.Append(",'" + UserId + "'");
            if (!string.IsNullOrEmpty(AccountId))
            {
                sb.Append(",'" + AccountId + "'");
            }
            sb.Append(",'" + OrderType + "'");
            sb.Append(",'" + Enclosure + "'");
            sb.Append(",'" + Flag + "'");
            sb.Append(",'" + Fp + "'");
            sb.Append(")");

            return sb.ToString();
        }

    }
}

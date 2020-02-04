using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SaleOrderItem
    {
        public SaleOrderItem()
        {
            //是否包含服务、服务开始日期、服务结束日期
            Service = "否";
            SerDateS = "";
            SerDateE = "";

            //有效回款
            ValidMoney = 0.00M;
            //未提成
            TcFlag = "000106";
            //提成日期
            TcDate = "";
        }

        public String OrderId { get; set; }
        [PrimaryKey]
        public String ItemId { get; set; }
        public String ProdectType { get; set; }
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
        //180510
        public String TcFlag { get; set; }
        public String TcDate { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [SaleOrderItem](");
            sb.Append("[OrderId]");
            sb.Append(",[ItemId]");
            sb.Append(",[ProdectType]");
            sb.Append(",[ProdectDesc]");
            sb.Append(",[ItemCount]");
            sb.Append(",[ItemPrice]");
            sb.Append(",[ItemMoney]");
            sb.Append(",[TaxMoney]");
            sb.Append(",[PresentMoney]");
            sb.Append(",[OtherMoney]");
            sb.Append(",[ValidMoney]");
            sb.Append(",[Service]");
            sb.Append(",[SerDateS]");
            sb.Append(",[SerDateE]");
            sb.Append(",[TcFlag]");
            sb.Append(",[TcDate]");
            sb.Append(") VALUES (");
            sb.Append("'" + OrderId + "'");
            sb.Append(",'" + ItemId + "'");
            sb.Append(",'" + ProdectType + "'");
            sb.Append(",'" + ProdectDesc + "'");
            sb.Append(",'" + ItemCount + "'");
            sb.Append(",'" + ItemPrice + "'");
            sb.Append(",'" + ItemMoney + "'");
            sb.Append(",'" + TaxMoney + "'");
            sb.Append(",'" + PresentMoney + "'");
            sb.Append(",'" + OtherMoney + "'");
            sb.Append(",'" + ValidMoney + "'");
            sb.Append(",'" + Service + "'");
            sb.Append(",'" + SerDateS + "'");
            sb.Append(",'" + SerDateE + "'");
            sb.Append(",'" + TcFlag + "'");
            sb.Append(",'" + TcDate + "'");
            sb.Append(")");
            return sb.ToString();
        }

    }
}


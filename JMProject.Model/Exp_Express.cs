using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Exp_Express
    {
        public Exp_Express()
        { }

        [PrimaryKey]
        public String LogisticCode { get; set; }
        public String ShipperCode { get; set; }
        public String OrderId { get; set; }
        public String ReceiverName { get; set; }
        public String Tel { get; set; }
        public String Mobile { get; set; }
        public String ProvinceName { get; set; }
        public String CityName { get; set; }
        public String ExpAreaName { get; set; }
        public String Address { get; set; }
        public String GoodsName { get; set; }
        public String State { get; set; }
        public String Reason { get; set; }
        public String ExpressTime { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Exp_Express](");
            sb.Append("[LogisticCode]");
            sb.Append(",[ShipperCode]");
            sb.Append(",[OrderId]");
            sb.Append(",[ReceiverName]");
            sb.Append(",[Tel]");
            sb.Append(",[Mobile]");
            sb.Append(",[ProvinceName]");
            sb.Append(",[CityName]");
            sb.Append(",[ExpAreaName]");
            sb.Append(",[Address]");
            sb.Append(",[GoodsName]");
            sb.Append(",[State]");
            sb.Append(",[Reason]");
            sb.Append(",[ExpressTime]");
            sb.Append(") VALUES (");
            sb.Append("'" + LogisticCode + "'");
            sb.Append(",'" + ShipperCode + "'");
            sb.Append(",'" + OrderId + "'");
            sb.Append(",'" + ReceiverName + "'");
            sb.Append(",'" + Tel + "'");
            sb.Append(",'" + Mobile + "'");
            sb.Append(",'" + ProvinceName + "'");
            sb.Append(",'" + CityName + "'");
            sb.Append(",'" + ExpAreaName + "'");
            sb.Append(",'" + Address + "'");
            sb.Append(",'" + GoodsName + "'");
            sb.Append(",'" + State + "'");
            sb.Append(",'" + Reason + "'");
            sb.Append(",'" + ExpressTime + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
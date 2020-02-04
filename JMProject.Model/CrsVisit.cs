using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class CrsVisit
    {
        public CrsVisit()
        { }

        [PrimaryKey]
        public Guid Id { get; set; }
        public Int32 Vyear { get; set; }
        public String Saler { get; set; }
        public String CustomID { get; set; }
        public String Falg { get; set; }
        public String VisitType { get; set; }
        public Decimal ByearPay { get; set; }
        public Decimal UpyearPay { get; set; }
        public Decimal SumPay { get; set; }
        public String VisitDate { get; set; }
        public String VisitGood { get; set; }
        public String Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [CrsVisit](");
            sb.Append("[Id]");
            sb.Append(",[Vyear]");
            sb.Append(",[Saler]");
            sb.Append(",[CustomID]");
            sb.Append(",[Falg]");
            sb.Append(",[VisitType]");
            sb.Append(",[ByearPay]");
            sb.Append(",[UpyearPay]");
            sb.Append(",[SumPay]");
            sb.Append(",[VisitDate]");
            sb.Append(",[VisitGood]");
            sb.Append(",[Remark]");
            sb.Append(") VALUES (");
            sb.Append("'" + Id + "'");
            sb.Append(",'" + Vyear + "'");
            sb.Append(",'" + Saler + "'");
            sb.Append(",'" + CustomID + "'");
            sb.Append(",'" + Falg + "'");
            sb.Append(",'" + VisitType + "'");
            sb.Append(",'" + ByearPay + "'");
            sb.Append(",'" + UpyearPay + "'");
            sb.Append(",'" + SumPay + "'");
            sb.Append(",'" + VisitDate + "'");
            sb.Append(",'" + VisitGood + "'");
            sb.Append(",'" + Remark + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
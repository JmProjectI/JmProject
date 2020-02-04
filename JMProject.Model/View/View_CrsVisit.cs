using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class View_CrsVisit
    {
        public View_CrsVisit()
        { }

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
        public String Name { get; set; }
        public String CityName { get; set; }
        public String HyName { get; set; }
        public String Lxr { get; set; }
        public String Tel { get; set; }
        public String Zw { get; set; }
        public String CustomerGrade { get; set; }
        
    }
}
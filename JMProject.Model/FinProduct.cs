using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinProduct
    {
        public FinProduct()
        { }

        public String TypeId { get; set; }
        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Spec { get; set; }
        public Int32 Ucount { get; set; }
        public String Pkey { get; set; }
        public Decimal Marketprice { get; set; }
        public Decimal Costprice { get; set; }
        public Int32 InitialCount { get; set; }
        public Int32 InCount { get; set; }
        public Int32 OutCount { get; set; }
        public Int32 stock { get; set; }
        public String Remake { get; set; }
    }
}

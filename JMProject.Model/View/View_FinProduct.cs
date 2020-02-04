using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinProduct
    {
        public String TypeId { get; set; }
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

        public String TypeName { get; set; }
        public String TypeRemake { get; set; }
        public String _parentId { get; set; }

        public String CusName { get; set; }
        public String SalerName { get; set; }
    }
}

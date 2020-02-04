using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model.Sys
{
    public class S_FinOutStockItem
    {
        public S_FinOutStockItem()
        { }

        public String OutStockId { get; set; }
        public String ProductId { get; set; }
        public Int32 ItemCount { get; set; }
        public Int32 OSCount { get; set; }
        public Int32 OutStockCount { get; set; }
    }
}

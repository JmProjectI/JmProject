using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_SaleVisit_LlztTx
    {
        public View_SaleVisit_LlztTx()
        { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ContactDate { get; set; }
        public string LlztName { get; set; }
        public string DemandType { get; set; }
        public string DemandTypeName { get; set; }
        public string Ywy { get; set; }
        public string ContactFlag { get; set; }
        public DateTime daoqidate { get; set; }
        public DateTime tixingdate { get; set; }
        public string ZsName { get; set; }
    }
}
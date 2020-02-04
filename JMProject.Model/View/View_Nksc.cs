using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_Nksc
    {
        public View_Nksc()
        { }

        public Guid id { get; set; }
        public string SaleOrderID { get; set; }
        public string CustomerID { get; set; }
        public string fileName { get; set; }        
        public string NkscDate { get; set; }
        public string NkscSBDate { get; set; }
        public string dwqc { get; set; }
        public string Lxr { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Region { get; set; }
        public string pfr { get; set; }
        public string flag { get; set; }
        public string wtfkFlag { get; set; }
        public string zddate { get; set; }
        public int xyzdsum { get; set; }
        public int bczdsum { get; set; }
        public int sysum { get; set; }
        public string tsyqtext { get; set; }
        public string NkscDateSC { get; set; }
        public string peoSC { get; set; }
        public string swfName { get; set; }
        public string NkscDateSCPDF { get; set; }
        public string NkscDatePDF { get; set; }
        public string peoPDF { get; set; }
        public string bz { get; set; }
        public string yhscfj { get; set; }
        public int AddBook { get; set; }
        public int IsUpdate { get; set; }
        public int IsUpdateE { get; set; }
        public string version { get; set; }

        public string FPImage { get; set; }
    }
}

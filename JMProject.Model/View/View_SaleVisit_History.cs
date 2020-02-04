using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_SaleVisit_History
    {
        public View_SaleVisit_History()
        { }

        public string Id { get; set; }//编号
        public string SaleCustomID { get; set; }//客户
        public string ContactDate { get; set; }//联络日期
        public string Offer { get; set; }//报价
        public string ContactDetails { get; set; }//本次联络详情
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SaleWeiXin
    {
        public SaleWeiXin()
        { }

        /// <summary>
        /// 微信绑定编号
        /// </summary>
        public String ID { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public String CusId { get; set; }

        /// <summary>
        /// 微信编号
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 是否新老客户 0--老客户、1--新客户
        /// </summary>
        public String IsCus { get; set; }
    }
}
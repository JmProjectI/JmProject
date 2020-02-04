using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinRemittance
    {
        public WeiXinRemittance()
        { }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CusId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 汇款单或入账通知书
        /// </summary>
        public string ImageName { get; set; }
    }
}

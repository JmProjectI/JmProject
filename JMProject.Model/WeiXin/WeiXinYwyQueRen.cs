using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinYwyQueRen
    {
        public WeiXinYwyQueRen()
        {
            OrderMx = new List<WeiXinYwyQueRenMx>();
        }
        
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        
        /// <summary>
        /// 是否老客户
        /// </summary>
        public string IsCus { get; set; }
        
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 微信编号
        /// </summary>
        public string CusOpenId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Years { get; set; }

        public List<WeiXinYwyQueRenMx> OrderMx { get; set; }
    }

    public class WeiXinYwyQueRenMx
    {
        public WeiXinYwyQueRenMx()
        { }

        /// <summary>
        /// 订单明细编号
        /// </summary>
        public string OrderMxId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 产品单价
        /// </summary>
        public string ItemPrice { get; set; }
    }
}
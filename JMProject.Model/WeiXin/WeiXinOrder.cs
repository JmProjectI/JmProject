using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinOrder
    {
        public WeiXinOrder()
        {
            OrderMX = new List<WeiXinOrderMX>();
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 老客户ID
        /// </summary>
        public string OldCusId { get; set; }
        /// <summary>
        /// 新客户ID
        /// </summary>
        public string CusId { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public string YwyId { get; set; }
        /// <summary>
        /// 订单状态  
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 是否新老客户
        /// </summary>
        public bool IsOldSaleCustom { get; set; }
        public List<WeiXinOrderMX> OrderMX { get; set; }

    }

    public class WeiXinOrderMX
    {
        /// <summary>
        /// 订单明细编号
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public string TypeID { get; set; }
        /// <summary>
        /// 产品金额
        /// </summary>
        public string Money { get; set; }
    }
}

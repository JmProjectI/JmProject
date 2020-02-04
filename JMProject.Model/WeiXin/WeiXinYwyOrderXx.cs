using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinYwyOrderXx
    {
        public WeiXinYwyOrderXx()
        { }

        //客户名称,合同状态,邮寄状态,开票状态//商品信息
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 合同状态
        /// </summary>
        public string FlagN { get; set; }

        /// <summary>
        /// 开票状态
        /// </summary>
        public string InvoiceFlagName { get; set; }

        /// <summary>
        /// 邮寄状态
        /// </summary>
        public string PostFlagName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemNames { get; set; }

        /// <summary>
        /// 产品金额
        /// </summary>
        public string ItemMoney { get; set; }

        //public List<OrderMx> OrderMx { get; set; }
    }

    //public class OrderMx
    //{
    //    /// <summary>
    //    /// 产品类别
    //    /// </summary>
    //    public string TypeName { get; set; }

    //    /// <summary>
    //    /// 单价
    //    /// </summary>
    //    public string ItemPrice { get; set; }
    //}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinQueRenXinX
    {
        public WeiXinQueRenXinX()
        {
            TypeName = new List<WeiXinQueRenXinXMX>();
        }
        
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Years { get; set; }

        /// <summary>
        /// 单位全称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Lxr { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public List<WeiXinQueRenXinXMX> TypeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Flag { get; set; }
    }

    public class WeiXinQueRenXinXMX
    {
        public WeiXinQueRenXinXMX(object _typename)
        {
            TypeName = _typename.ToString();
        }
        public string TypeName { get; set; }
    }
}
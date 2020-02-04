using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXin_GetWeb
    {
        public WeiXin_GetWeb()
        { }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 微信编号
        /// </summary>
        public string OpenId { get; set; }
    }
}

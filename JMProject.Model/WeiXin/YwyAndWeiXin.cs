using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class YwyAndWeiXin
    {
        public YwyAndWeiXin()
        { }

        /// <summary>
        /// 业务员编号
        /// </summary>
        public string YwyId { get; set; }

        /// <summary>
        /// 微信编号
        /// </summary>
        public string OpenId { get; set; }
    }
}

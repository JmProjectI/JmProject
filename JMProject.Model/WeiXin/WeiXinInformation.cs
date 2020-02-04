using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinInformation
    {
        public WeiXinInformation()
        {
            
        }

        /// <summary>
        /// 单位全称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Invoice { get; set; }

        /// <summary>
        /// 社会统一信用代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Lxr { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 单位地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 是否做过内控手册
        /// </summary>
        public int IsNksc { get; set; }
    }
}

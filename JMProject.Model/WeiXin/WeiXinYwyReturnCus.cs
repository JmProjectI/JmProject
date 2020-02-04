using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinYwyReturnCus
    {
        public WeiXinYwyReturnCus()
        { }

        /// <summary>
        /// 客户名称
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
        /// 合同明细
        /// </summary>
        public string ItemNames { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal ItemMoney { get; set; }
    }
}
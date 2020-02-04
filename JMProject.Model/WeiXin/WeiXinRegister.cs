using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinRegister
    {
        public WeiXinRegister()
        { }

        /// <summary>
        /// 微信编号
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CusId { get; set; }

        /// <summary>
        /// 改革前单位名称
        /// </summary>
        public string OldCusName { get; set; }

        /// <summary>
        /// 改革前社会统一信用代码
        /// </summary>
        public string OldCusCode { get; set; }

        /// <summary>
        /// 改革后单位名称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 改革后社会统一信用代码
        /// </summary>
        public string CusCode { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Invoice { get; set; }

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
        /// 业务员编号
        /// </summary>
        public string YwyId { get; set; }
    }
}

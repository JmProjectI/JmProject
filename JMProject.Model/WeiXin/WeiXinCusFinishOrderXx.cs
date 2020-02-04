using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinCusFinishOrderXx
    {
        public WeiXinCusFinishOrderXx()
        { }

        //返回合同状态相关信息( 根据客户编号 ) 商品信息分开显示,商品进度状态
        /// <summary>
        /// 产品名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Years { get; set; }

        /// <summary>
        /// 内控手册/内控报告状态
        /// </summary>
        public string Flag { get; set; }
    }
}

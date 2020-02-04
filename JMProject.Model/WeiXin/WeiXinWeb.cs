using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class WeiXinWeb
    {
        public WeiXinWeb()
        {
            Web = "http://www.baeit.com:7654";
        }

        /// <summary>
        /// 网址
        /// </summary>
        public string Web { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Users { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 温馨提示
        /// </summary>
        public string Remark { get; set; }
    }
}

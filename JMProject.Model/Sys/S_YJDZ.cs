using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class S_YJDZ
    {
        public string ID { get; set; }//编号
        public string Code { get; set; }//社会统一信用代码
        public string Name { get; set; }//单位全称
        public string QQ { get; set; }//单位全称
        public string Invoice { get; set; }//发票抬头 Invoice
        public string QtLxr { get; set; }//发票收件人
        public string QtTel { get; set; }//联系电话
        public string Address { get; set; }//邮寄地址        
    }
}

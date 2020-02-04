using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class WeiXinProductType
    {
        public WeiXinProductType()
        { }

        /// <summary>
        /// 产品编号
        /// </summary>
        [PrimaryKey]
        public String ProductTypeId { get; set; }
        
        /// <summary>
        /// 产品名称
        /// </summary>
        public String ProductTypeName { get; set; }
    }
}

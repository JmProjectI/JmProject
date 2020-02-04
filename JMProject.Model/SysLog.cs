using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SysLog
    {
        public SysLog()
        { }

        [PrimaryKey]
        public Guid Id { get; set; }
        public String Operator { get; set; }
        public String Message { get; set; }
        public String Type { get; set; }
        public String Module { get; set; }
        public DateTime CreateTime { get; set; }
        public String LogType { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_Zcyw
    {
        public Nksc_Zcyw()
        {
            id = Guid.NewGuid();
        }

        public Guid id { get; set; }

        public int sort { get; set; }
        public string zcmoney { get; set; }
        public string zctype { get; set; }
        public string zcspr { get; set; }
        public string zcywtype { get; set; }
    }
}

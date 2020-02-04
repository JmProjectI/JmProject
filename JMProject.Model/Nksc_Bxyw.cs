using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_Bxyw
    {
        public Nksc_Bxyw()
        {
            id = Guid.NewGuid();
        }

        public Guid id { get; set; }

        public int sort { get; set; }
        public string bxmoney { get; set; }
        public string bxtype { get; set; }
        public string bxspr { get; set; }
    }
}

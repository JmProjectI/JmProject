using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_cghtsq
    {
        public Nksc_cghtsq()
        {
            id = Guid.NewGuid();
        }

        public Guid id { get; set; }

        public int sort { get; set; }
        public string jkmoney { get; set; }
        public string jktype { get; set; }
        public string jkspr { get; set; }
        public string htywtype { get; set; }
    }
}

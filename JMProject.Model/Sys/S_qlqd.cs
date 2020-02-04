using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    public class S_qlqd
    {
        public S_qlqd()
        {
            qlqds = new List<Nksc_qlqd>();
        }

        public String leder { get; set; }
        public IList<Nksc_qlqd> qlqds { get; set; }
    }
}

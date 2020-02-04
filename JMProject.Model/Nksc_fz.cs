using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_fz
    {
        public Nksc_fz()
        {
            id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public int sort { get; set; }
        public string fzzwmc { get; set; }
        public string ldfzmc { get; set; }
        public string ldfzfg { get; set; }
        public string fzzwDY { get; set; }
        
    }
}
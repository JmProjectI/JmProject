using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_kszn
    {
        public Nksc_kszn()
        {
            id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public Guid zid { get; set; }
        public int sort { get; set; }
        public string ksmc { get; set; }
        public string kszn { get; set; }
        
    }
}
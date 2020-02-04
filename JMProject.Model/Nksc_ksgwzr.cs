using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_ksgwzr
    {
        public Nksc_ksgwzr()
        {
            id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public int sort { get; set; }
        public string ksgw { get; set; }
        public string ksgwzr { get; set; }
    }
}
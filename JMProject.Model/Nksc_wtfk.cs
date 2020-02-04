using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc_wtfk
    {
        public Nksc_wtfk()
        {

        }

        public Guid id { get; set; }
        [PrimaryKey]
        public string fkid { get; set; }
        public string wtFile { get; set; }
        public string riqi { get; set; }
        public string flags { get; set; }
        
    }
}
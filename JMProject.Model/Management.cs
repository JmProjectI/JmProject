using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Management
    {
        public Management()
        {
        }

        [PrimaryKey]
        [Identity]
        public int id { get; set; }

        public string name { get; set; }
    }
}

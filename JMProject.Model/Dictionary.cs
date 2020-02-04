using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Dictionary
    {
        public Dictionary()
        { }

        [PrimaryKey]
        public string DicID { get; set; }

        public string DicName { get; set; }

        public string DicImg { get; set; }

        public string DicFlag { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinProductType
    {
        public FinProductType()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Remake { get; set; }
        public String _parentId { get; set; }
    }
}

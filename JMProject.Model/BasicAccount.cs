using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class BasicAccount
    {
        public BasicAccount()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Key { get; set; }
        public String Bank { get; set; }
        public String BankNum { get; set; }
        public String SNum { get; set; }
    }
}

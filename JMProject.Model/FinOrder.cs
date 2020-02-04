using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class FinOrder
    {
        public FinOrder()
        { }

        [PrimaryKey]
        public String Id { get; set; }
        public String OrderDate { get; set; }
        public String SupplierId { get; set; }
	    public String ProductId { get; set; }
	    public String PkeyStart { get; set; }
        public String PkeyEnd { get; set; }
        public String UserId { get; set; }
        public String Remake { get; set; }
    }
}
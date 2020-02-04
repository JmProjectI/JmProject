using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.View
{
    public class View_FinOrder
    {
        public String Id { get; set; }
        public String OrderDate { get; set; }
        public String SupplierId { get; set; }
        public String ProductId { get; set; }
        public String PkeyStart { get; set; }
        public String PkeyEnd { get; set; }
        public String UserId { get; set; }
        public String Remake { get; set; }

        public String Name { get; set; }
        public String Linkman { get; set; }
        public String Phone { get; set; }
        public String Tel { get; set; }
        public String Address { get; set; }
        public String SupplierRemake { get; set; }

        public String ZsName { get; set; }

        //public int ItemCount { get; set; }
        public decimal ItemMoney { get; set; }

    }
}

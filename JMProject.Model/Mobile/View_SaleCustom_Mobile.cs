using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model.Mobile
{
    public class View_SaleCustom_Mobile
    {
        public View_SaleCustom_Mobile()
        {
        }
        
        [PrimaryKey]
        public string ID { get; set; }
	    public string CDate { get; set; }
        public string Ywy { get; set; }
        public string YwyName { get; set; }
	    public string Name { get; set; }
	    public string Lxr { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Finance { get; set; }
        public string CzjName { get; set; }
        public string Province { get; set; }
        public string SfName { get; set; }
        public string UpID { get; set; }
        public string UpName { get; set; }
        public string Region { get; set; }
        public string CityName { get; set; }
        public string QyName { get; set; }
        public string Remark { get; set; }
        
    }
}

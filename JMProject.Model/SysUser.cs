using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SysUser
    {
        public SysUser()
        { }

        public String RoleID { get; set; }
        [PrimaryKey]
        public String Id { get; set; }
        public String Name { get; set; }
        public String Pwd { get; set; }
        public String Pic { get; set; }
        public String ZsName { get; set; }
        public String Phone { get; set; }
        public String Tel { get; set; }
        public String IcCard { get; set; }
        public String Birthday { get; set; }
        public String Address { get; set; }
        public String Remake { get; set; }

    }
}

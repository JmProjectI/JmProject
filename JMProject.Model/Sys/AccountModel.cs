using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Sys
{
    [Serializable]
    public class AccountModel
    {
        public String RoleID { get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public String ZsName { get; set; }
    }
}

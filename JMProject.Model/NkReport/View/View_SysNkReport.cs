using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class View_SysNkReport
    {
        public View_SysNkReport()
        { }
        public string Id { get; set; }
        public Guid Zid { get; set; }
        public string date { get; set; }
        public string flag { get; set; }
        public string czr { get; set; }
        public string FlagName { get; set; }
        public string CzrName { get; set; }
    }
}

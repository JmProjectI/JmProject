using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model.Esayui
{
    public class EasyUIJsonTree
    {
        public string id { get; set; }
        public string text { get; set; }
        public string iconCls { get; set; }
        public string state { get; set; }
        public IList<EasyUIJsonTree> children { get; set; }
        public object attributes { get; set; }
    }
}

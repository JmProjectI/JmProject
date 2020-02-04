using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Model
{
    public class OrderCount
    {
        public OrderCount()
        {}

        public string OrderId { get; set; }
        public string Name { get; set; }
        public string YwyName { get; set; }
        public int Count { get; set; }
        public int Index { get; set; }
    }
}
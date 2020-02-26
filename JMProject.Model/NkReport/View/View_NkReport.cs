using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class View_NkReport
    {
        public View_NkReport()
        {
        }

        public string OrderId { get; set; }
        public string CustomId { get; set; }
        public Guid Id { get; set; }
        public string Years { get; set; }
        public string Flag { get; set; }
        public string Name { get; set; }
        public string Invoice { get; set; }
        public string Tjrq { get; set; }
        public string Tsyqtext { get; set; }
        public string Shrq { get; set; }
        public string Shr { get; set; }
        public string Zzrq { get; set; }
        public string Zzr { get; set; }
        public string Yjrq { get; set; }
        public string Yjr { get; set; }
        public string Fsrq { get; set; }
        public string Fsr { get; set; }
        public string Lsr { get; set; }
        public string ShrName { get; set; }
        public string ZzrName { get; set; }
        public string YjrName { get; set; }
        public string FsrName { get; set; }
    }
}
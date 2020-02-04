using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SaleVisit
    {
        public SaleVisit()
        { }

        [PrimaryKey]
        public string Id { get; set; }
        public string SaleCustomID { get; set; }
        public string ContactDate { get; set; }
        public string ContactTime { get; set; }
        public string Intention { get; set; }
        public string ContactFlag { get; set; }
        public string ContactSituation { get; set; }
        public string Progress { get; set; }
        public string ContactMode { get; set; }
        public string ContactDetails { get; set; }
        public string Offer { get; set; }
        public string NetContactTime { get; set; }
        public string ContactTarget { get; set; }
        public string Flag { get; set; }
        public string AuditDate { get; set; }
        public string Auditor { get; set; }
        public string AuditDetails { get; set; }
        public string AuditState { get; set; }
        public string Ywy { get; set; }
        public string DemandType { get; set; }
        public string NextTime { get; set; }
        public decimal Amount { get; set; }
        public string SaleOrderID { get; set; }
        public string Fj { get; set; }
    }
}

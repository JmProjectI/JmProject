using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class View_TecCusService
    {
        public View_TecCusService()
        { }

        public String Id { get; set; }
        public String GroupId { get; set; }
        public String Custom { get; set; }
        public String Ywy { get; set; }
        public String ServiceType { get; set; }
        public String BugType { get; set; }
        public String StartDate { get; set; }
        public Decimal TakeDay { get; set; }
        public Decimal TakeTime { get; set; }
        public String Remake { get; set; }
        public String CustomName { get; set; }
        public String ServiceTypeName { get; set; }
        public String BugTypeName { get; set; }
        public String ZsName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [View_TecCusService](");
            sb.Append("[Id]");
            sb.Append(",[GroupId]");
            sb.Append(",[Custom]");
            sb.Append(",[Ywy]");
            sb.Append(",[ServiceType]");
            sb.Append(",[BugType]");
            sb.Append(",[StartDate]");
            sb.Append(",[TakeDay]");
            sb.Append(",[TakeTime]");
            sb.Append(",[Remake]");
            sb.Append(",[CustomName]");
            sb.Append(",[ServiceTypeName]");
            sb.Append(",[BugTypeName]");
            sb.Append(",[ZsName]");
            sb.Append(") VALUES (");
            sb.Append("'" + Id + "'");
            sb.Append(",'" + GroupId + "'");
            sb.Append(",'" + Custom + "'");
            sb.Append(",'" + Ywy + "'");
            sb.Append(",'" + ServiceType + "'");
            sb.Append(",'" + BugType + "'");
            sb.Append(",'" + StartDate + "'");
            sb.Append(",'" + TakeDay + "'");
            sb.Append(",'" + TakeTime + "'");
            sb.Append(",'" + Remake + "'");
            sb.Append(",'" + CustomName + "'");
            sb.Append(",'" + ServiceTypeName + "'");
            sb.Append(",'" + BugTypeName + "'");
            sb.Append(",'" + ZsName + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class SaleCustom
    {
        public SaleCustom()
        {
        }

        [PrimaryKey]
        public string ID { get; set; }
        public string CDate { get; set; }
        public string Ywy { get; set; }
        public string Name { get; set; }
        public string BM { get; set; }
        public string Lxr { get; set; }
        public string Zw { get; set; }
        public string Phone { get; set; }
        public string Industry { get; set; }
        public string UpID { get; set; }
        public string Province { get; set; }
        public string Xydj { get; set; }
        public string Gx { get; set; }
        public string Zyx { get; set; }
        public string Tel { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LxrSR { get; set; }
        public string QtLxr { get; set; }
        public string QtTel { get; set; }
        public string Bank { get; set; }
        public string CardNum { get; set; }
        public string SuiH { get; set; }
        public string Desc { get; set; }
        public string Remark { get; set; }
        public string Flag { get; set; }
        public string Uid { get; set; }
        public string Source { get; set; }
        public string Region { get; set; }
        public string CustomerType { get; set; }
        public string CustomerGrade { get; set; }
        public string Code { get; set; }
        public string Invoice { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Finance { get; set; }
        public string YwyName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [SaleCustom](");
            sb.Append("[ID]");
            sb.Append(",[CDate]");
            sb.Append(",[Ywy]");
            sb.Append(",[Name]");
            sb.Append(",[Lxr]");
            sb.Append(",[Phone]");
            sb.Append(",[Tel]");
            sb.Append(",[QQ]");
            sb.Append(",[Email]");
            sb.Append(",[Address]");
            sb.Append(",[LxrSR]");
            sb.Append(",[QtLxr]");
            sb.Append(",[QtTel]");
            sb.Append(",[Bank]");
            sb.Append(",[CardNum]");
            sb.Append(",[SuiH]");
            sb.Append(",[Desc]");
            sb.Append(",[Remark]");
            sb.Append(",[Flag]");
            sb.Append(",[Uid]");    
            sb.Append(",[Code]");
            sb.Append(",[Invoice]");
            sb.Append(",[UserName]");
            sb.Append(",[UserPwd]"); 
            sb.Append(",[YwyName]");
            if (!string.IsNullOrEmpty(BM))
            {
                sb.Append(",[BM]");
            }
            if (!string.IsNullOrEmpty(Zw))
            {
                sb.Append(",[Zw]");
            }
            if (!string.IsNullOrEmpty(Industry))
            {
                sb.Append(",[Industry]");
            }
            if (!string.IsNullOrEmpty(UpID))
            {
                sb.Append(",[UpID]");
            }
            if (!string.IsNullOrEmpty(Province))
            {
                sb.Append(",[Province]");
            }
            if (!string.IsNullOrEmpty(Xydj))
            {
                sb.Append(",[Xydj]");
            }
            if (!string.IsNullOrEmpty(Gx))
            {
                sb.Append(",[Gx]");
            }
            if (!string.IsNullOrEmpty(Zyx))
            {
                sb.Append(",[Zyx]");
            }
            if (!string.IsNullOrEmpty(Source))
            {
                sb.Append(",[Source]");
            }
            if (!string.IsNullOrEmpty(Region))
            {
                sb.Append(",[Region]");
            }
            if (!string.IsNullOrEmpty(CustomerType))
            {
                sb.Append(",[CustomerType]");
            }
            if (!string.IsNullOrEmpty(CustomerGrade))
            {
                sb.Append(",[CustomerGrade]");
            }
            if (!string.IsNullOrEmpty(Finance))
            {
                sb.Append(",[Finance]");
            }
            sb.Append(") VALUES (");
            sb.Append("'" + ID + "'");
            sb.Append(",'" + CDate + "'");
            sb.Append(",'" + Ywy + "'");
            sb.Append(",'" + Name + "'");
            sb.Append(",'" + Lxr + "'");
            sb.Append(",'" + Phone + "'");
            sb.Append(",'" + Tel + "'");
            sb.Append(",'" + QQ + "'");
            sb.Append(",'" + Email + "'");
            sb.Append(",'" + Address + "'");
            sb.Append(",'" + LxrSR + "'");
            sb.Append(",'" + QtLxr + "'");
            sb.Append(",'" + QtTel + "'");
            sb.Append(",'" + Bank + "'");
            sb.Append(",'" + CardNum + "'");
            sb.Append(",'" + SuiH + "'");
            sb.Append(",'" + Desc + "'");
            sb.Append(",'" + Remark + "'");
            sb.Append(",'" + Flag + "'");
            sb.Append(",'" + Uid + "'");
            sb.Append(",'" + Code + "'");
            sb.Append(",'" + Invoice + "'");
            sb.Append(",'" + UserName + "'");
            sb.Append(",'" + UserPwd + "'");
            sb.Append(",'" + YwyName + "'");
            if (!string.IsNullOrEmpty(BM))
            {
                sb.Append(",'" + BM + "'");
            }
            if (!string.IsNullOrEmpty(Zw))
            {
                sb.Append(",'" + Zw + "'");
            }
            if (!string.IsNullOrEmpty(Industry))
            {
                sb.Append(",'" + Industry + "'");
            }
            if (!string.IsNullOrEmpty(UpID))
            {
                sb.Append(",'" + UpID + "'");
            }
            if (!string.IsNullOrEmpty(Province))
            {
                sb.Append(",'" + Province + "'");
            }
            if (!string.IsNullOrEmpty(Xydj))
            {
                sb.Append(",'" + Xydj + "'");
            }
            if (!string.IsNullOrEmpty(Gx))
            {
                sb.Append(",'" + Gx + "'");
            }
            if (!string.IsNullOrEmpty(Zyx))
            {
                sb.Append(",'" + Zyx + "'");
            }
            if (!string.IsNullOrEmpty(Source))
            {
                sb.Append(",'" + Source + "'");
            }
            if (!string.IsNullOrEmpty(Region))
            {
                sb.Append(",'" + Region + "'");
            }
            if (!string.IsNullOrEmpty(CustomerType))
            {
                sb.Append(",'" + CustomerType + "'");
            }
            if (!string.IsNullOrEmpty(CustomerGrade))
            {
                sb.Append(",'" + CustomerGrade + "'");
            }
            if (!string.IsNullOrEmpty(Finance))
            {
                sb.Append(",'" + Finance + "'");
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}      
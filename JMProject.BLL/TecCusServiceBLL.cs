using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using JMProject.Dal;
using JMProject.Common;
using JMProject.Model;
using JMProject.Model.Esayui;
using JMProject.Model.View;

namespace JMProject.BLL
{
    public class TecCusServiceBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public TecCusServiceBLL()
        { }

        public int Insert(TecCusService model)
        {
            return dao.Insert<TecCusService>(model);
        }
        public int Update(TecCusService model)
        {
            return dao.Update<TecCusService>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from TecCusService where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from TecCusService";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "000001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("000000");
            }
            return id;
        }
        public string MaxDateid(string D)
        {
            string id = "";
            String tsql = "select max(Id) from TecCusService where ID Like '" + D + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = D + "0001";
            }
            else
            {
                id = D + (int.Parse(result.Substring(8)) + 1).ToString("0000");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from TecCusService" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from TecCusService" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from TecCusService" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_TecCusService> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_TecCusService";
            string Fields = "[Id],[GroupId],[Custom],[Ywy],[ServiceType],[BugType],[StartDate],[TakeDay],[TakeTime],[Remake],[CustomName],[ServiceTypeName],[BugTypeName],[ZsName]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = "Order by " + pager.sort + " " + pager.order;
            }
            else
            {
                Order = "Order by  ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_TecCusService>("Proc_Page", sp);
        }

        public TecCusService GetRow(string Id)
        {
            return dao.GetRow<TecCusService>("select * from TecCusService where Id='" + Id + "'");
        }
        public DataTable GetData()
        {
            return GetData("");
        }
        public DataTable GetData(String _where)
        {
            return GetData("*", _where);
        }
        public DataTable GetData(String fieldsName, String _where)
        {
            return GetData(fieldsName, _where, "TecCusService");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }
        public bool Tran(Dictionary<string, object> tsqls)
        {
            return dao.Transaction(tsqls);
        }

        public List<TecCusServiceWeek> GetWeekData(DateTime sday)
        {
            string day1 = sday.ToString("yyyy-MM-dd");
            string day2 = sday.AddDays(1).ToString("yyyy-MM-dd");
            string day3 = sday.AddDays(2).ToString("yyyy-MM-dd");
            string day4 = sday.AddDays(3).ToString("yyyy-MM-dd");
            string day5 = sday.AddDays(4).ToString("yyyy-MM-dd");
            string day6 = sday.AddDays(5).ToString("yyyy-MM-dd");

            StringBuilder sb = new StringBuilder();
            sb.Append("select TO1.ZsName,yc1,sm1,yc2,sm2,yc3,sm3,yc4,sm4,yc5,sm5,yc6,sm6 from SysUser TO1");
            sb.Append(" left join (");
            sb.Append(" select ZsName");
            //sb.Append("--第一天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day1 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc1");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day1 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm1");
            //sb.Append("--第二天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day2 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc2");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day2 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm2");
            //sb.Append("--第三天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day3 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc3");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day3 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm3	");
            //sb.Append("--第四天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day4 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc4");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day4 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm4");
            //sb.Append("--第五天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day5 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc5");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day5 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm5");
            //sb.Append("--第六天");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day6 + "' and TI0.ServiceTypeName='远程'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as yc6");
            sb.Append(",STUFF((select ','+CustomName from View_TecCusService TI0");
            sb.Append("	 where T.ZsName=TI0.ZsName and TI0.StartDate='" + day6 + "' and TI0.ServiceTypeName='上门'");
            sb.Append("	 FOR XML PATH('')),1,1,'') as sm6 ");
            sb.Append(" from View_TecCusService T");
            sb.Append(" where StartDate>='" + day1 + "' and StartDate<='" + day6 + "'");
            sb.Append(" group by ZsName");
            sb.Append(" ) TO2 on TO1.ZsName=TO2.ZsName");
            sb.Append(" where RoleID='06'");
            return dao.Select<TecCusServiceWeek>(sb.ToString());
        }


        public DataTable GetMonthData(string sdate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select TO1.ZsName");
            sb.Append(",case when yc is null then 0 else yc end yc");
            sb.Append(",case when sm is null then 0 else sm end sm from SysUser TO1");
            sb.Append(" left join (");
            sb.Append("select ZsName");
            sb.Append(" , SUM(case when ServiceTypeName='远程' then 1 else 0 end) yc");
            sb.Append(" , SUM(case when ServiceTypeName='上门' then 1 else 0 end) sm");
            sb.Append(" from View_TecCusService");
            sb.Append(" where StartDate like '" + sdate + "%'");
            sb.Append(" group by ZsName");
            sb.Append(" ) TO2 on TO1.ZsName=TO2.ZsName");
            sb.Append(" where RoleID='06'");
            return dao.Select(sb.ToString());
        }

        public String getCName(string sdate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select STUFF(");
            sb.Append("(select ','+maxName from (");
            sb.Append(" select CustomName+'('+cast(COUNT(*) as varchar(5))+'次)' maxName from ");
            //sb.Append(" View_TecCusService where StartDate like '" + sdate + "%'");
            sb.Append(" (select distinct CustomName,StartDate from View_TecCusService where StartDate like '" + sdate + "%') TI0");
            sb.Append(" group by CustomName having COUNT(*)=(");
            sb.Append(" select MAX(cs)");
            sb.Append("  from (");
            sb.Append(" select COUNT(*) cs from (select distinct CustomName,StartDate from View_TecCusService where StartDate like '" + sdate + "%') TII group by CustomName");
            
            sb.Append(" ) TI2)");
            sb.Append(" ) TO1 FOR XML PATH(''))");
            sb.Append(" ,1,1,'') as cusName ");
            return dao.GetScalar(sb.ToString()).ToStringEx();
        }

        public String getTName(string sdate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select STUFF(");
            sb.Append(" (select ','+maxName from (");
            sb.Append("select CustomName+'（远程'+cast(yc as varchar(5))+' - 上门'+cast(sm as varchar(5))+'）（'+Remake+'）' maxName from (");
            sb.Append(" select CustomName,yc,sm,case when Remake is null then '' else Remake end Remake from (");
            sb.Append(" select CustomName");
            sb.Append(" ,SUM(case when ServiceTypeName='远程' then TakeDay else 0 end) yc");
            sb.Append(" ,SUM(case when ServiceTypeName='上门' then TakeDay else 0 end) sm");

            sb.Append(",(select STUFF( (select '、'+Remake from (");
            sb.Append(" select Remake from View_TecCusService TTTI ");
            sb.Append(" where TTTI.StartDate like '" + sdate + "%' and Remake<>'' ");
            sb.Append(" and CustomName=TS.CustomName) TTTI0 FOR XML PATH('')),1,1,'') as Remake) Remake");

            sb.Append(" from ");
            sb.Append(" (select distinct CustomName,ServiceTypeName,StartDate,TakeDay from View_TecCusService where StartDate like '" + sdate + "%')");
            sb.Append(" TS where CustomName in (");

            sb.Append(" select CustomName from ");
            //sb.Append(" View_TecCusService where StartDate like '" + sdate + "%'");
            sb.Append(" (select distinct CustomName,StartDate,TakeDay from View_TecCusService where StartDate like '" + sdate + "%') TII");
            sb.Append("  group by CustomName having sum(TakeDay)>2)");
            sb.Append(" group by CustomName) TA) T");
            sb.Append(" ) TO1 FOR XML PATH(''))");
            sb.Append("  ,1,1,'') as cusName ");
            sb.Append("");
            return dao.GetScalar(sb.ToString()).ToStringEx();
        }
    }
}

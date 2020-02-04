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

namespace JMProject.BLL
{
    public class CrsVisitBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public CrsVisitBLL()
        { }

        public int Insert(CrsVisit model)
        {
            return dao.Insert<CrsVisit>(model);
        }
        public int Update(CrsVisit model)
        {
            return dao.Update<CrsVisit>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from CrsVisit where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from CrsVisit";
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
            String tsql = "select max(Id) from CrsVisit where ID Like '" + D + "%'";
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
            String tsql = "select count(*) from CrsVisit" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from CrsVisit" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from CrsVisit" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_CrsVisit> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_CrsVisit";
            string Fields = "[Id],[Vyear],[Saler],[CustomID],[Falg],[VisitType],[ByearPay],[UpyearPay],[SumPay],[VisitDate],[VisitGood],[Remark],Name,CityName,HyName,Lxr,Tel,Zw,CustomerGrade";
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
                Order = "Order by Id ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_CrsVisit>("Proc_Page", sp);
        }
        public CrsVisit GetRow(CrsVisit model)
        {
            return dao.GetRow<CrsVisit>(model);
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
            return GetData(fieldsName, _where, "CrsVisit");
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

        /// <summary>
        /// 上门客户
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="Lxxf">是否连续两年</param>
        /// <param name="where">条件（含地区、行业等等）</param>
        /// <returns></returns>
        public DataTable GetVisit(int year, bool Lxxf, string where, string whereYwy)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("declare @pyear int");
            sb.Append(" set @pyear=" + year);
            sb.Append(" select TTout.ID,TTout.Name,Industry,Region,CustomerGrade,YwyName,bmoney,umoney");
            sb.Append(" ,BasicCity.Name CityName,DicHy.ItemName HyName,DicGrade.ItemName GradeName from(");
            sb.Append(" select * from (");
            sb.Append(" select ID,Name,Industry,Region,CustomerGrade,YwyName");
            sb.Append(" ,sum(case when [year]=@pyear then itemMoney else 0.00 end) bmoney");
            sb.Append(" ,sum(case when [year]=@pyear-1 then itemMoney else 0.00 end) umoney");
            sb.Append(" from SaleCustom T");
            sb.Append(" left join (");
            sb.Append("	select left(OrderDate,4) year,SaleCustomId,sum(ItemMoney) ItemMoney from SaleOrder T");
            sb.Append("	inner join SaleOrderItem T1 on T.Id=T1.OrderId");
            sb.Append("	group by left(OrderDate,4),SaleCustomId");
            sb.Append(" ) T1 on T1.SaleCustomId=T.ID");
            sb.Append("	 where not exists (select * from View_CrsVisit T2 where Vyear=@pyear and VisitType='0' and T.Region=T2.Region and T.Industry=T2.Industry)");
            if (!string.IsNullOrEmpty(whereYwy))
            {
                sb.Append(whereYwy);
            }
            sb.Append(" group by ID,Name,Industry,Region,CustomerGrade,YwyName) TOut");
            sb.Append(" where 1=1");
            
            if (Lxxf)
            {
                //是=连续两年  本年与去年 合同金额大于0
                sb.Append(" and bmoney>0 and umoney>0");
            }
            else
            {
                //否=连续两年  本年 合同金额大于0
                sb.Append(" and bmoney>0 ");
            }
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(where);
            }
            sb.Append(" ) TTout");
            sb.Append(" LEFT OUTER JOIN BasicCity AS BasicCity ON TTout.Region = BasicCity.ID");
            sb.Append(" LEFT OUTER JOIN DictionaryItem AS DicHy ON TTout.Industry = DicHy.ItemID ");
            sb.Append(" LEFT OUTER JOIN DictionaryItem AS DicGrade ON TTout.CustomerGrade = DicGrade.ItemID");
            DataTable dt = dao.Select(sb.ToString());
            return dt;
        }

        /// <summary>
        /// 上门客户
        /// </summary>
        /// <param name="year">年度</param>
        /// <param name="Lxxf">是否连续两年</param>
        /// <param name="where">条件（含地区、行业等等）</param>
        /// <param name="whereC">条件（2财务软件  3内控客户）</param>
        /// <returns></returns>
        public DataTable GetVisitPhone(int year, bool Lxxf, string where, string whereC, string VisitType, string whereYwy)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("declare @pyear int");
            sb.Append(" set @pyear=" + year);
            sb.Append(" select TTout.ID,TTout.Name,Industry,Region,CustomerGrade,YwyName,bmoney,umoney");
            sb.Append(" ,BasicCity.Name CityName,DicHy.ItemName HyName,DicGrade.ItemName GradeName from(");
            sb.Append(" select * from (");
            sb.Append(" select ID,Name,Industry,Region,CustomerGrade,YwyName");
            sb.Append(" ,sum(case when [year]=@pyear then itemMoney else 0.00 end) bmoney");
            sb.Append(" ,sum(case when [year]=@pyear-1 then itemMoney else 0.00 end) umoney");
            sb.Append(" from SaleCustom T");
            sb.Append(" left join (");
            sb.Append("	select left(OrderDate,4) year,SaleCustomId,sum(ItemMoney) ItemMoney from SaleOrder T");
            sb.Append("	inner join SaleOrderItem T1 on T.Id=T1.OrderId");
            if (!string.IsNullOrEmpty(whereC))
            {
                sb.Append(whereC);
            }
            sb.Append("	group by left(OrderDate,4),SaleCustomId");
            sb.Append(" ) T1 on T1.SaleCustomId=T.ID");
            sb.Append("	 where not exists (select * from View_CrsVisit T2 where Vyear=@pyear and VisitType='" + VisitType + "' and T.Region=T2.Region and T.Industry=T2.Industry)");
            sb.Append(" and ID not in (select CustomID from View_CrsVisit where Vyear=@pyear and VisitType='0')");
            if (!string.IsNullOrEmpty(whereYwy))
            {
                sb.Append(whereYwy);
            }
            sb.Append(" group by ID,Name,Industry,Region,CustomerGrade,YwyName) TOut");
            sb.Append(" where 1=1");
            if (Lxxf)
            {
                sb.Append(" and bmoney>0 and umoney>0");
            }
            else
            {
                sb.Append(" and bmoney>0 ");
            }
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append(where);
            }
            sb.Append(" ) TTout");
            sb.Append(" LEFT OUTER JOIN BasicCity AS BasicCity ON TTout.Region = BasicCity.ID");
            sb.Append(" LEFT OUTER JOIN DictionaryItem AS DicHy ON TTout.Industry = DicHy.ItemID ");
            sb.Append(" LEFT OUTER JOIN DictionaryItem AS DicGrade ON TTout.CustomerGrade = DicGrade.ItemID");
            DataTable dt = dao.Select(sb.ToString());
            return dt;
        }
    }
}
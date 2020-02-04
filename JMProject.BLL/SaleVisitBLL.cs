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
using JMProject.Model.Sys;

namespace JMProject.BLL
{
    public class SaleVisitBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public SaleVisitBLL()
        { }

        public string MaxId()
        {
            string id = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(Id) from SaleVisit";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "00000001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("00000000");
            }
            return id;
        }

        public string GetStrName(string name, string where)
        {
            return dao.GetScalar("select " + name + " from SaleVisit " + where).ToStringEx();
        }

        public int Insert(SaleVisit model)
        {
            return dao.Insert<SaleVisit>(model);
        }

        public int Update(SaleVisit model)
        {
            return dao.Update<SaleVisit>(model);
        }

        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        public int Delete(String Id)
        {
            //删除拜访沟通
            return dao.Delete("delete from SaleVisit where Id='" + Id + "'");
        }

        public List<View_SaleVisit> SelectAll(string Where, GridPager pager)
        {
            //string str = "";
            //dao.GetScalar("(select stuff((select '，'+dbo.DictionaryItem.ItemName from dbo.DictionaryItem where dbo.DictionaryItem.ItemID in "
            //    + "(dbo.SaleVisit.DemandType) for xml path('')),1,1,'')) AS DemandTypeName");
            string Order = string.Empty;
            string Table = "View_SaleVisit";
            string Fields = "[Id],[SaleCustomID],[ContactDate],[ContactTime],[Intention],[ContactFlag],[ContactSituation],[Progress]"
                + ",[ContactMode],[ContactDetails],[Offer],[NetContactTime],[ContactTarget],[Flag],[AuditDate],[Auditor],[AuditDetails]"
                + ",[AuditState],[Ywy],[YwyName],[Name],[Lxr],[YxName],[LlztName],[LlqkName],[JdName],[LlfsName],[XcllsjName],[ZtName]"
                + ",[ShRen],[ShztName],[DemandType],[DemandTypeName],[NextTime],[Amount],[SaleOrderID],[EndDay],[Fj]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            List<View_SaleVisit> lis = dao.ProExecSelect<View_SaleVisit>("Proc_Page", sp);
            for (int i = 0; i < lis.Count; i++)
            {
                string[] lislen = lis[i].DemandType.Split(',');
                if (lislen.Length > 1)
                {
                    string str = lis[i].DemandType;
                    string XQType = dao.GetScalar("select stuff((select '，'+ItemName from DictionaryItem where ItemID in (" + str + ") for xml path('')),1,1,'') as name").ToStringEx();
                    lis[i].DemandTypeName = XQType;
                }
            }
            return lis;
        }

        public List<View_SaleVisit> SelectAllSum(string Where)
        {
            string Table = "View_SaleVisit";
            string Fields = "'合计' as [Name],sum(Amount) as [Amount]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_SaleVisit>("select " + Fields + " from " + Table + " " + Where);
        }

        public List<View_SaleVisit_History> SelectAll_History(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleVisit";
            string Fields = "[Id],[SaleCustomID],[ContactDate],[Offer],[ContactDetails]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_SaleVisit_History>("Proc_Page", sp);
        }

        public List<View_SaleVisit_AuditDetails> SelectAll_AuditDetails(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleVisit";
            string Fields = "[Id],[AuditDetails]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_SaleVisit_AuditDetails>("Proc_Page", sp);
        }

        public List<View_SaleVisit_Up> SelectAll_Fj(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleVisit";
            string Fields = "[Id],[Fj]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_SaleVisit_Up>("Proc_Page", sp);
        }

        public View_SaleVisit GetRow(string Id)
        {
            return dao.GetRow<View_SaleVisit>("select * from View_SaleVisit where Id='" + Id + "'");
        }

        public View_SaleVisit GetRows(View_SaleVisit model)
        {
            View_SaleVisit list = dao.GetRow<View_SaleVisit>(model);
            string str = list.DemandType;
            string XQType = dao.GetScalar("select stuff((select '，'+ItemName from DictionaryItem where ItemID in (" + str + ") for xml path('')),1,1,'') as name").ToStringEx();
            list.DemandTypeName = XQType;
            return list;
        }

        public List<View_SaleVisit_LlztTx> SelectAll_LlztTx(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleVisit_LlztTx";
            string Fields = "[Id],[Name],[ContactDate],[LlztName],[DemandType],[DemandTypeName],[Ywy],[ContactFlag],[daoqidate],[tixingdate],[ZsName]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            List<View_SaleVisit_LlztTx> lis = dao.ProExecSelect<View_SaleVisit_LlztTx>("Proc_Page", sp);
            for (int i = 0; i < lis.Count; i++)
            {
                string[] lislen = lis[i].DemandType.Split(',');
                if (lislen.Length > 1)
                {
                    string str = lis[i].DemandType;
                    string XQType = dao.GetScalar("select stuff((select '，'+ItemName from DictionaryItem where ItemID in (" + str + ") for xml path('')),1,1,'') as name").ToStringEx();
                    lis[i].DemandTypeName = XQType;
                }
            }
            return lis;
        }        
    }
}
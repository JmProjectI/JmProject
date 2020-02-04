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
    public class FinOutStockBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinOutStockBLL()
        { }

        public int Insert(FinOutStock model)
        {
            return dao.Insert<FinOutStock>(model);
        }

        public int Update(FinOutStock model)
        {
            return dao.Update<FinOutStock>(model);
        }

        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        public Dictionary<string, object> GetDeleteSql(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            List<FinOutStockItem> items = dao.Select<FinOutStockItem>("select * from FinOutStockItem where OutStockId='" + id + "'");
            foreach (var item in items)
            {
                string sqlProduct = "update FinProduct set OutCount=OutCount-" + item.OutStockCount + ",stock=stock+" + item.OutStockCount + " where Id='" + item.ProductId + "'";
                tsqls.Add(sqlProduct, null);
            }
            tsqls.Add("delete from FinOutStockItem where OutStockId='" + id + "'", null);
            return tsqls;
        }

        public bool Delete(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            List<FinOutStockItem> items = dao.Select<FinOutStockItem>("select * from FinOutStockItem where OutStockId='" + id + "'");
            foreach (var item in items)
            {
                string sqlProduct = "update FinProduct set OutCount=OutCount-" + item.OutStockCount + ",stock=stock+" + item.OutStockCount + " where Id='" + item.ProductId + "'";
                tsqls.Add(sqlProduct, null);
            }
            tsqls.Add("delete from FinOutStockItem where OutStockId='" + id + "'", null);
            tsqls.Add("delete from FinOutStock where OSId='" + id + "'", null);
            return dao.Transaction(tsqls);
        }

        public string Maxid(string D)
        {
            string id = "";
            String tsql = "select max(OSId) from FinOutStock where OSId Like '" + D + "%'";
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
            String tsql = "select count(*) from FinOutStock" + where;
            return dao.IsExists(tsql);
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOutStock" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from View_FinOutStock" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_FinOutStock> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOutStock";
            string Fields = "[OrderId],[OSId],[OSdate],[OutStockCount],[Uid],[ZsName]";
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
                Order = "Order by OSId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_FinOutStock>("Proc_Page", sp);
        }

        public List<View_FinOutStock> SelectAllSum(string Where)
        {
            string Table = "View_FinOutStock";
            string Fields = "'' as [OrderId],'合计' as [OSId],sum(OutStockCount) as [OutStockCount]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_FinOutStock>("select " + Fields + " from " + Table + " " + Where);
        }

        public FinOutStock GetRow(FinOutStock model)
        {
            return dao.GetRow<FinOutStock>(model);
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
            return GetData(fieldsName, _where, "FinOutStock");
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

        public int GetStockItemCount(string _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOutStockItem" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public List<View_FinOutStockItem> SelectItemAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOutStockItem";
            string Fields = "[OutStockId],[SaleOrderItemId],[ProductId],[Marketprice],[Costprice],[OutStockCount],[ProName],[ProSpec],[ProUcount],[ProPkey],[ProductType],[TypeName]";
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
            return dao.ProExecSelect<View_FinOutStockItem>("Proc_Page", sp);
        }

        public List<View_OutStock> SelectOutStockAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_OutStock";
            string Fields = "[OrderId],[OSId],[OSdate],[OutStockCount],[Uid],[ZsName],[SaleOrderItemId],[ProductId],[Marketprice],[Costprice]"
                + ",[ProName],[ProSpec],[ProUcount],[ProPkey],[ProductType],[TypeName]";
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
                Order = "Order by OSId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_OutStock>("Proc_Page", sp);
        }

        public List<View_OutStock> SelectOutStockAllSum(string Where)
        {
            string Table = "View_OutStock";
            string Fields = "'' as [OrderId],'合计' as [OSId],sum(OutStockCount) as [OutStockCount]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_OutStock>("select " + Fields + " from " + Table + " " + Where);
        }
    }
}

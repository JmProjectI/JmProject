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
    public class FinOrderBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinOrderBLL()
        { }

        public int Insert(FinOrder model)
        {
            return dao.Insert<FinOrder>(model);
        }
        public int Update(FinOrder model)
        {
            return dao.Update<FinOrder>(model);
        }
        public Dictionary<string, object> GetDeleteSql(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            List<FinOrderItem> items = dao.Select<FinOrderItem>("select * from FinOrderItem where OrderId='" + id + "'");
            FinProductBLL pbll = new FinProductBLL();
            foreach (var item in items)
            {
                //string sqlProduct = "";
                //string pstock = pbll.GetNameStr("stock", "and TypeId='" + item.ProductTypeId + "' and Name='" + item.ProName + "' and Pkey='" + item.Pkey + "'");
                //if (int.Parse(pstock) != item.ItemCount)
                //{
                //string sqlProduct = "update FinProduct set InCount=InCount-" + item.ItemCount + ",stock=stock-" + item.ItemCount + " where TypeId='" + item.ProductTypeId + "' and Name='" + item.ProName + "' and Pkey='" + item.Pkey + "'";
                //}
                //else
                //{
                //    sqlProduct = "delete from FinProduct where TypeId='" + item.ProductTypeId + "' and Name='" + item.ProName + "' and Pkey='" + item.Pkey + "'";
                //}
                //tsqls.Add(sqlProduct, null);
            }
            tsqls.Add("delete from FinOrderItem where OrderId='" + id + "'", null);
            return tsqls;
        }

        public bool Delete(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            //List<FinProduct> items = dao.Select<FinProduct>("select * from FinProduct where Spec='" + id + "'");
            //foreach (var item in items)
            //{
            //    string sqlProduct = "delete from FinProductItem where ProId in (select Id from FinProduct where Spec='" + id + "')";
            //    tsqls.Add(sqlProduct, null);
            //}
            tsqls.Add("delete from FinProductItem where ProId in (select Id from FinProduct where Spec='" + id + "')", null);
            tsqls.Add("delete from FinProduct where Spec='" + id + "'", null);
            tsqls.Add("delete from FinOrderItem where OrderId='" + id + "'", null);
            tsqls.Add("delete from FinOrder where Id='" + id + "'", null);
            return dao.Transaction(tsqls);
        }

        public string Maxid(string D)
        {
            string id = "";
            String tsql = "select max(Id) from FinOrder where Id Like '" + D + "%'";
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
            String tsql = "select count(*) from FinOrder" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOrder" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinOrder" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_FinOrder> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOrder";
            string Fields = "[Id],[OrderDate],[SupplierId],[Name],[UserId],[ZsName],[Remake],[ItemMoney]";
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
            return dao.ProExecSelect<View_FinOrder>("Proc_Page", sp);
        }
        public FinOrder GetRow(FinOrder model)
        {
            return dao.GetRow<FinOrder>(model);
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
            return GetData(fieldsName, _where, "FinOrder");
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

        public List<View_FinOrderItem> SelectItemAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOrderItem";
            string Fields = "[OrderId],[ItemId],[ModularID],[TypeName],[CbMoney],[JcMoney],[CsCount],[AddCount],[AddMoney],[SumCount],[sumMoney],[ItemName]";
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
                Order = "Order by ItemId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_FinOrderItem>("Proc_Page", sp);
        }
    }
}
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
    public class FinProductItemBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinProductItemBLL()
        { }

        public string Max_FinProductItem(string D)
        {
            string id = "";
            String tsql = "select max(ItemID) from FinProductItem where ItemID Like '" + D + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = D + "000001";
            }
            else
            {
                id = D + (int.Parse(result.Substring(8)) + 1).ToString("000000");
            }
            return id;
        }

        public int Insert(FinProductItem model)
        {
            return dao.Insert<FinProductItem>(model);
        }
        public int Update(FinProductItem model)
        {
            return dao.Update<FinProductItem>(model);
        }

        public int Delete(String _where)
        {
            string where = " where 1=1 " + _where;
            string tsql = "delete from FinProductItem " + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select COUNT(*) from FinProductItem" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinProductItem" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_FinProductItem> SelectItemAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinProductItem";
            string Fields = "[ItemID],[ProId],[ModularID],[CbMoney],[JcMoney],[CsCount],[AddCount],[AddMoney],[SumCount],[sumMoney],[TypeName]";
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
            return dao.ProExecSelect<View_FinProductItem>("Proc_Page", sp);
        }
    }
}

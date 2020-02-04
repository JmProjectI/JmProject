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
    public class FinSalesCommissionBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinSalesCommissionBLL()
        { }

        public int Insert(FinSalesCommission model)
        {
            return dao.Insert<FinSalesCommission>(model);
        }
        public int Update(FinSalesCommission model)
        {
            return dao.Update<FinSalesCommission>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from FinSalesCommission where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from FinSalesCommission";
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
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinSalesCommission" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinSalesCommission" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinSalesCommission" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_FinSalesCommission> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinSalesCommission";
            string Fields = "_parentId,TypeId,Name,[Id],[Unfinished],[Finished],[Nonsalesman]";
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
            return dao.ProExecSelect<View_FinSalesCommission>("Proc_Page", sp);
        }
        public View_FinSalesCommission GetRow(View_FinSalesCommission model)
        {
            return dao.GetRow<View_FinSalesCommission>("select * from View_FinSalesCommission where Id='" + model.Id + "'");
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
            return GetData(fieldsName, _where, "FinSalesCommission");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }
    }
}

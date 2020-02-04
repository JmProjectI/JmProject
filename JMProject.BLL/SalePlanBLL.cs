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
    public class SalePlanBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SalePlanBLL()
        { }

        public int Insert(SalePlan model)
        {
            return dao.Insert<SalePlan>(model);
        }
        public int Update(SalePlan model)
        {
            return dao.Update<SalePlan>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from SalePlan where Id='" + id + "'");
        }
        public string Maxid(string Year)
        {
            string id = "";
            String tsql = "select max(Id) from SalePlan where Year='" + Year + "'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = Year + "0001";
            }
            else
            {
                id = Year + (int.Parse(result.Substring(4)) + 1).ToString("0000");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SalePlan" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SalePlan" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SalePlan" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_SalePlan> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SalePlan";
            string Fields = "[Id],[Year],[Saler],[YearTarget],[MonthTarget],[AddedTarget],[AddedTarget1],[ZsName]";
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
            return dao.ProExecSelect<View_SalePlan>("Proc_Page", sp);
        }
        public SalePlan GetRow(SalePlan model)
        {
            return dao.GetRow<SalePlan>(model);
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
            return GetData(fieldsName, _where, "SalePlan");
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
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
    //111111111122
    public class BasicAccountBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public BasicAccountBLL()
        { }

        public int Insert(BasicAccount model)
        {
            return dao.Insert<BasicAccount>(model);
        }
        public int Update(BasicAccount model)
        {
            return dao.Update<BasicAccount>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from BasicAccount where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from BasicAccount";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "01";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("00");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicAccount" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicAccount" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from BasicAccount" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<BasicAccount> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "BasicAccount";
            string Fields = "[Id],[Name],[Key],[Bank],[BankNum],[SNum]";
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
            return dao.ProExecSelect<BasicAccount>("Proc_Page", sp);
        }
        public BasicAccount GetRow(BasicAccount model)
        {
            return dao.GetRow<BasicAccount>(model);
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
            return GetData(fieldsName, _where, "BasicAccount");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public DataTable GetDataTsql(String tsql)
        {
            DataTable dt = dao.Select(tsql);
            return dt;
        }
        public int Insert(string tsql)
        {
            return dao.Insert(tsql);
        }
    }
}

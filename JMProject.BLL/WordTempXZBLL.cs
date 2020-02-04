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
    public class WordTempXZBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public WordTempXZBLL()
        { }

        public int Insert(WordTempXZ model)
        {
            return dao.Insert<WordTempXZ>(model);
        }
        public int Update(WordTempXZ model)
        {
            return dao.Update<WordTempXZ>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from WordTempXZ where ID='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(ID) from WordTempXZ";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "0001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("0000");
            }
            return id;
        }

        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from WordTempXZ" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from WordTempXZ" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from WordTempXZ" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<WordTempXZ> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "WordTempXZ";
            string Fields = "[ID],[dkey],[zz],[fz],[qtks],[cy]";
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
                Order = "Order by ID ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<WordTempXZ>("Proc_Page", sp);
        }
        public WordTempXZ GetRow(WordTempXZ model)
        {
            return dao.GetRow<WordTempXZ>(model);
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
            return GetData(fieldsName, _where, "WordTempXZ");
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
    }
}

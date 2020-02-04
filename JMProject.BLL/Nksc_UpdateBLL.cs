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
    public class Nksc_UpdateBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public Nksc_UpdateBLL()
        { }

        public int Insert(Nksc_Update model)
        {
            return dao.Insert<Nksc_Update>(model);
        }

        public int Update(Nksc_Update model)
        {
            return dao.Update<Nksc_Update>(model);
        }

        public int Delete(String id)
        {
            return dao.Delete("delete from Nksc_Update where id='" + id + "'");
        }

        public string Maxid()
        {
            string id = "";
            String tsql = "select max(id) from Nksc_Update";
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
            String tsql = "select count(*) from Nksc_Update" + where;
            return dao.IsExists(tsql);
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from Nksc_Update" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from Nksc_Update" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<Nksc_Update> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "Nksc_Update";
            string Fields = "[id],[CustomerID],[NkscDate],[versionS],[versionE],[UpdateFlag]";
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
                Order = "Order by id ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<Nksc_Update>("Proc_Page", sp);
        }

        public Nksc_Update GetRow(Nksc_Update model)
        {
            return dao.GetRow<Nksc_Update>(model);
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
            return GetData(fieldsName, _where, "Nksc_Update");
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

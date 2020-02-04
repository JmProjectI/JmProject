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
    public class FinProductTypeBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinProductTypeBLL()
        { }

        public int Insert(FinProductType model)
        {
            return dao.Insert<FinProductType>(model);
        }
        public int Update(FinProductType model)
        {
            return dao.Update<FinProductType>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from FinProductType where Id='" + id + "'");
        }
        public string Maxid(string _parentId)
        {
            string id = "";
            string where = " where 1=1";
            if (string.IsNullOrEmpty(_parentId))
            {
                where += " and len(Id)=2";
            }
            else
            {
                where += " and _parentId='" + _parentId + "'";
            }
            String tsql = "select max(Id) from FinProductType" + where;
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = _parentId + "01";
            }
            else
            {
                if (string.IsNullOrEmpty(_parentId))
                {
                    id = (int.Parse(result) + 1).ToString("00");
                }
                else
                {
                    id = _parentId + (int.Parse(result.Substring(_parentId.Length)) + 1).ToString("00");
                }
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinProductType" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinProductType" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinProductType" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<FinProductType> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "FinProductType";
            string Fields = "[Id],[Name],[Remake],[_parentId]";
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
            return dao.ProExecSelect<FinProductType>("Proc_Page", sp);
        }

        public List<WeiXinProductType> WxSelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "FinProductType";
            string Fields = "[Id],[Name]";
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
            return dao.ProExecSelect<WeiXinProductType>("Proc_Page", sp);
        }

        public FinProductType GetRow(FinProductType model)
        {
            return dao.GetRow<FinProductType>(model);
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
            return GetData(fieldsName, _where, "FinProductType");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public List<string> getLastName()
        {
            string tsql = "SELECT Name FROM FinProductType t WHERE NOT EXISTS(SELECT * FROM FinProductType WHERE Id LIKE t.Id+'_%') order by t.Id";
            DataTable dt = dao.Select(tsql);
            List<string> result = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                result.Add(item["Name"].ToStringEx());
            }
            return result;
        }
    }
}


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
    public class WordTempFileBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public WordTempFileBLL()
        { }

        public int Insert(WordTempFile model)
        {
            return dao.Insert<WordTempFile>(model);
        }
        public int Update(WordTempFile model)
        {
            return dao.Update<WordTempFile>(model);
        }
        public int UpdateSortAdd(int sort)
        {
            return dao.Update("update WordTempFile set Sort=Sort+1 where Sort>=" + sort);
        }

        public int Delete(String id)
        {
            dao.Update("update WordTempFile set Sort=Sort-1 where Sort>=(select Sort from WordTempFile where ID='" + id + "')");
            return dao.Delete("delete from WordTempFile where ID='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(ID) from WordTempFile";
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
        public string MaxDateid(string D)
        {
            string id = "";
            String tsql = "select max(ID) from WordTempFile where ID Like '" + D + "%'";
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
            String tsql = "select count(*) from WordTempFile" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from WordTempFile" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from WordTempFile" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<WordTempFile> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "WordTempFile";
            string Fields = "[ID],[Name],[WordFile],[NewPage],[ywKey],[Sort]";
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
            return dao.ProExecSelect<WordTempFile>("Proc_Page", sp);
        }
        public WordTempFile GetRow(WordTempFile model)
        {
            return dao.GetRow<WordTempFile>(model);
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
            return GetData(fieldsName, _where, "WordTempFile");
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
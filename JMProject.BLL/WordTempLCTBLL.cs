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
    public class WordTempLCTBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public WordTempLCTBLL()
        { }

        public int Insert(WordTempLCT model)
        {
            return dao.Insert<WordTempLCT>(model);
        }
        public int Update(WordTempLCT model)
        {
            return dao.Update<WordTempLCT>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from WordTempLCT where ID='" + id + "'");
        }
        public Guid Maxid()
        {            
            return Guid.NewGuid();
        }

        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from WordTempLCT" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from WordTempLCT" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from WordTempLCT" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<WordTempLCT> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "WordTempLCT";
            string Fields = "[ID],[wkey],[dkey],[formate],[fontName],[fontSize],[x],[y],[w],[h]";
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
            return dao.ProExecSelect<WordTempLCT>("Proc_Page", sp);
        }
        public WordTempLCT GetRow(WordTempLCT model)
        {
            return dao.GetRow<WordTempLCT>(model);
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
            return GetData(fieldsName, _where, "WordTempLCT");
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
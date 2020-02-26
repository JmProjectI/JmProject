using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Model;
using JMProject.Dal;
using JMProject.Common;
using System.Data;
using System.Data.SqlClient;
using JMProject.Model.Esayui;

namespace JMProject.BLL
{
    public class DictionaryBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public int Insert(DictionaryItem model)
        {
            return dao.Insert<DictionaryItem>(model);
        }
        public int Update(DictionaryItem model)
        {
            return dao.Update<DictionaryItem>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from DictionaryItem where ItemID='" + id + "'");
        }

        public string Maxid()
        {
            string id = "";
            String tsql = "select max(ItemID) from DictionaryItem";
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
            String tsql = "select count(*) from DictionaryItem" + where;
            return dao.IsExists(tsql);
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from DictionaryItem" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from DictionaryItem" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public String GetNameStr(String fieldName,string Tname, String _where)
        {
            String where = " where DicName='"+ Tname +"' " + _where;
            String tsql = "select " + fieldName + " from View_Dic" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<Dictionary> SelectAll(string Where, string Order)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(Order))
            {
                Order = "Order by " + Order;
            }
            else
            {
                Order = "Order by DicID ASC";
            }
            string tsql = "select * from Dictionary " + Where + Order;
            List<Dictionary> result = dao.Select<Dictionary>(tsql);
            return result;
        }

        public List<DictionaryItem> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "DictionaryItem";
            string Fields = "[DicID],[ItemID],[ItemName],[ItemDesc],[ItemFlag]";
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
                Order = "Order by ItemID ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<DictionaryItem>("Proc_Page", sp);
        }

        public Dictionary GetRow(Dictionary model)
        {
            return dao.GetRow<Dictionary>(model);
        }

        public DictionaryItem GetRowItem(DictionaryItem model)
        {
            return dao.GetRow<DictionaryItem>(model);
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
            return GetData(fieldsName, _where, "DictionaryItem");
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

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
    public class BasicCityPLBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public BasicCityPLBLL()
        { }

        public int Insert(BasicCityPL model)
        {
            return dao.Insert<BasicCityPL>(model);
        }
        public int Update(BasicCityPL model)
        {
            return dao.Update<BasicCityPL>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from BasicCityPL where ID='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(ID) from BasicCityPL";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "00000001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("00000000");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicCityPL" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicCityPL" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from View_BasicCityPL" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_BasicCityPL> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_BasicCityPL";
            string Fields = "[ID],[Name],[Sfid],[SfName]";
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
            return dao.ProExecSelect<View_BasicCityPL>("Proc_Page", sp);
        }
        public BasicCityPL GetRow(BasicCityPL model)
        {
            return dao.GetRow<BasicCityPL>(model);
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
            return GetData(fieldsName, _where, "View_BasicCityPL");
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

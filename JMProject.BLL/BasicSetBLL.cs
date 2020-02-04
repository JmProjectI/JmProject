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
    public class BasicSetBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public BasicSetBLL()
        { }

        public int Insert(BasicSet model)
        {
            return dao.Insert<BasicSet>(model);
        }
        public int Update(BasicSet model)
        {
            return dao.Update("Update BasicSet SET [PercentZ] = '" + model.PercentZ + "',[PercentY] = '" + model.PercentY + "',[PercentC] = '" + model.PercentC + "',[PercentN] = '" + model.PercentN + "' WHERE [Userid] = '" + model.Userid + "'");
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from BasicSet where Userid='" + id + "'");
        }
        
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicSet" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from BasicSet" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from BasicSet" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<BasicSet> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "BasicSet";
            string Fields = "[Userid],[PercentZ],[PercentY],[PercentC],[PercentN]";
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
                Order = "Order by Userid ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<BasicSet>("Proc_Page", sp);
        }
        public BasicSet GetRow(BasicSet model)
        {
            return dao.GetRow<BasicSet>(model);
        }

        public BasicSet GetRow(string Userid)
        {
            return dao.GetRow<BasicSet>("select * from BasicSet where Userid='" + Userid + "'");
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
            return GetData(fieldsName, _where, "BasicSet");
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
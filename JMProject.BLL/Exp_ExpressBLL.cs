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
    public class Exp_ExpressBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public Exp_ExpressBLL()
        { }

        public int Insert(Exp_Express model)
        {
            return dao.Insert<Exp_Express>(model);
        }
        public int Update(Exp_Express model)
        {
            return dao.Update<Exp_Express>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from Exp_Express where LogisticCode='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(LogisticCode) from Exp_Express";
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
            String tsql = "select max(LogisticCode) from Exp_Express where ID Like '" + D + "%'";
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
            String tsql = "select count(*) from Exp_Express" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from Exp_Express" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from Exp_Express" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<Exp_Express> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "Exp_Express";
            string Fields = "[LogisticCode],[ShipperCode],[OrderId],[ReceiverName],[Tel],[Mobile],[ProvinceName],[CityName],[ExpAreaName],[Address],[GoodsName],[State],[Reason],[ExpressTime]";
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
                Order = "Order by LogisticCode ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<Exp_Express>("Proc_Page", sp);
        }
        public Exp_Express GetRow(string _where)
        {
            string where = " where 1=1 " + _where;
            return dao.GetRow<Exp_Express>("select * from Exp_Express" + where);
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
            return GetData(fieldsName, _where, "Exp_Express");
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
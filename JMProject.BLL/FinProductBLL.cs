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
    public class FinProductBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public FinProductBLL()
        { }

        public int Insert(FinProduct model)
        {
            return dao.Insert<FinProduct>(model);
        }

        public int Update(FinProduct model)
        {
            return dao.Update<FinProduct>(model);
        }

        public int Delete(String id)
        {
            return dao.Delete("delete from FinProduct where Id='" + id + "'");
        }

        public string Maxid(string TypeId)
        {
            string id = "";
            //String tsql = "select max(Id) from FinProduct where TypeId='" + TypeId + "'";
            String tsql = "select max(Id) from FinProduct";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "000000000001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("000000000000");
            }
            return id;
        }

        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinProduct" + where;
            return dao.IsExists(tsql);
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinProduct" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinProduct" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_FinProduct> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinProduct";
            string Fields = "[TypeId],[Id],[Name],[Spec],[Ucount],[Pkey],[Marketprice],[Costprice],[InitialCount],[InCount],[OutCount],[stock],"
                + "[Remake],[TypeName],[CusName],[SalerName]";
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
            return dao.ProExecSelect<View_FinProduct>("Proc_Page", sp);
        }

        public FinProduct GetRow(FinProduct model)
        {
            return dao.GetRow<FinProduct>(model);
        }

        public FinProduct GetRow(string Id)
        {
            string where = " and Id='" + Id + "'";
            DataTable dt = GetData(where);
            FinProduct model = new FinProduct();
            model.TypeId = dt.Rows[0]["TypeId"].ToStringEx();
            model.Id = dt.Rows[0]["Id"].ToStringEx();
            model.Name = dt.Rows[0]["Name"].ToStringEx();
            model.Spec = dt.Rows[0]["Spec"].ToStringEx();
            model.Ucount = Convert.ToInt32(dt.Rows[0]["Ucount"]);
            model.Pkey = dt.Rows[0]["Pkey"].ToStringEx();
            model.Marketprice = Convert.ToDecimal(dt.Rows[0]["Marketprice"]);
            model.Costprice = Convert.ToDecimal(dt.Rows[0]["Costprice"]);
            model.InitialCount = Convert.ToInt32(dt.Rows[0]["InitialCount"]);
            model.InCount = Convert.ToInt32(dt.Rows[0]["InCount"]);
            model.OutCount = Convert.ToInt32(dt.Rows[0]["OutCount"]);
            model.stock = Convert.ToInt32(dt.Rows[0]["stock"]);
            model.Remake = dt.Rows[0]["Remake"].ToStringEx();
            return model;
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
            return GetData(fieldsName, _where, "FinProduct");
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

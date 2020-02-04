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
    public class FinOrderInvoiceBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinOrderInvoiceBLL()
        { }

        public int Insert(FinOrderInvoice model)
        {
            return dao.Insert<FinOrderInvoice>(model);
        }
        public int Update(FinOrderInvoice model)
        {
            return dao.Update<FinOrderInvoice>(model);
        }
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from FinOrderInvoice where Id='" + id + "'");
        }
        public string Maxid(string D)
        {
            string id = "";
            String tsql = "select max(Id) from FinOrderInvoice where Id Like '" + D + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = D + "01";
            }
            else
            {
                id = D + (int.Parse(result.Substring(8)) + 1).ToString("00");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOrderInvoice" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOrderInvoice" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinOrderInvoice" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_FinOrderInvoice> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOrderInvoice";
            string Fields = "[OrderId],[Id],[AccountId],[Invoicedate],[Invoicemoney],[Receivablemoney],[Remark],[Name],[Key]";
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
            return dao.ProExecSelect<View_FinOrderInvoice>("Proc_Page", sp);
        }

        public List<View_FinOrderInvoice> SelectAllSum(string Where)
        {
            string Table = "View_FinOrderInvoice";
            string Fields = "'' as [OrderId],'合计' as [Id],sum(Invoicemoney) as [Invoicemoney],sum(Receivablemoney) as [Receivablemoney]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_FinOrderInvoice>("select " + Fields + " from " + Table + " " + Where);
        }

        public List<EasyUIModel> SelectAllComb(string Where)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<EasyUIModel>("select Id,[Key]+' 开('+cast(Invoicemoney as varchar(20))+'） 收('+cast(Receivablemoney as varchar(20))+'）' as [Key],AccountId,Receivablemoney,Invoicemoney from View_FinOrderInvoice " + Where);
        }

        public FinOrderInvoice GetRow(FinOrderInvoice model)
        {
            return dao.GetRow<FinOrderInvoice>(model);
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
            return GetData(fieldsName, _where, "FinOrderInvoice");
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

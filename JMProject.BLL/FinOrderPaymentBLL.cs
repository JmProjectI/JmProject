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
    public class FinOrderPaymentBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public FinOrderPaymentBLL()
        { }

        public int Insert(FinOrderPayment model)
        {
            return dao.Insert<FinOrderPayment>(model);
        }
        public int Update(FinOrderPayment model)
        {
            return dao.Update<FinOrderPayment>(model);
        }
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from FinOrderPayment where Id='" + id + "'");
        }
        public string Maxid(string D)
        {
            string id = "";
            String tsql = "select max(Id) from FinOrderPayment where Id Like '" + D + "%'";
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
            String tsql = "select count(*) from FinOrderPayment" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from FinOrderPayment" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from FinOrderPayment" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_FinOrderPayment> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_FinOrderPayment";
            string Fields = "[OrderId],[Id],[Paymentaccount],[Paymentdate],[Paymentmoney],[Remark],[Name],[Key]";
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
                Order = "Order by  ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_FinOrderPayment>("Proc_Page", sp);
        }

        public List<View_FinOrderPayment> SelectAllSum(string Where)
        {
            string Table = "View_FinOrderPayment";
            string Fields = "'' as [OrderId],'合计' as [Id],sum(Paymentmoney) as [Paymentmoney]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_FinOrderPayment>("select " + Fields + " from " + Table + " " + Where);
        }

        public FinOrderPayment GetRow(FinOrderPayment model)
        {
            return dao.GetRow<FinOrderPayment>(model);
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
            return GetData(fieldsName, _where, "FinOrderPayment");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public List<View_DesktopInvoice> Select_InvoiceAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_DesktopInvoice";
            string Fields = "[Id],[Paymentaccount],[Paymentdate],[Paymentmoney],[Remark],[Saler],[PaymentFlag],[Name],[Key],[ZsName],[HkFlag],[CustomName]";
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
                Order = "Order by  ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_DesktopInvoice>("Proc_Page", sp);
        }
    }
}
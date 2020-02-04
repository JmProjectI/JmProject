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
    public class SaleOrderItemBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SaleOrderItemBLL()
        { }

        public int Insert(SaleOrderItem model)
        {
            return dao.Insert<SaleOrderItem>(model);
        }
        public int Update(SaleOrderItem model)
        {
            return dao.Update<SaleOrderItem>(model);
        }
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from SaleOrderItem where ItemId='" + id + "'");
        }
        public string Maxid(string Id)
        {
            //string id = "";
            String tsql = "select max(ItemId) from SaleOrderItem where ItemId like '" + Id + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            //if (result == "")
            //{
            //    id = Id + "01";
            //}
            //else
            //{
            //    id = Id + (int.Parse(result.Substring(12)) + 1).ToString("00");
            //}
            return result;
        }
        
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="_where">不需要带where,直接and开头</param>
        /// <returns></returns>
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleOrderItem" + where;
            return dao.IsExists(tsql);
        }

        /// <summary>
        /// 查询行数
        /// </summary>
        /// <param name="_where">不需要带where,直接and开头</param>
        /// <returns></returns>
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleOrderItem" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from View_SaleOrderItem" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public String GetStrName(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SaleOrderItem" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<View_SaleOrderItem> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleOrderItem";
            string Fields = "[OrderId],[ItemId],[ProdectType],[TypeName],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[Service],[SerDateS],[SerDateE],[ServiceMonth],[OSCount],[CostMoney],[TcFlag],[TcName],[TcDate]";
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
                Order = "Order by ItemId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_SaleOrderItem>("Proc_Page", sp);
        }
        public SaleOrderItem GetRow(SaleOrderItem model)
        {
            return dao.GetRow<SaleOrderItem>(model);
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
            return GetData(fieldsName, _where, "SaleOrderItem");
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
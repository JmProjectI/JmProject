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
    public class BasicProvinceBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public BasicProvinceBLL()
        { }

        public BasicProvince GetRow(BasicProvince model)
        {
            return dao.GetRow<BasicProvince>(model);
        }

        public string GetStringName(string Name, string where)
        {
            return dao.GetScalar("select " + Name + " from BasicProvince " + where).ToStringEx();
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from BasicProvince" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public string MaxId()
        {
            string id = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(Pid) from BasicProvince";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = "0001";
            }
            else
            {
                id = (int.Parse(result) + 1).ToString("0000");
            }
            return id;
        }

        public int Insert(BasicProvince model)
        {
            return dao.Insert<BasicProvince>(model);
        }

        public int Update(BasicProvince model)
        {
            return dao.Update<BasicProvince>(model);
        }

        public int Delete(String Id)
        {
            //删除客户记录
            return dao.Delete("delete from BasicProvince where Pid='" + Id + "'");
        }

        public List<BasicProvince> SelectAll_Tree(string Where, string Order)
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
                Order = "Order by Pid ASC";
            }
            string tsql = "select * from BasicProvince " + Where + Order;
            List<BasicProvince> result = dao.Select<BasicProvince>(tsql);
            return result;
        }

        public List<BasicProvince> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "BasicProvince";
            string Fields = "[Pid],[Name],[beizhu]";
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
                Order = "Order by Pid ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<BasicProvince>("Proc_Page", sp);
        }
    }
}

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
    public class BasicCityBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public BasicCityBLL()
        { }

        public BasicCity GetRow(BasicCity model)
        {
            return dao.GetRow<BasicCity>(model);
        }

        public string MaxId()
        {
            string id = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(ID) from BasicCity";
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

        public int Insert(BasicCity model)
        {
            return dao.Insert<BasicCity>(model);
        }

        public int Update(BasicCity model)
        {
            return dao.Update<BasicCity>(model);
        }

        public int Delete(String Id)
        {
            //删除客户记录
            return dao.Delete("delete from BasicCity where ID='" + Id + "'");
        }

        public string GetStringName(string Name, string where)
        {
            return dao.GetScalar("select " + Name + " from BasicCity " + where).ToStringEx();
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from BasicCity" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_BasicCity> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_BasicCity";
            string Fields = "[Pid],[ID],[Name],[Code],[PostCode],[beizhu],[CityPLName],[Sfid],[SfName]";

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
            return dao.ProExecSelect<View_BasicCity>("Proc_Page", sp);
        }
    }
}

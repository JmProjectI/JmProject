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
    public class SysUserBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SysUserBLL()
        { }

        public int Insert(string tsql)
        {
            return dao.Insert(tsql);
        }
        public int Insert(SysUser model)
        {
            return dao.Insert<SysUser>(model);
        }
        public int Update(string sql)
        {
            return dao.Update(sql);
        }
        public int Update(SysUser model)
        {
            return dao.Update<SysUser>(model);
        }
        public int Delete(String id)
        {
            dao.Delete("delete from SysModuleUser where UserId='" + id + "'");
            dao.Delete("delete from SysModuleOperateUser where UserId='" + id + "'");
            return dao.Delete("delete from SysUser where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from SysUser";
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
            String tsql = "select count(*) from SysUser" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SysUser" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SysUser" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<SysUser> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SysUser";
            string Fields = "[RoleID],[Id],[Name],[Pwd],[Pic],[ZsName],[Phone],[Tel],[IcCard],[Birthday],[Address],[Remake]";
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
            return dao.ProExecSelect<SysUser>("Proc_Page", sp);
        }

        public SysUser GetRow(string logname, string logpass)
        {
            return dao.GetRow<SysUser>("select * from SysUser where Name='" + logname + "' and Pwd='" + logpass + "'");
        }
        public SysUser GetRow(SysUser model)
        {
            return dao.GetRow<SysUser>(model);
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
            return GetData(fieldsName, _where, "SysUser");
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

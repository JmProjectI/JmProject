using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Model;
using JMProject.Dal;
using JMProject.Common;
using System.Data;
using System.Data.SqlClient;
using JMProject.Model.Esayui;
using JMProject.Model.Sys;

namespace JMProject.BLL
{
    public class SysModuleBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SysModuleBLL()
        { }

        public int Insert(SysModule model)
        {
            return dao.Insert<SysModule>(model);
        }
        public int Update(SysModule model)
        {
            return dao.Update<SysModule>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from SysModule where Id='" + id + "'");
        }
        public string Maxid(string _parentId)
        {
            string id = "";
            string where = " where 1=1";
            if (string.IsNullOrEmpty(_parentId))
            {
                where += " and len(Id)=2";
            }
            else
            {
                where += " and _parentId='" + _parentId + "'";
            }
            String tsql = "select max(Id) from SysModule" + where;
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = _parentId + "01";
            }
            else
            {
                if (string.IsNullOrEmpty(_parentId))
                {
                    id = (int.Parse(result) + 1).ToString("00");
                }
                else
                {
                    id = _parentId + (int.Parse(result.Substring(2)) + 1).ToString("00");
                }
            }
            return id;
        }
        public int MaxSort(string _parentId)
        {
            int sort = 1;
            string where = " where 1=1";
            if (string.IsNullOrEmpty(_parentId))
            {
                where += " and len(Id)=2";
            }
            else
            {
                where += " and _parentId='" + _parentId + "'";
            }
            String tsql = "select max(sort) from SysModule" + where;
            string result = dao.GetScalar(tsql).ToStringEx();
            if (!string.IsNullOrEmpty(result))
            {
                sort = int.Parse(result) + 1;
            }
            return sort;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SysModule" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SysModule" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SysModule" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<SysModule> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SysModule";
            string Fields = "[Id],[Name],[Icon],[Url],[Sort],[_parentId]";
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
            return dao.ProExecSelect<SysModule>("Proc_Page", sp);
        }

        public List<SysModuleRole> SelectAllRole(string Rid, string RType)
        {
            string Order = " Order by Id ASC,Sort ASC";
            string Where = " Where 1=1 ";
            string Table = "SysModule";

            string RTable = "";
            string RTableID = "";
            if (RType == "R")
            {
                RTable = "SysModuleRole";
                RTableID = "RoleId";
            }
            else
            {
                RTable = "SysModuleUser";
                RTableID = "UserId";
            }
            string Fields = "[Id],[Name],[Icon],[Url],[Sort],[_parentId],(select COUNT(*) from " + RTable + " where ModuleId=SysModule.Id and " + RTableID + "='" + Rid + "') HavRole";

            return dao.Select<SysModuleRole>("select " + Fields +" from "+ Table + Where + Order);
        }

        public SysModule GetRow(SysModule model)
        {
            return dao.GetRow<SysModule>(model);
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
            return GetData(fieldsName, _where, "SysModule");
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


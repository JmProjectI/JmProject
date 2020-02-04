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
using JMProject.Model.Sys;

namespace JMProject.BLL
{
    public class SysModuleOperateBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SysModuleOperateBLL()
        { }

        public int Insert(SysModuleOperate model)
        {
            return dao.Insert<SysModuleOperate>(model);
        }
        public int Update(SysModuleOperate model)
        {
            return dao.Update<SysModuleOperate>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from SysModuleOperate where Id='" + id + "'");
        }
        public string Maxid(string ModuleId)
        {
            string id = "";
            String tsql = "select max(Id) from SysModuleOperate where ModuleId='" + ModuleId + "'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = ModuleId + "001";
            }
            else
            {
                id = ModuleId + (int.Parse(result.Substring(ModuleId.Length)) + 1).ToString("000");
            }
            return id;
        }
        public int MaxSort(string ModuleId)
        {
            int sort = 1;
            String tsql = "select max(Sort) from SysModuleOperate where ModuleId='" + ModuleId + "'";
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
            String tsql = "select count(*) from SysModuleOperate" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SysModuleOperate" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SysModuleOperate" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<SysModuleOperate> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SysModuleOperate";
            string Fields = "[Id],[ModuleId],[Name],[KeyCode],[IsValid],[Sort]";
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
            return dao.ProExecSelect<SysModuleOperate>("Proc_Page", sp);
        }
        public List<SysModuleOperateRole> SelectAllRole(string ModuleId, string Rid, string RType)
        {
            string Where = " Where ModuleId = '" + ModuleId + "'";
            string Order = " Order by Sort ASC";

            string RTable = "";
            string RTableID = "";
            if (RType == "R")
            {
                RTable = "SysModuleOperateRole";
                RTableID = "RoleId";
            }
            else
            {
                RTable = "SysModuleOperateUser";
                RTableID = "UserId";
            }

            string tsql = "select [Id],[ModuleId],[Name],[KeyCode],[IsValid],[Sort]"
                        + ",(select COUNT(*) from " + RTable + " where ModuleOpId=SysModuleOperate.Id and " + RTableID + "='" + Rid + "') HavRole"
                        + " from SysModuleOperate " + Where + Order;
            return dao.Select<SysModuleOperateRole>(tsql);
        }
        public SysModuleOperate GetRow(SysModuleOperate model)
        {
            return dao.GetRow<SysModuleOperate>(model);
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
            return GetData(fieldsName, _where, "SysModuleOperate");
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
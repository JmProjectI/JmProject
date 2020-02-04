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
    public class ModuleRoleBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public bool isExist(String _where,string Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from " + Table + where;
            return dao.IsExists(tsql);
        }

        public List<permModel> SelectAll(string Where)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            string Order = "Order by Sort ASC";
            string tsql = "select KeyCode,IsValid from View_SysModuleOperate_User " + Where + Order;
            List<permModel> result = dao.Select<permModel>(tsql);
            return result;
        }

        #region 角色+模块
        public int InsertRole(string RoleId, string ModuleId)
        {
            return dao.Insert("insert into SysModuleRole values ('" + RoleId + "','" + ModuleId + "')");
        }
        public int DeleteRole(string RoleId, string ModuleId)
        {
            return dao.Delete("delete from SysModuleRole where RoleId='" + RoleId + "' and ModuleId='" + ModuleId + "'");
        }
        public bool isExistRole(string RoleId, string ModuleId)
        {
            String where = " where RoleId='" + RoleId + "' and ModuleId='" + ModuleId + "' ";
            String tsql = "select count(*) from SysModuleRole" + where;
            return dao.IsExists(tsql);
        }
        #endregion

        #region 用户+模块
        public int InsertUser(string UserId, string ModuleId)
        {
            return dao.Insert("insert into SysModuleUser values ('" + UserId + "','" + ModuleId + "')");
        }
        public int DeleteUser(string UserId, string ModuleId)
        {
            return dao.Delete("delete from SysModuleUser where UserId='" + UserId + "' and ModuleId='" + ModuleId + "'");
        }
        public bool isExistUser(string UserId, string ModuleId)
        {
            String where = " where UserId='" + UserId + "' and ModuleId='" + ModuleId + "' ";
            String tsql = "select count(*) from SysModuleUser" + where;
            return dao.IsExists(tsql);
        }
        #endregion

        #region 角色+操作码
        public int InsertOptRole(string RoleId, string ModuleOpId)
        {
            return dao.Insert("insert into SysModuleOperateRole values ('" + RoleId + "','" + ModuleOpId + "')");
        }
        public int DeleteOptRole(string RoleId, string ModuleOpId)
        {
            return dao.Delete("delete from SysModuleOperateRole where RoleId='" + RoleId + "' and ModuleOpId='" + ModuleOpId + "'");
        }
        public bool isExistOptRole(string RoleId, string ModuleOpId)
        {
            String where = " where RoleId='" + RoleId + "' and ModuleOpId='" + ModuleOpId + "' ";
            String tsql = "select count(*) from SysModuleOperateRole" + where;
            return dao.IsExists(tsql);
        }
        #endregion

        #region 用户+操作码
        public int InsertOptUser(string UserId, string ModuleOpId)
        {
            return dao.Insert("insert into SysModuleOperateUser values ('" + UserId + "','" + ModuleOpId + "')");
        }
        public int DeleteOptUser(string UserId, string ModuleOpId)
        {
            return dao.Delete("delete from SysModuleOperateUser where UserId='" + UserId + "' and ModuleOpId='" + ModuleOpId + "'");
        }
        public bool isExistOptUser(string UserId, string ModuleOpId)
        {
            String where = " where UserId='" + UserId + "' and ModuleOpId='" + ModuleOpId + "' ";
            String tsql = "select count(*) from SysModuleOperateUser" + where;
            return dao.IsExists(tsql);
        }
        #endregion
    }
}

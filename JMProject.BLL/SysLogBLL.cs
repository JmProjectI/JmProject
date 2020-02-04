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
    public class SysLogBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SysLogBLL()
        { }

        //000037 用户日志
        public int AddLogUser(string Operator, string Message, string Type, string Module)
        {
            SysLog model = new SysLog();
            model.Id = Guid.NewGuid();
            model.Operator = Operator;
            model.Message = Message;
            model.Type = Type;
            model.Module = Module;
            model.CreateTime = DateTime.Now;
            model.LogType = "000037";
            return dao.Insert(model);
        }
        //000038 异常日志
        public int AddLogExp(string Operator, string Message, string Type, string Module)
        {
            SysLog model = new SysLog();
            model.Id = Guid.NewGuid();
            model.Operator = Operator;
            model.Message = Message;
            model.Type = Type;
            model.Module = Module;
            model.CreateTime = DateTime.Now;
            model.LogType = "000038";
            return dao.Insert(model);
        }
        public int Insert(SysLog model)
        {
            return dao.Insert<SysLog>(model);
        }
        public int Update(SysLog model)
        {
            return dao.Update<SysLog>(model);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from SysLog where Id='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max(Id) from SysLog";
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
            String tsql = "select count(*) from SysLog" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SysLog" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SysLog" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<SysLog> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SysLog";
            string Fields = "[Id],[Operator],[Message],[Type],[Module],[CreateTime],[LogType]";
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
            return dao.ProExecSelect<SysLog>("Proc_Page", sp);
        }
        public SysLog GetRow(string Id)
        {
            DataTable dt = dao.Select("select [Id],[Operator],[Message],[Type],[Module],[CreateTime],[LogType] from SysLog where Id='" + Id + "'");
            SysLog model = new SysLog();
            model.Id = Guid.Parse(dt.Rows[0]["Id"].ToStringEx());
            model.Operator = dt.Rows[0]["Operator"].ToStringEx();
            model.Message = dt.Rows[0]["Message"].ToStringEx();
            model.Type = dt.Rows[0]["Type"].ToStringEx();
            model.Module = dt.Rows[0]["Module"].ToStringEx();
            model.CreateTime = Convert.ToDateTime(dt.Rows[0]["CreateTime"]);
            model.LogType = dt.Rows[0]["LogType"].ToStringEx();
            return model;
        }
        public SysLog GetRow(SysLog model)
        {
            return dao.GetRow<SysLog>(model);
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
            return GetData(fieldsName, _where, "SysLog");
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


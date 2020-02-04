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
    public class ManagementBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public ManagementBLL()
        { }

        public List<Management> SelectAll()
        {
            string Table = "select * from Management";

            return dao.Select<Management>(Table);
        }

        public string GetNameStr(string zid, string _where)
        {
            string where = " where 1=1 " + _where;
            string tsql = "select " + zid + " from Management " + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public int Insert(string tsql)
        {
            return dao.Insert(tsql);
        }

        public int Delete(string id)
        {
            return dao.Delete("delete from Management where id='" + id + "'");
        }
    }
}

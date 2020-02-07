﻿using System;
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
    public class JsDataBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public JsDataBLL()
        { }

        public int Insert(JsData model)
        {
            return dao.Insert<JsData>(model);
        }
        public int Update(JsData model)
        {
            return dao.Update<JsData>(model);
        }

        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }
        public int Delete(String id)
        {
            return dao.Delete("delete from JsData where ='" + id + "'");
        }
        public string Maxid()
        {
            string id = "";
            String tsql = "select max() from JsData";
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
        public string MaxDateid(string D)
        {
            string id = "";
            String tsql = "select max() from JsData where ID Like '" + D + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = D + "0001";
            }
            else
            {
                id = D + (int.Parse(result.Substring(8)) + 1).ToString("0000");
            }
            return id;
        }
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from JsData" + where;
            return dao.IsExists(tsql);
        }
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from JsData" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from JsData" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<JsData> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "JsData";
            string Fields = "[ID],[Name],[Code],[Lxr],[JsYear],[JsKey],[Vlevel],[RegKey]";
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
            return dao.ProExecSelect<JsData>("Proc_Page", sp);
        }
        public JsData GetRow(string tsql)
        {
            JsData result = null;
            DataTable dt = dao.Select(tsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                result = new JsData();
                result.ID = dr["ID"].ToString();
                result.Name = dr["Name"].ToString();
                result.Code = dr["Code"].ToString();
                result.Lxr = dr["Lxr"].ToString();
                result.JsYear = dr["JsYear"].ToString();
                result.JsKey = dr["JsKey"].ToString();
                result.Vlevel = dr["Vlevel"].ToString();
                result.RegKey = dr["RegKey"].ToString();
            }
            return result;
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
            return GetData(fieldsName, _where, "JsData");
        }
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }
        public bool Tran(Dictionary<string, object> tsqls)
        {
            return dao.Transaction(tsqls);
        }
    }
}
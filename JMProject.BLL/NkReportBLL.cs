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
using JMProject.Model.Sys;

namespace JMProject.BLL
{
    public class NkReportBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public NkReportBLL()
        { }


        #region 隶属关系 NkReport_MJLSGX
        public List<NkReport_MJLSGX> Select_NkReport_MJLSGX(string Where, string Order, string TableName)
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
                Order = "Order by Id ASC";
            }
            string tsql = "select Id,Id +' | '+ Name Name,_parentId from " + TableName + " " + Where + Order;
            List<NkReport_MJLSGX> result = dao.Select<NkReport_MJLSGX>(tsql);
            return result;
        }
        #endregion

        #region 归属部门 NkReport_MJBMBS
        public List<NkReport_MJBMBS> Select_NkReport_MJBMBS(string Where, string Order, string TableName)
        {
            //归属部门 NkReport_MJBMBS
            //单位预算管理级次 NkReport_MJDWYSJC
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
                Order = "Order by Id ASC";
            }
            string tsql = "select Id,Id +' | '+ Name Name from " + TableName + " " + Where + Order;
            List<NkReport_MJBMBS> result = dao.Select<NkReport_MJBMBS>(tsql);
            return result;
        }
        #endregion

        #region 内控报告列表
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from NkReport" + where;
            return dao.IsExists(tsql);
        }

        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from NkReport" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from NkReport" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_NkReport> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_NkReport";
            string Fields = "[OrderId],[CustomId],[Id],[Years],[Flag],[Name],[Invoice],[MxId],[Tjrq],[Tsyqtext],[Shrq],[Shr],"
                + "[Zzrq],[Zzr],[Yjrq],[Yjr],[Fsrq],[Fsr],[Lsr],[ShrName],[ZzrName],[YjrName],[FsrName]";
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
                Order = "Order by OrderId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_NkReport>("Proc_Page", sp);
        }

        public List<View_SysNkReport> ReportSelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SysNkReport";
            string Fields = "[Id],[Zid],[date],[flag],[czr],[FlagName],[CzrName]";
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
            return dao.ProExecSelect<View_SysNkReport>("Proc_Page", sp);
        }

        public string MaxId()
        {
            string id = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(Id) from NkReport_Progress";
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

        public int Insert(NkReport model)
        {
            return dao.Insert<NkReport>(model);
        }
        #endregion


        public int Insert(NkReport_Progress model)
        {
            return dao.Insert<NkReport_Progress>(model);
        }
    }
}

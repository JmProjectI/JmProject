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
    public class WeiXinApiBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public WeiXinApiBLL()
        { }

        public string MaxId(string date)
        {
            string id = "";
            //string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(ID) from SaleWeiXin where ID like '" + date + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = date + "000001";
            }
            else
            {
                id = date + (int.Parse(result.Substring(8)) + 1).ToString("000000");
            }
            return id;
        }

        /// <summary>
        /// 增加方法
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Insert(SaleWeiXin model)
        {
            return dao.Insert<SaleWeiXin>(model);
        }

        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public int Update(SaleWeiXin model)
        {
            return dao.Update(model);
        }

        /// <summary>
        /// 修改方法
        /// </summary>
        /// <param name="tsql">sql语句</param>
        /// <returns></returns>
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public int Delete(String id)
        {
            return dao.Delete("delete from SaleWeiXin where ID='" + id + "'");
        }

        /// <summary>
        /// 获取表的某个指定值
        /// </summary>
        /// <param name="Name">字段名</param>
        /// <param name="where">查询条件(需要where开头)</param>
        /// <returns></returns>
        public string GetStrName(string Name, string Table, string where)
        {
            String _where = " where 1=1 " + where;
            return dao.GetScalar("select " + Name + " from " + Table + " " + _where).ToStringEx();
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <returns></returns>
        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleWeiXin" + where;
            return dao.IsExists(tsql);
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <returns></returns>
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleWeiXin" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        /// <summary>
        /// 获取某个字段值
        /// </summary>
        /// <param name="fieldName">要查询的字段名</param>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <returns></returns>
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SaleWeiXin" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }
        public List<SaleWeiXin> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SaleWeiXin";
            string Fields = "[ID],[Zid],[OpenId]";
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
            return dao.ProExecSelect<SaleWeiXin>("Proc_Page", sp);
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public SaleWeiXin GetRow(SaleWeiXin model)
        {
            return dao.GetRow<SaleWeiXin>(model);
        }

        /// <summary>
        /// 获取数据行数
        /// </summary>
        /// <param name="Id">编号</param>
        /// <returns></returns>
        public SaleWeiXin GetRow(string Id)
        {
            return dao.GetRow<SaleWeiXin>("select * from SaleWeiXin where ID='" + Id + "'");
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>返回DataTable</returns>
        public DataTable GetData()
        {
            return GetData("");
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetData(String _where)
        {
            return GetData("*", _where);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fieldsName">要查询字段名</param>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetData(String fieldsName, String _where)
        {
            return GetData(fieldsName, _where, "SaleWeiXin");
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fieldsName">要查询字段名</param>
        /// <param name="_where">查询条件(不需要带where,and开头)</param>
        /// <param name="Table">要查询的表名</param>
        /// <returns></returns>
        public DataTable GetData(String fieldsName, String _where, String Table)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldsName + " from " + Table + where;
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        /// <summary>
        /// 执行操作带回滚
        /// </summary>
        /// <param name="tsqls">sql语句</param>
        /// <returns></returns>
        public bool Tran(Dictionary<string, object> tsqls)
        {
            return dao.Transaction(tsqls);
        }


        /****************************************************/
        /// <summary>
        /// 插入方法
        /// </summary>
        /// <param name="tsql">要执行的插入语句</param>
        /// <returns></returns>
        public int Insert(string tsql)
        {
            return dao.Insert(tsql);
        }

        /// <summary>
        /// 查询某个指定值
        /// </summary>
        /// <param name="Zid">字段名</param>
        /// <param name="where">条件(不需要带where,直接and开头)</param>
        /// <returns></returns>
        public string WxGetStrName(string Zid, string where)
        {
            string Where = " where 1=1 " + where;
            string tsql = "select " + Zid + " from OpenIdAndYwyId " + Where;
            return dao.GetScalar(tsql).ToStringEx();
        }

        ///// <summary>
        ///// 查询某个指定值
        ///// </summary>
        ///// <param name="where">条件(不需要带where,直接and开头)</param>
        ///// <returns></returns>
        //public int WxGetCount(string where)
        //{
        //    string Where = " where 1=1 ";
        //    return Convert.ToInt32(dao.GetScalar("select Count(*) from OpenIdAndYwyId " + Where).ToStringEx());
        //}

        /// <summary>
        /// 查询某个指定值
        /// </summary>
        /// <param name="where">条件(不需要带where,直接and开头)</param>
        /// <returns></returns>
        public int WxGetCount(string tsql)
        {
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        /*******************************************View_WeiXinYwyOrderXx**/

        public List<WeiXinYwyQueRen> Select_YwyQueRen(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_WeiXinYwyOrderXx";
            string Fields = "CusOpenId,OrderId,CusName,Years,IsCus";
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
                Order = "Order by OrderId desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<WeiXinYwyQueRen>("Proc_Page", sp);
        }
    }
}

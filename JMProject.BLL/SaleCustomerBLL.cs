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
using JMProject.Model.Mobile;

namespace JMProject.BLL
{
    public class SaleCustomerBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public SaleCustomerBLL()
        { }

        public string MaxId(string date)
        {
            string id = "";
            //string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(ID) from SaleCustom where ID like '" + date + "%'";
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

        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleCustom" + where;
            return dao.IsExists(tsql);
        }

        /// <summary>
        /// 获取表的某个指定值
        /// </summary>
        /// <param name="Name">字段名</param>
        /// <param name="_where">查询条件(不需要带where,直接and开头)</param>
        /// <returns></returns>
        public string GetNameStr(String Name, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + Name + " from View_SaleCustom" + where;
            return dao.GetScalar(tsql).ToStringEx();
        }

        /// <summary>
        /// 获取表的某个指定值
        /// </summary>
        /// <param name="Name">字段名</param>
        /// <param name="where">查询条件(需要where开头)</param>
        /// <returns></returns>
        public string GetStrName(string Name, string where)
        {
            return dao.GetScalar("select " + Name + " from SaleCustom " + where).ToStringEx();
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

        public decimal GetMoney(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select sum(ItemMoney) ItemMoney from SaleOrder T inner join SaleOrderItem T1 on T.Id=T1.OrderId" + where;
            string money = dao.GetScalar(tsql).ToStringEx();
            if (string.IsNullOrEmpty(money))
            {
                return 0.00M;
            }
            else
            {
                return decimal.Parse(money);
            }
        }

        public int GetCount(string where)
        {
            return Convert.ToInt32(dao.GetScalar("select count(*) from SaleCustom " + where).ToStringEx());
        }

        public string Insert(SaleCustom model)
        {
            string CusID = "";
            if (dao.Insert<SaleCustom>(model) > 0)
            {
                CusID = model.ID;
            }
            return CusID;
            //return dao.Insert<SaleCustom>(model);
        }

        public int Inserts(SaleCustom model)
        {
            //string CusID = "";
            //if (dao.Insert<SaleCustom>(model) > 0)
            //{
            //    CusID = model.ID;
            //}
            //return CusID;
            return dao.Insert<SaleCustom>(model);
        }

        public int Update(SaleCustom model)
        {
            return dao.Update<SaleCustom>(model);
        }
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        public int Delete(String Id)
        {
            //删除客户记录
            return dao.Delete("delete from SaleCustom where ID='" + Id + "'");
        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <returns></returns>
        public bool Verification(string Uname, string Id)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where = " and ID !='" + Id + "'";
            }
            object obj = dao.GetScalar("select count(*) from SaleCustom where UserName='" + Uname + "' " + where);
            if (Convert.ToInt32(obj) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SaleCustom> SelectAllComb(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SaleCustom";
            string Fields = "[ID],[Name]";
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
            return dao.ProExecSelect<SaleCustom>("Proc_Page", sp);
        }

        public List<S_SCustom> SelectAll_Phone(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleCustom_Phone";
            string Fields = "[ID],[Name],[Lxr],[Phone],[Address],ItemMoney,Invoicemoney,Paymentmoney";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = " Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = " Order by " + pager.sort + " " + pager.order;
            }
            else
            {
                Order = " Order by Id DESC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<S_SCustom>("Proc_Page", sp);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="Where">查询条件(不需要where,and开头)</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<View_SaleCustom> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleCustom";
            string Fields = "[ID],[CDate],[Ywy],[Name],[BM],[Lxr],[Zw],[Phone],[Industry],[UpID],[Province],[Xydj],[Gx],[Zyx],[Tel],[QQ]"
                + ",[Email],[Address],[LxrSR],[QtLxr],[QtTel],[Bank],[CardNum],[SuiH],[Desc],[Remark],[Flag],[Uid],[Source],[Region]"
                + ",[CustomerType],[CustomerGrade],[Code],[Invoice],[UserName],[UserPwd],[Finance],[YwyName],[BmName],[ZwName],[HyName]"
                + ",[UpName],[SfName],[XydjName],[GxName],[ZyxName],[CjrName],[LyName],[CityName],[TypeName],[DjName],[CzjName]";
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
                Order = "Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_SaleCustom>("Proc_Page", sp);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="Where">查询条件(不需要where,and开头)</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<WeiXinSaleCustom> Return_SelectYwyAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "SaleCustom";
            string Fields = "[ID],[Name]";
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
                Order = "Order by ID desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<WeiXinSaleCustom>("Proc_Page", sp);
        }
        
        public List<WeiXinYwyReturnCus> WeiXinYwyReturnCus(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_YwyReturnCus";
            string Fields = "CusName,Lxr,Phone,ItemNames,ItemMoney";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = "Order by " + pager.sort + " " + pager.order;
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<WeiXinYwyReturnCus>("Proc_Page", sp);
        }

        /// <summary>
        /// 微信获取客户信息
        /// </summary>
        /// <param name="Where">查询条件(不需要where,and开头)</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<WeiXinInformation> WeiXinSelectAll(string Where)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = " Where 1=1 " + Where;
            }
            string Table = "View_SaleCustom T";
            string Fields = "Name as CusName,[Invoice],[Code],[Lxr],[Phone],[Address],(select COUNT(*) from Nksc where CustomerID=T.ID) as IsNksc";
            return dao.Select<WeiXinInformation>("select " + Fields + " from " + Table + " " + Where);
        }

        /// <summary>
        /// 微信客户确认信息后返回的信息（网址、用户名、密码、温馨提示）
        /// </summary>
        /// <param name="Where">查询条件(不需要where,and开头)</param>
        /// <returns></returns>
        public List<WeiXinWeb> WeiXinSelectWeb(string OrderId)
        {
            string tsql = "select T1.UserName as Users,T1.UserPwd as Pwd,T2.beizhu as Remark from SaleOrder T "
                + "join SaleCustom T1 on T1.ID=T.SaleCustomId join BasicCity T2 on T2.ID=T1.Region where T.Id='" + OrderId + "'";

            return dao.Select<WeiXinWeb>(tsql);
        }

        /// <summary>
        /// 微信个人中心查看网址、用户名、密码
        /// </summary>
        /// <param name="Where">查询条件(不需要where,and开头)</param>
        /// <returns></returns>
        public List<WeiXinWeb> WeiXinGeRenWeb(string CusId)
        {
            string tsql = "select UserName as Users,UserPwd as Pwd from SaleCustom where ID='" + CusId + "'";

            return dao.Select<WeiXinWeb>(tsql);
        }

        public SaleCustom GetRow(string logname, string logpass)
        {
            return dao.GetRow<SaleCustom>("select * from SaleCustom where UserName='" + logname + "' and UserPwd='" + logpass + "'");
        }

        public S_YJDZ GetCustomRow(string ID)
        {
            return dao.GetRow<S_YJDZ>("select ID,Code,Name,QtLxr,QtTel,Address,Invoice,QQ from SaleCustom where ID='" + ID + "'");
        }

        public SaleCustom GetRow(SaleCustom model)
        {
            return dao.GetRow<SaleCustom>(model);
        }

        public View_SaleCustom GetRow(string Where)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.GetRow<View_SaleCustom>("select * from View_SaleCustom " + Where);
        }

        public View_SaleCustom_Mobile GetRowMobile(string Where)
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.GetRow<View_SaleCustom_Mobile>("select * from View_SaleCustom_Mobile " + Where);
        }

        public List<ServiceTime> SelectService(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_ViewSerivce";
            string Fields = "[TypeName],[SerDateS],[SerDateE],[ServiceMonth]";
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
                Order = "Order by SaleCustomId ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<ServiceTime>("Proc_Page", sp);
        }

        public DataTable SelectCustom(string orderid)
        {
            string sql = "select T1.Address,T1.Lxr,T1.Phone,T1.SfName,T1.UpName,T1.CityName from SaleOrder T0 inner join View_SaleCustom T1 on T0.SaleCustomId=T1.ID where T0.Id='" + orderid + "'";
            return dao.Select(sql);
        }
    }
}

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
    public class SaleOrderBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public SaleOrderBLL()
        { }

        public int Insert(SaleOrder model)
        {
            return dao.Insert<SaleOrder>(model);
        }
        public int Update(SaleOrder model)
        {
            return dao.Update<SaleOrder>(model);
        }
        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }
        public Dictionary<string, object> GetDeleteSql(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            //List<SaleOrderItem> items = dao.Select<SaleOrderItem>("select * from SaleOrderItem where OrderId='" + id + "'");
            //foreach (var item in items)
            //{
            //    string sqlProduct = "update FinProduct set OutCount=OutCount-" + item.ItemCount + ",stock=stock+" + item.ItemCount + " where Id='" + item.ProductId + "'";
            //    tsqls.Add(sqlProduct, null);
            //}
            tsqls.Add("delete from SaleOrderItem where OrderId='" + id + "'", null);
            return tsqls;
        }

        public int GetOIDeleteSql(String _where)
        {
            string where = " where 1=1 " + _where;
            string sql = "delete from SaleOrderItem " + where;
            return dao.Delete(sql);
        }

        public bool Delete(String id)
        {
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            tsqls.Add("delete from Nksc where SaleOrderID='" + id + "'", null);
            tsqls.Add("delete from SaleOrderItem where OrderId='" + id + "'", null);
            tsqls.Add("delete from SaleOrder where Id='" + id + "'", null);
            return dao.Transaction(tsqls);
        }

        public string Maxid(string D)
        {
            string id = "";
            String tsql = "select max(Id) from SaleOrder where Id Like '" + D + "%'";
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
            String tsql = "select count(*) from SaleOrder" + where;
            return dao.IsExists(tsql);
        }

        /// <summary>
        /// 查询行数
        /// </summary>
        /// <param name="_where">不需要带whre,直接and开头</param>
        /// <returns></returns>
        public int GetCount(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from SaleOrder" + where;
            return Convert.ToInt32(dao.GetScalar(tsql));
        }

        /// <summary>
        /// 查询指定某个值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="_where">条件(不需要带whre,直接and开头)</param>
        /// <returns></returns>
        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from View_SaleOrder" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        /// <summary>
        /// 查询指定某个值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="_where">条件(不需要带whre,直接and开头)</param>
        /// <returns></returns>
        public String GetStrName(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from SaleOrder" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<View_SaleOrder> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_SaleOrder";
            string Fields = "[Id],[OrderDate],[Saler],[Finshed],[Remake],[Enclosure],[Key]"
                + ",[CheckFlag],[CheckDate],[SalerName],[InvoiceFlagName],[PaymentFlagName],[OutStockFlagName]"
                + ",[CheckFlagName],[OrderTypeName],[Name],[Region],[CityName],[Lxr],[Phone],[Address],[Invoice]"
                + ",[Code],[Zyx],[CustomerType],[ZsName],[ItemCount]"//,[ZyxName],[TypeName],[Industry],[HyName]
                + ",[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Invoicemoney],[Receivablemoney]"
                + ",[Paymentmoney],[OSCount],[flagDown],[ItemNames],[unTc],[Tc],[nkscflag],[nid],[Fp],[SuiH]"
                + ",[BankCard],[Flag]";
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
            return dao.ProExecSelect<View_SaleOrder>("Proc_Page", sp);
        }

        public List<WeiXinYwyOrderXx> WeiXinYwyOrderXx(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_WeiXinYwyOrderXx";
            string Fields = "Name,FlagN,InvoiceFlagName,ItemNames,ItemMoney,PostFlagName";
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
            return dao.ProExecSelect<WeiXinYwyOrderXx>("Proc_Page", sp);
        }

        public List<WeiXinCusFinishOrderXx> WeiXinCusOrderXx(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_WeiXinCusOrderXx";
            string Fields = "TypeName,Years,NkscFlag+ReportFlag Flag";
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
            return dao.ProExecSelect<WeiXinCusFinishOrderXx>("Proc_Page", sp);
        }

        public List<WeiXinQueRenXinX> Cus_OrderXinXi(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_CusOrderXinXi";
            string Fields = "OrderId,Years,CusName,Lxr,Phone,Address,Flag";
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
            return dao.ProExecSelect<WeiXinQueRenXinX>("Proc_Page", sp);
        }

        public DataTable SelectDaoC(string where)
        {
            string sql = "select ''''+Id as 编号,OrderDate as 日期,CityName as 地区,Name as 单位全称,invoice 发票抬头,''''+code 社会统一新用代码,HyName 行业,Lxr as 联系人,Phone as 手机号"
                + ",QtLxr as 其他联系人,QtTel as 其他联系电话,ItemNames as 合同明细,DjName as 客户等级"
                + ",InvoiceFlagName as 发票状态,PaymentFlagName as 回款状态,OutStockFlagName as 出库状态,(case "
                + "when Tc>0 and unTc>0 then '部分提成' "
                + "when Tc>0 and unTc=0 then '已提成' "
                + "when Tc=0 and unTc>0 then '未提成' "
                + "else '' end) as 提成状态,"
                + "ItemMoney as 合计金额,Invoicemoney as 开票金额, Paymentmoney as 回款金额 from View_SaleOrder where 1=1 " + where
                + " order by OrderDate desc";
            return dao.Select(sql);
        }

        public DataTable SelectDaoChu(string where)
        {
            string sql = "select ''''+Id as 合同编号,Name as 客户名称,SalerName as 经手人,'' as 出库日期,'' as 序列号 from View_SaleOrder where 1=1 "
                + where + " order by Id";
            return dao.Select(sql);
        }

        /// <summary>
        /// 导出销售单
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable SelectDC_xsd(string where)
        {
            string sql = "select OrderDate as 日期,[Key] as 账户,Invoice as 名称,''''+Code as 社会统一信用代码,ItemMoney 合同金额,"
                + "ItemNames as 合同明细,Remake 描述 from View_SaleOrder where 1=1 "
                + where + " order by Id";
            return dao.Select(sql);
        }

        /// <summary>
        /// 待开销售单导出
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable SelectDkDC_xsd(string where)
        {
            string sql = "select Fp as 推送日期,[Key] as 账户,Invoice as 名称,''''+Code as 社会统一信用代码,ItemMoney 合同金额,"
                + "SuiH as '地址、电话',BankCard as 开户行及账户,ItemNames as 合同明细,Remake 描述 from View_SaleOrder where 1=1 "
                + where + " order by Id";
            return dao.Select(sql);
        }

        public List<View_SaleOrder> SelectAllSum(string Where)
        {
            string Table = "View_SaleOrder";
            string Fields = "'合计' as [Name],sum(ItemCount) as [ItemCount],sum(ItemMoney) as [ItemMoney],sum(TaxMoney) as [TaxMoney],sum(PresentMoney) as [PresentMoney],sum(OtherMoney) as [OtherMoney]"
                + ",sum(ValidMoney) as [ValidMoney],sum(Invoicemoney) as [Invoicemoney],sum(Receivablemoney) as [Receivablemoney],sum(Paymentmoney) as [Paymentmoney],sum(OSCount) as [OSCount]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            return dao.Select<View_SaleOrder>("select " + Fields + " from " + Table + " " + Where);
        }
        public View_SaleOrder GetRow(string Id)
        {
            return dao.GetRow<View_SaleOrder>("select * from View_SaleOrder where Id='" + Id + "'");
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
            return GetData(fieldsName, _where, "SaleOrder");
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

        public List<S_CustomReprot> SelectCustomReprot(string Where, string Whereitem, GridPager pager)
        {
            string Order = string.Empty;

            //if (!string.IsNullOrEmpty(Where))
            //{
            Where = "Where 1=1 " + Where;
            //}

            //SaleCustomId Name ItemMoney

            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = "Order by T." + pager.sort + " " + pager.order;
            }
            else
            {
                Order = "Order by T.SaleCustomId ASC";
            }

            StringBuilder sbc = new StringBuilder();
            sbc.Append(" SELECT count(*) FROM ");
            sbc.Append("(");
            sbc.Append(" select ROW_NUMBER() OVER (" + Order + ")AS Row,* from (");
            sbc.Append(" select SaleCustomId,Name,sum(ItemMoney) ItemMoney,sum(Invoicemoney) Invoicemoney,sum(Paymentmoney) Paymentmoney");
            sbc.Append(" ,[ItemNames]=stuff((select ''+[ItemNames] from View_CustomReprot t " + Where + " and SaleCustomId=tb.SaleCustomId and Name=tb.Name for xml path('')), 1, 1, '') ");
            sbc.Append(" from View_CustomReprot tb");
            sbc.Append(" " + Where);
            sbc.Append(" group by SaleCustomId,Name");
            sbc.Append(" ) T");
            sbc.Append("  WHERE 1=1 " + Whereitem);
            sbc.Append("  ) ");
            sbc.Append("TT");

            pager.totalRows = Convert.ToInt32(dao.GetScalar(sbc.ToString()));

            int start = (pager.page - 1) * pager.rows + 1;
            int end = pager.page * pager.rows;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * FROM ");
            sb.Append("(");
            sb.Append(" select ROW_NUMBER() OVER (" + Order + ")AS Row,* from (");
            sb.Append(" select SaleCustomId,Name,sum(ItemMoney) ItemMoney,sum(Invoicemoney) Invoicemoney,sum(Paymentmoney) Paymentmoney");
            sb.Append(" ,[ItemNames]=stuff((select ''+[ItemNames] from View_CustomReprot t " + Where + " and SaleCustomId=tb.SaleCustomId and Name=tb.Name for xml path('')), 1, 1, '') ");
            sb.Append(" from View_CustomReprot tb");
            sb.Append(" " + Where);
            sb.Append(" group by SaleCustomId,Name");
            sb.Append(" ) T");
            sb.Append("  WHERE 1=1 " + Whereitem);
            sb.Append("  ) ");
            sb.Append("TT");
            sb.Append(" WHERE TT.Row between " + start + " and " + end);

            return dao.Select<S_CustomReprot>(sb.ToString());
        }
    }
}
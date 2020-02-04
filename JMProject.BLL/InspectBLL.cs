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
    public class InspectBLL
    {
        DBHelperSql dao = new DBHelperSql();

        public InspectBLL()
        { }

        public string MaxId()
        {
            string id = "";
            string date = DateTime.Now.ToString("yyyyMMdd");
            String tsql = "select max(Id) from Nksc_inspect where Id like '" + date + "%'";
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
            String tsql = "select count(*) from Nksc_inspect" + where;
            return dao.IsExists(tsql);
        }

        public View_Inspect GetRow(string Id)
        {
            return dao.GetRow<View_Inspect>("select * from View_Inspect where Id='" + Id + "'");
        }

        public string GetStrName(string Name, string where)
        {
            return dao.GetScalar("select " + Name + " from Nksc_inspect " + where).ToStringEx();
        }

        public int Insert(Nksc_inspect model)
        {
            return dao.Insert<Nksc_inspect>(model);
        }

        public int Update(Nksc_inspect model)
        {
            return dao.Update<Nksc_inspect>(model);
        }

        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        public int Delete(String Id)
        {
            //删除客户记录
            return dao.Delete("delete from Nksc_inspect where Id='" + Id + "'");
        }

        public List<View_Inspect> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_Inspect";
            string Fields = "[Id],[CustomerID],[Content],[CusId],[CDate],[Ywy],[Name],[BM],[Lxr],[Zw],[Phone],[Industry],[UpID],[Province],[Xydj]"
                + ",[Gx],[Zyx],[Tel],[QQ],[Email],[Address],[LxrSR],[QtLxr],[QtTel],[Bank],[CardNum],[SuiH],[Desc],[Remark],[Flag],[Uid],[Source]"
                + ",[Region],[CustomerType],[CustomerGrade],[Code],[Invoice],[UserName],[UserPwd],[Finance],[YwyName],[BmName],[ZwName],[HyName]"
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
            return dao.ProExecSelect<View_Inspect>("Proc_Page", sp);
        }

        public List<Nksc_inspect> GetComb(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "Nksc_inspect";
            string Fields = "[Content] as Id,[Content]";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = " group by Content " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = " Order by " + pager.sort + " " + pager.order;
            }
            else
            {
                Order = " Order by Id desc";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));
            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<Nksc_inspect>("Proc_Page", sp);
        }
    }
}

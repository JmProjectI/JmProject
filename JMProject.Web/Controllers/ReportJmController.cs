using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Model.Esayui;
using JMProject.BLL;
using JMProject.Model.Sys;

namespace JMProject.Web.Controllers
{
    public class ReportJmController : Controller
    {
        /// <summary>
        /// 客户数量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomReprot()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData_CustomReprot(string NameS,string OrderDateS,string OrderDateE,string DiQuS,string userS
            ,string ItemNames,string ItemMoneyS,string ItemMoneyE,string Radiobzh,
            GridPager pager)
        {

            string where = "";
            string whereItem = "";
            if (!string.IsNullOrEmpty(NameS))
            {
                where += "and Name like '%" + NameS + "%'";
            }
            if (!string.IsNullOrEmpty(ItemNames))
            {
                whereItem += " and ";
                string dqwhere = "";
                foreach (string item in ItemNames.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "ItemNames like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " " + Radiobzh + " ItemNames like '%" + item + "%'";
                    }
                }
                whereItem += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(DiQuS))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in DiQuS.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "Region = '" + item + "'";
                    }
                    else
                    {
                        dqwhere += " or Region = '" + item + "'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(OrderDateS))
            {
                where += " and OrderDate >= '" + OrderDateS + "'";
            }
            if (!string.IsNullOrEmpty(OrderDateE))
            {
                where += " and OrderDate <= '" + OrderDateE + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and ItemMoney >= '" + ItemMoneyS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and ItemMoney <= '" + ItemMoneyE + "'";
            }
            if (!string.IsNullOrEmpty(userS))
            {
                where += " and Saler = '" + userS + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            List<S_CustomReprot> result = bll.SelectCustomReprot(where,whereItem, pager);
            
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Web.AttributeEX;
using JMProject.BLL;
using JMProject.Model.Esayui;
using JMProject.Model;

namespace JMProject.Web.Controllers
{
    public class NkReportController : BaseController
    {
        #region 报告基础表

        public ActionResult GetTree_MJLSGX(string tName)
        {
            //隶属关系 NkReport_MJLSGX
            //单位所在地区 NkReport_MJSZDQ
            //支出功能分类 NkReport_MJKMDM
            switch (tName)
            {
                case "1":
                    tName = "NkReport_MJLSGX";
                    break;
                case "2":
                    tName = "NkReport_MJSZDQ";
                    break;
                case "3":
                    tName = "NkReport_MJKMDM";
                    break;
                default:
                    tName = "NkReport_MJBMBS";
                    break;
            }
            NkReportBLL bll = new NkReportBLL();
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            List<NkReport_MJLSGX> result = bll.Select_NkReport_MJLSGX("and _parentId=''", "", tName);
            foreach (NkReport_MJLSGX dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                json.Add(item);
                GetTree_MJLSGX_mx(item, tName);
            }
            return Json(json);
        }

        private void GetTree_MJLSGX_mx(EasyUIJsonTree parent, string tName)
        {
            NkReportBLL bll = new NkReportBLL();
            List<NkReport_MJLSGX> result = bll.Select_NkReport_MJLSGX("and _parentId='" + parent.id + "'", "", tName);
            if (result != null && result.Count > 0)
            {
                parent.state = "closed";
            }
            foreach (NkReport_MJLSGX dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                if (parent.children == null)
                    parent.children = new List<EasyUIJsonTree>();
                parent.children.Add(item);
                if (dr.Id.EndsWith("00"))
                {
                    GetTree_MJLSGX_mx(item, tName);
                }
            }
        }

        public ActionResult GetTree_MJBMBS(string tName)
        {
            //归属部门 NkReport_MJBMBS
            //单位预算管理级次 NkReport_MJDWYSJC
            //单位基本性质 NkReport_MJDWXZ
            //预算管理级次 NkReport_MJYSGLJC
            //内部控制体系建设的开展进度 NkReport_MJNKJD
            //内部控制适用的管理业务领域 NkReport_MJNKSYLY
            switch (tName)
            {
                case "1":
                    tName = "NkReport_MJBMBS";
                    break;
                case "2":
                    tName = "NkReport_MJDWYSJC";
                    break;
                case "3":
                    tName = "NkReport_MJDWXZ";
                    break;
                case "4":
                    tName = "NkReport_MJYSGLJC";
                    break;
                case "5":
                    tName = "NkReport_MJNKJD";
                    break;
                case "6":
                    tName = "NkReport_MJNKSYLY";
                    break;
                default:
                    tName = "NkReport_MJBMBS";
                    break;
            }
            NkReportBLL bll = new NkReportBLL();
            IList<NkReport_MJBMBS> result = bll.Select_NkReport_MJBMBS("", "", tName);
            return Json(result);
        }
        #endregion

        /// <summary>
        /// 客户填报
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [SupportFilter]
        public ActionResult Nkbg()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("手册管理");
            return View();
        }


        #region 内控报告列表 NkReport
        [SupportFilter]
        public ActionResult Report()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("内控报告");
            return View();
        }

        [HttpPost]
        public ActionResult Report_Data(string DiQuS, string NameS, string flag,
            string NkscSBDateS, string NkscSBDateE, string Uname, GridPager pager)
        {
            string where = " where 1=1 ";
            NkReportBLL bll = new NkReportBLL();
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
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS + "%'";
            }

            IList<View_NkReport> list = bll.SelectAll(where, pager);

            var griddata = new { total = pager.totalRows, rows = list };
            return Json(griddata);
        }

        [HttpPost]
        public ActionResult SysReport_Data(string Id, GridPager pager)
        {
            NkReportBLL bll = new NkReportBLL();
            string where = " where Zid='" + Id + "'";

            IList<View_SysNkReport> list = bll.ReportSelectAll(where, pager);

            var griddata = new { total = pager.totalRows, rows = list };
            return Json(griddata);
        }

        #endregion
    }
}

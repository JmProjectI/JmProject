using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.BLL;
using JMProject.Model;
using Newtonsoft.Json;
using JMProject.Web.AttributeEX;
using JMProject.Common;
using System.IO;
using JMProject.Model.Esayui;
using JMProject.Model.View;
using System.Data;

namespace JMProject.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [AuthorizeAttributeEx]
        public ActionResult Index()
        {
            ViewBag.Title = "单位内部客服管理系统";
            ViewBag.Company = "长春佳盟信息科技有限责任公司";
            return View();
        }

        [AuthorizeAttributeEx]
        public ActionResult IndexCustom()
        {
            ViewBag.Title = "行政事业单位内部控制手册基础信息采集系统";
            ViewBag.Company = GetUserTrueName();
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        #region 文件上传
        [HttpPost]
        public ActionResult Upload()
        {
            //string fileName = Request["name"].Replace("、", "-");
            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/FileSaleOrder");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错
            return Json(JsonHandler.CreateMessage(0, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Merge()
        {
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/FileSaleOrder");//文件上传目录
            var fileName = Request["fileName"];//文件名
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            System.IO.Directory.Delete(dir);//删除文件夹

            return Json(JsonHandler.CreateMessage(0, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        [SupportFilter]
        public ActionResult MyDesktop()
        {
            AddLogLook("我的桌面");

            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            ViewBag.YwyType = "";
            //03 业务员
            if (roleId == "03")
            {
                ViewBag.userS = GetUserId();
            }
            //01、03
            if (roleId == "01" || roleId == "03")
            {
                ViewBag.YwyType = "Ywy";
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetData_Desktop_Visit(string userS, GridPager pager)
        {
            string where = " and ShztName='未读' ";
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy = '" + userS.Trim() + "'";
            }

            SaleVisitBLL bll = new SaleVisitBLL();
            List<View_SaleVisit> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_Desktop_Invoice(string userS, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += " and Saler = '" + userS.Trim() + "'";
            }
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            List<View_DesktopInvoice> result = bll.Select_InvoiceAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_Desktop_Date(string userS, GridPager pager)
        {
            string where = " and NextTime>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and NextTime<='" + DateTime.Now.AddDays(3).ToString("yyyy-MM-dd") + "' ";
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy = '" + userS.Trim() + "'";
            }

            SaleVisitBLL bll = new SaleVisitBLL();
            List<View_SaleVisit> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_Desktop_VisitLlztTx(string userS, GridPager pager)
        {
            string where = " and tixingdate<='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and daoqidate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and ContactFlag<>'000080' ";
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy = '" + userS.Trim() + "'";
            }

            SaleVisitBLL bll = new SaleVisitBLL();
            List<View_SaleVisit_LlztTx> result = bll.SelectAll_LlztTx(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }
    }
}

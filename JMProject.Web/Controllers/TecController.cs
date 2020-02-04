using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.BLL;
using JMProject.Model;
using JMProject.Model.Esayui;
using JMProject.Common;
using JMProject.Web.AttributeEX;
using JMProject.Model.View;
using System.IO;
using System.Data;

namespace JMProject.Web.Controllers
{
    public class TecController : BaseController
    {
        #region TecCusService

        [SupportFilter]
        public ActionResult TecCusService()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("服务管理");

            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            //06 技术员
            if (roleId == "06")
            {
                ViewBag.userS = GetUserId();
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetData_TecCusService(string StartDate, string Ywy, string BugType, string ServiceType, string CustomName, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(StartDate))
            {
                where += "and StartDate like '" + StartDate + "%'";
            }
            if (!string.IsNullOrEmpty(Ywy))
            {
                where += "and Ywy = '" + Ywy + "'";
            }
            if (!string.IsNullOrEmpty(BugType))
            {
                where += "and BugType = '" + BugType + "'";
            }
            if (!string.IsNullOrEmpty(ServiceType))
            {
                where += "and ServiceType = '" + ServiceType + "'";
            }
            if (!string.IsNullOrEmpty(CustomName))
            {
                where += "and CustomName like '%" + CustomName + "%'";
            }
            TecCusServiceBLL bll = new TecCusServiceBLL();
            List<View_TecCusService> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_TecCusService(string Id, bool AddType = false)
        {
            TecCusServiceBLL bll = new TecCusServiceBLL();
            ViewBag.AddType = AddType;
            TecCusService result = new TecCusService();
            if (!AddType)
            {
                result = bll.GetRow(Id);
            }
            else
            {
                //获取当前用户角色ID
                string roleId = GetUserRoleID();
                //06 技术员
                if (roleId == "06")
                {
                    result.Ywy = GetUserId();
                }
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_TecCusService(TecCusService model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Id) && !AddType)
            {
                return Json(JsonHandler.CreateMessage(0, " 编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Custom))
            {
                return Json(JsonHandler.CreateMessage(0, " 客户 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Ywy))
            {
                return Json(JsonHandler.CreateMessage(0, " 业务员 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ServiceType))
            {
                return Json(JsonHandler.CreateMessage(0, " 服务类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.BugType))
            {
                return Json(JsonHandler.CreateMessage(0, " 问题类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.StartDate))
            {
                return Json(JsonHandler.CreateMessage(0, " 开始时间 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (model.TakeDay == 0 && model.TakeTime == 0)
            {
                return Json(JsonHandler.CreateMessage(0, " 耗时天数和小时不能同时为0"), JsonRequestBehavior.AllowGet);
            }
            model.GroupId = string.IsNullOrEmpty(model.GroupId) ? "" : model.GroupId;
            model.Custom = string.IsNullOrEmpty(model.Custom) ? "" : model.Custom;
            model.Ywy = string.IsNullOrEmpty(model.Ywy) ? "" : model.Ywy;
            model.ServiceType = string.IsNullOrEmpty(model.ServiceType) ? "" : model.ServiceType;
            model.BugType = string.IsNullOrEmpty(model.BugType) ? "" : model.BugType;
            model.StartDate = string.IsNullOrEmpty(model.StartDate) ? "" : model.StartDate;
            model.TakeDay = model.TakeDay < 0 ? 0 : model.TakeDay;
            model.TakeTime = model.TakeTime < 0 ? 0 : model.TakeTime;
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            TecCusServiceBLL bll = new TecCusServiceBLL();
            if (AddType)
            {
                //创建
                model.Id = bll.MaxDateid(DateTime.Now.ToString("yyyyMMdd"));
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加服务管理:" + model.Id, Suggestion.Succes, "服务管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加服务管理:" + model.Id, Suggestion.Error, "服务管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改服务管理:" + model.Id, Suggestion.Succes, "服务管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改服务管理:" + model.Id, Suggestion.Error, "服务管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_TecCusService(string Id)
        {
            TecCusServiceBLL bll = new TecCusServiceBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除服务管理:" + Id, Suggestion.Succes, "服务管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除服务管理:" + Id, Suggestion.Error, "服务管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        //导出
        public FileResult CreateTecCusServiceWeekReport(string Sdate)
        {
            try
            {
                DateTime sday = Convert.ToDateTime(Sdate);
                TecCusServiceBLL bll = new TecCusServiceBLL();
                List<TecCusServiceWeek> result = bll.GetWeekData(sday);
                MemoryStream ms = ExcelHelper.Export_Week(sday, result);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", sday.ToString("yy年M月") + "实施上报表" + DateTime.Now.ToString("HHmmss") + ".xls");
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public FileResult CreateTecCusServiceMonthReport(string Sdate)
        {
            try
            {
                DateTime sday = Convert.ToDateTime(Sdate);
                TecCusServiceBLL bll = new TecCusServiceBLL();
                DataTable result = bll.GetMonthData(Sdate);
                string cCusText = bll.getCName(Sdate);
                string tCusText = bll.getTName(Sdate);
                MemoryStream ms = ExcelHelper.Export_Month(sday.ToString("yy年M月统计"), result, cCusText, tCusText);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", Sdate + "月统计表" + DateTime.Now.ToString("HHmmss") + ".xls");
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        #endregion


    }
}

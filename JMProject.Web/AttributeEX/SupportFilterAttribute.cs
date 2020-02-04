using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Model.Sys;
using JMProject.BLL;

namespace JMProject.Web.AttributeEX
{
    public class SupportFilterAttribute : ActionFilterAttribute
    {
        public string ActionName { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Action加上[SupportFilter]在执行actin之前执行以下代码，通过[SupportFilter(ActionName="Index")]指定参数
        /// </summary>
        /// <param name="filterContext">页面传过来的上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Account"] == null)
            {
                HttpContext.Current.Response.Write("Session已过期，请重新登陆！");
                filterContext.Result = new EmptyResult();
                return;
            }

            if (ActionName == "CUD")
            {
                return;
            }

            //用户编号
            string userID = (HttpContext.Current.Session["Account"] as AccountModel).Id;

            //URL路径（/Basic/Dictionary）
            string filePath = HttpContext.Current.Request.FilePath;
            SysModuleBLL modulebll = new SysModuleBLL();

            if (filePath != "/Home/MyDesktop")
            {
                //获取模块编号
                string moduleID = modulebll.GetNameStr("Id", " and [Url]='" + filePath + "'");
                ModuleRoleBLL bll = new ModuleRoleBLL();

                if (!bll.isExist("and UserId='" + userID + "' and ModuleId='" + moduleID + "'", "SysModuleUser"))
                {
                    HttpContext.Current.Response.Write("你没有操作权限，请联系管理员！");
                    filterContext.Result = new EmptyResult();
                    return;
                }

                List<permModel> perm = null;//(List<permModel>)HttpContext.Current.Session[filePath];
                if (perm == null)
                {
                    perm = bll.SelectAll(" and UserId='" + userID + "' and ModuleId='" + moduleID + "'");
                    HttpContext.Current.Session[filePath] = perm;
                }
            }
            return;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}
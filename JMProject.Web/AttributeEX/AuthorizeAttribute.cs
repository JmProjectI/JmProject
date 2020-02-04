using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Model.Sys;

namespace JMProject.Web.AttributeEX
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttributeEx : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            AccountModel loginUser = filterContext.HttpContext.Session["Account"] as AccountModel;
            //When user has not login yet
            if (loginUser == null || string.IsNullOrEmpty(loginUser.Id))
            {
                var redirectUrl = "~/";
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
                else
                {
                    filterContext.HttpContext.Response.AddHeader("SessionTimeout", "true");
                    filterContext.Result = new HttpStatusCodeResult(403, "Session已过期，请登录！");
                }
                return;
            }
        }
    }
}
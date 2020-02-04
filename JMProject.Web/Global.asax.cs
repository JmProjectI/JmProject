using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JMProject.BLL;
using JMProject.Model.Sys;

namespace JMProject.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                "P", // 路由名称
                "P/{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Mobile", action = "Login", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Admin", // 路由名称
                "Admin/{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Account", action = "Login", id = UrlParameter.Optional } // 参数默认值
            );

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Account", action = "LoginCustom", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            if (lastError != null)
            {
                //异常信息
                string strExceptionMessage = string.Empty;

                //对HTTP 404做额外处理，其他错误全部当成500服务器错误
                HttpException httpError = lastError as HttpException;
                if (httpError != null)
                {
                    //获取错误代码
                    int httpCode = httpError.GetHttpCode();
                    strExceptionMessage = httpError.Message;
                    if (httpCode == 400 || httpCode == 404)
                    {
                        Response.StatusCode = 404;
                        //跳转到指定的静态404信息页面，根据需求自己更改URL
                        Response.WriteFile("~/404.htm");
                        Server.ClearError();
                        return;
                    }
                }

                strExceptionMessage = lastError.Message;

                /*-----------------------------------------------------
                 * 此处代码可根据需求进行日志记录，或者处理其他业务流程
                 * ---------------------------------------------------*/
                string s = HttpContext.Current.Request.Url.ToString();
                string a = lastError.HelpLink;
                string b = lastError.Source;
                string c = lastError.StackTrace;//详细错误信息
                string d = lastError.TargetSite.ToString();
                string f = lastError.Data.ToString();
                string Operator = "000001";
                SysLogBLL bll = new SysLogBLL();
                if (Session["Account"] != null)
                {
                    AccountModel info = (AccountModel)Session["Account"];
                    Operator = info.Id;
                }
                bll.AddLogExp(Operator, c, strExceptionMessage, s);

                //string exceptionOperator = "~/Home/Error";
                //try
                //{
                //    if (!String.IsNullOrEmpty(exceptionOperator))
                //    {
                //        HttpServerUtility server = HttpContext.Current.Server;
                //        exceptionOperator = new System.Web.UI.Control().ResolveUrl(exceptionOperator);
                //        string url = string.Format("{0}?ErrorUrl={1}", exceptionOperator, server.UrlEncode(s));
                //        //string script = String.Format("<script language='javascript' type='text/javascript'>window.top.location='{0}';</script>", url);
                //        string script = String.Format("<script language='javascript' type='text/javascript'>window.location.href='{0}';</script>", url);                        
                //        //Response.Write(script);
                //        //Response.End();
                //        Server.ClearError();
                //        Response.Write(url);
                //        Response.End();
                //        //Server.Transfer(url);
                //    }
                //}
                //catch { }

                /*
                 * 跳转到指定的http 500错误信息页面
                 * 跳转到静态页面一定要用Response.WriteFile方法                 
                 */
                //Response.StatusCode = 500;
                //Response.WriteFile("~/HttpError/500.html");

                //一定要调用Server.ClearError()否则会触发错误详情页（就是黄页）
                //Server.ClearError();
                //Server.Transfer("~/HttpError/500.aspx");
            }
        }
    }
}
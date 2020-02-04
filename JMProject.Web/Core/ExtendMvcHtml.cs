using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using JMProject.Model.Sys;

namespace JMProject.Web.Core
{
    public static class ExtendMvcHtml
    {
        /// <summary>
        /// 权限按钮
        /// </summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="id">控件Id</param>
        /// <param name="icon">控件icon图标class</param>
        /// <param name="text">控件的名称</param>
        /// <param name="perm">权限列表</param>
        /// <param name="keycode">操作码</param>
        /// <param name="hr">分割线</param>
        /// <returns>html</returns>
        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon, string text, List<permModel> perm, string keycode, bool hr)
        {
            if (perm.Where(a => a.KeyCode == keycode).Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<a id=\"{0}\" style=\"float: left;\" class=\"l-btn l-btn-plain\">", id);
                if (!string.IsNullOrEmpty(icon))
                {
                    sb.AppendFormat("   <span class=\"l-btn-left l-btn-icon-left\">");
                }
                else
                {
                    sb.AppendFormat("   <span class=\"l-btn-left\">");
                }
                sb.AppendFormat("       <span class=\"l-btn-text\">{0}</span>", text);
                if (!string.IsNullOrEmpty(icon))
                {
                    sb.AppendFormat("       <span class=\"l-btn-icon {0}\"></span>", icon);
                }
                sb.AppendFormat("   </span></a>");
                if (hr)
                {
                    sb.Append("<div class=\"datagrid-btn-separator\"></div>");
                }
                return new MvcHtmlString(sb.ToString());
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        /// <summary>
        /// 普通按钮
        /// </summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="id">控件Id</param>
        /// <param name="icon">控件icon图标class</param>
        /// <param name="text">控件的名称</param>
        /// <param name="hr">分割线</param>
        /// <returns>html</returns>
        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon, string text, bool hr)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<a id=\"{0}\" style=\"float: left;\" class=\"l-btn l-btn-plain\">", id);
            if (!string.IsNullOrEmpty(icon))
            {
                sb.AppendFormat("   <span class=\"l-btn-left l-btn-icon-left\">");
            }
            else
            {
                sb.AppendFormat("   <span class=\"l-btn-left\">");
            }
            sb.AppendFormat("       <span class=\"l-btn-text\">{0}</span>", text);
            if (!string.IsNullOrEmpty(icon))
            {
                sb.AppendFormat("       <span class=\"l-btn-icon {0}\"></span>", icon);
            }
            sb.AppendFormat("   </span></a>");
            if (hr)
            {
                sb.Append("<div class=\"datagrid-btn-separator\"></div>");
            }
            return new MvcHtmlString(sb.ToString());

        }
    }
}
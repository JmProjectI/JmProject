using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Model.Esayui;
using JMProject.BLL;
using JMProject.Model;
using JMProject.Common;
using JMProject.Model.Sys;
using JMProject.Web.AttributeEX;
using System.IO;

namespace JMProject.Web.Controllers
{
    public class SystemController : BaseController
    {
        #region 模块管理
        [SupportFilter]
        public ActionResult SysModule()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("模块管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetTree_SysModule()
        {
            string userID = GetUserId();
            if (userID.Length == 14)
            {
                string RoleID = GetUserRoleID();
                GridPager pager = new GridPager { page = 1, rows = 1000, sort = "_parentId,Sort", order = "asc" };
                string where = "and len(ID)=2 and ID in (select ModuleId from SysModuleRole where RoleId='" + RoleID + "' and len(ModuleId)=2)";//
                SysModuleBLL bll = new SysModuleBLL();
                List<SysModule> parent = bll.SelectAll(where, pager);
                where = "and len(ID)<>2 and ID in (select ModuleId from SysModuleRole where RoleId='" + RoleID + "' and len(ModuleId)<>2)";//
                List<SysModule> child = bll.SelectAll(where, pager);
                var result = new { parent = parent, child = child };
                return Json(result);
            }
            else
            {
                GridPager pager = new GridPager { page = 1, rows = 1000, sort = "_parentId,Sort", order = "asc" };
                string where = "and len(ID)=2 and ID in (select ModuleId from SysModuleUser where UserId='" + userID + "' and len(ModuleId)=2)";//
                SysModuleBLL bll = new SysModuleBLL();
                List<SysModule> parent = bll.SelectAll(where, pager);
                where = "and len(ID)<>2 and ID in (select ModuleId from SysModuleUser where UserId='" + userID + "' and len(ModuleId)<>2)";//
                List<SysModule> child = bll.SelectAll(where, pager);
                var result = new { parent = parent, child = child };
                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult GetData_SysModule()
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id,Sort", order = "asc" };
            string where = "";
            SysModuleBLL bll = new SysModuleBLL();
            List<SysModule> result = bll.SelectAll(where, pager);
            var json = new { rows = result };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_SysModuleRole(string Rid, string RType)
        {
            SysModuleBLL bll = new SysModuleBLL();
            List<SysModuleRole> result = bll.SelectAllRole(Rid, RType);
            var json = new { rows = result };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SysModule(string Empty, SysModule result, bool AddType = false)
        {
            SysModuleBLL bll = new SysModuleBLL();
            ViewBag.AddType = AddType;
            if (!ViewBag.AddType)
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.Icon = "pic_22";
                if (!string.IsNullOrEmpty(result._parentId))
                {
                    result._parentId = result._parentId.Substring(0, 2);
                    result.Id = bll.Maxid(result._parentId);
                    result.Sort = bll.MaxSort(result._parentId);
                }
                else
                {
                    result.Id = bll.Maxid("");
                    result.Sort = bll.MaxSort("");
                }
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SysModule(SysModule model, bool AddType = false)
        {
            SysModuleBLL bll = new SysModuleBLL();
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Icon))
            {
                return Json(JsonHandler.CreateMessage(0, "图标 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Url))
            {
                return Json(JsonHandler.CreateMessage(0, "连接 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            //if (string.IsNullOrEmpty(model.Sort))
            //{
            //    return Json(JsonHandler.CreateMessage(0, " Sort 不允许为空"), JsonRequestBehavior.AllowGet);
            //}
            if (!string.IsNullOrEmpty(model._parentId))
            {
                if (model._parentId.Length != 2)
                {
                    return Json(JsonHandler.CreateMessage(0, "上级编号长度必须2位"), JsonRequestBehavior.AllowGet);
                }
                if (!bll.isExist("and Id='" + model._parentId + "'"))
                {
                    return Json(JsonHandler.CreateMessage(0, "上级编号不存在"), JsonRequestBehavior.AllowGet);
                }
            }

            if (AddType)
            {
                //创建
                //model.Id = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模块:" + model.Id, Suggestion.Succes, "模块管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模块:" + model.Id, Suggestion.Error, "模块管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模块:" + model.Id, Suggestion.Succes, "模块管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模块:" + model.Id, Suggestion.Error, "模块管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SysModule(string Id)
        {
            SysModuleBLL bll = new SysModuleBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除模块:" + Id, Suggestion.Succes, "模块管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除模块:" + Id, Suggestion.Error, "模块管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 模块操作码(增删改)

        [HttpPost]
        public JsonResult GetData_SysModuleOperateRole(string ModuleId, string Rid, string RType)
        {
            SysModuleOperateBLL bll = new SysModuleOperateBLL();
            List<SysModuleOperateRole> result = bll.SelectAllRole(ModuleId, Rid, RType);
            var json = new
            {
                total = result.Count,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_SysModuleOperate(string ModuleId, GridPager pager)
        {
            pager.page = 1;
            pager.rows = 1000;
            string where = "";
            //if (!string.IsNullOrEmpty(ModuleId))
            //{
            where += "and ModuleId = '" + ModuleId + "'";
            //}
            SysModuleOperateBLL bll = new SysModuleOperateBLL();
            List<SysModuleOperate> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SysModuleOperate(string Empty, SysModuleOperate result, bool AddType = false)
        {
            SysModuleOperateBLL bll = new SysModuleOperateBLL();
            ViewBag.AddType = AddType;
            if (!string.IsNullOrEmpty(result.Id))
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.Id = bll.Maxid(result.ModuleId);
                result.Sort = bll.MaxSort(result.ModuleId);
                result.IsValid = true;
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SysModuleOperate(SysModuleOperate model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ModuleId))
            {
                return Json(JsonHandler.CreateMessage(0, "菜单编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.KeyCode))
            {
                return Json(JsonHandler.CreateMessage(0, "操作码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            SysModuleOperateBLL bll = new SysModuleOperateBLL();
            if (AddType)
            {
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模块操作码:" + model.Id, Suggestion.Succes, "模块管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模块操作码:" + model.Id, Suggestion.Error, "模块管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模块操作码:" + model.Id, Suggestion.Succes, "模块管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模块操作码:" + model.Id, Suggestion.Error, "模块管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SysModuleOperate(string Id)
        {
            SysModuleOperateBLL bll = new SysModuleOperateBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除模块操作码:" + Id, Suggestion.Succes, "模块管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除模块操作码:" + Id, Suggestion.Error, "模块管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 角色管理

        [SupportFilter]
        public ActionResult SysRole()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("角色管理");
            return View();
        }

        [HttpPost]
        public ActionResult GetTree_SysRole(bool all, bool user)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            SysRoleBLL bll = new SysRoleBLL();
            string where = " and Len(Id)=2 ";
            List<SysRole> result = bll.SelectAll(where, pager);
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            foreach (SysRole dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                item.attributes = "R";
                GetTree_SysRole_Child(item, user);
                if (user)
                {
                    GetTree_SysUser(item);//添加用户
                }
                json.Add(item);
            }
            if (all)
            {
                EasyUIJsonTree itemall = new EasyUIJsonTree();
                itemall.id = "";
                itemall.text = "全部";
                itemall.attributes = "R";
                json.Insert(0, itemall);
            }

            return Json(json);
        }

        private void GetTree_SysRole_Child(EasyUIJsonTree parent, bool user)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            SysRoleBLL bll = new SysRoleBLL();
            string where = " and Id like '" + parent.id + "%' and len(Id)=" + (parent.id.Length + 2) + " ";
            List<SysRole> result = bll.SelectAll(where, pager);
            if (result.Count > 0 && parent.children == null)
            {
                parent.children = new List<EasyUIJsonTree>();
            }
            foreach (SysRole dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                item.attributes = "R";
                GetTree_SysRole_Child(item, user);
                if (user)
                {
                    GetTree_SysUser(item);//添加用户
                }
                parent.children.Add(item);
            }
        }

        private void GetTree_SysUser(EasyUIJsonTree parent)
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
            SysUserBLL bll = new SysUserBLL();
            string where = " and RoleId = '" + parent.id + "' ";
            List<SysUser> result = bll.SelectAll(where, pager);
            if (result.Count > 0 && parent.children == null)
            {
                parent.children = new List<EasyUIJsonTree>();
            }
            foreach (SysUser dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name + "(" + dr.ZsName + ")";
                item.iconCls = "pic_198";
                item.attributes = "U";
                parent.children.Add(item);
            }
        }

        [HttpPost]
        public JsonResult GetData_SysRole(string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name))
            {
                where += " and Name like '%" + Name + "%'";
            }
            SysRoleBLL bll = new SysRoleBLL();
            List<SysRole> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SysRole(string PId, SysRole result, bool AddType = false)
        {
            SysRoleBLL bll = new SysRoleBLL();
            ViewBag.AddType = AddType;
            if (!string.IsNullOrEmpty(result.Id))
            {
                result = bll.GetRow(result);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SysRole(SysRole model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            SysRoleBLL bll = new SysRoleBLL();
            if (AddType)
            {
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加角色管理:" + model.Id, Suggestion.Succes, "角色管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加角色管理:" + model.Id, Suggestion.Error, "角色管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改角色管理:" + model.Id, Suggestion.Succes, "角色管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改角色管理:" + model.Id, Suggestion.Error, "角色管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SysRole(string Id)
        {
            SysRoleBLL bll = new SysRoleBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除角色管理:" + Id, Suggestion.Succes, "角色管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除角色管理:" + Id, Suggestion.Error, "角色管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 员工管理

        [SupportFilter]
        public ActionResult SysUser()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("员工管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_SysUser(string RoleID, string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(RoleID))
            {
                where += "and RoleID like '" + RoleID + "%'";
            }
            if (!string.IsNullOrEmpty(Name))
            {
                where += "and Name like '%" + Name + "%'";
            }
            SysUserBLL bll = new SysUserBLL();
            List<SysUser> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SysUser(string RoleID = "", string Id = "")
        {
            SysUserBLL bll = new SysUserBLL();
            SysUser result = new SysUser { Id = Id };
            if (!string.IsNullOrEmpty(Id))
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.RoleID = RoleID;
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SysUser(SysUser model)
        {
            SysUserBLL bll = new SysUserBLL();
            if (string.IsNullOrEmpty(model.RoleID))
            {
                return Json(JsonHandler.CreateMessage(0, "角色 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "用户名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (bll.isExist("and Name='" + model.Name + "' and Id<>'" + model.Id + "'"))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户名 已存在"), JsonRequestBehavior.AllowGet);
                }
            }
            if (string.IsNullOrEmpty(model.Pwd))
            {
                return Json(JsonHandler.CreateMessage(0, "密码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ZsName))
            {
                return Json(JsonHandler.CreateMessage(0, "姓名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                return Json(JsonHandler.CreateMessage(0, "手机号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Birthday))
            {
                return Json(JsonHandler.CreateMessage(0, "出生日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Pic = string.IsNullOrEmpty(model.Pic) ? "" : model.Pic;
            model.Tel = string.IsNullOrEmpty(model.Tel) ? "" : model.Tel;
            model.IcCard = string.IsNullOrEmpty(model.IcCard) ? "" : model.IcCard;
            model.Address = string.IsNullOrEmpty(model.Address) ? "" : model.Address;
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;

            if (string.IsNullOrEmpty(model.Id))
            {
                //创建
                model.Id = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    bll.Insert("Insert into SysModuleUser(UserId,ModuleId) select '" + model.Id + "' as UserId,ModuleId from SysModuleRole where RoleId='" + model.RoleID + "'");
                    bll.Insert("Insert into SysModuleOperateUser(UserId,ModuleOpId) select '" + model.Id + "' as UserId,ModuleOpId from SysModuleOperateRole where RoleId='" + model.RoleID + "'");
                    LogHelper.AddLogUser(GetUserId(), "添加员工管理:" + model.Id, Suggestion.Succes, "员工管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加员工管理:" + model.Id, Suggestion.Error, "员工管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改员工管理:" + model.Id, Suggestion.Succes, "员工管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改员工管理:" + model.Id, Suggestion.Error, "员工管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SysUser(string Id)
        {
            SysUserBLL bll = new SysUserBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除员工管理:" + Id, Suggestion.Succes, "员工管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除员工管理:" + Id, Suggestion.Error, "员工管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult GetComb_Users(bool All = false, string RoleId = null)
        {
            string where = "";
            if (!string.IsNullOrEmpty(RoleId))
            {
                where = " and RoleID='" + RoleId + "'";
            }
            GridPager pager = new GridPager { page = 1, rows = 50, sort = "id", order = "asc" };
            SysUserBLL bll = new SysUserBLL();
            IList<SysUser> list = bll.SelectAll(where, pager);
            if (All)
            {
                list.Insert(0, new SysUser() { Id = "", ZsName = "全部" });
            }
            return Json(list);
        }
        #endregion

        #region 权限管理

        [SupportFilter]
        public ActionResult SysModuleRole()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("权限管理");
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult SetSysModuleRole(string RId, string RType, string ModuleID, bool isCheck)
        {
            if (string.IsNullOrEmpty(RId))
            {
                return Json(JsonHandler.CreateMessage(0, "角色/用户编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(RType))
            {
                return Json(JsonHandler.CreateMessage(0, "角色/用户类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(ModuleID))
            {
                return Json(JsonHandler.CreateMessage(0, "模块 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            ModuleRoleBLL bll = new ModuleRoleBLL();
            if (RType == "R")//角色
            {
                if (isCheck)
                {
                    //赋权
                    if (bll.isExistRole(RId, ModuleID))
                    {
                        return Json(JsonHandler.CreateMessage(0, "该角色已包涵此权限"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bll.InsertRole(RId, ModuleID);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //删除赋权
                    if (bll.isExistRole(RId, ModuleID))
                    {
                        bll.DeleteRole(RId, ModuleID);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(JsonHandler.CreateMessage(0, "该角色已不包涵此权限,无法删除此权限"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (RType == "U")//用户
            {
                if (isCheck)
                {
                    //赋权
                    if (bll.isExistUser(RId, ModuleID))
                    {
                        return Json(JsonHandler.CreateMessage(0, "该用户已包涵此权限"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bll.InsertUser(RId, ModuleID);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //删除赋权
                    if (bll.isExistUser(RId, ModuleID))
                    {
                        bll.DeleteUser(RId, ModuleID);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(JsonHandler.CreateMessage(0, "该用户已不包涵此权限,无法删除此权限"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult SetSysModuleOptRole(string RId, string RType, string ModuleOpId, bool isCheck)
        {
            if (string.IsNullOrEmpty(RId))
            {
                return Json(JsonHandler.CreateMessage(0, "角色/用户编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(RType))
            {
                return Json(JsonHandler.CreateMessage(0, "角色/用户类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(ModuleOpId))
            {
                return Json(JsonHandler.CreateMessage(0, "操作码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            ModuleRoleBLL bll = new ModuleRoleBLL();
            if (RType == "R")//角色
            {
                if (isCheck)
                {
                    //赋权
                    if (bll.isExistOptRole(RId, ModuleOpId))
                    {
                        return Json(JsonHandler.CreateMessage(0, "该角色已包涵此权限"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bll.InsertOptRole(RId, ModuleOpId);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //删除赋权
                    if (bll.isExistOptRole(RId, ModuleOpId))
                    {
                        bll.DeleteOptRole(RId, ModuleOpId);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(JsonHandler.CreateMessage(0, "该角色已不包涵此权限,无法删除此权限"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (RType == "U")//用户
            {
                if (isCheck)
                {
                    //赋权
                    if (bll.isExistOptUser(RId, ModuleOpId))
                    {
                        return Json(JsonHandler.CreateMessage(0, "该用户已包涵此权限"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        bll.InsertOptUser(RId, ModuleOpId);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //删除赋权
                    if (bll.isExistOptUser(RId, ModuleOpId))
                    {
                        bll.DeleteOptUser(RId, ModuleOpId);
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(JsonHandler.CreateMessage(0, "该用户已不包涵此权限,无法删除此权限"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 日志管理

        [SupportFilter]
        public ActionResult Log()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("日志管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_Log(string LogType, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(LogType))
            {
                where += "and LogType = '" + LogType + "'";
            }
            SysLogBLL bll = new SysLogBLL();
            List<SysLog> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        public ActionResult Create_Log(string Id)
        {
            SysLogBLL bll = new SysLogBLL();
            SysLog result = bll.GetRow(Id);
            LogHelper.AddLogUser(GetUserId(), "查看日志详情:" + Id, Suggestion.Succes, "日志管理");
            return View(result);
        }

        #endregion

        #region 备份还原
        [SupportFilter]
        public ActionResult SysBackUp()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("备份还原");
            return View();
        }

        public ActionResult BackUp_Data(string type, string sort, string order, int page, int rows)
        {
            List<backinfo> list = new List<backinfo>();

            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/DBbackFile/"));
            foreach (FileInfo item in dir.GetFiles())
            {
                backinfo model = new backinfo() { date = item.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"), mc = item.Name };
                list.Add(model);
            }

            var total = list.Count;
            var griddata = new { total = total, rows = list };
            return Json(griddata);
        }

        public class backinfo
        {
            public string date { get; set; }
            public string mc { get; set; }
        }


        [HttpPost]
        public ActionResult backupDB()
        {
            try
            {
                string ZIPPath = Server.MapPath("~/DBbackFile/");// Server.MapPath("../Page_System/DBbackFile");
                if (!Directory.Exists(ZIPPath))
                {
                    Directory.CreateDirectory(ZIPPath);
                }
                string filePaht = ZIPPath + "\\" + "JMProject" + DateTime.Now.ToString("yyyyMMddHHMMss") + ".bak";
                if (DataBack.BakSql("JMProject", filePaht))
                {
                    LogHelper.AddLogUser(GetUserId(), "数据备份", Suggestion.Succes, "备份还原");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "数据备份", Suggestion.Error, "备份还原");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                LogHelper.AddLogUser(GetUserId(), "数据备份", Suggestion.Error, "备份还原");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult resDB(string fileName)
        {
            try
            {
                string ZIPPath = Server.MapPath("~/DBbackFile/");//路径
                string filePaht = ZIPPath + fileName;
                if (DataBack.ResSql("JMProject", filePaht))
                {
                    LogHelper.AddLogUser(GetUserId(), "数据还原", Suggestion.Succes, "备份还原");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "数据还原", Suggestion.Error, "备份还原");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                LogHelper.AddLogUser(GetUserId(), "数据还原", Suggestion.Error, "备份还原");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}

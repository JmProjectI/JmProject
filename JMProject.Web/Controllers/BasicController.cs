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

namespace JMProject.Web.Controllers
{
    public class BasicController : BaseController
    {
        #region 数据字典
        [SupportFilter]
        public ActionResult Dictionary()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("数据字典");
            return View();
        }

        [HttpPost]
        public ActionResult GetTree_Dictionary()
        {
            DictionaryBLL bll = new DictionaryBLL();
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            List<Dictionary> result = bll.SelectAll("and DicFlag='02'", "");
            foreach (Dictionary dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.DicID;
                item.text = dr.DicName;
                json.Add(item);
            }
            return Json(json);
        }

        [HttpPost]
        public ActionResult GetTree_DictionaryItem(string DicID)
        {
            GridPager pager = new GridPager { page = 1, rows = 50, sort = "ItemID", order = "asc" };
            DictionaryBLL bll = new DictionaryBLL();
            string where = "and DicID='" + DicID + "'";
            List<DictionaryItem> result = bll.SelectAll(where, pager);
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            foreach (DictionaryItem dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.ItemID;
                item.text = dr.ItemName;
                json.Add(item);
            }
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_DictionaryItem(string DicID, string ItemName, GridPager pager)
        {
            string where = "and DicID='" + DicID + "'";
            if (!string.IsNullOrEmpty(ItemName))
            {
                where += " and ItemName like '%" + ItemName + "%'";
            }
            DictionaryBLL bll = new DictionaryBLL();
            List<DictionaryItem> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_DicItem(string DicID = "", string ItemID = "")
        {
            DictionaryBLL bll = new DictionaryBLL();
            DictionaryItem result = new DictionaryItem { DicID = DicID, ItemID = ItemID };
            if (!string.IsNullOrEmpty(ItemID))
            {
                result = bll.GetRowItem(result);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_DicItem(DictionaryItem model)
        {
            if (string.IsNullOrEmpty(model.ItemName))
            {
                return Json(JsonHandler.CreateMessage(0, "名称不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.ItemFlag = string.IsNullOrEmpty(model.ItemFlag) ? "00" : model.ItemFlag;
            DictionaryBLL bll = new DictionaryBLL();
            if (string.IsNullOrEmpty(model.ItemID))
            {
                //创建
                model.ItemID = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加字典明细:" + model.ItemID, Suggestion.Succes, "数据字典");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加字典明细:" + model.ItemID, Suggestion.Error, "数据字典");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改字典明细:" + model.ItemID, Suggestion.Succes, "数据字典");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改字典明细:" + model.ItemID, Suggestion.Error, "数据字典");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_DicItem(string ItemID)
        {
            DictionaryBLL bll = new DictionaryBLL();
            if (bll.Delete(ItemID) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除字典明细:" + ItemID, Suggestion.Succes, "数据字典");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除字典明细:" + ItemID, Suggestion.Error, "数据字典");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        //数据字典
        [HttpPost]
        public ActionResult GetComb_DictionaryItem(string DicID, bool All = false)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemID", order = "asc" };
            DictionaryBLL bll = new DictionaryBLL();
            string where = "and DicID='" + DicID + "'";
            IList<DictionaryItem> list = bll.SelectAll(where, pager);
            if (All)
            {
                list.Insert(0, new DictionaryItem() { ItemID = "", ItemName = "全部" });
            }
            return Json(list);
        }

        //审批流程
        [HttpPost]
        public ActionResult GetComb_SpLcDictionaryItem(string DicID, bool All = false)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemDesc", order = "asc" };
            DictionaryBLL bll = new DictionaryBLL();
            string where = "and DicID='" + DicID + "'";
            IList<DictionaryItem> list = bll.SelectAll(where, pager);
            if (All)
            {
                list.Insert(0, new DictionaryItem() { ItemID = "", ItemName = "全部" });
            }
            return Json(list);
        }
        #endregion

        #region 省份
        //省份
        [HttpPost]
        public ActionResult Get_CombProvince()
        {
            string where = "";
            GridPager pager = new GridPager { page = 1, rows = 50, sort = "Pid", order = "asc" };
            BasicProvinceBLL bll = new BasicProvinceBLL();
            IList<BasicProvince> list = bll.SelectAll(where, pager);
            return Json(list);
        }

        [SupportFilter]
        public ActionResult BasicProvince()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("省份管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_BasicProvince(string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                where += "and Name like '%" + Name.Trim() + "%'";
            }
            BasicProvinceBLL bll = new BasicProvinceBLL();
            List<BasicProvince> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_BasicProvince(string Id = "")
        {
            BasicProvinceBLL bll = new BasicProvinceBLL();
            BasicProvince result = new BasicProvince();
            if (string.IsNullOrEmpty(Id))
            {
                result.Pid = Id;
            }
            else
            {
                result.Pid = Id;
                result = bll.GetRow(result);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_BasicProvince(BasicProvince model)
        {
            BasicProvinceBLL bll = new BasicProvinceBLL();
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "省份名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string str = bll.GetStringName("count(*)", "where Name='" + model.Name + "'");
                if (str != "0")
                {
                    return Json(JsonHandler.CreateMessage(0, "省份名称 已存在"), JsonRequestBehavior.AllowGet);
                }
            }

            if (string.IsNullOrEmpty(model.Pid))
            {
                //创建
                model.Pid = bll.MaxId();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加省份管理:" + model.Pid, Suggestion.Succes, "省份管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加省份管理:" + model.Pid, Suggestion.Error, "省份管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改省份管理:" + model.Pid, Suggestion.Succes, "省份管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改省份管理:" + model.Pid, Suggestion.Error, "省份管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult Delete_BasicProvince(string Id)
        {
            BasicProvinceBLL bll = new BasicProvinceBLL();
            SaleCustomerBLL boo = new SaleCustomerBLL();
            BasicCityBLL bl = new BasicCityBLL();
            string str = bl.GetStringName("count(*)", "where Pid='" + Id + "'");
            if (str != "0")
            {
                return Json(JsonHandler.CreateMessage(0, "该省份有相关联的地区 不可删除"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string val = boo.GetStrName("count(*)", "where Province='" + Id + "'");
                if (val != "0")
                {
                    return Json(JsonHandler.CreateMessage(0, "该省份有相关联的客户 不可删除"), JsonRequestBehavior.AllowGet);
                }
            }
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除省份管理:" + Id, Suggestion.Succes, "省份管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除省份管理:" + Id, Suggestion.Error, "省份管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 上级主管区域(九大地区)
        [HttpPost]
        public ActionResult Get_CombCityPL(string pid)
        {
            string where = "";
            if (!string.IsNullOrEmpty(pid))
            {
                where += " and Sfid='" + pid + "'";
            }
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ID", order = "asc" };
            BasicCityPLBLL bll = new BasicCityPLBLL();
            IList<View_BasicCityPL> list = bll.SelectAll(where, pager);
            return Json(list);
        }
        
        [HttpPost]
        public JsonResult GetData_BasicCityPL(string Pid, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Pid.Trim()))
            {
                where += " and Sfid = '" + Pid + "'";
            }
            BasicCityPLBLL bll = new BasicCityPLBLL();
            List<View_BasicCityPL> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_BasicCityPL(string Id = "", string pid = "")
        {
            BasicCityPLBLL bll = new BasicCityPLBLL();
            BasicCityPL result = new BasicCityPL();
            if (string.IsNullOrEmpty(Id))
            {
                result.Pid = pid;
            }
            else
            {
                result.ID = Id;
                result = bll.GetRow(result);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_BasicCityPL(BasicCityPL model)
        {
            BasicCityPLBLL bll = new BasicCityPLBLL();
            if (string.IsNullOrEmpty(model.Pid))
            {
                return Json(JsonHandler.CreateMessage(0, " 省份 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, " 名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string str = bll.GetNameStr("count(*)", " and Name='" + model.Name + "'");
                if (str != "0")
                {
                    return Json(JsonHandler.CreateMessage(0, "名称 已存在"), JsonRequestBehavior.AllowGet);
                }
            }
            if (string.IsNullOrEmpty(model.ID))
            {
                //创建
                model.ID = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加上级主管区域管理:" + model.ID, Suggestion.Succes, "上级主管区域管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加上级主管区域管理:" + model.ID, Suggestion.Error, "上级主管区域管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改上级主管区域管理:" + model.ID, Suggestion.Succes, "上级主管区域管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改上级主管区域管理:" + model.ID, Suggestion.Error, "上级主管区域管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_BasicCityPL(string ID)
        {
            BasicCityPLBLL bll = new BasicCityPLBLL();
            if (bll.Delete(ID) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除上级主管区域管理:" + ID, Suggestion.Succes, "上级主管区域管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除上级主管区域管理:" + ID, Suggestion.Error, "上级主管区域管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 地区(所在县市区)

        //地区
        [HttpPost]
        public ActionResult Get_CombCity(string pid)
        {
            string where = "";
            if (!string.IsNullOrEmpty(pid))
            {
                where += " and Pid='" + pid + "'";
            }
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Pid", order = "asc" };
            BasicCityBLL bll = new BasicCityBLL();
            IList<View_BasicCity> list = bll.SelectAll(where, pager);
            return Json(list);
        }

        [SupportFilter]
        public ActionResult BasicCity()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("地区管理");
            return View();
        }

        [HttpPost]
        public ActionResult GetTree_BasicProvince()
        {
            BasicProvinceBLL bll = new BasicProvinceBLL();
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            List<BasicProvince> result = bll.SelectAll_Tree("", "");
            foreach (BasicProvince dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Pid;
                item.text = dr.Name;
                json.Add(item);
            }
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_BasicCity(string Name, string Pid, string Sfid, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                where += "and Name like '%" + Name.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(Pid.Trim()))
            {
                where += " and Pid = '" + Pid.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(Sfid.Trim()))
            {
                where += " and Sfid = '" + Sfid.Trim() + "'";
            }
            BasicCityBLL bll = new BasicCityBLL();
            List<View_BasicCity> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_BasicCity(string Id = "", string pid = "")
        {
            BasicCityBLL bll = new BasicCityBLL();
            BasicCity result = new BasicCity();
            if (string.IsNullOrEmpty(Id))
            {
                result.Pid = pid;
            }
            else
            {
                result.ID = Id;
                result = bll.GetRow(result);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_BasicCity(BasicCity model)
        {
            BasicCityBLL bll = new BasicCityBLL();
            if (string.IsNullOrEmpty(model.Pid))
            {
                return Json(JsonHandler.CreateMessage(0, "上级主管区域 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "地区名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string str = bll.GetStringName("count(*)", "where Name='" + model.Name + "'");
                if (str != "0")
                {
                    return Json(JsonHandler.CreateMessage(0, "地区名称 已存在"), JsonRequestBehavior.AllowGet);
                }
            }
            if (string.IsNullOrEmpty(model.ID))
            {
                //创建
                model.ID = bll.MaxId();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加地区管理:" + model.ID, Suggestion.Succes, "地区管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加地区管理:" + model.ID, Suggestion.Error, "地区管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改地区管理:" + model.ID, Suggestion.Succes, "地区管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改地区管理:" + model.ID, Suggestion.Error, "地区管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult Delete_BasicCity(string Id)
        {
            BasicCityBLL bll = new BasicCityBLL();
            SaleCustomerBLL boo = new SaleCustomerBLL();

            string val = boo.GetStrName("count(*)", "where Region='" + Id + "' or UpID='" + Id + "'");
            if (val != "0")
            {
                return Json(JsonHandler.CreateMessage(0, "该地区有相关联的客户 不可删除"), JsonRequestBehavior.AllowGet);
            }

            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除地区管理:" + Id, Suggestion.Succes, "地区管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除地区管理:" + Id, Suggestion.Error, "地区管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult GetTree_Province(bool all)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Pid", order = "asc" };
            string where = "";
            BasicProvinceBLL bll = new BasicProvinceBLL();
            //省份
            IList<BasicProvince> result = bll.SelectAll(where, pager);
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            foreach (BasicProvince dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Pid;
                item.text = dr.Name;
                item.attributes = "A";
                GetTree_BasicCityPL(item);
                json.Add(item);
            }
            if (all)
            {
                EasyUIJsonTree itemall = new EasyUIJsonTree();
                itemall.id = "";
                itemall.text = "全部";
                itemall.attributes = "A";
                json.Insert(0, itemall);
            }
            return Json(json);
        }

        public void GetTree_BasicCityPL(EasyUIJsonTree parent)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ID", order = "asc" };
            BasicCityPLBLL bll = new BasicCityPLBLL();
            string where = " and Sfid like '" + parent.id + "%' ";
            List<View_BasicCityPL> result = bll.SelectAll(where, pager);
            if (result.Count > 0 && parent.children == null)
            {
                parent.children = new List<EasyUIJsonTree>();
            }
            foreach (View_BasicCityPL dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.ID;
                item.text = dr.Name;
                item.attributes = "B";
                GetTree_BasicCity(item);
                parent.children.Add(item);
            }
        }

        public void GetTree_BasicCity(EasyUIJsonTree parent)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ID", order = "asc" };
            BasicCityBLL bll = new BasicCityBLL();
            string where = " and pid like '" + parent.id + "%' ";
            List<View_BasicCity> result = bll.SelectAll(where, pager);
            if (result.Count > 0 && parent.children == null)
            {
                parent.children = new List<EasyUIJsonTree>();
            }
            foreach (View_BasicCity dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.ID;
                item.text = dr.Name;
                item.attributes = "C";
                parent.children.Add(item);
            }
        }

        #region 账户管理

        [SupportFilter]
        public ActionResult BasicAccount()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("账户管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetComb_BasicAccount()
        {
            string where = "";
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            BasicAccountBLL bll = new BasicAccountBLL();
            IList<BasicAccount> list = bll.SelectAll(where, pager);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetComb_BasicAccountAll()
        {
            string where = "";
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            BasicAccountBLL bll = new BasicAccountBLL();
            IList<BasicAccount> list = bll.SelectAll(where, pager);
            list.Insert(0, new BasicAccount() { Id = "", Name = "全部", Key = "全部" });
            return Json(list);
        }


        [HttpPost]
        public JsonResult GetData_BasicAccount(string Id, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where += "and Id = '" + Id + "'";
            }
            BasicAccountBLL bll = new BasicAccountBLL();
            List<BasicAccount> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_BasicAccount(string empty, BasicAccount result, bool AddType = false)
        {
            BasicAccountBLL bll = new BasicAccountBLL();
            ViewBag.AddType = AddType;
            if (!AddType)
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
        public JsonResult Create_BasicAccount(BasicAccount model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Key))
            {
                return Json(JsonHandler.CreateMessage(0, "简写 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Bank))
            {
                return Json(JsonHandler.CreateMessage(0, "开户行 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.BankNum))
            {
                return Json(JsonHandler.CreateMessage(0, "账号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.SNum))
            {
                return Json(JsonHandler.CreateMessage(0, "税号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            BasicAccountBLL bll = new BasicAccountBLL();
            if (AddType)
            {
                //创建
                model.Id = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加账户管理:" + model.Id, Suggestion.Succes, "账户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加账户管理:" + model.Id, Suggestion.Error, "账户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改账户管理:" + model.Id, Suggestion.Succes, "账户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改账户管理:" + model.Id, Suggestion.Error, "账户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_BasicAccount(string Id)
        {
            BasicAccountBLL bll = new BasicAccountBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除账户管理:" + Id, Suggestion.Succes, "账户管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除账户管理:" + Id, Suggestion.Error, "账户管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 物流
        public ActionResult Exp_Traces(string LogisticCode)
        {
            ViewBag.Perm = GetPermission();
            ViewBag.LogisticCode = LogisticCode;
            AddLogLook("快递管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_Exp_Traces(string LogisticCode, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(LogisticCode))
            {
                where += "and  LogisticCode= '" + LogisticCode + "'";
            }
            Exp_TracesBLL bll = new Exp_TracesBLL();
            List<Exp_Traces> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }
        #endregion

        #region 上门用户百分比设置

        [SupportFilter]
        public ActionResult BasicSet()
        {

            ViewBag.Perm = GetPermission();
            BasicSetBLL bll = new BasicSetBLL();
            BasicSet model = new BasicSet();
            string userid = GetUserId();
            if (bll.isExist(" and userid='" + userid + "'"))
            {
                model = bll.GetRow(userid);
            }
            else
            {
                model = bll.GetRow("000001");
                model.Userid = userid;
            }
            AddLogLook("上门设置");
            return View(model);
        }
        
        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_BasicSet(BasicSet model)
        {
            if (string.IsNullOrEmpty(model.Userid))
            {
                return Json(JsonHandler.CreateMessage(0, "用户不允许为空"), JsonRequestBehavior.AllowGet);
            }

            BasicSetBLL bll = new BasicSetBLL();
            if (!bll.isExist(" and userid='" + model.Userid + "'"))
            {
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加上门设置:" + model.Userid, Suggestion.Succes, "上门设置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加上门设置:" + model.Userid, Suggestion.Error, "上门设置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改上门设置:" + model.Userid, Suggestion.Succes, "上门设置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改上门设置:" + model.Userid, Suggestion.Error, "上门设置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion
    }
}
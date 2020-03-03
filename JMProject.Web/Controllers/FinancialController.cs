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
using JMProject.Model.View;
using System.Data;

namespace JMProject.Web.Controllers
{
    public class FinancialController : BaseController
    {
        #region 产品分类管理

        [SupportFilter]
        public ActionResult FinProductType()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("产品分类管理");
            return View();
        }

        public ActionResult FinProductTypeChoose()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult GetTree_FinProductType(bool all)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            FinProductTypeBLL bll = new FinProductTypeBLL();
            string where = " and Len(Id)=2 ";
            List<FinProductType> result = bll.SelectAll(where, pager);
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();
            foreach (FinProductType dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                GetTree_FinProductType_Child(item);
                json.Add(item);
            }
            if (all)
            {
                EasyUIJsonTree itemall = new EasyUIJsonTree();
                itemall.id = "";
                itemall.text = "全部";
                json.Insert(0, itemall);
            }

            return Json(json);
        }

        private void GetTree_FinProductType_Child(EasyUIJsonTree parent)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "asc" };
            FinProductTypeBLL bll = new FinProductTypeBLL();
            string where = " and Id like '" + parent.id + "%' and len(Id)=" + (parent.id.Length + 2) + " ";
            List<FinProductType> result = bll.SelectAll(where, pager);
            if (result.Count > 0 && parent.children == null)
            {
                parent.children = new List<EasyUIJsonTree>();
            }
            foreach (FinProductType dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.Id;
                item.text = dr.Name;
                GetTree_FinProductType_Child(item);
                parent.children.Add(item);
            }
        }

        [HttpPost]
        public JsonResult GetData_FinProductType()
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
            string where = "";
            FinProductTypeBLL bll = new FinProductTypeBLL();
            List<FinProductType> result = bll.SelectAll(where, pager);
            var json = new { rows = result };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinProductType(string PId, FinProductType result, bool AddType = false)
        {
            FinProductTypeBLL bll = new FinProductTypeBLL();
            ViewBag.AddType = AddType;
            if (!AddType)
            {
                //修改
                result = bll.GetRow(result);
            }
            else
            {
                //添加
                if (!string.IsNullOrEmpty(PId))
                {
                    result._parentId = PId;
                }
                result.Id = bll.Maxid(PId);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinProductType(FinProductType model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "产品分类编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "产品分类名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            FinProductTypeBLL bll = new FinProductTypeBLL();
            if (AddType)
            {
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品分类管理:" + model.Id, Suggestion.Succes, "产品分类管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品分类管理:" + model.Id, Suggestion.Error, "产品分类管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改产品分类管理:" + model.Id, Suggestion.Succes, "产品分类管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改产品分类管理:" + model.Id, Suggestion.Error, "产品分类管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinProductType(string Id)
        {
            FinProductTypeBLL bll = new FinProductTypeBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除产品分类管理:" + Id, Suggestion.Succes, "产品分类管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除产品分类管理:" + Id, Suggestion.Error, "产品分类管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 产品管理

        [SupportFilter]
        public ActionResult FinProduct()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("产品管理");
            return View();
        }

        public ActionResult FinProductChoose(string TypeId)
        {
            ViewBag.TypeId = TypeId;
            return View();
        }

        [HttpPost]
        public JsonResult GetData_FinProduct(string Pid, string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Pid))
            {
                where += " and TypeId like '" + Pid + "%'";
            }
            if (!string.IsNullOrEmpty(Name))
            {
                where += " and Pkey like '%" + Name + "%'";
            }
            FinProductBLL bll = new FinProductBLL();
            List<View_FinProduct> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinProduct(string PId, FinProduct result, bool AddType = false)
        {
            FinProductBLL bll = new FinProductBLL();
            ViewBag.AddType = AddType;
            if (!AddType)
            {
                result = bll.GetRow(result.Id);
            }
            else
            {
                result.TypeId = PId;
                result.Id = bll.Maxid(PId);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinProduct(FinProduct model, String Products, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "产品编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "产品名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.TypeId))
            {
                return Json(JsonHandler.CreateMessage(0, "产品分类 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.TypeId = string.IsNullOrEmpty(model.TypeId) ? "" : model.TypeId;
            model.Name = string.IsNullOrEmpty(model.Name) ? "" : model.Name;
            model.Spec = string.IsNullOrEmpty(model.Spec) ? "" : model.Spec;
            model.Pkey = string.IsNullOrEmpty(model.Pkey) ? "" : model.Pkey;
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            FinProductBLL bll = new FinProductBLL();
            FinProductItemBLL pibll = new FinProductItemBLL();
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            List<FinOrderItem> ListItem = ParseFromJson<List<FinOrderItem>>(Products);
            if (AddType)
            {
                model.InCount = 1;//入库总数
                model.OutCount = 0;//出库总数
                model.stock = 1;//库存总数
                //创建
                model.Id = bll.Maxid(model.TypeId);

                int ItemCount = pibll.GetCount(" and ProId='" + model.Id + "'");
                if (ItemCount > 0)
                {
                    tsqls.Add("delete from FinProductItem where ProId='" + model.Id + "'", null);
                }

                #region 库存管理明细表(插入数据)
                string sqlProductItem = "INSERT INTO FinProductItem([ItemID],[ProId],[ModularID],[CbMoney],[JcMoney],[CsCount],[AddCount],[AddMoney]"
                        + ",[SumCount],[sumMoney]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
                //库存成本
                Decimal CbMoney = 0.00M;
                //库存市场定价
                Decimal SumMoney = 0.00M;
                //模块名称
                string MkName = "";
                //明细编号
                string maxKCId = "";
                //库存名称
                string ProName = "";
                for (int k = 0; k < ListItem.Count; k++)//循环模块数
                {
                    FinOrderItem item = ListItem[k];
                    CbMoney += item.CbMoney;
                    SumMoney += item.sumMoney;
                    MkName += item.TypeName + "*" + item.SumCount.ToString() + "、";
                    //日期
                    string D = DateTime.Now.ToString("yyyyMMdd");
                    //入库单明细表(插入数据)
                    if (string.IsNullOrEmpty(maxKCId))
                    {
                        maxKCId = pibll.Max_FinProductItem(D);
                    }
                    else
                    {
                        maxKCId = D + (int.Parse(maxKCId.Substring(8)) + 1).ToString("000000");
                    }

                    string sqltext = string.Format(sqlProductItem, new object[] { maxKCId, model.Id, item.ModularID, item.CbMoney,item.JcMoney
                            ,item.CsCount, item.AddCount, item.AddMoney, item.SumCount, item.sumMoney });
                    tsqls.Add(sqltext, null);
                }
                #endregion

                if (model.TypeId.Substring(0, 6) == "010101" || model.TypeId.Substring(0, 6) == "010102")
                {
                    ProName = MkName.TrimEnd('、');
                }
                else
                {
                    ProName = model.Name;
                }

                #region 库存管理主表(插入数据)
                string sqlProduct = "INSERT INTO FinProduct([TypeId],[Id],[Name],[Spec],[Ucount],[Pkey],[Marketprice],[Costprice],[InitialCount]"
                    + ",[InCount],[OutCount],[stock],[Remake]) VALUES('" + model.TypeId + "','" + model.Id + "','" + ProName
                    + "','" + model.Spec + "','" + model.Ucount + "','" + model.Pkey + "','" + SumMoney + "','" + CbMoney
                    + "','" + model.InitialCount + "','" + model.InCount + "','" + model.OutCount + "','" + model.stock + "','" + model.Remake + "')";

                tsqls.Add(sqlProduct, null);
                #endregion

                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品管理:" + model.Id, Suggestion.Succes, "产品管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品管理:" + model.Id, Suggestion.Error, "产品管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                int ItemCount = pibll.GetCount(" and ProId='" + model.Id + "'");
                if (ItemCount > 0)
                {
                    tsqls.Add("delete from FinProductItem where ProId='" + model.Id + "'", null);
                }

                #region 库存管理明细表(插入数据)
                string sqlProductItem = "INSERT INTO FinProductItem([ItemID],[ProId],[ModularID],[CbMoney],[JcMoney],[CsCount],[AddCount],[AddMoney]"
                        + ",[SumCount],[sumMoney]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
                //库存成本
                Decimal CbMoney = 0.00M;
                //库存市场定价
                Decimal SumMoney = 0.00M;
                //模块名称
                string MkName = "";
                //明细编号
                string maxKCId = "";
                //库存名称
                string ProName = "";
                for (int k = 0; k < ListItem.Count; k++)//循环模块数
                {
                    FinOrderItem item = ListItem[k];
                    CbMoney += item.CbMoney;
                    SumMoney += item.sumMoney;
                    MkName += item.TypeName + "*" + item.SumCount.ToString() + "、";
                    //日期
                    string D = DateTime.Now.ToString("yyyyMMdd");
                    //入库单明细表(插入数据)
                    if (string.IsNullOrEmpty(maxKCId))
                    {
                        maxKCId = pibll.Max_FinProductItem(D);
                    }
                    else
                    {
                        maxKCId = D + (int.Parse(maxKCId.Substring(8)) + 1).ToString("000000");
                    }

                    string sqltext = string.Format(sqlProductItem, new object[] { maxKCId, model.Id, item.ModularID, item.CbMoney,item.JcMoney
                            ,item.CsCount, item.AddCount, item.AddMoney, item.SumCount, item.sumMoney });
                    tsqls.Add(sqltext, null);
                }
                #endregion

                if (model.TypeId.Substring(0, 6) == "010101" || model.TypeId.Substring(0, 6) == "010102")
                {
                    ProName = MkName.TrimEnd('、');
                }
                else
                {
                    ProName = model.Name;
                }

                #region 库存管理主表(插入数据)
                string sqlProduct = "UPDATE FinProduct SET TypeId='" + model.TypeId + "',Name='" + ProName + "',Pkey='" + model.Pkey
                    + "',Marketprice='" + SumMoney + "',Costprice='" + CbMoney + "',Remake='" + model.Remake + "' where Id='" + model.Id + "'";

                tsqls.Add(sqlProduct, null);
                #endregion

                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品管理:" + model.Id, Suggestion.Succes, "产品管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加产品管理:" + model.Id, Suggestion.Error, "产品管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }

                //model.InCount = int.Parse(bll.GetNameStr("InCount", "and Id='" + model.Id + "'"));
                //model.OutCount = int.Parse(bll.GetNameStr("OutCount", "and Id='" + model.Id + "'"));
                //int InitialCount = int.Parse(bll.GetNameStr("InitialCount", "and Id='" + model.Id + "'"));
                //model.stock = int.Parse(bll.GetNameStr("stock", "and Id='" + model.Id + "'")) - InitialCount + model.InitialCount;
                ////修改
                //if (bll.Update(model) > 0)
                //{
                //    LogHelper.AddLogUser(GetUserId(), "修改产品管理:" + model.Id, Suggestion.Succes, "产品管理");
                //    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    LogHelper.AddLogUser(GetUserId(), "修改产品管理:" + model.Id, Suggestion.Error, "产品管理");
                //    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                //}
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinProduct(string Id)
        {
            FinProductBLL bll = new FinProductBLL();
            string ChuKu = bll.GetNameStr("OutCount", " and Id='" + Id + "'");
            if (ChuKu == "1")
            {
                return Json(JsonHandler.CreateMessage(0, "此库存单已出库，无法删除！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                Dictionary<string, object> tsqls = new Dictionary<string, object>();

                string sqlitem = "delete from FinProductItem where ProId='" + Id + "'";
                tsqls.Add(sqlitem, null);

                string sqltext = "delete from FinProduct where Id='" + Id + "'";
                tsqls.Add(sqltext, null);

                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "删除产品管理:" + Id, Suggestion.Succes, "产品管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "删除产品管理:" + Id, Suggestion.Error, "产品管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region 供应商管理

        [SupportFilter]
        public ActionResult FinSupplier()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("供应商管理");
            return View();
        }

        public ActionResult GetData_FinSupplier_Comb()
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
            string where = "";
            FinSupplierBLL bll = new FinSupplierBLL();
            IList<FinSupplier> list = bll.SelectAll(where, pager);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetData_FinSupplier(string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name))
            {
                where += "and Name like '%" + Name + "%'";
            }
            FinSupplierBLL bll = new FinSupplierBLL();
            List<FinSupplier> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinSupplier(string Empty, FinSupplier result, bool AddType = false)
        {
            FinSupplierBLL bll = new FinSupplierBLL();
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
        public JsonResult Create_FinSupplier(FinSupplier model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Linkman = string.IsNullOrEmpty(model.Linkman) ? "" : model.Linkman;
            model.Phone = string.IsNullOrEmpty(model.Phone) ? "" : model.Phone;
            model.Tel = string.IsNullOrEmpty(model.Tel) ? "" : model.Tel;
            model.Address = string.IsNullOrEmpty(model.Address) ? "" : model.Address;
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            FinSupplierBLL bll = new FinSupplierBLL();
            if (AddType)
            {
                //创建
                model.Id = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加供应商管理:" + model.Id, Suggestion.Succes, "供应商管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加供应商管理:" + model.Id, Suggestion.Error, "供应商管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改供应商管理:" + model.Id, Suggestion.Succes, "供应商管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改供应商管理:" + model.Id, Suggestion.Error, "供应商管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinSupplier(string Id)
        {
            FinSupplierBLL bll = new FinSupplierBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除供应商管理:" + Id, Suggestion.Succes, "供应商管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除供应商管理:" + Id, Suggestion.Error, "供应商管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 入库单管理

        [SupportFilter]
        public ActionResult FinOrder()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("入库单管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_FinOrder(string Id, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where += "and Id = '" + Id + "'";
            }
            FinOrderBLL bll = new FinOrderBLL();
            List<View_FinOrder> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_FinOrderItem(string Id)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemId", order = "asc" };
            string where = "";
            where += "and OrderId = '" + Id + "'";
            FinOrderBLL bll = new FinOrderBLL();
            List<View_FinOrderItem> result = bll.SelectItemAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinOrder(string Empty, FinOrder result, bool AddType = false)
        {
            FinOrderBLL bll = new FinOrderBLL();
            ViewBag.AddType = AddType;
            ViewBag.Account = GetAccount();
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
        public JsonResult Create_FinOrder(FinOrder model, String Products, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.OrderDate))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.SupplierId))
            {
                return Json(JsonHandler.CreateMessage(0, "供应商 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.UserId))
            {
                return Json(JsonHandler.CreateMessage(0, "经手人 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            List<FinOrderItem> ListItem = ParseFromJson<List<FinOrderItem>>(Products);
            string TypeId = model.ProductId.Substring(0, 2);

            if (TypeId == "01")
            {
                if (string.IsNullOrEmpty(model.PkeyStart) || string.IsNullOrEmpty(model.PkeyEnd))
                {
                    return Json(JsonHandler.CreateMessage(0, "序列号段 不允许为空"), JsonRequestBehavior.AllowGet);
                }

                if (ListItem.Count < 1)
                {
                    return Json(JsonHandler.CreateMessage(0, "请添加商品"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.PkeyStart))
                {
                    return Json(JsonHandler.CreateMessage(0, "数量不允许为空"), JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.PkeyEnd))
                {
                    return Json(JsonHandler.CreateMessage(0, "单位不允许为空"), JsonRequestBehavior.AllowGet);
                }
            }
            //暂时允许为0
            //for (int i = 0; i < ListItem.Count; i++)
            //{
            //    FinOrderItem item = ListItem[i];
            //    if (item.JcMoney <= 0)
            //    {
            //        return Json(JsonHandler.CreateMessage(0, (i + 1) + "行入库数量必须大于零"), JsonRequestBehavior.AllowGet);
            //    }
            //}
            //备注
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            FinOrderBLL bll = new FinOrderBLL();
            FinProductBLL pbll = new FinProductBLL();
            FinProductItemBLL pibll = new FinProductItemBLL();
            string maxproID = "";
            if (AddType)//增加
            {
                //创建
                model.Id = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));
                Dictionary<string, object> tsqls = new Dictionary<string, object>();

                #region 入库单主表(插入数据)
                string sqlMain = "INSERT INTO FinOrder(Id,OrderDate,SupplierId,ProductId,PkeyStart,PkeyEnd,UserId,Remake) values('"
                    + model.Id + "','" + model.OrderDate + "','" + model.SupplierId + "','" + model.ProductId + "','" + model.PkeyStart
                    + "','" + model.PkeyEnd + "','" + model.UserId + "','" + model.Remake + "')";
                tsqls.Add(sqlMain, null);
                #endregion

                #region 入库单明细表(插入数据)
                string sqlItem = "INSERT INTO FinOrderItem([OrderId],[ItemId],[ModularID],[TypeName],[CbMoney],[JcMoney],[CsCount],[AddCount]"
                    + ",[AddMoney],[SumCount],[sumMoney]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";
                string FinItemId = "";
                //库存成本
                Decimal CbMoney = 0.00M;
                //库存市场定价
                Decimal SumMoney = 0.00M;
                //模块名称
                string MkName = "";
                for (int i = 0; i < ListItem.Count; i++)//循环模块数
                {
                    FinOrderItem item = ListItem[i];
                    //if(item.CbMoney)
                    CbMoney += item.CbMoney;
                    SumMoney += item.sumMoney;
                    MkName += item.TypeName + "*" + item.SumCount.ToString() + "、";
                    if (string.IsNullOrEmpty(FinItemId))
                    {
                        FinItemId = model.Id + "0001";
                    }
                    else
                    {
                        FinItemId = model.Id + (Convert.ToInt32(FinItemId.Substring(12)) + 1).ToString("0000");
                    }
                    string sqltext = string.Format(sqlItem, new object[] { model.Id, FinItemId, item.ModularID, item.TypeName, item.CbMoney, 
                        item.JcMoney,item.CsCount, item.AddCount, item.AddMoney, item.SumCount, item.sumMoney });

                    tsqls.Add(sqltext, null);
                }
                #endregion

                if (TypeId == "01")
                {
                    //获取序列号段的个数
                    int PkeyCount = Convert.ToInt32(model.PkeyEnd) - Convert.ToInt32(model.PkeyStart);
                    //库存明细表ID
                    string maxKCId = "";
                    //循环序列号的个数(插入库存主表和明细表)
                    for (int j = 0; j <= PkeyCount; j++)
                    {
                        string Pkey = (Convert.ToInt32(model.PkeyStart) + j).ToString();

                        //库存管理主表(插入数据)
                        string sqlProduct = "";
                        if (string.IsNullOrEmpty(maxproID))
                        {
                            maxproID = pbll.Maxid(model.ProductId);
                        }
                        else
                        {
                            maxproID = (int.Parse(maxproID) + 1).ToString("000000000000");
                        }
                        sqlProduct = "insert into FinProduct values('" + model.ProductId + "','" + maxproID + "','" + MkName.TrimEnd('、') + "','" + model.Id + "',0,'"
                            + Pkey + "','" + SumMoney + "','" + CbMoney + "',0,'1',0,'1','')";
                        tsqls.Add(sqlProduct, null);

                        string sqlProductItem = "INSERT INTO FinProductItem([ItemID],[ProId],[ModularID],[CbMoney],[JcMoney],[CsCount],[AddCount]"
                            + ",[AddMoney],[SumCount],[sumMoney]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";

                        for (int k = 0; k < ListItem.Count; k++)//循环模块数
                        {
                            FinOrderItem item = ListItem[k];
                            //日期
                            string D = DateTime.Now.ToString("yyyyMMdd");
                            //入库单明细表(插入数据)
                            if (string.IsNullOrEmpty(maxKCId))
                            {
                                maxKCId = pibll.Max_FinProductItem(D);
                            }
                            else
                            {
                                maxKCId = D + (int.Parse(maxKCId.Substring(8)) + 1).ToString("000000");
                            }

                            string sqltext = string.Format(sqlProductItem, new object[] { maxKCId, maxproID, item.ModularID, item.CbMoney,item.JcMoney
                            ,item.CsCount, item.AddCount, item.AddMoney, item.SumCount, item.sumMoney });
                            tsqls.Add(sqltext, null);

                            //if (pbll.isExist("and TypeId='" + model.ProductId + "'"))// and Name='" + item.TypeName + "' and Pkey='" + item.Pkey + "'
                            //{
                            //sqlProduct = "update FinProduct set stock=stock+1 where TypeId='" + model.ProductId + "'";// and Name='" + item.TypeName + "' and Pkey='" + item.Pkey + "'
                            //}
                            //else
                            //{
                            //[TypeId],[Id],[Name],[Spec],[Ucount],[Pkey],[Marketprice],[Costprice],[InitialCount],[InCount],[OutCount],[stock],[Remake]
                            //}
                        }
                    }
                }
                else
                {
                    string ProId = pbll.GetNameStr("Id", " and TypeId='" + model.ProductId + "'");
                    if (ProId == "")
                    {
                        FinProductTypeBLL tbll = new FinProductTypeBLL();
                        if (string.IsNullOrEmpty(maxproID))
                        {
                            maxproID = pbll.Maxid(model.ProductId);
                        }
                        else
                        {
                            maxproID = (int.Parse(maxproID) + 1).ToString("000000000000");
                        }
                        //产品类别名称
                        string typename = tbll.GetNameStr("Name", " and Id='" + model.ProductId + "'");
                        //数量
                        string Count = model.PkeyStart;
                        //市场价
                        string Money = model.PkeyEnd;

                        //插入语句
                        string sqlProduct = "insert into FinProduct([TypeId],[Id],[Name],[Spec],[Ucount],[Pkey],[Marketprice],[Costprice],[InitialCount],[InCount],[OutCount]"
                            + ",[stock],[Remake]) values('" + model.ProductId + "','" + maxproID + "','" + typename + "','" + model.Id + "',0,'','" + Money + "',0.00,0,'"
                            + Count + "',0,'" + Count + "','')";
                        //执行
                        tsqls.Add(sqlProduct, null);
                    }
                    else
                    {
                        //数量
                        string Count = model.PkeyStart;
                        //市场价
                        string Money = model.PkeyEnd;
                        string tsql = "update FinProduct set Marketprice='" + Money + "',InCount=InCount+" + Count + ",stock=stock+" + Count + " where Id='" + ProId + "'";
                        tsqls.Add(tsql, null);
                    }
                }
                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "添加入库单管理:" + model.Id, Suggestion.Succes, "入库单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加入库单管理:" + model.Id, Suggestion.Error, "入库单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else//修改
            {
                //Dictionary<string, object> tsqls = bll.GetDeleteSql(model.Id);
                //string sqlMain = "update FinOrder set OrderDate='" + model.OrderDate + "',SupplierId='" + model.SupplierId + "',UserId='" + model.UserId + "',Remake='" + model.Remake + "' where Id='" + model.Id + "'";
                //tsqls.Add(sqlMain, null);
                //string sqlItem = "INSERT INTO FinOrderItem([OrderId],[ItemId],[ProductTypeId],[ProName],[ProSpec],[ProUcount],[Pkey],[ItemCount],[ItemPrice],[ItemMoney]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
                //for (int i = 0; i < ListItem.Count; i++)
                //{
                //    FinOrderItem item = ListItem[i];
                //    item.ProSpec = string.IsNullOrEmpty(item.ProSpec) ? "" : item.ProSpec;
                //    item.ProUcount = string.IsNullOrEmpty(item.ProUcount) ? "" : item.ProUcount;
                //    item.Pkey = string.IsNullOrEmpty(item.Pkey) ? "" : item.Pkey;
                //    item.ItemId = model.Id + (i + 1).ToString("00");
                //    string sqltext = string.Format(sqlItem, new object[] { model.Id, item.ItemId, item.ProductTypeId, item.ProName, item.ProSpec, item.ProUcount, item.Pkey, item.ItemCount, item.ItemPrice, item.ItemMoney });
                //    tsqls.Add(sqltext, null);
                //    string sqlProduct = "";
                //    if (pbll.isExist("and TypeId='" + item.ProductTypeId + "' and Name='" + item.ProName + "' and Pkey='" + item.Pkey + "'"))
                //    {
                //        sqlProduct = "update FinProduct set InCount=InCount+" + item.ItemCount + ",stock=stock+" + item.ItemCount + ",Spec='" + item.ProSpec + "',Ucount='" + item.ProUcount + "',Costprice='" + item.ItemPrice + "' where TypeId='" + item.ProductTypeId + "' and Name='" + item.ProName + "' and Pkey='" + item.Pkey + "'";
                //    }
                //    else
                //    {
                //        if (string.IsNullOrEmpty(maxproID))
                //        {
                //            maxproID = pbll.Maxid(item.ProductTypeId);
                //        }
                //        else
                //        {
                //            maxproID = (int.Parse(maxproID) + 1).ToString("000000000000");
                //        }
                //        //[TypeId],[Id],[Name],[Spec],[Ucount],[Pkey],[Marketprice],[Costprice],[InitialCount],[InCount],[OutCount],[stock],[Remake]
                //        sqlProduct = "insert into FinProduct values('" + item.ProductTypeId + "','" + maxproID + "','" + item.ProName + "','" + item.ProSpec + "','"
                //            + item.ProUcount + "','" + item.Pkey + "','0.00','" + item.ItemPrice + "',0,'" + item.ItemCount + "',0,'" + item.ItemCount + "','')";
                //    }
                //    tsqls.Add(sqlProduct, null);
                //}
                ////修改
                //if (bll.Tran(tsqls))
                //{
                //    LogHelper.AddLogUser(GetUserId(), "修改入库单管理:" + model.Id, Suggestion.Succes, "入库单管理");
                //    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    LogHelper.AddLogUser(GetUserId(), "修改入库单管理:" + model.Id, Suggestion.Error, "入库单管理");
                //    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                //}
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinOrder(string Id)
        {
            FinOrderBLL bll = new FinOrderBLL();
            FinProductBLL pbll = new FinProductBLL();
            int OutCount = pbll.GetCount(" and Spec='" + Id + "' and OutCount<>0");
            if (OutCount > 0)
            {
                return Json(JsonHandler.CreateMessage(0, "此入库单相对应库存已出库，无法删除！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (bll.Delete(Id))
                {
                    LogHelper.AddLogUser(GetUserId(), "删除入库单管理:" + Id, Suggestion.Succes, "入库单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "删除入库单管理:" + Id, Suggestion.Error, "入库单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult FinProductModularChoose()
        {
            return View();
        }

        public JsonResult Get_FinProductModular(string DicID, GridPager pager)
        {
            string where = " and DicID='028'";
            DictionaryBLL bll = new DictionaryBLL();
            List<DictionaryItem> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_FinProductItem(string Id)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemId", order = "asc" };
            string where = "";
            where += "and ProId = '" + Id + "'";
            FinProductItemBLL bll = new FinProductItemBLL();
            List<View_FinProductItem> result = bll.SelectItemAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }
        #endregion

        #region 提成设置

        [SupportFilter]
        public ActionResult FinSalesCommission()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("提成设置");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_FinSalesCommission()
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "TypeId", order = "asc" };
            string where = "";
            FinSalesCommissionBLL bll = new FinSalesCommissionBLL();
            List<View_FinSalesCommission> result = bll.SelectAll(where, pager);
            var json = new { rows = result };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinSalesCommission(string PId, View_FinSalesCommission result)
        {
            FinSalesCommissionBLL bll = new FinSalesCommissionBLL();
            if (!string.IsNullOrEmpty(result.Id))
            {
                result = bll.GetRow(result);
                result.Unfinished = result.Unfinished * 100;
                result.Finished = result.Finished * 100;
                result.Nonsalesman = result.Nonsalesman * 100;
            }
            else
            {
                if (!string.IsNullOrEmpty(PId))
                {
                    result.TypeId = PId;
                }
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinSalesCommission(FinSalesCommission model)
        {
            if (string.IsNullOrEmpty(model.TypeId))
            {
                return Json(JsonHandler.CreateMessage(0, "分类 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Unfinished = model.Unfinished / 100;
            model.Finished = model.Finished / 100;
            model.Nonsalesman = model.Nonsalesman / 100;
            FinSalesCommissionBLL bll = new FinSalesCommissionBLL();
            if (string.IsNullOrEmpty(model.Id))
            {
                //创建
                model.Id = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加提成设置:" + model.Id, Suggestion.Succes, "提成设置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加提成设置:" + model.Id, Suggestion.Error, "提成设置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改提成设置:" + model.Id, Suggestion.Succes, "提成设置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改提成设置:" + model.Id, Suggestion.Error, "提成设置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinSalesCommission(string Id)
        {
            FinSalesCommissionBLL bll = new FinSalesCommissionBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除提成设置:" + Id, Suggestion.Succes, "提成设置");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除提成设置:" + Id, Suggestion.Error, "提成设置");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 销售单管理

        [SupportFilter]
        public ActionResult FinSaleOrder()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("销售单管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_FinSaleOrder(string Id, string NameS, string OrderDateS, string OrderDateE
            , string DiQuS, string userS, string dzS, string fpS, string ckS, string ItemNames, string Tc
            , string ItemMoneyS, string ItemMoneyE, string Account, string PayDateS, string PayDateE
            , GridPager pager)
        {
            string where = " and Flag='0'";
            if (!string.IsNullOrEmpty(Id))
            {
                where += "and Id = '" + Id + "'";
            }
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(ItemNames))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in ItemNames.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "ItemNames like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " or ItemNames like '%" + item + "%'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(Tc))
            {
                if (Tc == "0")
                {
                    //未提成
                    where += " and unTC>0 and TC=0";
                }
                else if (Tc == "1")
                {
                    //已提成
                    where += " and unTC=0 and TC>0";
                }
                else if (Tc == "2")
                {
                    //部分提成
                    where += " and unTC>0 and TC>0";
                }
            }
            if (!string.IsNullOrEmpty(OrderDateS))
            {
                where += " and OrderDate >= '" + OrderDateS + "'";
            }
            if (!string.IsNullOrEmpty(OrderDateE))
            {
                where += " and OrderDate <= '" + OrderDateE + "'";
            }
            if (!string.IsNullOrEmpty(DiQuS.Trim()))
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
            if (!string.IsNullOrEmpty(userS))
            {
                where += " and Saler = '" + userS + "'";
            }
            if (!string.IsNullOrEmpty(fpS))
            {
                where += " and InvoiceFlag = '" + fpS + "'";
            }
            if (!string.IsNullOrEmpty(dzS))
            {
                where += " and PaymentFlag = '" + dzS + "'";
            }
            if (!string.IsNullOrEmpty(ckS))
            {
                where += " and OutStockFlag = '" + ckS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and ItemMoney >= '" + ItemMoneyS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and ItemMoney <= '" + ItemMoneyE + "'";
            }
            if (!string.IsNullOrEmpty(Account))
            {
                where += " and Id in (select distinct OrderId from FinOrderInvoice where AccountId='" + Account + "')";
            }
            if (!string.IsNullOrEmpty(PayDateS))
            {
                where += " and Paymentdate >= '" + PayDateS + "'";
            }
            if (!string.IsNullOrEmpty(PayDateE))
            {
                where += " and Paymentdate <= '" + PayDateE + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            List<View_SaleOrder> result = bll.SelectAll(where, pager);
            List<View_SaleOrder> footer = bll.SelectAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        public ActionResult FinSaleOrderExcel(string Id, string NameS, string OrderDateS, string OrderDateE
            , string DiQuS, string userS, string dzS, string fpS, string ckS, string ItemNames, string Tc
            , string ItemMoneyS, string ItemMoneyE, string Account, string PayDateS, string PayDateE)
        {
            string where = " and Flag='0'";
            if (!string.IsNullOrEmpty(Id))
            {
                where += "and Id = '" + Id + "'";
            }
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(ItemNames))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in ItemNames.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "ItemNames like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " or ItemNames like '%" + item + "%'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(Tc))
            {
                if (Tc == "0")
                {
                    //未提成
                    where += " and unTC>0 and TC=0";
                }
                else if (Tc == "1")
                {
                    //已提成
                    where += " and unTC=0 and TC>0";
                }
                else if (Tc == "2")
                {
                    //部分提成
                    where += " and unTC>0 and TC>0";
                }
            }
            if (!string.IsNullOrEmpty(OrderDateS))
            {
                where += " and OrderDate >= '" + OrderDateS + "'";
            }
            if (!string.IsNullOrEmpty(OrderDateE))
            {
                where += " and OrderDate <= '" + OrderDateE + "'";
            }
            if (!string.IsNullOrEmpty(DiQuS.Trim()))
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
            if (!string.IsNullOrEmpty(userS))
            {
                where += " and Saler = '" + userS + "'";
            }
            if (!string.IsNullOrEmpty(fpS))
            {
                where += " and InvoiceFlag = '" + fpS + "'";
            }
            if (!string.IsNullOrEmpty(dzS))
            {
                where += " and PaymentFlag = '" + dzS + "'";
            }
            if (!string.IsNullOrEmpty(ckS))
            {
                where += " and OutStockFlag = '" + ckS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and ItemMoney >= '" + ItemMoneyS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and ItemMoney <= '" + ItemMoneyE + "'";
            }
            if (!string.IsNullOrEmpty(Account))
            {
                where += " and Id in (select distinct OrderId from FinOrderInvoice where AccountId='" + Account + "')";
            }
            if (!string.IsNullOrEmpty(PayDateS))
            {
                where += " and Paymentdate >= '" + PayDateS + "'";
            }
            if (!string.IsNullOrEmpty(PayDateE))
            {
                where += " and Paymentdate <= '" + PayDateE + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            DataTable dt = new DataTable();
            dt = bll.SelectDC_xsd(where);

            System.Web.UI.WebControls.DataGrid dgExport = null;
            // 当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;
            string filename = "导出销售单" + DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] str = null;

            if (dt != null)
            {
                // 设置编码和附件格式 
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "gb2312";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                // 导出excel文件 
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
                dgExport = new System.Web.UI.WebControls.DataGrid();
                dgExport.DataSource = dt.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                // 返回客户端 
                str = System.Text.Encoding.UTF8.GetBytes(strWriter.ToString());
            }
            return File(str, "attachment;filename=" + filename + ".xls");
        }

        #endregion

        #region 待开发票
        [SupportFilter]
        public ActionResult FinDkSaleOrder()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("待开发票");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_FinDkSaleOrder(string NameS, string DiQuS, string userS, string ItemNames, GridPager pager)
        {
            string where = " and Flag='0' and InvoiceFlag='000063' and Fp<>''";
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(ItemNames))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in ItemNames.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "ItemNames like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " or ItemNames like '%" + item + "%'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(DiQuS.Trim()))
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
            if (!string.IsNullOrEmpty(userS))
            {
                where += " and Saler = '" + userS + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            List<View_SaleOrder> result = bll.SelectAll(where, pager);
            List<View_SaleOrder> footer = bll.SelectAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        /// <summary>
        /// 待开发票导出
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="NameS"></param>
        /// <param name="OrderDateS"></param>
        /// <param name="OrderDateE"></param>
        /// <param name="DiQuS"></param>
        /// <param name="userS"></param>
        /// <param name="dzS"></param>
        /// <param name="fpS"></param>
        /// <param name="ckS"></param>
        /// <param name="ItemNames"></param>
        /// <param name="Tc"></param>
        /// <param name="ItemMoneyS"></param>
        /// <param name="ItemMoneyE"></param>
        /// <param name="Account"></param>
        /// <returns></returns>
        public ActionResult FinDkSaleOrderExcel(string NameS, string ItemNames, string DiQuS, string userS)
        {
            string where = " and Flag='0' and InvoiceFlag='000063' and Fp<>''";
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(ItemNames))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in ItemNames.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "ItemNames like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " or ItemNames like '%" + item + "%'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(DiQuS.Trim()))
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
            if (!string.IsNullOrEmpty(userS))
            {
                where += " and Saler = '" + userS + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            DataTable dt = new DataTable();
            dt = bll.SelectDkDC_xsd(where);

            System.Web.UI.WebControls.DataGrid dgExport = null;
            // 当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;
            string filename = "导出待开销售单" + DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] str = null;

            if (dt != null)
            {
                // 设置编码和附件格式 
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "gb2312";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                // 导出excel文件 
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
                dgExport = new System.Web.UI.WebControls.DataGrid();
                dgExport.DataSource = dt.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                // 返回客户端 
                str = System.Text.Encoding.UTF8.GetBytes(strWriter.ToString());
            }
            return File(str, "attachment;filename=" + filename + ".xls");
        }
        #endregion

        #region 销售单 出库单
        [HttpPost]
        public JsonResult GetData_FinOutStock(string OrderId, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(OrderId))
            {
                where += "and OrderId = '" + OrderId + "'";
            }
            FinOutStockBLL bll = new FinOutStockBLL();
            List<View_OutStock> result = bll.SelectOutStockAll(where, pager);
            List<View_OutStock> footer = bll.SelectOutStockAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_FinOutStockItem(string Id, GridPager pager)
        {
            pager.page = 1;
            pager.rows = 100;
            string where = "";
            //if (!string.IsNullOrEmpty(Id))
            //{
            where += "and OutStockId = '" + Id + "'";
            //}
            FinOutStockBLL bll = new FinOutStockBLL();
            List<View_FinOutStockItem> result = bll.SelectItemAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinOutStock(FinOutStock result, bool AddType = false)
        {
            FinOutStockBLL bll = new FinOutStockBLL();
            SaleOrderBLL sbll = new SaleOrderBLL();
            ViewBag.AddType = AddType;
            if (!AddType)
            {
                result = bll.GetRow(result);
                ViewBag.Account = bll.GetNameStr("ZsName", "and Uid='" + result.Uid + "'");
            }
            else
            {
                ViewBag.Account = GetUserTrueName();
                result.OSdate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinOutStock(FinOutStock model, String Products, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.OSdate))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.OrderId))
            {
                return Json(JsonHandler.CreateMessage(0, "合同编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            List<View_FinOutStockItem> ListItem = ParseFromJson<List<View_FinOutStockItem>>(Products);
            if (ListItem.Count < 1)
            {
                return Json(JsonHandler.CreateMessage(0, "请添加商品"), JsonRequestBehavior.AllowGet);
            }
            for (int i = 0; i < ListItem.Count; i++)
            {
                View_FinOutStockItem item = ListItem[i];
                if (string.IsNullOrEmpty(item.SaleOrderItemId))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行 合同明细编号 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(item.ProductId))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行 商品 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(item.ProductType))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行 商品分类 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (item.OutStockCount <= 0)
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行 出库数量必须大于零"), JsonRequestBehavior.AllowGet);
                }
                //else if (item.OutStockCount > item.ItemCount - item.OSCount)
                //{
                //    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行 出库数量超出最大可出库数"), JsonRequestBehavior.AllowGet);
                //}
            }

            FinOutStockBLL bll = new FinOutStockBLL();
            if (AddType)
            {
                //创建
                model.OSId = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));
                Dictionary<string, object> tsqls = new Dictionary<string, object>();
                string sqlMain = "INSERT INTO FinOutStock(OrderId,OSId,OSdate,Uid) values('" + model.OrderId + "','" + model.OSId + "','" + model.OSdate + "','" + GetUserId() + "')";
                tsqls.Add(sqlMain, null);
                string sqlItem = "INSERT INTO FinOutStockItem([OutStockId],[SaleOrderItemId],[ProductId],[Marketprice],[Costprice],[OutStockCount]) VALUES('{0}','{1}','{2}','{3}','{4}',{5})";

                for (int i = 0; i < ListItem.Count; i++)
                {
                    View_FinOutStockItem item = ListItem[i];
                    string sqltext = string.Format(sqlItem, new object[] { model.OSId, item.SaleOrderItemId, item.ProductId, item.Marketprice, item.Costprice, item.OutStockCount });
                    tsqls.Add(sqltext, null);

                    string sqlProduct = "update FinProduct set OutCount=OutCount+" + item.OutStockCount + ",stock=stock-" + item.OutStockCount + " where Id='" + item.ProductId + "'";
                    tsqls.Add(sqlProduct, null);
                }
                if (bll.Tran(tsqls))
                {
                    int ItemCount = int.Parse(new SaleOrderBLL().GetNameStr("ItemCount", "and Id='" + model.OrderId + "'"));
                    int OSCount = int.Parse(new SaleOrderBLL().GetNameStr("OSCount", "and Id='" + model.OrderId + "'"));
                    string OutStockFlag = "";
                    if (ItemCount != 0 && OSCount == 0)
                    {
                        OutStockFlag = "000069";//未出库
                    }
                    else if (ItemCount - OSCount == 0)
                    {
                        OutStockFlag = "000070";//已出库
                    }
                    else
                    {
                        OutStockFlag = "000105";//部分出库
                    }
                    bll.Update("update SaleOrder set OutStockFlag='" + OutStockFlag + "' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "添加出库单管理:" + model.OSId, Suggestion.Succes, "出库单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加出库单管理:" + model.OSId, Suggestion.Error, "出库单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Dictionary<string, object> tsqls = bll.GetDeleteSql(model.OSId);
                string sqlMain = "update FinOutStock set OSdate='" + model.OSdate + "',OrderId='" + model.OrderId + "',Uid='" + GetUserId() + "' where OSId='" + model.OSId + "'";
                tsqls.Add(sqlMain, null);
                string sqlItem = "INSERT INTO FinOutStockItem([OutStockId],[SaleOrderItemId],[ProductId],[Marketprice],[Costprice],[OutStockCount]) VALUES('{0}','{1}','{2}','{3}','{4}',{5})";
                for (int i = 0; i < ListItem.Count; i++)
                {
                    View_FinOutStockItem item = ListItem[i];
                    string sqltext = string.Format(sqlItem, new object[] { model.OSId, item.SaleOrderItemId, item.ProductId, item.Marketprice, item.Costprice, item.OutStockCount });
                    tsqls.Add(sqltext, null);

                    string sqlProduct = "update FinProduct set OutCount=OutCount+" + item.OutStockCount + ",stock=stock-" + item.OutStockCount + " where Id='" + item.ProductId + "'";
                    tsqls.Add(sqlProduct, null);
                }
                //修改
                if (bll.Tran(tsqls))
                {
                    int ItemCount = int.Parse(new SaleOrderBLL().GetNameStr("ItemCount", "and Id='" + model.OrderId + "'"));
                    int OSCount = int.Parse(new SaleOrderBLL().GetNameStr("OSCount", "and Id='" + model.OrderId + "'"));
                    string OutStockFlag = "";
                    if (ItemCount != 0 && OSCount == 0)
                    {
                        OutStockFlag = "000069";//未出库
                    }
                    else if (ItemCount - OSCount == 0)
                    {
                        OutStockFlag = "000070";//已出库
                    }
                    else
                    {
                        OutStockFlag = "000105";//部分出库
                    }
                    bll.Update("update SaleOrder set OutStockFlag='" + OutStockFlag + "' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "修改出库单管理:" + model.OSId, Suggestion.Succes, "出库单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改出库单管理:" + model.OSId, Suggestion.Error, "出库单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinOutStock(string Id, string OrderId)
        {
            FinOutStockBLL bll = new FinOutStockBLL();
            if (bll.Delete(Id))
            {
                int ItemCount = int.Parse(new SaleOrderBLL().GetNameStr("ItemCount", "and Id='" + OrderId + "'"));
                int OSCount = int.Parse(new SaleOrderBLL().GetNameStr("OSCount", "and Id='" + OrderId + "'"));
                string OutStockFlag = "";
                if (ItemCount != 0 && OSCount == 0)
                {
                    OutStockFlag = "000069";//未出库
                }
                else if (ItemCount - OSCount == 0)
                {
                    OutStockFlag = "000070";//已出库
                }
                else
                {
                    OutStockFlag = "000105";//部分出库
                }
                //未出库
                bll.Update("update SaleOrder set OutStockFlag='" + OutStockFlag + "' where Id='" + OrderId + "'");

                LogHelper.AddLogUser(GetUserId(), "删除出库单:" + Id, Suggestion.Succes, "出库单管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除出库单:" + Id, Suggestion.Error, "出库单管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 销售单 审核设置
        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SetFlag(string setType, string Id = "")
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            string DBFlag = "";//数据库 状态字段
            string DBDate = "";//数据库 日期字段
            if (setType == "Stock")
            {
                DBFlag = "OutStockFlag";
                DBDate = "OutStockDate";
            }
            else if (setType == "Check")
            {
                DBFlag = "CheckFlag";
                DBDate = "CheckDate";
            }
            ViewBag.Id = Id;
            ViewBag.setType = setType;
            ViewBag.DBFlag = bll.GetNameStr(DBFlag, "and Id='" + Id + "'");
            ViewBag.DBDate = bll.GetNameStr(DBDate, "and Id='" + Id + "'");
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult Create_SetFlag(string setType, string Id, string Flag, string Date)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(setType))
            {
                return Json(JsonHandler.CreateMessage(0, "类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Flag))
            {
                return Json(JsonHandler.CreateMessage(0, "状态 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Date))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            string TypeName = "";
            string DBFlag = "";//数据库 状态字段
            string DBDate = "";//数据库 日期字段
            if (setType == "Stock")
            {
                TypeName = "出库";
                DBFlag = "OutStockFlag";
                DBDate = "OutStockDate";
            }
            else if (setType == "Check")
            {
                TypeName = "审核";
                DBFlag = "CheckFlag";
                DBDate = "CheckDate";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            //修改
            if (bll.Update("update SaleOrder set " + DBFlag + "='" + Flag + "'," + DBDate + "='" + Date + "' where Id='" + Id + "'") > 0)
            {
                LogHelper.AddLogUser(GetUserId(), TypeName + "单据:" + Id, Suggestion.Succes, TypeName + "设置");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), TypeName + "单据:" + Id, Suggestion.Error, TypeName + "设置");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 发票管理

        [SupportFilter]
        public ActionResult FinOrderInvoice()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("发票管理");
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
        public JsonResult GetComb_OrderInvoice(string Id)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where += "and OrderId = '" + Id + "'";
            }
            FinOrderInvoiceBLL bll = new FinOrderInvoiceBLL();
            List<EasyUIModel> result = bll.SelectAllComb(where);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetData_FinOrderInvoice(string OrderId, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(OrderId))
            {
                where += "and OrderId = '" + OrderId + "'";
            }
            FinOrderInvoiceBLL bll = new FinOrderInvoiceBLL();
            List<View_FinOrderInvoice> result = bll.SelectAll(where, pager);
            List<View_FinOrderInvoice> footer = bll.SelectAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinOrderInvoice(string Empty, FinOrderInvoice result, bool AddType = false)
        {
            FinOrderInvoiceBLL bll = new FinOrderInvoiceBLL();
            SaleOrderBLL sbll = new SaleOrderBLL();
            ViewBag.AddType = AddType;
            ViewBag.AccountId = sbll.GetNameStr("AccountId", "and Id='" + result.OrderId + "'");
            ViewBag.ItemMoney = sbll.GetNameStr("ItemMoney", "and Id='" + result.OrderId + "'");
            ViewBag.Invoicemoney = sbll.GetNameStr("Invoicemoney", "and Id='" + result.OrderId + "'");
            ViewBag.Paymentmoney = sbll.GetNameStr("Paymentmoney", "and Id='" + result.OrderId + "'");
            if (!AddType)
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.Invoicedate = DateTime.Now.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(ViewBag.Invoicemoney))
                {
                    result.Invoicemoney = decimal.Parse(ViewBag.ItemMoney) - decimal.Parse(ViewBag.Invoicemoney);
                }
                else
                {
                    result.Invoicemoney = decimal.Parse(ViewBag.ItemMoney);
                }
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinOrderInvoice(FinOrderInvoice model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.OrderId))
            {
                return Json(JsonHandler.CreateMessage(0, "订单编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.AccountId))
            {
                return Json(JsonHandler.CreateMessage(0, "账户 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Invoicedate))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Remark = string.IsNullOrEmpty(model.Remark) ? "" : model.Remark;
            FinOrderInvoiceBLL bll = new FinOrderInvoiceBLL();
            if (AddType)
            {
                ////判断订单是否出库  01  03需要判断
                //DataTable dt = bll.GetData("*", " and OrderId='" + model.OrderId + "'", "SaleOrderItem");
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        string TypeId = dt.Rows[i]["ProdectType"].ToString().Substring(0, 2);
                //        if (TypeId == "01" || TypeId == "03")
                //        {
                //            FinOutStockBLL osbll = new FinOutStockBLL();
                //            if (osbll.GetStockItemCount(" and SaleOrderItemId='" + dt.Rows[i]["ItemId"].ToStringEx() + "'") < 1)
                //            {
                //                return Json(JsonHandler.CreateMessage(0, "请先出库后，再推送开发票！"), JsonRequestBehavior.AllowGet);
                //            }
                //        }
                //    }
                //}
                //创建
                model.Id = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));
                if (bll.Insert(model) > 0)
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + model.OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Invoicemoney", "and Id='" + model.OrderId + "'"));
                    string InvoiceFlag = "000063";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        InvoiceFlag = "000064";//已开票
                    }
                    else
                    {
                        InvoiceFlag = "000065";//部分开票
                    }
                    bll.Update("update SaleOrder set InvoiceFlag='" + InvoiceFlag + "',Fp='1' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "添加发票管理:" + model.Id, Suggestion.Succes, "发票管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加发票管理:" + model.Id, Suggestion.Error, "发票管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + model.OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Invoicemoney", "and Id='" + model.OrderId + "'"));
                    string InvoiceFlag = "000063";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        InvoiceFlag = "000064";//已开票
                    }
                    else
                    {
                        InvoiceFlag = "000065";//部分开票
                    }
                    bll.Update("update SaleOrder set InvoiceFlag='" + InvoiceFlag + "' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "修改发票管理:" + model.Id, Suggestion.Succes, "发票管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改发票管理:" + model.Id, Suggestion.Error, "发票管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinOrderInvoice(string Id, string OrderId)
        {
            FinOrderInvoiceBLL bll = new FinOrderInvoiceBLL();
            if (bll.Delete(Id) > 0)
            {
                if (!bll.isExist("and OrderId='" + OrderId + "'"))
                {
                    //未开票
                    bll.Update("update SaleOrder set InvoiceFlag='000063' where Id='" + OrderId + "'");
                }
                else
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Invoicemoney", "and Id='" + OrderId + "'"));
                    string InvoiceFlag = "000063";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        InvoiceFlag = "000064";//已开票
                    }
                    else
                    {
                        InvoiceFlag = "000065";//部分开票
                    }
                    bll.Update("update SaleOrder set InvoiceFlag='" + InvoiceFlag + "' where Id='" + OrderId + "'");
                }
                LogHelper.AddLogUser(GetUserId(), "删除发票管理:" + Id, Suggestion.Succes, "发票管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除发票管理:" + Id, Suggestion.Error, "发票管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 回款管理

        [SupportFilter]
        public ActionResult FinOrderPayment()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("回款管理");
            return View();
        }

        [HttpPost]
        public JsonResult Cet_PaymentMoney(string InvoiceId)
        {
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            string Paymentmoney = bll.GetNameStr("sum(Paymentmoney)", " and InvoiceId='" + InvoiceId + "'");
            decimal money = string.IsNullOrEmpty(Paymentmoney) ? 0.00M : decimal.Parse(Paymentmoney);
            var json = new
            {
                Paymentmoney = money
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_FinOrderPayment(string OrderId, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(OrderId))
            {
                where += "and OrderId = '" + OrderId + "'";
            }
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            List<View_FinOrderPayment> result = bll.SelectAll(where, pager);
            List<View_FinOrderPayment> footer = bll.SelectAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_FinOrderPayment(string Empty, FinOrderPayment result, bool AddType = false)
        {
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            SaleOrderBLL sbll = new SaleOrderBLL();
            FinOrderInvoiceBLL ibll = new FinOrderInvoiceBLL();
            ViewBag.AddType = AddType;
            ViewBag.ItemMoney = sbll.GetNameStr("ItemMoney", "and Id='" + result.OrderId + "'");
            ViewBag.Invoicemoney = sbll.GetNameStr("Invoicemoney", "and Id='" + result.OrderId + "'");
            ViewBag.Paymentmoney = sbll.GetNameStr("Paymentmoney", "and Id='" + result.OrderId + "'");
            if (!AddType)
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.Paymentdate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_FinOrderPayment(FinOrderPayment model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.OrderId))
            {
                return Json(JsonHandler.CreateMessage(0, "订单编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Paymentaccount))
            {
                return Json(JsonHandler.CreateMessage(0, "回款账户 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Paymentdate))
            {
                return Json(JsonHandler.CreateMessage(0, "回款日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            //if (model.Paymentmoney <= 0)
            //{
            //    return Json(JsonHandler.CreateMessage(0, "回款金额不能小于等于零"), JsonRequestBehavior.AllowGet);
            //}
            model.Remark = string.IsNullOrEmpty(model.Remark) ? "" : model.Remark;
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            if (AddType)
            {
                //创建
                model.Id = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));
                if (bll.Insert(model) > 0)
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + model.OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Paymentmoney", "and Id='" + model.OrderId + "'"));
                    string PaymentFlag = "000066";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        PaymentFlag = "000067";//已开票
                    }
                    else
                    {
                        PaymentFlag = "000068";//部分开票
                    }

                    bll.Update("update SaleOrder set PaymentFlag='" + PaymentFlag + "' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "添加回款管理:" + model.Id, Suggestion.Succes, "回款管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加回款管理:" + model.Id, Suggestion.Error, "回款管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + model.OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Paymentmoney", "and Id='" + model.OrderId + "'"));
                    string PaymentFlag = "000066";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        PaymentFlag = "000067";//已开票
                    }
                    else
                    {
                        PaymentFlag = "000068";//部分开票
                    }

                    bll.Update("update SaleOrder set PaymentFlag='" + PaymentFlag + "' where Id='" + model.OrderId + "'");
                    LogHelper.AddLogUser(GetUserId(), "修改回款管理:" + model.Id, Suggestion.Succes, "回款管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改回款管理:" + model.Id, Suggestion.Error, "回款管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_FinOrderPayment(string Id, string OrderId)
        {
            FinOrderPaymentBLL bll = new FinOrderPaymentBLL();
            if (bll.Delete(Id) > 0)
            {
                if (!bll.isExist("and OrderId='" + OrderId + "'"))
                {
                    //未回款
                    bll.Update("update SaleOrder set PaymentFlag='000066' where Id='" + OrderId + "'");
                }
                else
                {
                    decimal ItemMoney = decimal.Parse(new SaleOrderBLL().GetNameStr("ItemMoney", "and Id='" + OrderId + "'"));
                    decimal Invoicemoney = decimal.Parse(new SaleOrderBLL().GetNameStr("Paymentmoney", "and Id='" + OrderId + "'"));
                    string PaymentFlag = "000066";//未开票
                    if (ItemMoney - Invoicemoney == 0)
                    {
                        PaymentFlag = "000067";//已开票
                    }
                    else
                    {
                        PaymentFlag = "000068";//部分开票
                    }
                    //未回款
                    bll.Update("update SaleOrder set PaymentFlag='" + PaymentFlag + "' where Id='" + OrderId + "'");
                }
                LogHelper.AddLogUser(GetUserId(), "删除回款管理:" + Id, Suggestion.Succes, "回款管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除回款管理:" + Id, Suggestion.Error, "回款管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 提成管理
        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SaleOrderTC(string Id)
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("提成管理");
            SaleOrderBLL bll = new SaleOrderBLL();
            View_SaleOrder result = bll.GetRow(Id);
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Set_SaleOrderTC(string ids, bool TcFlag)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            string tsql = "";
            string text = "";

            string allid = "";
            string[] aryid = ids.TrimEnd(',').Split(',');
            foreach (var item in aryid)
            {
                allid += "'" + item + "',";
            }

            if (TcFlag)
            {
                tsql = "update SaleOrderItem set TcFlag='000107',TcDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where ItemId in (" + allid.TrimEnd(',') + ")";
                text = "订单已提：";
            }
            else
            {
                tsql = "update SaleOrderItem set TcFlag='000106',TcDate='' where ItemId in (" + allid.TrimEnd(',') + ")";
                text = "订单未提：";
            }
            if (bll.Update(tsql) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), text + ids, Suggestion.Succes, "提成管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), text + ids, Suggestion.Error, "提成管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}

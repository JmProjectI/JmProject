using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Common;
using JMProject.BLL;
using JMProject.Model;
using JMProject.Model.Sys;
using JMProject.Model.Esayui;
using JMProject.Model.Mobile;
using System.Text;
using JMProject.Model.View;

namespace JMProject.Web.Controllers
{
    public class MobileController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult SaleOrder()
        {
            return View();
        }

        public ActionResult SaleVisit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCustom(string Name, GridPager pager)
        {
            string roleId = GetUserRoleID();
            if (string.IsNullOrEmpty(roleId))
            {
                var json = new
                {
                    total = -1
                };
                return Json(json);
            }
            else
            {
                string where = "";
                if (!string.IsNullOrEmpty(Name.Trim()))
                {
                    where += "and Name like '%" + Name.Trim() + "%'";
                }

                //03 业务员
                if (roleId == "03")
                {
                    where += "and Ywy like '%" + GetUserId() + "%'";
                }

                SaleCustomerBLL bll = new SaleCustomerBLL();
                List<S_SCustom> result = bll.SelectAll_Phone(where, pager);
                var json = new
                {
                    total = pager.totalRows,
                    rows = result
                };
                return Json(json);
            }
        }

        #region 创建客户
        public ActionResult CreateCustomer(string Id = "")
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();
            View_SaleCustom_Mobile result = new View_SaleCustom_Mobile();
            if (!string.IsNullOrEmpty(Id))
            {
                result.ID = Id;
                result = bll.GetRowMobile("and Id='" + Id + "'");
            }
            else
            {
                result.Ywy = "";
                result.CzjName = "省";
                result.CDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return View(result);
        }

        [HttpPost]
        public JsonResult Create_Customer(View_SaleCustom_Mobile model_m)
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();

            if (string.IsNullOrEmpty(model_m.CDate))
            {
                return Json(JsonHandler.CreateMessage(0, "创建日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.Ywy))
            {
                return Json(JsonHandler.CreateMessage(0, "业务员 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "客户全称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (string.IsNullOrEmpty(model_m.ID))
                {
                    if (bll.isExist("and Name='" + model_m.Name + "'"))
                    {
                        return Json(JsonHandler.CreateMessage(0, "客户名称已存在"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (string.IsNullOrEmpty(model_m.Lxr))
            {
                return Json(JsonHandler.CreateMessage(0, "联系人 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.Phone))
            {
                return Json(JsonHandler.CreateMessage(0, "联系方式 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(model_m.UserName))
            {
                if (string.IsNullOrEmpty(model_m.UserPwd))
                {
                    return Json(JsonHandler.CreateMessage(0, "密码 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                else if (bll.Verification(model_m.UserName, model_m.ID))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户名 有重复"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model_m.UserPwd))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户名 不允许为空"), JsonRequestBehavior.AllowGet);
                }
            }
            if (string.IsNullOrEmpty(model_m.CzjName))
            {
                return Json(JsonHandler.CreateMessage(0, "财政局 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.QyName))
            {
                return Json(JsonHandler.CreateMessage(0, "所属区域 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            SaleCustom model = new SaleCustom();
            model.ID = model_m.ID;
            model.CDate = Convert.ToDateTime(model_m.CDate).ToString("yyyy-MM-dd");
            model.Ywy = model_m.Ywy;
            model.YwyName = model_m.YwyName;
            model.Name = model_m.Name;
            model.Lxr = model_m.Lxr;
            model.Phone = model_m.Phone;
            model.UserName = model_m.UserName;
            model.UserPwd = model_m.UserPwd;
            DictionaryBLL dicbll = new DictionaryBLL();
            model.Finance = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.CzjName + "'");
            string[] qys = model_m.QyName.Split(' ');
            model.Province = new BasicProvinceBLL().GetNameStr("Pid", "and Name='" + qys[0] + "'");
            model.UpID = new BasicCityPLBLL().GetNameStr("ID", "and Name='" + qys[1] + "'");
            model.Region = new BasicCityBLL().GetNameStr("ID", "and Name='" + qys[2] + "'");
            model.Remark = string.IsNullOrEmpty(model_m.Remark) ? "" : model_m.Remark;
            model.Uid = GetUserId();
            model.Address = "";
            if (string.IsNullOrEmpty(model.Finance))
            {
                return Json(JsonHandler.CreateMessage(0, "财政局 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Province))
            {
                return Json(JsonHandler.CreateMessage(0, "省份 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Region))
            {
                return Json(JsonHandler.CreateMessage(0, "地区 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.UpID))
            {
                return Json(JsonHandler.CreateMessage(0, "上级主管区域 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            //发票抬头 等于 单位名称
            model.Invoice = model.Name;
            if (string.IsNullOrEmpty(model.ID))
            {
                //创建
                model.ID = bll.MaxId(DateTime.Now.ToString("yyyyMMdd"));
                string CusID = "";
                CusID = bll.Insert(model).ToStringEx();
                if (!string.IsNullOrEmpty(CusID))
                {
                    LogHelper.AddLogUser(GetUserId(), "添加客户管理:" + model.ID, Suggestion.Succes, "客户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes, CusID), CusID, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加客户管理:" + model.ID, Suggestion.Error, "客户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error, ""), "", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                StringBuilder sb = new StringBuilder();
                sb.Append("update SaleCustom set ");
                sb.Append("CDate='" + model.CDate + "'");
                sb.Append(",Ywy='" + model.Ywy + "'");
                sb.Append(",YwyName='" + model.YwyName + "'");
                sb.Append(",Name='" + model.Name + "'");
                sb.Append(",Lxr='" + model.Lxr + "'");
                sb.Append(",Phone='" + model.Phone + "'");
                sb.Append(",UserName='" + model.UserName + "'");
                sb.Append(",UserPwd='" + model.UserPwd + "'");
                sb.Append(",Finance='" + model.Finance + "'");
                sb.Append(",Province='" + model.Province + "'");
                sb.Append(",UpID='" + model.UpID + "'");
                sb.Append(",Region='" + model.Region + "'");
                sb.Append(",Remark='" + model.Remark + "'");
                sb.Append(",Uid='" + GetUserId() + "'");
                sb.Append(" where ID='" + model.ID + "'");
                if (bll.Update(sb.ToString()) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改客户管理:" + model.ID, Suggestion.Succes, "客户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes, model.ID), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改客户管理:" + model.ID, Suggestion.Error, "客户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error, ""), JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region 创建合同
        public ActionResult CreateOrder(string Id = "", string CId = "")
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();
            View_SaleOrder_Mobile result = new View_SaleOrder_Mobile();
            if (!string.IsNullOrEmpty(CId))
            {
                result.Id = "";
                result.SaleCustomId = CId;
                result.Name = bll.GetNameStr("Name", "and ID='" + CId + "'");
                result.Saler = bll.GetNameStr("Ywy", "and ID='" + CId + "'"); ;
                result.OrderDate = DateTime.Now.ToString("yyyy-MM-dd");
                result.OrderTypeName = "新购";
                result.ItemNames = "内控手册编制";
                result.Remake = "";
            }
            else
            {

            }
            return View(result);
        }

        [HttpPost]
        public JsonResult Create_SaleOrder(View_SaleOrder_Mobile model_m)
        {
            if (string.IsNullOrEmpty(model_m.OrderDate))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.SaleCustomId))
            {
                return Json(JsonHandler.CreateMessage(0, "客户 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.Saler))
            {
                return Json(JsonHandler.CreateMessage(0, "业务员 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.OrderType))
            {
                return Json(JsonHandler.CreateMessage(0, "类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.ItemNames))
            {
                return Json(JsonHandler.CreateMessage(0, "合同明细 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.ItemCount))
            {
                return Json(JsonHandler.CreateMessage(0, "数量 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model_m.PresentMoney))
            {
                model_m.PresentMoney = "0";
            }
            if (string.IsNullOrEmpty(model_m.OtherMoney))
            {
                model_m.OtherMoney = "0";
            }

            //合同主表
            SaleOrder model = new SaleOrder();
            //model.Id
            model.OrderDate = Convert.ToDateTime(model_m.OrderDate).ToString("yyyy-MM-dd");
            model.SaleCustomId = model_m.SaleCustomId;
            model.Saler = model_m.Saler;
            DictionaryBLL dicbll = new DictionaryBLL();
            model.OrderType = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.OrderType + "'");
            if (string.IsNullOrEmpty(model.OrderType))
            {
                return Json(JsonHandler.CreateMessage(0, "类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.UserId = GetUserId();
            //合同明细表
            List<SaleOrderItem> ListItem = new List<SaleOrderItem>();
            SaleOrderItem item1 = new SaleOrderItem();
            item1.OrderId = "";
            item1.ItemId = "";
            item1.ProdectType = new FinProductTypeBLL().GetNameStr("Id", "and Name='" + model_m.ItemNames + "'");
            item1.ProdectDesc = model_m.ItemNames;
            item1.ItemCount = int.Parse(model_m.ItemCount);
            item1.ItemPrice = decimal.Parse(model_m.ItemPrice);
            item1.ItemMoney = item1.ItemCount * item1.ItemPrice;
            item1.TaxMoney = item1.ItemMoney * 0.05M;
            item1.PresentMoney = decimal.Parse(model_m.PresentMoney);
            item1.OtherMoney = decimal.Parse(model_m.OtherMoney);
            item1.ValidMoney = 0.00M;
            item1.TcFlag = "000106";
            item1.TcDate = "";
            item1.Service = "是";
            item1.SerDateS = DateTime.Now.ToString("yyyy-MM-dd");
            item1.SerDateE = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(item1.ProdectType))
            {
                return Json(JsonHandler.CreateMessage(0, "商品分类 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (item1.ItemCount <= 0)
            {
                return Json(JsonHandler.CreateMessage(0, "数量必须大于零"), JsonRequestBehavior.AllowGet);
            }

            ListItem.Add(item1);

            SaleOrderBLL bll = new SaleOrderBLL();
            if (string.IsNullOrEmpty(model.Id))
            {
                //是否创建内控手册
                bool isCreateNksc = false;
                //是否更新手册
                bool isUpdateNksc = false;
                FinProductTypeBLL bllproType = new FinProductTypeBLL();

                model.InvoiceFlag = "000063";//未开票
                model.PaymentFlag = "000066";//未回款
                model.OutStockFlag = "000069";//未出库
                model.CheckFlag = "000071";//未审核
                model.CheckDate = "";
                model.Finshed = false;
                model.AccountId = "null";
                model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
                model.Enclosure = string.IsNullOrEmpty(model.Enclosure) ? "" : model.Enclosure;

                //创建
                model.Id = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));

                Dictionary<string, object> tsqls = new Dictionary<string, object>();
                string sqlMain = "INSERT INTO SaleOrder(Id,OrderDate,SaleCustomId,Saler,InvoiceFlag,PaymentFlag,OutStockFlag,CheckFlag,CheckDate,Finshed,Remake,UserId,AccountId,OrderType,Enclosure) values('"
                    + model.Id + "','" + model.OrderDate + "','" + model.SaleCustomId + "','" + model.Saler + "','" + model.InvoiceFlag + "','" + model.PaymentFlag + "','"
                    + model.OutStockFlag + "','" + model.CheckFlag + "','" + model.CheckDate + "','" + model.Finshed + "','" + model.Remake + "','" + model.UserId + "'," + model.AccountId + ",'" + model.OrderType + "','" + model.Enclosure + "')";
                tsqls.Add(sqlMain, null);
                string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
                    + " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')";

                for (int i = 0; i < ListItem.Count; i++)
                {
                    SaleOrderItem item = ListItem[i];
                    if (!isCreateNksc && "0201" == item.ProdectType.ToStringEx())
                    {
                        isCreateNksc = true;
                    }
                    if (!isUpdateNksc && "0204" == item.ProdectType.ToStringEx())
                    {
                        isUpdateNksc = true;
                    }

                    item.TcDate = string.IsNullOrEmpty(item.TcDate) ? "" : item.TcDate;

                    item.ItemId = model.Id + (i + 1).ToString("00");
                    string sqltext = string.Format(sqlItem, new object[] { model.Id, item.ItemId, item.ProdectType,item.ProdectDesc, item.ItemCount, item.ItemPrice
                                   , item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate });
                    tsqls.Add(sqltext, null);
                }
                if (bll.Tran(tsqls))
                {
                    if (isCreateNksc)
                    {
                        NkscBLL nkbll = new NkscBLL();
                        //内控手册
                        Nksc nksc = new Nksc();
                        nksc.SaleOrderID = model.Id;//合同编号
                        nksc.CustomerID = model.SaleCustomId;//客户编号
                        nksc.dwqc = new SaleCustomerBLL().GetNameStr("Name", "and id='" + model.SaleCustomId + "'");
                        nksc.NkscDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        nksc.version = nkbll.GetMaxVersion();
                        nkbll.Insert(nksc);
                    }
                    if (isUpdateNksc)
                    {
                        NkscBLL nkbll = new NkscBLL();
                        //更新手册
                        Nksc_Update nkup = new Nksc_Update();
                        nkup.CustomerID = model.SaleCustomId;
                        nkup.NkscDate = DateTime.Now.ToString("yyyy-MM-dd");
                        nkup.versionS = nkbll.GetNameStr("version", "and CustomerID='" + model.SaleCustomId + "'");
                        nkup.versionE = nkbll.GetMaxVersion();
                        nkup.UpdateFlag = "0";
                        new Nksc_UpdateBLL().Insert(nkup);
                    }
                    LogHelper.AddLogUser(GetUserId(), "添加订单管理:" + model.Id, Suggestion.Succes, "订单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加订单管理:" + model.Id, Suggestion.Error, "订单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                model.InvoiceFlag = bll.GetNameStr("InvoiceFlag", "and Id='" + model.Id + "'");//开票
                model.PaymentFlag = bll.GetNameStr("PaymentFlag", "and Id='" + model.Id + "'");//回款
                model.OutStockFlag = bll.GetNameStr("OutStockFlag", "and Id='" + model.Id + "'");//出库
                model.CheckFlag = bll.GetNameStr("CheckFlag", "and Id='" + model.Id + "'");//审核
                model.CheckDate = bll.GetNameStr("CheckDate", "and Id='" + model.Id + "'");
                model.Finshed = bool.Parse(bll.GetNameStr("Finshed", "and Id='" + model.Id + "'"));

                Dictionary<string, object> tsqls = bll.GetDeleteSql(model.Id);
                string sqlMain = "update SaleOrder set OrderDate='" + model.OrderDate + "',SaleCustomId='" + model.SaleCustomId + "',Saler='" + model.Saler + "',InvoiceFlag='" + model.InvoiceFlag + "',PaymentFlag='"
                    + model.PaymentFlag + "',OutStockFlag='" + model.OutStockFlag + "',CheckFlag='" + model.CheckFlag + "',CheckDate='"
                    + model.CheckDate + "',Finshed='" + model.Finshed + "',Remake='" + model.Remake + "',UserId='" + model.UserId + "',AccountId=" + model.AccountId + ",OrderType='" + model.OrderType + "',Enclosure='" + model.Enclosure + "' where Id= '" + model.Id + "'";
                tsqls.Add(sqlMain, null);
                string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
                    + " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')";

                for (int i = 0; i < ListItem.Count; i++)
                {
                    SaleOrderItem item = ListItem[i];
                    item.ItemId = model.Id + (i + 1).ToString("00");
                    string sqltext = string.Format(sqlItem, new object[] { model.Id, item.ItemId, item.ProdectType,item.ProdectDesc, item.ItemCount, item.ItemPrice
                                   , item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate });
                    tsqls.Add(sqltext, null);
                }

                //修改
                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "修改订单管理:" + model.Id, Suggestion.Succes, "订单管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改订单管理:" + model.Id, Suggestion.Error, "订单管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region 拜访沟通
        [HttpPost]
        public ContentResult GetYiJian(string Id)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Content("已超时，请重新登陆");
            }
            else
            {
                //已读
                string sql = "update SaleVisit set AuditState='000103' where Id='" + Id + "' and Ywy='" + userId + "'";
                bll.Update(sql);
                string yijian = bll.GetStrName("AuditDetails", "where Id='" + Id + "'");
                yijian = string.IsNullOrEmpty(yijian) ? "无" : yijian;
                return Content(yijian);
            }
        }


        [HttpPost]
        public JsonResult GetVisit(string Name, GridPager pager)
        {
            string roleId = GetUserRoleID();
            if (string.IsNullOrEmpty(roleId))
            {
                var json = new
                {
                    total = -1
                };
                return Json(json);
            }
            else
            {
                string where = "";
                if (!string.IsNullOrEmpty(Name.Trim()))
                {
                    where += "and Name like '%" + Name.Trim() + "%'";
                }

                //03 业务员
                if (roleId == "03")
                {
                    where += "and Ywy like '%" + GetUserId() + "%'";
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
        }

        public ActionResult CreateVisit(string Id = "", string CId = "")
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();
            SaleVisitBLL bllvis = new SaleVisitBLL();
            View_SaleVisit result = new View_SaleVisit();

            ViewBag.viewbagXQ = new DictionaryBLL().SelectAll("and DicID='026'", new GridPager { page = 1, rows = 100, sort = "ItemID", order = "asc" });

            if (!string.IsNullOrEmpty(CId) && string.IsNullOrEmpty(Id))
            {
                result.Id = "";
                result.SaleCustomID = CId;
                result.Name = bll.GetNameStr("Name", "and ID='" + CId + "'");
                result.Ywy = bll.GetNameStr("Ywy", "and ID='" + CId + "'");
                result.ContactDate = DateTime.Now.ToString("yyyy-MM-dd");
                result.ContactTime = DateTime.Now.ToString("HH:mm");
                result.LlfsName = "上门";//联络方式
                result.DemandType = "";//需求
                result.ZtName = "新建";
                result.NextTime = DateTime.Now.ToString("yyyy-MM-dd");
                //result.OrderTypeName = "新购";
                //result.ItemNames = "内控手册编制";
                //result.Remake = "";
            }
            else if (!string.IsNullOrEmpty(Id))
            {
                result = bllvis.GetRow(Id);
            }
            return View(result);
        }

        [HttpPost]
        public JsonResult Create_SaleVisit(View_SaleVisit model_m)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            SaleVisit model = new SaleVisit();
            model.Id = model_m.Id;
            model.SaleCustomID = model_m.SaleCustomID;
            model.DemandType = model_m.DemandType;
            model.ContactDate = Convert.ToDateTime(model_m.ContactDate).ToString("yyyy-MM-dd");
            model.ContactTime = Convert.ToDateTime(model_m.ContactDate).ToString("HH:mm");
            DictionaryBLL dicbll = new DictionaryBLL();
            model.Intention = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.YxName + "'");
            model.ContactMode = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.LlfsName + "'");
            model.ContactFlag = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.LlztName + "'");
            model.ContactSituation = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.LlqkName + "'");
            model.Progress = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.JdName + "'");
            model.ContactDetails = model_m.ContactDetails;
            model.ContactTarget = model_m.ContactTarget;
            model.NextTime = model_m.NextTime;
            model.Amount = model_m.Amount;
            model.Offer = model_m.Offer;
            model.Flag = dicbll.GetNameStr("ItemID", "and ItemName='" + model_m.ZtName + "'");

            if (string.IsNullOrEmpty(model.SaleCustomID))
            {
                return Json(JsonHandler.CreateMessage(0, "客户名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.DemandType))
            {
                return Json(JsonHandler.CreateMessage(0, "需求分类 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactDate))
            {
                return Json(JsonHandler.CreateMessage(0, "联络日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactTime))
            {
                return Json(JsonHandler.CreateMessage(0, "联络时间 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Intention))
            {
                return Json(JsonHandler.CreateMessage(0, "是否意向 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactFlag))
            {
                return Json(JsonHandler.CreateMessage(0, "联络状态 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactSituation))
            {
                return Json(JsonHandler.CreateMessage(0, "联络情况 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Progress))
            {
                return Json(JsonHandler.CreateMessage(0, "进度 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactMode))
            {
                return Json(JsonHandler.CreateMessage(0, "联络方式 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactDetails))
            {
                return Json(JsonHandler.CreateMessage(0, "联络详情 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.NextTime))
            {
                return Json(JsonHandler.CreateMessage(0, "下次联络时间 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ContactTarget))
            {
                return Json(JsonHandler.CreateMessage(0, "下次联络目标 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Flag))
            {
                return Json(JsonHandler.CreateMessage(0, "状态 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Id))
            {
                model.AuditDate = "";
                model.AuditDetails = "";
                //创建
                model.Id = bll.MaxId();
                model.Ywy = GetUserId();
                model.SaleOrderID = "";
                model.Fj = "";
                model.NetContactTime = "000094";
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加拜访沟通:" + model.Id, Suggestion.Succes, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加拜访沟通:" + model.Id, Suggestion.Error, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                model.SaleOrderID = bll.GetStrName("SaleOrderID", " where Id='" + model.Id + "'");
                model.Ywy = bll.GetStrName("Ywy", " where Id='" + model.Id + "'");
                model.NetContactTime = bll.GetStrName("NetContactTime", " where Id='" + model.Id + "'");
                model.AuditDate = bll.GetStrName("AuditDate", " where Id='" + model.Id + "'");
                model.AuditDetails = bll.GetStrName("AuditDetails", " where Id='" + model.Id + "'");
                model.Auditor = bll.GetStrName("Auditor", " where Id='" + model.Id + "'");
                model.AuditState = bll.GetStrName("AuditState", " where Id='" + model.Id + "'");
                model.Fj = bll.GetStrName("Fj", " where Id='" + model.Id + "'");
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改拜访沟通:" + model.Id, Suggestion.Succes, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改拜访沟通:" + model.Id, Suggestion.Error, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        [HttpPost]
        public JsonResult Get_FinProductTypeLast()
        {
            FinProductTypeBLL bll = new FinProductTypeBLL();
            List<String> list = bll.getLastName();
            return Json(list);
        }
    }
}

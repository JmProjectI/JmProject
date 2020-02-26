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
using System.IO;
using Microsoft.International.Converters.PinYinConverter;
using Aspose.Words;
using Aspose.Words.Saving;
using System.Drawing;
using System.Text;
using O2S.Components.PDFRender4NET;
using System.Security.Cryptography;
using System.Net;

namespace JMProject.Web.Controllers
{
    public class SaleController : BaseController
    {
        public ActionResult CopyFile()
        {
            NkscBLL bll = new NkscBLL();
            DataTable dt = bll.Select("select dwqc,swfName from Nksc where flag='4' and swfName<>''");
            string path = Server.MapPath("\\UserPDF\\");
            string pathn = Server.MapPath("\\CopyFile\\");
            if (!Directory.Exists(Server.MapPath("\\CopyFile\\")))
            {
                Directory.CreateDirectory(Server.MapPath("\\CopyFile\\"));
            }
            foreach (DataRow item in dt.Rows)
            {
                string filename = item["swfName"].ToString();
                if (!string.IsNullOrEmpty(filename))
                {
                    if (System.IO.File.Exists(path + filename + ".docx"))
                    {
                        System.IO.File.Copy(path + filename + ".docx", pathn + filename + ".docx");
                    }
                }
            }
            return Content("成功");
        }

        #region 迅速建档
        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_CustomerSimple()
        {
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_CustomerSimple(string CDate
            , string Ywy
            , string Name
            , string Lxr
            , string Phone
            , string Industry
            , string UpID
            , string Province
            , string Xydj
            , string QQ
            , string Address
            , string Region
            , string Code
            , string Invoice
            , string UserName
            , string UserPwd
            , string Finance
            , string YwyName
            , decimal HTMoney
            , string HTItem, string AccountId)
        {
            SaleCustomerBLL cusbll = new SaleCustomerBLL();
            SaleOrderBLL bll = new SaleOrderBLL();

            string date = DateTime.Now.ToString("yyyyMMdd");
            string uid = GetUserId();

            List<SaleCustom> lis_customs = new List<SaleCustom>();

            SaleCustom scustom = new SaleCustom();
            scustom.ID = cusbll.MaxId(date);
            scustom.CDate = CDate;
            scustom.Ywy = Ywy;
            scustom.Name = Name;
            scustom.Lxr = Lxr;
            scustom.Phone = Phone;
            scustom.Industry = Industry;
            scustom.UpID = UpID;
            scustom.Province = Province;
            scustom.Xydj = Xydj;
            scustom.QQ = QQ;
            scustom.Address = Address;
            scustom.Region = Region;
            scustom.Code = Code;
            if (string.IsNullOrEmpty(Invoice))
            {
                scustom.Invoice = scustom.Name;
            }
            else
            {
                scustom.Invoice = Invoice;
            }
            scustom.UserName = UserName;
            scustom.UserPwd = UserPwd;
            scustom.Finance = Finance;
            scustom.YwyName = YwyName;
            scustom.Uid = uid;

            lis_customs.Add(scustom);


            List<S_Order> lis_orders = new List<S_Order>();
            S_Order odMain = new S_Order();

            odMain.KhName = Name;

            odMain.OrderMain.OrderDate = scustom.CDate;//订单日期
            odMain.OrderMain.Id = bll.Maxid(date);//订单编号
            odMain.OrderMain.SaleCustomId = scustom.ID;//客户
            odMain.OrderMain.Saler = scustom.Ywy;//业务员
            odMain.OrderMain.Fp = "0";//是否立即开票
            odMain.OrderMain.AccountId = AccountId;//账户
            odMain.OrderMain.UserId = uid;//操作员
            odMain.OrderMain.Remake = "";
            lis_orders.Add(odMain);

            decimal bgmoney = 0.00m;//报告金额
            if (HTItem.IndexOf("0203") >= 0)
            {
                bgmoney = 2000.00m;
            }
            string[] ItemNames = HTItem.Split(',');
            for (int i = 0; i < ItemNames.Length; i++)
            {
                SaleOrderItem OmxModel = new SaleOrderItem();
                OmxModel.OrderId = odMain.OrderMain.Id;
                OmxModel.ItemId = odMain.OrderMain.Id + (i + 1).ToString("00");

                OmxModel.ProdectType = ItemNames[i];
                if (OmxModel.ProdectType.StartsWith("0201"))
                {
                    OmxModel.ProdectDesc = "手册";
                    OmxModel.ItemPrice = HTMoney - bgmoney;
                }
                else if (OmxModel.ProdectType.StartsWith("0203"))
                {
                    OmxModel.ProdectDesc = "报告";
                    OmxModel.ItemPrice = bgmoney;
                }
                //else if (OmxModel.ProdectType.StartsWith("0204"))
                //{
                //    OmxModel.ProdectDesc = "更新";
                //    OmxModel.ItemPrice = HTMoney - bgmoney;
                //}
                OmxModel.ItemCount = 1;

                //成交金额=单价*数量
                OmxModel.ItemMoney = OmxModel.ItemCount * OmxModel.ItemPrice;
                //税金
                OmxModel.TaxMoney = OmxModel.ItemMoney * 0.05M;
                //礼品礼金
                OmxModel.PresentMoney = 0.00M;
                //其他金额
                OmxModel.OtherMoney = 0.00M;

                //是否包含服务、服务开始日期、服务结束日期
                if (OmxModel.ProdectType.StartsWith("0201") || OmxModel.ProdectType.StartsWith("0204"))
                {
                    OmxModel.Service = "是";
                    OmxModel.SerDateS = DateTime.Now.ToString("yyyy-MM-dd");
                    OmxModel.SerDateE = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                }
                odMain.OrderItems.Add(OmxModel);

            }

            Dictionary<string, object> tsql = create_tsqls(lis_orders, lis_customs, "");

            try
            {
                //修改
                if (bll.Tran(tsql))
                {
                    LogHelper.AddLogUser(GetUserId(), "内控速建:" + scustom.ID, Suggestion.Succes, "客户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes, ""), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "内控速建:" + scustom.ID, Suggestion.Error, "客户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error, ""), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message, ""), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Get_CustomerInfo(string Name)
        {
            //string where = "Pid=ID";
            string where = " and Pid<>ID";
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "ID", order = "asc" };
            BasicCityBLL bll = new BasicCityBLL();
            IList<View_BasicCity> list = bll.SelectAll(where, pager);

            string Level1 = "";
            string Level2 = "";
            string Level3 = "";
            string czj = "";

            foreach (var item in list)
            {
                if (Name.Contains(item.Name))
                {
                    Level1 = item.Sfid;
                    Level2 = item.Pid;
                    Level3 = item.ID;
                    string czjname = item.Name.Substring(item.Name.Length - 1);
                    if ("省" == czjname)
                    {
                        czj = "000040";
                    }
                    else if ("市" == czjname)
                    {
                        czj = "000060";
                    }
                    else if ("县" == czjname)
                    {
                        czj = "000061";
                    }
                    else if ("区" == czjname)
                    {
                        czj = "000062";
                    }
                    break;
                }
            }

            if (Level3 == "")
            {
                where = " and Pid=ID";
                list = bll.SelectAll(where, pager);
                foreach (var item in list)
                {
                    if (Name.Contains(item.Name))
                    {
                        Level1 = item.Sfid;
                        Level2 = item.Pid;
                        Level3 = item.ID;
                        string czjname = item.Name.Substring(item.Name.Length - 1);
                        if ("省" == czjname)
                        {
                            czj = "000040";
                        }
                        else if ("市" == czjname)
                        {
                            czj = "000060";
                        }
                        else if ("县" == czjname)
                        {
                            czj = "000061";
                        }
                        else if ("区" == czjname)
                        {
                            czj = "000062";
                        }
                        break;
                    }
                }
            }

            string UP = SpellHelper.GetSpellCode(Name);

            var json = new
            {
                Level1 = Level1,//省
                Level2 = Level2,//市
                Level3 = Level3,//县
                czj = czj,//财政局
                UP = UP//用户密码简拼
            };
            return Json(json);
        }
        #endregion

        #region 客户管理

        [SupportFilter]
        public ActionResult SaleCustomer()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("客户管理");

            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            //03 业务员
            if (roleId == "03")
            {
                ViewBag.userS = GetUserId();
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetData_SaleCustom_Model(string ID)
        {
            string where = "and ID='" + ID + "'";
            SaleCustomerBLL bll = new SaleCustomerBLL();
            View_SaleCustom model = bll.GetRow(where);
            return Json(model);
        }

        public ActionResult GetData_SaleCustom_Comb()
        {
            GridPager pager = new GridPager { page = 1, rows = 3000, sort = "Id", order = "asc" };
            string where = "";
            SaleCustomerBLL bll = new SaleCustomerBLL();
            IList<SaleCustom> list = bll.SelectAllComb(where, pager);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetData_SaleCustomer(string Name, string DiQuS, string userS, string dengjS, string Industry, string zyxS, GridPager pager, string Vyear = null)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                where += "and Name like '%" + Name.Trim() + "%'";
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
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy like '%" + userS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(dengjS.Trim()))
            {
                where += "and CustomerGrade = '" + dengjS.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(Industry.Trim()))
            {
                where += "and Industry = '" + Industry.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(zyxS.Trim()))
            {
                where += "and Zyx = '" + zyxS.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(Vyear))
            {
                where += "and ID not in (select CustomID from CrsVisit where Vyear='" + Vyear + "')";
            }
            SaleCustomerBLL bll = new SaleCustomerBLL();
            List<View_SaleCustom> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_Customer(string Id = "", bool Copy = false)
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();
            SaleCustom result = new SaleCustom();
            if (!string.IsNullOrEmpty(Id))
            {
                GridPager pager = new GridPager { page = 1, rows = 1000, sort = "SaleCustomId", order = "asc" };
                List<ServiceTime> res = bll.SelectService(" and SaleCustomId='" + Id + "' and Service='是'", pager);
                ViewBag.Services = res;
                result.ID = Id;
                result = bll.GetRow(result);
                if (Copy)
                {
                    result.ID = "";
                    result.Name = "";
                    result.Lxr = "";
                    result.Phone = "";
                    result.Invoice = "";
                    result.Code = "";
                    result.UserName = "";
                    result.UserPwd = "";
                    result.Address = "";
                    result.QtLxr = "";
                    result.QtTel = "";
                    result.QQ = "";
                    result.Uid = GetUserId();
                }
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_Customer(SaleCustom model)
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();

            if (string.IsNullOrEmpty(model.CDate))
            {
                return Json(JsonHandler.CreateMessage(0, "创建日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "客户全称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (string.IsNullOrEmpty(model.ID))
                {
                    if (bll.isExist("and Name='" + model.Name + "'"))
                    {
                        return Json(JsonHandler.CreateMessage(0, "客户名称已存在"), JsonRequestBehavior.AllowGet);
                    }
                }
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
            if (string.IsNullOrEmpty(model.Finance))
            {
                return Json(JsonHandler.CreateMessage(0, "财政局 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Lxr))
            {
                return Json(JsonHandler.CreateMessage(0, "主要联系人 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                return Json(JsonHandler.CreateMessage(0, "手机 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                if (string.IsNullOrEmpty(model.UserPwd))
                {
                    return Json(JsonHandler.CreateMessage(0, "密码 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                else if (bll.Verification(model.UserName, model.ID))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户名 有重复"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.UserPwd))
                {
                    return Json(JsonHandler.CreateMessage(0, "用户名 不允许为空"), JsonRequestBehavior.AllowGet);
                }
            }
            //发票抬头 等于 单位名称
            if (string.IsNullOrEmpty(model.Invoice))
            {
                model.Invoice = model.Name;
            }
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
                if (bll.Update(model) > 0)
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

        public JsonResult Delete_SaleCustomer(string Id)
        {
            SaleCustomerBLL bll = new SaleCustomerBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除客户管理:" + Id, Suggestion.Succes, "客户管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除客户管理:" + Id, Suggestion.Error, "客户管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region 计划

        [SupportFilter]
        public ActionResult SalePlan()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("计划管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_SalePlan(string Name, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name))
            {
                where += "and Year = '" + Name + "'";
            }
            SalePlanBLL bll = new SalePlanBLL();
            List<View_SalePlan> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SalePlan(string Empty, SalePlan result, bool AddType = false)
        {
            SalePlanBLL bll = new SalePlanBLL();
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
        public JsonResult Create_SalePlan(SalePlan model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Year))
            {
                return Json(JsonHandler.CreateMessage(0, "年度 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Saler))
            {
                return Json(JsonHandler.CreateMessage(0, "业务员 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            SalePlanBLL bll = new SalePlanBLL();
            model.Year = string.IsNullOrEmpty(model.Year) ? "" : model.Year;
            model.Saler = string.IsNullOrEmpty(model.Saler) ? "" : model.Saler;
            if (AddType)
            {
                if (bll.isExist("and Year='" + model.Year + "' and Saler='" + model.Saler + "'"))
                {
                    return Json(JsonHandler.CreateMessage(0, "该业务员" + model.Year + "年已有计划，无法创建"), JsonRequestBehavior.AllowGet);
                }
                //创建
                model.Id = bll.Maxid(model.Year.ToString());
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加计划管理:" + model.Id, Suggestion.Succes, "计划管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加计划管理:" + model.Id, Suggestion.Error, "计划管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改计划管理:" + model.Id, Suggestion.Succes, "计划管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改计划管理:" + model.Id, Suggestion.Error, "计划管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SalePlan(string Id)
        {
            SalePlanBLL bll = new SalePlanBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除计划管理:" + Id, Suggestion.Succes, "计划管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除计划管理:" + Id, Suggestion.Error, "计划管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 订单管理

        [SupportFilter]//跳转界面
        public ActionResult SaleOrder()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("订单管理");
            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            //03 业务员
            if (roleId == "03")
            {
                ViewBag.userS = GetUserId();
            }
            //06 技术员
            if (roleId == "06")
            {
                ViewBag.userS = GetUserId();
            }
            return View();
        }

        [HttpPost]
        public JsonResult Get_OrderToOutStock(string Id)
        {
            FinOutStockBLL bll = new FinOutStockBLL();
            int count = bll.GetStockItemCount(" and SaleOrderItemId='" + Id + "'");
            if (count > 0)
            {
                return Json(JsonHandler.CreateMessage(1, "此商品已出库，无法删除当前商品！"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetData_SaleOrder(string Id, string SaleCustomId, string NameS, string OrderDateS, string OrderDateE
            , string DiQuS, string userS, string dzS, string fpS, string ckS, string ItemNames, string nkscflag, string Tc
            , string ItemMoneyS, string ItemMoneyE, string Industry, string Flag
            , GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where += " and Id = '" + Id + "'";
            }
            if (!string.IsNullOrEmpty(SaleCustomId))
            {
                where += " and SaleCustomId = '" + SaleCustomId + "'";
            }
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS + "%'";
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
            if (!string.IsNullOrEmpty(DiQuS))
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
            if (!string.IsNullOrEmpty(nkscflag) && nkscflag != "0")
            {
                if (!nkscflag.StartsWith("n"))
                {
                    where += " and nkscflag = '" + nkscflag + "' ";
                }
                else
                {
                    where += " and nkscflag <> '" + nkscflag.TrimStart('n') + "' ";
                }
            }
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and ItemMoney >= '" + ItemMoneyS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and ItemMoney <= '" + ItemMoneyE + "'";
            }
            if (!string.IsNullOrEmpty(Industry))
            {
                where += " and Industry = '" + Industry + "'";
            }
            if (!string.IsNullOrEmpty(Flag))
            {
                where += " and Flag='" + Flag + "'";
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

        [HttpPost]
        public JsonResult GetData_SaleOrderItem(string Id)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemId", order = "asc" };
            string where = "";
            where += "and OrderId = '" + Id + "'";
            SaleOrderItemBLL bll = new SaleOrderItemBLL();
            List<View_SaleOrderItem> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }
        
        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SaleOrder(View_SaleOrder result, string CId, string nktype, string VId = "", bool AddType = false)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            ViewBag.nktype = nktype;
            ViewBag.CId = CId;
            ViewBag.VId = VId;
            ViewBag.AddType = AddType;
            ViewBag.Account = GetAccount();
            if (!AddType)
            {
                result = bll.GetRow(result.Id);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleOrder(SaleOrder model, String Products, string VId, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.OrderDate))
            {
                return Json(JsonHandler.CreateMessage(0, "日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.SaleCustomId))
            {
                return Json(JsonHandler.CreateMessage(0, "客户 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Saler))
            {
                return Json(JsonHandler.CreateMessage(0, "业务员 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.OrderType))
            {
                return Json(JsonHandler.CreateMessage(0, "类型 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.UserId))
            {
                return Json(JsonHandler.CreateMessage(0, "经手人 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            model.Enclosure = string.IsNullOrEmpty(model.Enclosure) ? "" : model.Enclosure;

            if (string.IsNullOrEmpty(model.AccountId))
            {
                model.AccountId = "null";
            }
            else
            {
                model.AccountId = "'" + model.AccountId + "'";
            }

            List<SaleOrderItem> ListItem = ParseFromJson<List<SaleOrderItem>>(Products);
            if (ListItem.Count < 1)
            {
                return Json(JsonHandler.CreateMessage(0, "请添加商品"), JsonRequestBehavior.AllowGet);
            }
            for (int i = 0; i < ListItem.Count; i++)
            {
                SaleOrderItem item = ListItem[i];
                if (string.IsNullOrEmpty(item.ProdectType))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行商品分类 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(item.ProdectDesc))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行商品描述 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (item.ItemCount <= 0)
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行成交数量必须大于零"), JsonRequestBehavior.AllowGet);
                }
                //if (item.ItemPrice <= 0)
                //{
                //    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行成交单价必须大于零"), JsonRequestBehavior.AllowGet);
                //}
                //if (item.ItemMoney <= 0)
                //{
                //    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行成交金额必须大于零"), JsonRequestBehavior.AllowGet);
                //}
                if (string.IsNullOrEmpty(item.Service))
                {
                    return Json(JsonHandler.CreateMessage(0, (i + 1) + "行包含服务费 不允许为空"), JsonRequestBehavior.AllowGet);
                }
                if (item.Service == "是")
                {
                    if (string.IsNullOrEmpty(item.SerDateS))
                    {
                        return Json(JsonHandler.CreateMessage(0, (i + 1) + "行服务开始日期 不允许为空"), JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(item.SerDateE))
                    {
                        return Json(JsonHandler.CreateMessage(0, (i + 1) + "行服务结束日期 不允许为空"), JsonRequestBehavior.AllowGet);
                    }
                }
                if (item.ItemMoney != item.ItemCount * item.ItemPrice)
                {
                    item.ItemMoney = item.ItemCount * item.ItemPrice;
                }
                //有效回款=成交金额-成本单价*数量-税金-礼品金额-其他费用
                item.ValidMoney = 0.00M;//item.ItemMoney - item.Costprice * item.ItemCount - item.TaxMoney - item.PresentMoney - item.OtherMoney;
            }

            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            SaleOrderBLL bll = new SaleOrderBLL();
            if (AddType)
            {
                //是否创建内控手册
                bool isCreateNksc = false;
                //是否更新手册
                bool isUpdateNksc = false;
                //是否创建内控报告
                bool isNkReport = false;
                FinProductTypeBLL bllproType = new FinProductTypeBLL();

                model.InvoiceFlag = "000063";//未开票
                model.PaymentFlag = "000066";//未回款
                model.OutStockFlag = "000069";//未出库
                model.CheckFlag = "000071";//未审核
                model.CheckDate = "";
                model.Finshed = false;

                //创建
                model.Id = bll.Maxid(DateTime.Now.ToString("yyyyMMdd"));

                Dictionary<string, object> tsqls = new Dictionary<string, object>();
                string sqlMain = "INSERT INTO SaleOrder(Id,OrderDate,SaleCustomId,Saler,InvoiceFlag,PaymentFlag,OutStockFlag,CheckFlag,CheckDate,Finshed,Remake,UserId,AccountId,OrderType,Enclosure,Flag) values('"
                    + model.Id + "','" + model.OrderDate + "','" + model.SaleCustomId + "','" + model.Saler + "','" + model.InvoiceFlag + "','" + model.PaymentFlag + "','"
                    + model.OutStockFlag + "','" + model.CheckFlag + "','" + model.CheckDate + "','" + model.Finshed + "','" + model.Remake + "','" + model.UserId + "',"
                    + model.AccountId + ",'" + model.OrderType + "','" + model.Enclosure + "','0')";
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
                    if (!isNkReport && "0203" == item.ProdectType.ToStringEx())
                    {
                        isNkReport = true;
                    }

                    item.TcDate = string.IsNullOrEmpty(item.TcDate) ? "" : item.TcDate;

                    item.ItemId = model.Id + (i + 1).ToString("00");
                    string sqltext = string.Format(sqlItem, new object[] { model.Id, item.ItemId, item.ProdectType,item.ProdectDesc, item.ItemCount, item.ItemPrice
                                   , item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate });
                    tsqls.Add(sqltext, null);
                }
                if (bll.Tran(tsqls))
                {
                    if (!string.IsNullOrEmpty(VId))
                    {
                        new SaleVisitBLL().Update("Update SaleVisit set SaleOrderID='" + model.Id + "',Flag='000102' where Id='" + VId + "'");
                    }
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
                    if (isNkReport)
                    {
                        //创建内控报告主表
                        NkReport report = new NkReport();
                        report.OrderId = model.Id;
                        report.CustomId = model.SaleCustomId;
                        new NkReportBLL().Insert(report);
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

                Dictionary<string, object> tsqls = new Dictionary<string, object>();

                string sqlMain = "update SaleOrder set OrderDate='" + model.OrderDate + "',SaleCustomId='" + model.SaleCustomId
                    + "',Saler='" + model.Saler + "',InvoiceFlag='" + model.InvoiceFlag + "',PaymentFlag='" + model.PaymentFlag
                    + "',OutStockFlag='" + model.OutStockFlag + "',CheckFlag='" + model.CheckFlag + "',CheckDate='" + model.CheckDate
                    + "',Finshed='" + model.Finshed + "',Remake='" + model.Remake + "',UserId='" + model.UserId
                    + "',AccountId=" + model.AccountId + ",OrderType='" + model.OrderType + "',Enclosure='" + model.Enclosure
                    + "' where Id= '" + model.Id + "'";
                tsqls.Add(sqlMain, null);

                string sqlItemI = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
                    + " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')";

                string sqlItemU = "UPDATE SaleOrderItem SET [OrderId]='{0}',[ProdectType]='{1}',[ProdectDesc]='{2}',[ItemCount]='{3}'"
                    + ",[ItemPrice]='{4}',[ItemMoney]='{5}',[TaxMoney]='{6}',[PresentMoney]='{7}',[OtherMoney]='{8}',[ValidMoney]='{9}'"
                    + ",[Service]='{10}',[SerDateS]='{11}',[SerDateE]='{12}',[TcFlag]='{13}',[TcDate]='{14}' WHERE ItemId='{15}' ";

                string ItemId = "";
                for (int i = 0; i < ListItem.Count; i++)
                {
                    FinOutStockBLL OSbll = new FinOutStockBLL();
                    SaleOrderItem item = ListItem[i];
                    if (!string.IsNullOrEmpty(item.ItemId))
                    {
                        ItemId += item.ItemId + ",";
                    }
                }


                if (string.IsNullOrEmpty(ItemId))
                {
                    int c = bll.GetOIDeleteSql(" and OrderId='" + model.Id + "'");
                    for (int j = 0; j < ListItem.Count; j++)
                    {
                        SaleOrderItem item = ListItem[j];
                        item.ItemId = model.Id + (j + 1).ToString("00");
                        string sqltext = string.Format(sqlItemI, new object[] { model.Id, item.ItemId, item.ProdectType,item.ProdectDesc, item.ItemCount, item.ItemPrice
                                   , item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate });
                        tsqls.Add(sqltext, null);
                    }
                }
                else
                {
                    int c = bll.GetOIDeleteSql(" and OrderId='" + model.Id + "' and ItemId not in(" + ItemId.TrimEnd(',') + ")");

                    string OIid = new SaleOrderItemBLL().Maxid(model.Id);
                    for (int j = 0; j < ListItem.Count; j++)
                    {
                        SaleOrderItem item = ListItem[j];
                        if (!string.IsNullOrEmpty(item.ItemId))
                        {
                            string sqltext = string.Format(sqlItemU, new object[] { model.Id, item.ProdectType,item.ProdectDesc,
                                item.ItemCount, item.ItemPrice, item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,
                                item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate,item.ItemId });
                            tsqls.Add(sqltext, null);
                        }
                        else
                        {
                            OIid = model.Id + (int.Parse(OIid.Substring(12)) + 1).ToString("00");
                            string sqltext = string.Format(sqlItemI, new object[] { model.Id, OIid, item.ProdectType,item.ProdectDesc,
                                item.ItemCount, item.ItemPrice, item.ItemMoney,item.TaxMoney,item.PresentMoney,item.OtherMoney,item.ValidMoney,
                                item.Service,item.SerDateS,item.SerDateE,item.TcFlag,item.TcDate });
                            tsqls.Add(sqltext, null);
                        }
                    }
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

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_SaleOrder(string Id)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            if (bll.Delete(Id))
            {
                LogHelper.AddLogUser(GetUserId(), "删除订单管理:" + Id, Suggestion.Succes, "订单管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除订单管理:" + Id, Suggestion.Error, "订单管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出合同、导出出库单
        /// </summary>
        /// <returns></returns>        
        public ActionResult SaleOrderExcel(string Id, string SaleCustomId, string NameS, string OrderDateS, string OrderDateE
            , string DiQuS, string userS, string dzS, string fpS, string ckS, string ItemNames, string nkscflag, string Tc
            , string ItemMoneyS, string ItemMoneyE, bool Flag, string Industry)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Id))
            {
                where += " and Id = '" + Id + "'";
            }
            if (!string.IsNullOrEmpty(SaleCustomId))
            {
                where += " and SaleCustomId = '" + SaleCustomId + "'";
            }
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and Name like '%" + NameS + "%'";
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
            if (!string.IsNullOrEmpty(DiQuS))
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
            if (!string.IsNullOrEmpty(nkscflag) && nkscflag != "0")
            {
                if (!nkscflag.StartsWith("n"))
                {
                    where += " and nkscflag = '" + nkscflag + "' ";
                }
                else
                {
                    where += " and nkscflag <> '" + nkscflag.TrimStart('n') + "' ";
                }
            }
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and ItemMoney >= '" + ItemMoneyS + "'";
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and ItemMoney <= '" + ItemMoneyE + "'";
            }
            if (!string.IsNullOrEmpty(Industry))
            {
                where += " and Industry = '" + Industry + "'";
            }
            SaleOrderBLL bll = new SaleOrderBLL();
            DataTable dt = new DataTable();
            if (Flag)
            {
                dt = bll.SelectDaoC(where);
            }
            else
            {
                dt = bll.SelectDaoChu(where);
            }

            System.Web.UI.WebControls.DataGrid dgExport = null;
            // 当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;
            string filename = "导出合同" + DateTime.Now.ToString("yyyyMMddHHmmss");
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

        /// <summary>
        /// 合同待定
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AddType"></param>
        /// <returns></returns>
        [SupportFilter(ActionName = "CUD")]//弹出窗体
        public ActionResult Create_SaleOrderDD(string Id, bool AddType = false)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            AddLogLook("合同管理待定处理");
            View_SaleOrder result = new View_SaleOrder();
            if (!AddType)
            {
                result = bll.GetRow(Id);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleODD(string Id, string Flag)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            //修改订单的状态
            string tsql = "update SaleOrder set Flag='" + Flag + "' where Id='" + Id + "'";
            //修改
            if (bll.Update(tsql) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "合同管理待定处理:" + Id, Suggestion.Succes, "合同管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "合同管理待定处理:" + Id, Suggestion.Error, "合同管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入出库单
        /// </summary>
        /// <returns></returns>
        [SupportFilter(ActionName = "CUD")]//弹出的新界面
        public ActionResult ImportExcel()
        {
            return View();
        }

        /// <summary>
        /// 导入出库单方法
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public ActionResult ImportXls(string filename, string sheetname)
        {
            string result = "";

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "UploadExcel");

            DataTable dt = ExcelHelper.Read(localPath + "\\" + filename, sheetname);

            Dictionary<string, object> tsqls = new Dictionary<string, object>();

            FinOutStockBLL bll = new FinOutStockBLL();
            FinProductBLL pbll = new FinProductBLL();
            FinProductTypeBLL tbll = new FinProductTypeBLL();
            SaleOrderBLL obll = new SaleOrderBLL();
            SaleOrderItemBLL oibll = new SaleOrderItemBLL();
            SysUserBLL ubll = new SysUserBLL();

            if (dt.Rows.Count < 1)
            {
                result += "格式错误或没有可导入数据\n";
            }

            string D = DateTime.Now.ToString("yyyyMMdd");
            string OsId = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region 判断
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,合同编号不允许为空！\n";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,客户名称不允许为空！\n";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][2].ToString()))
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,经手人不允许为空！\n";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,出库日期不允许为空！\n";
                }
                if (string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,序列号不允许为空！\n";
                }
                #endregion

                #region 出库单主表
                string OrderId = "";
                //查询订单号
                int Ocount = obll.GetCount(" and Id='" + dt.Rows[i][0].ToString() + "'");

                if (Ocount < 1)
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,合同编号不存在！\n";
                }
                else
                {
                    OrderId = dt.Rows[i][0].ToString();
                }

                //获取最大编号
                if (string.IsNullOrEmpty(OsId))
                {
                    OsId = bll.Maxid(D);
                }
                else
                {
                    OsId = D + (Convert.ToInt32(OsId.Substring(8)) + 1).ToString("0000");
                }

                //经手人
                string Uid = ubll.GetNameStr("Id", " and ZsName='" + dt.Rows[i][2].ToString() + "'");

                //出库日期
                string Osdate = "";
                try
                {
                    Osdate = Convert.ToDateTime(dt.Rows[i][3]).ToString("yyyy-MM-dd");
                }
                catch
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,出库日期格式不正确！\n";
                }

                //插入出库单(主表)
                string sqltext = "insert into FinOutStock([OrderId],[OSId],[OSdate],[Uid]) values('" + OrderId + "','" + OsId + "','"
                    + Osdate + "','" + Uid + "')";
                tsqls.Add(sqltext, null);
                #endregion

                #region 出库单明细表
                //主表编号
                string OutStockId = OsId;

                //订单明细编号
                string TypeId = oibll.GetNameStr("ProdectType", " and OrderId='" + dt.Rows[i][0].ToString() + "' and (_parentId='010101' or _parentId='010102')");
                string ItemId = oibll.GetNameStr("ItemId", " and OrderId='" + OrderId + "'");
                string SaleOrderItemId = ItemId;

                //根据序列号获取库存编号、市场价、成本价
                string Pkey = "1712" + dt.Rows[i][4].ToString();
                DataTable Pdt = pbll.GetData("Id,Marketprice,Costprice", " and Pkey='" + Pkey + "'");

                //库存编号
                string ProductId = Pdt.Rows[0]["Id"].ToString();

                //市场价
                Decimal MarketPrice = Convert.ToDecimal(Pdt.Rows[0]["Marketprice"].ToString());

                //成本价
                Decimal CostPrice = Convert.ToDecimal(Pdt.Rows[0]["Costprice"].ToString());

                //出库数量
                int OutStockCount = 1;

                string sqlItem = "insert into FinOutStockItem([OutStockId],[SaleOrderItemId],[ProductId],[Marketprice],[Costprice],[OutStockCount])"
                    + " values('" + OsId + "','" + SaleOrderItemId + "','" + ProductId + "'," + MarketPrice + "," + CostPrice + "," + OutStockCount + ")";
                tsqls.Add(sqlItem, null);
                #endregion

                #region 修改库存主表库存数量
                string sqlProduct = "update FinProduct set OutCount=OutCount+1,Stock=Stock-1 where Id='" + ProductId + "'";
                tsqls.Add(sqlProduct, null);
                #endregion

                #region 修改合同的出库状态
                string sqlOrder = "update SaleOrder set OutStockFlag='000070' where Id='" + dt.Rows[i][0].ToString() + "'";
                tsqls.Add(sqlOrder, null);
                #endregion

                //result += sqltext + "\n";
                //result += sqlItem + "\n";
                //result += sqlProduct + "\n";
            }

            if (dt.Rows.Count < 1)
            {
                result += "未找到要导入的数据。\n";
            }
            else if (string.IsNullOrEmpty(result))
            {
                if (bll.Tran(tsqls))
                {
                    result = "OK成功导入(" + dt.Rows.Count + "）条数据。";
                }
                else
                {
                    result += "导入失败。";
                }
            }

            if (result.StartsWith("OK"))
            {
                return Json(JsonHandler.CreateMessage(1, result.Replace("OK", "")), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, result), JsonRequestBehavior.AllowGet);
            }
        }

        [SupportFilter(ActionName = "CUD")]//弹出的新界面
        public ActionResult ImportOrder()
        {
            return View();
        }

        /// <summary>
        /// 导入合同
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportOrder(string filename, string sheetname)
        {
            try
            {
                string result = "";

                string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "UploadOrderXls");

                DataTable dt = ExcelHelper.Read(localPath + "\\" + filename, sheetname);

                SaleOrderBLL bll = new SaleOrderBLL();
                SaleCustomerBLL cbll = new SaleCustomerBLL();
                SysUserBLL ubll = new SysUserBLL();
                DictionaryBLL dbll = new DictionaryBLL();
                FinProductTypeBLL pbll = new FinProductTypeBLL();
                NkscBLL nbll = new NkscBLL();

                if (dt.Rows.Count < 1)
                {
                    result += "格式错误或没有可导入数据\n";
                }

                DictionaryBLL dicbll = new DictionaryBLL();
                GridPager pager = new GridPager { page = 1, rows = 100, sort = "ItemID", order = "asc" };
                List<DictionaryItem> hy_list = dicbll.SelectAll("and DicID='001'", pager);//行业
                List<DictionaryItem> czj_list = dicbll.SelectAll("and DicID='005'", pager);//财政局 行政区域

                pager.sort = "ID";
                List<View_BasicCity> city_list = new BasicCityBLL().SelectAll("", pager);//所在区县
                List<S_Order> lis_orders = new List<S_Order>();
                List<SaleCustom> lis_customs = new List<SaleCustom>();

                //订单编号(前) 日期部分
                string IdHead = DateTime.Now.ToString("yyyyMMdd");
                //订单编号(后) 后4位
                int maxID = 0;
                //客户最大编号
                int cmaxID = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //业务员
                    string UsId = "";
                    if (string.IsNullOrEmpty(dt.Rows[i]["业务员"].ToStringEx()))
                    {
                        result += "第" + (i + 2) + "行,业务员不允许为空！\n";
                    }
                    else
                    {
                        UsId = ubll.GetNameStr("Id", " and ZsName='" + dt.Rows[i]["业务员"].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(UsId))
                        {
                            result += "第" + (i + 2) + "行, 业务员不存，请核对业务员是否正确！\n";
                        }
                    }

                    //客户
                    string CusId = "";
                    if (string.IsNullOrEmpty(dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx()))
                    {
                        result += "第" + (i + 2) + "行,客户名称不允许为空！\n";
                    }
                    else
                    {
                        CusId = cbll.GetNameStr("ID", " and Name='" + dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(CusId))
                        {
                            SaleCustom scusmodel = lis_customs.FirstOrDefault(s => s.Name == dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx());
                            if (scusmodel == null)
                            {
                                #region 客户
                                if (cmaxID < 1)
                                {
                                    CusId = new SaleCustomerBLL().MaxId(IdHead);
                                    cmaxID = int.Parse(CusId.Substring(8));
                                }
                                else
                                {
                                    CusId = IdHead + (++cmaxID).ToString("000000");
                                }

                                SaleCustom sCustom = new SaleCustom();
                                sCustom.ID = CusId;
                                sCustom.CDate = DateTime.Now.ToString("yyyy-MM-dd");
                                sCustom.Ywy = UsId;

                                if (string.IsNullOrEmpty(dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx()))
                                {
                                    result += "第" + (i + 2) + "行, 客户名称不允许为空！\n";
                                }
                                sCustom.Name = dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx();


                                sCustom.Lxr = dt.Rows[i]["联系人"].ToStringEx();
                                sCustom.Phone = dt.Rows[i]["手机"].ToStringEx();
                                sCustom.QQ = dt.Rows[i]["QQ"].ToStringEx();
                                sCustom.Code = dt.Rows[i]["社会信用代码"].ToStringEx();
                                sCustom.Invoice = dt.Rows[i]["发票抬头（全称，用于开票）"].ToStringEx();
                                sCustom.Address = dt.Rows[i]["邮寄地址"].ToStringEx();
                                sCustom.Bank = dt.Rows[i]["开户行"].ToStringEx();
                                sCustom.CardNum = dt.Rows[i]["账号"].ToStringEx();

                                sCustom.Province = "0001";//省份

                                DictionaryItem czj = czj_list.FirstOrDefault(s => s.ItemName == dt.Rows[i]["财政局"].ToStringEx());
                                if (czj != null)
                                {
                                    sCustom.Finance = czj.ItemID;
                                }
                                else
                                {
                                    result += "第" + (i + 2) + "行, 财政局不允许为空！\n";
                                }

                                DictionaryItem hy = hy_list.FirstOrDefault(s => s.ItemName == dt.Rows[i]["行业"].ToStringEx());
                                if (hy != null)
                                {
                                    sCustom.Industry = hy.ItemID;
                                }

                                View_BasicCity city = city_list.FirstOrDefault(s => s.Name == dt.Rows[i]["所在区县"].ToStringEx());
                                if (city != null)
                                {
                                    sCustom.Region = city.ID;//所在区县
                                    sCustom.UpID = city.Pid;//上级主管区域
                                }
                                else
                                {
                                    result += "第" + (i + 2) + "行, 所在区县不允许为空！\n";
                                }

                                sCustom.Uid = GetUserId();
                                lis_customs.Add(sCustom);

                                #endregion
                            }
                            else
                            {
                                CusId = scusmodel.ID;
                            }
                        }
                    }

                    S_Order odMain = lis_orders.FirstOrDefault(s => s.OrderMain.SaleCustomId == CusId && s.OrderMain.Saler == UsId);
                    if (odMain == null)
                    {
                        #region 订单主表

                        //订单日期
                        string OrderDate = Convert.ToDateTime(dt.Rows[i]["上报日期"]).ToString("yyyy-MM-dd");
                        //订单编号
                        string SaleOrderId = "";
                        if (maxID > 0)
                        {
                            string id = IdHead + (++maxID).ToString("0000");
                            SaleOrderId = id;
                        }
                        else
                        {
                            SaleOrderId = bll.Maxid(IdHead);
                            maxID = int.Parse(SaleOrderId.Substring(8));
                        }

                        //是否立即开票
                        string Fp = "";
                        if (string.IsNullOrEmpty(dt.Rows[i]["是否马上开发票"].ToStringEx()))
                        {
                            result += "第" + (i + 2) + "行,是否马上开发票不允许为空！\n";
                        }
                        else
                        {
                            if (dt.Rows[i]["是否马上开发票"].ToStringEx() == "是")
                            {
                                Fp = "1";
                            }
                            else
                            {
                                Fp = "0";
                            }
                        }

                        //账户
                        string AccId = "";
                        //if (!string.IsNullOrEmpty(dt.Rows[i][10].ToStringEx()))
                        //{
                        //    AccId = new BasicAccountBLL().GetNameStr("Id", " and [key]='" + dt.Rows[i][10].ToStringEx() + "'");
                        //}

                        odMain = new S_Order();

                        odMain.KhName = dt.Rows[i]["客户名称（用于做内控手册显示）"].ToStringEx();

                        odMain.OrderMain.OrderDate = OrderDate;//订单日期
                        odMain.OrderMain.Id = SaleOrderId;//订单编号
                        odMain.OrderMain.SaleCustomId = CusId;//客户
                        odMain.OrderMain.Saler = UsId;//业务员
                        odMain.OrderMain.Fp = Fp;//是否立即开票
                        odMain.OrderMain.AccountId = AccId;//账户
                        odMain.OrderMain.UserId = GetUserId();//操作员
                        odMain.OrderMain.Remake = dt.Rows[i]["开票说明"].ToStringEx();
                        lis_orders.Add(odMain);

                        #endregion
                    }

                    #region 订单明细表

                    int itemid = odMain.OrderItems.Count + 1;

                    SaleOrderItem OmxModel = new SaleOrderItem();
                    OmxModel.OrderId = odMain.OrderMain.Id;
                    OmxModel.ItemId = odMain.OrderMain.Id + itemid.ToString("00");

                    #region 商品分类
                    if (string.IsNullOrEmpty(dt.Rows[i]["商品分类"].ToStringEx()))
                    {
                        result += "第" + (i + 2) + "行,商品分类不允许为空！\n";
                    }
                    else
                    {
                        string Ptype = pbll.GetNameStr("Id", " and Name='" + dt.Rows[i]["商品分类"].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(Ptype))
                        {
                            result += (i + 2) + "行,未能找到商品分类！\n";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(CusId))
                            {
                                if (odMain.OrderItems.FirstOrDefault(s => s.ProdectType == Ptype) != null)
                                {
                                    result += (i + 2) + "行,订单中商品分类【" + dt.Rows[i]["商品分类"].ToStringEx() + "】重复，请检查是否包含重复数据！\n";
                                }
                            }
                            OmxModel.ProdectType = Ptype;
                        }
                    }
                    #endregion

                    #region 商品参数（描述）
                    if (string.IsNullOrEmpty(dt.Rows[i]["商品参数（描述）"].ToStringEx()))
                    {
                        result += "第" + (i + 2) + "行,商品参数(描述)不允许为空！\n";
                    }
                    else
                    {
                        OmxModel.ProdectDesc = dt.Rows[i]["商品参数（描述）"].ToStringEx();
                    }
                    #endregion

                    #region 数量
                    if (string.IsNullOrEmpty(dt.Rows[i]["数量"].ToStringEx()))
                    {
                        result += "第" + (i + 2) + "行,数量不允许为空！\n";
                    }
                    else
                    {
                        try
                        {
                            OmxModel.ItemCount = Convert.ToInt32(dt.Rows[i]["数量"].ToStringEx());
                        }
                        catch
                        {
                            result += (i + 2) + "行,数量格式不正确！\n";
                        }
                    }
                    #endregion

                    #region 单价
                    if (string.IsNullOrEmpty(dt.Rows[i]["单价"].ToString()))
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,单价不允许为空！\n";
                    }
                    else
                    {
                        try
                        {
                            OmxModel.ItemPrice = Convert.ToDecimal(dt.Rows[i]["单价"].ToStringEx());
                        }
                        catch
                        {
                            result += (i + 2) + "行,单价格式不正确！\n";
                        }
                    }
                    #endregion

                    //成交金额=单价*数量
                    OmxModel.ItemMoney = OmxModel.ItemCount * OmxModel.ItemPrice;
                    //税金
                    OmxModel.TaxMoney = OmxModel.ItemMoney * 0.05M;

                    #region 礼品礼金
                    try
                    {
                        if (dt.Rows[i]["礼品金额"].ToStringEx() == "")
                        {
                            OmxModel.PresentMoney = 0.00M;
                        }
                        else
                        {
                            OmxModel.PresentMoney = Convert.ToDecimal(dt.Rows[i]["礼品金额"]);
                        }
                    }
                    catch
                    {
                        result += (i + 2) + "行,礼品礼金格式错误\n";
                    }
                    #endregion

                    #region 其他金额
                    try
                    {
                        if (dt.Rows[i]["其他费用"].ToStringEx() == "")
                        {
                            OmxModel.OtherMoney = 0.00M;
                        }
                        else
                        {
                            OmxModel.OtherMoney = Convert.ToDecimal(dt.Rows[i]["其他费用"]);
                        }
                    }
                    catch
                    {
                        result += (i + 2) + "行,其他费用格式错误\n";
                    }
                    #endregion

                    //是否包含服务、服务开始日期、服务结束日期
                    if (OmxModel.ProdectType.Substring(0, 2) == "01")
                    {
                        OmxModel.Service = "是";
                        OmxModel.SerDateS = DateTime.Now.ToString("yyyy-MM-dd");
                        OmxModel.SerDateE = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                    }

                    odMain.OrderItems.Add(OmxModel);

                    #endregion

                }

                if (dt.Rows.Count < 1)
                {
                    result += "未找到要导入的数据。\n";
                }
                else if (string.IsNullOrEmpty(result))
                {
                    Dictionary<string, object> tsqls = create_tsqls(lis_orders, lis_customs, "");
                    if (bll.Tran(tsqls))
                    {
                        result = "OK成功导入(" + dt.Rows.Count + ")条数据。";
                    }
                    else
                    {
                        result += "导入失败。";
                    }
                }

                if (result.StartsWith("OK"))
                {
                    return Json(JsonHandler.CreateMessage(1, result.Replace("OK", "")), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, result), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 生成插入语句
        /// </summary>
        /// <param name="lis_orders"></param>
        /// <returns></returns>
        private Dictionary<string, object> create_tsqls(List<S_Order> lis_orders, List<SaleCustom> lis_customs, string updateSql)
        {
            NkscBLL nbll = new NkscBLL();
            string MaxVersion = nbll.GetMaxVersion();//最大版本

            Dictionary<string, object> tsqls = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(updateSql))
            {
                foreach (var sCustom in lis_customs)
                {
                    tsqls.Add(sCustom.ToString(), null);
                }
            }
            else
            {
                tsqls.Add(updateSql, null);
            }

            foreach (var main in lis_orders)
            {
                tsqls.Add(main.OrderMain.ToString(), null);
                foreach (var item in main.OrderItems)
                {
                    tsqls.Add(item.ToString(), null);
                    if (item.ProdectType.StartsWith("0201"))//内控手册
                    {
                        Nksc nksc = new Nksc();
                        nksc.SaleOrderID = main.OrderMain.Id;//合同编号
                        nksc.CustomerID = main.OrderMain.SaleCustomId;//客户编号
                        nksc.dwqc = main.KhName;
                        nksc.NkscDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        nksc.version = MaxVersion;
                        tsqls.Add(nksc.ToString(), null);
                    }
                    else if (item.ProdectType.StartsWith("0203"))//内控报告
                    {
                        //创建内控报告主表
                        NkReport report = new NkReport();
                        report.OrderId = main.OrderMain.Id;
                        report.CustomId = main.OrderMain.SaleCustomId;
                        tsqls.Add(report.ToString(), null);
                    }
                    else if (item.ProdectType.StartsWith("0204"))//更新手册
                    {
                        Nksc_Update nkup = new Nksc_Update();
                        nkup.CustomerID = main.OrderMain.SaleCustomId;
                        nkup.NkscDate = main.OrderMain.OrderDate;
                        nkup.versionS = nbll.GetNameStr("version", "and CustomerID='" + nkup.CustomerID + "'");
                        nkup.versionE = MaxVersion;
                        nkup.UpdateFlag = "0";
                        tsqls.Add(nkup.ToString(), null);
                    }
                }
            }

            return tsqls;
        }

        #region 上传附件导入合同
        [HttpPost]
        public JsonResult UploadOrderXls()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            string fileName = Request["name"].Replace("、", "-");
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/UploadOrderXls/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MergeOrderXls()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/UploadOrderXls/");//Upload 文件夹
            var fileName = Request["fileName"].Replace("、", "-");//文件名
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

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取导入合同SheetName
        [HttpPost]
        public ActionResult GetOExcelSheetName(string filename)
        {
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "UploadOrderXls");
            List<string> list = ExcelHelper.GetSheetName(localPath + "\\" + filename);
            List<ExcelModel> models = new List<ExcelModel>();
            for (int i = 0; i < list.Count; i++)
            {
                ExcelModel model = new ExcelModel();
                model.id = i.ToString("00");
                model.name = list[i];
                models.Add(model);
            }
            return Json(models);
        }
        #endregion

        #endregion

        #region 拜访沟通

        [SupportFilter]
        public ActionResult SaleVisit()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("拜访沟通");

            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            //03 业务员
            if (roleId == "03")
            {
                ViewBag.userS = GetUserId();
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetData_SaleVisit(string Name, string userS, string did, string dtype, string xqfl
                        , string ContactDateS, string ContactDateE, string NextTimeS, string NextTimeE, string sfyx, string llzt, string jd, string zt, string cl
                        , GridPager pager)
        {
            string where = "";
            //获取用户类别
            SysUserBLL bl = new SysUserBLL();
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                where += "and Name like '%" + Name.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy = '" + userS.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(did.Trim()))
            {
                if (dtype == "A")
                {
                    //省份
                    where += "and Sfid = '" + did.Trim() + "'";
                }
                else if (dtype == "B")
                {
                    //九大地区(上级主管区域)
                    where += "and Pid = '" + did.Trim() + "'";
                }
                else if (dtype == "C")
                {
                    //所属市区县
                    where += "and Cityid = '" + did.Trim() + "'";
                }
            }
            if (!string.IsNullOrEmpty(ContactDateS))
            {
                where += " and ContactDate >= '" + ContactDateS + "'";
            }
            if (!string.IsNullOrEmpty(ContactDateE))
            {
                where += " and ContactDate <= '" + ContactDateE + "'";
            }
            if (!string.IsNullOrEmpty(NextTimeS))
            {
                where += " and NextTime >= '" + NextTimeS + "'";
            }
            if (!string.IsNullOrEmpty(NextTimeE))
            {
                where += " and NextTime <= '" + NextTimeE + "'";
            }
            if (!string.IsNullOrEmpty(xqfl))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in xqfl.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "DemandType like '%" + item + "%'";
                    }
                    else
                    {
                        dqwhere += " or DemandType like '%" + item + "%'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(sfyx))//意向
            {
                where += " and Intention = '" + sfyx + "'";
            }
            if (!string.IsNullOrEmpty(llzt))//联络状态
            {
                where += " and ContactFlag = '" + llzt + "'";
            }
            if (!string.IsNullOrEmpty(jd))//进度
            {
                where += " and Progress = '" + jd + "'";
            }
            if (!string.IsNullOrEmpty(cl))//
            {
                if (cl == "0")
                {
                    where += " and (Auditor = '' or Auditor is null)";
                }
                else
                {
                    where += " and Auditor != ''";
                }
            }
            if (!string.IsNullOrEmpty(zt))//
            {
                where += " and Flag = '" + zt + "'";
            }
            SaleVisitBLL bll = new SaleVisitBLL();
            List<View_SaleVisit> result = bll.SelectAll(where, pager);
            List<View_SaleVisit> footer = bll.SelectAllSum(where);
            var json = new
            {
                total = pager.totalRows,
                rows = result,
                footer = footer
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetLxrName(string Id)
        {
            SaleCustomerBLL Cusbll = new SaleCustomerBLL();
            string str = Cusbll.GetStrName("Lxr", " where ID='" + Id + "'");

            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ContactDate", order = "desc" };
            SaleVisitBLL bll = new SaleVisitBLL();
            string where = " and SaleCustomID='" + Id + "'";
            List<View_SaleVisit_History> result = bll.SelectAll_History(where, pager);
            var json = new
            {
                Lxr = str,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult Get_LxrName(string Id)
        {
            SaleCustomerBLL Cusbll = new SaleCustomerBLL();
            string str = Cusbll.GetStrName("Lxr", " where ID='" + Id + "'");

            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "desc" };
            SaleVisitBLL bll = new SaleVisitBLL();
            string where = " and SaleCustomID='" + Id + "'";
            List<View_SaleVisit_AuditDetails> result = bll.SelectAll_AuditDetails(where, pager);
            var json = new
            {
                Lxr = str,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult Get_LxrUp(string Id)
        {
            SaleCustomerBLL Cusbll = new SaleCustomerBLL();
            string str = Cusbll.GetStrName("Lxr", " where ID='" + Id + "'");

            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "desc" };
            SaleVisitBLL bll = new SaleVisitBLL();
            string where = " and SaleCustomID='" + Id + "'";
            List<View_SaleVisit_Up> result = bll.SelectAll_Fj(where, pager);
            var json = new
            {
                Lxr = str,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SaleVisit(string Id = "", bool LookType = false)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            View_SaleVisit result = new View_SaleVisit();
            ViewBag.Look = LookType;
            if (!string.IsNullOrEmpty(Id))
            {
                GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
                result.Id = Id;
                result = bll.GetRows(result);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleVisit(SaleVisit model)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
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
                model.NetContactTime = bll.GetStrName("NetContactTime", " where Id='" + model.Id + "'");
                model.AuditDate = bll.GetStrName("AuditDate", " where Id='" + model.Id + "'");
                model.AuditDetails = bll.GetStrName("AuditDetails", " where Id='" + model.Id + "'");
                model.Auditor = bll.GetStrName("Auditor", " where Id='" + model.Id + "'");
                model.AuditState = bll.GetStrName("AuditState", " where Id='" + model.Id + "'");
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

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_SaleVisitAudit(string Id = "")
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            View_SaleVisit result = new View_SaleVisit();
            if (!string.IsNullOrEmpty(Id))
            {
                GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
                result.Id = Id;
                result = bll.GetRows(result);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleVisitAudit(string Id, string AuditDetails)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            if (string.IsNullOrEmpty(Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            // 未读 000104  已读 000103
            string tsql = "update SaleVisit set AuditState='000104',AuditDetails='" + AuditDetails + "',AuditDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "',Auditor='" + GetUserId() + "' where Id='" + Id + "'";
            //修改
            if (bll.Update(tsql) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "拜访沟通处理意见:" + Id, Suggestion.Succes, "拜访沟通");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "拜访沟通处理意见:" + Id, Suggestion.Error, "拜访沟通");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete_SaleVisit(string Id)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除拜访沟通:" + Id, Suggestion.Succes, "拜访沟通");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除拜访沟通:" + Id, Suggestion.Error, "拜访沟通");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update_AuditState(string Id, string userS)
        {
            SaleVisitBLL bll = new SaleVisitBLL();
            if (!string.IsNullOrEmpty(userS))
            {
                string sql = "update SaleVisit set AuditState='000103' where Id='" + Id + "'";
                if (bll.Update(sql) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "标记已读拜访沟通:" + Id, Suggestion.Succes, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "标记已读拜访沟通:" + Id, Suggestion.Error, "拜访沟通");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
        }

        #region 上传附件
        [HttpPost]
        public ActionResult UploadSaleVisit()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string nkscid = nksc.GetNameStr("id", " and CustomerID='" + uid + "'");
            if (nksc.GetFlag(nkscid))
            {
                return Json(JsonHandler.CreateMessage(0, "本手册已审核，如需修改请与我们联系"), JsonRequestBehavior.AllowGet);
            }

            string fileName = Request["name"].Replace("、", "-");
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/UploadSaleVisit");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MergeSaleVisit()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string nkscid = nksc.GetNameStr("id", " and CustomerID='" + uid + "'");
            if (nksc.GetFlag(nkscid))
            {
                return Json(JsonHandler.CreateMessage(0, "本手册已审核，如需修改请与我们联系"), JsonRequestBehavior.AllowGet);
            }
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/UploadSaleVisit");//Upload 文件夹
            var fileName = Request["fileName"].Replace("、", "-");//文件名
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

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除附件

        [HttpPost]
        public ActionResult DeleteVisitFJ(string Id, string name)
        {
            if (string.IsNullOrEmpty(GetUserId()))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(name))
            {
                return Json(JsonHandler.CreateMessage(0, "要删除的文件名为空"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                SaleVisitBLL visit = new SaleVisitBLL();
                string Fj = visit.GetStrName("Fj", "where Id='" + Id + "'");
                Fj = Fj.Replace(name + "、", "");
                //nksc.Update("update SaleVisit set Fj='" + Fj + "' where Id='" + Id + "'");
                var uploadDir = Server.MapPath("~/UploadSaleVisit/" + name);//UploadSaleVisit 文件夹
                var finalPath = Path.Combine(uploadDir, name);
                System.IO.File.Delete(finalPath);
                return Json(JsonHandler.CreateMessage(1, "删除成功"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region 服务管理
        [SupportFilter]
        public ActionResult SaleService()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("服务管理");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_SaleService(string start, string end)
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };

            string where = "";
            //获取用户类别
            SysUserBLL bl = new SysUserBLL();

            SaleVisitBLL bll = new SaleVisitBLL();
            List<View_SaleVisit> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }
        #endregion

        #region 导入客户
        public ActionResult UploadExcel()
        {
            return View();
        }

        #region 返回汉字全拼的方法
        private List<string> quanpin(string txt)
        {
            char[] Cr = txt.ToCharArray();
            List<string> lpy = new List<string>();
            //循环textBox1里的值
            for (int i = 0; i < txt.Length; i++)
            {
                if (ChineseChar.IsValidChar(Cr[i]))
                {
                    ChineseChar CC = new ChineseChar(Cr[i]);
                    foreach (string item in CC.Pinyins)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (!lpy.Contains(item.Substring(0, item.Length - 1)))
                                lpy.Add(item.Substring(0, item.Length - 1));
                        }
                    }
                }
            }
            return lpy;
        }
        #endregion

        public int GetRandomSeedbyGuid()
        {
            return new Guid().GetHashCode();
        }

        [HttpPost]
        public ActionResult Import(string filename, string sheetname)
        {
            string result = "";

            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "UploadExcel");

            DataTable dt = ExcelHelper.Read(localPath + "\\" + filename, sheetname);

            Dictionary<string, object> tsqls = new Dictionary<string, object>();

            NkscBLL bll = new NkscBLL();
            SaleOrderBLL obll = new SaleOrderBLL();

            if (dt.Rows.Count < 1)
            {
                result += "格式错误或没有可导入数据\n";
            }
            string SaleOrderId = "";
            string CusId = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SaleCustom model = new SaleCustom();
                SaleOrder Omodel = new Model.SaleOrder();
                SaleOrderItem OmxModel = new SaleOrderItem();
                Nksc Nmodel = new Nksc();
                SaleCustomerBLL scbll = new SaleCustomerBLL();
                SysUserBLL ubll = new SysUserBLL();
                BasicCityPLBLL bcpbll = new BasicCityPLBLL();
                BasicCityBLL bcbll = new BasicCityBLL();
                DictionaryBLL dbll = new DictionaryBLL();
                FinProductTypeBLL pbll = new FinProductTypeBLL();

                string CusCount = scbll.GetNameStr("count(*)", " and Name='" + dt.Rows[i][0].ToStringEx() + "'");
                //判断用户是否存在，不存在则创建
                if (CusCount == "0")//不存在
                {
                    #region 客户
                    if (!string.IsNullOrEmpty(CusId))
                    {
                        string id = DateTime.Now.ToString("yyyyMMdd") + (int.Parse(CusId.Substring(8)) + 1).ToString("000000");
                        CusId = id;
                    }
                    else
                    {
                        CusId = scbll.MaxId(DateTime.Now.ToString("yyyyMMdd"));
                    }
                    model.ID = CusId;//编号
                    model.CDate = DateTime.Now.ToString("yyyy-MM-dd");//创建日期    
                    #region 单位名称
                    if (dt.Rows[i][0].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,单位名称不允许为空！\n";
                    }
                    else
                    {
                        model.Name = dt.Rows[i][0].ToStringEx();//单位名称
                    }
                    #endregion
                    model.Code = dt.Rows[i][1].ToStringEx();//信用代码
                    #region 发票名头
                    if (dt.Rows[i][2].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,发票名头不允许为空！\n";
                    }
                    else
                    {
                        model.Invoice = dt.Rows[i][2].ToStringEx();//发票名头
                    }
                    #endregion
                    #region 联系人
                    if (dt.Rows[i][3].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,联系人不允许为空！\n";
                    }
                    else
                    {
                        model.Lxr = dt.Rows[i][3].ToStringEx();//联系人
                    }
                    #endregion
                    #region 电话
                    if (dt.Rows[i][4].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,电话不允许为空！\n";
                    }
                    else
                    {
                        model.Phone = dt.Rows[i][4].ToStringEx();//电话
                    }
                    #endregion
                    #region 地址
                    if (dt.Rows[i][5].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,地址不允许为空！\n";
                    }
                    else
                    {
                        model.Address = dt.Rows[i][5].ToStringEx();//地址
                    }
                    #endregion
                    #region 业务员
                    string YwyId = ubll.GetNameStr("Id", " and ZsName='" + dt.Rows[i][6].ToStringEx() + "'");
                    if (string.IsNullOrEmpty(YwyId))
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,未能找到业务员！\n";
                    }
                    else
                    {
                        model.Ywy = YwyId;
                        model.Uid = YwyId;
                        model.YwyName = dt.Rows[i][6].ToStringEx();
                    }
                    #endregion
                    #region 上级主管区域
                    if (dt.Rows[i][7].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,上级主管区域不允许为空！\n";
                    }
                    else
                    {
                        string cityPlId = bcpbll.GetNameStr("ID", " and Name='" + dt.Rows[i][7].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(cityPlId))
                        {
                            //提示错误
                            result += "第" + (i + 2) + "行,未能找到上级主管区域！\n";
                        }
                        else
                        {
                            model.UpID = cityPlId;//上级主管区域
                        }
                    }
                    #endregion
                    #region 省份
                    if (dt.Rows[i][8].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,省份不允许为空！\n";
                    }
                    else
                    {
                        model.Province = dt.Rows[i][8].ToStringEx();//省份
                    }
                    #endregion
                    #region 行业
                    if (dt.Rows[i][9].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,行业不允许为空！\n";
                    }
                    else
                    {
                        string HangYe = dbll.GetNameStr("ItemID", " and DicID='001' and ItemName='" + dt.Rows[i][9].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(HangYe))
                        {
                            //提示错误
                            result += "第" + (i + 2) + "行,未能找到行业！\n";
                        }
                        else
                        {
                            model.Industry = dt.Rows[i][9].ToStringEx();//行业
                        }
                    }
                    #endregion
                    #region 地区
                    if (dt.Rows[i][10].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,地区不允许为空！\n";
                    }
                    else
                    {
                        string cityId = bcbll.GetStringName("ID", " where Name='" + dt.Rows[i][10].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(cityId))
                        {
                            //提示错误
                            result += "第" + (i + 2) + "行,未能找到地区！\n";
                        }
                        else
                        {
                            model.Region = cityId;//地区
                        }
                    }
                    #endregion
                    #region 财政局
                    if (dt.Rows[i][11].ToStringEx() == "")
                    {
                        //提示错误
                        result += "第" + (i + 2) + "行,财政局不允许为空！\n";
                    }
                    else
                    {
                        string xzName = dbll.GetNameStr("ItemID", " and DicID='005' and ItemName='" + dt.Rows[i][11].ToStringEx() + "'");
                        if (string.IsNullOrEmpty(xzName))
                        {
                            //提示错误
                            result += "第" + (i + 2) + "行,,未能找到财政局！\n";
                        }
                        else
                        {
                            model.Finance = xzName;
                        }
                    }
                    #endregion
                    model.QtLxr = dt.Rows[i][20].ToStringEx();
                    model.QtTel = dt.Rows[i][21].ToStringEx();
                    model.BM = null;//部门
                    model.Zw = null;//职务
                    model.Xydj = null;//信用等级
                    model.Gx = null;//与客户关系
                    model.Zyx = null;//客户重要性
                    model.Source = null;//客户来源
                    model.CustomerType = null;//客户类别
                    model.CustomerGrade = null;//客户等级
                    if (dt.Rows[i][13].ToStringEx() == "内控手册编制")
                    {
                        string DwName = dt.Rows[i][0].ToStringEx();
                        int HCount = dt.Rows[i][0].ToStringEx().Length;
                        string UserNP = "";
                        for (int j = 0; j < HCount; j++)
                        {
                            string Ming = DwName.Substring(j, 1);
                            List<string> list = quanpin(Ming);
                            for (int k = 0; k < list.Count; k++)
                            {
                                UserNP += list[k].Substring(0, 1);
                                break;
                            }
                        }
                        model.UserName = UserNP.ToLower();
                        model.UserPwd = UserNP.ToLower();

                        string UCount = scbll.GetNameStr("count(*)", " and UserName='" + model.UserName + "'");
                        if (Convert.ToInt32(UCount) > 0)
                        {
                            //生成随机数添加到用户名和密码后
                            string SjSum = model.UserName;
                            Random random = new Random();
                            for (int l = 0; l < 4; l++)
                            {
                                SjSum += random.Next(0, 9);
                            }
                            model.UserName = SjSum;
                            model.UserPwd = SjSum;
                        }
                    }

                    string sqlCus = "INSERT INTO SaleCustom([ID],[CDate],[Ywy],[Name],[BM],[Lxr],[Zw],[Phone],[Industry],[UpID],[Province],[Xydj]"
                        + ",[Gx],[Zyx],[Address],[QtLxr],[QtTel],[Uid],[Source],[Region],[CustomerType],[CustomerGrade],[Code],[Invoice],[UserName]"
                        + ",[UserPwd],[Finance],[YwyName]) VALUES('" + model.ID + "','" + model.CDate + "','" + model.Ywy + "','" + model.Name
                        + "',null,'" + model.Lxr + "',null,'" + model.Phone + "',null,'" + model.UpID + "','" + model.Province + "',null,null,null,'"
                        + model.Address + "','" + model.QtLxr + "','" + model.QtTel + "','" + model.Uid + "',null,'" + model.Region + "',null,null,'"
                        + model.Code + "','" + model.Invoice + "','" + model.UserName + "','" + model.UserPwd + "','" + model.Finance
                        + "','" + model.YwyName + "')";
                    tsqls.Add(sqlCus, null);
                    #endregion
                }
                else//已存在
                {
                    model.ID = scbll.GetNameStr("ID", " and Name='" + dt.Rows[i][0].ToStringEx() + "'");
                    model.Ywy = scbll.GetNameStr("Ywy", " and Name='" + dt.Rows[i][0].ToStringEx() + "'");

                    //是否需要修改信用代码、发票名头
                    bool bl = false;
                    //查询信用代码、发票名头
                    string Code = scbll.GetNameStr("Code", " and Name='" + dt.Rows[i][0].ToStringEx() + "'");
                    string Invoice = scbll.GetNameStr("Invoice", " and Name='" + dt.Rows[i][0].ToStringEx() + "'");
                    //修改sql语句
                    string Usql = "update SaleCustom set {0} where Name='" + dt.Rows[i][0].ToStringEx() + "'";
                    string sql = "";
                    //信用代码
                    if (Code != dt.Rows[i][1].ToStringEx())
                    {
                        bl = true;
                        sql = "Code='" + dt.Rows[i][1].ToStringEx() + "'";
                    }
                    //发票名头
                    if (Invoice != dt.Rows[i][2].ToStringEx())
                    {
                        bl = true;
                        if (!string.IsNullOrEmpty(Invoice))
                        {
                            sql += ",Invoice='" + dt.Rows[i][2].ToStringEx() + "'";
                        }
                        else
                        {
                            sql = "Invoice='" + dt.Rows[i][2].ToStringEx() + "'";
                        }
                    }
                    if (bl)
                    {
                        tsqls.Add(string.Format(Usql, sql), null);
                    }
                }

                #region 订单主表

                #region 是否马上开发票
                if (dt.Rows[i][12].ToStringEx() == "")
                {
                    //提示错误
                    result += "第" + (i + 2) + "行,是否马上开发票不允许为空！\n";
                }
                else
                {
                    string Fp = "";
                    if (dt.Rows[i][12].ToStringEx() == "是")
                    {
                        Fp = "1";
                    }
                    else
                    {
                        Fp = "0";
                    }
                    Omodel.Fp = Fp;
                }
                #endregion

                if (!string.IsNullOrEmpty(SaleOrderId))
                {
                    string id = DateTime.Now.ToString("yyyyMMdd") + (int.Parse(SaleOrderId.Substring(8)) + 1).ToString("0000");
                    SaleOrderId = id;
                }
                else
                {
                    SaleOrderId = obll.Maxid(DateTime.Now.ToString("yyyyMMdd"));
                }
                Omodel.Id = SaleOrderId;
                Omodel.OrderDate = DateTime.Now.ToString("yyyy-MM-dd");
                Omodel.SaleCustomId = model.ID;//客户
                Omodel.Saler = model.Ywy;//业务员

                #region 账户
                if (dt.Rows[i][19].ToStringEx() == "")
                {
                    Omodel.AccountId = "";
                }
                else
                {
                    string AccId = new BasicAccountBLL().GetNameStr("Id", " and [key]='" + dt.Rows[i][19].ToStringEx() + "'");
                    Omodel.AccountId = AccId;//账户
                }
                #endregion

                Omodel.OrderType = "000073";//合同类别

                Omodel.InvoiceFlag = "000063";//未开票
                Omodel.PaymentFlag = "000066";//未回款
                Omodel.OutStockFlag = "000069";//未出库
                Omodel.CheckFlag = "000071";//未审核

                Omodel.CheckDate = "";
                Omodel.Finshed = false;
                Omodel.Remake = "";//备注
                Omodel.Enclosure = "";
                Omodel.UserId = GetUserId();

                //主表
                string sqlMain = "INSERT INTO SaleOrder(Id,OrderDate,SaleCustomId,Saler,InvoiceFlag,PaymentFlag,OutStockFlag,CheckFlag,CheckDate"
                    + ",Finshed,Remake,UserId,AccountId,OrderType,Enclosure,Fp) values('" + Omodel.Id + "','" + Omodel.OrderDate
                    + "','" + Omodel.SaleCustomId + "','" + Omodel.Saler + "','" + Omodel.InvoiceFlag + "','" + Omodel.PaymentFlag
                    + "','" + Omodel.OutStockFlag + "','" + Omodel.CheckFlag + "','" + Omodel.CheckDate + "','" + Omodel.Finshed
                    + "','" + Omodel.Remake + "','" + Omodel.UserId + "',null,'" + Omodel.OrderType + "','" + Omodel.Enclosure + "','" + Omodel.Fp + "')";
                tsqls.Add(sqlMain, null);
                #endregion

                #region 订单明细表
                //明细表
                OmxModel.OrderId = Omodel.Id;

                if (string.IsNullOrEmpty(OmxModel.ItemId))
                {
                    OmxModel.ItemId = Omodel.Id + (i + 1).ToString("00");
                }
                else
                {
                    OmxModel.ItemId = OmxModel.ItemId.Substring(0, 12) + (Convert.ToInt32(OmxModel.ItemId.Substring(12)) + 1).ToString("00");
                }

                #region 商品分类
                if (dt.Rows[i][13].ToStringEx() == "")
                {
                    result += (i + 2) + "行,商品分类不允许为空！\n";
                }
                else
                {
                    string Ptype = pbll.GetNameStr("Id", " and Name='" + dt.Rows[i][13].ToStringEx() + "'");
                    if (string.IsNullOrEmpty(Ptype))
                    {
                        result += (i + 2) + "行,未能找到商品分类！\n";
                    }
                    else
                    {
                        OmxModel.ProdectType = Ptype;
                    }
                }
                #endregion

                #region 商品参数（描述）
                if (dt.Rows[i][14].ToStringEx() == "")
                {
                    result += (i + 2) + "行,商品参数（描述）不允许为空！\n";
                }
                else
                {
                    OmxModel.ProdectDesc = dt.Rows[i][14].ToStringEx();
                }
                #endregion

                #region 数量
                if (dt.Rows[i][15].ToStringEx() == "")
                {
                    result += (i + 2) + "行,数量不允许为空！\n";
                }
                else
                {
                    try
                    {
                        OmxModel.ItemCount = Convert.ToInt32(dt.Rows[i][15].ToStringEx());
                    }
                    catch
                    {
                        result += (i + 2) + "行,数量格式不正确！\n";
                    }
                }
                #endregion

                #region 单价
                if (dt.Rows[i][16].ToStringEx() == "")
                {
                    result += (i + 2) + "行,单价不允许为空！\n";
                }
                else
                {
                    try
                    {
                        OmxModel.ItemPrice = Convert.ToDecimal(dt.Rows[i][16].ToStringEx());
                    }
                    catch
                    {
                        result += (i + 2) + "行,单价格式不正确！\n";
                    }
                }
                #endregion

                OmxModel.ItemMoney = OmxModel.ItemCount * OmxModel.ItemPrice;//成交金额=单价*数量
                OmxModel.TaxMoney = Convert.ToDecimal(Convert.ToDouble(OmxModel.ItemMoney) * 0.05);//税金

                #region 礼品礼金
                try
                {
                    if (dt.Rows[i][17].ToStringEx() == "")
                    {
                        OmxModel.PresentMoney = 0.00M;
                    }
                    else
                    {
                        OmxModel.PresentMoney = Convert.ToDecimal(dt.Rows[i][17]);//礼品礼金
                    }
                }
                catch
                {
                    result += (i + 2) + "行,礼品礼金格式错误\n";
                }
                #endregion

                #region 其他金额
                try
                {
                    if (dt.Rows[i][18].ToStringEx() == "")
                    {
                        OmxModel.OtherMoney = 0.00M;
                    }
                    else
                    {
                        OmxModel.OtherMoney = Convert.ToDecimal(dt.Rows[i][18]);//其他费用
                    }
                }
                catch
                {
                    result += (i + 2) + "行,其他费用格式错误\n";
                }
                #endregion

                //是否包含服务、服务开始日期、服务结束日期
                if (!string.IsNullOrEmpty(OmxModel.ProdectType))
                {
                    if (OmxModel.ProdectType.Substring(0, 2) == "01")
                    {
                        OmxModel.Service = "是";
                        OmxModel.SerDateS = DateTime.Now.ToString("yyyy-MM-dd");
                        OmxModel.SerDateE = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                    }
                    if (OmxModel.ProdectType.StartsWith("0201"))//新增手册
                    {
                        #region 内控手册
                        Nmodel.SaleOrderID = Omodel.Id;//合同编号
                        Nmodel.CustomerID = model.ID;//客户编号
                        Nmodel.dwqc = model.Name;
                        Nmodel.NkscDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        Nmodel.version = bll.GetMaxVersion();

                        string NkscDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string version = bll.GetMaxVersion();
                        string sqlNksc = Nmodel.ToString();
                        tsqls.Add(sqlNksc, null);
                        #endregion
                    }
                    else if (OmxModel.ProdectType.StartsWith("0204"))//更新手册
                    {
                        Nksc_Update nkup = new Nksc_Update();
                        nkup.CustomerID = model.ID;
                        nkup.NkscDate = DateTime.Now.ToString("yyyy-MM-dd");
                        nkup.versionS = bll.GetNameStr("version", "and CustomerID='" + model.ID + "'");
                        nkup.versionE = bll.GetMaxVersion();
                        nkup.UpdateFlag = "0";
                        string sqlNkscG = nkup.ToString();
                        tsqls.Add(sqlNkscG, null);
                    }
                    else if (OmxModel.ProdectType.StartsWith("0205"))//加印手册
                    {
                        //xyzdsum，bczdsum，sysum
                        string sql = "update Nksc set xyzdsum=xyzdsum+" + OmxModel.ItemCount + ",sysum=xyzdsum+" + OmxModel.ItemCount + "-bczdsum where CustomerID='" + model.ID + "'";
                        tsqls.Add(sql, null);
                    }
                }

                string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney]"
                    + ",[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
                    + " VALUES('" + OmxModel.OrderId + "','" + OmxModel.ItemId + "','" + OmxModel.ProdectType + "','" + OmxModel.ProdectDesc
                    + "','" + OmxModel.ItemCount + "','" + OmxModel.ItemPrice + "','" + OmxModel.ItemMoney + "','" + OmxModel.TaxMoney
                    + "','" + OmxModel.PresentMoney + "','" + OmxModel.OtherMoney + "','" + 0.00 + "','" + OmxModel.Service
                    + "','" + OmxModel.SerDateS + "','" + OmxModel.SerDateE + "','000106','')";
                tsqls.Add(sqlItem, null);
                #endregion
            }

            if (dt.Rows.Count < 1)
            {
                result += "未找到要导入的数据。\n";
            }
            else if (string.IsNullOrEmpty(result))
            {
                if (bll.Tran(tsqls))
                {
                    result = "OK成功导入(" + dt.Rows.Count + "）条数据。";
                }
                else
                {
                    result += "导入失败。";
                }
            }

            if (result.StartsWith("OK"))
            {
                return Json(JsonHandler.CreateMessage(1, result.Replace("OK", "")), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, result), JsonRequestBehavior.AllowGet);
            }
        }

        #region 上传附件
        [HttpPost]
        public JsonResult UploadXls()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            string fileName = Request["name"].Replace("、", "-");
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/UploadExcel/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MergeXls()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/UploadExcel/");//Upload 文件夹
            var fileName = Request["fileName"].Replace("、", "-");//文件名
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

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取SheetName
        [HttpPost]
        public ActionResult GetExcelSheetName(string filename)
        {
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "UploadExcel");
            List<string> list = ExcelHelper.GetSheetName(localPath + "\\" + filename);
            List<ExcelModel> models = new List<ExcelModel>();
            for (int i = 0; i < list.Count; i++)
            {
                ExcelModel model = new ExcelModel();
                model.id = i.ToString("00");
                model.name = list[i];
                models.Add(model);
            }
            return Json(models);
        }
        #endregion

        #endregion

        public ActionResult SWFView(string id)
        {
            NkscBLL bll = new NkscBLL();
            string swfName = bll.GetNameStr("swfName", " and id='" + id + "'");
            if (string.IsNullOrEmpty(swfName))
            {
                ViewBag.fileName = "empty";
                ViewBag.maxpage = 1;
            }
            else
            {
                ViewBag.fileName = swfName;
                string path = Server.MapPath("\\UserPDF\\");
                if (!System.IO.File.Exists(path + swfName + ".pdf"))
                {
                    ViewBag.maxpage = 1;
                }
                else
                {
                    PDFFile doc = PDFFile.Open(path + swfName + ".pdf");
                    //Document doc = new Document(path + swfName + ".pdf");
                    ViewBag.maxpage = doc.PageCount;
                }
            }
            return View();
        }

        ImageSaveOptions iso = new ImageSaveOptions(SaveFormat.Jpeg);

        public ContentResult WordToSwf(string swfName, int pageindex)
        {
            string path = Server.MapPath("\\UserPDF\\");
            string FilePath = "";
            bool bl = false;
            if (!Directory.Exists(path + swfName))//判断文件夹是否存在
            {
                Directory.CreateDirectory(path + swfName);
                bl = true;
            }
            else
            {
                if (!System.IO.File.Exists(path + swfName + "\\" + swfName + pageindex + ".jpg"))//判断页数是否存在
                {
                    bl = true;
                }
            }
            if (bl)
            {
                //Document doc = new Document(path + swfName + ".docx");
                ////MemoryStream WStream = new MemoryStream();
                //iso.PrettyFormat = true;
                //iso.ImageColorMode = ImageColorMode.None;
                //iso.PageIndex = pageindex;
                //iso.Resolution = 97;
                //doc.Save(path + swfName + "\\" + swfName + pageindex + ".jpg", iso);
                //FilePath = "/UserPDF/" + swfName + "/" + swfName + pageindex + ".jpg";
                if (!System.IO.File.Exists(path + swfName + ".pdf"))
                {
                    FilePath = "/UserPDF/null.jpg";
                }
                else
                {
                    PDFFile doc = PDFFile.Open(path + swfName + ".pdf");
                    // 56 * ( 1到10 )
                    Bitmap pageImage = doc.GetPageImage(pageindex, 56 * (int)5);
                    pageImage.Save(path + swfName + "\\" + swfName + pageindex + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    pageImage.Dispose();
                    FilePath = "/UserPDF/" + swfName + "/" + swfName + pageindex + ".jpg";
                }
            }
            else
            {
                FilePath = "/UserPDF/" + swfName + "/" + swfName + pageindex + ".jpg";
            }

            //System.Threading.Thread Thread2 = new System.Threading.Thread(() =>
            //{
            //    PdftoImg(pageindex, swfName);
            //});
            //Thread2.Start();

            return Content(FilePath);
        }

        private void PdftoImg(int pageindex, string swfName)
        {
            string path = Server.MapPath("\\UserPDF\\");

            PDFFile doc = PDFFile.Open(path + swfName + ".pdf");

            int min = pageindex < 2 ? 0 : pageindex;
            int max = pageindex + 5 >= doc.PageCount ? doc.PageCount : pageindex + 5;

            for (int i = min; i < max; i++)
            {
                if (!System.IO.File.Exists(path + swfName + "\\" + swfName + i + ".jpg"))//判断页数是否存在
                {
                    Bitmap pageImage = doc.GetPageImage(i, 56 * (int)5);
                    pageImage.Save(path + swfName + "\\" + swfName + i + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    pageImage.Dispose();
                }
            }
        }

        /// <summary>
        /// 上传完文件后，执行数据库操作
        /// </summary>
        /// <returns></returns>
        public ActionResult Merge_Swf()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/UserPDF/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
            var finalPath = Path.Combine(uploadDir, fileName);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）
            if (System.IO.File.Exists(finalPath))
            {
                System.IO.File.Delete(finalPath);
            }
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

            NkscBLL bll = new NkscBLL();
            string[] Z_M = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string str_name = "";
            for (int i = 0; i < Z_M.Length; i++)
            {
                if (fileName.ToLower().StartsWith(Z_M[i]))
                {
                    str_name = fileName.ToLower().TrimStart(Convert.ToChar(Z_M[i]));
                    break;
                }
            }
            SaleCustomerBLL scbll = new SaleCustomerBLL();
            string strname = str_name.Split('.')[0];
            string fname = strname.Substring(0, strname.Length - 8);
            string cusId = scbll.GetNameStr("ID", "and Name='" + fname + "'");
            //判断当前上传文件与前一次是否一致
            string FileNames = fileName.Split('.')[0];//当前上传文件名
            string swfname = bll.GetNameStr("swfName", "and CustomerID='" + cusId + "'");//上次上传文件名
            if (!string.IsNullOrEmpty(swfname))
            {
                if (swfname != FileNames)//判断最近两次上传的文件是否一致
                {
                    //删除上一次查看手册的文件夹
                    string path = Server.MapPath("\\UserPDF\\");
                    if (Directory.Exists(path + swfname))
                    {
                        Directory.Delete(path + swfname, true);
                    }
                }
            }
            //判断、修改上传字段
            if (string.IsNullOrEmpty(cusId))
            {
                System.IO.File.Delete(finalPath);
                return Json(JsonHandler.CreateMessage(0, str_name + " 未找到"), JsonRequestBehavior.AllowGet);
            }
            bll.Update("update Nksc set swfName='" + FileNames + "' where CustomerID='" + cusId + "'");

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        #region 快递管理

        [HttpPost]
        public JsonResult GetData_Exp_Express(string OrderId, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(OrderId))
            {
                where += "and OrderId = '" + OrderId + "'";
            }
            Exp_ExpressBLL bll = new Exp_ExpressBLL();
            List<Exp_Express> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_Exp_Express(string OrderId, string LogisticCode, bool AddType = false)
        {
            Exp_ExpressBLL bll = new Exp_ExpressBLL();
            ViewBag.AddType = AddType;
            Exp_Express result = new Exp_Express();
            if (!AddType)
            {
                string where = "and LogisticCode='" + LogisticCode + "' and OrderId='" + OrderId + "'";
                result = bll.GetRow(where);
            }
            else
            {
                SaleCustomerBLL scbll = new SaleCustomerBLL();
                DataTable dt = scbll.SelectCustom(OrderId);

                result.OrderId = OrderId;
                result.ShipperCode = "YD";
                result.ReceiverName = dt.Rows[0]["Lxr"].ToStringEx();
                result.Tel = "";
                result.Mobile = dt.Rows[0]["Phone"].ToStringEx();
                result.ProvinceName = dt.Rows[0]["SfName"].ToStringEx();
                result.CityName = dt.Rows[0]["CityName"].ToStringEx().IndexOf("市") > 0 ? dt.Rows[0]["CityName"].ToStringEx() : dt.Rows[0]["UpName"].ToStringEx();
                result.ExpAreaName = dt.Rows[0]["CityName"].ToStringEx().IndexOf("市") > 0 ? "" : dt.Rows[0]["CityName"].ToStringEx();
                result.Address = dt.Rows[0]["Address"].ToStringEx();
                result.GoodsName = "发票*1 合同*1 手册*2";
                result.State = "0";
                result.Reason = "";
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_Exp_Express(Exp_Express model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.LogisticCode))
            {
                return Json(JsonHandler.CreateMessage(0, " 运单号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ShipperCode))
            {
                return Json(JsonHandler.CreateMessage(0, " 快递公司编码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.OrderId))
            {
                return Json(JsonHandler.CreateMessage(0, " 合同编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ReceiverName))
            {
                return Json(JsonHandler.CreateMessage(0, " 收件人 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Tel) && string.IsNullOrEmpty(model.Mobile))
            {
                return Json(JsonHandler.CreateMessage(0, " 座机号与手机号必须录入一项"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ProvinceName))
            {
                return Json(JsonHandler.CreateMessage(0, " 收件省 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.CityName))
            {
                return Json(JsonHandler.CreateMessage(0, " 收件市 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            //if (string.IsNullOrEmpty(model.ExpAreaName))
            //{
            //    return Json(JsonHandler.CreateMessage(0, " 收件区 不允许为空"), JsonRequestBehavior.AllowGet);
            //}
            if (string.IsNullOrEmpty(model.Address))
            {
                return Json(JsonHandler.CreateMessage(0, " 详细地址 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            //if (string.IsNullOrEmpty(model.GoodsName))
            //{
            //    return Json(JsonHandler.CreateMessage(0, " 物品名称 不允许为空"), JsonRequestBehavior.AllowGet);
            //}

            model.Tel = string.IsNullOrEmpty(model.Tel) ? "" : model.Tel;
            model.Mobile = string.IsNullOrEmpty(model.Mobile) ? "" : model.Mobile;
            model.ExpAreaName = string.IsNullOrEmpty(model.ExpAreaName) ? "" : model.ExpAreaName;
            model.GoodsName = string.IsNullOrEmpty(model.GoodsName) ? "" : model.GoodsName;
            model.Reason = string.IsNullOrEmpty(model.Reason) ? "" : model.Reason;
            Exp_ExpressBLL bll = new Exp_ExpressBLL();
            if (AddType)
            {
                model.ExpressTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (bll.Insert(model) > 0)
                {
                    string url = "http://www.baeit.cn/Express/Index?LogisticCode=" + model.LogisticCode;
                    string resultMsg = HttpHelper.HttpGet(url);
                    if (resultMsg != "成功")
                    {
                        return Json(JsonHandler.CreateMessage(0, "订阅失败：" + resultMsg), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LogHelper.AddLogUser(GetUserId(), "添加快递管理:" + model.LogisticCode, Suggestion.Succes, "快递管理");
                        return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加快递管理:" + model.LogisticCode, Suggestion.Error, "快递管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改快递管理:" + model.LogisticCode, Suggestion.Succes, "快递管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改快递管理:" + model.LogisticCode, Suggestion.Error, "快递管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region 物流
        public JsonResult Subscriber(string LogisticCode)
        {
            string url = "http://www.baeit.cn/Express/Index?LogisticCode=" + LogisticCode;
            string resultMsg = HttpHelper.HttpGet(url);
            if (resultMsg == "成功")
            {
                return Json(JsonHandler.CreateMessage(1, "订阅成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, "订阅失败:" + resultMsg), JsonRequestBehavior.AllowGet);
            }
        }
        //[SupportFilter]
        //public ActionResult Exp_Traces()
        //{
        //    ViewBag.Perm = GetPermission();
        //    AddLogLook("快递管理");
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetData_Exp_Traces(string , GridPager pager)
        //{
        //    string where = "";
        //    if (!string.IsNullOrEmpty())
        //    {
        //        where += "and  = '" +  + "'";
        //    }
        //    Exp_TracesBLL bll = new Exp_TracesBLL();
        //    List<Exp_Traces> result = bll.SelectAll(where, pager);
        //    var json = new
        //    {
        //        total = pager.totalRows,
        //        rows = result
        //    };
        //    return Json(json);
        //}
        #endregion

        /// <summary>
        /// 立即开票
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AddType"></param>
        /// <returns></returns>
        [SupportFilter(ActionName = "CUD")]//弹出窗体
        public ActionResult Create_SaleOrderTS(string Id, bool AddType = false)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            AddLogLook("合同管理立即开票");
            View_SaleOrder result = new View_SaleOrder();
            if (!AddType)
            {
                result = bll.GetRow(Id);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleOTS(string Id, string Flag)
        {
            SaleOrderBLL bll = new SaleOrderBLL();
            //修改订单的状态
            string TsDate = "";
            if (Flag == "1")
            {
                TsDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string tsql = "update SaleOrder set Fp='" + TsDate + "' where Id='" + Id + "'";
            //修改
            if (bll.Update(tsql) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "合同管理立即开票:" + Id, Suggestion.Succes, "合同管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "合同管理立即开票:" + Id, Suggestion.Error, "合同管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [SupportFilter]
        public ActionResult SaleJcDw()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("检查单位");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_SaleJcDw(string Name, string DiQuS, string userS, string dengjS, string Industry, string zyxS, string YearID, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Name.Trim()))
            {
                where += "and Name like '%" + Name.Trim() + "%'";
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
            if (!string.IsNullOrEmpty(userS.Trim()))
            {
                where += "and Ywy like '%" + userS.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(dengjS.Trim()))
            {
                where += "and CustomerGrade = '" + dengjS.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(Industry.Trim()))
            {
                where += "and Industry = '" + Industry.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(zyxS.Trim()))
            {
                where += "and Zyx = '" + zyxS.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(YearID.Trim()))
            {
                where += "and Content = '" + YearID.Trim() + "'";
            }
            InspectBLL bll = new InspectBLL();
            List<View_Inspect> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        /// <summary>
        /// 检查单位
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AddType"></param>
        /// <returns></returns>
        [SupportFilter(ActionName = "CUD")]//弹出窗体
        public ActionResult Create_SaleJcDw(string Id, bool AddType = false)
        {
            InspectBLL bll = new InspectBLL();
            AddLogLook("销售管理检查单位");
            View_Inspect result = new View_Inspect();
            if (!AddType)
            {
                result = bll.GetRow(Id);
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_SaleJcDw(Nksc_inspect model)
        {
            InspectBLL bll = new InspectBLL();
            if (string.IsNullOrEmpty(model.Id))
            {
                //创建
                model.Id = bll.MaxId();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加查检单位统计", Suggestion.Succes, "销售管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加查检单位统计", Suggestion.Error, "销售管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改查检单位统计", Suggestion.Succes, "销售管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改查检单位统计", Suggestion.Error, "销售管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 删除检查单位
        /// </summary>
        /// <param name="Id">编号</param>
        /// <returns></returns>
        public JsonResult Delete_SaleJcDw(string Id)
        {
            InspectBLL bll = new InspectBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除检查单位:" + Id, Suggestion.Succes, "客户管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除检查单位:" + Id, Suggestion.Error, "客户管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetComb_Year()
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "Id", order = "desc" };
            InspectBLL bll = new InspectBLL();
            IList<Nksc_inspect> list = bll.GetComb("", pager);
            return Json(list);
        }

        #region 回访管理

        [SupportFilter]
        public ActionResult CrsVisit()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("回访管理");

            //获取当前用户角色ID
            string roleId = GetUserRoleID();
            ViewBag.userS = "";
            //03 业务员
            if (roleId == "03")
            {
                ViewBag.userS = GetUserId();
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetData_CrsVisit(string Vyear, string DiQu, string Industry, string VisitType, string Saler, string Falg, string CustomerGrade, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Vyear))
            {
                where += "and Vyear = '" + Vyear + "'";
            }
            if (!string.IsNullOrEmpty(DiQu))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in DiQu.Split(','))
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
            if (!string.IsNullOrEmpty(Industry))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in Industry.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "Industry = '" + item + "'";
                    }
                    else
                    {
                        dqwhere += " or Industry = '" + item + "'";
                    }
                }
                where += "(" + dqwhere + ")";
            }
            if (!string.IsNullOrEmpty(CustomerGrade))
            {
                where += "and CustomerGrade = '" + CustomerGrade + "'";
            }
            if (!string.IsNullOrEmpty(VisitType))
            {
                if (VisitType == "1")
                {
                    where += "and VisitType <> '0'";
                }
                else
                {
                    where += "and VisitType = '" + VisitType + "'";
                }
            }
            if (!string.IsNullOrEmpty(Saler) && Saler != "全部")
            {
                where += "and Saler like '%" + Saler + "%'";
            }
            if (!string.IsNullOrEmpty(Falg))
            {
                where += "and Falg = '" + Falg + "'";
            }
            CrsVisitBLL bll = new CrsVisitBLL();
            List<View_CrsVisit> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetData_CrsVisit_His(string SaleCustomId, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(SaleCustomId))
            {
                where += "and CustomID = '" + SaleCustomId + "'";
            }

            CrsVisitBLL bll = new CrsVisitBLL();
            List<View_CrsVisit> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult HFD_CrsVisit(string Id, bool AddType = false)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            ViewBag.Id = Id;
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult HFD_CrsVisit(string Id, string VisitDate, string VisitGood, string Remark)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            if (string.IsNullOrEmpty(Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(VisitDate))
            {
                return Json(JsonHandler.CreateMessage(0, "回访日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(VisitGood))
            {
                return Json(JsonHandler.CreateMessage(0, "回访物品 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            tsqls.Add("update CrsVisit set Falg='1',VisitDate='" + VisitDate + "',VisitGood='" + VisitGood + "',Remark='" + Remark + "' where Id='" + Id + "'", null);

            if (bll.Tran(tsqls))
            {
                LogHelper.AddLogUser(GetUserId(), "添加回访单:" + Id, Suggestion.Succes, "回访单");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "添加回访单:" + Id, Suggestion.Error, "回访单");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Add_CrsVisit(string Vyear, string VisitType, string Saler = "")
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            ViewBag.Vyear = Vyear;
            ViewBag.VisitType = VisitType;
            ViewBag.Saler = Saler;
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Add_CrsVisit(string Id, string Vyear, string VisitType, string remake)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            if (string.IsNullOrEmpty(Id))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            Dictionary<string, object> tsqls = new Dictionary<string, object>();

            SaleCustomerBLL sbll = new SaleCustomerBLL();

            string Saler = sbll.GetNameStr("YwyName", "and ID='" + Id + "'");
            decimal bPay = sbll.GetMoney(" and SaleCustomId='" + Id + "' and OrderDate like '" + Vyear + "%'");
            decimal uPay = sbll.GetMoney(" and SaleCustomId='" + Id + "' and OrderDate like '" + (int.Parse(Vyear) - 1) + "%'");
            CrsVisit model = new CrsVisit();
            model.Id = Guid.NewGuid();
            model.Vyear = int.Parse(Vyear);
            model.Saler = Saler;
            model.CustomID = Id;
            model.Falg = "0";// 0未回访 1已回访
            model.VisitType = VisitType;// 0上门 1电话
            model.ByearPay = bPay;
            model.UpyearPay = uPay;
            model.SumPay = 0.00M;
            model.VisitDate = "";
            model.VisitGood = "";
            model.Remark = "";
            tsqls.Add(model.ToString(), null);

            if (bll.Tran(tsqls))
            {
                LogHelper.AddLogUser(GetUserId(), "添加回访客户:" + Id, Suggestion.Succes, "回访客户");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "添加回访客户:" + Id, Suggestion.Error, "回访客户");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_CrsVisit(string PId, CrsVisit result, bool AddType = false)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            ViewBag.AddType = AddType;
            //CrsVisit result = new CrsVisit { Id = Id };
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
        public JsonResult Create_CrsVisit(bool Lxxf, int Vyear, string DiQu, string Industry, string VisitType, string VisitTypeSequel
            , string ItemMoneyS, string ItemMoneyE, string ItemMoneySup, string ItemMoneyEup)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            DataTable dt = new DataTable();

            string where = "";
            if (!string.IsNullOrEmpty(ItemMoneyS))
            {
                where += " and bmoney>=" + ItemMoneyS;
            }
            if (!string.IsNullOrEmpty(ItemMoneyE))
            {
                where += " and bmoney<=" + ItemMoneyE;
            }
            if (!string.IsNullOrEmpty(ItemMoneySup))
            {
                where += " and umoney>=" + ItemMoneySup;
            }
            if (!string.IsNullOrEmpty(ItemMoneyEup))
            {
                where += " and umoney<=" + ItemMoneyEup;
            }

            if (!string.IsNullOrEmpty(DiQu))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in DiQu.Split(','))
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
            if (!string.IsNullOrEmpty(Industry))
            {
                where += " and ";
                string dqwhere = "";
                foreach (string item in Industry.Split(','))
                {
                    if (dqwhere == "")
                    {
                        dqwhere += "Industry = '" + item + "'";
                    }
                    else
                    {
                        dqwhere += " or Industry = '" + item + "'";
                    }
                }
                where += "(" + dqwhere + ")";
            }

            //获取当前用户角色ID
            string roleId = GetUserRoleID();

            Dictionary<string, object> tsqls = new Dictionary<string, object>();

            BasicSetBLL bllper = new BasicSetBLL();
            string userid = GetUserId();
            BasicSet modelBs = new BasicSet();
            if (bllper.isExist(" and userid='" + userid + "'"))
            {
                modelBs = bllper.GetRow(userid);
            }
            else
            {
                modelBs = bllper.GetRow("000001");
                modelBs.Userid = userid;
            }

            if (VisitType == "0")
            {
                string whereYwy = "";
                if (roleId == "03")
                {
                    whereYwy += " and Ywy like '%" + userid + "%'";
                }

                dt = bll.GetVisit(Vyear, Lxxf, where, whereYwy);

                //主管单位个数
                DataRow[] drZ = dt.Select("GradeName='主管单位'");
                //Math.Ceiling 向上取整
                int countZ = (int)Math.Ceiling((double)drZ.Length * (double)modelBs.PercentZ / 100);//80%
                int[] aryZ = UseHashTableToNonRepeatedRandom(countZ, 0, drZ.Length);

                for (int i = 0; i < aryZ.Length; i++)
                {
                    CrsVisit model = new CrsVisit();
                    model.Id = Guid.NewGuid();
                    model.Vyear = Vyear;
                    model.Saler = drZ[aryZ[i]]["YwyName"].ToString();
                    model.CustomID = drZ[aryZ[i]]["ID"].ToString();
                    model.Falg = "0";// 0未回访 1已回访
                    model.VisitType = VisitType;// 0上门 1电话  2财务软件  3内控客户
                    model.ByearPay = Convert.ToDecimal(drZ[aryZ[i]]["bmoney"]);
                    model.UpyearPay = Convert.ToDecimal(drZ[aryZ[i]]["umoney"]);
                    model.SumPay = 0.00M;
                    model.VisitDate = "";
                    model.VisitGood = "";
                    model.Remark = "";
                    tsqls.Add(model.ToString(), null);
                }

                //一般单位个数
                DataRow[] drY = dt.Select("GradeName='一般单位'");
                int countY = (int)Math.Ceiling((double)drY.Length * (double)modelBs.PercentY / 100);//20%
                int[] aryY = UseHashTableToNonRepeatedRandom(countY, 0, drY.Length);

                for (int i = 0; i < aryY.Length; i++)
                {
                    CrsVisit model = new CrsVisit();
                    model.Id = Guid.NewGuid();
                    model.Vyear = Vyear;
                    model.Saler = drY[aryY[i]]["YwyName"].ToString();
                    model.CustomID = drY[aryY[i]]["ID"].ToString();
                    model.Falg = "0";// 0未回访 1已回访
                    model.VisitType = VisitType;// 0上门 1电话  2财务软件  3内控客户
                    model.ByearPay = Convert.ToDecimal(drY[aryY[i]]["bmoney"]);
                    model.UpyearPay = Convert.ToDecimal(drY[aryY[i]]["umoney"]);
                    model.SumPay = 0.00M;
                    model.VisitDate = "";
                    model.VisitGood = "";
                    model.Remark = "";
                    tsqls.Add(model.ToString(), null);
                }
            }
            else
            {
                double percent = 0;

                string whereYwy = "";
                if (roleId == "03")
                {
                    whereYwy += " and Ywy like '%" + userid + "%'";
                }

                string whereC = " where 1=1 ";
                VisitType = VisitTypeSequel;
                //2财务软件  3内控客户
                if (VisitType == "2")
                {
                    whereC += " and ProdectType like '01%'";
                    percent = (double)modelBs.PercentC / 100;
                }
                else
                {
                    whereC += " and ProdectType like '02%'";
                    percent = (double)modelBs.PercentN / 100;
                }
                dt = bll.GetVisitPhone(Vyear, Lxxf, where, whereC, VisitType, whereYwy);

                //Math.Ceiling 向上取整
                int countZ = (int)Math.Ceiling((double)dt.Rows.Count * percent);
                int[] aryZ = UseHashTableToNonRepeatedRandom(countZ, 0, dt.Rows.Count);
                for (int i = 0; i < aryZ.Length; i++)
                {
                    CrsVisit model = new CrsVisit();
                    model.Id = Guid.NewGuid();
                    model.Vyear = Vyear;
                    model.Saler = dt.Rows[aryZ[i]]["YwyName"].ToString();
                    model.CustomID = dt.Rows[aryZ[i]]["ID"].ToString();
                    model.Falg = "0";// 0未回访 1已回访
                    model.VisitType = VisitType;// 0上门 1电话  2财务软件  3内控客户
                    model.ByearPay = Convert.ToDecimal(dt.Rows[aryZ[i]]["bmoney"]);
                    model.UpyearPay = Convert.ToDecimal(dt.Rows[aryZ[i]]["umoney"]);
                    model.SumPay = 0.00M;
                    model.VisitDate = "";
                    model.VisitGood = "";
                    model.Remark = "";
                    tsqls.Add(model.ToString(), null);
                }
            }



            if (tsqls.Count > 0)
            {
                if (bll.Tran(tsqls))
                {
                    LogHelper.AddLogUser(GetUserId(), "添加回访管理:" + Vyear + "年度", Suggestion.Succes, "回访管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加回访管理:" + Vyear + "年度", Suggestion.Error, "回访管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "添加回访管理:" + Vyear + "年度", Suggestion.Error, "回访管理");
                return Json(JsonHandler.CreateMessage(0, "没有符合条件的单位"), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 利用Hashtable
        /// </summary>
        private int[] UseHashTableToNonRepeatedRandom(int length, int minValue, int maxValue)
        {
            System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random(seed);
            for (int i = 0; hashtable.Count < length; i++)
            {
                int nValue = random.Next(minValue, maxValue);
                if (!hashtable.ContainsValue(nValue))
                {
                    hashtable.Add(i, nValue);
                    //hashtable.Add(nValue, nValue);        // 将 key 和 value设置成一样的值，导致hashtable无法按添加顺序输出数组
                    //Console.WriteLine(nValue.ToString());
                }
            }
            int[] array = new int[hashtable.Count];
            hashtable.Values.CopyTo(array, 0);
            return array;
        }


        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_CrsVisit(string Id)
        {
            CrsVisitBLL bll = new CrsVisitBLL();
            if (bll.Delete(Id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除回访管理:" + Id, Suggestion.Succes, "回访管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除回访管理:" + Id, Suggestion.Error, "回访管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 激活决算
        [HttpPost]
        public JsonResult GetJsKey()
        {
            string JsKey = string.Empty;
            string Code = string.Empty;
            string Lxr = string.Empty;
            string JsYear = string.Empty;
            try
            {
                int dataLen = Convert.ToInt32(Request.InputStream.Length);
                byte[] bytes = new byte[dataLen];
                Request.InputStream.Read(bytes, 0, dataLen);
                string requestStringData = Encoding.UTF8.GetString(bytes);
                string[] parmarters = requestStringData.Split('&');
                if (parmarters.Length != 4)
                {
                    return Json(JsonHandler.CreateMessage(0, "请求参数格式错误"), JsonRequestBehavior.AllowGet);
                }
                JsKey = parmarters[0].Split('=')[1];
                Code = parmarters[1].Split('=')[1];
                Lxr = parmarters[2].Split('=')[1];
                JsYear = parmarters[3].Split('=')[1];
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(JsKey))
            {
                return Json(JsonHandler.CreateMessage(0, "邀请码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Code))
            {
                return Json(JsonHandler.CreateMessage(0, "社会统一信用代码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Lxr))
            {
                return Json(JsonHandler.CreateMessage(0, "联系人 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(JsYear))
            {
                return Json(JsonHandler.CreateMessage(0, "年度 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                JsDataBLL bll = new JsDataBLL();
                JsData jsdata1 = bll.GetRow("select * from JsData where Code='" + Code + "' and Lxr='" + Lxr + "' and JsYear='" + JsYear + "'");
                if (jsdata1 != null)
                {
                    if (!string.IsNullOrEmpty(jsdata1.JsKey))
                    {
                        if (jsdata1.JsKey == JsKey)
                        {
                            return Json(JsonHandler.CreateMessage(1, jsdata1.RegKey), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(JsonHandler.CreateMessage(0, "本单位已激活无法再次激活,请正确填写激活信息"), JsonRequestBehavior.AllowGet);
                        }
                    }
                    string newkey = reconcile(JsKey);
                    newkey += "*" + jsdata1.Vlevel + "*" + jsdata1.JsYear;
                    string result = Encrypt(newkey);

                    bll.Update("update JsData set JsKey='" + JsKey + "',RegKey='" + result + "' where Code='" + Code + "' and Lxr='" + Lxr + "' and JsYear='" + JsYear + "' and Vlevel='" + jsdata1.Vlevel + "'");

                    return Json(JsonHandler.CreateMessage(1, result), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "未找到需要激活的单位信息,请正确填写激活信息"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error + ":" + ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_JsKey(string CId)
        {
            SaleCustomerBLL cusbll = new SaleCustomerBLL();
            AddLogLook("激活决算");
            string Cname = cusbll.GetNameStr("Name", "and ID='" + CId + "'");
            ViewBag.CId = CId;
            ViewBag.Cname = Cname;
            return View();
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Save_JsKeyOrder(string CDate, string CId, string AccountId, decimal HTMoney
            , string Jsyear, string Key, string Vlevel, string UserKey, string Ywy)
        {
            SaleCustomerBLL cusbll = new SaleCustomerBLL();
            SaleOrderBLL bll = new SaleOrderBLL();

            string date = DateTime.Now.ToString("yyyyMMdd");
            string uid = GetUserId();

            string regText = string.Format("决算年度：{0}  邀请码：{1}  激活码：{2}", Jsyear, Key, UserKey);
            string UpCusSql = "update SaleCustom set [Desc]=(case when [Desc] is null then '' else [Desc] end)+' " + regText + "' where ID='" + CId + "'";

            List<S_Order> lis_orders = new List<S_Order>();
            S_Order odMain = new S_Order();

            odMain.KhName = cusbll.GetNameStr("Name", "and ID='" + CId + "'");

            odMain.OrderMain.OrderDate = CDate;//订单日期
            odMain.OrderMain.Id = bll.Maxid(date);//订单编号
            odMain.OrderMain.SaleCustomId = CId;//客户
            odMain.OrderMain.Saler = Ywy;//业务员
            odMain.OrderMain.Fp = "0";//是否立即开票
            odMain.OrderMain.AccountId = AccountId;//账户
            odMain.OrderMain.UserId = uid;//操作员
            odMain.OrderMain.Remake = "";

            SaleOrderItem OmxModel = new SaleOrderItem();
            OmxModel.OrderId = odMain.OrderMain.Id;
            OmxModel.ItemId = odMain.OrderMain.Id + "01";
            OmxModel.ProdectType = "0503";
            OmxModel.ProdectDesc = "决算辅助";
            OmxModel.ItemPrice = HTMoney;
            OmxModel.ItemCount = 1;
            //成交金额=单价*数量
            OmxModel.ItemMoney = OmxModel.ItemCount * OmxModel.ItemPrice;
            //税金
            OmxModel.TaxMoney = OmxModel.ItemMoney * 0.05M;
            //礼品礼金
            OmxModel.PresentMoney = 0.00M;
            //其他金额
            OmxModel.OtherMoney = 0.00M;

            OmxModel.Service = "是";
            OmxModel.SerDateS = DateTime.Now.ToString("yyyy-MM-dd");
            OmxModel.SerDateE = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

            odMain.OrderItems.Add(OmxModel);
            lis_orders.Add(odMain);

            Dictionary<string, object> tsql = create_tsqls(lis_orders, null, UpCusSql);

            try
            {
                //修改
                if (bll.Tran(tsql))
                {
                    LogHelper.AddLogUser(GetUserId(), "激活决算建档", Suggestion.Succes, "客户管理");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "激活决算建档", Suggestion.Error, "客户管理");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message, ""), JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_JsKey(string Jsyear, string Key, string Vlevel)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return Json(JsonHandler.CreateMessage(0, "邀请码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Jsyear))
            {
                return Json(JsonHandler.CreateMessage(0, "年度 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Vlevel))
            {
                return Json(JsonHandler.CreateMessage(0, "客户级别 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            try
            {
                string newkey = reconcile(Key);
                newkey += "*" + Vlevel + "*" + Jsyear;
                string result = Encrypt(newkey);
                LogHelper.AddLogUser(GetUserId(), "激活决算", Suggestion.Succes, "客户管理");
                return Json(JsonHandler.CreateMessage(1, result), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                LogHelper.AddLogUser(GetUserId(), "激活决算", Suggestion.Error, "客户管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error + ":" + ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 解开 字符串
        /// </summary>
        /// <param name="cup"></param>
        /// <param name="disk"></param>
        /// <returns></returns>
        private string reconcile(string bkey)
        {
            string newKey = string.Empty;
            for (int i = 0; i < bkey.Length; i++)
            {
                if ((i + 1) % 5 != 0)
                {
                    newKey += bkey[i];
                }
            }
            return newKey.Replace("#", "-");
        }

        private string Encrypt(string value)
        {
            byte[] rgbKey = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = Encoding.UTF8.GetBytes(value);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        #endregion

        public JsonResult TY_http()
        {
            string httpPath = "http://localhost:63912/WeiXinApi/Return_XinXi";
            string par = "{ID:\"20171231000510\"}";
            //HttpHelper http = new HttpHelper(httpPath);
            string result = Post(httpPath, par, "text/html;charset=UTF-8", Encoding.UTF8);
            return Json(result);
        }

        public string Post(string uri, string data, string ContentType, Encoding encoding)
        {
            return CommonHttpRequest(uri, data, "POST", ContentType, encoding);
        }

        public string CommonHttpRequest(string uri, string data, string type, string ContentType, Encoding encoding)
        {
            //Web访问对象，构造请求的url地址
            string serviceUrl = uri;

            //构造http请求的对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            //转成网络流
            byte[] buf = encoding.GetBytes(data);// System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);
            //设置
            myRequest.Method = type;
            myRequest.ContentLength = buf.Length;
            myRequest.ContentType = ContentType; //"application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;
            // 发送请求
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(buf, 0, buf.Length);
            newStream.Close();
            // 获得接口返回值
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string ReturnXml = reader.ReadToEnd();
            reader.Close();
            myResponse.Close();
            return ReturnXml;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.BLL;
using JMProject.Model;
using JMProject.Model.View;
using JMProject.Common;
using System.Text;
using System.IO;
using JMProject.Model.Sys;
using JMProject.Web.AttributeEX;
using JMProject.Model.Esayui;
using System.Data;

namespace JMProject.Web.Controllers
{
    public class NkscController : BaseController
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NkscPrint(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Content("编号不允许为空");
            }
            NkscBLL bll = new NkscBLL();
            string where = " and id='" + id + "'";
            View_Full model = new View_Full();
            model.NkscModel = bll.GetModel(where);
            if (string.IsNullOrEmpty(model.NkscModel.CustomerID))
            {
                return Content("单位内控手册信息不存在");
            }
            model.Nksc_fzModel = bll.SelectNkscfz(model.NkscModel.id);
            model.Nksc_ksModel = bll.SelectNkscks(model.NkscModel.id);
            model.Nksc_cghtsqModel = bll.SelectNksccg(model.NkscModel.id, "0");
            model.Nksc_ZxcghtsqModel = bll.SelectNksccg(model.NkscModel.id, "1");
            model.Nksc_ZcywModel = bll.SelectNksczc(model.NkscModel.id, "0");
            model.Nksc_CzZcywModel = bll.SelectNksczc(model.NkscModel.id, "1");
            model.Nksc_FczZcywModel = bll.SelectNksczc(model.NkscModel.id, "2");
            model.Nksc_JkywModel = bll.SelectNkscjk(model.NkscModel.id);
            model.Nksc_BxywModel = bll.SelectNkscbx(model.NkscModel.id);
            model.CustomModel = new SaleCustomerBLL().GetCustomRow(model.NkscModel.CustomerID);
            ViewBag.Invoice = model.CustomModel.Invoice;
            if (string.IsNullOrEmpty(model.NkscModel.dwqc))
            {
                model.NkscModel.dwqc = model.CustomModel.Name;//new SaleCustomerBLL().GetNameStr("Name", "and ID='" + model.NkscModel.CustomerID + "'");
            }
            return View(model);
        }

        //载入手册信息
        public ActionResult Scxx(string cid)
        {
            ViewBag.Manager = "0";
            string where = "";
            NkscBLL bll = new NkscBLL();
            if (string.IsNullOrEmpty(cid))
            {
                if (string.IsNullOrEmpty(GetUserId()))
                {
                    return Content("已超时，请重新登陆");
                }
                where = " and CustomerID='" + GetUserId() + "'";
            }
            else
            {
                where = " and id='" + cid + "'";
                ViewBag.Manager = "1";
            }
            View_Full model = new View_Full();
            model.NkscModel = bll.GetModel(where);
            if (string.IsNullOrEmpty(model.NkscModel.CustomerID))
            {
                return Content("单位内控手册信息不存在");
            }
            model.Nksc_fzModel = bll.SelectNkscfz(model.NkscModel.id);
            model.Nksc_ksModel = bll.SelectNkscks(model.NkscModel.id);
            model.Nksc_cghtsqModel = bll.SelectNksccg(model.NkscModel.id, "0");
            model.Nksc_ZxcghtsqModel = bll.SelectNksccg(model.NkscModel.id, "1");
            model.Nksc_ZcywModel = bll.SelectNksczc(model.NkscModel.id, "0");
            model.Nksc_CzZcywModel = bll.SelectNksczc(model.NkscModel.id, "1");
            model.Nksc_FczZcywModel = bll.SelectNksczc(model.NkscModel.id, "2");
            model.Nksc_JkywModel = bll.SelectNkscjk(model.NkscModel.id);
            model.Nksc_BxywModel = bll.SelectNkscbx(model.NkscModel.id);
            model.CustomModel = new SaleCustomerBLL().GetCustomRow(model.NkscModel.CustomerID);
            ViewBag.Invoice = model.CustomModel.Invoice;
            if (string.IsNullOrEmpty(model.NkscModel.dwqc))
            {
                model.NkscModel.dwqc = model.CustomModel.Name;//new SaleCustomerBLL().GetNameStr("Name", "and ID='" + model.NkscModel.CustomerID + "'");
            }
            model.Nksc_qlqdModel = bll.SelectQlqd(model.NkscModel.id.ToString());
            return View(model);
        }

        //修改手册信息
        [HttpPost]
        public ActionResult addNksc()
        {
            NkscBLL bll = new NkscBLL();
            Guid id = Guid.Parse(Request.Form["id"]);//编号
            string CustomerID = Request.Form["CustomerID"];//客户
            string Step = Request.Form["Step"];//向导页码
            Dictionary<string, object> tsqls = new Dictionary<string, object>();
            string tsql = "update Nksc set {0} where id='" + id + "'";
            #region 第一页 基础信息
            if (Step == "Step1")
            {
                string dwjc = Request.Form["dwjc"];//手册颁发日期
                string dwqc = Request.Form["dwqc"];//单位全称 发票抬头（Invoice）  
                string Invoice = Request.Form["Invoice"];
                string Code = Request.Form["Code"];//社会统一信用代码
                string Lxr = Request.Form["Lxr"];//发票收件人
                string Phone = Request.Form["Phone"];//手机号

                string QQ = Request.Form["QQ"];//QQ

                string Address = Request.Form["Address"];//邮寄地址
                string syfw0415 = Request.Form["syfw0415"]; //本手册适用范围
                //考勤时间
                string kqsjswS = Request.Form["kqsjswS"];
                string kqsjswE = Request.Form["kqsjswE"];
                string kqsjxwS = Request.Form["kqsjxwS"];
                string kqsjxwE = Request.Form["kqsjxwE"];
                string kqsjswSd = Request.Form["kqsjswSd"];
                string kqsjswEd = Request.Form["kqsjswEd"];
                string kqsjxwSd = Request.Form["kqsjxwSd"];
                string kqsjxwEd = Request.Form["kqsjxwEd"];
                string bhks = Request.Form["bhks"];//本单位包含所有科室
                string dwjj = Request.Form["dwjj"];//单位简介

                StringBuilder sql_cus = new StringBuilder();
                sql_cus.Append("update SaleCustom set ");
                sql_cus.Append("Name='" + dwqc + "',Code='" + Code + "'");
                sql_cus.Append(",QtLxr='" + Lxr + "',QtTel='" + Phone + "'");
                sql_cus.Append(",Invoice='" + Invoice + "',Address='" + Address + "' ");
                sql_cus.Append(",QQ='" + QQ + "' ");
                sql_cus.Append(" where ID='" + CustomerID + "'");
                tsqls.Add(sql_cus.ToString(), null);

                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("dwjc='" + dwjc + "'");
                sbnk.Append(",dwqc='" + dwqc + "'");
                sbnk.Append(",syfw0415='" + syfw0415 + "'");
                sbnk.Append(",kqsjswS='" + kqsjswS + "'");
                sbnk.Append(",kqsjswE='" + kqsjswE + "'");
                sbnk.Append(",kqsjxwS='" + kqsjxwS + "'");
                sbnk.Append(",kqsjxwE='" + kqsjxwE + "'");
                sbnk.Append(",kqsjswSd='" + kqsjswSd + "'");
                sbnk.Append(",kqsjswEd='" + kqsjswEd + "'");
                sbnk.Append(",kqsjxwSd='" + kqsjxwSd + "'");
                sbnk.Append(",kqsjxwEd='" + kqsjxwEd + "'");
                sbnk.Append(",bhks='" + bhks + "'");
                sbnk.Append(",dwjj='" + dwjj + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
            }
            #endregion
            #region 第二页 职务
            else if (Step == "Step2")
            {
                StringBuilder sbnk = new StringBuilder();
                string dzzjgmc = Request.Form["dzzjgmc"];
                string zzzwmc = Request.Form["zzzwmc"];
                string ldzzmc = Request.Form["ldzzmc"];
                string ldzzfg = Request.Form["ldzzfg"];
                string zzzwDY = Request.Form["zzzwDY"];
                string fzzwmc1 = Request.Form["fzzwmc1"];
                string ldfzmc1 = Request.Form["ldfzmc1"];
                string ldfzfg1 = Request.Form["ldfzfg1"];
                string fzzwDY = Request.Form["fzzwDY"];
                sbnk.Append("dzzjgmc='" + dzzjgmc + "'");
                sbnk.Append(",zzzwmc='" + zzzwmc + "'");
                sbnk.Append(",ldzzmc='" + ldzzmc + "'");
                sbnk.Append(",ldzzfg='" + ldzzfg + "'");
                sbnk.Append(",zzzwDY='" + zzzwDY + "'");
                sbnk.Append(",fzzwmc1='" + fzzwmc1 + "'");
                sbnk.Append(",ldfzmc1='" + ldfzmc1 + "'");
                sbnk.Append(",ldfzfg1='" + ldfzfg1 + "'");
                sbnk.Append(",fzzwDY='" + fzzwDY + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
                //副职
                string sort = Request.Form["sort"];
                string fzzwmc2 = Request.Form["fzzwmc"];
                string ldfzmc2 = Request.Form["ldfzmc"];
                string ldfzfg2 = Request.Form["ldfzfg"];
                string fzzwDY2 = Request.Form["fzzwDYY"];
                bll.Insert_fz(tsqls, id, sort, fzzwmc2, ldfzmc2, ldfzfg2, fzzwDY2);
            }
            #endregion
            #region 第三页 内控小组
            else if (Step == "Step3")
            {
                string scqtbm = Request.Form["scqtbm"];
                string scxzbm = Request.Form["scxzbm"];
                string nkldxzcy = Request.Form["nkldxzcy"];
                string nkldxzzz = Request.Form["nkldxzzz"];
                string nkldxzfzz = Request.Form["nkldxzfzz"];
                string nbkzgzxzzz01 = Request.Form["nbkzgzxzzz01"];
                string nbkzgzxzfzz01 = Request.Form["nbkzgzxzfzz01"];
                string nbkzgzxzzzcy01 = Request.Form["nbkzgzxzzzcy01"];
                string nbkzgzxzzzqt01 = Request.Form["nbkzgzxzzzqt01"];
                string fxpgxzqtks = Request.Form["fxpgxzqtks"];
                string fxpgxzcy = Request.Form["fxpgxzcy"];
                string fxpgxzzz = Request.Form["fxpgxzzz"];
                string fxpgxzfzz = Request.Form["fxpgxzfzz"];
                string ysldxzcy = Request.Form["ysldxzcy"];
                string ysldxzqdks = Request.Form["ysldxzqdks"];
                string ysldxzzz = Request.Form["ysldxzzz"];
                string ysldxzfzz = Request.Form["ysldxzfzz"];
                string zfcgxzcy = Request.Form["zfcgxzcy"];
                string zfcgxzqdks = Request.Form["zfcgxzqdks"];
                string zfcgxzzz = Request.Form["zfcgxzzz"];
                string zfcgxzfzz = Request.Form["zfcgxzfzz"];
                string gyzcxzcy = Request.Form["gyzcxzcy"];
                string gyzcxzqdks = Request.Form["gyzcxzqdks"];
                string gyzcxzzz = Request.Form["gyzcxzzz"];
                string gyzcxzfzz = Request.Form["gyzcxzfzz"];
                string jdjcxzcy = Request.Form["jdjcxzcy"];
                string jdjcxzqdks = Request.Form["jdjcxzqdks"];
                string jdjcxzzz = Request.Form["jdjcxzzz"];
                string jdjcxzfzz = Request.Form["jdjcxzfzz"];

                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("scqtbm='" + scqtbm + "'");
                sbnk.Append(",scxzbm='" + scxzbm + "'");

                sbnk.Append(",nkldxzcy='" + nkldxzcy + "'");
                sbnk.Append(",nkldxzzz='" + nkldxzzz + "'");
                sbnk.Append(",nkldxzfzz='" + nkldxzfzz + "'");

                sbnk.Append(",nbkzgzxzzz01='" + nbkzgzxzzz01 + "'");
                sbnk.Append(",nbkzgzxzfzz01='" + nbkzgzxzfzz01 + "'");
                sbnk.Append(",nbkzgzxzzzcy01='" + nbkzgzxzzzcy01 + "'");
                sbnk.Append(",nbkzgzxzzzqt01='" + nbkzgzxzzzqt01 + "'");

                sbnk.Append(",fxpgxzqtks='" + fxpgxzqtks + "'");
                sbnk.Append(",fxpgxzcy='" + fxpgxzcy + "'");
                sbnk.Append(",fxpgxzzz='" + fxpgxzzz + "'");
                sbnk.Append(",fxpgxzfzz='" + fxpgxzfzz + "'");

                sbnk.Append(",ysldxzcy='" + ysldxzcy + "'");
                sbnk.Append(",ysldxzqdks='" + ysldxzqdks + "'");
                sbnk.Append(",ysldxzzz='" + ysldxzzz + "'");
                sbnk.Append(",ysldxzfzz='" + ysldxzfzz + "'");

                sbnk.Append(",zfcgxzcy='" + zfcgxzcy + "'");
                sbnk.Append(",zfcgxzqdks='" + zfcgxzqdks + "'");
                sbnk.Append(",zfcgxzzz='" + zfcgxzzz + "'");
                sbnk.Append(",zfcgxzfzz='" + zfcgxzfzz + "'");

                sbnk.Append(",gyzcxzcy='" + gyzcxzcy + "'");
                sbnk.Append(",gyzcxzqdks='" + gyzcxzqdks + "'");
                sbnk.Append(",gyzcxzzz='" + gyzcxzzz + "'");
                sbnk.Append(",gyzcxzfzz='" + gyzcxzfzz + "'");

                sbnk.Append(",jdjcxzcy='" + jdjcxzcy + "'");
                sbnk.Append(",jdjcxzqdks='" + jdjcxzqdks + "'");
                sbnk.Append(",jdjcxzzz='" + jdjcxzzz + "'");
                sbnk.Append(",jdjcxzfzz='" + jdjcxzfzz + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
            }
            #endregion
            #region 第四页 牵头归口科室
            else if (Step == "Step4")
            {
                string bhyw = Request.Form["bhyw"];//本单位包含业务内容
                string ywslzdmc = Request.Form["ywslzdmc"];//业务梳理制度的包括业务流程名称 
                string nbsjks = Request.Form["nbsjks"];//内部审计负责科室名称
                string zdjcsshpjks = Request.Form["zdjcsshpjks"];//重大决策实施后评价负责科室
                string bxrgwzdks = Request.Form["bxrgwzdks"];//不相容岗位分离制度解释科室
                string bzndlgjhks = Request.Form["bzndlgjhks"];//编制年度岗位轮岗计划的科室
                string bnlgdgwmc = Request.Form["bnlgdgwmc"];//不能轮岗的岗位包括哪些?
                string zdbgdgkks = Request.Form["zdbgdgkks"];//重大事项内部报告的归口管理科室
                string zwxxgkks = Request.Form["zwxxgkks"];//政务信息公开制度牵头科室
                string xxgkzrjjks = Request.Form["xxgkzrjjks"];//信息公开责任追究归口科室
                string fzxxglxtks = Request.Form["fzxxglxtks"];//负责单位信息管理系统的科室
                string xxxcgzqtks = Request.Form["xxxcgzqtks"];//新闻宣传工作牵头管理科室
                string ksglks = Request.Form["ksglks"];//考勤管理的归口科室
                string lzjmytgkks = Request.Form["lzjmytgkks"];//廉政诫勉约谈的归口科室 
                string bdwsrbk = Request.Form["bdwsrbk"];//本单位收入包括哪些内容
                string srywgkks = Request.Form["srywgkks"];//收入业务的归口管理部门
                string jfzcgkks = Request.Form["jfzcgkks"];//经费支出的归口管理部门 
                string zfcgzlgkks = Request.Form["zfcgzlgkks"];//政府采购资料的保管归口科室
                string rsglzdgkks = Request.Form["rsglzdgkks"];//人事管理制度的归口科室
                string rsglhbks = Request.Form["rsglhbks"];//人事管理的回避规定监督归口科室
                string nzkhgkks = Request.Form["nzkhgkks"];//年终考核string开展民主评议的归口科室
                string zdbgcdks = Request.Form["zdbgcdks"];//重大事项报告存档科室
                string htgkks1 = Request.Form["htgkks1"];//合同管理的归口科室
                string gwglgkks = Request.Form["gwglgkks"];//公文管理归口科室
                string gdzccz = Request.Form["gdzccz"];//固定资产处置的归口科室
                string gdzcdb = Request.Form["gdzcdb"];//固定资产调拨的归口科室
                string gdzcgz = Request.Form["gdzcgz"];//固定资产购置的归口科室
                string gdzcqc = Request.Form["gdzcqc"];//固定资产清查的归口科室
                string bgypglgkks = Request.Form["bgypglgkks"];//办公用品管理归口科室
                string yzglgkks = Request.Form["yzglgkks"];//印章管理归口科室
                string gwkzd = Request.Form["gwkzd"];//公务卡制度
                string gwkglks = Request.Form["gwkglks"];//公务卡制度管理归口科室
                string gwkjdks = Request.Form["gwkjdks"];//公务卡监督执行归口科室
                string EngineRoom = Request.Form["EngineRoom"];//是否有机房
                string zxzjgl = Request.Form["zxzjgl"];//是否需要财政专项资金管理办法
                string czzxzjgkks = Request.Form["czzxzjgkks"];//财政专项资金归口科室
                string jsxmgkks01 = Request.Form["jsxmgkks01"];//建设项目归口主管科室
                string jsxmjxpjks01 = Request.Form["jsxmjxpjks01"];//建设项目绩效评价管理科室

                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("bhyw='" + bhyw + "'");//本单位包含业务内容
                sbnk.Append(",ywslzdmc='" + ywslzdmc + "'");//业务梳理制度的包括业务流程名称
                sbnk.Append(",nbsjks='" + nbsjks + "'");//内部审计负责科室名称
                sbnk.Append(",zdjcsshpjks='" + zdjcsshpjks + "'");//重大决策实施后评价负责科室
                sbnk.Append(",bxrgwzdks='" + bxrgwzdks + "'");//不相容岗位分离制度解释科室
                sbnk.Append(",bzndlgjhks='" + bzndlgjhks + "'");//编制年度岗位轮岗计划的科室
                sbnk.Append(",bnlgdgwmc='" + bnlgdgwmc + "'");//不能轮岗的岗位包括哪些?
                sbnk.Append(",zdbgdgkks='" + zdbgdgkks + "'");//重大事项内部报告的归口管理科室
                sbnk.Append(",zwxxgkks='" + zwxxgkks + "'");//政务信息公开制度牵头科室
                sbnk.Append(",xxgkzrjjks='" + xxgkzrjjks + "'");//信息公开责任追究归口科室
                sbnk.Append(",fzxxglxtks='" + fzxxglxtks + "'");//负责单位信息管理系统的科室
                sbnk.Append(",xxxcgzqtks='" + xxxcgzqtks + "'");//新闻宣传工作牵头管理科室
                sbnk.Append(",ksglks='" + ksglks + "'");//考勤管理的归口科室
                sbnk.Append(",lzjmytgkks='" + lzjmytgkks + "'");//廉政诫勉约谈的归口科室 
                sbnk.Append(",bdwsrbk='" + bdwsrbk + "'");//本单位收入包括哪些内容
                sbnk.Append(",srywgkks='" + srywgkks + "'");//收入业务的归口管理部门
                sbnk.Append(",jfzcgkks='" + jfzcgkks + "'");//经费支出的归口管理部门 
                sbnk.Append(",zfcgzlgkks='" + zfcgzlgkks + "'");//政府采购资料的保管归口科室
                sbnk.Append(",rsglzdgkks='" + rsglzdgkks + "'");//人事管理制度的归口科室
                sbnk.Append(",rsglhbks='" + rsglhbks + "'");//人事管理的回避规定监督归口科室
                sbnk.Append(",nzkhgkks='" + nzkhgkks + "'");//年终考核string开展民主评议的归口科室
                sbnk.Append(",zdbgcdks='" + zdbgcdks + "'");//重大事项报告存档科室
                sbnk.Append(",htgkks1='" + htgkks1 + "'");//合同管理的归口科室
                sbnk.Append(",gwglgkks='" + gwglgkks + "'");//公文管理归口科室
                sbnk.Append(",gdzccz='" + gdzccz + "'");//固定资产处置的归口科室
                sbnk.Append(",gdzcdb='" + gdzcdb + "'");//固定资产调拨的归口科室
                sbnk.Append(",gdzcgz='" + gdzcgz + "'");//固定资产购置的归口科室
                sbnk.Append(",gdzcqc='" + gdzcqc + "'");//固定资产清查的归口科室
                sbnk.Append(",bgypglgkks='" + bgypglgkks + "'");//办公用品管理归口科室
                sbnk.Append(",yzglgkks='" + yzglgkks + "'");//印章管理归口科室
                sbnk.Append(",gwkzd='" + gwkzd + "'");//公务卡制度
                sbnk.Append(",gwkglks='" + gwkglks + "'");//公务卡制度管理归口科室
                sbnk.Append(",gwkjdks='" + gwkjdks + "'");//公务卡监督执行归口科室
                sbnk.Append(",EngineRoom='" + EngineRoom + "'");//是否有机房
                sbnk.Append(",zxzjgl='" + zxzjgl + "'");//是否需要财政专项资金管理办法
                sbnk.Append(",czzxzjgkks='" + czzxzjgkks + "'");//财政专项资金归口科室
                sbnk.Append(",jsxmgkks01='" + jsxmgkks01 + "'");//建设项目归口主管科室
                sbnk.Append(",jsxmjxpjks01='" + jsxmjxpjks01 + "'");//建设项目绩效评价管理科室
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
            }
            #endregion
            #region 第五页 资金审批
            else if (Step == "Step5")
            {
                //政府采购合同授权审批设置
                string Radio_cghtsq = Request.Form["Radio_cghtsq"];
                string cghtMoney = Request.Form["cghtMoney"];
                string cghtType = Request.Form["cghtType"];
                string cghtSpr = Request.Form["cghtSpr"];
                //自行采购合同授权审批设置
                string Radio_zxcgsq = Request.Form["Radio_zxcgsq"];
                string zxcghtMoney = Request.Form["zxcghtMoney"];
                string zxcghtType = Request.Form["zxcghtType"];
                string zxcghtSpr = Request.Form["zxcghtSpr"];
                //资金支付审批权限：支出管理制度
                string Radio_zjzf = Request.Form["Radio_zjzf"];
                string zcywMoney = Request.Form["zcywMoney"];
                string zcywType = Request.Form["zcywType"];
                string zcywSpr = Request.Form["zcywSpr"];
                //财政专项资金支付审批权限：支出管理制度
                string Radio_czzxzj = Request.Form["Radio_czzxzj"];
                string czzcywMoney = Request.Form["czzcywMoney"];
                string czzcywType = Request.Form["czzcywType"];
                string czzcywSpr = Request.Form["czzcywSpr"];
                //非财政专项资金审批权限：支出管理制度
                string Radio_fczzxzj = Request.Form["Radio_fczzxzj"];
                string fczzcywMoney = Request.Form["fczzcywMoney"];
                string fczzcywType = Request.Form["fczzcywType"];
                string fczzcywSpr = Request.Form["fczzcywSpr"];
                //借款审批权限：支出管理制度 
                string Radio_jksh = Request.Form["Radio_jksh"];
                string jkywMoney = Request.Form["jkywMoney"];
                string jkywType = Request.Form["jkywType"];
                string jkywSpr = Request.Form["jkywSpr"];
                //报销审批权限：资金支出审批管理办法 
                string Radio_bxsh = Request.Form["Radio_bxsh"];
                string bxywMoney = Request.Form["bxywMoney"];
                string bxywType = Request.Form["bxywType"];
                string bxywSpr = Request.Form["bxywSpr"];
                string jine041509 = Request.Form["jine041509"];//零星支出
                string jine0407 = Request.Form["jine0407"];//固定资产 一般设备
                string jine0408 = Request.Form["jine0408"];//固定资产 专用设备

                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("Radio_cghtsq='" + Radio_cghtsq + "'");
                sbnk.Append(",Radio_zxcgsq='" + Radio_zxcgsq + "'");
                sbnk.Append(",Radio_zjzf='" + Radio_zjzf + "'");
                sbnk.Append(",Radio_czzxzj='" + Radio_czzxzj + "'");
                sbnk.Append(",Radio_fczzxzj='" + Radio_fczzxzj + "'");
                sbnk.Append(",Radio_jksh='" + Radio_jksh + "'");
                sbnk.Append(",Radio_bxsh='" + Radio_bxsh + "'");
                sbnk.Append(",jine041509='" + jine041509 + "'");
                sbnk.Append(",jine0407='" + jine0407 + "'");
                sbnk.Append(",jine0408='" + jine0408 + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);

                bll.Insert_Cght(tsqls, id, cghtMoney, cghtType, cghtSpr, "0");//政府采购合同
                bll.Insert_Cght(tsqls, id, zxcghtMoney, zxcghtType, zxcghtSpr, "1");//自行采购合同
                bll.Insert_Zc(tsqls, id, zcywMoney, zcywType, zcywSpr, "0");//预算内资金支出
                bll.Insert_Zc(tsqls, id, czzcywMoney, czzcywType, czzcywSpr, "1");//财政专项资金支出
                bll.Insert_Zc(tsqls, id, fczzcywMoney, fczzcywType, fczzcywSpr, "2");//非财政专项资金支出
                bll.Insert_Jk(tsqls, id, jkywMoney, jkywType, jkywSpr);//借款
                bll.Insert_Bx(tsqls, id, bxywMoney, bxywType, bxywSpr);//报销
            }
            #endregion
            #region 第六页 科室职能岗位职责
            else if (Step == "Step6")
            {
                string kssort = Request.Form["kssort"];
                string ksmc = Request.Form["ksmc"];
                string kszn = Request.Form["kszn"];
                string kszr = Request.Form["kszr"];
                bll.Insert_ks(tsqls, id, kssort, ksmc, kszn, kszr);
            }
            #endregion
            #region 第七页 制度与附件(差旅费、培训费等等)
            else if (Step == "Step7")
            {
                string Radioclf = Request.Form["Radioclf"];
                string Radiohyf = Request.Form["Radiohyf"];
                string Radiopxf = Request.Form["Radiopxf"];
                string Radiogwzdf = Request.Form["Radiogwzdf"];
                string Radiobzz = Request.Form["Radiobzz"];

                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("Radioclf='" + Radioclf + "'");
                sbnk.Append(",Radiohyf='" + Radiohyf + "'");
                sbnk.Append(",Radiopxf='" + Radiopxf + "'");
                sbnk.Append(",Radiogwzdf='" + Radiogwzdf + "'");
                sbnk.Append(",Radiobzz='" + Radiobzz + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
            }
            #endregion
            #region 第八页 权力清单
            else if (Step == "Step8")
            {
                tsqls.Add("delete from Nksc_qlqd where id='" + id + "'", null);
                string[] parmaters = Request.Form.AllKeys;

                for (int i = 0; i < parmaters.Length; i++)
                {
                    string parName = parmaters[i];
                    if (parName == "id" || parName == "CustomerID" || parName == "Step")
                    {
                        continue;
                    }
                    string[] itemAry = parName.Split('_');

                    Nksc_qlqd model = new Nksc_qlqd();
                    model.id = id;
                    model.qlsx = itemAry[1];
                    model.qlsxname = itemAry[2];
                    model.leder = itemAry[3];
                    model.qltext = Request.Form[parName];
                    model.qlsort = int.Parse(itemAry[4]);
                    tsqls.Add(model.ToString(), null);
                }
            }
            #endregion
            #region 末页 提交手册
            else if (Step == "StepEnd")
            {
                string flag = Request.Form["flag"];
                string NkscSBDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sbnk = new StringBuilder();
                sbnk.Append("flag='" + flag + "'");
                sbnk.Append(",NkscSBDate='" + NkscSBDate + "'");
                sbnk.Append(",version='" + bll.GetMaxVersion() + "'");
                tsqls.Add(string.Format(tsql, sbnk.ToString()), null);
            }
            #endregion
            if (bll.Tran(tsqls))
            {
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        //手册更新
        public ActionResult Scgx(string cid)
        {
            return Content("建设中...");
            ViewBag.Manager = "0";
            string where = "";
            NkscBLL bll = new NkscBLL();
            Nksc_UpdateBLL ubll = new Nksc_UpdateBLL();
            if (string.IsNullOrEmpty(cid))
            {
                if (string.IsNullOrEmpty(GetUserId()))
                {
                    return Content("已超时，请重新登陆");
                }
                where = " and CustomerID='" + GetUserId() + "'";
            }
            else
            {
                where = " and id='" + cid + "'";
                ViewBag.Manager = "1";
            }
            View_Full model = new View_Full();
            model.NkscModel = bll.GetModel(where);
            if (string.IsNullOrEmpty(model.NkscModel.CustomerID))
            {
                return Content("单位内控手册信息不存在");
            }
            else
            {
                if (!ubll.isExist("and CustomerID='" + model.NkscModel.CustomerID + "' and UpdateFlag='0'"))
                {
                    return Content("您未更新手册！");
                }
                else
                {
                    ViewBag.versionS = ubll.GetNameStr("versionS", "and CustomerID='" + model.NkscModel.CustomerID + "' and UpdateFlag='0'");
                    if (ViewBag.versionS == "000")
                    {
                        //return Content("您手册版本过低，请单击左侧【手册信息】，补全全部信息并提交。");
                    }
                    ViewBag.versionE = ubll.GetNameStr("versionE", "and CustomerID='" + model.NkscModel.CustomerID + "' and UpdateFlag='0'");
                }
            }
            model.Nksc_fzModel = bll.SelectNkscfz(model.NkscModel.id);
            model.Nksc_ksModel = bll.SelectNkscks(model.NkscModel.id);
            model.Nksc_cghtsqModel = bll.SelectNksccg(model.NkscModel.id, "0");
            model.Nksc_ZxcghtsqModel = bll.SelectNksccg(model.NkscModel.id, "1");
            model.Nksc_ZcywModel = bll.SelectNksczc(model.NkscModel.id, "0");
            model.Nksc_CzZcywModel = bll.SelectNksczc(model.NkscModel.id, "1");
            model.Nksc_FczZcywModel = bll.SelectNksczc(model.NkscModel.id, "2");
            model.Nksc_JkywModel = bll.SelectNkscjk(model.NkscModel.id);
            model.Nksc_BxywModel = bll.SelectNkscbx(model.NkscModel.id);
            model.CustomModel = new SaleCustomerBLL().GetCustomRow(model.NkscModel.CustomerID);
            ViewBag.Invoice = model.CustomModel.Invoice;
            if (string.IsNullOrEmpty(model.NkscModel.dwqc))
            {
                model.NkscModel.dwqc = model.CustomModel.Name;
            }
            model.Nksc_qlqdModel = bll.SelectQlqd(model.NkscModel.id.ToString());
            return View(model);
        }

        #region 版本
        //省份
        [HttpPost]
        public ActionResult Get_CombVersion()
        {
            string where = "";
            GridPager pager = new GridPager { page = 1, rows = 50, sort = "versionS", order = "asc" };
            NkscBLL bll = new NkscBLL();
            IList<Nksc_Version> list = bll.SelectAllVersion(where, pager);
            list.Insert(0, new Nksc_Version { versionS = "全部" });
            return Json(list);
        }
        #endregion

        #region 附件管理

        #region 上传附件
        [HttpPost]
        public ActionResult Upload()
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
            var dir = Server.MapPath("~/Upload/" + GetAccount().Name);//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Merge()
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
            var uploadDir = Server.MapPath("~/Upload/" + GetAccount().Name);//Upload 文件夹
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


            nksc.Update("update Nksc set yhscfj=yhscfj+'" + fileName + "、' where CustomerID='" + uid + "'");

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除附件

        [HttpPost]
        public ActionResult DeleteFJ(string name)
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
                NkscBLL nksc = new NkscBLL();
                string uid = GetUserId();
                string yhscfj = nksc.GetNameStr("yhscfj", " and CustomerID='" + uid + "'");
                yhscfj = yhscfj.Replace(name + "、", "");
                nksc.Update("update Nksc set yhscfj='" + yhscfj + "' where CustomerID='" + uid + "'");
                var uploadDir = Server.MapPath("~/Upload/" + GetAccount().Name);//Upload 文件夹
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

        #region 文档管理
        public ActionResult DownFile()
        {
            ViewBag.Admin = "0";//0客户 1管理员
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Content("已超时，请重新登陆");
            }
            if (uid.Length == 14)
            {
                string flagDown = new NkscBLL().GetNameStr("flagDown", " and CustomerID='" + uid + "'");
                if (flagDown != "1")
                {
                    return Content("您没有权限下载文档！");
                }
            }
            else
            {
                ViewBag.Admin = "1";
            }
            ManagementBLL mll = new ManagementBLL();
            List<Management> models = mll.SelectAll();
            return View(models);
        }

        #region 上传文档管理
        [HttpPost]
        public ActionResult Upload_DownFile()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();

            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/DownFile/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Merge_DownFile()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/DownFile/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
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

            ManagementBLL bll = new ManagementBLL();
            bll.Insert("insert into Management values('" + fileName + "')");
            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除附件

        [HttpPost]
        public ActionResult Delete_DownFile(string name)
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
                NkscBLL nksc = new NkscBLL();

                nksc.Delete("Delete from Management where name='" + name + "'");
                var uploadDir = Server.MapPath("~/DownFile/");//Upload 文件夹
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

        #region 问题反馈
        public ActionResult WTFK()
        {
            ViewBag.flag = "";
            ViewBag.wtfkFlag = "";
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Content("已超时，请重新登陆");
            }
            NkscBLL bll = new NkscBLL();
            string ncid = bll.GetNameStr("id", " and CustomerID='" + uid + "'");
            if (string.IsNullOrEmpty(ncid))
            {
                return Content("单位内控手册信息不存在,无法反馈问题");
            }
            Guid id = Guid.Parse(ncid);
            ViewBag.flag = bll.GetNameStr("flag", " and id='" + id + "'");
            ViewBag.wtfkFlag = bll.GetNameStr("wtfkFlag", " and id='" + id + "'");
            List<Nksc_wtfk> result = bll.SelectNkscWtfk(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Nksc_Wtfk(string id)
        {
            NkscBLL bll = new NkscBLL();
            List<Nksc_wtfk> result = bll.SelectNkscWtfk(Guid.Parse(id));
            return Json(result);
        }

        [HttpPost]
        public ActionResult Upload_wtfk()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string cusName = new SaleCustomerBLL().GetNameStr("Name", "and id='" + uid + "'");
            string fileName = cusName + "(问题反馈)" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/DownWTFK/" + GetAccount().Name);//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, fileName), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Merge_wtfk()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();

            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/DownWTFK/" + GetAccount().Name);//Upload 文件夹
            var fileName = Request["fileName"];//文件名
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

            Guid nkscid = Guid.Parse(nksc.GetNameStr("id", " and CustomerID='" + uid + "'"));

            Nksc_wtfk model = new Nksc_wtfk();
            model.id = nkscid;
            model.fkid = nksc.maxFkid(DateTime.Now.ToString("yyyyMMdd"));
            model.riqi = DateTime.Now.ToString("yyyy-MM-dd");
            model.wtFile = fileName;
            model.flags = "0";
            nksc.Insert_wtfk(model);
            return Json(JsonHandler.CreateMessage(1, fileName, model.fkid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete_wtfk(string fkid, string name)
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(name))
            {
                return Json(JsonHandler.CreateMessage(0, "要删除的文件名为空"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                NkscBLL nksc = new NkscBLL();
                nksc.Delete("delete from Nksc_wtfk where fkid='" + fkid + "'");
                var uploadDir = Server.MapPath("~/DownWTFK/" + GetAccount().Name);//DownWTFK 文件夹
                var finalPath = Path.Combine(uploadDir, name);
                System.IO.File.Delete(finalPath);
                return Json(JsonHandler.CreateMessage(1, "删除成功"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 用户提交问题反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateWtfkZT()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            try
            {
                NkscBLL bll = new NkscBLL();
                string id = bll.GetNameStr("id", " and CustomerID='" + uid + "'");
                string wtfkFlag = bll.GetNameStr("wtfkFlag", " and CustomerID='" + uid + "'");
                if (wtfkFlag != "0")
                {
                    return Json(JsonHandler.CreateMessage(0, "您已提交问题反馈,不能再次提交问题反馈"), JsonRequestBehavior.AllowGet);
                }
                string flag = bll.GetNameStr("flag", " and CustomerID='" + uid + "'");
                if (flag == "6" || flag == "8")
                {
                    return Json(JsonHandler.CreateMessage(0, "[已定稿或已装订]的手册,不能提交问题反馈"), JsonRequestBehavior.AllowGet);
                }
                if (!bll.isExistwtfk("and id='" + id + "' and flags='0'"))
                {
                    return Json(JsonHandler.CreateMessage(0, "没有可提交问题反馈文档，请先上传问题反馈文档"), JsonRequestBehavior.AllowGet);
                }
                int count = 0;
                count = bll.Update("update Nksc_wtfk set flags='1' where id='" + id + "' and flags='0'");
                bll.Update("update Nksc set wtfkFlag='1' where id='" + id + "'");

                return Json(JsonHandler.CreateMessage(1, "提交成功"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 查看手册（Flash）
        public ActionResult PFDView(string id)
        {
            NkscBLL bll = new NkscBLL();
            string swfName = bll.GetNameStr("swfName", " and id='" + id + "'");
            if (string.IsNullOrEmpty(swfName))
            {
                ViewBag.fileName = "empty.swf";
            }
            else
            {
                ViewBag.fileName = swfName;
            }
            return View();
        }
        #endregion

        #region 内控手册分发(FLASH)
        public ActionResult NkscFF()
        {
            NkscBLL bll = new NkscBLL();
            List<S_SWF> result = bll.SelectAll_Swf();
            return View(result);
        }

        #region 上传 手册 FLASH
        [HttpPost]
        public ActionResult Upload_Swf()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }

            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/UserPDF/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

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
            string strname = str_name.Split('.')[0];
            string fname = strname.Substring(0, strname.Length - 8);
            string cusId = new SaleCustomerBLL().GetNameStr("ID", "and Name='" + fname + "'");
            if (string.IsNullOrEmpty(cusId))
            {
                System.IO.File.Delete(finalPath);
                return Json(JsonHandler.CreateMessage(0, fname + " 未找到"), JsonRequestBehavior.AllowGet);
            }
            bll.Update("update Nksc set swfName='" + fileName + "' where CustomerID='" + cusId + "'");

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 内控手册列表
        [SupportFilter]
        public ActionResult Nksc()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("手册管理");
            return View();
        }

        [HttpPost]
        public ActionResult Nksc_Data(string type, string DiQuS, string NameS, string flag, string fkflag, string riqiS, string riqiE, string NkscDateS, string NkscDateE
            , string NkscSBDateS, string NkscSBDateE, string fkpdfflag, string IsUpdate, string VersionS, GridPager pager)
        {
            string where = "";
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
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and dwqc like '%" + NameS.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(fkpdfflag) && fkpdfflag != "0")
            {
                if (fkpdfflag == "1")
                {
                    where += " and NkscDatePDF <> '' ";
                }
                else
                {
                    where += " and NkscDatePDF = '' ";
                }
            }
            if (!string.IsNullOrEmpty(flag) && flag != "0")
            {
                if (!flag.StartsWith("n"))
                {
                    where += " and flag = '" + flag + "' ";
                }
                else
                {
                    where += " and flag <> '" + flag.TrimStart('n') + "' ";
                }
            }
            if (!string.IsNullOrEmpty(fkflag) && fkflag != "0")
            {
                if (fkflag == "1")
                {
                    where += " and wtfkFlag = '0' ";
                }
                else if (fkflag == "2")
                {
                    where += " and wtfkFlag = '1' ";
                }
                else if (fkflag == "3")
                {
                    where += " and wtfkFlag = '2' ";
                }
            }

            if (!string.IsNullOrEmpty(riqiS))
            {
                where += " and zddate >= '" + riqiS + "'";
            }
            if (!string.IsNullOrEmpty(riqiE))
            {
                where += " and zddate <= '" + riqiE + "'";
            }

            if (!string.IsNullOrEmpty(NkscDateS))
            {
                where += " and NkscDate>='" + NkscDateS + "'";
            }
            if (!string.IsNullOrEmpty(NkscDateE))
            {
                where += " and NkscDate<='" + NkscDateE + "'";
            }

            if (!string.IsNullOrEmpty(NkscSBDateS))
            {
                where += " and NkscSBDate>='" + NkscSBDateS + "'";
            }
            if (!string.IsNullOrEmpty(NkscSBDateE))
            {
                where += " and NkscSBDate<='" + NkscSBDateE + "'";
            }
            if (IsUpdate == "1")
            {
                where += " and IsUpdate>0";
            }
            else if (IsUpdate == "2")
            {
                where += " and IsUpdate=0";
            }
            if (!string.IsNullOrEmpty(VersionS) && VersionS != "全部")
            {
                where += " and version='" + VersionS + "'";
            }
            NkscBLL bll = new NkscBLL();
            IList<View_Nksc> list = bll.SelectAll(where, pager);

            var griddata = new { total = pager.totalRows, rows = list };
            return Json(griddata);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult NkscFlag(string id, string flag
            //派发人 | 执行人
            , string pfName
            //协议装订数量  定稿描述
            , string zdsum, string txtbz
            //装订日期|发送PDF日期|手册领取日期  装订数量  剩余数量
            , string zddate, string bcsum, string sysum
            )
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(JsonHandler.CreateMessage(0, "请选择一个内控手册"), JsonRequestBehavior.AllowGet);
            }

            NkscBLL bll = new NkscBLL();
            try
            {
                int count = 0;
                if (flag == "1")//弃审
                {
                    count = bll.Update("update Nksc set flag='" + flag + "',NkscSBDate='' where id='" + id + "'");
                }
                else if (flag == "3")//派工
                {
                    if (string.IsNullOrEmpty(pfName))
                    {
                        return Json(JsonHandler.CreateMessage(0, "请输入派发员工名称"), JsonRequestBehavior.AllowGet);
                    }
                    count = bll.Update("update Nksc set flag='" + flag + "',pfr='" + pfName + "' where id='" + id + "'");
                }
                else if (flag == "6")//定稿确认
                {
                    count = bll.Update("update Nksc set flag='" + flag + "',xyzdsum=" + zdsum + ", bz='" + txtbz + "' where id='" + id + "'");
                }
                else if (flag == "8")//装订
                {
                    count = bll.Update("update Nksc set flag='" + flag + "',zddate='" + zddate + "',bczdsum=" + bcsum + ", bz='" + txtbz + "' where id='" + id + "'");
                }
                else if (flag == "10")//已发送PDF
                {
                    if (string.IsNullOrEmpty(zddate))
                    {
                        return Json(JsonHandler.CreateMessage(0, "请选择一个日期"), JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(pfName))
                    {
                        return Json(JsonHandler.CreateMessage(0, "请输入执行人"), JsonRequestBehavior.AllowGet);
                    }
                    count = bll.Update("update Nksc set NkscDatePDF='" + zddate + "',peoPDF='" + pfName + "' where id='" + id + "'");
                }
                else if (flag == "11")//手册领取
                {
                    if (string.IsNullOrEmpty(zddate))
                    {
                        return Json(JsonHandler.CreateMessage(0, "请选择一个日期"), JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(pfName))
                    {
                        return Json(JsonHandler.CreateMessage(0, "请输入执行人"), JsonRequestBehavior.AllowGet);
                    }
                    //修改更新记录为 完成状态
                    bll.Update("update Nksc_Update set UpdateFlag='1' where CustomerID=(select top 1 CustomerID from Nksc where id='" + id + "')");
                    count = bll.Update("update Nksc set flag='" + flag + "',NkscDateSC='" + zddate + "',peoSC='" + pfName + "' where id='" + id + "'");
                }
                else if (flag == "12")//已生成PDF
                {
                    count = bll.Update("update Nksc set NkscDateSCPDF='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where id='" + id + "'");
                }
                else//2初审 4用户核对中 5编制完成 13待定
                {
                    count = bll.Update("update Nksc set flag='" + flag + "' where id='" + id + "'");
                }
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, "操作成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "保存失败，数据无变化"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量下载附件
        /// </summary>
        /// <param name="NameS">用户名</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownFJ(string cid)
        {
            string path = Server.MapPath("~//Upload");
            string NameS = new SaleCustomerBLL().GetNameStr("UserName", "and ID='" + cid + "'");
            if (Directory.GetFiles(path + "\\" + NameS).Length < 1)
            {
                return Json(JsonHandler.CreateMessage(0, "没有可下载附件"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (System.IO.File.Exists(path + "\\" + NameS + ".rar"))
                {
                    System.IO.File.Delete(path + "\\" + NameS + ".rar");
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, "删除历史压缩附件失败"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                bool bl = ZipClass.ZipFile(path + "\\" + NameS + ".rar", path + "\\" + NameS);
                if (bl)
                {
                    return Json(JsonHandler.CreateMessage(1, NameS + ".rar"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "压缩失败"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 弃审问题反馈
        /// </summary>
        /// <param name="ids">id</param>
        /// <param name="flags">flags</param>
        /// <returns></returns>
        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult Nksc_Wtfk_Flag(string id, string flag)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(JsonHandler.CreateMessage(0, "请选择一个内控手册"), JsonRequestBehavior.AllowGet);
            }
            try
            {
                NkscBLL bll = new NkscBLL();
                int count = 0;
                if (flag == "0")
                {
                    count = bll.Update("update Nksc set wtfkFlag='" + flag + "' where id='" + id + "'");
                }
                else
                {
                    count = bll.Update("update Nksc_wtfk set flags='" + flag + "' where id='" + id + "'");
                    bll.Update("update Nksc set wtfkFlag='" + flag + "' where id='" + id + "'");
                }
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, "操作成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "提交失败，数据无变化"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        // 特殊描述
        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult UpdateTsyq(string id, string TsyqName)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(JsonHandler.CreateMessage(0, "请选择一个内控手册"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(TsyqName))
            {
                return Json(JsonHandler.CreateMessage(0, "请输入特殊要求描述"), JsonRequestBehavior.AllowGet);
            }

            NkscBLL bll = new NkscBLL();
            try
            {
                int count = 0;
                count = bll.Update("update Nksc set tsyqtext='" + TsyqName + "' where id='" + id + "'");
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, "操作成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "操作失败，数据无变化"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="DiQuS">地区</param>
        /// <param name="NameS">客户全称</param>
        /// <param name="flag">手册状态</param>
        /// <param name="fkflag">反馈状态</param>
        /// <param name="riqiS">开始日期</param>
        /// <param name="riqiE">结束日期</param>
        /// <returns></returns>        
        public ActionResult DaoCExcel(string DiQuS, string NameS, string flag, string fkflag, string riqiS, string riqiE, string NkscDateS, string NkscDateE
            , string NkscSBDateS, string NkscSBDateE, string fkpdfflag, string IsUpdate)
        {
            string where = "";
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
            if (!string.IsNullOrEmpty(NameS))
            {
                where += " and dwqc like '%" + NameS + "%' ";
            }
            if (!string.IsNullOrEmpty(fkpdfflag) && fkpdfflag != "0")
            {
                if (fkpdfflag == "1")
                {
                    where += " and NkscDatePDF <> '' ";
                }
                else
                {
                    where += " and NkscDatePDF = '' ";
                }
            }
            if (!string.IsNullOrEmpty(flag) && flag != "0")
            {
                if (!flag.StartsWith("n"))
                {
                    where += " and flag = '" + flag + "' ";
                }
                else
                {
                    where += " and flag <> '" + flag.TrimStart('n') + "' ";
                }
            }
            if (!string.IsNullOrEmpty(fkflag) && fkflag != "0")
            {
                if (fkflag == "1")
                {
                    where += " and wtfkbs = '0' ";
                }
                else if (fkflag == "2")
                {
                    where += " and wtfkbs <> '0' ";
                }
            }

            if (!string.IsNullOrEmpty(riqiS))
            {
                where += " and zddate >= '" + riqiS + "'";
            }
            if (!string.IsNullOrEmpty(riqiE))
            {
                where += " and zddate <= '" + riqiE + "'";
            }

            if (!string.IsNullOrEmpty(NkscDateS))
            {
                where += " and NkscDate>='" + NkscDateS + "'";
            }
            if (!string.IsNullOrEmpty(NkscDateE))
            {
                where += " and NkscDate<='" + NkscDateE + "'";
            }

            if (!string.IsNullOrEmpty(NkscSBDateS))
            {
                where += " and NkscSBDate>='" + NkscSBDateS + "'";
            }
            if (!string.IsNullOrEmpty(NkscSBDateE))
            {
                where += " and NkscSBDate<='" + NkscSBDateE + "'";
            }
            if (IsUpdate == "1")
            {
                where += " and IsUpdate>0";
            }
            else if (IsUpdate == "2")
            {
                where += " and IsUpdate=0";
            }

            NkscBLL bll = new NkscBLL();
            DataTable dt = bll.SelectDaoC(where);

            System.Web.UI.WebControls.DataGrid dgExport = null;
            // 当前对话 
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // IO用于导出并返回excel文件 
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;
            string filename = "内控导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
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

        #region 内控手册 更新历史记录
        [HttpPost]
        public JsonResult GetData_NkscUpdate(string CustomerID, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(CustomerID))
            {
                where += "and CustomerID = '" + CustomerID + "'";
            }
            Nksc_UpdateBLL bll = new Nksc_UpdateBLL();
            List<Nksc_Update> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }


        [HttpPost]
        public JsonResult Delete_NkscUpdate(string id)
        {
            Nksc_UpdateBLL bll = new Nksc_UpdateBLL();
            if (bll.Delete(id) > 0)
            {
                //LogHelper.AddLogUser(GetUserId(), "删除xx管理:" + id, Suggestion.Succes, "xx管理");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                //LogHelper.AddLogUser(GetUserId(), "删除xx管理:" + id, Suggestion.Error, "xx管理");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 授权
        [HttpPost]
        public ActionResult NkscFlagDown(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(JsonHandler.CreateMessage(0, "请选择一个合同"), JsonRequestBehavior.AllowGet);
            }

            NkscBLL bll = new NkscBLL();
            try
            {
                string Msg = "";
                string flagDown = bll.GetNameStr("flagDown", "and SaleOrderID='" + id + "'");
                if (string.IsNullOrEmpty(flagDown))
                {
                    return Json(JsonHandler.CreateMessage(0, "当前客户没有内控手册"), JsonRequestBehavior.AllowGet);
                }
                else if (flagDown == "0")
                {
                    flagDown = "1";
                    Msg = "授权成功";
                }
                else
                {
                    flagDown = "0";
                    Msg = "取消授权成功";
                }
                int count = 0;
                count = bll.Update("update Nksc set flagDown='" + flagDown + "' where SaleOrderID='" + id + "'");
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, Msg), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "保存失败，数据无变化"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 生成word文档
        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult BuildWord(string id, string stype, bool B5, bool x4)
        {
            #region 原来的
            try
            {
                NkscBLL bll = new NkscBLL();
                string uid = bll.GetNameStr("CustomerID", " and id='" + id + "'");
                Dictionary<string, string> datas = bll.GetKeyAndData(id);
                Dictionary<string, ImgModel> datasImg = bll.GetKeyAndDataImg(id, stype);
                string TempFile = "";

                string dwgl = bll.GetNameStr("dzzjgmc", " and id='" + id + "'").Trim();//党组织

                string bhyw = bll.GetNameStr("bhyw", " and id='" + id + "'");
                bool htgl = bhyw.IndexOf('8') >= 0;//合同管理
                bool jsxm = bhyw.IndexOf('7') >= 0;//建设项目
                bool zfcg = bhyw.IndexOf('5') >= 0;//政府采购
                bool sfyw = bhyw.IndexOf('a') >= 0;//收费业务
                bool zwyw = bhyw.IndexOf('b') >= 0;//债务业务
                bool dwtzyw = bhyw.IndexOf('c') >= 0;//对外投资业务

                bool ywcm_ysyw = bhyw.IndexOf('1') >= 0;// 预算业务
                bool ywcm_szyw_fssr = bhyw.IndexOf('a') >= 0;//非税收入
                bool ywcm_szyw_srdjrz = bhyw.IndexOf('d') >= 0;//收入登记入账
                bool ywcm_szyw_czpjgl = bhyw.IndexOf('e') >= 0;//票据管理
                bool ywcm_szyw_czsqzf = bhyw.IndexOf('g') >= 0;//财政授权支付
                bool ywcm_szyw_czzjzf = bhyw.IndexOf('f') >= 0;//财政直接支付
                bool ywcm_zfcg_zfgmff = bhyw.IndexOf('i') >= 0;//政府购买服务流程
                bool ywcm_zcgl_gyzcczcj = bhyw.IndexOf('k') >= 0;//国有资产出租出借业务
                bool ywcm_zcgl_gyzcsrsj = bhyw.IndexOf('m') >= 0;//国有资产收入上缴流程
                bool ywcm_zcgl_gyzcdjbgzx = bhyw.IndexOf('n') >= 0;//国有资产产权登记、变更、注销流程
                bool ywcm_zcgl_gyzccqjfdc = bhyw.IndexOf('o') >= 0;//国有资产产权纠纷调处流程
                bool ywcm_zcgl_zcpglc = bhyw.IndexOf('p') >= 0;//资产评估流程
                bool ywcm_jsxm_gkzbyqzb = bhyw.IndexOf('q') >= 0;//建设项目公开招标流程、邀请招标流程 
                bool ywcm_jsxm_sjbgqsqz = bhyw.IndexOf('r') >= 0;//建设项目设计变更、洽商签证流程


                bool iscgsq = bll.GetNameStr("Radio_cghtsq", " and id='" + id + "'") == "2";//采购合同审批
                bool iszxcgsq = bll.GetNameStr("Radio_zxcgsq", " and id='" + id + "'") == "2";//单位自行采购审批


                bool isEngineRoom = bll.GetNameStr("EngineRoom", " and id='" + id + "'") != "0";//是否有机房
                bool isgwk = bll.GetNameStr("gwkzd", " and id='" + id + "'") != "0";//是否有公务卡制度
                bool iszxzjgl = bll.GetNameStr("zxzjgl", " and id='" + id + "'") != "0";//是否需要财政专项资金管理办法

                bool isclf = bll.GetNameStr("Radioclf", " and id='" + id + "'") != "0";//差旅费
                bool ishyf = bll.GetNameStr("Radiohyf", " and id='" + id + "'") != "0";//会议费
                bool ispxf = bll.GetNameStr("Radiopxf", " and id='" + id + "'") != "0";//培训费
                bool iszdf = bll.GetNameStr("Radiogwzdf", " and id='" + id + "'") != "0";//招待费
                bool isbz = bll.GetNameStr("Radiobzz", " and id='" + id + "'") == "1";//报账制

                bool isjkyw = bll.GetNameStr("Radio_jksh", " and id='" + id + "'") != "2";//借款业务

                if (B5 || stype == "1")
                {
                    if (x4)
                    {
                        TempFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\tempB5-m4.docx");
                    }
                    else
                    {
                        TempFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\tempB5.docx");
                    }
                }
                else
                {
                    //TempFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\tempB5-m4.docx");

                    //随即 temp1 与 temp2
                    //string RandKey = NumHelper.GetDH("temps");
                    //NumHelper.UpdateDH("tempCount", "temps");
                    //TempFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\temp" + RandKey + ".docx");

                    TempFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\tempA4.docx");
                }

                string dpname = bll.GetNameStr("dwqc", "and id='" + id + "'");
                string fileName = dpname + DateTime.Now.ToString("yyyyMMdd") + ".docx";
                string newFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\" + fileName);
                string imgFileDir = Path.Combine(HttpRuntime.AppDomainAppPath, @"UserImg\" + uid + @"\");
                if (!Directory.Exists(imgFileDir))
                {
                    Directory.CreateDirectory(imgFileDir);
                }
                string imgFileDirGD = Path.Combine(HttpRuntime.AppDomainAppPath, @"GDimg\");

                WordHelperModel parameter = new WordHelperModel
                {
                    dwgl = dwgl,
                    sfyw = sfyw,
                    htgl = htgl,
                    jsxm = jsxm,
                    zfcg = zfcg,
                    zwyw = zwyw,
                    dwtzyw = dwtzyw,
                    isclf = isclf,
                    ishyf = ishyf,
                    ispxf = ispxf,
                    iszdf = iszdf,
                    isbz = isbz,
                    isjkyw = isjkyw,
                    isEngineRoom = isEngineRoom,
                    isgwk = isgwk,
                    iszxzjgl = iszxzjgl,
                    iscgsq = iscgsq,
                    iszxcgsq = iszxcgsq,
                    ywcm_ysyw = ywcm_ysyw,
                    ywcm_szyw_fssr = ywcm_szyw_fssr,
                    ywcm_szyw_srdjrz = ywcm_szyw_srdjrz,
                    ywcm_szyw_czpjgl = ywcm_szyw_czpjgl,
                    ywcm_szyw_czsqzf = ywcm_szyw_czsqzf,
                    ywcm_szyw_czzjzf = ywcm_szyw_czzjzf,
                    ywcm_zfcg_zfgmff = ywcm_zfcg_zfgmff,
                    ywcm_zcgl_gyzcczcj = ywcm_zcgl_gyzcczcj,
                    ywcm_zcgl_gyzcsrsj = ywcm_zcgl_gyzcsrsj,
                    ywcm_zcgl_gyzcdjbgzx = ywcm_zcgl_gyzcdjbgzx,
                    ywcm_zcgl_gyzccqjfdc = ywcm_zcgl_gyzccqjfdc,
                    ywcm_zcgl_zcpglc = ywcm_zcgl_zcpglc,
                    ywcm_jsxm_gkzbyqzb = ywcm_jsxm_gkzbyqzb,
                    ywcm_jsxm_sjbgqsqz = ywcm_jsxm_sjbgqsqz
                };

                string rst = WordHelper.ReplaceToWord(datas, datasImg, TempFile, newFile, imgFileDir, imgFileDirGD, parameter);

                if (!rst.StartsWith("Error"))
                {
                    bll.UpdateFileName(fileName, id);
                    return Json(JsonHandler.CreateMessage(1, "生成成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "生成失败：（" + rst + "）"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
            #endregion
        }

        #endregion

        // 检查单位保存
        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult InsertJcdw(string id, string jcnf, string CustomerID)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(JsonHandler.CreateMessage(0, "请选择一个内控手册"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(jcnf))
            {
                return Json(JsonHandler.CreateMessage(0, "请输入检查年份"), JsonRequestBehavior.AllowGet);
            }

            InspectBLL bll = new InspectBLL();
            Nksc_inspect model = new Nksc_inspect();
            try
            {
                int count = 0;
                model.Id = bll.MaxId();
                model.CustomerID = CustomerID;
                model.Content = jcnf;
                count = bll.Insert(model);
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, "操作成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "操作失败，数据无变化"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        #region 手册封皮
        public ActionResult NkscPhoto()
        {
            ViewBag.Perm = GetPermission();
            if (string.IsNullOrEmpty(GetUserId()))
            {
                return Content("已超时，请重新登陆");
            }
            string where = "";
            NkscBLL bll = new NkscBLL();
            where = " and CustomerID='" + GetUserId() + "'";
            string FpImage = bll.GetNameStr("FPImage", where);
            ViewBag.Flag = FpImage;
            ViewBag.FpImage = FpImage;
            AccountModel model = GetAccount();
            ViewBag.Name = model.Name;
            return View();
        }
        #endregion

        #region  手册封皮图片

        #region 上传附件
        [HttpPost]
        public ActionResult NkscFpUpload()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string nkscid = nksc.GetNameStr("id", " and CustomerID='" + uid + "'");
            if (nksc.GetZDFlag(nkscid))
            {
                return Json(JsonHandler.CreateMessage(0, "本手册已装订，无法上传手册封皮图片"), JsonRequestBehavior.AllowGet);
            }

            string fileName = Request["name"].Replace("、", "-");
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/UploadFengPi/" + GetAccount().Name);//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            data.SaveAs(filePath);//报错

            return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NkscFpMerge()
        {
            string uid = GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登陆"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string nkscid = nksc.GetNameStr("id", " and CustomerID='" + uid + "'");
            if (nksc.GetZDFlag(nkscid))
            {
                return Json(JsonHandler.CreateMessage(0, "本手册已装订，无法上传手册封皮图片"), JsonRequestBehavior.AllowGet);
            }
            var guid = Request["guid"];//GUID
            var fileName = Request["fileName"].Replace("、", "-");//文件名
            var uploadDir = Server.MapPath("~/UploadFengPi/" + GetAccount().Name);//Upload 文件夹
            string FpImage = nksc.GetNameStr("FPImage", " and CustomerID='" + uid + "'");
            try
            {
                System.IO.File.Delete(uploadDir + "/" + FpImage);
            }
            catch { }
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

            nksc.Update("update Nksc set FPImage='" + fileName + "' where CustomerID='" + uid + "'");

            return Json(JsonHandler.CreateMessage(1, "成功", GetAccount().Name), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除附件
        [HttpPost]
        public ActionResult DeleteNkscFp(string name)
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
                NkscBLL nksc = new NkscBLL();
                string uid = GetUserId();
                nksc.Update("update Nksc set FPImage='' where CustomerID='" + uid + "'");
                var uploadDir = Server.MapPath("~/UploadFengPi/" + GetAccount().Name);//Upload 文件夹
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
    }
}
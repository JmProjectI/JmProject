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
using Newtonsoft.Json;
using Microsoft.International.Converters.PinYinConverter;
using System.Data;
using System.IO;

namespace JMProject.Web.Controllers
{
    public class WeiXinApiController : BaseController
    {
        /// <summary>
        /// 自动生成用户名和密码
        /// </summary>
        /// <param name="txt">单位名称</param>
        /// <returns></returns>
        private string quanpin(string txt)
        {
            string qp = "";
            char[] Cr = txt.ToCharArray();
            //循环textBox1里的值
            for (int i = 0; i < txt.Length; i++)
            {
                if (ChineseChar.IsValidChar(Cr[i]))
                {
                    List<string> lpy = new List<string>();
                    ChineseChar CC = new ChineseChar(Cr[i]);
                    foreach (string item in CC.Pinyins)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (!lpy.Contains(item.Substring(0, item.Length - 1)))
                                lpy.Add(item.Substring(0, item.Length - 1));
                        }
                    }
                    qp += lpy[0].Substring(0, 1).ToLower();
                }
            }
            return qp;
        }

        /// <summary>
        /// 自动获取地区
        /// </summary>
        /// <param name="Name">地区名称</param>
        /// <returns></returns>
        private DiQu getDQ(string Name)
        {
            string where = " and Pid<>ID";
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "ID", order = "asc" };
            BasicCityBLL bll = new BasicCityBLL();
            IList<View_BasicCity> list = bll.SelectAll(where, pager);

            string Province = "";
            string UpID = "";
            string Region = "";
            string Finance = "";

            foreach (var item in list)
            {
                if (Name.Contains(item.Name))
                {
                    Province = item.Sfid;
                    UpID = item.Pid;
                    Region = item.ID;
                    string czjname = item.Name.Substring(item.Name.Length - 1);
                    if ("省" == czjname)
                    {
                        Finance = "000040";
                    }
                    else if ("市" == czjname)
                    {
                        Finance = "000060";
                    }
                    else if ("县" == czjname)
                    {
                        Finance = "000061";
                    }
                    else if ("区" == czjname)
                    {
                        Finance = "000062";
                    }
                    break;
                }
            }

            if (Region == "")
            {
                where = " and Pid=ID";
                list = bll.SelectAll(where, pager);
                foreach (var item in list)
                {
                    if (Name.Contains(item.Name))
                    {
                        Province = item.Sfid;
                        UpID = item.Pid;
                        Region = item.ID;
                        string czjname = item.Name.Substring(item.Name.Length - 1);
                        if ("省" == czjname)
                        {
                            Finance = "000040";
                        }
                        else if ("市" == czjname)
                        {
                            Finance = "000060";
                        }
                        else if ("县" == czjname)
                        {
                            Finance = "000061";
                        }
                        else if ("区" == czjname)
                        {
                            Finance = "000062";
                        }
                        break;
                    }
                }
            }

            //string UP = SpellHelper.GetSpellCode(Name);
            string usname = quanpin(Name);

            if (string.IsNullOrEmpty(Province) || string.IsNullOrEmpty(UpID)
                || string.IsNullOrEmpty(Region) || string.IsNullOrEmpty(Finance))
            {
                return null;
            }

            DiQu result = new DiQu
            {
                Province = Province,//省
                UpID = UpID,//市
                Region = Region,//县
                Finance = Finance,//财政局
                UP = usname//用户密码简拼
            };
            return result;
        }

        /// <summary>
        /// 生成插入语句
        /// </summary>
        /// <param name="lis_orders">订单实体类</param>
        /// <param name="lis_customs">客户实体类</param>
        /// <param name="updateSql">SQL语句</param>
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
                        NkscBLL bll = new NkscBLL();
                        if (Convert.ToInt32(bll.GetNameStr("count(*)", " and CusTomerId='" + main.OrderMain.SaleCustomId + "'")) < 1)
                        {
                            Nksc nksc = new Nksc();
                            nksc.SaleOrderID = main.OrderMain.Id;//合同编号
                            nksc.CustomerID = main.OrderMain.SaleCustomId;//客户编号
                            nksc.dwqc = main.KhName;
                            nksc.NkscDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            nksc.version = MaxVersion;
                            tsqls.Add(nksc.ToString(), null);
                        }
                        //复制上一年的内控手册
                    }
                    else if (item.ProdectType.StartsWith("0203"))//内控报告
                    {
                        //创建内控报告主表
                        NkReport report = new NkReport();
                        report.OrderId = main.OrderMain.Id;
                        report.CustomId = main.OrderMain.SaleCustomId;
                        tsqls.Add(report.ToString(), null);

                        //创建内控报告进度表
                        NkReport_Progress progress = new NkReport_Progress();
                        progress.Id = new NkReportBLL().MaxId();
                        progress.Zid = report.Id;
                        progress.Tjrq = DateTime.Now.ToString("yyyy-MM-dd");
                        tsqls.Add(progress.ToString(), null);
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
                        //复制上一年的内控手册并修改成未提交状态
                    }
                }
            }

            return tsqls;
        }

        /// <summary>
        /// 用户关注微信公众号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Insert_YwyAndWeiXin()
        {
            YwyAndWeiXin model = HttpPostStream.ConvertT<YwyAndWeiXin>(Request.InputStream);

            try
            {
                WeiXinApiBLL bll = new WeiXinApiBLL();

                string tsql = "select Count(*) from OpenIdAndYwyId where OpenId='" + model.OpenId + "'";
                int count = bll.WxGetCount(tsql);
                if (count < 1)
                {
                    if (bll.Insert("insert into OpenIdAndYwyId(YwyId,OpenId) values('" + model.YwyId + "','" + model.OpenId + "')") > 0)
                    {
                        return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //bll.Insert("insert into OpenIdAndYwyId(YwyId,OpenId) values('" + count + "','" + tsql.Replace("'", "") + "')");
                        //bll.Insert("insert into OpenIdAndYwyId(YwyId,OpenId) values('" + count + "','" + "insert into OpenIdAndYwyId(YwyId,OpenId) values(" + model.YwyId + "," + model.OpenId + ")" + "')");
                        return Json(JsonHandler.CreateMessage(0, "失败"), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //tsql = "select Count(123) from OpenIdAndYwyId where OpenId='" + model.OpenId + "'";
                    //bll.Insert("insert into OpenIdAndYwyId(YwyId,OpenId) values('" + count + "','" + tsql.Replace("'", "") + "')");
                    return Json(JsonHandler.CreateMessage(1, "成功"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 是否是绑定的用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsBinding()
        {
            WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            WeiXinApiBLL wx = new WeiXinApiBLL();
            NkscBLL nbll = new NkscBLL();

            string YwyId = wx.GetStrName("YwyId", "WeiXinYwyId", " and OpenId='" + model.OpenId + "'");
            if (!string.IsNullOrEmpty(YwyId))
            {
                return Json(JsonHandler.CreateMessage(1, "", YwyId), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string CusId = wx.GetNameStr("CusId", " and OpenId='" + model.OpenId + "'").ToStringEx();
                if (!string.IsNullOrEmpty(CusId))
                {
                    string count = nbll.GetNameStr("COUNT(*)", " and CustomerID='" + CusId + "'");
                    return Json(JsonHandler.CreateMessage(1, CusId, count), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, ""), JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 老用户绑定（测试过）
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(WeiXinRegister model)
        {
            //WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            try
            {
                #region 判断
                if (string.IsNullOrEmpty(model.OpenId.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "已超时，请重新登录！"), JsonRequestBehavior.AllowGet);
                }
                if (model.OldCusCode.Length != 18)
                {
                    return Json(JsonHandler.CreateMessage(0, "改革前社会统一信用代码不正确，请输入18位社会统一信用代码！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.CusCode.Length != 18)
                {
                    return Json(JsonHandler.CreateMessage(0, "改革后社会统一信用代码不正确，请输入18位社会统一信用代码！"), JsonRequestBehavior.AllowGet);
                }
                #endregion

                SaleCustomerBLL bll = new SaleCustomerBLL();
                WeiXinApiBLL wx = new WeiXinApiBLL();

                //查询客户编号(按改革后单位名称查询)
                string CusId = bll.GetStrName("ID", " where Name='" + model.CusName + "' and Code='" + model.CusCode + "'");

                //判断绑定单位是否存在(改革后单位名称)
                if (string.IsNullOrEmpty(CusId.ToStringEx()))//不存在(改革后单位名称)
                {
                    //查询客户编号(按改革前单位名称查询)
                    string SaleCusId = bll.GetStrName("ID", " where Name='" + model.OldCusName + "' and Code='" + model.OldCusCode + "'");

                    //判断绑定单位是否存在(改革前单位名称)
                    if (string.IsNullOrEmpty(SaleCusId.ToStringEx()))//不存在(改革前单位名称)
                    {
                        return Json(JsonHandler.CreateMessage(0, "未找到要注册的客户信息"), JsonRequestBehavior.AllowGet);
                    }
                    else//存在(改革前单位名称)
                    {
                        SaleWeiXin swx = new SaleWeiXin();
                        string ID = wx.MaxId(DateTime.Now.ToString("yyyyMMdd"));
                        swx.ID = ID;//编号
                        swx.CusId = SaleCusId;//单位编号
                        swx.OpenId = model.OpenId;//微信OpenId
                        swx.IsCus = "0";//老客户

                        try
                        {
                            string Zid = "";
                            if (model.Invoice.ToStringEx() != "")
                            {
                                Zid += ",Invoice='" + model.Invoice + "'";
                            }
                            if (model.Lxr.ToStringEx() != "")
                            {
                                Zid += ",Lxr='" + model.Lxr + "'";
                            }
                            if (model.Phone.ToStringEx() != "")
                            {
                                Zid += ",Phone='" + model.Phone + "'";
                            }
                            if (model.Address.ToStringEx() != "")
                            {
                                Zid += ",Address='" + model.Address + "'";
                            }
                            if (model.Lxr.ToStringEx() != "" && model.Phone.ToStringEx() != "")
                            {
                                Zid += ",Remark=Remark+'  '+Lxr+'  '+Phone";
                            }
                            else if (model.Lxr.ToStringEx() != "" && model.Phone.ToStringEx() == "")
                            {
                                Zid += ",Remark=Remark+'  '+Lxr";
                            }
                            else if (model.Lxr.ToStringEx() == "" && model.Phone.ToStringEx() != "")
                            {
                                Zid += ",Remark=Remark+'  '+Phone";
                            }
                            int count = bll.Update("update SaleCustom set Name='" + model.CusName + "',Code='" + model.CusCode + "'" +
                                Zid + " where ID='" + SaleCusId + "'");
                            //绑定微信
                            wx.Insert(swx);
                            return Json(JsonHandler.CreateMessage(1, SaleCusId), JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
                        }
                        #region
                        ////查询此微信号是否绑定过单位（客户编号）
                        //string SaleId = wx.GetNameStr("Zid", " and OpenId='" + _openId + "'");

                        //if (!string.IsNullOrEmpty(SaleId))//此微信已绑定
                        //{
                        //    if (SaleId == SaleCusId)//此微信号绑定的单位与当前绑定的单位相同
                        //    {
                        //        return Json(JsonHandler.CreateMessage(0, "此微信已绑定过本单位，无需要再次绑定！"), JsonRequestBehavior.AllowGet);
                        //    }
                        //    else//此微信号绑定的单位与当前绑定的单位不相同
                        //    {
                        //        return Json(JsonHandler.CreateMessage(0, "此微信已绑定其他单位，不可绑定多个单位！"), JsonRequestBehavior.AllowGet);
                        //    }
                        //}
                        //else//此微信未绑定
                        //{
                        //    //查询此单位是否已绑定微信
                        //    string OpenID = wx.GetNameStr("OpenId", " and Zid='" + SaleCusId + "'");

                        //    if (!string.IsNullOrEmpty(OpenID))//此单位已绑定
                        //    {
                        //        if (OpenID == _openId)//此单位绑定的微信与当前绑定的微信相同
                        //        {
                        //            return Json(JsonHandler.CreateMessage(0, "此微信已绑定过本单位，无需要再次绑定！"), JsonRequestBehavior.AllowGet);
                        //        }
                        //        else//此单位绑定的微信与当前绑定的微信不相同
                        //        {
                        //            return Json(JsonHandler.CreateMessage(0, "此单位已绑定其他微信，不可绑定多个微信！"), JsonRequestBehavior.AllowGet);
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
                else//存在(改革后单位名称)
                {
                    SaleWeiXin swx = new SaleWeiXin();
                    string ID = wx.MaxId(DateTime.Now.ToString("yyyyMMdd"));
                    swx.ID = ID;//编号
                    swx.CusId = CusId;//单位编号
                    swx.OpenId = model.OpenId;//微信OpenId
                    swx.IsCus = "0";//老客户

                    try
                    {
                        string tsql = "update SaleCustom set {0} where ID='" + CusId + "'";
                        string Zid = "";
                        if (model.Invoice.ToStringEx() != "")
                        {
                            Zid += ",Invoice='" + model.Invoice + "'";
                        }
                        if (model.Lxr.ToStringEx() != "")
                        {
                            Zid += ",Lxr='" + model.Lxr + "'";
                        }
                        if (model.Phone.ToStringEx() != "")
                        {
                            Zid += ",Phone='" + model.Phone + "'";
                        }
                        if (model.Address.ToStringEx() != "")
                        {
                            Zid += ",Address='" + model.Address + "'";
                        }
                        if (model.Lxr.ToStringEx() != "" && model.Phone.ToStringEx() != "")
                        {
                            Zid += ",Remark=Remark+'  '+Lxr+'  '+Phone";
                        }
                        else if (model.Lxr.ToStringEx() != "" && model.Phone.ToStringEx() == "")
                        {
                            Zid += ",Remark=Remark+'  '+Lxr";
                        }
                        else if (model.Lxr.ToStringEx() == "" && model.Phone.ToStringEx() != "")
                        {
                            Zid += ",Remark=Remark+'  '+Phone";
                        }

                        if (!string.IsNullOrEmpty(Zid))
                        {
                            int count = bll.Update(string.Format(tsql, Zid.Substring(1)));
                        }
                        //绑定微信
                        wx.Insert(swx);
                        return Json(JsonHandler.CreateMessage(1, CusId), JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 新用户注册（测试过）
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>string openId, string name, string Invoice, string code, string address, string lxr, string phone, string YwyId
        [HttpPost]
        public JsonResult RegisterNew(WeiXinRegister model)
        {
            //WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            try
            {
                SaleCustomerBLL bll = new SaleCustomerBLL();
                SysUserBLL sys = new SysUserBLL();
                WeiXinApiBLL wxa = new WeiXinApiBLL();

                SaleCustom scmodel = new SaleCustom();
                SaleWeiXin swxmodel = new SaleWeiXin();

                #region 判断验证
                if (string.IsNullOrEmpty(model.OpenId.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "已超时，请重新登录！"), JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.CusName.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "开票单位全称不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.CusName.Length > 50)
                {
                    return Json(JsonHandler.CreateMessage(0, "开票单位全称长度不允许大于50个字！"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int count = bll.GetCount(" where Name='" + model.CusName + "'");
                    if (count > 0)
                    {
                        return Json(JsonHandler.CreateMessage(0, "开票单位全称已存在！"), JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(model.CusCode.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "社会统一信用代码不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.CusCode.Length != 18)
                {
                    return Json(JsonHandler.CreateMessage(0, "社会统一信用代码不正确，请输入18位社会统一信用代码！"), JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Lxr.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "联系人不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.Lxr.Length > 30)
                {
                    return Json(JsonHandler.CreateMessage(0, "联系人长度不允许大于30个字！"), JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Phone.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "联系方式不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.Phone.Length > 20)
                {
                    return Json(JsonHandler.CreateMessage(0, "联系方式长度不允许大于20个字！"), JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(model.Address.ToStringEx()))
                {
                    return Json(JsonHandler.CreateMessage(0, "收件地址不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else if (model.Address.Length > 100)
                {
                    return Json(JsonHandler.CreateMessage(0, "收件地址长度不允许大于100个字！"), JsonRequestBehavior.AllowGet);
                }

                DiQu dq = getDQ(model.CusName);
                if (dq == null)
                {
                    return Json(JsonHandler.CreateMessage(0, "无法识别该开票单位全称！"), JsonRequestBehavior.AllowGet);
                }
                #endregion

                //添加客户
                //微信ID、单位名称Name、发票抬头Invoice、社会统一信用代码Code、单位地址Address、联系人Lxr、电话Phone
                scmodel.ID = bll.MaxId(DateTime.Now.ToString("yyyyMMdd"));//编号
                scmodel.Name = model.CusName;//单位名称
                scmodel.Invoice = model.Invoice;//发票抬头
                scmodel.Code = model.CusCode;//社会统一信用代码
                scmodel.Address = model.Address;//单位地址
                scmodel.Lxr = model.Lxr;//联系人
                scmodel.Phone = model.Phone;//电话
                scmodel.CDate = DateTime.Now.ToString("yyyy-MM-dd");//日期
                scmodel.Ywy = wxa.WxGetStrName("YwyId", " and OpenId='" + model.OpenId + "'");//业务员
                scmodel.YwyName = sys.GetNameStr("ZsName", " and Id='" + scmodel.Ywy + "'");//业务员姓名

                //编号ID、日期CDate、业务员Ywy、上级主管区域UpID、省份Province、所在市县区Region、财政局Finance、业务员姓名YwyName                
                scmodel.UpID = dq.UpID;//上级主管区域
                scmodel.Province = dq.Province;//省份
                scmodel.Region = dq.Region;//所在市县区
                scmodel.Finance = dq.Finance;//财政局
                scmodel.UserName = dq.UP;//用户密码简拼
                scmodel.UserPwd = dq.UP;//用户密码简拼

                //添加微信绑定
                swxmodel.ID = wxa.MaxId(DateTime.Now.ToString("yyyyMMdd"));//编号
                swxmodel.CusId = scmodel.ID;//客户编号
                swxmodel.OpenId = model.OpenId;//微信号
                swxmodel.IsCus = "1";//新客户

                try
                {
                    bll.Inserts(scmodel);
                    wxa.Insert(swxmodel);
                    return Json(JsonHandler.CreateMessage(1, scmodel.ID), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改客户基本信息（测试过）
        /// </summary>
        /// <param name="CusId">客户编号</param>
        /// <param name="zdname">字段名</param>
        /// <param name="zdtext">字段值</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update_CusXinXi(string CusId, string zdname, string zdtext)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            try
            {
                SaleCustomerBLL bll = new SaleCustomerBLL();

                #region 判断验证
                string result = "";
                switch (zdname)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            result = "单位全称不允许为空！";
                        }
                        else if (zdtext.Length > 50)
                        {
                            result = "单位全称长度不可以大于50个字！";
                        }
                        else
                        {
                            if (bll.isExist(" and Name='" + zdtext + "' and ID<>'" + CusId + "'"))
                            {
                                result = "单位全称已存在！";
                            }
                        }
                        break;
                    case "Invoice":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            result = "发票抬头不允许为空！";
                        }
                        else if (zdtext.Length > 100)
                        {
                            result = "发票抬头长度不可以大于100个字！";
                        }
                        break;
                    case "Code":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            return Json(JsonHandler.CreateMessage(0, "社会统一信用代码不允许为空！"), JsonRequestBehavior.AllowGet);
                        }
                        if (zdtext.Length != 18)
                        {
                            return Json(JsonHandler.CreateMessage(0, "社会统一信用代码不正确，请输入18位社会统一信用代码！"), JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case "Lxr":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            return Json(JsonHandler.CreateMessage(0, "联系人不允许为空！"), JsonRequestBehavior.AllowGet);
                        }
                        if (zdtext.Length > 30)
                        {
                            return Json(JsonHandler.CreateMessage(0, "联系人长度不可以大于30个字！"), JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case "Phone":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            return Json(JsonHandler.CreateMessage(0, "联系方式不允许为空！"), JsonRequestBehavior.AllowGet);
                        }
                        if (zdtext.Length > 20)
                        {
                            return Json(JsonHandler.CreateMessage(0, "联系方式长度不可以大于20个字！"), JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case "Address":
                        if (string.IsNullOrEmpty(zdtext))
                        {
                            return Json(JsonHandler.CreateMessage(0, "收件地址不允许为空！"), JsonRequestBehavior.AllowGet);
                        }
                        if (zdtext.Length > 100)
                        {
                            return Json(JsonHandler.CreateMessage(0, "收件地址长度不可以大于100个字！"), JsonRequestBehavior.AllowGet);
                        }
                        break;
                    default:
                        result = "";
                        break;
                }

                if (!string.IsNullOrEmpty(result))
                {
                    return Json(JsonHandler.CreateMessage(0, result), JsonRequestBehavior.AllowGet);
                }
                #endregion

                if (bll.Update("update SaleCustom set " + zdname + "='" + zdtext + "' where ID='" + CusId + "'") > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "修改失败！"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 返回个人信息(发票抬头、社会统一信用代码、收件人姓名、收件人电话、邮寄地址、是否做过内控手册)（测试过）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_XinXi()
        {
            WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            string where = " and ID='" + model.CusId + "'";
            SaleCustomerBLL bll = new SaleCustomerBLL();
            List<WeiXinInformation> result = bll.WeiXinSelectAll(where);
            return Json(result.Count == 0 ? null : result[0]);
        }

        /// <summary>
        /// 返回客户信息单位网址,用户名,密码,温馨提示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_Web()
        {
            //
            WeiXin_GetWeb model = HttpPostStream.ConvertT<WeiXin_GetWeb>(Request.InputStream);

            SaleCustomerBLL bll = new SaleCustomerBLL();
            SaleOrderBLL obll = new SaleOrderBLL();
            SaleOrderItemBLL ibll = new SaleOrderItemBLL();

            if (string.IsNullOrEmpty(model.OrderId))
            {
                return Json(null);//返回异常内容
            }
            else
            {
                if (obll.GetCount(" and Id='" + model.OrderId + "'") < 1)
                {
                    return Json(null);//返回异常内容
                }
            }

            if (string.IsNullOrEmpty(model.OpenId))
            {
                return Json(null);//返回异常内容
            }
            else
            {
                WeiXinApiBLL wxbll = new WeiXinApiBLL();
                if (wxbll.GetCount(" and OpenId='" + model.OpenId + "'") < 1)
                {
                    return Json(null);//返回异常内容
                }
            }
            int count = ibll.GetCount(" and OrderId='" + model.OrderId + "' and ProdectType='0203'");

            List<WeiXinWeb> result = bll.WeiXinSelectWeb(model.OrderId);
            if (count > 0)
            {
                //返回第一种提示
                result[0].Remark = result[0].Remark.Split('#')[0];
            }
            else
            {
                //返回第二种提示
                result[0].Remark = result[0].Remark.Split('#')[1];
            }
            return Json(result.Count == 0 ? null : result[0]);
        }

        /// <summary>
        /// 微信个人中心查看网址、用户名、密码
        /// </summary>
        /// <param name="CusId">客户编号</param>
        /// <param name="OpenId">微信编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_UsPwd(string CusId, string OpenId)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            SaleCustomerBLL bll = new SaleCustomerBLL();
            WeiXinApiBLL wxbll = new WeiXinApiBLL();
            SaleOrderItemBLL oibll = new SaleOrderItemBLL();
            SaleOrderBLL obll = new SaleOrderBLL();

            #region 判断
            if (CusId.ToStringEx() == "")
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登录！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            else
            {
                if (bll.GetCount(" where ID='" + CusId + "'") < 1)
                {
                    return Json(JsonHandler.CreateMessage(0, "无法识别客户编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }
            }

            if (OpenId.ToStringEx() == "")
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登录！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            else
            {
                if (wxbll.GetCount(" and OpenId='" + OpenId + "'") < 1)
                {
                    return Json(JsonHandler.CreateMessage(0, "无法识别客户信息！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }
            }
            #endregion

            int count = oibll.GetCount(" and OrderId in (select Id from SaleOrder where SaleCustomId='" + CusId + "')"
                + " and (ProdectType='0201' or ProdectType='0203' or ProdectType='0204')");

            if (count < 1)
            {
                return Json(JsonHandler.CreateMessage(0, "请先报名后，再查看用户名和密码！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }

            List<WeiXinWeb> result = bll.WeiXinGeRenWeb(CusId);
            var json = new
            {
                type = result.Count,
                rows = result.Count == 0 ? null : result[0]
            };
            return Json(json);
        }

        /// <summary>
        /// 获取产品类别只返回  手册，更新，报告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetData_FinProductType()
        {
            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "Id", order = "asc" };
            string where = " and (Id='0201' or Id='0203' or Id='0204')";
            FinProductTypeBLL bll = new FinProductTypeBLL();
            List<WeiXinProductType> result = bll.WxSelectAll(where, pager);
            return Json(result);
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="OpenId">微信编号</param>
        /// <param name="CusId">客户编号</param>
        /// <param name="proid">产品编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create_Order(string OpenId, string CusId, string proid)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            try
            {
                WeiXinOrder model = new WeiXinOrder();
                WeiXinApiBLL wxapi = new WeiXinApiBLL();
                SaleCustomerBLL cusbll = new SaleCustomerBLL();
                SaleOrderBLL bll = new SaleOrderBLL();
                SaleOrderItemBLL ibll = new SaleOrderItemBLL();
                SysUserBLL ubll = new SysUserBLL();

                #region 判断
                if (string.IsNullOrEmpty(CusId))
                {
                    return Json(JsonHandler.CreateMessage(0, "客户编号不允许为空！"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!cusbll.isExist(" and ID='" + CusId + "'"))
                    {
                        return Json(JsonHandler.CreateMessage(0, "客户编号不存在！"), JsonRequestBehavior.AllowGet);
                    }
                    model.CusId = CusId;
                }
                if (string.IsNullOrEmpty(proid))
                {
                    return Json(JsonHandler.CreateMessage(0, "请选择产品！"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] result = proid.Substring(1).Split(',');
                    for (int i = 0; i < result.Length; i++)
                    {
                        if ("0201,0203,0204".IndexOf(result[i]) < 0)
                        {
                            return Json(JsonHandler.CreateMessage(0, "无法识别产品！" + result[i]), JsonRequestBehavior.AllowGet);
                        }

                        WeiXinOrderMX mxmodel = new WeiXinOrderMX();
                        mxmodel.TypeID = result[i];
                        mxmodel.Money = "0.00";
                        model.OrderMX.Add(mxmodel);
                    }
                }
                #endregion

                bool bl = true;
                string TiMess = "";
                string where = "";

                if (proid.Contains("0201") || proid.Contains("0204"))
                {
                    where += " or ProdectType='0201' or ProdectType='0204'";
                }
                if (proid.Contains("0203"))
                {
                    where += " or ProdectType='0203'";
                }

                where = " and (" + where.Substring(4) + ")";

                DataTable dt = bll.GetData("*", " and SaleCustomId='" + CusId + "' and Years=" + DateTime.Now.Year + where, "View_WeiXinIsOrder");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bl = false;
                    if (!string.IsNullOrEmpty(TiMess))
                    {
                        TiMess = "已购置内控手册和内控报告，不可重复下达订单！";
                    }
                    else
                    {
                        switch (dt.Rows[i]["ProdectType"].ToString())
                        {
                            case "0201"://手册
                            case "0204"://更新
                                TiMess = "已购置内控手册，不可重复下达订单";
                                break;
                            case "0203"://报告
                                TiMess = "已购置内控报告，不可重复下达订单";
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!bl)
                {
                    return Json(JsonHandler.CreateMessage(0, TiMess), JsonRequestBehavior.AllowGet);
                }

                string date = DateTime.Now.ToString("yyyyMMdd");
                string uid = GetUserId();

                List<SaleCustom> lis_customs = new List<SaleCustom>();

                List<S_Order> lis_orders = new List<S_Order>();
                S_Order odMain = new S_Order();

                //1、订单主表：
                //必录项：Id合同编号(自动生成)、OrderDate合同日期(自动填写)、SaleCustomId客户编号(必传的)、
                //Saler业务员(通过客户编号自动回填)、OrderType(自动填写-默认“新购”)、UserId操作员(默认系统管理员)            

                string YwyID = new WeiXinApiBLL().WxGetStrName("YwyId", " and OpenId='" + OpenId + "'");
                odMain.OrderMain.Id = bll.Maxid(date);//订单编号
                odMain.OrderMain.OrderDate = DateTime.Now.ToString("yyyy-MM-dd");
                odMain.OrderMain.SaleCustomId = model.CusId;//客户
                odMain.OrderMain.Saler = YwyID;//业务员
                odMain.OrderMain.Fp = "0";//是否立即开票
                odMain.OrderMain.AccountId = "";//账户
                odMain.OrderMain.UserId = "000001";//操作员
                odMain.OrderMain.Remake = "";
                odMain.OrderMain.Flag = "2";//合同状态：0=正常、1=待定、2=待销售确认、3=待客户确认
                lis_orders.Add(odMain);

                //2、订单明细表：
                //必录项：OrderId订单主表编号(根据编号自动回填)、ItemId编号(自动生成)、ProdectType产品分类(必传的)、
                //ProdectDesc产品摘要(自动填写)、ItemCount数量(自动填写)、ItemPrice单价(必传的)、
                //ItemMoney成交金额(自动计算=单价*数量)、TaxMoney税金(自动计算=成交金额*0.05M)、
                //PresentMoney礼品礼金(默认0.00)、OtherMoney其他金额(默认0.00)、
                //ProdectType产品分类(=0201/0204，Service、SerDateS、SerDateE三个字段填写值)、TcFlag提成状态(默认000106)、
                //TcDate提成日期(默认空)
                for (int i = 0; i < model.OrderMX.Count; i++)
                {
                    SaleOrderItem OmxModel = new SaleOrderItem();
                    OmxModel.OrderId = odMain.OrderMain.Id;
                    OmxModel.ItemId = odMain.OrderMain.Id + (i + 1).ToString("00");

                    OmxModel.ProdectType = model.OrderMX[i].TypeID;
                    if (OmxModel.ProdectType.StartsWith("0201"))
                    {
                        OmxModel.ProdectDesc = "手册";
                        OmxModel.ItemPrice = 0.00M;
                    }
                    else if (OmxModel.ProdectType.StartsWith("0203"))
                    {
                        OmxModel.ProdectDesc = "报告";
                        OmxModel.ItemPrice = 0.00M;
                    }
                    else if (OmxModel.ProdectType.StartsWith("0204"))
                    {
                        OmxModel.ProdectDesc = "更新";
                        OmxModel.ItemPrice = 0.00M;
                    }
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
                //生成订单
                if (bll.Tran(tsql))
                {
                    //订单生成成功后返回业务员的微信编号
                    string YwyOpenId = cusbll.GetStrName("OpenId", "WeiXinYwyId", " and YwyId='" + YwyID + "'");
                    return Json(JsonHandler.CreateMessage(1, YwyOpenId), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改订单状态    后半部分开发时再说（订单金额（金额空或负数只该状态）  (确认是否是新老客户)）
        /// </summary>
        /// <param name="OrderId">客户编号</param>
        /// <param name="Flag">状态</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update_OrderFlag(string OrderId, string Flag)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            try
            {
                #region 判断
                if (string.IsNullOrEmpty(OrderId))
                {
                    return Json(JsonHandler.CreateMessage(0, "无法识别订单编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }
                if (!",0,1,2,3".Contains(',' + Flag) || string.IsNullOrEmpty(Flag))
                {
                    return Json(JsonHandler.CreateMessage(0, "无法识别订单状态！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }
                #endregion

                SaleOrderBLL obll = new SaleOrderBLL();
                obll.Update("update SaleOrder set Flag=" + Flag + " where Id='" + OrderId + "'");
                return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);//返回订单编号
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);//返回异常内容
            }
        }

        /// <summary>
        /// 查询客户提交确认
        /// </summary>
        /// <param name="CusId">客户编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Cus_OrderXinXi(string CusId)
        {
            //WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (string.IsNullOrEmpty(CusId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别客户编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }

            SaleOrderBLL obll = new SaleOrderBLL();
            SaleOrderItemBLL ibll = new SaleOrderItemBLL();

            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "OrderId", order = "desc" };

            List<WeiXinQueRenXinX> result = obll.Cus_OrderXinXi(" and CusID='" + CusId + "'", pager);
            foreach (var item in result)
            {
                DataTable dt = ibll.GetData("TypeName", " and OrderId='" + item.OrderId + "'", "View_WeiXiOrderItem");
                foreach (DataRow dtr in dt.Rows)
                {
                    item.TypeName.Add(new WeiXinQueRenXinXMX(dtr["TypeName"]));
                }
            }

            var json = new
            {
                total = result.Count,
                rows = result
            };
            return Json(json);
        }

        /// <summary>
        /// 返回订单信息( 根据客户编号 ) 类别名称、年度、内控手册/内控报告状态（手册/报告进度）  返回电子合同样式???
        /// </summary>
        /// <param name="CusId">客户编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_CusFinishOrder(string CusId)
        {
            //WeiXinRegister model = HttpPostStream.ConvertT<WeiXinRegister>(Request.InputStream);

            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (string.IsNullOrEmpty(CusId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别客户编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }

            GridPager pager = new GridPager { page = 1, rows = 1000, sort = "OrderId", order = "asc" };

            SaleOrderBLL obll = new SaleOrderBLL();
            List<WeiXinCusFinishOrderXx> result = obll.WeiXinCusOrderXx(" and SaleCustomId='" + CusId + "'", pager);

            var json = new
            {
                total = result.Count,
                rows = result.Count == 0 ? null : result
            };
            return Json(json);
        }

        /// <summary>
        /// 返回业务员确认订单信息( 根据业务员编号 )  客户名称、微信编号、订单编号、年度，子表-订单明细表编号、产品类别名称、单价
        /// </summary>
        /// <param name="YwyId">业务员编号</param>
        /// <param name="pager">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_YwyOrder(string YwyId, GridPager pager)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            WeiXinApiBLL wxapi = new WeiXinApiBLL();
            SaleOrderItemBLL ibll = new SaleOrderItemBLL();

            #region 判断
            if (string.IsNullOrEmpty(YwyId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别业务员编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            if (pager == null)
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别页数参数！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            #endregion

            List<WeiXinYwyQueRen> result = wxapi.Select_YwyQueRen(" and YwyId='" + YwyId + "'", pager);
            foreach (var item in result)
            {
                DataTable dt = ibll.GetData("ItemId,TypeName,ItemPrice", " and OrderId='" + item.OrderId + "'", "View_WeiXiOrderItem");
                foreach (DataRow dtr in dt.Rows)
                {
                    WeiXinYwyQueRenMx mx = new WeiXinYwyQueRenMx();
                    mx.OrderMxId = dtr["ItemId"].ToStringEx();
                    mx.TypeName = dtr["TypeName"].ToStringEx();
                    mx.ItemPrice = dtr["ItemPrice"].ToStringEx();
                    item.OrderMx.Add(mx);
                }
            }

            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        /// <summary>
        /// 业务员确认订单信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="Flag">订单状态</param>
        /// <param name="OrderMxId">明细表编号</param>
        /// <param name="ItemPrice">明细表单价</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Ywy_QueRenOrder(string OrderId, string Flag, string OrderMxId, string ItemPrice)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            SaleOrderBLL obll = new SaleOrderBLL();
            SaleOrderItemBLL ibll = new SaleOrderItemBLL();
            WeiXinApiBLL wxapi = new WeiXinApiBLL();

            #region 判断、验证
            if (string.IsNullOrEmpty(OrderId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            if (string.IsNullOrEmpty(Flag))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单状态！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            if (string.IsNullOrEmpty(OrderMxId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单明细编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            if (string.IsNullOrEmpty(ItemPrice))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单金额！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            //修改明细表金额
            string[] MxId = OrderMxId.Split(',');
            string[] Money = ItemPrice.Split(',');

            if (MxId.Length != Money.Length)
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单明细参数！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            try
            {
                for (int i = 0; i < Money.Length; i++)
                {
                    decimal JinE = Convert.ToDecimal(Money[i]);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别订单金额！" + ex.Message), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            #endregion

            try
            {
                for (int i = 0; i < MxId.Length; i++)
                {
                    int ItemCount = 1;

                    //成交金额=单价*数量
                    decimal ItemMoney = ItemCount * Convert.ToDecimal(Money[i]);
                    //税金
                    decimal TaxMoney = ItemMoney * 0.05M;

                    ibll.Update("update SaleOrderItem set ItemPrice='" + Money[i] + "',ItemMoney='" + ItemMoney + "',TaxMoney='"
                        + TaxMoney + "' where ItemId='" + MxId[i] + "'");
                }

                //修改订单状态
                obll.Update("update SaleOrder set Flag='" + Flag + "' where Id='" + OrderId + "'");

                //查询订单的客户编号
                string NewCusId = obll.GetStrName("SaleCustomId", " and Id='" + OrderId + "'");

                //修改绑定微信客户的新老客户标识
                wxapi.Update("update SaleWeiXin set IsCus='0' where CusId='" + NewCusId + "'");

                return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);//成功
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);//返回异常内容
            }
        }

        /// <summary>
        /// 返回客户信息(根据业务员编号)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Ywy_ReturnCus(string YwyId, string CusName, GridPager pager)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            if (string.IsNullOrEmpty(YwyId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别业务员编号！"), JsonRequestBehavior.AllowGet);//返回异常内容
            }

            string where = " and YwyId='" + YwyId + "'";

            if (!string.IsNullOrEmpty(CusName))
            {
                where += " and CusName like '%" + CusName + "%'";
            }

            SaleCustomerBLL bll = new SaleCustomerBLL();
            List<WeiXinYwyReturnCus> result = bll.WeiXinYwyReturnCus(where, pager);

            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        /// <summary>
        /// 返回客户编号、客户名称
        /// </summary>
        /// <param name="YwyId">业务员编号</param>
        /// <param name="CusName">单位名称</param>
        /// <param name="pager">分页</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Return_SaleCustomer(string YwyId, string CusName, GridPager pager)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            string where = " and Ywy='" + YwyId + "'";

            if (!string.IsNullOrEmpty(CusName))
            {
                where += " and Name like '%" + CusName + "%'";
            }

            SaleCustomerBLL bll = new SaleCustomerBLL();
            List<WeiXinSaleCustom> result = bll.Return_SelectYwyAll(where, pager);

            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        /// <summary>
        /// 业务员确认新客户是老客户，则修改客户相关的表单（绑定微信表的客户ID、订单主表的客户ID、内控手册表客户ID(如果老客户ID存在手册，则删除)、
        /// 内控报告表客户ID、客户表(客户名称、发票抬头、社会统一信用代码、联系人、联系方式、地址)）
        /// </summary>
        /// <param name="CusId">老客户ID</param>
        /// <param name="OpenId">绑定微信ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update_IsCustom(string CusId, string OpenId)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            #region 判断
            if (string.IsNullOrEmpty(CusId))
            {
                return Json(JsonHandler.CreateMessage(0, "无法识别客户编号！"), JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(OpenId))
            {
                return Json(JsonHandler.CreateMessage(0, "已超时，请重新登录！"), JsonRequestBehavior.AllowGet);
            }
            #endregion

            //老客户Id不为空时，修改订单主表的客户编号字段、修改微信绑定表的客户编号、根据老客户编号修改联系人和电话
            SaleCustomerBLL cusbll = new SaleCustomerBLL();
            SaleOrderBLL obll = new SaleOrderBLL();
            WeiXinApiBLL wxbll = new WeiXinApiBLL();
            NkscBLL nksc = new NkscBLL();

            try
            {
                #region 判断
                //新绑定的客户编号
                string NewCusId = wxbll.GetNameStr("CusId", " and OpenId='" + OpenId + "'");

                if (string.IsNullOrEmpty(NewCusId))
                {
                    return Json(JsonHandler.CreateMessage(0, "未找到绑定的客户！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }

                //新生成订单编号
                string NewOrderId = obll.GetStrName("Id", " and SaleCustomId='" + NewCusId + "'");

                if (string.IsNullOrEmpty(NewOrderId))
                {
                    return Json(JsonHandler.CreateMessage(0, "未找到绑定的订单！"), JsonRequestBehavior.AllowGet);//返回异常内容
                }
                #endregion

                //1、修改微信绑定表的客户编号、新老客户标识(根据微信编号、新客户编号)
                wxbll.Update("update SaleWeiXin set CusId='" + CusId + "',IsCus='1' where OpenId='" + OpenId + "' and CusId='" + NewCusId + "'");

                //2、修改订单主表的客户编号字段
                obll.Update("update SaleOrder set SaleCustomId='" + CusId + "' where Id in (select Id from SaleOrder where SaleCustomId='" + NewCusId + "')");

                //3、修改内控手册
                if (nksc.isExist(" and CustomerID='" + CusId + "'"))
                {
                    nksc.Delete("delete from Nksc where CustomerID='" + NewCusId + "'");
                }
                else
                {
                    nksc.Update("update Nksc set CustomerID='" + CusId + "' where CustomerID='" + NewCusId + "'");
                }

                //4、修改内控报告
                nksc.Update("update NkReport set CustomId='" + CusId + "' where CustomId='" + NewCusId + "'");

                //5、根据老客户编号修改联系人和电话
                string Lxr = cusbll.GetStrName("Lxr", " where ID='" + NewCusId + "'");
                string Phone = cusbll.GetStrName("Phone", " where ID='" + NewCusId + "'");
                string Invoice = cusbll.GetStrName("Invoice", " where ID='" + NewCusId + "'");
                string Address = cusbll.GetStrName("Address", " where ID='" + NewCusId + "'");
                string CusCode = cusbll.GetStrName("Code", " where ID='" + NewCusId + "'");
                string CusName = cusbll.GetStrName("Name", " where ID='" + NewCusId + "'");
                cusbll.Update("update SaleCustom set Name='" + CusName + "',Invoice='" + Invoice + "',Address='" + Address + "',Code='"
                    + CusCode + "',Lxr='" + Lxr + "',Phone='" + Phone + "',Remark=Remark+Lxr+Phone where ID='" + CusId + "'");
                cusbll.Delete(NewCusId);

                return Json(JsonHandler.CreateMessage(1, CusName), JsonRequestBehavior.AllowGet);//返回异常内容
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, ex.Message), JsonRequestBehavior.AllowGet);//返回异常内容
            }
        }

        /**************************************************************************************/

        //返回合同状态相关信息( 根据业务员编号 )  客户名称,合同状态,商品信息,邮寄 状态,开票状态

        /// <summary>
        /// 客户上传 汇款单或入账通知书(SaleOrder表附件  Enclosure)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload_Remittance()
        {
            WeiXinRemittance model = HttpPostStream.ConvertT<WeiXinRemittance>(Request.InputStream);

            try
            {
                //判断订单编号的文件夹是否存在，不存在则创建
                if (!Directory.Exists(Server.MapPath("~/WeiXinImage/" + model.OrderId)))
                {
                    //创建文件夹
                    Directory.CreateDirectory("~/WeiXinImage/" + model.OrderId);
                    //复制文件到自己的订单编号的文件夹里
                    System.IO.File.Copy("E:\\webroot\\" + model.ImageName, Server.MapPath("~/WeiXinImage/" + model.OrderId + "/" + model.ImageName));
                }
                else
                {
                    System.IO.File.Copy("E:\\webroot\\" + model.ImageName, Server.MapPath("~/WeiXinImage/" + model.OrderId + "/" + model.ImageName));
                }

                SaleOrderBLL obll = new SaleOrderBLL();
                int count = obll.Update("update SaleOrder set Enclosure='" + model.ImageName + "' where Id='" + model.OrderId + "'");
                if (count > 0)
                {
                    return Json(JsonHandler.CreateMessage(1, "上传成功！"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "上传失败！"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonHandler.CreateMessage(0, "上传失败" + ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        //快递信息（先不做）
    }
}
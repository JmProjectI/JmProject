using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Web.AttributeEX;
using System.Data;
using JMProject.Common;
using JMProject.BLL;
using JMProject.Model;
using System.IO;
using JMProject.Model.Esayui;
using JMProject.Model.Sys;
using System.Text;

namespace JMProject.Web.Controllers
{
    public class WordController : BaseController
    {
        public ActionResult UpImg()
        {
            //word模版所在路径
            string wdSavePath1 = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\");
            DirectoryInfo dir = new DirectoryInfo(wdSavePath1);
            FileInfo[] fis = dir.GetFiles("*.docx");

            NkscBLL bll = new NkscBLL();
            string ksbm = "科室";
            foreach (FileInfo item in fis)
            {
                if (DateTime.Compare(item.CreationTime, Convert.ToDateTime("2019-05-21")) > 0)
                {
                    string Fname = item.Name.Substring(0, item.Name.Length - 13);

                    DataTable dt_nksc = bll.SelectNkscDataName(Fname);
                    string id = dt_nksc.Rows[0]["id"].ToString();
                    string uid = bll.GetNameStr("CustomerID", " and id='" + id + "'");

                    string where = "";
                    List<WordTempFile> list_file = bll.SelectTempFile(" and ID='0402'");
                    List<WordTempKey> list_key = bll.SelectTempKey(where);
                    List<WordTempXZ> list_xz = bll.SelectTempXZ(where);
                    List<WordTempLCT> list_lct = bll.SelectTempLCT(where);
                    List<Nksc_fz> list_nkfz = bll.SelectNkscfz(Guid.Parse(id));
                    DataTable dtDep = bll.SelectAddress(uid);

                    //word模版所在路径
                    string wdPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"WordTempFileS\");
                    //word模版所在路径
                    string wdSavePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTempXZ\");
                    //流程图模版所在路径
                    string imgPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"WordTempFileS\ImgLCT\");

                    //最终生成图片所在路径
                    string imgFileDir = Path.Combine(HttpRuntime.AppDomainAppPath, @"UserImg\" + uid + @"\");

                    JMProject.Word.WdHelper wdhelper = new JMProject.Word.WdHelper(wdPath, wdSavePath, imgPath, imgFileDir);
                    wdhelper.SetGDTempKey(SetGDKey, dtDep);//设置固定替换
                    wdhelper.SetDeleteBookMarkt(GetDeleteBookMarkt, dt_nksc);//包含业务 （决定是否删除书签）
                    string fileName = wdhelper.CreateWord(ksbm, dt_nksc, list_file, list_key, list_xz, list_lct, list_nkfz, Get_ZH_Value);

                }
            }
            return Content("成功");
        }

        #region 生成word文档

        [AuthorizeAttributeEx]
        [HttpPost]
        public ActionResult BuildWord(string id, string stype, bool B5, bool x4)
        {
            try
            {
                string ksbm = stype == "1" ? "部门" : "科室";

                NkscBLL bll = new NkscBLL();
                string uid = bll.GetNameStr("CustomerID", " and id='" + id + "'");
                DataTable dt_nksc = bll.SelectNkscData(id);
                string where = "";
                List<WordTempFile> list_file = bll.SelectTempFile(where);
                List<WordTempKey> list_key = bll.SelectTempKey(where);
                List<WordTempXZ> list_xz = bll.SelectTempXZ(where);
                List<WordTempLCT> list_lct = bll.SelectTempLCT(where);
                List<Nksc_fz> list_nkfz = bll.SelectNkscfz(Guid.Parse(id));
                DataTable dtDep = bll.SelectAddress(uid);

                //word模版所在路径
                string wdPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"WordTempFileS\");
                //word模版所在路径
                string wdSavePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"wordTemp\");
                //流程图模版所在路径
                string imgPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"WordTempFileS\ImgLCT\");

                //最终生成图片所在路径
                string imgFileDir = Path.Combine(HttpRuntime.AppDomainAppPath, @"UserImg\" + uid + @"\");

                JMProject.Word.WdHelper wdhelper = new JMProject.Word.WdHelper(wdPath, wdSavePath, imgPath, imgFileDir);
                wdhelper.SetGDTempKey(SetGDKey, dtDep);//设置固定替换
                wdhelper.SetDeleteBookMarkt(GetDeleteBookMarkt, dt_nksc);//包含业务 （决定是否删除书签）
                string fileName = wdhelper.CreateWord(ksbm, dt_nksc, list_file, list_key, list_xz, list_lct, list_nkfz, Get_ZH_Value);

                if (!fileName.StartsWith("Error"))
                {
                    bll.UpdateFileName(fileName, id);
                    return Json(JsonHandler.CreateMessage(1, "生成成功"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "生成失败：（" + fileName + "）"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ee)
            {
                return Json(JsonHandler.CreateMessage(0, ee.Message), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取所有包含业务 与 前台挂钩
        /// </summary>
        /// <param name="dtnk"></param>
        /// <returns></returns>
        public List<string> GetDeleteBookMarkt(DataTable dtnk)
        {
            List<string> list_del = new List<string>();

            string dzzjgmc = dtnk.Rows[0]["dzzjgmc"].ToStringEx();
            //string dzzjgmc = "无";
            if (!string.IsNullOrEmpty(dzzjgmc))
            {
                list_del.Add(dzzjgmc);//党组织机构名称
            }

            string bhyw = dtnk.Rows[0]["bhyw"].ToStringEx();
            string EngineRoom = dtnk.Rows[0]["EngineRoom"].ToStringEx();
            string zxzjgl = dtnk.Rows[0]["zxzjgl"].ToStringEx();
            string Radioclf = dtnk.Rows[0]["Radioclf"].ToStringEx();
            string Radiohyf = dtnk.Rows[0]["Radiohyf"].ToStringEx();
            string Radiopxf = dtnk.Rows[0]["Radiopxf"].ToStringEx();
            string Radiogwzdf = dtnk.Rows[0]["Radiogwzdf"].ToStringEx();
            string Radiobzz = dtnk.Rows[0]["Radiobzz"].ToStringEx();
            string Radio_cghtsq = dtnk.Rows[0]["Radio_cghtsq"].ToStringEx();
            string Radio_zxcgsq = dtnk.Rows[0]["Radio_zxcgsq"].ToStringEx();
            string Radio_jksh = dtnk.Rows[0]["Radio_jksh"].ToStringEx();

            //string bhyw = "f";
            //string EngineRoom = "0";
            //string zxzjgl = "0";
            //string Radioclf = "0";
            //string Radiohyf = "0";
            //string Radiopxf = "0";
            //string Radiogwzdf = "0";
            //string Radiobzz = "0";

            //string Radio_cghtsq = "2";
            //string Radio_zxcgsq = "2";
            //string Radio_jksh = "2";

            if (bhyw.IndexOf('8') >= 0)
            {
                list_del.Add("合同管理业务");
            }
            if (bhyw.IndexOf('7') >= 0)
            {
                list_del.Add("建设项目业务");
            }
            if (bhyw.IndexOf('5') >= 0)
            {
                list_del.Add("政府采购业务");
            }
            if (bhyw.IndexOf('a') >= 0)
            {
                list_del.Add("非税收入");
            }
            if (bhyw.IndexOf('b') >= 0)
            {
                list_del.Add("债务业务");
            }
            //if (bhyw.IndexOf('c') >= 0)
            //{
            //    list_del.Add("对外投资业务");
            //}
            if (bhyw.IndexOf('1') >= 0)
            {
                list_del.Add("预算业务");
            }
            if (bhyw.IndexOf('d') >= 0)
            {
                list_del.Add("收入登记入账");
            }
            if (bhyw.IndexOf('e') >= 0 || bhyw.IndexOf('4') >= 0)
            {
                list_del.Add("票据管理");
            }
            if (bhyw.IndexOf('g') >= 0)
            {
                list_del.Add("财政授权支付");
            }
            if (bhyw.IndexOf('f') >= 0)
            {
                list_del.Add("财政直接支付");
            }
            if (bhyw.IndexOf('i') >= 0)
            {
                list_del.Add("政府购买服务流程");
            }
            if (bhyw.IndexOf('6') >= 0)
            {
                list_del.Add("资产业务控制");
            }
            if (bhyw.IndexOf('k') >= 0)
            {
                list_del.Add("国有资产出租出借业务");
            }
            if (bhyw.IndexOf('m') >= 0)
            {
                list_del.Add("国有资产收入上缴流程");
            }
            if (bhyw.IndexOf('n') >= 0)
            {
                list_del.Add("国有资产产权登记、变更、注销流程");
            }
            if (bhyw.IndexOf('o') >= 0)
            {
                list_del.Add("国有资产产权纠纷调处流程");
            }
            if (bhyw.IndexOf('p') >= 0)
            {
                list_del.Add("资产评估流程");
            }
            if (bhyw.IndexOf('q') >= 0)
            {
                list_del.Add("建设项目公开招标流程、邀请招标流程");
            }
            if (bhyw.IndexOf('r') >= 0)
            {
                list_del.Add("建设项目设计变更、洽商签证流程");
            }
            if (bhyw.IndexOf('h') >= 0)
            {
                list_del.Add("公务卡");
            }

            if (!string.IsNullOrEmpty(EngineRoom) && EngineRoom != "0")
            {
                list_del.Add("机房");
            }
            if (!string.IsNullOrEmpty(zxzjgl) && zxzjgl != "0")
            {
                list_del.Add("财政专项资金管理办法");
            }
            if (Radioclf != "0")
            {
                list_del.Add("差旅费");
            }
            if (Radiohyf != "0")
            {
                list_del.Add("会议费");
            }
            if (Radiopxf != "0")
            {
                list_del.Add("培训费");
            }
            if (Radiogwzdf != "0")
            {
                list_del.Add("招待费");
            }
            if (Radiobzz == "1")
            {
                list_del.Add("报账制");
            }

            if (!string.IsNullOrEmpty(Radio_cghtsq) && Radio_cghtsq != "2")
            {
                list_del.Add("采购合同审批");
            }
            if (!string.IsNullOrEmpty(Radio_zxcgsq) && Radio_zxcgsq != "2")
            {
                list_del.Add("单位自行采购审批");
            }
            if (!string.IsNullOrEmpty(Radio_jksh) && Radio_jksh != "2")
            {
                list_del.Add("借款业务");
            }

            return list_del;
        }

        /// <summary>
        /// 扫描整个文档  替换
        /// </summary>
        /// <param name="dtDep"></param>
        /// <returns></returns>
        public Dictionary<string, string> SetGDKey(DataTable dtDep)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (dtDep != null && dtDep.Rows.Count > 0)
            {
                //地区名称
                string dqmc = dtDep.Rows[0]["dqmcName"].ToStringEx();
                if (!string.IsNullOrEmpty(dqmc))
                {
                    result.Add("九台区", dqmc);
                }

                //上级主管区域
                string sjzgqy = dtDep.Rows[0]["sjzgqyName"].ToStringEx();
                if (!string.IsNullOrEmpty(sjzgqy))
                {
                    result.Add("长春地区", sjzgqy.Replace("室", "").Replace("州", "") + "地区");
                    result.Add("长春市市直", sjzgqy + "市直");
                }

                //财政局
                string czf = dtDep.Rows[0]["xzqyName"].ToStringEx();
                if (!string.IsNullOrEmpty(czf))
                {
                    result.Add("区财政", czf + "财政");
                    result.Add("区级", czf + "级");
                    result.Add("区直", czf + "直");
                    result.Add("区政府", czf + "政府");
                    result.Add("区委", czf + "委");
                    //result.Add("区委组织部", czf + "委组织部");
                    result.Add("区人社局", czf + "人社局");
                    result.Add("区审计局", czf + "审计局");
                    result.Add("区外专局", czf + "外专局");
                    result.Add("区纪律检查委员会", czf + "纪律检查委员会");
                    result.Add("区纪检委", czf + "纪检委");
                    result.Add("区纪委", czf + "纪委");
                    if (czf == "县")
                    {
                        result.Add("区中级人民法院", czf + "人民法院");
                    }
                    else
                    {
                        result.Add("区中级人民法院", czf + "中级人民法院");
                    }

                    result.Add("区人民检察院", czf + "人民检察院");
                    result.Add("区主要负责", czf + "主要负责");
                    result.Add("区人大", czf + "人大");
                    //result.Add("区政协办公厅", czf + "政协办公厅");
                    result.Add("区工商联", czf + "工商联");

                    result.Add("区有关人员", czf + "有关人员");
                    result.Add("区有关单位", czf + "有关单位");
                    result.Add("区政协", czf + "政协");
                    result.Add("区会议代表", czf + "会议代表");
                    result.Add("区应急", czf + "应急");
                    result.Add("区交换站", czf + "交换站");

                    result.Add("我区", "我" + czf);
                    result.Add("本区", "本" + czf);
                    result.Add("全区", "全" + czf);
                }
            }
            return result;
        }

        public void Get_ZH_Value(DataTable dt, string WordKey, string DBKey, Dictionary<string, string> Keyword)
        {
            string id = dt.Rows[0]["id"].ToString();
            if (WordKey == "key_DZZJGMC")
            {
                string dzzjgmc = dt.Rows[0][DBKey].ToStringEx() == "无" ? "" : dt.Rows[0][DBKey].ToStringEx();
                Keyword.Add(WordKey, dzzjgmc);
                return;
            }
            else if (WordKey == "key_GJZ1")
            {
                string str_gjz1 = " ";

                if (dt.Rows[0]["dzzjgmc"].ToStringEx() == "无")
                {
                    str_gjz1 = dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。&p";
                    str_gjz1 += dt.Rows[0]["fzzwmc1"].ToStringEx() + dt.Rows[0]["ldfzmc1"].ToStringEx() + "：分管" + dt.Rows[0]["ldfzfg1"].ToStringEx() + "工作。";
                }
                else
                {
                    if (dt.Rows[0]["zzzwDY"].ToStringEx() != "1")
                    {
                        str_gjz1 = dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。&p";
                    }
                    else
                    {
                        str_gjz1 = dt.Rows[0]["dzzjgmc"].ToStringEx() + "成员、" + dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。&p";
                    }
                    if (dt.Rows[0]["fzzwDY"].ToStringEx() != "1")
                    {
                        str_gjz1 += dt.Rows[0]["fzzwmc1"].ToStringEx() + dt.Rows[0]["ldfzmc1"].ToStringEx() + "：分管" + dt.Rows[0]["ldfzfg1"].ToStringEx() + "工作。";
                    }
                    else
                    {
                        str_gjz1 += dt.Rows[0]["dzzjgmc"].ToStringEx() + "成员、" + dt.Rows[0]["fzzwmc1"].ToStringEx() + dt.Rows[0]["ldfzmc1"].ToStringEx() + "：分管" + dt.Rows[0]["ldfzfg1"].ToStringEx() + "工作。";
                    }
                }
                Keyword.Add(WordKey, str_gjz1);
                return;
            }
            else if (WordKey == "key_GJZ2")
            {
                string str_gjz2 = "，";
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz2 = "，副组长由" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "担任，";
                }
                Keyword.Add(WordKey, str_gjz2);
                return;
            }
            else if (WordKey == "key_GJZ3")
            {
                string str_gjz3 = dt.Rows[0]["zzzwmc"].ToStringEx();
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz3 = dt.Rows[0]["zzzwmc"].ToStringEx() + "或" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "";
                }
                Keyword.Add(WordKey, str_gjz3);
                return;
            }
            else if (WordKey == "key_GJZ4")
            {
                string str_gjz4 = dt.Rows[0]["zzzwmc"].ToStringEx();
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz4 = dt.Rows[0]["zzzwmc"].ToStringEx() + "、" + dt.Rows[0]["fzzwmc1"].ToStringEx();
                }
                Keyword.Add(WordKey, str_gjz4);
                return;
            }
            else if (WordKey == "key_GJZ5")
            {
                string str_gjz5 = "";
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz5 = dt.Rows[0]["fzzwmc1"].ToStringEx();
                }
                else
                {
                    str_gjz5 = dt.Rows[0]["zzzwmc"].ToStringEx();
                }
                Keyword.Add(WordKey, str_gjz5);
                return;
            }
            else if (WordKey == "key_GJZ6")
            {
                string str_gjz6 = "需报" + dt.Rows[0]["zzzwmc"].ToStringEx() + "审批。";
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz6 = "一般事宜由" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "审批，重要需报" + dt.Rows[0]["zzzwmc"].ToStringEx() + "审批。";
                }
                Keyword.Add(WordKey, str_gjz6);
                return;
            }
            else if (WordKey == "key_GJZ7")
            {
                string str_gjz7 = "负责人";
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                {
                    str_gjz7 = "负责人、" + dt.Rows[0]["fzzwmc1"].ToStringEx();
                }
                Keyword.Add(WordKey, str_gjz7);
                return;
            }
            else if (WordKey == "key_dzcylist")
            {
                NkscBLL bll = new NkscBLL();
                List<Nksc_fz> list_fz = bll.SelectNkscfz(Guid.Parse(id));
                string dzcylist = "";
                foreach (Nksc_fz drow in list_fz)
                {
                    string dzcy = "";
                    if (dt.Rows[0]["dzzjgmc"].ToStringEx() == "无")
                    {
                        dzcy = string.Format("{0}{1}：分管{2}工作。&p", drow.fzzwmc, drow.ldfzmc, drow.ldfzfg);
                    }
                    else
                    {
                        if (drow.fzzwDY != "1")
                        {
                            dzcy = string.Format("{0}{1}：分管{2}工作。&p", drow.fzzwmc, drow.ldfzmc, drow.ldfzfg);
                        }
                        else
                        {
                            dzcy = string.Format(dt.Rows[0]["dzzjgmc"].ToStringEx() + "成员、{0}{1}：分管{2}工作。&p", drow.fzzwmc, drow.ldfzmc, drow.ldfzfg);
                        }
                    }
                    dzcylist += dzcy;
                }
                Keyword.Add(WordKey, dzcylist);
                return;
            }
            else if (WordKey == "key_gwznlist")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlks = "select * from Nksc_kszn where zid='" + id + "' order by sort";
                DataTable dtks = bll.Select(tsqlks);
                string allks = "";
                foreach (DataRow itemks in dtks.Rows)
                {
                    string ks = "";
                    ks += itemks["ksmc"].ToStringEx() + "&p";
                    ks += itemks["kszn"].ToStringEx().Replace("\n", "&p") + "&p";

                    DataTable gwzrdt = bll.Select("select * from Nksc_ksgwzr where id='" + itemks["id"] + "' order by sort");
                    foreach (DataRow drow in gwzrdt.Rows)
                    {
                        string gw = "";
                        gw += drow["ksgw"].ToStringEx() + "&p";
                        gw += drow["ksgwzr"].ToStringEx().Replace("\n", "&p") + "&p";
                        ks += gw;
                    }
                    allks += ks;
                }
                if (string.IsNullOrEmpty(allks))
                {
                    allks = " ";
                }
                Keyword.Add(WordKey, allks);
                return;
            }
            else if (WordKey == "key_zjzfsp0419" || WordKey == "key_zjzfspqx0419")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlje = "select * from Nksc_Zcyw where id='" + id + "' order by zcywtype,sort";
                DataTable dtje = bll.Select(tsqlje);
                string allje = "";
                for (int i = 0; i < dtje.Rows.Count; i++)
                {
                    DataRow itemje = dtje.Rows[i];
                    if (itemje["zcmoney"].ToStringEx() == "")
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            if (itemje["zcywtype"].ToStringEx() == "0")
                            {
                                allje += "预算内资金支付业务需经" + itemje["zcspr"] + "审核审批";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "1")
                            {
                                allje += "财政专项资金支付业务需经" + itemje["zcspr"] + "审核审批";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "2")
                            {
                                allje += "非财政专项资金支付业务需经" + itemje["zcspr"] + "审核审批";
                            }
                        }
                        else
                        {
                            if (itemje["zcywtype"].ToStringEx() == "0")
                            {
                                allje += "预算内资金支付业务需经" + itemje["zcspr"] + "审核审批；";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "1")
                            {
                                allje += "财政专项资金支付业务需经" + itemje["zcspr"] + "审核审批；";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "2")
                            {
                                allje += "非财政专项资金支付业务需经" + itemje["zcspr"] + "审核审批；";
                            }
                        }
                    }
                    else
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            if (itemje["zcywtype"].ToStringEx() == "0")
                            {
                                allje += "预算内资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "1")
                            {
                                allje += "财政专项资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "2")
                            {
                                allje += "非财政专项资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批";
                            }
                        }
                        else
                        {
                            if (itemje["zcywtype"].ToStringEx() == "0")
                            {
                                allje += "预算内资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批；";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "1")
                            {
                                allje += "财政专项资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批；";
                            }
                            else if (itemje["zcywtype"].ToStringEx() == "2")
                            {
                                allje += "非财政专项资金支付单笔支出金额在" + itemje["zcmoney"] + "元" + itemje["zctype"] + "的，由" + itemje["zcspr"] + "审核审批；";
                            }
                        }
                    }
                }
                Keyword.Add(WordKey, allje);
                return;
            }
            else if (WordKey == "key_jkyw0419")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlje = "select * from Nksc_Jkyw where id='" + id + "' order by sort";
                DataTable dtje = bll.Select(tsqlje);
                string allje = "";
                for (int i = 0; i < dtje.Rows.Count; i++)
                {
                    DataRow itemje = dtje.Rows[i];
                    if (itemje["jkmoney"].ToStringEx() == "")
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "借款业务需经" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "借款业务需经" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                    else
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "借款金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "借款金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                }
                Keyword.Add(WordKey, allje);
                return;
            }
            else if (WordKey == "key_cghtsq01")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlje = "select * from Nksc_cghtsq where id='" + id + "' and htywtype='0' order by sort";
                DataTable dtje = bll.Select(tsqlje);
                string allje = "";
                for (int i = 0; i < dtje.Rows.Count; i++)
                {
                    DataRow itemje = dtje.Rows[i];
                    if (itemje["jkmoney"].ToStringEx() == "")
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "合同审批授权业务需经" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "合同审批授权业务需经" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                    else
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                }
                Keyword.Add(WordKey, allje);
                return;
            }
            else if (WordKey == "key_dwzxcgsq01")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlje = "select * from Nksc_cghtsq where id='" + id + "' and htywtype='1' order by sort";
                DataTable dtje = bll.Select(tsqlje);
                string allje = "";
                for (int i = 0; i < dtje.Rows.Count; i++)
                {
                    DataRow itemje = dtje.Rows[i];
                    if (itemje["jkmoney"].ToStringEx() == "")
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "单位自行采购金额审批授权业务需经" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "单位自行采购金额审批授权业务需经" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                    else
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "单位自行采购金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "单位自行采购金额在" + itemje["jkmoney"] + "元" + itemje["jktype"] + "的，由" + itemje["jkspr"] + "审核审批；";
                        }
                    }
                }
                Keyword.Add(WordKey, allje);
                return;
            }
            else if (WordKey == "key_bxyw0419")
            {
                NkscBLL bll = new NkscBLL();
                String tsqlje = "select * from Nksc_Bxyw where id='" + id + "' order by sort";
                DataTable dtje = bll.Select(tsqlje);
                string allje = "";
                for (int i = 0; i < dtje.Rows.Count; i++)
                {
                    DataRow itemje = dtje.Rows[i];
                    if (itemje["bxmoney"].ToStringEx() == "")
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "报销业务需经" + itemje["bxspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "报销业务需经" + itemje["bxspr"] + "审核审批；";
                        }
                    }
                    else
                    {
                        if (i == dtje.Rows.Count - 1)
                        {
                            allje += "报销金额在" + itemje["bxmoney"] + "元" + itemje["bxtype"] + "的，由" + itemje["bxspr"] + "审核审批";
                        }
                        else
                        {
                            allje += "报销金额在" + itemje["bxmoney"] + "元" + itemje["bxtype"] + "的，由" + itemje["bxspr"] + "审核审批；";
                        }
                    }
                }
                Keyword.Add(WordKey, allje);
                return;
            }
            else if (WordKey == "key_kqsjxxxx")
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["kqsjswSd"].ToStringEx())
                    || string.IsNullOrEmpty(dt.Rows[0]["kqsjswEd"].ToStringEx())
                    || string.IsNullOrEmpty(dt.Rows[0]["kqsjxwSd"].ToStringEx())
                    || string.IsNullOrEmpty(dt.Rows[0]["kqsjxwEd"].ToStringEx()))
                {
                    string sw = dt.Rows[0]["kqsjswS"].ToStringEx() + "- " + dt.Rows[0]["kqsjswE"].ToStringEx();
                    string xw = dt.Rows[0]["kqsjxwS"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwE"].ToStringEx();
                    string neirong = "上午：" + sw + "、下午：" + xw + "";
                    Keyword.Add(WordKey, neirong);
                    return;
                }
                else
                {
                    string sw = dt.Rows[0]["kqsjswS"].ToStringEx() + "- " + dt.Rows[0]["kqsjswE"].ToStringEx();
                    string xw = dt.Rows[0]["kqsjxwS"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwE"].ToStringEx();
                    string dsw = dt.Rows[0]["kqsjswSd"].ToStringEx() + "- " + dt.Rows[0]["kqsjswEd"].ToStringEx();
                    string dxw = dt.Rows[0]["kqsjxwSd"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwEd"].ToStringEx();
                    string neirong = "夏令时 上午：" + sw + "、下午：" + xw + "&p冬令时 上午：" + dsw + "、下午：" + dxw + "";
                    Keyword.Add(WordKey, neirong);
                    return;
                }
            }
            else if (WordKey == "key_jine041509")
            {
                if (dt.Rows[0][DBKey].ToStringEx() == "0.00")
                {
                    Keyword.Add("（1）#jine041509元以下的零星支出。", "");
                    Keyword.Add("（2）出差人员必须随身携带的差旅费。", "（1）出差人员必须随身携带的差旅费。");
                    Keyword.Add("（3）发放给职工的个人支出。", "（2）发放给职工的个人支出。");
                    Keyword.Add("（4）根据有关规定需要支付现金的其他费用。", "（3）根据有关规定需要支付现金的其他费用。");
                    return;
                }
                else
                {
                    Keyword.Add(WordKey, dt.Rows[0][DBKey].ToStringEx());
                    return;
                }
            }
            else if (WordKey == "key_qlqd")
            {
                
                NkscBLL bll = new NkscBLL();
                string zzzwmc = dt.Rows[0]["zzzwmc"].ToStringEx();
                string ldzzmc = dt.Rows[0]["ldzzmc"].ToStringEx();
                string ldzzfg = dt.Rows[0]["ldzzfg"].ToStringEx();
                string fzzwmc1 = dt.Rows[0]["fzzwmc1"].ToStringEx();
                string ldfzmc1 = dt.Rows[0]["ldfzmc1"].ToStringEx();
                string ldfzfg1 = dt.Rows[0]["ldfzfg1"].ToStringEx();

                List<Nksc_fz> list_fz = bll.SelectNkscfz(Guid.Parse(id));
                Nksc_fz scfz = new Nksc_fz();
                scfz.id = Guid.Parse(id);
                scfz.fzzwmc = fzzwmc1;
                scfz.ldfzmc = ldfzmc1;
                scfz.ldfzfg = ldfzfg1;
                list_fz.Insert(0, scfz);

                Nksc_fz sczz = new Nksc_fz();
                sczz.id = Guid.Parse(id);
                sczz.fzzwmc = zzzwmc;
                sczz.ldfzmc = ldzzmc;
                sczz.ldfzfg = ldzzfg;
                list_fz.Insert(0, sczz);

                StringBuilder QlqdValue = new StringBuilder();
                List<Nksc_qlqd> list_qlqd = bll.SelectQlqd(id);

                foreach (Nksc_fz item in list_fz)
                {
                    QlqdValue.Append(item.ldfzmc + item.fzzwmc + "职权清单&");
                    QlqdValue.Append("职权行使人：" + item.ldfzmc + "&");
                    QlqdValue.Append("工作分工：" + item.ldfzfg + "&☆");

                    int xh = 0;
                    List<Nksc_qlqd> list_qlqds = list_qlqd.Where(s => s.leder == item.ldfzmc + item.fzzwmc).ToList<Nksc_qlqd>();
                    for (int i = 0; i < list_qlqds.Count; i++)
                    {
                        QlqdValue.Append(++xh + "★" + list_qlqds[i].qlsxname + "★" + list_qlqds[i].qltext.Replace("\n", "&") + "★");
                    }
                    QlqdValue.Append("☆");
                }
                Keyword.Add(WordKey, QlqdValue.ToString().TrimEnd('☆').TrimEnd('★'));
            }
        }

        #endregion

        #region 替换类型

        [SupportFilter]
        public ActionResult WordTempType()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("替换类型");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_WordTempType(string ID, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(ID))
            {
                where += "and ID = '" + ID + "'";
            }
            WordTempTypeBLL bll = new WordTempTypeBLL();
            List<WordTempType> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_WordTempType(string ID, bool AddType = false)
        {
            WordTempTypeBLL bll = new WordTempTypeBLL();
            ViewBag.AddType = AddType;
            WordTempType result = new WordTempType { ID = ID };
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
        public JsonResult Create_WordTempType(WordTempType model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, " 名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Remark))
            {
                return Json(JsonHandler.CreateMessage(0, " 描述 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Name = string.IsNullOrEmpty(model.Name) ? "" : model.Name;
            model.Remark = string.IsNullOrEmpty(model.Remark) ? "" : model.Remark;
            WordTempTypeBLL bll = new WordTempTypeBLL();
            if (AddType)
            {
                //创建
                model.ID = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加替换类型:" + model.ID, Suggestion.Succes, "替换类型");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加替换类型:" + model.ID, Suggestion.Error, "替换类型");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改替换类型:" + model.ID, Suggestion.Succes, "替换类型");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改替换类型:" + model.ID, Suggestion.Error, "替换类型");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_WordTempType(string ID)
        {
            WordTempTypeBLL bll = new WordTempTypeBLL();
            if (bll.Delete(ID) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除替换类型:" + ID, Suggestion.Succes, "替换类型");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除替换类型:" + ID, Suggestion.Error, "替换类型");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetComb_WordTempType(bool All = false)
        {
            GridPager pager = new GridPager { page = 1, rows = 100, sort = "ID", order = "asc" };
            WordTempTypeBLL bll = new WordTempTypeBLL();
            string where = "";
            IList<WordTempType> list = bll.SelectAll(where, pager);
            if (All)
            {
                list.Insert(0, new WordTempType() { ID = "", Name = "全部", Remark = "" });
            }
            return Json(list);
        }

        #endregion

        #region 模板文件

        [SupportFilter]
        public ActionResult WordTempFile()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("模板文件");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_WordTempFile(string ID, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(ID))
            {
                where += "and ID = '" + ID + "'";
            }
            WordTempFileBLL bll = new WordTempFileBLL();
            List<WordTempFile> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_WordTempFile(string ID, bool AddType = false)
        {
            WordTempFileBLL bll = new WordTempFileBLL();
            ViewBag.AddType = AddType;
            WordTempFile result = new WordTempFile { ID = ID };
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
        public JsonResult Create_WordTempFile(WordTempFile model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.ID))
            {
                return Json(JsonHandler.CreateMessage(0, "编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.WordFile))
            {
                return Json(JsonHandler.CreateMessage(0, "文件名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.NewPage))
            {
                return Json(JsonHandler.CreateMessage(0, "是否另起一页 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ywKey))
            {
                return Json(JsonHandler.CreateMessage(0, "对应业务代码 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            model.Name = string.IsNullOrEmpty(model.Name) ? "" : model.Name;
            model.WordFile = string.IsNullOrEmpty(model.WordFile) ? "" : model.WordFile;
            model.NewPage = string.IsNullOrEmpty(model.NewPage) ? "" : model.NewPage;
            model.ywKey = string.IsNullOrEmpty(model.ywKey) ? "" : model.ywKey;

            WordTempFileBLL bll = new WordTempFileBLL();
            if (AddType)
            {
                bll.UpdateSortAdd(model.Sort);
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模板文件:" + model.ID, Suggestion.Succes, "模板文件");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模板文件:" + model.ID, Suggestion.Error, "模板文件");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模板文件:" + model.ID, Suggestion.Succes, "模板文件");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模板文件:" + model.ID, Suggestion.Error, "模板文件");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_WordTempFile(string ID)
        {
            WordTempFileBLL bll = new WordTempFileBLL();
            if (bll.Delete(ID) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除模板文件:" + ID, Suggestion.Succes, "模板文件");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除模板文件:" + ID, Suggestion.Error, "模板文件");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetTree_WordTempFile(bool isAll = true)
        {
            GridPager pager = new GridPager { page = 1, rows = 50, sort = "Sort", order = "asc" };
            WordTempFileBLL bll = new WordTempFileBLL();
            string where = "";
            List<WordTempFile> result = bll.SelectAll(where, pager);
            IList<EasyUIJsonTree> json = new List<EasyUIJsonTree>();

            EasyUIJsonTree itemAll = null;
            if (isAll)
            {
                itemAll = new EasyUIJsonTree();
                itemAll.id = "";
                itemAll.text = "全部";
                itemAll.children = new List<EasyUIJsonTree>();
                json.Add(itemAll);
            }

            foreach (WordTempFile dr in result)
            {
                EasyUIJsonTree item = new EasyUIJsonTree();
                item.id = dr.ID;
                item.text = dr.WordFile;

                if (isAll)
                {
                    itemAll.children.Add(item);
                }
                else
                {
                    json.Add(item);
                }
            }
            return Json(json);
        }
        #endregion

        #region 模板关键字

        [SupportFilter]
        public ActionResult WordTempKey()
        {
            ViewBag.Perm = GetPermission();
            AddLogLook("模板关键字");
            return View();
        }

        [HttpPost]
        public JsonResult GetData_WordTempKey(string Pid, string WordKey, string KeyType, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(Pid))
            {
                where += "and Zid = '" + Pid + "'";
            }
            if (!string.IsNullOrEmpty(WordKey))
            {
                where += "and WordKey like '%" + WordKey + "%'";
            }
            if (!string.IsNullOrEmpty(KeyType))
            {
                where += "and KeyType = '" + KeyType + "'";
            }
            WordTempKeyBLL bll = new WordTempKeyBLL();
            List<View_WordTempKey> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_WordTempKey(string PId, WordTempKey result, bool AddType = false)
        {
            WordTempKeyBLL bll = new WordTempKeyBLL();
            ViewBag.AddType = AddType;
            if (!AddType)
            {
                result = bll.GetRow(result);
            }
            else
            {
                result.Zid = PId;
                //if (!string.IsNullOrEmpty(PId))
                //{
                //    result._parentId = PId;
                //}
                //result.Id = bll.Maxid(PId);
            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_WordTempKey(WordTempKey model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.Zid))
            {
                return Json(JsonHandler.CreateMessage(0, "上级编号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.WordKey))
            {
                return Json(JsonHandler.CreateMessage(0, "Word关键字 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.KeyType))
            {
                return Json(JsonHandler.CreateMessage(0, "替换方式 不允许为空"), JsonRequestBehavior.AllowGet);
            }

            model.Zid = string.IsNullOrEmpty(model.Zid) ? "" : model.Zid;
            model.WordKey = string.IsNullOrEmpty(model.WordKey) ? "" : model.WordKey;
            model.DBKey = string.IsNullOrEmpty(model.DBKey) ? "" : model.DBKey;
            model.KeyType = string.IsNullOrEmpty(model.KeyType) ? "" : model.KeyType;
            model.Desc = string.IsNullOrEmpty(model.Desc) ? "" : model.Desc;
            model.ywType = string.IsNullOrEmpty(model.ywType) ? "" : model.ywType;
            WordTempKeyBLL bll = new WordTempKeyBLL();
            if (AddType)
            {
                //创建
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模板关键字:" + model.id, Suggestion.Succes, "模板关键字");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加模板关键字:" + model.id, Suggestion.Error, "模板关键字");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模板关键字:" + model.id, Suggestion.Succes, "模板关键字");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改模板关键字:" + model.id, Suggestion.Error, "模板关键字");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_WordTempKey(string id)
        {
            WordTempKeyBLL bll = new WordTempKeyBLL();
            if (bll.Delete(id) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除模板关键字:" + id, Suggestion.Succes, "模板关键字");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除模板关键字:" + id, Suggestion.Error, "模板关键字");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 小组关键字

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_WordTempXZ(string dkey, bool AddType = false)
        {
            WordTempXZBLL bll = new WordTempXZBLL();
            ViewBag.AddType = AddType;
            WordTempXZ result = new WordTempXZ { dkey = dkey };
            if (!AddType)
            {
                result = bll.GetRow(result);
                result.dkey = dkey;
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_WordTempXZ(WordTempXZ model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.dkey))
            {
                return Json(JsonHandler.CreateMessage(0, " Wrod关键字 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.zz))
            {
                return Json(JsonHandler.CreateMessage(0, " 正职字段名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.fz))
            {
                return Json(JsonHandler.CreateMessage(0, " 副职字段名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.cy))
            {
                return Json(JsonHandler.CreateMessage(0, " 成员字段名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.dkey = string.IsNullOrEmpty(model.dkey) ? "" : model.dkey;
            model.zz = string.IsNullOrEmpty(model.zz) ? "" : model.zz;
            model.fz = string.IsNullOrEmpty(model.fz) ? "" : model.fz;
            model.qtks = string.IsNullOrEmpty(model.qtks) ? "" : model.qtks;
            model.cy = string.IsNullOrEmpty(model.cy) ? "" : model.cy;
            WordTempXZBLL bll = new WordTempXZBLL();
            if (AddType || string.IsNullOrEmpty(model.ID))
            {
                //创建
                model.ID = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加小组关键字:" + model.ID, Suggestion.Succes, "小组关键字");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加小组关键字:" + model.ID, Suggestion.Error, "小组关键字");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改小组关键字:" + model.ID, Suggestion.Succes, "小组关键字");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改小组关键字:" + model.ID, Suggestion.Error, "小组关键字");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region 流程图配置

        [HttpPost]
        public JsonResult GetData_WordTempLCT(string wkey, GridPager pager)
        {
            string where = "";
            if (!string.IsNullOrEmpty(wkey))
            {
                where += "and wkey = '" + wkey + "'";
            }
            WordTempLCTBLL bll = new WordTempLCTBLL();
            List<WordTempLCT> result = bll.SelectAll(where, pager);
            var json = new
            {
                total = pager.totalRows,
                rows = result
            };
            return Json(json);
        }

        [SupportFilter(ActionName = "CUD")]
        public ActionResult Create_WordTempLCT(string wkey, bool AddType = false)
        {
            WordTempLCTBLL bll = new WordTempLCTBLL();
            ViewBag.AddType = AddType;
            WordTempLCT result = new WordTempLCT { wkey = wkey };
            if (!AddType)
            {
                result = bll.GetRow(result);
                if (string.IsNullOrEmpty(result.wkey))
                {
                    result.wkey = wkey;
                }
                if (string.IsNullOrEmpty(result.formate))
                {
                    result.formate = "{0}";
                }
                if (string.IsNullOrEmpty(result.fontName))
                {
                    result.fontName = "黑体";
                }
                if (result.fontSize <= 0)
                {
                    result.fontSize = 12;
                }
            }
            else
            {

            }
            return View(result);
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Create_WordTempLCT(WordTempLCT model, bool AddType = false)
        {
            if (string.IsNullOrEmpty(model.wkey))
            {
                return Json(JsonHandler.CreateMessage(0, " 书签名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.dkey))
            {
                return Json(JsonHandler.CreateMessage(0, " 数据库字段 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.formate))
            {
                return Json(JsonHandler.CreateMessage(0, " 格式化字符串 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.fontName))
            {
                return Json(JsonHandler.CreateMessage(0, " 字体名称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.wkey = string.IsNullOrEmpty(model.wkey) ? "" : model.wkey;
            model.dkey = string.IsNullOrEmpty(model.dkey) ? "" : model.dkey;
            model.formate = string.IsNullOrEmpty(model.formate) ? "" : model.formate;
            model.fontName = string.IsNullOrEmpty(model.fontName) ? "" : model.fontName;
            WordTempLCTBLL bll = new WordTempLCTBLL();
            if (AddType)
            {
                //创建
                model.ID = bll.Maxid();
                if (bll.Insert(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "添加流程图配置:" + model.ID, Suggestion.Succes, "流程图配置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "添加流程图配置:" + model.ID, Suggestion.Error, "流程图配置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //修改
                if (bll.Update(model) > 0)
                {
                    LogHelper.AddLogUser(GetUserId(), "修改流程图配置:" + model.ID, Suggestion.Succes, "流程图配置");
                    return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LogHelper.AddLogUser(GetUserId(), "修改流程图配置:" + model.ID, Suggestion.Error, "流程图配置");
                    return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult Delete_WordTempLCT(string ID)
        {
            WordTempLCTBLL bll = new WordTempLCTBLL();
            if (bll.Delete(ID) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "删除流程图配置:" + ID, Suggestion.Succes, "流程图配置");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "删除流程图配置:" + ID, Suggestion.Error, "流程图配置");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 领导权力清单
        [HttpPost]
        public JsonResult Create_QLQD(String nkid)
        {
            List<S_qlqd> result = new List<S_qlqd>();

            string citiname = new SaleCustomerBLL().GetNameStr("CzjName", " and ID=(select CustomerID from Nksc where id='" + nkid + "')");

            NkscBLL bll = new NkscBLL();
            List<string> leders = bll.Select_leder(nkid);
            string dzzjgmc = bll.GetNameStr("dzzjgmc", "and id='" + nkid + "'");
            List<Nksc_qlqd> qlqds = bll.SelectQlqd("00000000-0000-0000-0000-000000000000");

            //正职模板
            List<Nksc_qlqd> zzTemp = qlqds.Where(s => s.leder.Equals("zz")).ToList();
            //副职模板
            List<Nksc_qlqd> fzTemp = qlqds.Where(s => s.leder.Equals("fz")).ToList();

            S_qlqd zz = new S_qlqd();
            string zw = bll.GetNameStr("zzzwmc", "and id='" + nkid + "'");

            if (string.IsNullOrEmpty(dzzjgmc) || dzzjgmc == "无")
            {
                dzzjgmc = zw + "办公会";
            }
            else
            {
                dzzjgmc = dzzjgmc + "会";
            }

            int qlsort = 0;

            string mc = bll.GetNameStr("ldzzmc", "and id='" + nkid + "'");
            string fg = bll.GetNameStr("ldzzfg", "and id='" + nkid + "'");
            zz.leder = mc + zw;
            zz.qlqds = zzTemp;
            if (!leders.Contains(mc + zw))
            {
                foreach (Nksc_qlqd item in zz.qlqds)
                {
                    item.qltext = item.qltext.Replace("XXX科室", fg).Replace("key_DZZJGMC会", dzzjgmc).Replace("区委", citiname + "委").Replace("区政府", citiname + "政府").Replace("区直", citiname + "直");
                    item.qlsort = ++qlsort;
                }
                result.Add(zz);
            }


            string fzw = bll.GetNameStr("fzzwmc1", "and id='" + nkid + "'");
            string fmc = bll.GetNameStr("ldfzmc1", "and id='" + nkid + "'");
            if (!string.IsNullOrEmpty(fzw) && !string.IsNullOrEmpty(fmc))
            {
                S_qlqd fz = new S_qlqd();
                fz.leder = fmc + fzw;
                if (!leders.Contains(fmc + fzw))
                {
                    Copy((List<Nksc_qlqd>)fz.qlqds, fzTemp, fzw, zw, dzzjgmc, citiname, ref qlsort);
                    result.Add(fz);
                }
            }

            List<Nksc_fz> Nksc_fzModel = bll.SelectNkscfz(Guid.Parse(nkid));
            foreach (Nksc_fz fzitem in Nksc_fzModel)
            {
                S_qlqd fz = new S_qlqd();
                fz.leder = fzitem.ldfzmc + fzitem.fzzwmc;
                if (!leders.Contains(fzitem.ldfzmc + fzitem.fzzwmc))
                {
                    Copy((List<Nksc_qlqd>)fz.qlqds, fzTemp, fzitem.fzzwmc, zw, dzzjgmc, citiname, ref qlsort);
                    result.Add(fz);
                }
            }

            return Json(result);
        }

        private void Copy(List<Nksc_qlqd> qlqds, List<Nksc_qlqd> fztmp, string fzmc, string zzmc, string dzzmc, string citiname, ref int qlsort)
        {
            foreach (Nksc_qlqd item in fztmp)
            {
                Nksc_qlqd nitem = new Nksc_qlqd();
                nitem.id = item.id;
                nitem.leder = item.leder;
                nitem.qlsx = item.qlsx;
                nitem.qlsxname = item.qlsxname;
                nitem.qltext = item.qltext.Replace("#zw#", fzmc).Replace("key_zzzwmc", zzmc).Replace("key_DZZJGMC会", dzzmc).Replace("区委", citiname + "委").Replace("区政府", citiname + "政府");
                nitem.qlsort = ++qlsort;
                qlqds.Add(nitem);
            }
        }

        [HttpPost]
        public JsonResult Delete_Qlqd(string nkid, string leder)
        {
            NkscBLL bll = new NkscBLL();
            string[] leders = leder.TrimEnd('-').Split('-');
            if (bll.Delete_Qlqd(nkid, leders) > 0)
            {
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete_QlqdAll(string nkid)
        {
            NkscBLL bll = new NkscBLL();
            if (bll.Delete_Qlqd(nkid) > 0)
            {
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 会议纪要
        public FileResult BuildWord_Hyjy(string id)
        {
            NkscBLL bll = new NkscBLL();
            string Ldall = bll.getld(id);
            DataTable dt_nk1 = bll.SelectNkscData("DWQC,nkldxzzz,nkldxzfzz,nkldxzcy", id);

            string dePname = dt_nk1.Rows[0]["DWQC"].ToStringEx();
            string dir = Server.MapPath("~/wordHyjy/" + dePname + "/");
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            string tempPath = Server.MapPath("~/wordHyjy/Temp/");

            JMProject.Word.WordHyjy _word = new JMProject.Word.WordHyjy();


            DataTable dt_nk2 = bll.SelectNkscData("nbkzgzxzzz01,nbkzgzxzfzz01,nbkzgzxzzzcy01,nbkzgzxzzzqt01", id);

            string fxpgxzcy = bll.GetNameStr("fxpgxzcy", " and id='" + id + "'");
            string bhyw = bll.GetNameStr("bhyw", " and id='" + id + "'");

            DataTable dt_nk4 = bll.SelectNkscData("ldzzmc,nkldxzcy,nkldxzzz,nkldxzfzz", id);

            string DWQC = dt_nk1.Rows[0]["DWQC"].ToStringEx();
            string ldzzmc = dt_nk4.Rows[0]["ldzzmc"].ToStringEx();
            string nbkzgzxzzzcy01 = dt_nk2.Rows[0]["nbkzgzxzzzcy01"].ToStringEx();

            _word.Create1(tempPath, dir, dt_nk1, Ldall);
            _word.Create2(tempPath, dir, dt_nk2);
            _word.Create3(tempPath, dir, fxpgxzcy, bhyw, DWQC);
            DateTime ldhysj = _word.Create4(tempPath, dir, dt_nk4);
            _word.Create5(tempPath, dir, ldzzmc, DWQC, ldhysj);
            _word.Create6(tempPath, dir, ldzzmc, nbkzgzxzzzcy01, ldhysj);
            _word.Create7(tempPath, dir, ldzzmc, nbkzgzxzzzcy01);

            string FileToZip = Server.MapPath("~/wordHyjy/") + dePname + ".zip";
            ZipClass.ZipFile(FileToZip, dir);
            System.IO.Directory.Delete(dir, true);
            return File(FileToZip, "application/zip", dePname + ".zip");
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Drawing;
using System.Data;
using System.Reflection;

namespace JMProject.Common
{
    public class WordHelper
    {
        #region 生成手册文件
        /// <summary>
        /// 替换word中的文本，并导出word
        /// </summary>
        /// <param name="datas">要替换的 关键字与值</param>
        /// <param name="datasImg">要替换的 图片</param>
        /// <param name="TempFile">模版文件路径</param>
        /// <param name="newFile">新文件路径</param>
        /// <param name="imgFileDir">生成图片存放路径</param>
        /// <param name="imgFileDirGD">固定填空图片路径</param>
        /// <param name="parameter">制度相关明细参数 WordHelperModel类型</param>
        /// <returns></returns>
        public static string ReplaceToWord(Dictionary<string, string> datas, Dictionary<string, ImgModel> datasImg
                                    , string TempFile, string newFile, string imgFileDir, string imgFileDirGD, WordHelperModel parameter)
        {

            string[] ary_font = new string[] { "仿宋", "黑体", "楷体", "宋体 (中文正文)", "幼圆" };
            string[] ary_xyqy = new string[] { "序  言", "前  言" };

            string xyqyDesc = "DWQC，为进一步提高内部管理水平，规范权利运行机制和单位内部控制，加强廉政风险防控机制的建设，"
                        + "全面贯彻落实财政部颁布的《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》精神，"
                        + "结合本单位工作实际情况组织编制《DWQC内部控制手册》,本册作为建立、执行、评价单位内部控制的依据。"
                        + "确保单位从思想上提高管理水平，增强风险防控意识和能力，保证单位协调、持续、快速发展。"
                        + "\r本册的编制实施对完善单位内部控制体系，进一步规范内部管理和业务流程，有效防控单位风险，保证财务信息真实完整，维护单位财产安全与高效运行具有重要意义。"
                        + "\r本册在编制过程中遵循全面性、重要性、制衡性、适应性、有效性的基本原则，结合实际工作，通过梳理业务流程"
                        + "，拟定风险控制措施，为建立本单位内部控制体系奠定坚实基础。"
                        + "本册是本单位工作必备工具，本册在试运行期间，将根据实际执行情况和发现的问题进行不断完善，敬请批评指正。";

            string xyqyDesc1 = "为了贯彻落实财政部颁布的《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》"
                        + "等文件精神，进一步强化我单位内部管理工作，结合我单位实际，我们组织汇编了《DWQC内部控制手册》，作为建立、执行、评价本单位内部控制的依据。"
                        + "\r在手册的编制过程中，我单位领导干部深入调研分析，充分沟通交流，通过对各科室的业务风险管理程序、制度、岗位职责的认真梳理、调整，"
                        + "使制度与流程的优化工作更加具有了广泛的群众基础，极大地提高了广大干部的业务能力和风险防范意识，发挥了内控体系建设反腐倡廉、防范风险的作用。"
                        + "\r随着内部控制政策的不断推进，我们要在实际工作中定期对手册进行更新，不断完善业务风险管理程序，全面推进单位内部管理向科学化、精细化迈进。"
                        + "\r\r\r\rDWQC\r#bfrq";

            string xyqyDesc2 = "为提高DWQC内部管理水平，建立权利运行和权力监督机制，增强单位内部控制，加强廉政风险防控机制的建设，全面贯彻落实财政部颁布的"
                        + "《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》精神，结合DWQC工作实际情况，组织编制《DWQC内部控制手册》。"
                        + "\r本单位内部控制手册作为建立、执行、评价单位内部控制的依据。确保单位从思想上提高管理水平，增强风险防控意识和能力，保证单位协调、持续、快速发展。"
                        + "\r本单位内部控制手册的编制实施对完善单位内部控制体系，进一步规范内部管理及业务流程，有效防控单位风险，保证财务信息真实完整，"
                        + "维护单位财产安全与高效运行等方面具有极其重要意义。"
                        + "\r本单位内部控制手册在编制过程中，应当遵循全面性、有效性、适应性、重要性、制衡性五大基本原则，通过梳理业务流程，结合工作实际"
                        + "，拟定风险控制措施，为建立本单位内部控制体系奠定坚实基础。"
                        + "\r本手册是本单位开展工作的必备工具，本手册在试运行期间，将根据实际执行情况和发现的问题进行不断完善，敬请各级领导批评指正。";

            string xyqyDesc3 = "为了提升DWQC内部管理水平，实现治理体系现代化，规范DWQC权利运行机制，加强廉政风险防控建设，贯彻落实党的十八届四中全会通过的"
                        + "《中共中央关于全面推进依法治国若干重大问题的决定》明确提出：“对财政资金分配使用、国有资产监管、政府投资、政府采购、公共资源转让、"
                        + "公共工程建设等权力集中的部门和岗位实行分事行权、分岗设权、分级授权，定期轮岗，强化内部流程控制，防止权力滥用”的指示精神，依据财政部颁布的"
                        + "《行政事业单位内部控制规范（试行）》和《关于全面推进行政事业单位内部控制建设的指导意见》，以及吉林省财政厅《行政事业单位内部控制规范基本指引》"
                        + "指导思想，结合本单位工作实际情况组织编制《DWQC内部控制手册》,本手册是建立、执行、评价本单位内部控制的重要依据。"
                        + "\rDWQC内部控制手册严格遵循《行政事业单位内部控制规范》的全面性、重要性、制衡性、适应性、有效性的基本原则，梳理业务流程，"
                        + "找出风险点并拟定风险控制措施，结合实际，为建立与实施DWQC内部控制奠定坚实基础。手册是本单位工作必备工具书，本手册试运行期间，"
                        + "根据实际执行情况和发现的问题不断完善，敬请批评指正。";

            string xyqyDesc4 = "为贯彻落实党的十八届四中全会通过的《中共中央关于全面推进依法治国若干重大问题的决定》的重要指示精神，提升DWQC内部管理水平，"
                        + "实现治理能力现代化，规范本单位权利运行与权力监督工作机制，建设廉政风险防控制度，对本单位重要领域和关键岗位实行分事行权、分岗设权、"
                        + "分级授权，关键岗位定期轮岗，强化内部流程控制，防止权力滥用，并根据财政部颁布的《行政事业单位内部控制规范（试行）》和"
                        + "《关于全面推进行政事业单位内部控制建设的指导意见》，结合本单位实际情况组织编制《DWQC内部控制手册》,本手册是建立、执行、评价本单位内部控制的重要依据。"
                        + "\r本手册严格遵循《内部控制规范》的五项基本原则和八个控制方法，开展业务流程梳理，找出风险点并拟定风险控制措施，结合实际，"
                        + "为建立与实施DWQC内部控制奠定坚实基础。手册是本单位工作必备工具书，本手册试运行期间，根据实际执行情况和发现的问题不断完善，敬请批评指正。";

            string xyqyDesc5 = "为贯彻落实党的十八届四中全会提出的“全面推进依法治国”指示精神，加强“对财政资金分配使用、国有资产监管、政府投资、政府采购、公共资源转让"
                        + "、公共工程建设等权力集中的部门和岗位实行分事行权、分岗设权、分级授权，定期轮岗，强化内部流程控制，防止权力滥用”的管理控制，"
                        + "我单位依据2012年财政部引发的《行政事业单位内部控制规范（试行）》和2015年财政部引发的《关于全面推进行政事业单位内部控制建设的指导意见》文件要求，"
                        + "结合本单位实际，组织汇编了《DWQC内部控制手册》。"
                        + "\r《内部控制手册》将成为DWQC日常工作指南和基础管理制度，本手册将严格遵循《行政事业单位内部控制规范》的全面性、重要性、制衡性、适应性、"
                        + "有效性的基本原则，规范权力运行机制和单位内部控制，加强廉政建设，通过对各经济业务的梳理，分析风险并制定风险控制办法和目标，"
                        + "明确重大领域和关键岗位的工作机制、授权范围、权力职责。"
                        + "\r通过本手册的编制，完善单位内部控制体系建设与执行，进一步规范DWQC内部管理和业务流程，有效防控单位经济业务活动风险，确保本单位财产安全和使用有效，"
                        + "保证本单位财务信息真实完整，提高本单位服务效率效能具有重要意义。";

            string[] ary_xyqydesc = new string[] { xyqyDesc, xyqyDesc1, xyqyDesc2, xyqyDesc3, xyqyDesc4, xyqyDesc5 };

            object oMissing = System.Reflection.Missing.Value;

            word.Application app = null;
            word.Document doc = null;
            object physicNewFile = newFile;//生成文件路径
            object fileName = TempFile;//模板文件路径
            string wordhead = "";//页眉文字内容

            try
            {
                app = new word.Application();//创建word应用程序
                //打开模板文件
                doc = app.Documents.Open(ref fileName,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //随机 前言或序言
                string xyqyStr = ary_xyqy[new Random().Next(2)];
                //段内容 关键字集合
                Dictionary<string, string> datas_sort = new Dictionary<string, string>();
                //长内容 关键字集合
                Dictionary<string, string> datas_long = new Dictionary<string, string>();
                datas_long.Add("#XYQY", xyqyStr);
                datas_long.Add("#XY_SJ", ary_xyqydesc[new Random().Next(6)]);
                foreach (var item in datas)
                {
                    //超长内容  单独处理
                    if (item.Key == "#gwznlist" || item.Key == "#dzcylist" || item.Key == "#dwjj" || item.Key == "#ywslzdmc" ||
                        item.Key == "#zjzfsp0419" || item.Key == "#zjzfspqx0419" || item.Key == "#scxzbm" || item.Key == "#nkldxzcy" ||
                        item.Key == "#nbkzgzxzzzcy01")
                    {
                        string newValue = string.IsNullOrEmpty(item.Value) ? " " : item.Value;
                        datas_long.Add(item.Key, newValue);
                    }
                    else if (item.Key == "#bhyw" || String.IsNullOrEmpty(item.Value))//本单位包含业务内容
                    {
                        continue;
                    }
                    else
                    {
                        datas_sort.Add(item.Key, item.Value);
                        if (item.Key == "DWQC")
                        {
                            wordhead = item.Value + "内部控制手册";
                        }
                    }
                }

                None_All(doc, app, parameter);

                // 党组  党支部  党委  无
                None_dwgl(doc, app, parameter.dwgl);

                //if (!parameter.sfyw)//无收费业务
                //{
                //    None_sfyw(doc, app);
                //}

                if (!parameter.htgl)//无合同管理
                {
                    None_htgl(doc, app);
                }
                if (!parameter.jsxm)//无建设项目
                {
                    None_jsxm(doc, app);
                }
                if (!parameter.zfcg)//无政府采购
                {
                    None_zfcg(doc, app);
                }
                //else
                //{
                //采购合同审批与 自行采购
                None_zfcghtsq(doc, app, parameter.iscgsq, parameter.iszxcgsq);
                //}
                if (!parameter.zwyw)//无债务业务 zdgl_zwyw1
                {
                    None_zwyw(doc, app);
                }
                if (!parameter.dwtzyw)//无对外投资业务 zdgl_dwtzyw
                {
                    None_dwtzyw(doc, app);
                }

                //
                if (!parameter.isgwk)
                {
                    None_gwk(doc, app);
                }

                if (!parameter.iszxzjgl)
                {
                    None_zxzjgl(doc, app);
                }

                if (!parameter.isEngineRoom)//无 机房 zdgl_jifang
                {
                    None_EngineRoom(doc, app);
                }

                if (!parameter.isjkyw)//无借款业务
                {
                    None_jkgl(doc, app);
                }

                if (!parameter.isclf)//无差旅费
                {
                    None_clf(doc, app);
                }
                if (!parameter.ishyf)//无会议费
                {
                    None_hyf(doc, app);
                }
                if (!parameter.ispxf)//无培训费
                {
                    None_pxf(doc, app);
                }
                if (!parameter.iszdf)//无招待费
                {
                    None_zdf(doc, app);
                }

                setTitleText(app);

                //报账制度 或 会计制度
                IsBZorKJ(doc, app, parameter.isbz);

                //长内容替换
                ReplaceLong(app, datas_long);

                //短内容替换
                ReplaceSort(app, datas_sort);

                //替换页眉的字
                setPageHeader(app, wordhead);

                #region 获取图片 并把 图片插入对应书签中

                FlowChart fChart = new FlowChart();
                Dictionary<string, string> imagesRes = new Dictionary<string, string>();
                foreach (var item in datasImg)
                {
                    if (item.Value.imgtype == ImgType.XZ)
                    {
                        string imgFile = imgFileDir + item.Key + ".jpg";
                        Bitmap bit_img = fChart.FXPG(item.Value.zz, item.Value.fz, item.Value.qtks, item.Value.cy);
                        bit_img.Save(imgFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imagesRes.Add(item.Key, imgFile);
                    }
                    else if (item.Value.imgtype == ImgType.ZZJG)
                    {
                        string imgFile = imgFileDir + item.Key + ".jpg";
                        Bitmap bit_img = new zzjgChart().img_zzjg(item.Value.zz, item.Value.zzfgs, item.Value.fzzlist);
                        bit_img.Save(imgFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imagesRes.Add(item.Key, imgFile);
                    }
                    else if (item.Value.imgtype == ImgType.LCT)
                    {
                        string img_yt = imgFileDirGD + item.Value.ImgFileName;
                        Image bit_img = fChart.EditImageText(img_yt, item.Value.ImgTitle, item.Value.ImgTitleFont, item.Value.ImgTitleRect);
                        string img_xt = imgFileDir + item.Value.ImgFileName;
                        bit_img.Save(img_xt, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imagesRes.Add(item.Key, img_xt);
                    }
                }
                InsertImg(app, doc, imagesRes);

                #endregion

                int fontindex = int.Parse(NumHelper.GetDH("font"));
                NumHelper.UpdateDH("fontCount", "font");
                //设置随机字体
                doc.Content.Font.Name = ary_font[fontindex - 1];

                //对替换好的word模板另存为一个新的word文档
                doc.SaveAs(physicNewFile,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                return newFile;
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref oMissing, ref oMissing, ref oMissing);//关闭word文档
                }
                if (app != null)
                {
                    app.Quit(ref oMissing, ref oMissing);//退出word应用程序
                }
            }
        }

        /// <summary>
        /// 集合
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_All(word.Document doc, word.Application app, WordHelperModel parameter)
        {
            if (!parameter.ywcm_ysyw)
            {
                ReplaceBook(doc, "ywcm_ysyw");//预算业务
            }

            if (!parameter.ywcm_szyw_fssr)
            {
                ReplaceBook(doc, "ywcm_szyw_fssr");//非税收入
            }

            if (!parameter.ywcm_szyw_srdjrz)
            {
                ReplaceBook(doc, "ywcm_szyw_srdjrz");//收入登记入账
            }

            if (!parameter.ywcm_szyw_czpjgl)
            {
                ReplaceBook(doc, "ywcm_szyw_czpjgl1");
                ReplaceBook(doc, "ywcm_szyw_czpjgl2");
                ReplaceBook(doc, "ywcm_szyw_czpjgl3");
            }

            if (!parameter.ywcm_szyw_czsqzf)
            {
                //财政授权支付
                ReplaceBook(doc, "ywcm_szyw_czsqzf1");
                ReplaceBook(doc, "ywcm_szyw_czsqzf2");
            }

            //财政直接支付
            if (parameter.ywcm_szyw_czzjzf)
            {
                //收入业务流程图
                ReplaceBook(doc, "ywcm_szyw_czzjzf1");
            }
            else
            {
                //财政直接支付流程
                ReplaceBook(doc, "ywcm_szyw_czzjzf2");
            }

            if (!parameter.ywcm_zfcg_zfgmff)
            {
                //政府购买服务流程
                ReplaceBook(doc, "ywcm_zfcg_zfgmff");
            }

            if (!parameter.ywcm_zcgl_gyzcczcj)
            {
                //国有资产出租出借业务
                ReplaceBook(doc, "ywcm_zcgl_gyzcczcj");
            }

            if (!parameter.ywcm_zcgl_gyzcsrsj)
            {
                //国有资产收入上缴流程
                ReplaceBook(doc, "ywcm_zcgl_gyzcsrsj");
            }

            if (!parameter.ywcm_zcgl_gyzcdjbgzx)
            {
                //国有资产产权登记、变更、注销流程 
                ReplaceBook(doc, "ywcm_zcgl_gyzcdjbgzx");
            }

            if (!parameter.ywcm_zcgl_gyzccqjfdc)
            {
                //国有资产产权纠纷调处流程 
                ReplaceBook(doc, "ywcm_zcgl_gyzccqjfdc");
            }

            if (!parameter.ywcm_zcgl_zcpglc)
            {
                //资产评估流程
                ReplaceBook(doc, "ywcm_zcgl_zcpglc");
            }

            if (!parameter.ywcm_jsxm_gkzbyqzb)
            {
                //建设项目公开招标流程、邀请招标流程 
                ReplaceBook(doc, "ywcm_jsxm_gkzbyqzb");
            }

            if (!parameter.ywcm_jsxm_sjbgqsqz)
            {
                //建设项目设计变更、洽商签证流程
                ReplaceBook(doc, "ywcm_jsxm_sjbgqsqz");
            }
        }

        /// <summary>
        /// 是否需要财政专项资金管理办法
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_zxzjgl(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_zxzjgl");
        }

        /// <summary>
        /// 无公务卡
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_gwk(word.Document doc, word.Application app)
        {
            //ReplaceBook(doc, "zdgl_gwk");//取消 标签
            ReplaceBook(doc, "ywcm_szyw_gwk1");
            ReplaceBook(doc, "ywcm_szyw_gwk2");
        }

        /// <summary>
        /// 无机房
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_EngineRoom(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_jifang");
        }

        /// <summary>
        /// 随即获取标题
        /// </summary>
        /// <param name="app"></param>
        private static void setTitleText(word.Application app)
        {
            Dictionary<string, string> datas_sort = new Dictionary<string, string>();
            string[] ary_BT1 = new string[] { "概述", "手册概述", "概论", "总则" };//4
            string[] ary_BT2 = new string[] { "手册编制意义与目的", "编制目的", "编制意义与目的", "内控意义与目的", "内控手册的意义", "编制意义和目的", "内控手册意义" };//7
            string[] ary_BT3 = new string[] { "政策依据", "手册政策依据", "编制依据" };//3
            string[] ary_BT4 = new string[] { "工作目标", "内控工作目标", "实施目标" };//3
            string[] ary_BT5 = new string[] { "基本原则", "编制原则", "内控基本原则", "编制基本原则" };//4
            string[] ary_BT6 = new string[] { "手册框架", "编制框架", "内控手册框架" };//3
            string[] ary_BT7 = new string[] { "编制要素", "内控编制要素", "手册编制要素" };//3
            string[] ary_BT8 = new string[] { "编制说明", "编制过程", "手册编制说明", "手册编制过程", "手册形成过程" };//5
            string[] ary_BT9 = new string[] { "生效日期", "手册生效日期" };//2
            string[] ary_BT10 = new string[] { "适用范围", "使用说明", "手册使用", "手册使用说明" };//4
            string[] ary_BT11 = new string[] { "内部控制环境", "内部环境", "单位内部环境" };//3
            string[] ary_BT12 = new string[] { "单位简介", "单位情况简介" };//2
            string[] ary_BT13 = new string[] { "班子成员信息", "班子成员与分工", "领导班子构成", "领导班子成员和分工", "领导班子" };//5
            string[] ary_BT14 = new string[] { "单位组织结构", "组织结构图", "组织结构", "单位组织结构图" };//4
            string[] ary_BT15 = new string[] { "内部控制过程", "控制过程" };//2
            string[] ary_BT16 = new string[] { "内部控制组织职能", "内控组织职能" };//2
            string[] ary_BT17 = new string[] { "内部控制组织结构图", "内控组织结构图" };//2
            string[] ary_BT18 = new string[] { "概述", "控制措施概述" };//2
            string[] ary_BT19 = new string[] { "政策依据和相关管理制度", "政策依据与管理制度" };//2
            string[] ary_BT20 = new string[] { "信息与沟通", "信息与沟通管理" };//2

            datas_sort.Add("#BT_20", ary_BT20[new Random().Next(2)]);
            datas_sort.Add("#BT_19", ary_BT19[new Random().Next(2)]);
            datas_sort.Add("#BT_18", ary_BT18[new Random().Next(2)]);
            datas_sort.Add("#BT_17", ary_BT17[new Random().Next(2)]);
            datas_sort.Add("#BT_16", ary_BT16[new Random().Next(2)]);
            datas_sort.Add("#BT_15", ary_BT15[new Random().Next(2)]);
            datas_sort.Add("#BT_14", ary_BT14[new Random().Next(4)]);
            datas_sort.Add("#BT_13", ary_BT13[new Random().Next(5)]);
            datas_sort.Add("#BT_12", ary_BT12[new Random().Next(2)]);
            datas_sort.Add("#BT_11", ary_BT11[new Random().Next(3)]);
            datas_sort.Add("#BT_10", ary_BT10[new Random().Next(4)]);
            datas_sort.Add("#BT_9", ary_BT9[new Random().Next(2)]);
            datas_sort.Add("#BT_8", ary_BT8[new Random().Next(5)]);
            datas_sort.Add("#BT_7", ary_BT7[new Random().Next(3)]);
            datas_sort.Add("#BT_6", ary_BT6[new Random().Next(3)]);
            datas_sort.Add("#BT_5", ary_BT5[new Random().Next(4)]);
            datas_sort.Add("#BT_4", ary_BT4[new Random().Next(3)]);
            datas_sort.Add("#BT_3", ary_BT3[new Random().Next(3)]);
            datas_sort.Add("#BT_2", ary_BT2[new Random().Next(7)]);
            datas_sort.Add("#BT_1", ary_BT1[new Random().Next(4)]);
            ReplaceSort(app, datas_sort);
        }

        #region 长内容关键字替换 (只能替换一个)
        private static void ReplaceLong(word.Application app, Dictionary<string, string> datas_long)
        {
            foreach (var item in datas_long)
            {
                app.Selection.Find.ClearFormatting();
                app.Selection.HomeKey(word.WdUnits.wdStory, Missing.Value);
                app.Selection.Find.Text = item.Key;//需要查找的字符

                if (app.Selection.Find.Execute()) //只负责找到匹配字符          
                {
                    app.Selection.TypeText(item.Value);//在找到的字符区域写数据
                }
            }
        }
        #endregion

        #region 短内容关键字替换 (能替换所有关键字)
        private static void ReplaceSort(word.Application app, Dictionary<string, string> datas_sort)
        {
            object oMissing = System.Reflection.Missing.Value;
            object replace = word.WdReplace.wdReplaceAll;

            foreach (var item in datas_sort)
            {
                app.Selection.Find.Replacement.ClearFormatting();
                app.Selection.Find.ClearFormatting();
                app.Selection.HomeKey(word.WdUnits.wdStory, Missing.Value);
                app.Selection.Find.Text = item.Key;//需要被替换的文本
                app.Selection.Find.Replacement.Text = item.Value;//替换文本 

                //执行替换操作
                app.Selection.Find.Execute(
                ref oMissing, ref oMissing,
                ref oMissing, ref oMissing,
                ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref replace,
                ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);
            }
        }
        #endregion

        #region 设置页眉内容
        private static void setPageHeader(word.Application app, string wordhead)
        {
            app.ActiveWindow.ActivePane.View.SeekView = word.WdSeekView.wdSeekCurrentPageHeader;
            app.Selection.WholeStory();
            app.Selection.Font.Color = word.WdColor.wdColorBlack;
            app.Selection.TypeText(wordhead);
            app.ActiveWindow.ActivePane.View.SeekView = word.WdSeekView.wdSeekMainDocument;
        }
        #endregion

        #region 书签设置为空
        /// <summary>
        /// 书签设置为空
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="bookName"></param>
        private static void ReplaceBook(word.Document doc, object bookName)
        {
            word.Range range = doc.Bookmarks.get_Item(ref bookName).Range;
            range.Text = "";
        }
        #endregion

        #region 向 指定书签插入 图片
        private static void InsertImg(word.Application app, word.Document doc, Dictionary<string, string> images)
        {
            foreach (var item in images)
            {
                object bk = item.Key;
                string filename = item.Value;
                if (doc.Bookmarks.Exists(item.Key))
                {
                    object range = doc.Bookmarks.get_Item(ref bk).Range;
                    //定义该图片是否为外部链接
                    object linkToFile = false;//默认
                    //定义插入的图片是否随word一起保存
                    object saveWithDocument = true;
                    //向word中写入图片
                    doc.InlineShapes.AddPicture(filename, ref linkToFile, ref saveWithDocument, ref range);

                    object unite = Microsoft.Office.Interop.Word.WdUnits.wdStory;
                    app.Selection.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;//居中显示图片
                }
            }
        }
        #endregion

        /// <summary>
        /// 是否报账制度
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="isBZ"></param>
        private static void IsBZorKJ(word.Document doc, word.Application app, bool isBZ)
        {
            if (isBZ)
            {
                //ReplaceBook(doc, "zdgl_kjz1");
                ReplaceBook(doc, "zdgl_kjz2");
            }
            else
            {
                ReplaceBook(doc, "zdgl_bzz1");
            }
        }

        /// <summary>
        /// 无借款业务
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_jkgl(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "jkgl_1");
            ReplaceBook(doc, "jkgl_2");
            ReplaceBook(doc, "jkgl_3");
            ReplaceBook(doc, "jkgl_4");

            Dictionary<string, string> datas_dz = new Dictionary<string, string>();
            datas_dz.Add("支出事前申请、借款或报销", "支出事前申请、报销");
            datas_dz.Add("范围，明确支出借款和报销业务流程", "范围，明确支出和报销业务流程");
            datas_dz.Add("在支付控制方面应当重点加强以下三个环节的控制", "在支付控制方面应当重点加强以下两个环节的控制");
            datas_dz.Add("凭领导签批的“借款审批单”", "");
            datas_dz.Add("现金借款凭据，必须附在记账凭证之后。收回借款时，应当另开收据，不得退还原借款凭据。\r", "");

            ReplaceSort(app, datas_dz);
        }

        /// <summary>
        /// 无差旅费制度
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_clf(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_clf1");
            ReplaceBook(doc, "zdgl_clf2");
            ReplaceBook(doc, "zdgl_clf3");
            ReplaceBook(doc, "zdgl_clf4");
        }

        /// <summary>
        /// 无会议费制度
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_hyf(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_hyf1");
            ReplaceBook(doc, "zdgl_hyf2");
            ReplaceBook(doc, "zdgl_hyf3");
        }

        /// <summary>
        /// 无培训费制度
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_pxf(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_pxf1");
            ReplaceBook(doc, "zdgl_pxf2");
            ReplaceBook(doc, "zdgl_pxf3");
        }

        /// <summary>
        /// 无招待费制度
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_zdf(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_zdf1");
        }

        /// <summary>
        /// 无合同管理
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_htgl(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "htgl_1");
            ReplaceBook(doc, "htgl_2");
            ReplaceBook(doc, "htgl_3");
            ReplaceBook(doc, "htgl_4");
            ReplaceBook(doc, "htgl_5");

            //构造数据
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("合同业务管理风险。\r", "");
            datas.Add("合同管理业务；", "");
            datas.Add("、合同业务控制", "");
            datas.Add("和合同管理", "");
            datas.Add("、合同管理", "");
            datas.Add("、合同", "");
            ReplaceSort(app, datas);
        }

        /// <summary>
        /// 无收费业务
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_sfyw(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "sfgl_1");
            ReplaceBook(doc, "sfgl_2");
        }

        /// <summary>
        /// 无建设项目
        /// </summary>
        /// <param name="doc"></param>
        private static void None_jsxm(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "jsxm_1");
            ReplaceBook(doc, "jsxm_2");
            ReplaceBook(doc, "jsxm_3");

            //构造数据
            Dictionary<string, string> datas = new Dictionary<string, string>();
            datas.Add("建设项目由建设单位办理款项支付手续，", "");
            datas.Add("建设项目业务管理风险。\r", "");
            datas.Add("建设项目管理业务；", "");
            datas.Add("、建设项目管理", "");
            datas.Add("、建设项目控制", "");
            datas.Add("、建设项目", "");

            ReplaceSort(app, datas);
        }

        /// <summary>
        /// 无采购合同申请、无单位自行采购申请
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="iscgsq">无采购合同审批</param>
        /// <param name="iszxcgsq">无单位自行采购审批</param>
        private static void None_zfcghtsq(word.Document doc, word.Application app, bool iscgsq, bool iszxcgsq)
        {
            if (iszxcgsq)
            {
                ReplaceBook(doc, "zfcgyw_9");//ywcm_zfcg_dwzxcg
            }
            if (iscgsq)
            {
                ReplaceBook(doc, "zfcgyw_10");//ywcm_zfcg_cghtgl
            }
        }

        /// <summary>
        /// 无 政府采购
        /// </summary>
        /// <param name="doc"></param>
        private static void None_zfcg(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zfcgyw_1");//ywcm_zfcg_zfcgyw
            ReplaceBook(doc, "zfcgyw_2");//ywcm_zfcg_ldxz
            ReplaceBook(doc, "zfcgyw_3");//ywcm_zfcg_ldxzjgt
            ReplaceBook(doc, "zfcgyw_4");//ywcm_zfcg_text1
            ReplaceBook(doc, "zfcgyw_5");//ywcm_zfcg_text2
            ReplaceBook(doc, "zfcgyw_6");//ywcm_zfcg_zfcg
            ReplaceBook(doc, "zfcgyw_7");//ywcm_zfcg_text3
            ReplaceBook(doc, "zfcgyw_8");//ywcm_zfcg_table1

            //构造数据
            Dictionary<string, string> datas = new Dictionary<string, string>();

            datas.Add("对建设工程、大型修缮、信息化项目及大宗物资采购等重大事项，应按要求对业务事项的目的、方案的可行性、计划的科学性及金额的合理性等方面进行综合立项评审。", "");
            datas.Add("重点通报建设项目、大宗物资采购、对外投资等重大预算项目执行情况。", "");
            datas.Add("8.是否严格执行集中采购管理规定，对大额采购行为进行控制。", "");
            datas.Add(";属政府集中采购的，按规定参加政府采购，办理资金结算", "");
            datas.Add("大宗物资采购、贵重设备购置等情况。", "");
            datas.Add("，参与采购活动，并负责组织验收", "");
            datas.Add("政府采购业务管理风险。\r", "");

            datas.Add("、政府采购业务控制", "");

            datas.Add("、采购与验收管理", "");
            datas.Add("、大宗采购行为", "");
            datas.Add("、大宗物品采购", "");
            datas.Add("、采购及验收", "");
            datas.Add("业务采购、", "");
            datas.Add("、采购管理", "");
            datas.Add("采购、", "");

            ReplaceSort(app, datas);
        }

        /// <summary>
        /// 无党务管理
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_dwgl(word.Document doc, word.Application app, string dzjg)
        {
            if (dzjg == "无")
            {
                ReplaceBook(doc, "dwgl_1");
                ReplaceBook(doc, "dwgl_2");
                ReplaceBook(doc, "dwgl_3");

                Dictionary<string, string> datas_dz = new Dictionary<string, string>();
                datas_dz.Add("#DZZJGMC认为需要交流轮岗的岗位和对象。", "单位领导认为需要交流轮岗的岗位和对象。");
                datas_dz.Add("#DZZJGMC成员为成员", "科室成员为成员");
                datas_dz.Add("#DZZJGMC会，#DZZJGMC扩大会，", "");
                datas_dz.Add("#DZZJGMC书记/", "");
                datas_dz.Add("#DZZJGMC办公室、", "");
                datas_dz.Add("和#DZZJGMC会议", "");
                datas_dz.Add("或#DZZJGMC会议", "");
                datas_dz.Add("或#DZZJGMC会", "");
                datas_dz.Add("#DZZJGMC会/", "");
                datas_dz.Add("#DZZJGMC书记、", "");
                datas_dz.Add("#DZZJGMC成员、", "");
                datas_dz.Add("#DZZJGMC、", "");
                datas_dz.Add("#DZZJGMC", "#zzzwmc办公会");
                ReplaceSort(app, datas_dz);
            }
            else if (dzjg == "党委")
            {
                ReplaceBook(doc, "dwgl_dzb");
            }
            else if (dzjg == "党支部")
            {
                ReplaceBook(doc, "dwgl_dw");
            }
            else if (dzjg == "党组")
            {
                ReplaceBook(doc, "dwgl_dzb");
                ReplaceBook(doc, "dwgl_dw");
            }
        }

        /// <summary>
        /// 无债务业务
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_zwyw(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_zwyw1");
            ReplaceBook(doc, "zdgl_zwyw2");
        }

        /// <summary>
        /// 无对外投资业务
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        private static void None_dwtzyw(word.Document doc, word.Application app)
        {
            ReplaceBook(doc, "zdgl_dwtzyw");
        }
        #endregion

        #region 生成问题反馈文件
        public static string CreateWord(DataTable data, string filepath, string imgpath)
        {
            word.Application app = null;
            word.Document doc = null;
            try
            {
                object filename = filepath;
                object oMissing = System.Reflection.Missing.Value;
                //创建Word文档
                app = new word.Application();//创建word应用程序
                doc = app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                word.Table newTable = doc.Tables.Add(app.Selection.Range, data.Rows.Count * 4, 2, ref oMissing, ref oMissing);
                newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;// wdLineStyleThickThinLargeGap;
                newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                newTable.Columns[1].Width = 100f;
                newTable.Columns[2].Width = 320f;

                int index = 0;
                foreach (DataRow item in data.Rows)
                {
                    newTable.Cell(index * 4 + 1, 1).Range.Text = "问题" + (index + 1) + "所在页码";
                    newTable.Cell(index * 4 + 1, 2).Range.Text = item["wtpage"].ToString();

                    newTable.Cell(index * 4 + 2, 1).Range.Text = "问题" + (index + 1) + "原内容";
                    newTable.Cell(index * 4 + 2, 2).Range.Text = item["BeforText"].ToString();

                    newTable.Cell(index * 4 + 3, 1).Range.Text = "问题" + (index + 1) + "修复后内容";
                    newTable.Cell(index * 4 + 3, 2).Range.Text = item["wtcontent"].ToString();

                    newTable.Cell(index * 4 + 4, 1).Range.Text = "问题" + (index + 1) + "图片";
                    if (item["wtimg"].ToString() != "up.jpg")
                    {
                        //插入图片
                        string FileName = imgpath + @"\" + item["wtimg"];//图片所在路径
                        object LinkToFile = false;
                        object SaveWithDocument = true;
                        object Anchor = newTable.Cell(index * 4 + 4, 2).Range;
                        doc.Application.ActiveDocument.InlineShapes.AddPicture(FileName, ref LinkToFile, ref SaveWithDocument, ref Anchor);
                    }
                    else
                    {
                        newTable.Cell(index * 4 + 4, 2).Range.Text = "无";
                    }
                    index++;
                }

                doc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                return filepath;
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
            finally
            {
                object oMissing = System.Reflection.Missing.Value;
                if (doc != null)
                {
                    doc.Close(ref oMissing, ref oMissing, ref oMissing);//关闭word文档
                }
                if (app != null)
                {
                    app.Quit(ref oMissing, ref oMissing);//退出word应用程序
                }
            }
        }
        #endregion
    }
}

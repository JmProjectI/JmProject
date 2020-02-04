using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Word
{
    public class ConstData_QYXY
    {

        public string Get_QY_Titile()
        {
            string[] ary_xyqy = new string[] { "序  言", "前  言" };
            return ary_xyqy[new Random().Next(2)];
        }

        public string Get_QY_Text()
        {
            string xyqyDesc = "DWQC，为进一步提高内部管理水平，规范权利运行机制和单位内部控制，加强廉政风险防控机制的建设，"
                    + "全面贯彻落实财政部颁布的《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》精神，"
                    + "结合本单位工作实际情况组织编制《DWQC内部控制手册》,本册作为建立、执行、评价单位内部控制的依据。"
                    + "确保单位从思想上提高管理水平，增强风险防控意识和能力，保证单位协调、持续、快速发展。"
                    + "&p本册的编制实施对完善单位内部控制体系，进一步规范内部管理和业务流程，有效防控单位风险，保证财务信息真实完整，维护单位财产安全与高效运行具有重要意义。"
                    + "&p本册在编制过程中遵循全面性、重要性、制衡性、适应性、有效性的基本原则，结合实际工作，通过梳理业务流程"
                    + "，拟定风险控制措施，为建立本单位内部控制体系奠定坚实基础。"
                    + "本册是本单位工作必备工具，本册在试运行期间，将根据实际执行情况和发现的问题进行不断完善，敬请批评指正。";

            string xyqyDesc1 = "为了贯彻落实财政部颁布的《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》"
                        + "等文件精神，进一步强化我单位内部管理工作，结合我单位实际，我们组织汇编了《DWQC内部控制手册》，作为建立、执行、评价本单位内部控制的依据。"
                        + "&p在手册的编制过程中，我单位领导干部深入调研分析，充分沟通交流，通过对各科室的业务风险管理程序、制度、岗位职责的认真梳理、调整，"
                        + "使制度与流程的优化工作更加具有了广泛的群众基础，极大地提高了广大干部的业务能力和风险防范意识，发挥了内控体系建设反腐倡廉、防范风险的作用。"
                        + "&p随着内部控制政策的不断推进，我们要在实际工作中定期对手册进行更新，不断完善业务风险管理程序，全面推进单位内部管理向科学化、精细化迈进。"
                        + "&p&p&p&pDWQC&pkey_bfrq";

            string xyqyDesc2 = "为提高DWQC内部管理水平，建立权利运行和权力监督机制，增强单位内部控制，加强廉政风险防控机制的建设，全面贯彻落实财政部颁布的"
                        + "《行政事业单位内部控制规范（试行）》和吉林省财政厅《行政事业单位内部控制规范基本指引》精神，结合DWQC工作实际情况，组织编制《DWQC内部控制手册》。"
                        + "&p本单位内部控制手册作为建立、执行、评价单位内部控制的依据。确保单位从思想上提高管理水平，增强风险防控意识和能力，保证单位协调、持续、快速发展。"
                        + "&p本单位内部控制手册的编制实施对完善单位内部控制体系，进一步规范内部管理及业务流程，有效防控单位风险，保证财务信息真实完整，"
                        + "维护单位财产安全与高效运行等方面具有极其重要意义。"
                        + "&p本单位内部控制手册在编制过程中，应当遵循全面性、有效性、适应性、重要性、制衡性五大基本原则，通过梳理业务流程，结合工作实际"
                        + "，拟定风险控制措施，为建立本单位内部控制体系奠定坚实基础。"
                        + "&p本手册是本单位开展工作的必备工具，本手册在试运行期间，将根据实际执行情况和发现的问题进行不断完善，敬请各级领导批评指正。";

            string xyqyDesc3 = "为了提升DWQC内部管理水平，实现治理体系现代化，规范DWQC权利运行机制，加强廉政风险防控建设，贯彻落实党的十八届四中全会通过的"
                        + "《中共中央关于全面推进依法治国若干重大问题的决定》明确提出：“对财政资金分配使用、国有资产监管、政府投资、政府采购、公共资源转让、"
                        + "公共工程建设等权力集中的部门和岗位实行分事行权、分岗设权、分级授权，定期轮岗，强化内部流程控制，防止权力滥用”的指示精神，依据财政部颁布的"
                        + "《行政事业单位内部控制规范（试行）》和《关于全面推进行政事业单位内部控制建设的指导意见》，以及吉林省财政厅《行政事业单位内部控制规范基本指引》"
                        + "指导思想，结合本单位工作实际情况组织编制《DWQC内部控制手册》,本手册是建立、执行、评价本单位内部控制的重要依据。"
                        + "&pDWQC内部控制手册严格遵循《行政事业单位内部控制规范》的全面性、重要性、制衡性、适应性、有效性的基本原则，梳理业务流程，"
                        + "找出风险点并拟定风险控制措施，结合实际，为建立与实施DWQC内部控制奠定坚实基础。手册是本单位工作必备工具书，本手册试运行期间，"
                        + "根据实际执行情况和发现的问题不断完善，敬请批评指正。";

            string xyqyDesc4 = "为贯彻落实党的十八届四中全会通过的《中共中央关于全面推进依法治国若干重大问题的决定》的重要指示精神，提升DWQC内部管理水平，"
                        + "实现治理能力现代化，规范本单位权利运行与权力监督工作机制，建设廉政风险防控制度，对本单位重要领域和关键岗位实行分事行权、分岗设权、"
                        + "分级授权，关键岗位定期轮岗，强化内部流程控制，防止权力滥用，并根据财政部颁布的《行政事业单位内部控制规范（试行）》和"
                        + "《关于全面推进行政事业单位内部控制建设的指导意见》，结合本单位实际情况组织编制《DWQC内部控制手册》,本手册是建立、执行、评价本单位内部控制的重要依据。"
                        + "&p本手册严格遵循《内部控制规范》的五项基本原则和八个控制方法，开展业务流程梳理，找出风险点并拟定风险控制措施，结合实际，"
                        + "为建立与实施DWQC内部控制奠定坚实基础。手册是本单位工作必备工具书，本手册试运行期间，根据实际执行情况和发现的问题不断完善，敬请批评指正。";

            string xyqyDesc5 = "为贯彻落实党的十八届四中全会提出的“全面推进依法治国”指示精神，加强“对财政资金分配使用、国有资产监管、政府投资、政府采购、公共资源转让"
                        + "、公共工程建设等权力集中的部门和岗位实行分事行权、分岗设权、分级授权，定期轮岗，强化内部流程控制，防止权力滥用”的管理控制，"
                        + "我单位依据2012年财政部引发的《行政事业单位内部控制规范（试行）》和2015年财政部引发的《关于全面推进行政事业单位内部控制建设的指导意见》文件要求，"
                        + "结合本单位实际，组织汇编了《DWQC内部控制手册》。"
                        + "&p《内部控制手册》将成为DWQC日常工作指南和基础管理制度，本手册将严格遵循《行政事业单位内部控制规范》的全面性、重要性、制衡性、适应性、"
                        + "有效性的基本原则，规范权力运行机制和单位内部控制，加强廉政建设，通过对各经济业务的梳理，分析风险并制定风险控制办法和目标，"
                        + "明确重大领域和关键岗位的工作机制、授权范围、权力职责。"
                        + "&p通过本手册的编制，完善单位内部控制体系建设与执行，进一步规范DWQC内部管理和业务流程，有效防控单位经济业务活动风险，确保本单位财产安全和使用有效，"
                        + "保证本单位财务信息真实完整，提高本单位服务效率效能具有重要意义。";
            string[] ary_xyqydesc = new string[] { xyqyDesc, xyqyDesc1, xyqyDesc2, xyqyDesc3, xyqyDesc4, xyqyDesc5 };
            return ary_xyqydesc[new Random().Next(6)];
        }

        public string Get_BT1()
        {
            string[] ary_BT1 = new string[] { "概述", "手册概述", "概论", "总则" };//4
            return ary_BT1[new Random().Next(4)];
        }

        public string Get_BT2()
        {
            string[] ary_BT2 = new string[] { "手册编制意义与目的", "编制目的", "编制意义与目的", "内控意义与目的", "内控手册的意义", "编制意义和目的", "内控手册意义" };//7
            return ary_BT2[new Random().Next(7)];
        }

        public string Get_BT3()
        {
            string[] ary_BT3 = new string[] { "政策依据", "手册政策依据", "编制依据" };//3
            return ary_BT3[new Random().Next(3)];
        }

        public string Get_BT4()
        {
            string[] ary_BT4 = new string[] { "工作目标", "内控工作目标", "实施目标" };//3
            return ary_BT4[new Random().Next(3)];
        }

        public string Get_BT5()
        {
            string[] ary_BT5 = new string[] { "基本原则", "编制原则", "内控基本原则", "编制基本原则" };//4
            return ary_BT5[new Random().Next(4)];
        }

        public string Get_BT6()
        {
            string[] ary_BT6 = new string[] { "手册框架", "编制框架", "内控手册框架" };//3
            return ary_BT6[new Random().Next(3)];
        }

        public string Get_BT7()
        {
            string[] ary_BT7 = new string[] { "编制要素", "内控编制要素", "手册编制要素" };//3
            return ary_BT7[new Random().Next(3)];
        }

        public string Get_BT8()
        {
            string[] ary_BT8 = new string[] { "编制说明", "编制过程", "手册编制说明", "手册编制过程", "手册形成过程" };//5
            return ary_BT8[new Random().Next(5)];
        }

        public string Get_BT9()
        {
            string[] ary_BT9 = new string[] { "生效日期", "手册生效日期" };//2
            return ary_BT9[new Random().Next(2)];
        }

        public string Get_BT10()
        {
            string[] ary_BT10 = new string[] { "适用范围", "使用说明", "手册使用", "手册使用说明" };//4
            return ary_BT10[new Random().Next(4)];
        }

        public string Get_BT11()
        {
            string[] ary_BT11 = new string[] { "内部控制环境", "内部环境", "单位内部环境" };//3
            return ary_BT11[new Random().Next(3)];
        }

        public string Get_BT12()
        {
            string[] ary_BT12 = new string[] { "单位简介", "单位情况简介" };//2
            return ary_BT12[new Random().Next(2)];
        }

        public string Get_BT13()
        {
            string[] ary_BT13 = new string[] { "班子成员信息", "班子成员与分工", "领导班子构成", "领导班子成员和分工", "领导班子" };//5
            return ary_BT13[new Random().Next(5)];
        }

        public string Get_BT14()
        {
            string[] ary_BT14 = new string[] { "单位组织结构", "组织结构图", "组织结构", "单位组织结构图" };//4
            return ary_BT14[new Random().Next(4)];
        }

        public string Get_BT15()
        {
            string[] ary_BT15 = new string[] { "内部控制过程", "控制过程" };//2
            return ary_BT15[new Random().Next(2)];
        }

        public string Get_BT16()
        {
            string[] ary_BT16 = new string[] { "内部控制组织职能", "内控组织职能" };//2
            return ary_BT16[new Random().Next(2)];
        }

        public string Get_BT17()
        {
            string[] ary_BT17 = new string[] { "内部控制组织结构图", "内控组织结构图" };//2
            return ary_BT17[new Random().Next(2)];
        }

        public string Get_BT18()
        {
            string[] ary_BT18 = new string[] { "概述", "控制措施概述" };//2
            return ary_BT18[new Random().Next(2)];
        }

        public string Get_BT19()
        {
            string[] ary_BT19 = new string[] { "政策依据和相关管理制度", "政策依据与管理制度" };//2
            return ary_BT19[new Random().Next(2)];
        }

        public string Get_BT20()
        {
            string[] ary_BT20 = new string[] { "信息与沟通", "信息与沟通管理" };//2
            return ary_BT20[new Random().Next(2)];
        }

    }
}

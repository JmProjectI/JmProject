using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Word
{
    public class ConstData
    {
        public Dictionary<string, string> GetKeywordCS(string FileName)
        {
            Dictionary<string, string> Keywords = new Dictionary<string, string>();
            if (FileName.IndexOf("前言") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("key_XYQY", qyxy.Get_QY_Titile());
                Keywords.Add("key_XY_SJ", qyxy.Get_QY_Text());
            }
            else if (FileName.IndexOf("概述") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("key_BT_1", qyxy.Get_BT1());
                Keywords.Add("key_BT_2", qyxy.Get_BT2());
                Keywords.Add("key_BT_3", qyxy.Get_BT3());
                Keywords.Add("key_BT_4", qyxy.Get_BT4());
                Keywords.Add("key_BT_5", qyxy.Get_BT5());
                Keywords.Add("key_BT_6", qyxy.Get_BT6());
                Keywords.Add("key_BT_7", qyxy.Get_BT7());
                Keywords.Add("key_BT_8", qyxy.Get_BT8());
                Keywords.Add("key_BT_9", qyxy.Get_BT9());
                Keywords.Add("key_BT_10", qyxy.Get_BT10());
            }
            else if (FileName.IndexOf("内部控制环境") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("key_BT_11", qyxy.Get_BT11());
                Keywords.Add("key_BT_12", qyxy.Get_BT12());
                Keywords.Add("key_BT_13", qyxy.Get_BT13());
                Keywords.Add("key_BT_14", qyxy.Get_BT14());
                Keywords.Add("key_BT_15", qyxy.Get_BT15());
                Keywords.Add("key_BT_16", qyxy.Get_BT16());
                Keywords.Add("key_BT_17", qyxy.Get_BT17());
            }
            else if (FileName.IndexOf("控制措施") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("key_BT_18", qyxy.Get_BT18());
                Keywords.Add("key_BT_19", qyxy.Get_BT19());
            }
            else if (FileName.IndexOf("信息与沟通") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("key_BT_20", qyxy.Get_BT20());
            }
            return Keywords;
        }

        public Dictionary<string, string> GetKeywordDB(string FileName)
        {
            Dictionary<string, string> Keywords = new Dictionary<string, string>();
            if (FileName.IndexOf("封面") >= 0)
            {
                Keywords.Add("DWQC", "DWQC");
                Keywords.Add("key_bfrq", "bfrq");
            }
            else if (FileName.IndexOf("前言") >= 0)
            {
                ConstData_QYXY qyxy = new ConstData_QYXY();
                Keywords.Add("DWQC", "");
                Keywords.Add("key_bfrq", "");
            }
            #region 单位层面
            else if (FileName.IndexOf("概述") >= 0)
            {
                Keywords.Add("DWQC", "DWQC");
                Keywords.Add("key_zzzwmc", "zzzwmc");
                Keywords.Add("key_scqtbm", "scqtbm");
                Keywords.Add("key_scxzbm", "scxzbm");
                Keywords.Add("key_bfrq", "bfrq");
                Keywords.Add("key_syfw0415", "syfw0415");
            }
            else if (FileName.IndexOf("内部控制环境") >= 0)
            {
                //关键字
                Keywords.Add("DWQC", "DWQC");
                Keywords.Add("key_zzzwmc", "zzzwmc");
                Keywords.Add("key_dwjj", "dwjj");
                Keywords.Add("key_GJZ1", "GJZ1");
                Keywords.Add("key_dzcylist", "dzcylist");
                Keywords.Add("key_nkldxzzz", "nkldxzzz");
                Keywords.Add("key_nkldxzfzz", "nkldxzfzz");
                Keywords.Add("key_nkldxzcy", "nkldxzcy");
                Keywords.Add("key_nbkzgzxzzz01", "nbkzgzxzzz01");
                Keywords.Add("key_nbkzgzxzfzz01", "nbkzgzxzfzz01");
                Keywords.Add("key_nbkzgzxzzzcy01", "nbkzgzxzzzcy01");
                Keywords.Add("key_nbkzgzxzzzqt01", "nbkzgzxzzzqt01");
                Keywords.Add("key_fxpgxzzz", "fxpgxzzz");
                Keywords.Add("key_fxpgxzfzz", "fxpgxzfzz");
                Keywords.Add("key_fxpgxzcy", "fxpgxzcy");
                Keywords.Add("key_fxpgxzqtks", "fxpgxzqtks");
                Keywords.Add("key_ysldxzzz", "ysldxzzz");
                Keywords.Add("key_ysldxzfzz", "ysldxzfzz");
                Keywords.Add("key_ysldxzcy", "ysldxzcy");
                Keywords.Add("key_ysldxzqdks", "ysldxzqdks");
                Keywords.Add("key_zfcgxzzz", "zfcgxzzz");
                Keywords.Add("key_zfcgxzfzz", "zfcgxzfzz");
                Keywords.Add("key_zfcgxzcy", "zfcgxzcy");
                Keywords.Add("key_zfcgxzqdks", "zfcgxzqdks");
                Keywords.Add("key_gyzcxzzz", "gyzcxzzz");
                Keywords.Add("key_gyzcxzfzz", "gyzcxzfzz");
                Keywords.Add("key_gyzcxzcy", "gyzcxzcy");
                Keywords.Add("key_gyzcxzqdks", "gyzcxzqdks");
                Keywords.Add("key_jdjcxzzz", "jdjcxzzz");
                Keywords.Add("key_jdjcxzfzz", "jdjcxzfzz");
                Keywords.Add("key_jdjcxzcy", "jdjcxzcy");
                Keywords.Add("key_jdjcxzqdks", "jdjcxzqdks");
                Keywords.Add("key_nbsjks", "nbsjks");
                //书签
                //model.BookmarkerImage.Add("img_zzjg_dwzzjg", imgPath + "dwzzjg.jpg");
                //model.BookmarkerImage.Add("img_zzjg_fxpgxz", imgPath + "fxpgxz.jpg");
                //model.BookmarkerImage.Add("img_zzjg_gyzcxz", imgPath + "gyzcxz.jpg");
                //model.BookmarkerImage.Add("img_dwnbkzzzjgt", imgPath + "img_dwnbkzzzjgt.jpg");
                //model.BookmarkerImage.Add("img_zzjg_jdjcxz", imgPath + "jdjcxz.jpg");
                //model.BookmarkerImage.Add("img_zzjg_nkgzxz", imgPath + "nkgzxz.jpg");
                //model.BookmarkerImage.Add("img_zzjg_nkldxz", imgPath + "nkldxz.jpg");
                //model.BookmarkerImage.Add("img_zzjg_ysldxz", imgPath + "ysldxz.jpg");
                //model.BookmarkerImage.Add("img_zzjg_zfcgxz", imgPath + "zfcgxz.jpg");

                //model.BookmarkerTextRang.Add("zfcgyw_2", "");
                //model.BookmarkerTextRang.Add("zfcgyw_3", "");
            }
            else if (FileName.IndexOf("风险管理") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");

                //model.BookmarkerImage.Add("img_fxpggzlc", "");
            }
            else if (FileName.IndexOf("控制措施") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_DZZJGMC", "党组织机构名称");
                Keywords.Add("key_zdjcsshpjks", "审核评价科室");
                Keywords.Add("key_syfw0415", "syfw0415未知");
                Keywords.Add("key_bxrgwzdks", "bxrgwzdks未知");
                Keywords.Add("key_nzkhgkks", "nzkhgkks未知");
                Keywords.Add("key_bzndlgjhks", "bzndlgjhks未知");
                Keywords.Add("key_bnlgdgwmc", "bnlgdgwmc未知");

                //model.BookmarkerImage.Add("img_jtysjclc", imgPath + "img_jtysjclc.jpg");

                //model.BookmarkerTextRang.Add("htgl_2", "");
                //model.BookmarkerTextRang.Add("jsxm_2", "");
                //model.BookmarkerTextRang.Add("zdgl_zwyw2", "");
                //model.BookmarkerTextRang.Add("zfcgyw_4", "");
                //model.BookmarkerTextRang.Add("zfcgyw_5", "");
            }
            else if (FileName.IndexOf("信息与沟通") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_zdbgdgkks", "zdbgdgkks未知");
                Keywords.Add("key_xxgkzrjjks", "xxgkzrjjks未知");
                Keywords.Add("key_zdbgcdks", "zdbgcdks未知");

                //model.BookmarkerImage.Add("img_nkbgsplc", "");
            }
            else if (FileName.IndexOf("监督与评价") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_jdjcxzqdks", "jdjcxzqdks未知");
            }
            else if (FileName.IndexOf("信息管理系统") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_fzxxglxtks", "正职职务名称");

                //model.BookmarkerTextRang.Add("zdgl_jifang", "");
            }
            else if (FileName.IndexOf("单位科室职能与岗位职责") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_gwznlist", "科室岗位职责...");

                //model.BookmarkerTextRang.Add("zdgl_jifang", "");
            }
            else if (FileName.IndexOf("单位控制相关管理制度") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_nzkhgkks", "nzkhgkks未知");
                Keywords.Add("key_DZZJGMC", "DZZJGMC未知");
                Keywords.Add("key_lzjmytgkks", "lzjmytgkks未知");
                Keywords.Add("key_jdjcxzqdks", "jdjcxzqdks未知");
                Keywords.Add("key_GJZ2", "GJZ2未知");
                Keywords.Add("key_zwxxgkks", "zwxxgkks未知");
                Keywords.Add("key_xxxcgzqtks", "xxxcgzqtks未知");
                Keywords.Add("key_GJZ3", "GJZ3未知");
                Keywords.Add("key_ksglks", "ksglks未知");
                Keywords.Add("key_kqsjxxxx", "kqsjxxxx未知");
                Keywords.Add("key_GJZ4", "GJZ4未知");
                Keywords.Add("key_GJZ7", "GJZ7未知");
                Keywords.Add("key_gwglgkks", "gwglgkks未知");
                Keywords.Add("key_ywslzdmc", "ywslzdmc未知");
                Keywords.Add("key_nbkzgzxzzzqt01", "nbkzgzxzzzqt01未知");
                Keywords.Add("key_rsglzdgkks", "rsglzdgkks未知");
                Keywords.Add("key_rsglhbks", "rsglhbks未知");
                Keywords.Add("key_zjzfspqx0419", "zjzfspqx0419未知");
                Keywords.Add("key_czzxzjgkks", "czzxzjgkks未知");

                //model.BookmarkerImage.Add("img_daglywlc", imgPath + "img_daglywlc.jpg");

                //model.BookmarkerTextRang.Add("dwgl_1", "");
                //model.BookmarkerTextRang.Add("dwgl_2", "");
                //model.BookmarkerTextRang.Add("dwgl_3", "");
                //model.BookmarkerTextRang.Add("dwgl_dw", "");
                //model.BookmarkerTextRang.Add("dwgl_dzb", "");
                //model.BookmarkerTextRang.Add("htgl_5", "");
                //model.BookmarkerTextRang.Add("jkgl_1", "");
                //model.BookmarkerTextRang.Add("jsxm_3", "");
                //model.BookmarkerTextRang.Add("zdgl_bzz1", "");
                //model.BookmarkerTextRang.Add("zdgl_clf1", "");
                //model.BookmarkerTextRang.Add("zdgl_clf2", "");
                //model.BookmarkerTextRang.Add("zdgl_hyf1", "");
                //model.BookmarkerTextRang.Add("zdgl_hyf2", "");
                //model.BookmarkerTextRang.Add("zdgl_pxf1", "");
                //model.BookmarkerTextRang.Add("zdgl_pxf2", "");
                //model.BookmarkerTextRang.Add("zdgl_zxzjgl", "");
                //model.BookmarkerTextRang.Add("zfcgyw_1", "");
                //model.BookmarkerTextRang.Add("zfcgyw_7", "");
            }
            #endregion
            #region 业务层面
            else if (FileName.IndexOf("预算业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");

                //model.BookmarkerImage.Add("img_juesywlc", imgPath + "img_juesywlc.jpg");
                //model.BookmarkerImage.Add("img_ysbzspzxywlc", imgPath + "img_ysbzspzxywlc.jpg");
                //model.BookmarkerImage.Add("img_ystzywlc", imgPath + "img_ystzywlc.jpg");
                //model.BookmarkerImage.Add("img_ysywjxpjlc", imgPath + "img_ysywjxpjlc.jpg");
                //model.BookmarkerImage.Add("img_ysywsnghlct", imgPath + "img_ysywsnghlct.jpg");
                //model.BookmarkerImage.Add("img_ysywzxfxlct", imgPath + "img_ysywzxfxlct.jpg");
                //model.BookmarkerImage.Add("img_ysywnbysfjsplc", imgPath + "img_ysywnbysfjsplc.jpg");//新加入

                //model.BookmarkerTextRang.Add("ywcm_ysyw", "");
            }
            else if (FileName.IndexOf("收入、支出业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_bdwsrbk", "bdwsrbk");
                Keywords.Add("key_srywgkks", "srywgkks");
                Keywords.Add("key_jfzcgkks", "jfzcgkks");
                Keywords.Add("key_zjzfsp0419", "zjzfsp0419");
                Keywords.Add("key_jkyw0419", "jkyw0419");
                Keywords.Add("key_gwkglks", "gwkglks");
                Keywords.Add("key_gwkjdks", "gwkjdks");
                Keywords.Add("key_bxyw0419", "bxyw0419");

                //model.BookmarkerImage.Add("img_jkywlc", imgPath + "img_jkywlc.jpg");
                //model.BookmarkerImage.Add("img_szywczpjhxlc", imgPath + "img_szywczpjhxlc.jpg");
                //model.BookmarkerImage.Add("img_szywczsqzfsqlc", imgPath + "img_szywczsqzfsqlc.jpg");
                //model.BookmarkerImage.Add("img_szywczzjjfsqlc", imgPath + "img_szywczzjjfsqlc.jpg");
                //model.BookmarkerImage.Add("img_szywfssrsqlc", imgPath + "img_szywfssrsqlc.jpg");
                //model.BookmarkerImage.Add("img_szywgwkbxhklc", imgPath + "img_szywgwkbxhklc.jpg");
                //model.BookmarkerImage.Add("img_szywgwkslzxlc", imgPath + "img_szywgwkslzxlc.jpg");
                //model.BookmarkerImage.Add("img_szywkjdajlbgjyxhlc", imgPath + "img_szywkjdajlbgjyxhlc.jpg");
                //model.BookmarkerImage.Add("img_szywsrdjrzlc", imgPath + "img_szywsrdjrzlc.jpg");
                //model.BookmarkerImage.Add("img_szywsrzcfxbgsplc", imgPath + "img_szywsrzcfxbgsplc.jpg");
                //model.BookmarkerImage.Add("img_szywyhzhbgcxsqlc", imgPath + "img_szywyhzhbgcxsqlc.jpg");
                //model.BookmarkerImage.Add("img_szywyhzhklsqsplc", imgPath + "img_szywyhzhklsqsplc.jpg");
                //model.BookmarkerImage.Add("img_szywykjhsqsplc", imgPath + "img_szywykjhsqsplc.jpg");
                //model.BookmarkerImage.Add("img_szywzcsqsqsplc", imgPath + "img_szywzcsqsqsplc.jpg");
                //model.BookmarkerImage.Add("img_szywyssrywlc", imgPath + "img_szywyssrywlc.jpg");//新加入
                //model.BookmarkerImage.Add("img_szywczbksrywkzlc", imgPath + "img_szywczbksrywkzlc.jpg");//新加入
                //model.BookmarkerImage.Add("img_szywczpjsqlqywlc", imgPath + "img_szywczpjsqlqywlc.jpg");//新加入

                //model.BookmarkerTextRang.Add("jkgl_2", "");
                //model.BookmarkerTextRang.Add("jkgl_4", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czpjgl1", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czpjgl2", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czpjgl3", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czsqzf1", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czsqzf2", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czzjzf1", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_czzjzf2", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_fssr", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_gwk1", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_gwk2", "");
                //model.BookmarkerTextRang.Add("ywcm_szyw_srdjrz", "");
                //model.BookmarkerTextRang.Add("zdgl_clf4", "");
                //model.BookmarkerTextRang.Add("zdgl_hyf3", "");
                //model.BookmarkerTextRang.Add("zdgl_pxf3", "");
                //model.BookmarkerTextRang.Add("zdgl_zdf1", "");
                //model.BookmarkerTextRang.Add("zfcgyw_6", "");
            }
            else if (FileName.IndexOf("采购业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_GJZ4", "GJZ4");
                Keywords.Add("key_zfcgxzqdks", "zfcgxzqdks");
                Keywords.Add("key_zfcgzlgkks", "zfcgzlgkks");
                Keywords.Add("key_srywgkks", "srywgkks");
                Keywords.Add("key_dwzxcgsq01", "dwzxcgsq01");
                Keywords.Add("key_cghtsq01", "cghtsq01");
                Keywords.Add("key_gyzcxzqdks", "gyzcxzqdks");
                Keywords.Add("key_bgypglgkks", "bgypglgkks");

                //model.BookmarkerImage.Add("img_bgypgmywlc", imgPath + "img_bgypgmywlc.jpg");
                //model.BookmarkerImage.Add("img_zfcgbgsqsplc", imgPath + "img_zfcgbgsqsplc.jpg");
                //model.BookmarkerImage.Add("img_zfcggmfwlc", imgPath + "img_zfcggmfwlc.jpg");
                //model.BookmarkerImage.Add("img_zfcgldywlc", imgPath + "img_zfcgldywlc.jpg");
                //model.BookmarkerImage.Add("img_zfcgssjhsbsplc", imgPath + "img_zfcgssjhsbsplc.jpg");
                //model.BookmarkerImage.Add("img_zfcgywlct", imgPath + "img_zfcgywlct.jpg");
                //model.BookmarkerImage.Add("img_zfcgzycllc", imgPath + "img_zfcgzycllc.jpg");

                //model.BookmarkerTextRang.Add("ywcm_zfcg_zfgmff", "");
                //model.BookmarkerTextRang.Add("zfcgyw_10", "");
                //model.BookmarkerTextRang.Add("zfcgyw_6", "");
                //model.BookmarkerTextRang.Add("zfcgyw_9", "");
            }
            else if (FileName.IndexOf("资产业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_jine041509", "jine041509");
                Keywords.Add("key_gyzcxzqdks", "gyzcxzqdks");
                Keywords.Add("key_jine0407", "jine0407");
                Keywords.Add("key_jine0408", "jine0408");
                Keywords.Add("key_gdzcgz", "gdzcgz");
                Keywords.Add("key_gdzcdb", "gdzcdb");
                Keywords.Add("key_gdzccz", "gdzccz");
                Keywords.Add("key_gdzcqc", "gdzcqc");

                //model.BookmarkerImage.Add("img_gdzcczywlc", imgPath + "img_gdzcczywlc.jpg");
                //model.BookmarkerImage.Add("img_gdzcdbywlc", imgPath + "img_gdzcdbywlc.jpg");
                //model.BookmarkerImage.Add("img_gdzcgzywlc", imgPath + "img_gdzcgzywlc.jpg");
                //model.BookmarkerImage.Add("img_gdzcqcywlc", imgPath + "img_gdzcqcywlc.jpg");
                //model.BookmarkerImage.Add("img_zcywcqdjbgzxlc", imgPath + "img_zcywcqdjbgzxlc.jpg");
                //model.BookmarkerImage.Add("img_zcywcqjfdclc", imgPath + "img_zcywcqjfdclc.jpg");
                //model.BookmarkerImage.Add("img_zcywczcjywlc", imgPath + "img_zcywczcjywlc.jpg");
                //model.BookmarkerImage.Add("img_zcywndbgbzshlc", imgPath + "img_zcywndbgbzshlc.jpg");
                //model.BookmarkerImage.Add("img_zcywpglc", imgPath + "img_zcywpglc.jpg");
                //model.BookmarkerImage.Add("img_zcywpzjhsbsplc", imgPath + "img_zcywpzjhsbsplc.jpg");
                //model.BookmarkerImage.Add("img_zcywsrsjlc", imgPath + "img_zcywsrsjlc.jpg");

                //model.BookmarkerTextRang.Add("ywcm_zcgl_gyzccqjfdc", "");
                //model.BookmarkerTextRang.Add("ywcm_zcgl_gyzcczcj", "");
                //model.BookmarkerTextRang.Add("ywcm_zcgl_gyzcdjbgzx", "");
                //model.BookmarkerTextRang.Add("ywcm_zcgl_gyzcsrsj", "");
                //model.BookmarkerTextRang.Add("ywcm_zcgl_zcpglc", "");
            }
            else if (FileName.IndexOf("建设项目业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_jsxmgkks01", "jsxmgkks01");
                Keywords.Add("key_GJZ5", "GJZ5");
                Keywords.Add("key_DZZJGMC", "DZZJGMC");
                Keywords.Add("key_jsxmjxpjks01", "jsxmjxpjks01");

                //model.BookmarkerImage.Add("img_jsxmdagllc", imgPath + "img_jsxmdagllc.jpg");
                //model.BookmarkerImage.Add("img_jsxmfkywlc", imgPath + "img_jsxmfkywlc.jpg");
                //model.BookmarkerImage.Add("img_jsxmgkzbyqzblc", imgPath + "img_jsxmgkzbyqzblc.jpg");
                //model.BookmarkerImage.Add("img_jsxmjgjsywlc", imgPath + "img_jsxmjgjsywlc.jpg");
                //model.BookmarkerImage.Add("img_jsxmsjbgqslc", imgPath + "img_jsxmsjbgqslc.jpg");
                //model.BookmarkerImage.Add("img_jsxmywgllc", imgPath + "img_jsxmywgllc.jpg");

                ////model.BookmarkerTextRang.Add("jsxm_1", "");
                //model.BookmarkerTextRang.Add("ywcm_jsxm_gkzbyqzb", "");
                //model.BookmarkerTextRang.Add("ywcm_jsxm_sjbgqsqz", "");
            }
            else if (FileName.IndexOf("合同业务控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_htgkks1", "htgkks1");

                //model.BookmarkerImage.Add("img_htywlc", imgPath + "img_htywlc.jpg");
                //model.BookmarkerImage.Add("img_htywhtjfcllc", imgPath + "img_htywhtjfcllc.jpg");//新增

                //model.BookmarkerTextRang.Add("htgl_1", "");
            }
            else if (FileName.IndexOf("印章及票据管理控制") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_yzglgkks", "yzglgkks");
                Keywords.Add("key_GJZ6", "GJZ6");

                //model.BookmarkerImage.Add("img_yzjjywlc", imgPath + "img_yzjjywlc.jpg");
                //model.BookmarkerImage.Add("img_yzkzywlc", imgPath + "img_yzkzywlc.jpg");
                //model.BookmarkerImage.Add("img_yzsyywlc", imgPath + "img_yzsyywlc.jpg");
                //model.BookmarkerImage.Add("img_yztyywlc", imgPath + "img_yztyywlc.jpg");
            }
            else if (FileName.IndexOf("债务业务管理制度") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_DZZJGMC", "DZZJGMC");

                //model.BookmarkerTextRang.Add("zdgl_zwyw1", "");
            }
            else if (FileName.IndexOf("对外投资业务管理制度") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_DZZJGMC", "DZZJGMC");

                //model.BookmarkerTextRang.Add("zdgl_dwtzyw", "");
            }
            else if (FileName.IndexOf("业务相关表格") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");

                //model.BookmarkerTextRang.Add("htgl_3", "");
                //model.BookmarkerTextRang.Add("jkgl_3", "");
                //model.BookmarkerTextRang.Add("zdgl_clf3", "");
                //model.BookmarkerTextRang.Add("zdgl_kjz2", "");
            }
            #endregion
            else if (FileName.IndexOf("内部控制的监督与评价") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
                Keywords.Add("key_zzzwmc", "正职职务名称");
                Keywords.Add("key_jdjcxzcy", "监督检查小组成员");
            }
            else if (FileName.IndexOf("相关规章、制度清单汇编") >= 0)
            {
                Keywords.Add("DWQC", "长春佳盟信息科技有限责任公司");
            }
            return Keywords;
        }
    }
}

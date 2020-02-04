using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using JMProject.Dal;
using JMProject.Common;
using JMProject.Model;
using JMProject.Model.Esayui;
using JMProject.Model.View;
using JMProject.Model.Sys;
using System.Drawing;

namespace JMProject.BLL
{
    public class NkscBLL
    {
        DBHelperSql dao = new DBHelperSql();
        public NkscBLL()
        { }

        public int Insert(Nksc model)
        {
            return dao.Insert<Nksc>(model);
        }

        public int Delete(string tsql)
        {
            return dao.Delete(tsql);
        }

        public int Update(string tsql)
        {
            return dao.Update(tsql);
        }

        public int UpdateFileName(String filename, String id)
        {
            return dao.Update("update Nksc set fileName='" + filename + "' where id='" + id + "'");
        }

        public bool Tran(Dictionary<string, object> tsqls)
        {
            return dao.Transaction(tsqls);
        }

        public Nksc GetModel(string _where)
        {
            string where = "where 1=1 " + _where;
            String tsql = "select * from Nksc " + where;
            DataTable dt = dao.Select(tsql);
            Nksc model = new Nksc();
            foreach (DataRow item in dt.Rows)
            {
                model.id = Guid.Parse(item["id"].ToStringEx());
                model.SaleOrderID = item["SaleOrderID"].ToStringEx();
                model.CustomerID = item["CustomerID"].ToStringEx();
                model.dwqc = item["dwqc"].ToStringEx();
                model.dwjc = item["dwjc"].ToStringEx();
                model.dwjj = item["dwjj"].ToStringEx();
                model.bhks = item["bhks"].ToStringEx();
                model.scqtbm = item["scqtbm"].ToStringEx();
                model.scxzbm = item["scxzbm"].ToStringEx();
                model.zzzwmc = item["zzzwmc"].ToStringEx();
                model.ldzzmc = item["ldzzmc"].ToStringEx();
                model.ldzzfg = item["ldzzfg"].ToStringEx();
                model.fzzwmc1 = item["fzzwmc1"].ToStringEx();
                model.ldfzmc1 = item["ldfzmc1"].ToStringEx();
                model.ldfzfg1 = item["ldfzfg1"].ToStringEx();
                model.fxpgxzqtks = item["fxpgxzqtks"].ToStringEx();
                model.fxpgxzcy = item["fxpgxzcy"].ToStringEx();
                model.fxpgxzzz = item["fxpgxzzz"].ToStringEx();
                model.fxpgxzfzz = item["fxpgxzfzz"].ToStringEx();
                model.nkldxzcy = item["nkldxzcy"].ToStringEx();
                model.nkldxzqdks = item["nkldxzqdks"].ToStringEx();
                model.nkldxzzz = item["nkldxzzz"].ToStringEx();
                model.nkldxzfzz = item["nkldxzfzz"].ToStringEx();
                model.ysldxzcy = item["ysldxzcy"].ToStringEx();
                model.ysldxzqdks = item["ysldxzqdks"].ToStringEx();
                model.ysldxzzz = item["ysldxzzz"].ToStringEx();
                model.ysldxzfzz = item["ysldxzfzz"].ToStringEx();
                model.zfcgxzcy = item["zfcgxzcy"].ToStringEx();
                model.zfcgxzqdks = item["zfcgxzqdks"].ToStringEx();
                model.zfcgxzzz = item["zfcgxzzz"].ToStringEx();
                model.zfcgxzfzz = item["zfcgxzfzz"].ToStringEx();
                model.gyzcxzcy = item["gyzcxzcy"].ToStringEx();
                model.gyzcxzqdks = item["gyzcxzqdks"].ToStringEx();
                model.gyzcxzzz = item["gyzcxzzz"].ToStringEx();
                model.gyzcxzfzz = item["gyzcxzfzz"].ToStringEx();
                model.jdjcxzcy = item["jdjcxzcy"].ToStringEx();
                model.jdjcxzqdks = item["jdjcxzqdks"].ToStringEx();
                model.jdjcxzzz = item["jdjcxzzz"].ToStringEx();
                model.jdjcxzfzz = item["jdjcxzfzz"].ToStringEx();
                model.nbsjks = item["nbsjks"].ToStringEx();
                model.zdjcsshpjks = item["zdjcsshpjks"].ToStringEx();
                model.bxrgwzdks = item["bxrgwzdks"].ToStringEx();
                model.bzndlgjhks = item["bzndlgjhks"].ToStringEx();
                model.bnlgdgwmc = item["bnlgdgwmc"].ToStringEx();
                model.zdbgdgkks = item["zdbgdgkks"].ToStringEx();
                model.xxgkzrjjks = item["xxgkzrjjks"].ToStringEx();
                model.zdbgcdks = item["zdbgcdks"].ToStringEx();
                model.fzxxglxtks = item["fzxxglxtks"].ToStringEx();
                model.xxxcgzqtks = item["xxxcgzqtks"].ToStringEx();
                model.ksglks = item["ksglks"].ToStringEx();
                model.bhyw = item["bhyw"].ToStringEx();

                model.bdwsrbk = item["bdwsrbk"].ToStringEx();
                model.srywgkks = item["srywgkks"].ToStringEx();
                model.jfzcgkks = item["jfzcgkks"].ToStringEx();
                model.zfcgzlgkks = item["zfcgzlgkks"].ToStringEx();
                model.rsglzdgkks = item["rsglzdgkks"].ToStringEx();
                model.rsglhbks = item["rsglhbks"].ToStringEx();
                model.nzkhgkks = item["nzkhgkks"].ToStringEx();
                model.lzjmytgkks = item["lzjmytgkks"].ToStringEx();
                model.ywslzdmc = item["ywslzdmc"].ToStringEx();
                model.yhscfj = item["yhscfj"].ToStringEx();

                model.kqsjswS = item["kqsjswS"].ToStringEx();
                model.kqsjswE = item["kqsjswE"].ToStringEx();
                model.kqsjxwS = item["kqsjxwS"].ToStringEx();
                model.kqsjxwE = item["kqsjxwE"].ToStringEx();

                model.kqsjswSd = item["kqsjswSd"].ToStringEx();
                model.kqsjswEd = item["kqsjswEd"].ToStringEx();
                model.kqsjxwSd = item["kqsjxwSd"].ToStringEx();
                model.kqsjxwEd = item["kqsjxwEd"].ToStringEx();

                model.htgkks1 = item["htgkks1"].ToStringEx();

                model.fileName = item["fileName"].ToStringEx();

                model.flag = item["flag"].ToStringEx();
                model.pfr = item["pfr"].ToStringEx();

                model.dzzjgmc = item["dzzjgmc"].ToStringEx();
                model.flagDown = item["flagDown"].ToStringEx();
                model.flagMoney = item["flagMoney"].ToStringEx();

                model.swfName = item["swfName"].ToStringEx();
                model.tsyqtext = item["tsyqtext"].ToStringEx();
                model.zddate = item["zddate"].ToStringEx();
                model.xyzdsum = int.Parse(item["xyzdsum"].ToStringEx());
                model.bczdsum = int.Parse(item["bczdsum"].ToStringEx());
                model.sysum = int.Parse(item["sysum"].ToStringEx());
                model.bz = item["bz"].ToStringEx();

                model.wtfkFlag = item["wtfkFlag"].ToStringEx();
                model.NkscDate = item["NkscDate"].ToStringEx();

                model.NkscSBDate = item["NkscSBDate"].ToStringEx();
                model.syfw0415 = item["syfw0415"].ToStringEx();
                model.jine0407 = item["jine0407"].ToStringEx();
                model.jine0408 = item["jine0408"].ToStringEx();
                model.jine041509 = item["jine041509"].ToStringEx();
                model.Radioclf = item["Radioclf"].ToStringEx();
                model.Radiohyf = item["Radiohyf"].ToStringEx();
                model.Radiopxf = item["Radiopxf"].ToStringEx();
                model.Radiogwzdf = item["Radiogwzdf"].ToStringEx();
                model.Radiobzz = item["Radiobzz"].ToStringEx();

                model.Radio_zjzf = item["Radio_zjzf"].ToStringEx();
                model.Radio_jksh = item["Radio_jksh"].ToStringEx();
                model.Radio_bxsh = item["Radio_bxsh"].ToStringEx();

                model.NkscDatePDF = item["NkscDatePDF"].ToStringEx();
                model.peoPDF = item["peoPDF"].ToStringEx();
                model.NkscDateSC = item["NkscDateSC"].ToStringEx();
                model.peoSC = item["peoSC"].ToStringEx();
                model.zzzwDY = item["zzzwDY"].ToStringEx();
                model.fzzwDY = item["fzzwDY"].ToStringEx();

                model.NkscDateSCPDF = item["NkscDateSCPDF"].ToStringEx();

                model.TcFlag = item["TcFlag"].ToStringEx();
                model.TcDate = item["TcDate"].ToStringEx();

                model.EngineRoom = item["EngineRoom"].ToStringEx();
                model.gwglgkks = item["gwglgkks"].ToStringEx();
                model.zwxxgkks = item["zwxxgkks"].ToStringEx();

                model.gwkglks = item["gwkglks"].ToStringEx();
                model.gwkjdks = item["gwkjdks"].ToStringEx();

                model.gdzccz = item["gdzccz"].ToStringEx();
                model.gdzcdb = item["gdzcdb"].ToStringEx();
                model.gdzcgz = item["gdzcgz"].ToStringEx();
                model.gdzcqc = item["gdzcqc"].ToStringEx();
                model.bgypglgkks = item["bgypglgkks"].ToStringEx();
                model.yzglgkks = item["yzglgkks"].ToStringEx();

                model.gwkzd = item["gwkzd"].ToStringEx();
                model.czzxzjgkks = item["czzxzjgkks"].ToStringEx();

                model.zxzjgl = item["zxzjgl"].ToStringEx();

                //180305
                model.jsxmgkks01 = item["jsxmgkks01"].ToStringEx();
                model.jsxmjxpjks01 = item["jsxmjxpjks01"].ToStringEx();
                model.Radio_cghtsq = item["Radio_cghtsq"].ToStringEx();
                model.nbkzgzxzzz01 = item["nbkzgzxzzz01"].ToStringEx();
                model.nbkzgzxzfzz01 = item["nbkzgzxzfzz01"].ToStringEx();
                model.nbkzgzxzzzcy01 = item["nbkzgzxzzzcy01"].ToStringEx();
                model.nbkzgzxzzzqt01 = item["nbkzgzxzzzqt01"].ToStringEx();
                model.version = item["version"].ToStringEx();

                model.Radio_zxcgsq = item["Radio_zxcgsq"].ToStringEx();
                model.Radio_czzxzj = item["Radio_czzxzj"].ToStringEx();
                model.Radio_fczzxzj = item["Radio_fczzxzj"].ToStringEx();

                model.FPImage = item["FPImage"].ToStringEx();

            }
            return model;
        }

        public List<View_Nksc> SelectAll(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "View_Nksc";
            //string Fields = "[id], [CustomerID], [dwqc], [dwjc],[fileName],[flag],[pfr],[flagDown],[flagMoney],[fileNameWT],[swfName],[Bz],[tsyqtext],[pdfname],[zddate],[xyzdsum],[bczdsum],[sysum],[wtfkFlag],[NkscDate],[NkscSBDate],[NkscDatePDF],[peoPDF],[NkscDateSC],[peoSC],[NkscDateSCPDF],[AddBook],TcFlag,TcDate";
            string Fields = "[id],[SaleOrderID], [CustomerID],[fileName], [dwqc],[NkscDate],[NkscSBDate],[Lxr],[Phone],[UserName],[Region],pfr,flag,wtfkFlag,zddate,xyzdsum,bczdsum,sysum,tsyqtext,NkscDateSC,peoSC,swfName,NkscDateSCPDF,NkscDatePDF,peoPDF,bz,yhscfj,AddBook,IsUpdate,IsUpdateE,[version],FPImage";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = "Order by " + pager.sort + " " + pager.order;
            }
            else
            {
                Order = "Order by NkscSBDate ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<View_Nksc>("Proc_Page", sp);
        }

        public DataTable SelectDaoC(string where)
        {
            string sql = "select dwqc as 客户全称,NkscDate as 建档日期,Lxr as 联系人,Phone as 手机号,QtLxr as 其他联系人,QtTel as 其他电话,(case flag "
                + "when '1' then '未提交'"
                + "when '2' then 'A已初审'"
                + "when '3' then 'B已派工'"
                + "when '4' then 'D用户核对中'"
                + "when '5' then 'C已编制完成'"
                + "when '6' then 'E已定稿'"
                + "when '7' then '已提交'"
                + "when '8' then 'F已装订'"
                + "when '9' then 'G装订后有修改'"
                + "when '11' then 'I手册已领取'"
                + "else '' end) as 手册状态,"
                + "tsyqtext as 特殊需求,NkscDatePDF as 发送PDF日期, peoPDF as 发送PDF人, NkscDateSC as 手册领取日期, peoSC as 分发手册人, xyzdsum as 总数量,Address as 地址 from View_NkscExcel where 1=1 " + where + " order by NkscDate desc";
            return dao.Select(sql);

        }

        public List<Nksc_fz> SelectNkscfz(Guid id)
        {
            String tsql = "select * from Nksc_fz where id='" + id + "' order by sort";
            DataTable dt = dao.Select(tsql);
            List<Nksc_fz> list = new List<Nksc_fz>();
            foreach (DataRow item in dt.Rows)
            {
                Nksc_fz model = new Nksc_fz();
                model.id = id;
                model.sort = int.Parse(item["sort"].ToStringEx());
                model.fzzwmc = item["fzzwmc"].ToStringEx();
                model.ldfzmc = item["ldfzmc"].ToStringEx();
                model.ldfzfg = item["ldfzfg"].ToStringEx();
                model.fzzwDY = item["fzzwDY"].ToStringEx();
                list.Add(model);
            }
            return list;
        }

        public List<Nksc_Zcyw> SelectNksczc(Guid id, string zcywtype)
        {
            String tsql = "select * from Nksc_Zcyw where id='" + id + "' and zcywtype='" + zcywtype + "' order by sort";
            DataTable dt = dao.Select(tsql);
            List<Nksc_Zcyw> list = new List<Nksc_Zcyw>();
            foreach (DataRow item in dt.Rows)
            {
                Nksc_Zcyw model = new Nksc_Zcyw();
                model.id = id;
                model.sort = int.Parse(item["sort"].ToStringEx());
                model.zcmoney = item["zcmoney"].ToStringEx();
                model.zctype = item["zctype"].ToStringEx();
                model.zcspr = item["zcspr"].ToStringEx();
                model.zcywtype = item["zcywtype"].ToStringEx();
                list.Add(model);
            }
            return list;
        }

        public List<Nksc_Jkyw> SelectNkscjk(Guid id)
        {
            String tsql = "select * from Nksc_Jkyw where id='" + id + "' order by sort";
            return dao.Select<Nksc_Jkyw>(tsql);
        }

        public List<Nksc_Bxyw> SelectNkscbx(Guid id)
        {
            String tsql = "select * from Nksc_Bxyw where id='" + id + "' order by sort";
            return dao.Select<Nksc_Bxyw>(tsql);
        }

        public List<Nksc_cghtsq> SelectNksccg(Guid id, string htywtype)
        {
            String tsql = "select * from Nksc_cghtsq where id='" + id + "' and htywtype='" + htywtype + "' order by sort";
            return dao.Select<Nksc_cghtsq>(tsql);
        }

        public List<View_KS> SelectNkscks(Guid id)
        {
            String tsql = "select * from Nksc_kszn where zid='" + id + "' order by sort";
            DataTable dt = dao.Select(tsql);
            List<View_KS> list = new List<View_KS>();
            foreach (DataRow item in dt.Rows)
            {
                View_KS model = new View_KS();
                Nksc_kszn ksznmodel = new Nksc_kszn();

                ksznmodel.id = Guid.Parse(item["id"].ToStringEx());
                ksznmodel.zid = id;
                ksznmodel.sort = int.Parse(item["sort"].ToStringEx());
                ksznmodel.ksmc = item["ksmc"].ToStringEx();
                ksznmodel.kszn = item["kszn"].ToStringEx();

                List<Nksc_ksgwzr> gwzrlist = new List<Nksc_ksgwzr>();
                DataTable gwzrdt = dao.Select("select * from Nksc_ksgwzr where id='" + ksznmodel.id + "' order by sort");
                foreach (DataRow drow in gwzrdt.Rows)
                {
                    Nksc_ksgwzr zrmodel = new Nksc_ksgwzr();
                    zrmodel.id = ksznmodel.id;
                    zrmodel.sort = int.Parse(drow["sort"].ToStringEx());
                    zrmodel.ksgw = drow["ksgw"].ToStringEx();
                    zrmodel.ksgwzr = drow["ksgwzr"].ToStringEx();
                    gwzrlist.Add(zrmodel);
                }
                model.kszn = ksznmodel;
                model.ksgwzr = gwzrlist;

                list.Add(model);
            }
            return list;
        }

        public void Insert_fz(Dictionary<string, object> tsqls, Guid id, string sort, string fzzwmc, string ldfzmc, string ldfzfg, string fzzwDY)
        {
            string[] sort_ary = sort.Split(',');
            string[] fzzwmc_ary = fzzwmc.Split(',');
            string[] ldfzmc_ary = ldfzmc.Split(',');
            string[] ldfzfg_ary = ldfzfg.Split(',');
            string[] fzzwDY_ary = fzzwDY.Split(',');
            tsqls.Add("delete from Nksc_fz where id='" + id + "'", null);
            if (sort_ary.Length > 0 && !string.IsNullOrEmpty(sort_ary[0]))
            {
                for (int i = 0; i < sort_ary.Length; i++)
                {
                    int sort1 = int.Parse(sort_ary[i]);
                    string fzzwmc1 = fzzwmc_ary[i];
                    string ldfzmc1 = ldfzmc_ary[i];
                    string ldfzfg1 = ldfzfg_ary[i];
                    string fzzwDY1 = fzzwDY_ary[i];
                    tsqls.Add("insert into Nksc_fz(id,sort,fzzwmc,ldfzmc,ldfzfg,fzzwDY) values('" + id + "'," + sort1 + ",'" + fzzwmc1 + "','" + ldfzmc1 + "','" + ldfzfg1 + "','" + fzzwDY1 + "')", null);
                }
            }
        }

        public void Insert_Cght(Dictionary<string, object> tsqls, Guid id, string cghtMoney, string cghtType, string cghtSpr, string htywtype)
        {
            string[] cghtMoney_ary = cghtMoney.Split('∮');
            string[] cghtType_ary = cghtType.Split('∮');
            string[] cghtSpr_ary = cghtSpr.Split('∮');
            tsqls.Add("delete from Nksc_cghtsq where id='" + id + "' and htywtype='" + htywtype + "'", null);
            if (cghtSpr_ary.Length > 0 && !string.IsNullOrEmpty(cghtSpr_ary[0]))
            {
                for (int i = 0; i < cghtSpr_ary.Length; i++)
                {
                    int sort = i + 1;
                    string jkmoney = cghtMoney_ary[i];
                    string jktype = cghtType_ary[i];
                    string jkspr = cghtSpr_ary[i];
                    tsqls.Add("insert into Nksc_cghtsq(id,sort,jkmoney,jktype,jkspr,htywtype) values('" + id + "'," + sort + ",'" + jkmoney + "','" + jktype + "','" + jkspr + "','" + htywtype + "')", null);
                }
            }
        }

        public void Insert_Zc(Dictionary<string, object> tsqls, Guid id, string zcywMoney, string zcywType, string zcywSpr, string zcywtype)
        {
            string[] zcywMoney_ary = zcywMoney.Split('∮');
            string[] zcywType_ary = zcywType.Split('∮');
            string[] zcywSpr_ary = zcywSpr.Split('∮');
            tsqls.Add("delete from Nksc_Zcyw where id='" + id + "' and zcywtype='" + zcywtype + "'", null);
            if (zcywSpr_ary.Length > 0 && !string.IsNullOrEmpty(zcywSpr_ary[0]))
            {
                for (int i = 0; i < zcywSpr_ary.Length; i++)
                {
                    int sort = i + 1;
                    string zcmoney = zcywMoney_ary[i];
                    string zctype = zcywType_ary[i];
                    string zcspr = zcywSpr_ary[i];
                    tsqls.Add("insert into Nksc_Zcyw(id,sort,zcmoney,zctype,zcspr,zcywtype) values('" + id + "'," + sort + ",'" + zcmoney + "','" + zctype + "','" + zcspr + "','" + zcywtype + "')", null);
                }
            }
        }

        public void Insert_Jk(Dictionary<string, object> tsqls, Guid id, string jkywMoney, string jkywType, string jkywSpr)
        {
            string[] jkywMoney_ary = jkywMoney.Split('∮');
            string[] jkywType_ary = jkywType.Split('∮');
            string[] jkywSpr_ary = jkywSpr.Split('∮');
            tsqls.Add("delete from Nksc_Jkyw where id='" + id + "'", null);
            if (jkywSpr_ary.Length > 0 && !string.IsNullOrEmpty(jkywSpr_ary[0]))
            {
                for (int i = 0; i < jkywSpr_ary.Length; i++)
                {
                    int sort = i + 1;
                    string jkmoney = jkywMoney_ary[i];
                    string jktype = jkywType_ary[i];
                    string jkspr = jkywSpr_ary[i];
                    tsqls.Add("insert into Nksc_Jkyw(id,sort,jkmoney,jktype,jkspr) values('" + id + "'," + sort + ",'" + jkmoney + "','" + jktype + "','" + jkspr + "')", null);
                }
            }
        }

        public void Insert_Bx(Dictionary<string, object> tsqls, Guid id, string bxywMoney, string bxywType, string bxywSpr)
        {
            string[] bxywMoney_ary = bxywMoney.Split('∮');
            string[] bxywType_ary = bxywType.Split('∮');
            string[] bxywSpr_ary = bxywSpr.Split('∮');
            tsqls.Add("delete from Nksc_Bxyw where id='" + id + "'", null);
            if (bxywSpr_ary.Length > 0 && !string.IsNullOrEmpty(bxywSpr_ary[0]))
            {
                for (int i = 0; i < bxywSpr_ary.Length; i++)
                {
                    int sort = i + 1;
                    string bxmoney = bxywMoney_ary[i];
                    string bxtype = bxywType_ary[i];
                    string bxspr = bxywSpr_ary[i];
                    tsqls.Add("insert into Nksc_Bxyw(id,sort,bxmoney,bxtype,bxspr) values('" + id + "'," + sort + ",'" + bxmoney + "','" + bxtype + "','" + bxspr + "')", null);
                }
            }
        }

        public void Insert_ks(Dictionary<string, object> tsqls, Guid id, string kssort, string ksmc, string kszn, string kszr)
        {
            string[] kssort_ary = kssort.Split('∮');
            string[] ksmc_ary = ksmc.Split('∮');
            string[] kszn_ary = kszn.Split('∮');
            string[] kszr_ary = kszr.Split('☉');
            tsqls.Add("delete from Nksc_ksgwzr where id in (select id from Nksc_kszn where zid='" + id + "')", null);
            tsqls.Add("delete from Nksc_kszn where zid='" + id + "'", null);
            if (kssort_ary.Length > 0 && !string.IsNullOrEmpty(kssort_ary[0]))
            {
                for (int i = 0; i < kssort_ary.Length; i++)
                {
                    int sortV = int.Parse(kssort_ary[i]);
                    string ksmcV = ksmc_ary[i].ToStringEx();
                    string ksznV = kszn_ary[i].ToStringEx();
                    Guid maxid = Guid.NewGuid();
                    tsqls.Add("insert into Nksc_kszn(id,zid,sort,ksmc,kszn) values('" + maxid + "','" + id + "'," + sortV + ",'" + ksmcV + "','" + ksznV + "')", null);

                    string[] ksgwzr_ary = kszr_ary[i].Split('¤');

                    string[] gwsort_ary = ksgwzr_ary[0].Split('∮');
                    string[] gwmc_ary = ksgwzr_ary[1].Split('∮');
                    string[] gwzr_ary = ksgwzr_ary[2].Split('∮');
                    for (int j = 0; j < gwsort_ary.Length; j++)
                    {
                        int sortGW = int.Parse(gwsort_ary[j]);
                        string ksgw = gwmc_ary[j].ToStringEx();
                        string ksgwzr = gwzr_ary[j].ToStringEx();
                        tsqls.Add("insert into Nksc_ksgwzr(id,sort,ksgw,ksgwzr) values('" + maxid + "'," + sortGW + ",'" + ksgw + "','" + ksgwzr + "')", null);
                    }
                }
            }
        }

        public String GetNameStr(String fieldName, String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select " + fieldName + " from Nksc" + where;
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public String GetMaxVersion()
        {
            String tsql = "select max(versionS) from Nksc_Version";
            object result = dao.GetScalar(tsql);
            return result.ToStringEx();
        }

        public List<Nksc_Version> SelectAllVersion(string Where, GridPager pager)
        {
            string Order = string.Empty;
            string Table = "Nksc_Version";
            string Fields = "versionS";
            if (!string.IsNullOrEmpty(Where))
            {
                Where = "Where 1=1 " + Where;
            }
            if (!string.IsNullOrEmpty(pager.sort))
            {
                Order = "Order by " + pager.sort + " " + pager.order;
            }
            else
            {
                Order = "Order by versionS ASC";
            }

            pager.totalRows = Convert.ToInt32(dao.GetScalar("select count(*) from " + Table + " " + Where));

            List<object> sp = new List<object>();
            sp.Add(new SqlParameter("@Table", Table));
            sp.Add(new SqlParameter("@Fields", Fields));
            sp.Add(new SqlParameter("@Where", Where));
            sp.Add(new SqlParameter("@Order", Order));
            sp.Add(new SqlParameter("@currentpage", pager.page));
            sp.Add(new SqlParameter("@pagesize", pager.rows));
            return dao.ProExecSelect<Nksc_Version>("Proc_Page", sp);
        }

        public List<S_SWF> SelectAll_Swf()
        {
            string Table = "select id,swfName from Nksc where swfName<>''";
            return dao.Select<S_SWF>(Table);
        }

        /// <summary>
        /// 获取内控手册的状态
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public bool GetFlag(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            String tsql = "select flag from Nksc where id='" + id + "'";
            string obj = dao.GetScalar(tsql).ToStringEx();
            if (obj == "1" || obj == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// 获取内控手册的是否装订状态
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public bool GetZDFlag(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            String tsql = "select flag from Nksc where id='" + id + "'";
            string obj = dao.GetScalar(tsql).ToStringEx();
            if (obj == "8" || obj == "9" || obj == "11" || obj == "13")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 生成word关键字替换

        public Dictionary<string, string> GetKeyAndData(string id)
        {
            String tsql = "select * from Nksc where id='" + id + "'";
            DataTable dt = dao.Select(tsql);

            String tsqlkey = "select * from Dkey";
            DataTable dtkey = dao.Select(tsqlkey);

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (DataRow item in dtkey.Rows)
            {
                if (item["WordKey"].ToString() == "#DZZJGMC")
                {
                    if (dt.Rows[0]["dzzjgmc"].ToStringEx() == "无")
                    {
                        string dzzjgmc = "";
                        result.Add(item["WordKey"].ToString(), dzzjgmc);
                    }
                    else
                    {
                        result.Add(item["WordKey"].ToString(), dt.Rows[0][item["DBKey"].ToString()].ToStringEx());
                    }
                }
                else if (item["WordKey"].ToString() == "#GJZ1")
                {
                    string str_gjz1 = " ";

                    if (dt.Rows[0]["dzzjgmc"].ToStringEx() == "无")
                    {
                        str_gjz1 = dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。^p";
                        str_gjz1 += dt.Rows[0]["fzzwmc1"].ToStringEx() + dt.Rows[0]["ldfzmc1"].ToStringEx() + "：分管" + dt.Rows[0]["ldfzfg1"].ToStringEx() + "工作。";
                    }
                    else
                    {
                        if (dt.Rows[0]["zzzwDY"].ToStringEx() != "1")
                        {
                            str_gjz1 = dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。^p";
                        }
                        else
                        {
                            str_gjz1 = dt.Rows[0]["dzzjgmc"].ToStringEx() + "成员、" + dt.Rows[0]["zzzwmc"].ToStringEx() + dt.Rows[0]["ldzzmc"].ToStringEx() + "：主持" + dt.Rows[0]["DWQC"].ToStringEx() + "全面工作；" + dt.Rows[0]["ldzzfg"].ToStringEx() + "。^p";
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
                    result.Add(item["WordKey"].ToString(), str_gjz1);
                }
                else if (item["WordKey"].ToString() == "#GJZ2")
                {
                    string str_gjz2 = "，";
                    if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                    {
                        str_gjz2 = "，副组长由" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "担任，";
                    }
                    result.Add(item["WordKey"].ToString(), str_gjz2);
                }
                else if (item["WordKey"].ToString() == "#GJZ3")
                {
                    string str_gjz3 = dt.Rows[0]["zzzwmc"].ToStringEx();
                    if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                    {
                        str_gjz3 = dt.Rows[0]["zzzwmc"].ToStringEx() + "或" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "";
                    }
                    result.Add(item["WordKey"].ToString(), str_gjz3);
                }
                else if (item["WordKey"].ToString() == "#GJZ4")
                {
                    string str_gjz4 = dt.Rows[0]["zzzwmc"].ToStringEx();
                    if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                    {
                        str_gjz4 = dt.Rows[0]["zzzwmc"].ToStringEx() + "、" + dt.Rows[0]["fzzwmc1"].ToStringEx();
                    }
                    result.Add(item["WordKey"].ToString(), str_gjz4);
                }
                else if (item["WordKey"].ToString() == "#GJZ5")
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
                    result.Add(item["WordKey"].ToString(), str_gjz5);
                }
                else if (item["WordKey"].ToString() == "#GJZ6")
                {
                    string str_gjz6 = "需报" + dt.Rows[0]["zzzwmc"].ToStringEx() + "审批。";
                    if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                    {
                        str_gjz6 = "一般事宜由" + dt.Rows[0]["fzzwmc1"].ToStringEx() + "审批，重要需报" + dt.Rows[0]["zzzwmc"].ToStringEx() + "审批。";
                    }
                    result.Add(item["WordKey"].ToString(), str_gjz6);
                }
                else if (item["WordKey"].ToString() == "#GJZ7")
                {
                    string str_gjz7 = "负责人";
                    if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "")
                    {
                        str_gjz7 = "负责人、" + dt.Rows[0]["fzzwmc1"].ToStringEx();
                    }
                    result.Add(item["WordKey"].ToString(), str_gjz7);
                }
                else if (item["WordKey"].ToString() == "#dzcylist")
                {
                    DataTable dtfz = dao.Select("select * from Nksc_fz where id='" + id + "'");
                    string dzcylist = "";
                    foreach (DataRow drow in dtfz.Rows)
                    {
                        string dzcy = "";
                        if (dt.Rows[0]["dzzjgmc"].ToStringEx() == "无")
                        {
                            dzcy = string.Format("{0}{1}：分管{2}工作。\r", drow["fzzwmc"], drow["ldfzmc"], drow["ldfzfg"]);
                        }
                        else
                        {
                            if (drow["fzzwDY"].ToStringEx() != "1")
                            {
                                dzcy = string.Format("{0}{1}：分管{2}工作。\r", drow["fzzwmc"], drow["ldfzmc"], drow["ldfzfg"]);
                            }
                            else
                            {
                                dzcy = string.Format(dt.Rows[0]["dzzjgmc"].ToStringEx() + "成员、{0}{1}：分管{2}工作。\r", drow["fzzwmc"], drow["ldfzmc"], drow["ldfzfg"]);
                            }
                        }
                        dzcylist += dzcy;
                    }
                    if (string.IsNullOrEmpty(dzcylist))
                    {
                        dzcylist = " ";
                    }
                    result.Add(item["WordKey"].ToString(), dzcylist);
                }
                else if (item["WordKey"].ToString() == "#gwznlist")
                {
                    String tsqlks = "select * from Nksc_kszn where zid='" + id + "' order by sort";
                    DataTable dtks = dao.Select(tsqlks);
                    string allks = "";
                    foreach (DataRow itemks in dtks.Rows)
                    {
                        string ks = "";
                        ks += itemks["ksmc"].ToStringEx() + "\r";
                        //ks += "科室职能\r";
                        ks += itemks["kszn"].ToStringEx() + "\r";
                        //ks += "岗位职责\r";

                        DataTable gwzrdt = dao.Select("select * from Nksc_ksgwzr where id='" + itemks["id"] + "' order by sort");
                        foreach (DataRow drow in gwzrdt.Rows)
                        {
                            string gw = "";
                            //gw += "岗位名称：" + drow["ksgw"].ToStringEx() + "\r";
                            gw += drow["ksgw"].ToStringEx() + "\r";
                            //gw += "岗位责任\r";
                            gw += drow["ksgwzr"].ToStringEx() + "\r";
                            ks += gw;
                        }
                        allks += ks;
                    }
                    if (string.IsNullOrEmpty(allks))
                    {
                        allks = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allks);
                }
                else if (item["WordKey"].ToString() == "#zjzfsp0419" || item["WordKey"].ToString() == "#zjzfspqx0419")
                {
                    String tsqlje = "select * from Nksc_Zcyw where id='" + id + "' order by zcywtype,sort";
                    DataTable dtje = dao.Select(tsqlje);
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
                    if (string.IsNullOrEmpty(allje))
                    {
                        allje = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allje);
                }
                else if (item["WordKey"].ToString() == "#jkyw0419")
                {
                    String tsqlje = "select * from Nksc_Jkyw where id='" + id + "' order by sort";
                    DataTable dtje = dao.Select(tsqlje);
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
                    if (string.IsNullOrEmpty(allje))
                    {
                        allje = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allje);
                }
                else if (item["WordKey"].ToString() == "#cghtsq01")
                {
                    String tsqlje = "select * from Nksc_cghtsq where id='" + id + "' and htywtype='0' order by sort";
                    DataTable dtje = dao.Select(tsqlje);
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
                    if (string.IsNullOrEmpty(allje))
                    {
                        allje = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allje);
                }
                else if (item["WordKey"].ToString() == "#dwzxcgsq01")
                {
                    String tsqlje = "select * from Nksc_cghtsq where id='" + id + "' and htywtype='1' order by sort";
                    DataTable dtje = dao.Select(tsqlje);
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
                    if (string.IsNullOrEmpty(allje))
                    {
                        allje = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allje);
                }
                else if (item["WordKey"].ToString() == "#bxyw0419")
                {
                    String tsqlje = "select * from Nksc_Bxyw where id='" + id + "' order by sort";
                    DataTable dtje = dao.Select(tsqlje);
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
                    if (string.IsNullOrEmpty(allje))
                    {
                        allje = " ";
                    }
                    result.Add(item["WordKey"].ToString(), allje);
                }
                else if (item["WordKey"].ToString() == "#kqsjxxxx")
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["kqsjswSd"].ToStringEx())
                        || string.IsNullOrEmpty(dt.Rows[0]["kqsjswEd"].ToStringEx())
                        || string.IsNullOrEmpty(dt.Rows[0]["kqsjxwSd"].ToStringEx())
                        || string.IsNullOrEmpty(dt.Rows[0]["kqsjxwEd"].ToStringEx()))
                    {
                        string sw = dt.Rows[0]["kqsjswS"].ToStringEx() + "- " + dt.Rows[0]["kqsjswE"].ToStringEx();
                        string xw = dt.Rows[0]["kqsjxwS"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwE"].ToStringEx();
                        string neirong = "上午：" + sw + "、下午：" + xw + "";
                        result.Add(item["WordKey"].ToString(), neirong);
                    }
                    else
                    {
                        string sw = dt.Rows[0]["kqsjswS"].ToStringEx() + "- " + dt.Rows[0]["kqsjswE"].ToStringEx();
                        string xw = dt.Rows[0]["kqsjxwS"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwE"].ToStringEx();
                        string dsw = dt.Rows[0]["kqsjswSd"].ToStringEx() + "- " + dt.Rows[0]["kqsjswEd"].ToStringEx();
                        string dxw = dt.Rows[0]["kqsjxwSd"].ToStringEx() + "- " + dt.Rows[0]["kqsjxwEd"].ToStringEx();
                        string neirong = "夏令时 上午：" + sw + "、下午：" + xw + " ^p冬令时 上午：" + dsw + "、下午：" + dxw + "";
                        result.Add(item["WordKey"].ToString(), neirong);
                    }
                }
                else if (item["WordKey"].ToString() == "#jine041509")
                {
                    if (dt.Rows[0][item["DBKey"].ToString()].ToStringEx() == "0.00")
                    {
                        result.Add("（1）#jine041509元以下的零星支出。", " ");
                        result.Add("（2）出差人员必须随身携带的差旅费。", "（1）出差人员必须随身携带的差旅费。");
                        result.Add("（3）发放给职工的个人支出。", "（2）发放给职工的个人支出。");
                        result.Add("（4）根据有关规定需要支付现金的其他费用。", "（3）根据有关规定需要支付现金的其他费用。");
                    }
                    else
                    {
                        result.Add(item["WordKey"].ToString(), dt.Rows[0][item["DBKey"].ToString()].ToStringEx());
                    }
                }
                else
                {
                    result.Add(item["WordKey"].ToString(), dt.Rows[0][item["DBKey"].ToString()].ToStringEx());
                }
            }

            //财政局   上级主管区域    地区名称
            string sql = "select CzjName as xzqyName,UpName as sjzgqyName,CityName as dqmcName from View_SaleCustom where ID='" + dt.Rows[0]["CustomerID"] + "'";
            DataTable dtDep = dao.Select(sql);
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

        public Dictionary<string, ImgModel> GetKeyAndDataImg(string id, string stype)
        {
            String tsql = "select * from Nksc where id='" + id + "'";
            DataTable dt = dao.Select(tsql);
            Dictionary<string, ImgModel> result = new Dictionary<string, ImgModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 各各内控小组图
                //内部控制领导小组
                ImgModel nkldxz = new ImgModel();
                nkldxz.zz = dt.Rows[0]["nkldxzzz"].ToStringEx();
                nkldxz.fz = dt.Rows[0]["nkldxzfzz"].ToStringEx();
                nkldxz.qtks = "";//dt.Rows[0]["nkldxzqdks"].ToStringEx();
                nkldxz.cy = dt.Rows[0]["nkldxzcy"].ToStringEx();
                nkldxz.imgtype = ImgType.XZ;
                result.Add("nkldxz", nkldxz);
                //内部控制工作小组
                ImgModel nkgzxz = new ImgModel();
                nkgzxz.zz = dt.Rows[0]["nbkzgzxzzz01"].ToStringEx();
                nkgzxz.fz = dt.Rows[0]["nbkzgzxzfzz01"].ToStringEx();
                nkgzxz.qtks = dt.Rows[0]["nbkzgzxzzzqt01"].ToStringEx();
                nkgzxz.cy = dt.Rows[0]["nbkzgzxzzzcy01"].ToStringEx();
                nkgzxz.imgtype = ImgType.XZ;
                result.Add("nkgzxz", nkgzxz);
                //风险评估小组
                ImgModel fxpgxz = new ImgModel();
                fxpgxz.zz = dt.Rows[0]["fxpgxzzz"].ToStringEx();
                fxpgxz.fz = dt.Rows[0]["fxpgxzfzz"].ToStringEx();
                fxpgxz.qtks = dt.Rows[0]["fxpgxzqtks"].ToStringEx();
                fxpgxz.cy = dt.Rows[0]["fxpgxzcy"].ToStringEx();
                fxpgxz.imgtype = ImgType.XZ;
                result.Add("fxpgxz", fxpgxz);
                //预算管理领导小组
                ImgModel ysldxz = new ImgModel();
                ysldxz.zz = dt.Rows[0]["ysldxzzz"].ToStringEx();
                ysldxz.fz = dt.Rows[0]["ysldxzfzz"].ToStringEx();
                ysldxz.qtks = dt.Rows[0]["ysldxzqdks"].ToStringEx();
                ysldxz.cy = dt.Rows[0]["ysldxzcy"].ToStringEx();
                ysldxz.imgtype = ImgType.XZ;
                result.Add("ysldxz", ysldxz);
                //政府采购领导小组
                ImgModel zfcgxz = new ImgModel();
                zfcgxz.zz = dt.Rows[0]["zfcgxzzz"].ToStringEx();
                zfcgxz.fz = dt.Rows[0]["zfcgxzfzz"].ToStringEx();
                zfcgxz.qtks = dt.Rows[0]["zfcgxzqdks"].ToStringEx();
                zfcgxz.cy = dt.Rows[0]["zfcgxzcy"].ToStringEx();
                zfcgxz.imgtype = ImgType.XZ;
                result.Add("zfcgxz", zfcgxz);
                //国有资产管理领导小组
                ImgModel gyzcxz = new ImgModel();
                gyzcxz.zz = dt.Rows[0]["gyzcxzzz"].ToStringEx();
                gyzcxz.fz = dt.Rows[0]["gyzcxzfzz"].ToStringEx();
                gyzcxz.qtks = dt.Rows[0]["gyzcxzqdks"].ToStringEx();
                gyzcxz.cy = dt.Rows[0]["gyzcxzcy"].ToStringEx();
                gyzcxz.imgtype = ImgType.XZ;
                result.Add("gyzcxz", gyzcxz);
                //监督检查工作小组
                ImgModel jdjcxz = new ImgModel();
                jdjcxz.zz = dt.Rows[0]["jdjcxzzz"].ToStringEx();
                jdjcxz.fz = dt.Rows[0]["jdjcxzfzz"].ToStringEx();
                jdjcxz.qtks = dt.Rows[0]["jdjcxzqdks"].ToStringEx();
                jdjcxz.cy = dt.Rows[0]["jdjcxzcy"].ToStringEx();
                jdjcxz.imgtype = ImgType.XZ;
                result.Add("jdjcxz", jdjcxz);
                #endregion

                #region 组织架构图
                ImgModel dwzzjg = new ImgModel();
                //副职职务名称1      副职领导姓名1       副职领导1分管科室
                if (dt.Rows[0]["fzzwmc1"].ToStringEx() != "" && dt.Rows[0]["ldfzmc1"].ToStringEx() != "")
                {
                    //正职
                    dwzzjg.zz = dt.Rows[0]["zzzwmc"].ToStringEx();
                    dwzzjg.zzfgs = new ImageName();
                    dwzzjg.zzfgs.Name = "分管科室";
                    dwzzjg.zzfgs.Childs = new List<ImageName>();
                    string ldzzfg = dt.Rows[0]["ldzzfg"].ToStringEx().Replace("分管", "");
                    if (!string.IsNullOrEmpty(ldzzfg))
                    {
                        foreach (var item in ldzzfg.Split('、'))
                        {
                            ImageName img_zzfg = new ImageName() { Name = item };
                            dwzzjg.zzfgs.Childs.Add(img_zzfg);
                        }
                    }
                    //副职
                    dwzzjg.fzzlist = new List<ImageName>();
                    ImageName img_fz = new ImageName();
                    img_fz.Name = dt.Rows[0]["fzzwmc1"].ToStringEx();
                    img_fz.Childs = new List<ImageName>();
                    string ldfzfg1 = dt.Rows[0]["ldfzfg1"].ToStringEx();
                    if (!string.IsNullOrEmpty(ldfzfg1))
                    {
                        foreach (var item in ldfzfg1.Split('、'))
                        {
                            ImageName img_zzfg = new ImageName() { Name = item };
                            img_fz.Childs.Add(img_zzfg);
                        }
                    }
                    else
                    {
                        ImageName img_zzfg = new ImageName() { Name = "" };
                        img_fz.Childs.Add(img_zzfg);
                    }
                    dwzzjg.fzzlist.Add(img_fz);

                    string tsql_fz = "select * from Nksc_fz where id='" + id + "' order by sort";
                    DataTable dt_fz = dao.Select(tsql_fz);
                    for (int i = 0; i < dt_fz.Rows.Count; i++)
                    {
                        ImageName img_fz2 = new ImageName();
                        img_fz2.Name = dt_fz.Rows[i]["fzzwmc"].ToStringEx();
                        img_fz2.Childs = new List<ImageName>();
                        string ldfzfg2 = dt_fz.Rows[i]["ldfzfg"].ToStringEx();
                        if (!string.IsNullOrEmpty(ldfzfg2))
                        {
                            foreach (var item in ldfzfg2.Split('、'))
                            {
                                ImageName img_zzfg = new ImageName() { Name = item };
                                img_fz2.Childs.Add(img_zzfg);
                            }
                        }
                        else
                        {
                            ImageName img_zzfg = new ImageName() { Name = "" };
                            img_fz2.Childs.Add(img_zzfg);
                        }
                        dwzzjg.fzzlist.Add(img_fz2);
                    }
                }
                else
                {
                    //正职
                    dwzzjg.zz = dt.Rows[0]["zzzwmc"].ToStringEx();
                    dwzzjg.zzfgs = new ImageName();
                    dwzzjg.zzfgs.Childs = new List<ImageName>();
                    //副职
                    dwzzjg.fzzlist = new List<ImageName>();
                    ImageName img_fz = new ImageName();
                    img_fz.Name = "分管科室";
                    img_fz.Childs = new List<ImageName>();
                    string ldzzfg = dt.Rows[0]["ldzzfg"].ToStringEx().Replace("分管", "");
                    if (!string.IsNullOrEmpty(ldzzfg))
                    {
                        foreach (var item in ldzzfg.Split('、'))
                        {
                            ImageName img_zzfg = new ImageName() { Name = item };
                            img_fz.Childs.Add(img_zzfg);
                        }
                    }
                    dwzzjg.fzzlist.Add(img_fz);
                }
                dwzzjg.imgtype = ImgType.ZZJG;
                result.Add("dwzzjg", dwzzjg);
                #endregion

                #region 业务流程图

                Font HT10 = new Font("黑体", 10);
                Font HT11 = new Font("黑体", 11);
                Font HT12 = new Font("黑体", 12);
                Font HT13 = new Font("黑体", 13);
                Font HT14 = new Font("黑体", 14);
                string ksbm = stype == "1" ? "部门" : "科室";

                //1、 第二部分-第二章-第二节 借款业务流程（#zzzwmc）
                ImgModel img_jkywlc = new ImgModel();
                img_jkywlc.ImgFileName = "img_jkywlc.jpg";
                img_jkywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), ksbm + "负责人", "主管财务的" + ksbm };
                img_jkywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14, HT14 };
                img_jkywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(511, 39, 140, 55), new Rectangle(199, 39, 145, 55), new Rectangle(346, 39, 165, 55) };
                img_jkywlc.imgtype = ImgType.LCT;
                result.Add("img_jkywlc", img_jkywlc);

                //（模板中移除）2、 第二部分-第二章-第一节 预算内支出业务流程（3处 #srywgkks #zzzwmc #zzzwmc）
                //ImgModel img_ysnzcywlc = new ImgModel();
                //img_ysnzcywlc.ImgFileName = "img_ysnzcywlc.jpg";
                //img_ysnzcywlc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会", "本" + ksbm + "负责人核对、签字" };
                //img_ysnzcywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14, HT14, HT10 };
                //img_ysnzcywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(189, 39, 146, 55), new Rectangle(336, 39, 165, 55), new Rectangle(499, 39, 151, 55), new Rectangle(68, 222, 95, 60) };
                //img_ysnzcywlc.imgtype = ImgType.LCT;
                //result.Add("img_ysnzcywlc", img_ysnzcywlc);

                //2、 第二部分-第六章-第二节 合同业务管理流程（#zzzwmc）
                ImgModel img_htywlc = new ImgModel();
                img_htywlc.ImgFileName = "img_htywlc.jpg";
                img_htywlc.ImgTitle = new List<string>() { dt.Rows[0]["htgkks1"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会", "主管业务" + ksbm };
                img_htywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12, HT12 };
                img_htywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(204, 39, 164, 48), new Rectangle(531, 39, 163, 48), new Rectangle(43, 39, 163, 48) };
                img_htywlc.imgtype = ImgType.LCT;
                result.Add("img_htywlc", img_htywlc);

                //3、 第二部分-第七章-第二节 印章交接业务流程（#zzzwmc）
                ImgModel img_yzjjywlc = new ImgModel();
                img_yzjjywlc.ImgFileName = "img_yzjjywlc.jpg";
                img_yzjjywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), ksbm + "负责人" };
                img_yzjjywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_yzjjywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(475, 39, 226, 50), new Rectangle(257, 39, 228, 50) };
                img_yzjjywlc.imgtype = ImgType.LCT;
                result.Add("img_yzjjywlc", img_yzjjywlc);

                //4、 第二部分-第七章-第二节 印章刻制业务流程（#zzzwmc）
                ImgModel img_yzkzywlc = new ImgModel();
                img_yzkzywlc.ImgFileName = "img_yzkzywlc.jpg";
                img_yzkzywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), dt.Rows[0]["yzglgkks"].ToStringEx(), ksbm + "负责人" };
                img_yzkzywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12, HT12 };
                img_yzkzywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(379, 39, 170, 47), new Rectangle(550, 39, 170, 47), new Rectangle(214, 39, 170, 50) };
                img_yzkzywlc.imgtype = ImgType.LCT;
                result.Add("img_yzkzywlc", img_yzkzywlc);

                //5、 第二部分-第七章-第二节 印章使用业务流程（#zzzwmc）
                ImgModel img_yzsyywlc = new ImgModel();
                img_yzsyywlc.ImgFileName = "img_yzsyywlc.jpg";
                img_yzsyywlc.ImgTitle = new List<string>() { dt.Rows[0]["yzglgkks"].ToStringEx() + "负责人", dt.Rows[0]["zzzwmc"].ToStringEx(), ksbm + "负责人" };
                img_yzsyywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12, HT12 };
                img_yzsyywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(407, 39, 192, 50), new Rectangle(590, 39, 180, 50), new Rectangle(226, 39, 180, 50) };
                img_yzsyywlc.imgtype = ImgType.LCT;
                result.Add("img_yzsyywlc", img_yzsyywlc);

                //6、 第二部分-第七章-第二节 印章停用业务流程（#zzzwmc）
                ImgModel img_yztyywlc = new ImgModel();
                img_yztyywlc.ImgFileName = "img_yztyywlc.jpg";
                img_yztyywlc.ImgTitle = new List<string>() { dt.Rows[0]["yzglgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() };
                img_yztyywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_yztyywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(214, 39, 170, 50), new Rectangle(384, 39, 170, 50) };
                img_yztyywlc.imgtype = ImgType.LCT;
                result.Add("img_yztyywlc", img_yztyywlc);

                //7、 第二部分-第三章-第六节 办公用品购买业务流程（#zzzwmc）
                ImgModel img_bgypgmywlc = new ImgModel();
                img_bgypgmywlc.ImgFileName = "img_bgypgmywlc.jpg";
                img_bgypgmywlc.ImgTitle = new List<string>() { "申请" + ksbm, ksbm + "负责人", dt.Rows[0]["bgypglgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() };
                img_bgypgmywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14, HT14, HT14 };
                img_bgypgmywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(45, 39, 127, 55), new Rectangle(173, 39, 127, 55), new Rectangle(301, 39, 127, 55), new Rectangle(553, 39, 127, 55) };
                img_bgypgmywlc.imgtype = ImgType.LCT;
                result.Add("img_bgypgmywlc", img_bgypgmywlc);

                //8、第二部分-第三章-第五节 政府采购业务流程（2处 #zfcgzlgkks #zfcgxzqdks）
                ImgModel img_zfcgywlct = new ImgModel();
                img_zfcgywlct.ImgFileName = "img_zfcgywlct.jpg";
                img_zfcgywlct.ImgTitle = new List<string>() { dt.Rows[0]["zfcgzlgkks"].ToStringEx(), dt.Rows[0]["zfcgxzqdks"].ToStringEx(), "相关业务" + ksbm, ksbm + "负责人" };
                img_zfcgywlct.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14, HT14, HT14 };
                img_zfcgywlct.ImgTitleRect = new List<Rectangle>() { new Rectangle(301, 39, 128, 67), new Rectangle(430, 39, 126, 67), new Rectangle(44, 39, 129, 67), new Rectangle(173, 39, 128, 67) };
                img_zfcgywlct.imgtype = ImgType.LCT;
                result.Add("img_zfcgywlct", img_zfcgywlct);

                //9、 第二部分-第三章-第五节 政府采购质疑处理流程（2处 #zfcgxzqdks #zzzwmc）
                ImgModel img_zfcgzycllc = new ImgModel();
                img_zfcgzycllc.ImgFileName = "img_zfcgzycllc.jpg";
                img_zfcgzycllc.ImgTitle = new List<string>() { dt.Rows[0]["zfcgxzqdks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() };
                img_zfcgzycllc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_zfcgzycllc.ImgTitleRect = new List<Rectangle>() { new Rectangle(218, 39, 160, 50), new Rectangle(379, 39, 162, 50) };
                img_zfcgzycllc.imgtype = ImgType.LCT;
                result.Add("img_zfcgzycllc", img_zfcgzycllc);

                //10、 第二部分-第四章-第八节 固定资产处置业务流程（#zzzwmc）
                ImgModel img_gdzcczywlc = new ImgModel();
                img_gdzcczywlc.ImgFileName = "img_gdzcczywlc.jpg";
                img_gdzcczywlc.ImgTitle = new List<string>() { dt.Rows[0]["gdzccz"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx(), "申请" + ksbm, "调入" + ksbm, ksbm + "负责人审核签字", ksbm + "负责人签字确认" };
                img_gdzcczywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT11, HT11, HT11, HT11, HT11, HT11 };
                img_gdzcczywlc.ImgTitleRect = new List<Rectangle>() {new Rectangle(297, 39, 129, 48), new Rectangle(681, 39, 129, 48)
                                                                , new Rectangle(39, 39, 129, 48) , new Rectangle(167, 39, 129, 48)
                                                                , new Rectangle(62, 213, 85, 36) , new Rectangle(196, 392, 82, 36)};
                img_gdzcczywlc.imgtype = ImgType.LCT;
                result.Add("img_gdzcczywlc", img_gdzcczywlc);

                //11、 第二部分-第四章-第九节 固定资产清查业务流程（#zzzwmc）
                ImgModel img_gdzcqcywlc = new ImgModel();
                img_gdzcqcywlc.ImgFileName = "img_gdzcqcywlc.jpg";
                img_gdzcqcywlc.ImgTitle = new List<string>() { dt.Rows[0]["gdzcqc"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx(), "使用" + ksbm };
                img_gdzcqcywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT11, HT11, HT11 };
                img_gdzcqcywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(173, 39, 128, 48), new Rectangle(558, 39, 130, 48), new Rectangle(45, 39, 128, 48) };
                img_gdzcqcywlc.imgtype = ImgType.LCT;
                result.Add("img_gdzcqcywlc", img_gdzcqcywlc);

                //12、 第二部分-第四章-第六节 固定资产购置业务流程（#zzzwmc）
                ImgModel img_gdzcgzywlc = new ImgModel();
                img_gdzcgzywlc.ImgFileName = "img_gdzcgzywlc.jpg";
                img_gdzcgzywlc.ImgTitle = new List<string>() { dt.Rows[0]["gdzcgz"].ToStringEx() + ",财务", dt.Rows[0]["zzzwmc"].ToStringEx(), "使用" + ksbm, ksbm + "负责人审核签字" };
                img_gdzcgzywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT11, HT11, HT11, HT10 };
                img_gdzcgzywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(184, 39, 118, 48),new Rectangle(421, 39, 118, 48) 
                                                                , new Rectangle(45, 39, 138, 48) , new Rectangle(75, 228, 75, 48) };
                img_gdzcgzywlc.imgtype = ImgType.LCT;
                result.Add("img_gdzcgzywlc", img_gdzcgzywlc);

                //13、 第二部分-第四章-第七节 固定资产调拨业务流程（#zzzwmc）
                ImgModel img_gdzcdbywlc = new ImgModel();
                img_gdzcdbywlc.ImgFileName = "img_gdzcdbywlc.jpg";
                img_gdzcdbywlc.ImgTitle = new List<string>() { dt.Rows[0]["gdzcdb"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx(), "调出" + ksbm, "调入" + ksbm, ksbm + "负责人审核签字", ksbm + "负责人签字确认" };
                img_gdzcdbywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT10, HT10, HT10, HT10, HT10, HT10 };
                img_gdzcdbywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(297, 39, 128, 48), new Rectangle(681, 39, 130, 48)
                                                                , new Rectangle(39, 39, 129, 48) , new Rectangle(167, 39, 129, 48)
                                                                , new Rectangle(60, 213, 85, 36) , new Rectangle(196, 392, 82, 36)};
                img_gdzcdbywlc.imgtype = ImgType.LCT;
                result.Add("img_gdzcdbywlc", img_gdzcdbywlc);

                //14、 第二部分-第五章-第二节 建设项目业务管理流程（#zzzwmc）
                ImgModel img_jsxmywgllc = new ImgModel();
                img_jsxmywgllc.ImgFileName = "img_jsxmywgllc.jpg";
                img_jsxmywgllc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会", "项目主管" + ksbm };
                img_jsxmywgllc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_jsxmywgllc.ImgTitleRect = new List<Rectangle>() { new Rectangle(187, 39, 142, 50), new Rectangle(472, 39, 142, 50) };
                img_jsxmywgllc.imgtype = ImgType.LCT;
                result.Add("img_jsxmywgllc", img_jsxmywgllc);

                //15、 第二部分-第五章-第三节 建设项目付款业务流程（#zzzwmc）
                ImgModel img_jsxmfkywlc = new ImgModel();
                img_jsxmfkywlc.ImgFileName = "img_jsxmfkywlc.jpg";
                img_jsxmfkywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), "项目主管" + ksbm };
                img_jsxmfkywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_jsxmfkywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(40, 39, 126, 55), new Rectangle(300, 39, 127, 55) };
                img_jsxmfkywlc.imgtype = ImgType.LCT;
                result.Add("img_jsxmfkywlc", img_jsxmfkywlc);

                //16、 第二部分-第五章-第四节 建设项目竣工决算业务流程（#zzzwmc）
                ImgModel img_jsxmjgjsywlc = new ImgModel();
                img_jsxmjgjsywlc.ImgFileName = "img_jsxmjgjsywlc.jpg";
                img_jsxmjgjsywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), "项目主管" + ksbm };
                img_jsxmjgjsywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_jsxmjgjsywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(171, 39, 128, 53), new Rectangle(300, 39, 127, 53) };
                img_jsxmjgjsywlc.imgtype = ImgType.LCT;
                result.Add("img_jsxmjgjsywlc", img_jsxmjgjsywlc);

                //17、 第二部分-第一章-第二节 决算业务流程（#ysldxzqdks）
                ImgModel img_juesywlc = new ImgModel();
                img_juesywlc.ImgFileName = "img_juesywlc.jpg";
                img_juesywlc.ImgTitle = new List<string>() { dt.Rows[0]["ysldxzqdks"].ToStringEx(), "业务" + ksbm };
                img_juesywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_juesywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(212, 39, 172, 50), new Rectangle(385, 39, 172, 50) };
                img_juesywlc.imgtype = ImgType.LCT;
                result.Add("img_juesywlc", img_juesywlc);

                //18、 第二部分-第一章-第二节 预算调整业务流程（#ysldxzqdks）
                ImgModel img_ystzywlc = new ImgModel();
                img_ystzywlc.ImgFileName = "img_ystzywlc.jpg";
                img_ystzywlc.ImgTitle = new List<string>() { dt.Rows[0]["ysldxzqdks"].ToStringEx(), "业务" + ksbm };
                img_ystzywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_ystzywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(233, 39, 170, 49), new Rectangle(45, 39, 158, 49) };
                img_ystzywlc.imgtype = ImgType.LCT;
                result.Add("img_ystzywlc", img_ystzywlc);

                //19、 第二部分-第一章-第一节 预算编制、审批、执行业务流程图（#ysldxzqdks）
                ImgModel img_ysbzspzxywlc = new ImgModel();
                img_ysbzspzxywlc.ImgFileName = "img_ysbzspzxywlc.jpg";
                img_ysbzspzxywlc.ImgTitle = new List<string>() { dt.Rows[0]["ysldxzqdks"].ToStringEx(), "业务" + ksbm };
                img_ysbzspzxywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_ysbzspzxywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(202, 39, 158, 49), new Rectangle(44, 39, 158, 49) };
                img_ysbzspzxywlc.imgtype = ImgType.LCT;
                result.Add("img_ysbzspzxywlc", img_ysbzspzxywlc);

                //20、 第一部分-第九章-机关运转管理制度--档案管理业务流程（#zzzwmc）
                ImgModel img_daglywlc = new ImgModel();
                img_daglywlc.ImgFileName = "img_daglywlc.jpg";
                img_daglywlc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), ksbm + "负责人" };
                img_daglywlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_daglywlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(193, 39, 161, 47), new Rectangle(355, 39, 161, 47) };
                img_daglywlc.imgtype = ImgType.LCT;
                result.Add("img_daglywlc", img_daglywlc);

                //21、 第一部分-第三章-第四节 风险评估工作流程（#zzzwmc）
                ImgModel img_fxpggzlc = new ImgModel();
                img_fxpggzlc.ImgFileName = "img_fxpggzlc.jpg";
                img_fxpggzlc.ImgTitle = new List<string>() { "各个" + ksbm, dt.Rows[0]["zzzwmc"].ToStringEx() };
                img_fxpggzlc.ImgTitleFont = new List<System.Drawing.Font>() { HT12, HT12 };
                img_fxpggzlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(214, 39, 168, 63), new Rectangle(548, 39, 168, 63) };
                img_fxpggzlc.imgtype = ImgType.LCT;
                result.Add("img_fxpggzlc", img_fxpggzlc);

                //22、 第一部分-第四章-第五节 集体议事决策流程图（2处（#zzzwmc）
                ImgModel img_jtysjclc = new ImgModel();
                img_jtysjclc.ImgFileName = "img_jtysjclc.jpg";
                img_jtysjclc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会", "业务" + ksbm, "监督" + ksbm };
                img_jtysjclc.ImgTitleFont = new List<System.Drawing.Font>() { HT11, HT11, HT11, HT11 };
                img_jtysjclc.ImgTitleRect = new List<Rectangle>() { new Rectangle(168, 39, 128, 47), new Rectangle(297, 39, 150, 47), new Rectangle(449, 39, 133, 47), new Rectangle(582, 39, 150, 47) };
                img_jtysjclc.imgtype = ImgType.LCT;
                result.Add("img_jtysjclc", img_jtysjclc);

                //23、 第一部分-第五章-第四节 内部报告审批流程（#zzzwmc）
                ImgModel img_nkbgsplc = new ImgModel();
                img_nkbgsplc.ImgFileName = "img_nkbgsplc.jpg";
                img_nkbgsplc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会", ksbm + "负责人", ksbm + "岗位", "相关" + ksbm };
                img_nkbgsplc.ImgTitleFont = new List<System.Drawing.Font>() { HT10, HT10, HT10, HT10 };
                img_nkbgsplc.ImgTitleRect = new List<Rectangle>() { new Rectangle(31, 32, 103, 39), new Rectangle(238, 32, 103, 39), new Rectangle(342, 32, 103, 39), new Rectangle(445, 32, 103, 39) };
                img_nkbgsplc.imgtype = ImgType.LCT;
                result.Add("img_nkbgsplc", img_nkbgsplc);

                #region 新增流程图

                //24政府采购方式变更申请及审批流程（2处 #srywgkks #zzzwmc办公会）
                ImgModel img_zfcgbgsqsplc = new ImgModel();
                img_zfcgbgsqsplc.ImgFileName = "img_zfcgbgsqsplc.jpg";
                img_zfcgbgsqsplc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会" };
                img_zfcgbgsqsplc.ImgTitleFont = new List<System.Drawing.Font>() { HT13, HT13 };
                img_zfcgbgsqsplc.ImgTitleRect = new List<Rectangle>() { new Rectangle(190, 39, 151, 47), new Rectangle(492, 39, 151, 47) };
                img_zfcgbgsqsplc.imgtype = ImgType.LCT;
                result.Add("img_zfcgbgsqsplc", img_zfcgbgsqsplc);

                //25政府购买服务流程（0处 ）
                ImgModel img_zfcggmfwlc = new ImgModel();
                img_zfcggmfwlc.ImgFileName = "img_zfcggmfwlc.jpg";
                img_zfcggmfwlc.ImgTitle = new List<string>();
                img_zfcggmfwlc.ImgTitleFont = null;
                img_zfcggmfwlc.ImgTitleRect = null;
                img_zfcggmfwlc.imgtype = ImgType.LCT;
                result.Add("img_zfcggmfwlc", img_zfcggmfwlc);

                //26政府采购六大方式业务流程（0处 ）
                ImgModel img_zfcgldywlc = new ImgModel();
                img_zfcgldywlc.ImgFileName = "img_zfcgldywlc.jpg";
                img_zfcgldywlc.ImgTitle = new List<string>();
                img_zfcgldywlc.ImgTitleFont = null;
                img_zfcgldywlc.ImgTitleRect = null;
                img_zfcgldywlc.imgtype = ImgType.LCT;
                result.Add("img_zfcgldywlc", img_zfcgldywlc);

                //27年度政府采购实施计划申报及审批流程（2处 #srywgkks #zzzwmc办公会）
                ImgModel img_zfcgssjhsbsplc = new ImgModel();
                img_zfcgssjhsbsplc.ImgFileName = "img_zfcgssjhsbsplc.jpg";
                img_zfcgssjhsbsplc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会" };
                img_zfcgssjhsbsplc.ImgTitleFont = new List<System.Drawing.Font>() { HT13, HT13 };
                img_zfcgssjhsbsplc.ImgTitleRect = new List<Rectangle>() { new Rectangle(341, 39, 151, 47), new Rectangle(643, 39, 152, 47) };
                img_zfcgssjhsbsplc.imgtype = ImgType.LCT;
                result.Add("img_zfcgssjhsbsplc", img_zfcgssjhsbsplc);



                //28建设项目档案管理流程（0处 ）
                ImgModel img_jsxmdagllc = new ImgModel();
                img_jsxmdagllc.ImgFileName = "img_jsxmdagllc.jpg";
                img_jsxmdagllc.ImgTitle = new List<string>();
                img_jsxmdagllc.ImgTitleFont = null;
                img_jsxmdagllc.ImgTitleRect = null;
                img_jsxmdagllc.imgtype = ImgType.LCT;
                result.Add("img_jsxmdagllc", img_jsxmdagllc);

                //29建设项目公开招标、邀请招标流程（0处 ）
                ImgModel img_jsxmgkzbyqzblc = new ImgModel();
                img_jsxmgkzbyqzblc.ImgFileName = "img_jsxmgkzbyqzblc.jpg";
                img_jsxmgkzbyqzblc.ImgTitle = new List<string>();
                img_jsxmgkzbyqzblc.ImgTitleFont = null;
                img_jsxmgkzbyqzblc.ImgTitleRect = null;
                img_jsxmgkzbyqzblc.imgtype = ImgType.LCT;
                result.Add("img_jsxmgkzbyqzblc", img_jsxmgkzbyqzblc);

                //30建设项目设计变更、洽商签证流程（1处 #zzzwmc办公会）
                ImgModel img_jsxmsjbgqslc = new ImgModel();
                img_jsxmsjbgqslc.ImgFileName = "img_jsxmsjbgqslc.jpg";
                img_jsxmsjbgqslc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会" };
                img_jsxmsjbgqslc.ImgTitleFont = new List<System.Drawing.Font>() { HT12 };
                img_jsxmsjbgqslc.ImgTitleRect = new List<Rectangle>() { new Rectangle(918, 38, 171, 49) };
                img_jsxmsjbgqslc.imgtype = ImgType.LCT;
                result.Add("img_jsxmsjbgqslc", img_jsxmsjbgqslc);



                //31预算三年规划编制及审批流程图
                ImgModel img_ysywsnghlct = new ImgModel();
                img_ysywsnghlct.ImgFileName = "img_ysywsnghlct.jpg";
                img_ysywsnghlct.ImgTitle = new List<string>();
                img_ysywsnghlct.ImgTitleFont = null;
                img_ysywsnghlct.ImgTitleRect = null;
                img_ysywsnghlct.imgtype = ImgType.LCT;
                result.Add("img_ysywsnghlct", img_ysywsnghlct);

                //32预算执行分析流程图
                ImgModel img_ysywzxfxlct = new ImgModel();
                img_ysywzxfxlct.ImgFileName = "img_ysywzxfxlct.jpg";
                img_ysywzxfxlct.ImgTitle = new List<string>();
                img_ysywzxfxlct.ImgTitleFont = null;
                img_ysywzxfxlct.ImgTitleRect = null;
                img_ysywzxfxlct.imgtype = ImgType.LCT;
                result.Add("img_ysywzxfxlct", img_ysywzxfxlct);

                //33预算绩效评价流程图
                ImgModel img_ysywjxpjlc = new ImgModel();
                img_ysywjxpjlc.ImgFileName = "img_ysywjxpjlc.jpg";
                img_ysywjxpjlc.ImgTitle = new List<string>();
                img_ysywjxpjlc.ImgTitleFont = null;
                img_ysywjxpjlc.ImgTitleRect = null;
                img_ysywjxpjlc.imgtype = ImgType.LCT;
                result.Add("img_ysywjxpjlc", img_ysywjxpjlc);



                //34国有资产配置计划申报及审批流程
                ImgModel img_zcywpzjhsbsplc = new ImgModel();
                img_zcywpzjhsbsplc.ImgFileName = "img_zcywpzjhsbsplc.jpg";
                img_zcywpzjhsbsplc.ImgTitle = new List<string>();
                img_zcywpzjhsbsplc.ImgTitleFont = null;
                img_zcywpzjhsbsplc.ImgTitleRect = null;
                img_zcywpzjhsbsplc.imgtype = ImgType.LCT;
                result.Add("img_zcywpzjhsbsplc", img_zcywpzjhsbsplc);

                //35国有资产出租、出借业务
                ImgModel img_zcywczcjywlc = new ImgModel();
                img_zcywczcjywlc.ImgFileName = "img_zcywczcjywlc.jpg";
                img_zcywczcjywlc.ImgTitle = new List<string>();
                img_zcywczcjywlc.ImgTitleFont = null;
                img_zcywczcjywlc.ImgTitleRect = null;
                img_zcywczcjywlc.imgtype = ImgType.LCT;
                result.Add("img_zcywczcjywlc", img_zcywczcjywlc);

                //36国有资产收入上缴流程
                ImgModel img_zcywsrsjlc = new ImgModel();
                img_zcywsrsjlc.ImgFileName = "img_zcywsrsjlc.jpg";
                img_zcywsrsjlc.ImgTitle = new List<string>();
                img_zcywsrsjlc.ImgTitleFont = null;
                img_zcywsrsjlc.ImgTitleRect = null;
                img_zcywsrsjlc.imgtype = ImgType.LCT;
                result.Add("img_zcywsrsjlc", img_zcywsrsjlc);

                //37国有资产产权登记、变更、注销
                ImgModel img_zcywcqdjbgzxlc = new ImgModel();
                img_zcywcqdjbgzxlc.ImgFileName = "img_zcywcqdjbgzxlc.jpg";
                img_zcywcqdjbgzxlc.ImgTitle = new List<string>();
                img_zcywcqdjbgzxlc.ImgTitleFont = null;
                img_zcywcqdjbgzxlc.ImgTitleRect = null;
                img_zcywcqdjbgzxlc.imgtype = ImgType.LCT;
                result.Add("img_zcywcqdjbgzxlc", img_zcywcqdjbgzxlc);

                //38国有资产产权纠纷调处流程
                ImgModel img_zcywcqjfdclc = new ImgModel();
                img_zcywcqjfdclc.ImgFileName = "img_zcywcqjfdclc.jpg";
                img_zcywcqjfdclc.ImgTitle = new List<string>();
                img_zcywcqjfdclc.ImgTitleFont = null;
                img_zcywcqjfdclc.ImgTitleRect = null;
                img_zcywcqjfdclc.imgtype = ImgType.LCT;
                result.Add("img_zcywcqjfdclc", img_zcywcqjfdclc);

                //39国有资产评估流程（1处 #zzzwmc班子会）
                ImgModel img_zcywpglc = new ImgModel();
                img_zcywpglc.ImgFileName = "img_zcywpglc.jpg";
                img_zcywpglc.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx() + "班子会" };
                img_zcywpglc.ImgTitleFont = new List<System.Drawing.Font>() { HT14 };
                img_zcywpglc.ImgTitleRect = new List<Rectangle>() { new Rectangle(439, 38, 161, 49) };
                img_zcywpglc.imgtype = ImgType.LCT;
                result.Add("img_zcywpglc", img_zcywpglc);

                //40国有资产年度报告编制及审批流程
                ImgModel img_zcywndbgbzshlc = new ImgModel();
                img_zcywndbgbzshlc.ImgFileName = "img_zcywndbgbzshlc.jpg";
                img_zcywndbgbzshlc.ImgTitle = new List<string>();
                img_zcywndbgbzshlc.ImgTitleFont = null;
                img_zcywndbgbzshlc.ImgTitleRect = null;
                img_zcywndbgbzshlc.imgtype = ImgType.LCT;
                result.Add("img_zcywndbgbzshlc", img_zcywndbgbzshlc);



                //41财政票据核销流程
                ImgModel img_szywczpjhxlc = new ImgModel();
                img_szywczpjhxlc.ImgFileName = "img_szywczpjhxlc.jpg";
                img_szywczpjhxlc.ImgTitle = new List<string>();
                img_szywczpjhxlc.ImgTitleFont = null;
                img_szywczpjhxlc.ImgTitleRect = null;
                img_szywczpjhxlc.imgtype = ImgType.LCT;
                result.Add("img_szywczpjhxlc", img_szywczpjhxlc);

                //42财政授权支付申请审批流程
                ImgModel img_szywczsqzfsqlc = new ImgModel();
                img_szywczsqzfsqlc.ImgFileName = "img_szywczsqzfsqlc.jpg";
                img_szywczsqzfsqlc.ImgTitle = new List<string>();
                img_szywczsqzfsqlc.ImgTitleFont = null;
                img_szywczsqzfsqlc.ImgTitleRect = null;
                img_szywczsqzfsqlc.imgtype = ImgType.LCT;
                result.Add("img_szywczsqzfsqlc", img_szywczsqzfsqlc);

                //43财政直接支付申请审批流程
                ImgModel img_szywczzjjfsqlc = new ImgModel();
                img_szywczzjjfsqlc.ImgFileName = "img_szywczzjjfsqlc.jpg";
                img_szywczzjjfsqlc.ImgTitle = new List<string>();
                img_szywczzjjfsqlc.ImgTitleFont = null;
                img_szywczzjjfsqlc.ImgTitleRect = null;
                img_szywczzjjfsqlc.imgtype = ImgType.LCT;
                result.Add("img_szywczzjjfsqlc", img_szywczzjjfsqlc);

                //44非税收入收取及缴款流程
                ImgModel img_szywfssrsqlc = new ImgModel();
                img_szywfssrsqlc.ImgFileName = "img_szywfssrsqlc.jpg";
                img_szywfssrsqlc.ImgTitle = new List<string>();
                img_szywfssrsqlc.ImgTitleFont = null;
                img_szywfssrsqlc.ImgTitleRect = null;
                img_szywfssrsqlc.imgtype = ImgType.LCT;
                result.Add("img_szywfssrsqlc", img_szywfssrsqlc);

                //45公务卡报销和还款流程
                ImgModel img_szywgwkbxhklc = new ImgModel();
                img_szywgwkbxhklc.ImgFileName = "img_szywgwkbxhklc.jpg";
                img_szywgwkbxhklc.ImgTitle = new List<string>();
                img_szywgwkbxhklc.ImgTitleFont = null;
                img_szywgwkbxhklc.ImgTitleRect = null;
                img_szywgwkbxhklc.imgtype = ImgType.LCT;
                result.Add("img_szywgwkbxhklc", img_szywgwkbxhklc);

                //46公务卡申领及注销流程
                ImgModel img_szywgwkslzxlc = new ImgModel();
                img_szywgwkslzxlc.ImgFileName = "img_szywgwkslzxlc.jpg";
                img_szywgwkslzxlc.ImgTitle = new List<string>();
                img_szywgwkslzxlc.ImgTitleFont = null;
                img_szywgwkslzxlc.ImgTitleRect = null;
                img_szywgwkslzxlc.imgtype = ImgType.LCT;
                result.Add("img_szywgwkslzxlc", img_szywgwkslzxlc);

                //47会计档案建立、保管、借阅、销毁
                ImgModel img_szywkjdajlbgjyxhlc = new ImgModel();
                img_szywkjdajlbgjyxhlc.ImgFileName = "img_szywkjdajlbgjyxhlc.jpg";
                img_szywkjdajlbgjyxhlc.ImgTitle = new List<string>();
                img_szywkjdajlbgjyxhlc.ImgTitleFont = null;
                img_szywkjdajlbgjyxhlc.ImgTitleRect = null;
                img_szywkjdajlbgjyxhlc.imgtype = ImgType.LCT;
                result.Add("img_szywkjdajlbgjyxhlc", img_szywkjdajlbgjyxhlc);

                //48收入、支出分析报告审批流程
                ImgModel img_szywsrzcfxbgsplc = new ImgModel();
                img_szywsrzcfxbgsplc.ImgFileName = "img_szywsrzcfxbgsplc.jpg";
                img_szywsrzcfxbgsplc.ImgTitle = new List<string>();
                img_szywsrzcfxbgsplc.ImgTitleFont = null;
                img_szywsrzcfxbgsplc.ImgTitleRect = null;
                img_szywsrzcfxbgsplc.imgtype = ImgType.LCT;
                result.Add("img_szywsrzcfxbgsplc", img_szywsrzcfxbgsplc);

                //49收入登记入账流程（2处 #srywgkks会计 #srywgkks出纳）
                ImgModel img_szywsrdjrzlc = new ImgModel();
                img_szywsrdjrzlc.ImgFileName = "img_szywsrdjrzlc.jpg";
                img_szywsrdjrzlc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx() + "会计", dt.Rows[0]["srywgkks"].ToStringEx() + "出纳" };
                img_szywsrdjrzlc.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14 };
                img_szywsrdjrzlc.ImgTitleRect = new List<Rectangle>() { new Rectangle(181, 39, 146, 53), new Rectangle(327, 39, 165, 53) };
                img_szywsrdjrzlc.imgtype = ImgType.LCT;
                result.Add("img_szywsrdjrzlc", img_szywsrdjrzlc);

                //50银行账户变更、撤销申请及备案流程
                ImgModel img_szywyhzhbgcxsqlc = new ImgModel();
                img_szywyhzhbgcxsqlc.ImgFileName = "img_szywyhzhbgcxsqlc.jpg";
                img_szywyhzhbgcxsqlc.ImgTitle = new List<string>();
                img_szywyhzhbgcxsqlc.ImgTitleFont = null;
                img_szywyhzhbgcxsqlc.ImgTitleRect = null;
                img_szywyhzhbgcxsqlc.imgtype = ImgType.LCT;
                result.Add("img_szywyhzhbgcxsqlc", img_szywyhzhbgcxsqlc);

                //51银行账户开立申请及审批流程
                ImgModel img_szywyhzhklsqsplc = new ImgModel();
                img_szywyhzhklsqsplc.ImgFileName = "img_szywyhzhklsqsplc.jpg";
                img_szywyhzhklsqsplc.ImgTitle = new List<string>();
                img_szywyhzhklsqsplc.ImgTitleFont = null;
                img_szywyhzhklsqsplc.ImgTitleRect = null;
                img_szywyhzhklsqsplc.imgtype = ImgType.LCT;
                result.Add("img_szywyhzhklsqsplc", img_szywyhzhklsqsplc);

                //52用款计划申请审批流程（1处 #srywgkks）
                ImgModel img_szywykjhsqsplc = new ImgModel();
                img_szywykjhsqsplc.ImgFileName = "img_szywykjhsqsplc.jpg";
                img_szywykjhsqsplc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx() };
                img_szywykjhsqsplc.ImgTitleFont = new List<System.Drawing.Font>() { HT14 };
                img_szywykjhsqsplc.ImgTitleRect = new List<Rectangle>() { new Rectangle(181, 38, 146, 55) };
                img_szywykjhsqsplc.imgtype = ImgType.LCT;
                result.Add("img_szywykjhsqsplc", img_szywykjhsqsplc);


                //53支出事前申请审批流程（3处 #srywgkks #zzzwmc #zzzwmc办公会）
                ImgModel img_szywzcsqsqsplc = new ImgModel();
                img_szywzcsqsqsplc.ImgFileName = "img_szywzcsqsqsplc.jpg";
                img_szywzcsqsqsplc.ImgTitle = new List<string>() { dt.Rows[0]["srywgkks"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx(), dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会" };
                img_szywzcsqsqsplc.ImgTitleFont = new List<System.Drawing.Font>() { HT14, HT14, HT14 };
                img_szywzcsqsqsplc.ImgTitleRect = new List<Rectangle>() { new Rectangle(181, 39, 146, 53), new Rectangle(492, 39, 151, 53), new Rectangle(643, 39, 150, 53) };
                img_szywzcsqsqsplc.imgtype = ImgType.LCT;
                result.Add("img_szywzcsqsqsplc", img_szywzcsqsqsplc);

                //54单位内部控制组织架构图（1处 #zzzwmc）
                ImgModel img_dwnbkzzzjgt = new ImgModel();
                img_dwnbkzzzjgt.ImgFileName = "img_dwnbkzzzjgt.jpg";
                img_dwnbkzzzjgt.ImgTitle = new List<string>() { dt.Rows[0]["zzzwmc"].ToStringEx() + "办公会" };
                img_dwnbkzzzjgt.ImgTitleFont = new List<System.Drawing.Font>() { HT14 };
                img_dwnbkzzzjgt.ImgTitleRect = new List<Rectangle>() { new Rectangle(400, 3, 220, 47) };
                img_dwnbkzzzjgt.imgtype = ImgType.LCT;
                result.Add("img_dwnbkzzzjgt", img_dwnbkzzzjgt);
                #endregion

                #endregion
            }
            return result;
        }

        #endregion

        #region 问题反馈
        public int Insert_wtfk(Nksc_wtfk model)
        {
            return dao.Insert<Nksc_wtfk>(model);
        }

        public string maxFkid(string D)
        {
            string id = "";
            String tsql = "select max(fkid) from Nksc_wtfk where fkid Like '" + D + "%'";
            string result = dao.GetScalar(tsql).ToStringEx();
            if (result == "")
            {
                id = D + "000001";
            }
            else
            {
                id = D + (int.Parse(result.Substring(8)) + 1).ToString("000000");
            }
            return id;
        }

        public List<Nksc_wtfk> SelectNkscWtfk(Guid id)
        {
            String tsql = "select * from Nksc_wtfk where id='" + id + "' order by riqi desc";
            DataTable dt = dao.Select(tsql);
            List<Nksc_wtfk> list = new List<Nksc_wtfk>();
            foreach (DataRow item in dt.Rows)
            {
                Nksc_wtfk model = new Nksc_wtfk();
                model.id = id;
                model.fkid = item["fkid"].ToStringEx();
                model.wtFile = item["wtFile"].ToStringEx();
                model.riqi = item["riqi"].ToStringEx();
                model.flags = item["flags"].ToStringEx();
                list.Add(model);
            }
            return list;
        }

        public bool isExistwtfk(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from Nksc_wtfk" + where;
            return dao.IsExists(tsql);
        }

        public bool isExist(String _where)
        {
            String where = " where 1=1 " + _where;
            String tsql = "select count(*) from Nksc" + where;
            return dao.IsExists(tsql);
        }
        #endregion

        public DataTable SelectNkscData(string id)
        {
            String tsql = "select * from Nksc where id='" + id + "'";
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public DataTable SelectNkscDataName(string dwqc)
        {
            String tsql = "select * from Nksc where dwqc='" + dwqc + "'";
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public DataTable SelectNkscData(string zdname, string id)
        {
            String tsql = "select " + zdname + " from Nksc where id='" + id + "'";
            DataTable dt = dao.Select(tsql);
            return dt;
        }

        public List<WordTempFile> SelectTempFile(string where)
        {
            List<WordTempFile> result = new List<WordTempFile>();
            String tsql = "select * from WordTempFile where 1=1 " + where + " order by Sort";
            result = dao.Select<WordTempFile>(tsql);
            return result;
        }

        public List<WordTempKey> SelectTempKey(string where)
        {
            List<WordTempKey> result = new List<WordTempKey>();
            String tsql = "select * from WordTempKey where 1=1 " + where;
            result = dao.Select<WordTempKey>(tsql);
            return result;
        }

        public List<WordTempXZ> SelectTempXZ(string where)
        {
            List<WordTempXZ> result = new List<WordTempXZ>();
            String tsql = "select * from WordTempXZ where 1=1 " + where;
            result = dao.Select<WordTempXZ>(tsql);
            return result;
        }

        public List<WordTempLCT> SelectTempLCT(string where)
        {
            List<WordTempLCT> result = new List<WordTempLCT>();
            String tsql = "select * from WordTempLCT where 1=1 " + where;
            result = dao.Select<WordTempLCT>(tsql);
            return result;
        }

        public DataTable Select(string tsql)
        {
            DataTable result = new DataTable();
            result = dao.Select(tsql);
            return result;
        }

        public DataTable SelectAddress(string CustomerID)
        {
            //财政局   上级主管区域    地区名称
            string sql = "select CzjName as xzqyName,UpName as sjzgqyName,CityName as dqmcName from View_SaleCustom where ID='" + CustomerID + "'";
            DataTable dtDep = dao.Select(sql);
            return dtDep;
        }

        public List<Nksc_qlqd> SelectQlqd(string nkid)
        {
            List<Nksc_qlqd> result = new List<Nksc_qlqd>();
            String tsql = "select * from Nksc_qlqd where id='" + nkid + "' order by qlsort";
            result = dao.Select<Nksc_qlqd>(tsql);
            return result;
        }

        public List<Nksc_qlqd> SelectQlqdAll(string nkid)
        {
            List<Nksc_qlqd> result = new List<Nksc_qlqd>();
            String tsql = "select * from Nksc_qlqd where id='" + nkid + "' order by leder,qlsx";
            result = dao.Select<Nksc_qlqd>(tsql);
            return result;
        }

        public List<string> SelectAll_ID()
        {
            List<string> result = new List<string>();
            String tsql = "select distinct id from Nksc_qlqd where id<>'00000000-0000-0000-0000-000000000000'";
            DataTable dt = dao.Select(tsql);
            foreach (DataRow item in dt.Rows)
            {
                result.Add(item["id"].ToString());
            }
            return result;
        }

        public List<string> Select_leder(string nkid)
        {
            List<string> result = new List<string>();
            String tsql = "select distinct leder from Nksc_qlqd where id='" + nkid + "'";
            DataTable dt = dao.Select(tsql);
            foreach (DataRow item in dt.Rows)
            {
                result.Add(item["leder"].ToString());
            }
            return result;
        }

        public int Delete_Qlqd(string nkid, string[] leders)
        {
            for (int i = 0; i < leders.Length; i++)
            {
                dao.Delete("delete from Nksc_qlqd where id='" + nkid + "' and leder='" + leders[i] + "'");
            }
            return 1;
        }
        public int Delete_Qlqd(string nkid)
        {
            return dao.Delete("delete from Nksc_qlqd where id='" + nkid + "'");
        }

        public string getld(string nkid)
        {
            object result = dao.GetScalar("select zzzwmc+''+ldzzmc+','+fzzwmc1+''+ldfzmc1 from Nksc where id='" + nkid + "'");
            List<Nksc_fz> fzs = SelectNkscfz(Guid.Parse(nkid));
            foreach (var item in fzs)
            {
                result += "," + item.fzzwmc + item.ldfzmc;
            }
            return result.ToStringEx();
        }
    }
}

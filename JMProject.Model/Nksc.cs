using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JMProject.Dal.TbColAttribute;

namespace JMProject.Model
{
    public class Nksc
    {
        public Nksc()
        {
            id = Guid.NewGuid();
            SaleOrderID = "";//订单合同号
            CustomerID = "";//客户编号
            dwqc = "";
            dwjc = "";
            dwjj = "";
            bhks = "";
            scqtbm = "";
            scxzbm = "";
            zzzwmc = "";
            ldzzmc = "";
            ldzzfg = "";
            fzzwmc1 = "";
            ldfzmc1 = "";
            ldfzfg1 = "";
            fxpgxzqtks = "";
            fxpgxzcy = "";
            fxpgxzzz = "";
            fxpgxzfzz = "";
            nkldxzcy = "";
            nkldxzqdks = "";
            nkldxzzz = "";
            nkldxzfzz = "";
            ysldxzcy = "";
            ysldxzqdks = "";
            ysldxzzz = "";
            ysldxzfzz = "";
            zfcgxzcy = "";
            zfcgxzqdks = "";
            zfcgxzzz = "";
            zfcgxzfzz = "";
            gyzcxzcy = "";
            gyzcxzqdks = "";
            gyzcxzzz = "";
            gyzcxzfzz = "";
            jdjcxzcy = "";
            jdjcxzqdks = "";
            jdjcxzzz = "";
            jdjcxzfzz = "";
            nbsjks = "";
            zdjcsshpjks = "";
            bxrgwzdks = "";
            bzndlgjhks = "";
            bnlgdgwmc = "财务负责人、会计、出纳";
            zdbgdgkks = "";
            xxgkzrjjks = "";
            zdbgcdks = "";
            fzxxglxtks = "";
            xxxcgzqtks = "";
            ksglks = "";
            bhyw = "";
            bdwsrbk = "";
            srywgkks = "";
            jfzcgkks = "";
            zfcgzlgkks = "";
            rsglzdgkks = "";
            rsglhbks = "";
            nzkhgkks = "";
            lzjmytgkks = "";
            ywslzdmc = "";
            yhscfj = "";
            kqsjswS = "";
            kqsjswE = "";
            kqsjxwS = "";
            kqsjxwE = "";
            htgkks1 = "";
            fileName = "";
            flag = "1";
            pfr = "";
            dzzjgmc = "";
            flagDown = "0";
            flagMoney = "0";
            swfName = "";
            tsyqtext = "";
            zddate = "";
            xyzdsum = 0;
            bczdsum = 0;
            sysum = 0;
            bz = "";
            kqsjswSd = "";
            kqsjswEd = "";
            kqsjxwSd = "";
            kqsjxwEd = "";
            wtfkFlag = "0";
            NkscDate = "";
            NkscSBDate = "";
            syfw0415 = "";
            jine0407 = "";
            jine0408 = "";
            jine041509 = "";
            Radioclf = "";
            Radiohyf = "";
            Radiopxf = "";
            Radiogwzdf = "";
            Radiobzz = "";
            Radio_zjzf = "1";
            Radio_jksh = "1";
            Radio_bxsh = "1";
            NkscDatePDF = "";
            peoPDF = "";
            NkscDateSC = "";
            peoSC = "";
            zzzwDY = "";
            fzzwDY = "";
            NkscDateSCPDF = "";
            AddBook = "";
            TcFlag = "";
            TcDate = "";
            EngineRoom = "";
            gwglgkks = "";
            zwxxgkks = "";
            gwkglks = "";
            gwkjdks = "";
            gdzccz = "";
            gdzcdb = "";
            gdzcgz = "";
            gdzcqc = "";
            bgypglgkks = "";
            yzglgkks = "";
            gwkzd = "0";
            czzxzjgkks = "";
            zxzjgl = "0";
            jsxmgkks01 = "";
            jsxmjxpjks01 = "";
            Radio_cghtsq = "1";
            nbkzgzxzzz01 = "";
            nbkzgzxzfzz01 = "";
            nbkzgzxzzzcy01 = "";
            nbkzgzxzzzqt01 = "";
            version = "";
            Radio_zxcgsq = "1";
            Radio_czzxzj = "1";
            Radio_fczzxzj = "1";

            FPImage = "";
        }

        [PrimaryKey]
        public Guid id { get; set; }
        public string SaleOrderID { get; set; }
        public string CustomerID { get; set; }
        public string dwqc { get; set; }
        public string dwjc { get; set; }
        public string dwjj { get; set; }
        public string bhks { get; set; }
        public string scqtbm { get; set; }
        public string scxzbm { get; set; }
        public string zzzwmc { get; set; }
        public string ldzzmc { get; set; }
        public string ldzzfg { get; set; }
        public string fzzwmc1 { get; set; }
        public string ldfzmc1 { get; set; }
        public string ldfzfg1 { get; set; }
        public string fxpgxzqtks { get; set; }
        public string fxpgxzcy { get; set; }
        public string fxpgxzzz { get; set; }
        public string fxpgxzfzz { get; set; }
        public string nkldxzcy { get; set; }
        public string nkldxzqdks { get; set; }
        public string nkldxzzz { get; set; }
        public string nkldxzfzz { get; set; }
        public string ysldxzcy { get; set; }
        public string ysldxzqdks { get; set; }
        public string ysldxzzz { get; set; }
        public string ysldxzfzz { get; set; }
        public string zfcgxzcy { get; set; }
        public string zfcgxzqdks { get; set; }
        public string zfcgxzzz { get; set; }
        public string zfcgxzfzz { get; set; }
        public string gyzcxzcy { get; set; }
        public string gyzcxzqdks { get; set; }
        public string gyzcxzzz { get; set; }
        public string gyzcxzfzz { get; set; }
        public string jdjcxzcy { get; set; }
        public string jdjcxzqdks { get; set; }
        public string jdjcxzzz { get; set; }
        public string jdjcxzfzz { get; set; }
        public string nbsjks { get; set; }
        public string zdjcsshpjks { get; set; }
        public string bxrgwzdks { get; set; }
        public string bzndlgjhks { get; set; }
        public string bnlgdgwmc { get; set; }
        public string zdbgdgkks { get; set; }
        public string xxgkzrjjks { get; set; }
        public string zdbgcdks { get; set; }
        public string fzxxglxtks { get; set; }
        public string xxxcgzqtks { get; set; }
        public string ksglks { get; set; }
        public string bhyw { get; set; }

        public string bdwsrbk { get; set; }
        public string srywgkks { get; set; }
        public string jfzcgkks { get; set; }
        public string zfcgzlgkks { get; set; }
        public string rsglzdgkks { get; set; }
        public string rsglhbks { get; set; }
        public string nzkhgkks { get; set; }
        public string lzjmytgkks { get; set; }
        public string ywslzdmc { get; set; }
        public string yhscfj { get; set; }

        public string kqsjswS { get; set; }
        public string kqsjswE { get; set; }
        public string kqsjxwS { get; set; }
        public string kqsjxwE { get; set; }
        public string htgkks1 { get; set; }

        public string fileName { get; set; }

        public string flag { get; set; }
        public string pfr { get; set; }

        public string dzzjgmc { get; set; }
        public string flagDown { get; set; }
        public string flagMoney { get; set; }

        public string swfName { get; set; }

        public string tsyqtext { get; set; }

        public string zddate { get; set; }
        public int xyzdsum { get; set; }
        public int bczdsum { get; set; }
        public int sysum { get; set; }
        public string bz { get; set; }

        public string kqsjswSd { get; set; }
        public string kqsjswEd { get; set; }
        public string kqsjxwSd { get; set; }
        public string kqsjxwEd { get; set; }

        public string wtfkFlag { get; set; }
        public string NkscDate { get; set; }

        public string NkscSBDate { get; set; }
        public string syfw0415 { get; set; }
        public string jine0407 { get; set; }
        public string jine0408 { get; set; }
        public string jine041509 { get; set; }
        public string Radioclf { get; set; }
        public string Radiohyf { get; set; }
        public string Radiopxf { get; set; }
        public string Radiogwzdf { get; set; }
        public string Radiobzz { get; set; }

        //预算内资金支付审批
        public string Radio_zjzf { get; set; }
        public string Radio_jksh { get; set; }
        public string Radio_bxsh { get; set; }

        public string NkscDatePDF { get; set; }
        public string peoPDF { get; set; }
        public string NkscDateSC { get; set; }
        public string peoSC { get; set; }
        public string zzzwDY { get; set; }
        public string fzzwDY { get; set; }

        public string NkscDateSCPDF { get; set; }
        public string AddBook { get; set; }


        public string TcFlag { get; set; }
        public string TcDate { get; set; }

        public string EngineRoom { get; set; }
        public string gwglgkks { get; set; }
        public string zwxxgkks { get; set; }

        public string gwkglks { get; set; }
        public string gwkjdks { get; set; }

        public string gdzccz { get; set; }
        public string gdzcdb { get; set; }
        public string gdzcgz { get; set; }
        public string gdzcqc { get; set; }
        public string bgypglgkks { get; set; }
        public string yzglgkks { get; set; }

        public string gwkzd { get; set; }
        public string czzxzjgkks { get; set; }

        public string zxzjgl { get; set; }

        //180305
        public string jsxmgkks01 { get; set; }
        public string jsxmjxpjks01 { get; set; }
        //政府采购合同审批
        public string Radio_cghtsq { get; set; }
        public string nbkzgzxzzz01 { get; set; }
        public string nbkzgzxzfzz01 { get; set; }
        public string nbkzgzxzzzcy01 { get; set; }
        public string nbkzgzxzzzqt01 { get; set; }
        public string version { get; set; }

        //自行采购合同审批
        public string Radio_zxcgsq { get; set; }
        //财政专项资金审批
        public string Radio_czzxzj { get; set; }
        //非财政专项资金审批
        public string Radio_fczzxzj { get; set; }

        public string FPImage { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [Nksc](");
            sb.Append("[id]");
            sb.Append(",[SaleOrderID]");
            sb.Append(",[CustomerID]");
            sb.Append(",[dwjc]");
            sb.Append(",[dwqc]");
            sb.Append(",[syfw0415]");
            sb.Append(",[kqsjswS]");
            sb.Append(",[kqsjswE]");
            sb.Append(",[kqsjxwS]");
            sb.Append(",[kqsjxwE]");
            sb.Append(",[kqsjswSd]");
            sb.Append(",[kqsjswEd]");
            sb.Append(",[kqsjxwSd]");
            sb.Append(",[kqsjxwEd]");
            sb.Append(",[bhks]");
            sb.Append(",[dwjj]");
            sb.Append(",[dzzjgmc]");
            sb.Append(",[zzzwmc]");
            sb.Append(",[ldzzmc]");
            sb.Append(",[ldzzfg]");
            sb.Append(",[zzzwDY]");
            sb.Append(",[fzzwmc1]");
            sb.Append(",[ldfzmc1]");
            sb.Append(",[ldfzfg1]");
            sb.Append(",[fzzwDY]");
            sb.Append(",[scqtbm]");
            sb.Append(",[scxzbm]");
            sb.Append(",[nkldxzcy]");
            sb.Append(",[nkldxzqdks]");
            sb.Append(",[nkldxzzz]");
            sb.Append(",[nkldxzfzz]");
            sb.Append(",[nbkzgzxzzz01]");
            sb.Append(",[nbkzgzxzfzz01]");
            sb.Append(",[nbkzgzxzzzcy01]");
            sb.Append(",[nbkzgzxzzzqt01]");
            sb.Append(",[fxpgxzqtks]");
            sb.Append(",[fxpgxzcy]");
            sb.Append(",[fxpgxzzz]");
            sb.Append(",[fxpgxzfzz]");
            sb.Append(",[ysldxzcy]");
            sb.Append(",[ysldxzqdks]");
            sb.Append(",[ysldxzzz]");
            sb.Append(",[ysldxzfzz]");
            sb.Append(",[zfcgxzcy]");
            sb.Append(",[zfcgxzqdks]");
            sb.Append(",[zfcgxzzz]");
            sb.Append(",[zfcgxzfzz]");
            sb.Append(",[gyzcxzcy]");
            sb.Append(",[gyzcxzqdks]");
            sb.Append(",[gyzcxzzz]");
            sb.Append(",[gyzcxzfzz]");
            sb.Append(",[jdjcxzcy]");
            sb.Append(",[jdjcxzqdks]");
            sb.Append(",[jdjcxzzz]");
            sb.Append(",[jdjcxzfzz]");
            sb.Append(",[bhyw]");
            sb.Append(",[ywslzdmc]");
            sb.Append(",[nbsjks]");
            sb.Append(",[zdjcsshpjks]");
            sb.Append(",[bxrgwzdks]");
            sb.Append(",[bzndlgjhks]");
            sb.Append(",[bnlgdgwmc]");
            sb.Append(",[zdbgdgkks]");
            sb.Append(",[zwxxgkks]");
            sb.Append(",[xxgkzrjjks]");
            sb.Append(",[fzxxglxtks]");
            sb.Append(",[xxxcgzqtks]");
            sb.Append(",[ksglks]");
            sb.Append(",[lzjmytgkks]");
            sb.Append(",[bdwsrbk]");
            sb.Append(",[srywgkks]");
            sb.Append(",[jfzcgkks]");
            sb.Append(",[zfcgzlgkks]");
            sb.Append(",[rsglzdgkks]");
            sb.Append(",[rsglhbks]");
            sb.Append(",[nzkhgkks]");
            sb.Append(",[zdbgcdks]");
            sb.Append(",[htgkks1]");
            sb.Append(",[gwglgkks]");
            sb.Append(",[gdzccz]");
            sb.Append(",[gdzcdb]");
            sb.Append(",[gdzcgz]");
            sb.Append(",[gdzcqc]");
            sb.Append(",[bgypglgkks]");
            sb.Append(",[yzglgkks]");
            sb.Append(",[gwkzd]");
            sb.Append(",[gwkglks]");
            sb.Append(",[gwkjdks]");
            sb.Append(",[EngineRoom]");
            sb.Append(",[zxzjgl]");
            sb.Append(",[czzxzjgkks]");
            sb.Append(",[jsxmgkks01]");
            sb.Append(",[jsxmjxpjks01]");
            sb.Append(",[Radio_cghtsq]");
            sb.Append(",[Radio_zjzf]");
            sb.Append(",[Radio_jksh]");
            sb.Append(",[Radio_bxsh]");
            sb.Append(",[jine041509]");
            sb.Append(",[jine0407]");
            sb.Append(",[jine0408]");
            sb.Append(",[Radioclf]");
            sb.Append(",[Radiohyf]");
            sb.Append(",[Radiopxf]");
            sb.Append(",[Radiogwzdf]");
            sb.Append(",[Radiobzz]");
            sb.Append(",[yhscfj]");
            sb.Append(",[NkscDate]");
            sb.Append(",[NkscSBDate]");
            sb.Append(",[flag]");
            sb.Append(",[fileName]");
            sb.Append(",[pfr]");
            sb.Append(",[flagDown]");
            sb.Append(",[swfName]");
            sb.Append(",[tsyqtext]");
            sb.Append(",[zddate]");
            sb.Append(",[xyzdsum]");
            sb.Append(",[bczdsum]");
            sb.Append(",[sysum]");
            sb.Append(",[wtfkFlag]");
            sb.Append(",[bz]");
            sb.Append(",[NkscDatePDF]");
            sb.Append(",[peoPDF]");
            sb.Append(",[NkscDateSC]");
            sb.Append(",[peoSC]");
            sb.Append(",[NkscDateSCPDF]");
            sb.Append(",[AddBook]");
            sb.Append(",[TcFlag]");
            sb.Append(",[TcDate]");
            sb.Append(",[flagMoney]");
            sb.Append(",[version]");
            sb.Append(",[Radio_zxcgsq]");
            sb.Append(",[Radio_czzxzj]");
            sb.Append(",[Radio_fczzxzj]");
            sb.Append(") VALUES (");
            sb.Append("'" + id + "'");
            sb.Append(",'" + SaleOrderID + "'");
            sb.Append(",'" + CustomerID + "'");
            sb.Append(",'" + dwjc + "'");
            sb.Append(",'" + dwqc + "'");
            sb.Append(",'" + syfw0415 + "'");
            sb.Append(",'" + kqsjswS + "'");
            sb.Append(",'" + kqsjswE + "'");
            sb.Append(",'" + kqsjxwS + "'");
            sb.Append(",'" + kqsjxwE + "'");
            sb.Append(",'" + kqsjswSd + "'");
            sb.Append(",'" + kqsjswEd + "'");
            sb.Append(",'" + kqsjxwSd + "'");
            sb.Append(",'" + kqsjxwEd + "'");
            sb.Append(",'" + bhks + "'");
            sb.Append(",'" + dwjj + "'");
            sb.Append(",'" + dzzjgmc + "'");
            sb.Append(",'" + zzzwmc + "'");
            sb.Append(",'" + ldzzmc + "'");
            sb.Append(",'" + ldzzfg + "'");
            sb.Append(",'" + zzzwDY + "'");
            sb.Append(",'" + fzzwmc1 + "'");
            sb.Append(",'" + ldfzmc1 + "'");
            sb.Append(",'" + ldfzfg1 + "'");
            sb.Append(",'" + fzzwDY + "'");
            sb.Append(",'" + scqtbm + "'");
            sb.Append(",'" + scxzbm + "'");
            sb.Append(",'" + nkldxzcy + "'");
            sb.Append(",'" + nkldxzqdks + "'");
            sb.Append(",'" + nkldxzzz + "'");
            sb.Append(",'" + nkldxzfzz + "'");
            sb.Append(",'" + nbkzgzxzzz01 + "'");
            sb.Append(",'" + nbkzgzxzfzz01 + "'");
            sb.Append(",'" + nbkzgzxzzzcy01 + "'");
            sb.Append(",'" + nbkzgzxzzzqt01 + "'");
            sb.Append(",'" + fxpgxzqtks + "'");
            sb.Append(",'" + fxpgxzcy + "'");
            sb.Append(",'" + fxpgxzzz + "'");
            sb.Append(",'" + fxpgxzfzz + "'");
            sb.Append(",'" + ysldxzcy + "'");
            sb.Append(",'" + ysldxzqdks + "'");
            sb.Append(",'" + ysldxzzz + "'");
            sb.Append(",'" + ysldxzfzz + "'");
            sb.Append(",'" + zfcgxzcy + "'");
            sb.Append(",'" + zfcgxzqdks + "'");
            sb.Append(",'" + zfcgxzzz + "'");
            sb.Append(",'" + zfcgxzfzz + "'");
            sb.Append(",'" + gyzcxzcy + "'");
            sb.Append(",'" + gyzcxzqdks + "'");
            sb.Append(",'" + gyzcxzzz + "'");
            sb.Append(",'" + gyzcxzfzz + "'");
            sb.Append(",'" + jdjcxzcy + "'");
            sb.Append(",'" + jdjcxzqdks + "'");
            sb.Append(",'" + jdjcxzzz + "'");
            sb.Append(",'" + jdjcxzfzz + "'");
            sb.Append(",'" + bhyw + "'");
            sb.Append(",'" + ywslzdmc + "'");
            sb.Append(",'" + nbsjks + "'");
            sb.Append(",'" + zdjcsshpjks + "'");
            sb.Append(",'" + bxrgwzdks + "'");
            sb.Append(",'" + bzndlgjhks + "'");
            sb.Append(",'" + bnlgdgwmc + "'");
            sb.Append(",'" + zdbgdgkks + "'");
            sb.Append(",'" + zwxxgkks + "'");
            sb.Append(",'" + xxgkzrjjks + "'");
            sb.Append(",'" + fzxxglxtks + "'");
            sb.Append(",'" + xxxcgzqtks + "'");
            sb.Append(",'" + ksglks + "'");
            sb.Append(",'" + lzjmytgkks + "'");
            sb.Append(",'" + bdwsrbk + "'");
            sb.Append(",'" + srywgkks + "'");
            sb.Append(",'" + jfzcgkks + "'");
            sb.Append(",'" + zfcgzlgkks + "'");
            sb.Append(",'" + rsglzdgkks + "'");
            sb.Append(",'" + rsglhbks + "'");
            sb.Append(",'" + nzkhgkks + "'");
            sb.Append(",'" + zdbgcdks + "'");
            sb.Append(",'" + htgkks1 + "'");
            sb.Append(",'" + gwglgkks + "'");
            sb.Append(",'" + gdzccz + "'");
            sb.Append(",'" + gdzcdb + "'");
            sb.Append(",'" + gdzcgz + "'");
            sb.Append(",'" + gdzcqc + "'");
            sb.Append(",'" + bgypglgkks + "'");
            sb.Append(",'" + yzglgkks + "'");
            sb.Append(",'" + gwkzd + "'");
            sb.Append(",'" + gwkglks + "'");
            sb.Append(",'" + gwkjdks + "'");
            sb.Append(",'" + EngineRoom + "'");
            sb.Append(",'" + zxzjgl + "'");
            sb.Append(",'" + czzxzjgkks + "'");
            sb.Append(",'" + jsxmgkks01 + "'");
            sb.Append(",'" + jsxmjxpjks01 + "'");
            sb.Append(",'" + Radio_cghtsq + "'");
            sb.Append(",'" + Radio_zjzf + "'");
            sb.Append(",'" + Radio_jksh + "'");
            sb.Append(",'" + Radio_bxsh + "'");
            sb.Append(",'" + jine041509 + "'");
            sb.Append(",'" + jine0407 + "'");
            sb.Append(",'" + jine0408 + "'");
            sb.Append(",'" + Radioclf + "'");
            sb.Append(",'" + Radiohyf + "'");
            sb.Append(",'" + Radiopxf + "'");
            sb.Append(",'" + Radiogwzdf + "'");
            sb.Append(",'" + Radiobzz + "'");
            sb.Append(",'" + yhscfj + "'");
            sb.Append(",'" + NkscDate + "'");
            sb.Append(",'" + NkscSBDate + "'");
            sb.Append(",'" + flag + "'");
            sb.Append(",'" + fileName + "'");
            sb.Append(",'" + pfr + "'");
            sb.Append(",'" + flagDown + "'");
            sb.Append(",'" + swfName + "'");
            sb.Append(",'" + tsyqtext + "'");
            sb.Append(",'" + zddate + "'");
            sb.Append(",'" + xyzdsum + "'");
            sb.Append(",'" + bczdsum + "'");
            sb.Append(",'" + sysum + "'");
            sb.Append(",'" + wtfkFlag + "'");
            sb.Append(",'" + bz + "'");
            sb.Append(",'" + NkscDatePDF + "'");
            sb.Append(",'" + peoPDF + "'");
            sb.Append(",'" + NkscDateSC + "'");
            sb.Append(",'" + peoSC + "'");
            sb.Append(",'" + NkscDateSCPDF + "'");
            sb.Append(",'" + AddBook + "'");
            sb.Append(",'" + TcFlag + "'");
            sb.Append(",'" + TcDate + "'");
            sb.Append(",'" + flagMoney + "'");
            sb.Append(",'" + version + "'");
            sb.Append(",'" + Radio_zxcgsq + "'");
            sb.Append(",'" + Radio_czzxzj + "'");
            sb.Append(",'" + Radio_fczzxzj + "'");
            sb.Append(")");
            return sb.ToString();
        }
    }
}

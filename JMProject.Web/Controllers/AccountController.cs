using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JMProject.Common;
using JMProject.Model.Sys;
using JMProject.Model;
using JMProject.BLL;
using JMProject.Web.AttributeEX;
using System.Data;
using System.Text;

namespace JMProject.Web.Controllers
{
    public class AccountController : BaseController
    {
        //修复 权力清单顺序
        public ActionResult UpdataSort()
        {
            NkscBLL bll = new NkscBLL();
            List<string> idary = bll.SelectAll_ID();
            foreach (string nkid in idary)
            {
                int qlsort = 1;

                string zz = bll.GetNameStr("ldzzmc", "and id='" + nkid + "'");
                string fz = bll.GetNameStr("ldfzmc1", "and id='" + nkid + "'");

                List<Nksc_qlqd> qlqds = bll.SelectQlqdAll(nkid);

                List<Nksc_qlqd> qlqds_zz = qlqds.Where(s => s.leder.Contains(zz)).OrderBy(s => s.qlsx).ToList<Nksc_qlqd>();
                foreach (Nksc_qlqd model in qlqds_zz)
                {
                    bll.Update("update Nksc_qlqd set qlsort=" + qlsort + " where id='" + model.id + "' and qlsx='" + model.qlsx + "' and leder='" + model.leder + "'");
                    qlsort++;
                }

                List<Nksc_qlqd> qlqds_fz = qlqds.Where(s => s.leder.Contains(fz)).OrderBy(s => s.qlsx).ToList<Nksc_qlqd>();
                foreach (Nksc_qlqd model in qlqds_fz)
                {
                    bll.Update("update Nksc_qlqd set qlsort=" + qlsort + " where id='" + model.id + "' and qlsx='" + model.qlsx + "' and leder='" + model.leder + "'");
                    qlsort++;
                }

                List<Nksc_fz> fzs = bll.SelectNkscfz(Guid.Parse(nkid));
                foreach (Nksc_fz item in fzs)
                {
                    List<Nksc_qlqd> qlqds_fzz = qlqds.Where(s => s.leder.Contains(item.ldfzmc)).OrderBy(s => s.qlsx).ToList<Nksc_qlqd>();
                    foreach (Nksc_qlqd model in qlqds_fzz)
                    {
                        bll.Update("update Nksc_qlqd set qlsort=" + qlsort + " where id='" + model.id + "' and qlsx='" + model.qlsx + "' and leder='" + model.leder + "'");
                        qlsort++;
                    }
                }
            }
            return Content("成功");
        }

        //public ActionResult Updata()
        //{
        //    try
        //    {
        //        //000001	admin	    超级用户
        //        //000002	songjie	    宋捷
        //        //000003	zouqiao	    邹撬
        //        //000004	liukuo	    刘阔
        //        //000005	tianzuo	    李天佐
        //        //000006	chengwei	韩成伟
        //        BasicAccountBLL bll = new BasicAccountBLL();
        //        //客户与用户
        //        string tsql = "select T0.User_ID,T0.USER_NAME,T0.User_Pwd,T0.Name AS LXR" +
        //                    ",T0.Tel as Phone,T0.Dwname,T0.Address,T0.tyxyID,T0.Dh" +
        //                    " ,T.*" +
        //                    " ,T1.ItemID as xzqyID" +
        //                    " ,T2.ID as sjzgqyID" +
        //                    " ,T3.ID as dqmcID" +
        //                    "  from WordTemp.dbo.U_User as T0" +
        //                    " left join WordTemp.dbo.View_Department T on T.id=T0.User_Group" +
        //                    " left join JMBusiness.dbo.DictionaryItem T1 on T1.DicID='005' and T1.ItemName=T.xzqyName" +
        //                    " left join JMBusiness.dbo.BasicCity T2 on T2.Name=T.sjzgqyName" +
        //                    " left join JMBusiness.dbo.BasicCity T3 on T3.Name=T.dqmcName " +
        //                    " where T.id<>'00'";
        //        DataTable dt = bll.GetDataTsql(tsql);
        //        string datestr = "20171231";
        //        string cdate = "2017-12-31";
        //        int id = 1;
        //        foreach (DataRow item in dt.Rows)
        //        {
        //            string dq = item["dqmcName"].ToStringEx();

        //            string ID = datestr + id.ToString("000000");
        //            string CDate = cdate;

        //            string Ywy = "";//业务员 (未完成)
        //            if (dq == "乾安县" || dq == "梅河口市" || dq == "榆树市" || dq == "扶余市" || dq == "公主岭市" || dq == "辉南县" || dq == "伊通满族自治县" || dq == "梨树县"
        //                || dq == "吉林省" || dq == "洮南市" || dq == "通化市" || dq == "范家屯")
        //            {
        //                Ywy = "000004";//刘阔 
        //            }
        //            else if (dq == "长春市" || dq == "安图县" || dq == "舒兰市" || dq == "农安县" || dq == "磐石市" || dq == "蛟河市" || dq == "珲春市" || dq == "桦甸市"
        //                || dq == "永吉县" || dq == "吉林市" || dq == "德惠市")
        //            {
        //                Ywy = "000005";//李天佐
        //            }
        //            else if (dq == "抚松县" || dq == "东丰县" || dq == "通化县" || dq == "松原市" || dq == "镇赉县" || dq == "宁江区" || dq == "柳河县" || dq == "白城市"
        //                || dq == "辽源市" || dq == "大安市")
        //            {
        //                Ywy = "000006";//韩成伟
        //            }
        //            else if (dq == "九台区" || dq == "昌邑区")
        //            {
        //                Ywy = "000003";//邹撬
        //            }
        //            else
        //            {
        //                Ywy = "000001";//超级用户
        //            }

        //            string Name = item["name"].ToStringEx();// item["Dwname"].ToStringEx().Trim() == "" ? item["name"].ToStringEx() : item["Dwname"].ToStringEx();
        //            string BM = null;
        //            string Lxr = "";
        //            string Zw = null;
        //            string Phone = "";
        //            string Industry = null;
        //            string UpID = item["sjzgqyID"].ToStringEx();
        //            string Province = "0001";
        //            string Xydj = null;
        //            string Gx = null;
        //            string Zyx = null;
        //            string Tel = "";
        //            string QQ = "";
        //            string Email = item["Dh"].ToStringEx();
        //            string Address = item["Address"].ToStringEx();
        //            string LxrSR = "";
        //            string QtLxr = "";
        //            string QtTel = "";
        //            string Bank = "";
        //            string CardNum = "";
        //            string SuiH = "";
        //            string Desc = item["User_ID"].ToStringEx();
        //            string Remark = "";
        //            string Flag = "";
        //            string Uid = "000001";
        //            string Source = null;
        //            string Region = item["dqmcID"].ToStringEx();
        //            string CustomerType = null;
        //            string CustomerGrade = null;
        //            string Code = item["tyxyID"].ToStringEx();
        //            string Invoice = item["Dwname"].ToStringEx();
        //            string UserName = Crypt.jiemi(item["USER_NAME"].ToStringEx());
        //            string UserPwd = Crypt.jiemi(item["User_Pwd"].ToStringEx());
        //            string Finance = item["xzqyID"].ToStringEx();
        //            if (!string.IsNullOrEmpty(item["LXR"].ToStringEx()))
        //            {
        //                string[] lxrs = item["LXR"].ToStringEx().Split(new string[] { "，", "、" }, StringSplitOptions.RemoveEmptyEntries);
        //                if (lxrs.Length == 1)
        //                {
        //                    Lxr = lxrs[0];
        //                    QtLxr = lxrs[0];
        //                }
        //                else if (lxrs.Length == 2)
        //                {
        //                    Lxr = lxrs[0];
        //                    QtLxr = lxrs[1];
        //                }
        //                else if (lxrs.Length > 2)
        //                {
        //                    Lxr = lxrs[0];
        //                    QtLxr = lxrs[1];
        //                    Remark += item["LXR"].ToStringEx();
        //                }
        //            }
        //            if (item["Phone"].ToStringEx().Length >= 14)
        //            {
        //                string[] lxrs = item["Phone"].ToStringEx().Split(new string[] { "，", "、", "," }, StringSplitOptions.RemoveEmptyEntries);
        //                if (lxrs.Length == 1)
        //                {
        //                    Phone = lxrs[0];
        //                    QtTel = lxrs[0];
        //                }
        //                else if (lxrs.Length == 2)
        //                {
        //                    Phone = lxrs[0];
        //                    QtTel = lxrs[1];
        //                }
        //                else if (lxrs.Length > 2)
        //                {
        //                    Phone = lxrs[0];
        //                    QtTel = lxrs[1];
        //                    Remark += " " + item["Phone"].ToStringEx();
        //                }
        //            }
        //            else
        //            {
        //                Phone = item["Phone"].ToStringEx();
        //            }
        //            bll.Insert("insert into SaleCustom values('" + ID + "','" + CDate + "','" + Ywy + "','" + Name + "',null,'" + Lxr + "',null,'" + Phone + "',null,'" + UpID + "','" + Province
        //                + "',null,null,null,'" + Tel + "','" + QQ + "','" + Email + "','" + Address + "','" + LxrSR + "','" + QtLxr + "','" + QtTel + "','" + Bank + "','" + CardNum + "','" + SuiH
        //                + "','" + Desc + "','" + Remark + "','" + Flag + "','" + Uid + "',null,'" + Region + "',null,null,'" + Code + "','" + Invoice
        //                + "','" + UserName + "','" + UserPwd + "','" + Finance + "')");
        //            id++;
        //        }

        //        SaleOrderBLL salebll = new SaleOrderBLL();

        //        //内控手册
        //        tsql = "select T.*,T1.ID as cusID,T1.Email,T1.Ywy,T1.Name as cusName from WordTemp.dbo.Nksc T inner join JMBusiness.dbo.SaleCustom T1 on T1.[Desc]=T.uid";
        //        dt = bll.GetDataTsql(tsql);
        //        foreach (DataRow item in dt.Rows)
        //        {
        //            #region 合同 发票 收款
        //            string Saler = "";//业务员
        //            string Account = "";//账户
        //            string InvoiceFlag = "";//账户
        //            string PaymentFlag = "";//账户

        //            //发票
        //            DataTable dtFP = bll.GetDataTsql("select * from WordTemp.dbo.View_Nksc_FP where id='" + item["id"] + "' order by sort");
        //            //收款
        //            DataTable dtSK = bll.GetDataTsql("select * from WordTemp.dbo.View_Nksc_SK where id='" + item["id"] + "' order by sort");

        //            if (dtSK == null || dtSK.Rows.Count == 0)
        //            {
        //                if (item["Email"].ToStringEx() == "免费" || item["Email"].ToStringEx().StartsWith("付"))
        //                {
        //                    PaymentFlag = "000067";//已回款
        //                }
        //                else
        //                {
        //                    PaymentFlag = "000066";//未回款
        //                }
        //            }
        //            else
        //            {
        //                decimal fpmoney = Convert.ToDecimal(dtFP.Compute("sum(ysmoney)", "TRUE"));
        //                decimal skmoney = Convert.ToDecimal(dtSK.Compute("sum(dzmoney)", "TRUE"));
        //                if (fpmoney == skmoney)
        //                {
        //                    PaymentFlag = "000067";//已回款
        //                }
        //                else
        //                {
        //                    PaymentFlag = "000068";//部分回款
        //                }
        //            }

        //            if (dtFP == null || dtFP.Rows.Count == 0)
        //            {
        //                Saler = item["Ywy"].ToStringEx();
        //                Account = "01";
        //                if (item["Email"].ToStringEx() == "免费" || item["Email"].ToStringEx().StartsWith("付"))
        //                {
        //                    InvoiceFlag = "000064";//已开票
        //                }
        //                else
        //                {
        //                    InvoiceFlag = "000063";//未开票
        //                }
        //            }
        //            else
        //            {
        //                if (dtFP.Rows[0]["xsren"].ToString() == "0001")
        //                {
        //                    Saler = "000003";//0001	邹撬		 000003
        //                }
        //                else if (dtFP.Rows[0]["xsren"].ToString() == "0002")
        //                {
        //                    Saler = "000002";//0002	宋捷		 000002
        //                }
        //                else if (dtFP.Rows[0]["xsren"].ToString() == "0003")
        //                {
        //                    Saler = "000005";//0003	李天佐		 000005
        //                }
        //                else if (dtFP.Rows[0]["xsren"].ToString() == "0004")
        //                {
        //                    Saler = "000006";//0004	韩成伟		 000006
        //                }
        //                else if (dtFP.Rows[0]["xsren"].ToString() == "0005")
        //                {
        //                    Saler = "000004";//0005	刘阔         000004
        //                }
        //                Account = dtFP.Rows[0]["fpaccount"].ToString().Trim();
        //                InvoiceFlag = "000064";//已开票
        //                //012	000063	未开票	
        //                //012	000064	已开票	
        //                //012	000065	部分开票
        //                //013	000066	未回款	
        //                //013	000067	已回款	
        //                //013	000068	部分回款
        //                //014	000069	未出库	
        //                //014	000070	已出库	
        //                //015	000071	未审核	
        //                //015	000072	已审核	
        //            }

        //            //016	000073	新购		00
        //            //016	000074	服务费		00
        //            //订单最大
        //            string saleId = salebll.Maxid(Convert.ToDateTime(item["NkscDate"]).ToString("yyyyMMdd"));
        //            string sqlMain = "INSERT INTO SaleOrder(Id,OrderDate,SaleCustomId,Saler,InvoiceFlag,PaymentFlag,OutStockFlag,CheckFlag,CheckDate,Finshed,Remake,UserId,AccountId,OrderType,Enclosure) values('"
        //            + saleId + "','" + Convert.ToDateTime(item["NkscDate"]).ToString("yyyy-MM-dd") + "','" + item["cusID"] + "','" + Saler + "','" + InvoiceFlag + "','" + PaymentFlag + "','000069','000071','','False','','000001','" + Account + "','000073','')";
        //            bll.Insert(sqlMain);

        //            if (dtFP != null && dtFP.Rows.Count > 0)
        //            {
        //                bool scgx = false;
        //                for (int itemC = 0; itemC < dtFP.Rows.Count; itemC++)
        //                {
        //                    DataRow itemFP = dtFP.Rows[itemC];

        //                    string ItemId = saleId + (itemC + 1).ToString("00");
        //                    string ProdectType = "";
        //                    int ItemCount = 0;
        //                    decimal ItemPrice = 0.00M;
        //                    decimal ItemMoney = Convert.ToDecimal(itemFP["fpmoney"]);
        //                    decimal TaxMoney = ItemMoney * 0.05M;
        //                    string Service = "否";
        //                    string SerDateS = "";
        //                    string SerDateE = "";
        //                    string ProdectDesc = "";
        //                    //025	000106  未提成
        //                    //025	000107	已提成	
        //                    string TcFlag = "000106";
        //                    string TcDate = "";


        //                    if (itemFP["TypeName"].ToStringEx() == "内控手册")
        //                    {
        //                        ProdectDesc = "内控手册";
        //                        ProdectType = "0201";
        //                        ItemCount = 1;
        //                        ItemPrice = ItemMoney;
        //                        Service = "是";
        //                        SerDateS = Convert.ToDateTime(item["NkscDate"]).ToString("yyyy-MM-dd");
        //                        SerDateE = Convert.ToDateTime(item["NkscDate"]).AddMonths(12).ToString("yyyy-MM-dd");
        //                        if (item["TcFlag"].ToStringEx() == "1")
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = item["TcDate"].ToStringEx();
        //                        }
        //                        else if (item["NkscDate"].ToStringEx().StartsWith("2017") && item["flagMoney"].ToStringEx() == "3")
        //                        {
        //                            //2017 开票且到账 自动为 已提状态
        //                            TcFlag = "000107";
        //                            TcDate = Convert.ToDateTime(itemFP["fpdate"]).ToString("yyyy-MM-dd");
        //                        }
        //                    }
        //                    else if (itemFP["TypeName"].ToStringEx() == "内控报告")
        //                    {
        //                        if (item["gwkxe"].ToStringEx() == "1")
        //                        {
        //                            ProdectDesc = "手册更新";
        //                            ProdectType = "0204";
        //                            ItemCount = 1;
        //                            ItemPrice = ItemMoney;
        //                            scgx = true;
        //                        }
        //                        else
        //                        {
        //                            ProdectDesc = "内控报告";
        //                            ProdectType = "0203";
        //                            ItemCount = 1;
        //                            ItemPrice = ItemMoney;
        //                        }
        //                        if (item["TcFlag"].ToStringEx() == "1" && item["TcDate"].ToStringEx().StartsWith("2018"))
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = item["TcDate"].ToStringEx();
        //                        }
        //                    }
        //                    else if (itemFP["TypeName"].ToStringEx() == "内控打分")
        //                    {
        //                        ProdectDesc = "内控打分";
        //                        ProdectType = "0202";
        //                        ItemCount = 1;
        //                        ItemPrice = ItemMoney;
        //                        if (item["TcFlag"].ToStringEx() == "1")
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = item["TcDate"].ToStringEx();
        //                        }
        //                        else
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = Convert.ToDateTime(itemFP["fpdate"]).ToString("yyyy-MM-dd");
        //                        }
        //                    }
        //                    else if (itemFP["TypeName"].ToStringEx() == "加印手册")
        //                    {
        //                        ProdectDesc = "加印手册";
        //                        ProdectType = "0205";
        //                        ItemCount = Convert.ToInt32(ItemMoney / 100);
        //                        ItemPrice = 100.00M;
        //                        if (item["TcFlag"].ToStringEx() == "1")
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = item["TcDate"].ToStringEx();
        //                        }
        //                        else
        //                        {
        //                            TcFlag = "000107";
        //                            TcDate = Convert.ToDateTime(itemFP["fpdate"]).ToString("yyyy-MM-dd");
        //                        }
        //                    }

        //                    string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
        //                        + " VALUES('" + saleId + "','" + ItemId + "','" + ProdectType + "','" + ProdectDesc + "','" + ItemCount + "','" + ItemPrice + "','" + ItemMoney + "','" + TaxMoney + "','0.00','0.00','0.00','" + Service + "','" + SerDateS + "','" + SerDateE + "','" + TcFlag + "','" + TcDate + "')";
        //                    bll.Insert(sqlItem);
        //                    //发票
        //                    string sqlFP = "insert into FinOrderInvoice values('" + saleId + "','" + itemFP["sort"] + "','" + itemFP["fpaccount"] + "','" + itemFP["fpdate"] + "','" + itemFP["fpmoney"] + "','" + itemFP["ysmoney"] + "','" + itemFP["bz"] + "')";
        //                    bll.Insert(sqlFP);

        //                    //收款
        //                    DataTable dtSKAdd = bll.GetDataTsql("select * from WordTemp.dbo.View_Nksc_SK where id='" + item["id"] + "' and fpsort='" + itemFP["sort"] + "' order by sort");
        //                    foreach (DataRow itemSK in dtSKAdd.Rows)
        //                    {
        //                        string sqlSK = "insert into FinOrderPayment values('" + saleId + "','" + itemSK["fpsort"] + "','" + itemSK["sort"] + "','" + itemSK["dzaccount"] + "','" + itemSK["dzdate"] + "','" + itemSK["dzmoney"] + "','" + itemSK["bz"] + "')";
        //                        bll.Insert(sqlSK);
        //                    }
        //                }

        //                //已更新手册 但 没有开更新发票
        //                if (!scgx && item["gwkxe"].ToStringEx() == "1")
        //                {
        //                    string ItemId = saleId + "03";
        //                    string ProdectType = "";
        //                    int ItemCount = 0;
        //                    decimal ItemPrice = 0.00M;
        //                    decimal ItemMoney = 0.00M;
        //                    string Service = "否";
        //                    string SerDateS = "";
        //                    string SerDateE = "";
        //                    string ProdectDesc = "";
        //                    string TcFlag = "000106";
        //                    string TcDate = "";

        //                    Dictionary<string, decimal> testDic = new Dictionary<string, decimal>();

        //                    testDic.Add("梨树县林业局", 1400);

        //                    testDic.Add("东丰县水利局", 1000);
        //                    testDic.Add("东丰县残疾人联合会", 1000);
        //                    testDic.Add("东丰县农业机械技术推广站", 3000);
        //                    testDic.Add("通化县发展和改革局", 3000);
        //                    testDic.Add("松原市城市客运管理中心", 1000);

        //                    testDic.Add("安图县市场监督管理局", 1000);
        //                    testDic.Add("公主岭市市场监督管理局", 2500);
        //                    testDic.Add("磐石市牛心镇人民政府", 1000);
        //                    //梅河口4家4000
        //                    testDic.Add("梅河口市市场监督管理局", 1000);
        //                    testDic.Add("梅河口市产品质量计量检测所", 1000);
        //                    testDic.Add("梅河口市产品质量检验所", 1000);
        //                    testDic.Add("梅河口市市场监督管理局稽查分局", 1000);
        //                    testDic.Add("舒兰市朝阳镇人民政府", 2500);
        //                    testDic.Add("舒兰市开原镇中心卫生院", 2500);
        //                    testDic.Add("舒兰市亮甲山乡卫生院", 1000);
        //                    testDic.Add("舒兰市发展和改革局", 2500);
        //                    testDic.Add("舒兰市医疗保险管理中心", 2500);
        //                    testDic.Add("舒兰市煤炭安全生产监督管理中心", 2500);

        //                    ItemMoney = testDic[item["cusName"].ToStringEx()];
        //                    ProdectDesc = "手册更新";
        //                    ProdectType = "0204";
        //                    ItemCount = 1;
        //                    decimal TaxMoney = ItemMoney * 0.05M;
        //                    ItemPrice = ItemMoney;

        //                    string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
        //                        + " VALUES('" + saleId + "','" + ItemId + "','" + ProdectType + "','" + ProdectDesc + "','" + ItemCount + "','" + ItemPrice + "','" + ItemMoney + "','" + TaxMoney + "','0.00','0.00','0.00','" + Service + "','" + SerDateS + "','" + SerDateE + "','" + TcFlag + "','" + TcDate + "')";
        //                    bll.Insert(sqlItem);

        //                    bll.Insert("update SaleOrder set PaymentFlag = '000068',InvoiceFlag='000065' where Id='" + saleId + "'");
        //                }
        //            }
        //            else
        //            {
        //                //无发票信息时候
        //                string ItemId = saleId + "01";
        //                string ProdectType = "";
        //                int ItemCount = 0;
        //                decimal ItemPrice = 0.00M;
        //                decimal ItemMoney = 0.00M;
        //                string TcFlag = "000106";
        //                string TcDate = "";

        //                bool createZero = false;//是否生成 零元 发票与回款

        //                if (item["Email"].ToStringEx() == "免费未知")
        //                {
        //                    ItemMoney = 0.00M;
        //                }
        //                else if (item["Email"].ToStringEx() == "免费" || item["Email"].ToStringEx().StartsWith("付"))
        //                {
        //                    ItemMoney = 0.00M;
        //                    TcFlag = "000107";
        //                    TcDate = Convert.ToDateTime(item["NkscDate"]).ToString("yyyy-MM-dd");
        //                    createZero = true;
        //                }
        //                else
        //                {
        //                    //前4位为金额
        //                    ItemMoney = Convert.ToDecimal(item["Email"].ToStringEx().Substring(0, 4));
        //                }
        //                decimal TaxMoney = ItemMoney * 0.05M;
        //                string Service = "否";
        //                string SerDateS = "";
        //                string SerDateE = "";
        //                string ProdectDesc = item["Email"].ToStringEx();

        //                //内控手册
        //                ProdectType = "0201";
        //                ItemCount = 1;
        //                ItemPrice = ItemMoney;
        //                Service = "是";
        //                SerDateS = Convert.ToDateTime(item["NkscDate"]).ToString("yyyy-MM-dd");
        //                SerDateE = Convert.ToDateTime(item["NkscDate"]).AddMonths(12).ToString("yyyy-MM-dd");

        //                string sqlItem = "INSERT INTO SaleOrderItem([OrderId],[ItemId],[ProdectType],[ProdectDesc],[ItemCount],[ItemPrice],[ItemMoney],[TaxMoney],[PresentMoney],[OtherMoney],[ValidMoney],[Service],[SerDateS],[SerDateE],[TcFlag],[TcDate])"
        //                    + " VALUES('" + saleId + "','" + ItemId + "','" + ProdectType + "','" + ProdectDesc + "','" + ItemCount + "','" + ItemPrice + "','" + ItemMoney + "','" + TaxMoney + "','0.00','0.00','0.00','" + Service + "','" + SerDateS + "','" + SerDateE + "','" + TcFlag + "','" + TcDate + "')";
        //                bll.Insert(sqlItem);

        //                if (createZero)
        //                {
        //                    string date = Convert.ToDateTime(item["NkscDate"]).ToString("yyyy-MM-dd");
        //                    string sort = new FinOrderInvoiceBLL().Maxid(Convert.ToDateTime(item["NkscDate"]).ToString("yyyyMMdd"));
        //                    //发票
        //                    string sqlFP = "insert into FinOrderInvoice values('" + saleId + "','" + sort + "','" + Account + "','" + date + "','" + ItemMoney + "','" + ItemMoney + "','" + ProdectDesc + "')";
        //                    bll.Insert(sqlFP);

        //                    //收款
        //                    string sqlSK = "insert into FinOrderPayment values('" + saleId + "','" + sort + "','" + sort + "','" + Account + "','" + date + "','" + ItemMoney + "','" + ProdectDesc + "')";
        //                    bll.Insert(sqlSK);
        //                }
        //            }

        //            //未开票 但 收款了
        //            DataTable dtSKnoFP = bll.GetDataTsql("select * from WordTemp.dbo.View_Nksc_SK T left join FinOrderPayment T1 on T1.Id=T.sort and T1.InvoiceId=T.fpsort where T1.Id is null and T.id='" + item["id"] + "'");
        //            foreach (DataRow itemSK in dtSKnoFP.Rows)
        //            {
        //                string sqlSK = "insert into FinOrderPayment values('" + saleId + "',null,'" + itemSK["sort"] + "','" + itemSK["dzaccount"] + "','" + itemSK["dzdate"] + "','" + itemSK["dzmoney"] + "','" + itemSK["bz"] + "')";
        //                bll.Insert(sqlSK);
        //            }
        //            #endregion

        //            string version = "000";
        //            //8 F已装订   11 I手册已领取
        //            if (item["flag"].ToStringEx() != "8" && item["flag"].ToStringEx() != "11")
        //            {
        //                version = "001";
        //            }

        //            #region 手册基本信息
        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("INSERT INTO [JMBusiness].[dbo].[Nksc]");
        //            sb.Append("([id]");
        //            sb.Append(",[SaleOrderID]");
        //            sb.Append(",[CustomerID]");
        //            sb.Append(",[dwjc]");
        //            sb.Append(",[dwqc]");
        //            sb.Append(",[syfw0415]");
        //            sb.Append(",[kqsjswS]");
        //            sb.Append(",[kqsjswE]");
        //            sb.Append(",[kqsjxwS]");
        //            sb.Append(",[kqsjxwE]");
        //            sb.Append(",[kqsjswSd]");
        //            sb.Append(",[kqsjswEd]");
        //            sb.Append(",[kqsjxwSd]");
        //            sb.Append(",[kqsjxwEd]");
        //            sb.Append(",[bhks]");
        //            sb.Append(",[dwjj]");
        //            sb.Append(",[dzzjgmc]");
        //            sb.Append(",[zzzwmc]");
        //            sb.Append(",[ldzzmc]");
        //            sb.Append(",[ldzzfg]");
        //            sb.Append(",[zzzwDY]");
        //            sb.Append(",[fzzwmc1]");
        //            sb.Append(",[ldfzmc1]");
        //            sb.Append(",[ldfzfg1]");
        //            sb.Append(",[fzzwDY]");
        //            sb.Append(",[scqtbm]");
        //            sb.Append(",[scxzbm]");
        //            sb.Append(",[nkldxzcy]");
        //            sb.Append(",[nkldxzqdks]");
        //            sb.Append(",[nkldxzzz]");
        //            sb.Append(",[nkldxzfzz]");
        //            sb.Append(",[nbkzgzxzzz01]");
        //            sb.Append(",[nbkzgzxzfzz01]");
        //            sb.Append(",[nbkzgzxzzzcy01]");
        //            sb.Append(",[nbkzgzxzzzqt01]");
        //            sb.Append(",[fxpgxzqtks]");
        //            sb.Append(",[fxpgxzcy]");
        //            sb.Append(",[fxpgxzzz]");
        //            sb.Append(",[fxpgxzfzz]");
        //            sb.Append(",[ysldxzcy]");
        //            sb.Append(",[ysldxzqdks]");
        //            sb.Append(",[ysldxzzz]");
        //            sb.Append(",[ysldxzfzz]");
        //            sb.Append(",[zfcgxzcy]");
        //            sb.Append(",[zfcgxzqdks]");
        //            sb.Append(",[zfcgxzzz]");
        //            sb.Append(",[zfcgxzfzz]");
        //            sb.Append(",[gyzcxzcy]");
        //            sb.Append(",[gyzcxzqdks]");
        //            sb.Append(",[gyzcxzzz]");
        //            sb.Append(",[gyzcxzfzz]");
        //            sb.Append(",[jdjcxzcy]");
        //            sb.Append(",[jdjcxzqdks]");
        //            sb.Append(",[jdjcxzzz]");
        //            sb.Append(",[jdjcxzfzz]");
        //            sb.Append(",[bhyw]");
        //            sb.Append(",[ywslzdmc]");
        //            sb.Append(",[nbsjks]");
        //            sb.Append(",[zdjcsshpjks]");
        //            sb.Append(",[bxrgwzdks]");
        //            sb.Append(",[bzndlgjhks]");
        //            sb.Append(",[bnlgdgwmc]");
        //            sb.Append(",[zdbgdgkks]");
        //            sb.Append(",[zwxxgkks]");
        //            sb.Append(",[xxgkzrjjks]");
        //            sb.Append(",[fzxxglxtks]");
        //            sb.Append(",[xxxcgzqtks]");
        //            sb.Append(",[ksglks]");
        //            sb.Append(",[lzjmytgkks]");
        //            sb.Append(",[bdwsrbk]");
        //            sb.Append(",[srywgkks]");
        //            sb.Append(",[jfzcgkks]");
        //            sb.Append(",[zfcgzlgkks]");
        //            sb.Append(",[rsglzdgkks]");
        //            sb.Append(",[rsglhbks]");
        //            sb.Append(",[nzkhgkks]");
        //            sb.Append(",[zdbgcdks]");
        //            sb.Append(",[htgkks1]");
        //            sb.Append(",[gwglgkks]");
        //            sb.Append(",[gdzccz]");
        //            sb.Append(",[gdzcdb]");
        //            sb.Append(",[gdzcgz]");
        //            sb.Append(",[gdzcqc]");
        //            sb.Append(",[bgypglgkks]");
        //            sb.Append(",[yzglgkks]");
        //            sb.Append(",[gwkzd]");
        //            sb.Append(",[gwkglks]");
        //            sb.Append(",[gwkjdks]");
        //            sb.Append(",[EngineRoom]");
        //            sb.Append(",[zxzjgl]");
        //            sb.Append(",[czzxzjgkks]");
        //            sb.Append(",[jsxmgkks01]");
        //            sb.Append(",[jsxmjxpjks01]");
        //            sb.Append(",[Radio_cghtsq]");
        //            sb.Append(",[Radio_zjzf]");
        //            sb.Append(",[Radio_jksh]");
        //            sb.Append(",[Radio_bxsh]");
        //            sb.Append(",[jine041509]");
        //            sb.Append(",[jine0407]");
        //            sb.Append(",[jine0408]");
        //            sb.Append(",[Radioclf]");
        //            sb.Append(",[Radiohyf]");
        //            sb.Append(",[Radiopxf]");
        //            sb.Append(",[Radiogwzdf]");
        //            sb.Append(",[Radiobzz]");
        //            sb.Append(",[yhscfj]");
        //            sb.Append(",[NkscDate]");
        //            sb.Append(",[NkscSBDate]");
        //            sb.Append(",[flag]");
        //            sb.Append(",[fileName]");
        //            sb.Append(",[pfr]");
        //            sb.Append(",[flagDown]");
        //            sb.Append(",[swfName]");
        //            sb.Append(",[tsyqtext]");
        //            sb.Append(",[zddate]");
        //            sb.Append(",[xyzdsum]");
        //            sb.Append(",[bczdsum]");
        //            sb.Append(",[sysum]");
        //            sb.Append(",[wtfkFlag]");
        //            sb.Append(",[bz]");
        //            sb.Append(",[NkscDatePDF]");
        //            sb.Append(",[peoPDF]");
        //            sb.Append(",[NkscDateSC]");
        //            sb.Append(",[peoSC]");
        //            sb.Append(",[NkscDateSCPDF]");
        //            sb.Append(",[AddBook]");
        //            sb.Append(",[TcFlag]");
        //            sb.Append(",[TcDate]");
        //            sb.Append(",[flagMoney]");
        //            sb.Append(",[version]) VALUES (");

        //            sb.Append("'" + item["id"] + "'");//id
        //            sb.Append(",'" + saleId + "'");//SaleOrderID
        //            sb.Append(",'" + item["cusID"] + "'");//,<CustomerID, varchar(14),>
        //            sb.Append(",'" + item["dwjc"] + "'");//,<dwjc, nvarchar(50),>
        //            string dwqc = string.IsNullOrEmpty(item["dwqc"].ToString()) ? item["cusName"].ToString() : item["dwqc"].ToString();
        //            sb.Append(",'" + dwqc + "'");//,<dwqc, nvarchar(100),>
        //            sb.Append(",'" + item["syfw0415"] + "'");//,<syfw0415, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjswS"] + "'");//,<kqsjswS, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjswE"] + "'");//,<kqsjswE, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjxwS"] + "'");//,<kqsjxwS, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjxwE"] + "'");//,<kqsjxwE, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjswSd"] + "'");//,<kqsjswSd, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjswEd"] + "'");//,<kqsjswEd, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjxwSd"] + "'");//,<kqsjxwSd, nvarchar(50),>
        //            sb.Append(",'" + item["kqsjxwEd"] + "'");//,<kqsjxwEd, nvarchar(50),>
        //            sb.Append(",''");//,<bhks, nvarchar(1000),>
        //            sb.Append(",'" + item["dwjj"] + "'");//,<dwjj, nvarchar(4000),>
        //            sb.Append(",'" + item["dzzjgmc"] + "'");//,<dzzjgmc, nvarchar(20),>
        //            sb.Append(",'" + item["zzzwmc"] + "'");//,<zzzwmc, nvarchar(50),>
        //            sb.Append(",'" + item["ldzzmc"] + "'");//,<ldzzmc, nvarchar(50),>
        //            sb.Append(",'" + item["ldzzfg"] + "'");//,<ldzzfg, nvarchar(200),>
        //            sb.Append(",'" + item["zzzwDY"] + "'");//,<zzzwDY, varchar(1),>
        //            sb.Append(",'" + item["fzzwmc1"] + "'");//,<fzzwmc1, nvarchar(50),>
        //            sb.Append(",'" + item["ldfzmc1"] + "'");//,<ldfzmc1, nvarchar(50),>
        //            sb.Append(",'" + item["ldfzfg1"] + "'");//,<ldfzfg1, nvarchar(200),>
        //            sb.Append(",'" + item["fzzwDY"] + "'");//,<fzzwDY, varchar(1),>
        //            sb.Append(",'" + item["scqtbm"] + "'");//,<scqtbm, nvarchar(50),>
        //            sb.Append(",'" + item["scxzbm"] + "'");//,<scxzbm, nvarchar(200),>
        //            sb.Append(",'" + item["nkldxzcy"] + "'");//,<nkldxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["nkldxzqdks"] + "'");//,<nkldxzqdks, nvarchar(50),>
        //            sb.Append(",'" + item["nkldxzzz"] + "'");//,<nkldxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["nkldxzfzz"] + "'");//,<nkldxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["nbkzgzxzzz01"] + "'");//,<nbkzgzxzzz01, nvarchar(50),>
        //            sb.Append(",'" + item["nbkzgzxzfzz01"] + "'");//,<nbkzgzxzfzz01, nvarchar(50),>
        //            sb.Append(",'" + item["nbkzgzxzzzcy01"] + "'");//,<nbkzgzxzzzcy01, nvarchar(200),>
        //            sb.Append(",'" + item["nbkzgzxzzzqt01"] + "'");//,<nbkzgzxzzzqt01, nvarchar(50),>
        //            sb.Append(",'" + item["fxpgxzqtks"] + "'");//,<fxpgxzqtks, nvarchar(50),>
        //            sb.Append(",'" + item["fxpgxzcy"] + "'");//,<fxpgxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["fxpgxzzz"] + "'");//,<fxpgxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["fxpgxzfzz"] + "'");//,<fxpgxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["ysldxzcy"] + "'");//,<ysldxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["ysldxzqdks"] + "'");//,<ysldxzqdks, nvarchar(50),>
        //            sb.Append(",'" + item["ysldxzzz"] + "'");//,<ysldxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["ysldxzfzz"] + "'");//,<ysldxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["zfcgxzcy"] + "'");//,<zfcgxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["zfcgxzqdks"] + "'");//,<zfcgxzqdks, nvarchar(50),>
        //            sb.Append(",'" + item["zfcgxzzz"] + "'");//,<zfcgxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["zfcgxzfzz"] + "'");//,<zfcgxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["gyzcxzcy"] + "'");//,<gyzcxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["gyzcxzqdks"] + "'");//,<gyzcxzqdks, nvarchar(50),>
        //            sb.Append(",'" + item["gyzcxzzz"] + "'");//,<gyzcxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["gyzcxzfzz"] + "'");//,<gyzcxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["jdjcxzcy"] + "'");//,<jdjcxzcy, nvarchar(200),>
        //            sb.Append(",'" + item["jdjcxzqdks"] + "'");//,<jdjcxzqdks, nvarchar(50),>
        //            sb.Append(",'" + item["jdjcxzzz"] + "'");//,<jdjcxzzz, nvarchar(50),>
        //            sb.Append(",'" + item["jdjcxzfzz"] + "'");//,<jdjcxzfzz, nvarchar(50),>
        //            sb.Append(",'" + item["bhyw"] + "'");//,<bhyw, nvarchar(50),>
        //            sb.Append(",'" + item["ywslzdmc"] + "'");//,<ywslzdmc, nvarchar(2000),>
        //            sb.Append(",'" + item["nbsjks"] + "'");//,<nbsjks, nvarchar(50),>
        //            sb.Append(",'" + item["zdjcsshpjks"] + "'");//,<zdjcsshpjks, nvarchar(50),>
        //            sb.Append(",'" + item["bxrgwzdks"] + "'");//,<bxrgwzdks, nvarchar(50),>
        //            sb.Append(",'" + item["bzndlgjhks"] + "'");//,<bzndlgjhks, nvarchar(50),>
        //            sb.Append(",'" + item["bnlgdgwmc"] + "'");//,<bnlgdgwmc, nvarchar(50),>
        //            sb.Append(",'" + item["zdbgdgkks"] + "'");//,<zdbgdgkks, nvarchar(50),>
        //            sb.Append(",'" + item["zwxxgkks"] + "'");//,<zwxxgkks, varchar(30),>
        //            sb.Append(",'" + item["xxgkzrjjks"] + "'");//,<xxgkzrjjks, nvarchar(50),>
        //            sb.Append(",'" + item["fzxxglxtks"] + "'");//,<fzxxglxtks, nvarchar(50),>
        //            sb.Append(",'" + item["xxxcgzqtks"] + "'");//,<xxxcgzqtks, nvarchar(50),>
        //            sb.Append(",'" + item["ksglks"] + "'");//,<ksglks, nvarchar(50),>
        //            sb.Append(",'" + item["lzjmytgkks"] + "'");//,<lzjmytgkks, nvarchar(50),>
        //            sb.Append(",'" + item["bdwsrbk"] + "'");//,<bdwsrbk, nvarchar(200),>
        //            sb.Append(",'" + item["srywgkks"] + "'");//,<srywgkks, nvarchar(50),>
        //            sb.Append(",'" + item["jfzcgkks"] + "'");//,<jfzcgkks, nvarchar(50),>
        //            sb.Append(",'" + item["zfcgzlgkks"] + "'");//,<zfcgzlgkks, nvarchar(50),>
        //            sb.Append(",'" + item["rsglzdgkks"] + "'");//,<rsglzdgkks, nvarchar(50),>
        //            sb.Append(",'" + item["rsglhbks"] + "'");//,<rsglhbks, nvarchar(50),>
        //            sb.Append(",'" + item["nzkhgkks"] + "'");//,<nzkhgkks, nvarchar(50),>
        //            sb.Append(",'" + item["zdbgcdks"] + "'");//,<zdbgcdks, nvarchar(50),>
        //            sb.Append(",'" + item["htgkks1"] + "'");//,<htgkks1, nvarchar(50),>
        //            sb.Append(",'" + item["gwglgkks"] + "'");//,<gwglgkks, varchar(30),>
        //            sb.Append(",'" + item["gdzccz"] + "'");//,<gdzccz, varchar(30),>
        //            sb.Append(",'" + item["gdzcdb"] + "'");//,<gdzcdb, varchar(30),>
        //            sb.Append(",'" + item["gdzcgz"] + "'");//,<gdzcgz, varchar(30),>
        //            sb.Append(",'" + item["gdzcqc"] + "'");//,<gdzcqc, varchar(30),>
        //            sb.Append(",'" + item["bgypglgkks"] + "'");//,<bgypglgkks, varchar(30),>
        //            sb.Append(",'" + item["yzglgkks"] + "'");//,<yzglgkks, varchar(30),>
        //            sb.Append(",'" + item["gwkzd"] + "'");//,<gwkzd, varchar(4),>
        //            sb.Append(",'" + item["gwkglks"] + "'");//,<gwkglks, varchar(30),>
        //            sb.Append(",'" + item["gwkjdks"] + "'");//,<gwkjdks, varchar(30),>
        //            sb.Append(",'" + item["EngineRoom"] + "'");//,<EngineRoom, varchar(2),>
        //            sb.Append(",'" + item["zxzjgl"] + "'");//,<zxzjgl, varchar(4),>
        //            sb.Append(",'" + item["czzxzjgkks"] + "'");//,<czzxzjgkks, varchar(30),>
        //            sb.Append(",'" + item["jsxmgkks01"] + "'");//,<jsxmgkks01, nvarchar(30),>
        //            sb.Append(",'" + item["jsxmjxpjks01"] + "'");//,<jsxmjxpjks01, nvarchar(30),>
        //            sb.Append(",'" + item["Radio_cghtsq"] + "'");//,<Radio_cghtsq, nvarchar(2),>
        //            sb.Append(",'" + item["Radio_zjzf"] + "'");//,<Radio_zjzf, varchar(2),>
        //            sb.Append(",'" + item["Radio_jksh"] + "'");//,<Radio_jksh, varchar(2),>
        //            sb.Append(",'" + item["Radio_bxsh"] + "'");//,<Radio_bxsh, varchar(2),>
        //            sb.Append(",'" + item["jine041509"] + "'");//,<jine041509, varchar(20),>
        //            sb.Append(",'" + item["jine0407"] + "'");//,<jine0407, varchar(20),>
        //            sb.Append(",'" + item["jine0408"] + "'");//,<jine0408, varchar(20),>
        //            sb.Append(",'" + item["Radioclf"] + "'");//,<Radioclf, varchar(4),>
        //            sb.Append(",'" + item["Radiohyf"] + "'");//,<Radiohyf, varchar(4),>
        //            sb.Append(",'" + item["Radiopxf"] + "'");//,<Radiopxf, varchar(4),>
        //            sb.Append(",'" + item["Radiogwzdf"] + "'");//,<Radiogwzdf, varchar(4),>
        //            sb.Append(",'" + item["Radiobzz"] + "'");//,<Radiobzz, varchar(4),>
        //            sb.Append(",'" + item["yhscfj"] + "'");//,<yhscfj, nvarchar(2000),>
        //            sb.Append(",'" + item["NkscDate"] + "'");//,<NkscDate, nvarchar(20),>
        //            sb.Append(",'" + item["NkscSBDate"] + "'");//,<NkscSBDate, varchar(20),>
        //            sb.Append(",'" + item["flag"] + "'");//,<flag, varchar(50),>
        //            sb.Append(",'" + item["fileName"] + "'");//,<fileName, nvarchar(500),>
        //            sb.Append(",'" + item["pfr"] + "'");//,<pfr, nvarchar(20),>
        //            sb.Append(",'" + item["flagDown"] + "'");//,<flagDown, varchar(50),>
        //            sb.Append(",'" + item["swfName"] + "'");//,<swfName, nvarchar(500),>
        //            sb.Append(",'" + item["tsyqtext"] + "'");//,<tsyqtext, nvarchar(2000),>
        //            sb.Append(",'" + item["zddate"] + "'");//,<zddate, nvarchar(20),>
        //            int xyzdsum = string.IsNullOrEmpty(item["xyzdsum"].ToStringEx()) ? 0 : Convert.ToInt32(item["xyzdsum"]);
        //            int addbook = string.IsNullOrEmpty(item["AddBook"].ToStringEx()) ? 0 : Convert.ToInt32(item["AddBook"]);
        //            sb.Append("," + (xyzdsum - addbook) + "");//,<xyzdsum, int,>
        //            sb.Append("," + item["bczdsum"] + "");//,<bczdsum, int,>
        //            sb.Append(",0");//,<sysum, int,>
        //            sb.Append(",'" + item["wtfkFlag"] + "'");//,<wtfkFlag, nvarchar(2),>
        //            sb.Append(",'" + item["bz"] + "'");//,<bz, text,>
        //            sb.Append(",'" + item["NkscDatePDF"] + "'");//,<NkscDatePDF, varchar(20),>
        //            sb.Append(",'" + item["peoPDF"] + "'");//,<peoPDF, nvarchar(50),>
        //            sb.Append(",'" + item["NkscDateSC"] + "'");//,<NkscDateSC, varchar(20),>
        //            sb.Append(",'" + item["peoSC"] + "'");//,<peoSC, varchar(20),>
        //            sb.Append(",'" + item["NkscDateSCPDF"] + "'");//,<NkscDateSCPDF, varchar(20),>
        //            sb.Append(",'0'");//,<AddBook, varchar(10),>
        //            sb.Append(",'" + item["TcFlag"] + "'");//,<TcFlag, varchar(10),>
        //            sb.Append(",'" + item["TcDate"] + "'");//,<TcDate, varchar(20),>
        //            sb.Append(",'" + item["flagMoney"] + "'");//,<flagMoney, varchar(50),>
        //            //if (item["gwkxe"].ToStringEx() == "1")
        //            //{
        //            //    sb.Append(",'001')");
        //            //}
        //            //else
        //            //{
        //            sb.Append(",'" + version + "')");//,<version, varchar(10),>)
        //            //}
        //            bll.Insert(sb.ToString());
        //            #endregion

        //            //更新记录表
        //            if (item["gwkxe"].ToStringEx() == "1")
        //            {
        //                Nksc_Update nkup = new Nksc_Update();
        //                nkup.CustomerID = item["cusID"].ToString();
        //                nkup.NkscDate = DateTime.Now.ToString("yyyy-MM-dd");
        //                nkup.versionS = "000";
        //                nkup.versionE = "001";
        //                nkup.UpdateFlag = "0";
        //                new Nksc_UpdateBLL().Insert(nkup);
        //            }
        //        }
        //        //副职
        //        tsql = "insert into Nksc_fz select * from WordTemp.dbo.Nksc_fz";
        //        bll.Insert(tsql);
        //        //采购审批
        //        tsql = "insert into Nksc_cghtsq select * from WordTemp.dbo.Nksc_cghtsq";
        //        bll.Insert(tsql);
        //        //支出
        //        tsql = "insert into Nksc_Zcyw select * from WordTemp.dbo.Nksc_Zcyw";
        //        bll.Insert(tsql);
        //        //借款
        //        tsql = "insert into Nksc_Jkyw select * from WordTemp.dbo.Nksc_Jkyw";
        //        bll.Insert(tsql);
        //        //报销
        //        tsql = "insert into Nksc_Bxyw select * from WordTemp.dbo.Nksc_Bxyw";
        //        bll.Insert(tsql);
        //        //岗位
        //        tsql = "insert into Nksc_ksgwzr select * from WordTemp.dbo.Nksc_ksgwzr";
        //        bll.Insert(tsql);
        //        //科室
        //        tsql = "insert into Nksc_kszn select * from WordTemp.dbo.Nksc_kszn";
        //        bll.Insert(tsql);
        //    }
        //    catch (Exception ee)
        //    {
        //        return Content(ee.Message);
        //    }
        //    return View();
        //}

        public ActionResult Login()
        {
            return View();
        }

        #region 登陆
        [HttpPost]
        public JsonResult Login(string logname, string logpass)
        {
            if (string.IsNullOrEmpty(logname))
            {
                return Json(JsonHandler.CreateMessage(0, "用户名不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(logpass))
            {
                return Json(JsonHandler.CreateMessage(0, "密码不允许为空"), JsonRequestBehavior.AllowGet);
            }
            SysUserBLL bll = new SysUserBLL();
            SysUser user = bll.GetRow(logname, logpass);
            if (string.IsNullOrEmpty(user.Id))
            {
                return Json(JsonHandler.CreateMessage(0, "用户名或密码错误"), JsonRequestBehavior.AllowGet);
            }
            AccountModel account = new AccountModel();
            account.RoleID = user.RoleID;
            account.Id = user.Id;
            account.Name = user.Name;
            account.ZsName = user.ZsName;
            Session["Account"] = account;
            Session.Timeout = 120;
            LogHelper.AddLogUser(GetUserId(), "登陆系统:" + logname, Suggestion.Succes, "登陆");
            return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult LoginCustom()
        {
            return View();
        }

        #region 客户登陆
        [HttpPost]
        public JsonResult LoginCustom(string logname, string logpass)
        {
            if (string.IsNullOrEmpty(logname))
            {
                return Json(JsonHandler.CreateMessage(0, "用户名不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(logpass))
            {
                return Json(JsonHandler.CreateMessage(0, "密码不允许为空"), JsonRequestBehavior.AllowGet);
            }
            SaleCustomerBLL bll = new SaleCustomerBLL();
            SaleCustom user = bll.GetRow(logname, logpass);
            if (string.IsNullOrEmpty(user.ID))
            {
                return Json(JsonHandler.CreateMessage(0, "用户名或密码错误"), JsonRequestBehavior.AllowGet);
            }
            NkscBLL nksc = new NkscBLL();
            string NkscFlag = nksc.GetNameStr("flag", " and CustomerID='" + user.ID + "'");
            if (NkscFlag == "13")
            {
                return Json(JsonHandler.CreateMessage(0, "登录失败，请与我们联系！\n客服电话：15764397330、13080026523\n客服QQ：248225280、2091274568"), JsonRequestBehavior.AllowGet);
            }
            AccountModel account = new AccountModel();
            account.RoleID = "05";
            account.Id = user.ID;
            account.Name = user.UserName;
            account.ZsName = user.Name;
            Session["Account"] = account;
            Session.Timeout = 120;
            //LogHelper.AddLogUser(GetUserId(), "登陆系统:" + logname, Suggestion.Succes, "客户登陆");
            return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 安全退出
        [HttpPost]
        public JsonResult Exit()
        {
            //清楚Session
            Session.Abandon();
            return Json(JsonHandler.CreateMessage(1, ""), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改密码
        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult UpdatePWD(string opassword, string password)
        {
            if (string.IsNullOrEmpty(opassword))
            {
                return Json(JsonHandler.CreateMessage(0, "原密码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(JsonHandler.CreateMessage(0, "新密码 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            SysUserBLL bll = new SysUserBLL();
            string UserID = GetUserId();
            string User_Pwd = bll.GetNameStr("Pwd", "and Id='" + UserID + "'");
            if (User_Pwd != opassword)
            {
                return Json(JsonHandler.CreateMessage(0, "原密码错误请重新输入"), JsonRequestBehavior.AllowGet);
            }
            if (bll.Update("update SysUser set Pwd='" + password + "' where Id='" + UserID + "'") > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "修改密码:" + UserID, Suggestion.Succes, "修改密码");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "修改密码:" + UserID, Suggestion.Error, "修改密码");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 获取个人信息
        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult GetUsers()
        {
            string userID = GetUserId();
            if (string.IsNullOrEmpty(userID))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            SysUserBLL bll = new SysUserBLL();
            SysUser result = new SysUser { Id = userID };
            result = bll.GetRow(result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //TODO 修改个人信息 方法
        [AuthorizeAttributeEx]
        [HttpPost]
        public JsonResult UpdateMy(SysUser model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(JsonHandler.CreateMessage(0, "昵称 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.ZsName))
            {
                return Json(JsonHandler.CreateMessage(0, "姓名 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Phone))
            {
                return Json(JsonHandler.CreateMessage(0, "手机号 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.Birthday))
            {
                return Json(JsonHandler.CreateMessage(0, "出生日期 不允许为空"), JsonRequestBehavior.AllowGet);
            }
            model.Pic = string.IsNullOrEmpty(model.Pic) ? "" : model.Pic;
            model.Tel = string.IsNullOrEmpty(model.Tel) ? "" : model.Tel;
            model.IcCard = string.IsNullOrEmpty(model.IcCard) ? "" : model.IcCard;
            model.Address = string.IsNullOrEmpty(model.Address) ? "" : model.Address;
            model.Remake = string.IsNullOrEmpty(model.Remake) ? "" : model.Remake;
            SysUserBLL bll = new SysUserBLL();
            model.RoleID = bll.GetNameStr("RoleID", " and Id='" + model.Id + "'");
            model.Pwd = bll.GetNameStr("Pwd", " and Id='" + model.Id + "'");
            model.Pic = bll.GetNameStr("Pic", " and Id='" + model.Id + "'");
            //修改
            if (bll.Update(model) > 0)
            {
                LogHelper.AddLogUser(GetUserId(), "修改个人信息:" + model.Id, Suggestion.Succes, "个人信息");
                return Json(JsonHandler.CreateMessage(1, Suggestion.Succes), JsonRequestBehavior.AllowGet);
            }
            else
            {
                LogHelper.AddLogUser(GetUserId(), "修改个人信息:" + model.Id, Suggestion.Error, "个人信息");
                return Json(JsonHandler.CreateMessage(0, Suggestion.Error), JsonRequestBehavior.AllowGet);
            }
        }
    }
}

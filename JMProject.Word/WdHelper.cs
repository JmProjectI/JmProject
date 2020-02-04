using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using JMProject.Model;
using System.Collections;
using System.Reflection;
using System.Drawing;

namespace JMProject.Word
{
    public class WdHelper
    {
        private string WordTempPath = "";//Word模版路径
        private string WordSavePath = "";//Word生成路径
        private string ImageTempPath = "";//流程图模版路径
        private string ImageSavePath = "";//保存图片路径

        Dictionary<string, string> GdKey;//固定字符串替换

        List<string> DeleteMarks;//需要删除的书签

        // 1 预算业务
        // 2 收入业务
        // 3 支出业务 
        // a 非税收入业务 
        // d 收入登记入账 
        // 4|e 财政票据业务 
        // f 财政直接支付 
        // g 财政授权支付 
        // h 公务卡 
        // 5 政府采购业务  
        // i 政府购买服务流程  
        // 6 资产业务 
        // k 国有资产出租出借业务  
        // m 国有资产收入上缴流程  
        // n 国有资产产权登记、变更、注销流程  
        // o 国有资产产权纠纷调处流程  
        // p 资产评估流程  
        // 7 建设项目业务  
        // q 建设项目公开招标流程、邀请招标流程  
        // r 建设项目设计变更、洽商签证流程  
        // 8 合同管理业务  
        // 9 印章管理业务 
        // b 债务业务  
        // c 对外投资业务  
        // 采购合同审批 
        // 单位自行采购审批 
        // 机房 
        // 财政专项资金管理办法 
        // 差旅费 
        // 会议费 
        // 培训费 
        // 招待费 
        // 报账制 
        // 借款业务 
        // 党务管理
        // 
        private string yws = "";//组合业务

        public WdHelper(string _WordTempPath, string _WordSavePath, string _ImageTempPath, string _ImageSavePath)
        {
            WordTempPath = _WordTempPath;
            WordSavePath = _WordSavePath;
            ImageTempPath = _ImageTempPath;
            ImageSavePath = _ImageSavePath;
        }

        public void SetGDTempKey(Func<DataTable, Dictionary<string, string>> func, DataTable dt)
        {
            GdKey = func(dt);
        }

        public void SetDeleteBookMarkt(Func<DataTable, List<string>> GetDeleteFun, DataTable dt)
        {
            DeleteMarks = GetDeleteFun(dt);
        }

        /// <summary>
        /// 返回文件名称
        /// </summary>
        /// <param name="dt_nksc"></param>
        /// <returns></returns>
        public string CreateWord(string ksbm, DataTable dt_nksc, List<WordTempFile> list_file, List<WordTempKey> list_key
            , List<WordTempXZ> list_xz, List<WordTempLCT> list_lct, List<Nksc_fz> list_nkfz
            , Action<DataTable, string, string, Dictionary<string, string>> func)
        {
            if (!Directory.Exists(ImageSavePath))
            {
                Directory.CreateDirectory(ImageSavePath);
            }
            else
            {
                Directory.Delete(ImageSavePath, true);
                Directory.CreateDirectory(ImageSavePath);
            }

            //单位名称
            string dpname = dt_nksc.Rows[0]["dwqc"].ToString();
            //要生成的文件名称 （单位名称+年月日.docx）
            string fileName = dpname + DateTime.Now.ToString("yyyyMMdd") + ".docx";

            //包含业务
            string bhyw = dt_nksc.Rows[0]["bhyw"].ToString();

            //Word替换规则
            Queue queue_word = new Queue();
            var insertFils = list_file.Where(s => s.ywKey == "0" || bhyw.Contains(s.ywKey)).OrderBy(s => s.Sort).ToList();

            //固定取值 类
            Type constdata = typeof(ConstData_QYXY);
            Object obj = System.Activator.CreateInstance(constdata);

            //ChartHelper chartHelper = new ChartHelper();
            //GroupChart groupChart = new GroupChart();

            string parentID = "";//上一个编号
            foreach (WordTempFile item in insertFils)
            {
                WordModel model = new WordModel();
                //待组合文件
                model.File = WordTempPath + item.WordFile;
                //单位层面 与 业务层面 后接的文档 不需要另起一页
                if (parentID == "04" || parentID == "05")
                {
                    model.NewPage = "00";
                }
                else
                {
                    model.NewPage = "01";
                }
                model.Sort = item.Sort;


                //固定替换  如:九台区 我区 本区 县人民政府等等
                foreach (var GDitem in GdKey)
                {
                    model.Keyword.Add(GDitem.Key, GDitem.Value);
                }

                //01固定值替换 / 02数据库取值 / 03书签删除 / 04书签插流程图 / 05数据库无关键字 / 06书签插小组图

                //01固定值替换
                var gdKey = list_key.Where(s => s.KeyType == "01" && s.Zid == item.ID);
                foreach (var itemkey in gdKey)
                {
                    MethodInfo metinfo = constdata.GetMethod(itemkey.DBKey);
                    string dvalue = metinfo.Invoke(obj, null).ToString().Replace("\n", "&p");
                    model.Keyword.Add(itemkey.WordKey, dvalue);
                }

                //02数据库取值
                var dbKey = list_key.Where(s => s.KeyType == "02" && s.Zid == item.ID);
                foreach (var itemkey in dbKey)
                {
                    string dvalue = "";
                    if (!itemkey.DBKey.StartsWith("●"))
                    {
                        dvalue = dt_nksc.Rows[0][itemkey.DBKey].ToString().Replace("\n", "&p");
                    }
                    else
                    {
                        dvalue = itemkey.DBKey.Substring(1).Replace("\n", "&p");
                    }
                     
                    model.Keyword.Add(itemkey.WordKey, dvalue);
                }

                //05数据库无关键字
                var dbnoneKey = list_key.Where(s => s.KeyType == "05" && s.Zid == item.ID);
                foreach (var itemkey in dbnoneKey)
                {
                    func(dt_nksc, itemkey.WordKey, itemkey.DBKey, model.Keyword);
                }

                // 03书签删除 (没有业务删除)
                var delKey = list_key.Where(s => s.KeyType == "03" && s.Zid == item.ID);
                foreach (var itemkey in delKey)
                {
                    if (!DeleteMarks.Contains(itemkey.ywType))
                    {
                        model.BookmarkerTextRang.Add(itemkey.WordKey, "");
                    }
                }

                // 08书签删除 (有业务删除)
                var delKey08 = list_key.Where(s => s.KeyType == "08" && s.Zid == item.ID);
                foreach (var itemkey in delKey08)
                {
                    if (DeleteMarks.Contains(itemkey.ywType))
                    {
                        model.BookmarkerTextRang.Add(itemkey.WordKey, "");
                    }
                }

                // 09替换内容 (没有有业务 替换 内容)
                var delKey09 = list_key.Where(s => s.KeyType == "09" && s.Zid == item.ID);
                foreach (var itemkey in delKey09)
                {
                    if (!DeleteMarks.Contains(itemkey.ywType))
                    {
                        model.KeywordGD.Add(itemkey.WordKey, itemkey.DBKey);
                    }
                }

                // 10替换内容 (有业务 替换 内容)
                var delKey10 = list_key.Where(s => s.KeyType == "10" && s.Zid == item.ID);
                foreach (var itemkey in delKey10)
                {
                    if (DeleteMarks.Contains(itemkey.ywType))
                    {
                        model.KeywordGD.Add(itemkey.WordKey, itemkey.DBKey);
                    }
                }

                //04书签 插流程图
                var imgKey = list_key.Where(s => s.KeyType == "04" && s.Zid == item.ID);
                foreach (var itemkey in imgKey)
                {
                    List<string> texts = new List<string>();
                    List<Font> mfonts = new List<Font>();
                    List<Rectangle> rects = new List<Rectangle>();
                    var lctKey = list_lct.Where(s => s.wkey == itemkey.WordKey);
                    foreach (var lctitemkey in lctKey)
                    {
                        string text = "";
                        if (lctitemkey.dkey.StartsWith("&"))
                        {
                            text = string.Format(lctitemkey.formate, ksbm);
                        }
                        else
                        {
                            text = string.Format(lctitemkey.formate, dt_nksc.Rows[0][lctitemkey.dkey].ToString());
                        }
                        texts.Add(text);

                        mfonts.Add(new Font(lctitemkey.fontName, lctitemkey.fontSize));
                        rects.Add(new Rectangle(lctitemkey.x, lctitemkey.y, lctitemkey.w, lctitemkey.h));
                    }

                    string img_yt = ImageTempPath + itemkey.DBKey;//模板
                    string img_xt = ImageSavePath + itemkey.DBKey;//生成
                    ImageHelper.EditImageText(img_yt, texts, mfonts, rects, img_xt);
                    model.BookmarkerImage.Add(itemkey.WordKey, img_xt);
                }

                //06书签 插小组图
                var xzKey = list_key.Where(s => s.KeyType == "06" && s.Zid == item.ID);
                foreach (var itemkey in xzKey)
                {
                    WordTempXZ xz = list_xz.FirstOrDefault<WordTempXZ>(s => s.dkey == itemkey.WordKey);
                    if (xz == null)
                    {
                        continue;
                    }

                    string zz = string.IsNullOrEmpty(xz.zz) ? "" : dt_nksc.Rows[0][xz.zz].ToString();
                    string fz = string.IsNullOrEmpty(xz.fz) ? "" : dt_nksc.Rows[0][xz.fz].ToString();
                    string qtks = string.IsNullOrEmpty(xz.qtks) ? "" : dt_nksc.Rows[0][xz.qtks].ToString();
                    string cy = string.IsNullOrEmpty(xz.cy) ? "" : dt_nksc.Rows[0][xz.cy].ToString();

                    string imgFile = ImageSavePath + itemkey.DBKey;
                    //Bitmap bit_img = chartHelper.CreateChart(zz, fz, qtks, cy);
                    Bitmap bit_img = new GroupChart().WorkGroupChart(zz, fz, qtks, cy);
                    bit_img.Save(imgFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    model.BookmarkerImage.Add(itemkey.WordKey, imgFile);
                }

                //07单位组织架构图
                WordTempKey zzKey = list_key.FirstOrDefault<WordTempKey>(s => s.KeyType == "07" && s.Zid == item.ID);
                if (zzKey != null)
                {
                    string zz = "";
                    ZzjgModel zzfgs = new ZzjgModel();
                    List<ZzjgModel> fzzlist = new List<ZzjgModel>();

                    //副职职务名称1      副职领导姓名1       副职领导1分管科室
                    if (dt_nksc.Rows[0]["fzzwmc1"].ToStringEx() != "" && dt_nksc.Rows[0]["ldfzmc1"].ToStringEx() != "")
                    {
                        //正职
                        zz = dt_nksc.Rows[0]["zzzwmc"].ToStringEx();
                        zzfgs.Name = "分管科室";
                        zzfgs.Childs = new List<ZzjgModel>();
                        string ldzzfg = dt_nksc.Rows[0]["ldzzfg"].ToStringEx().Replace("分管", "");
                        if (!string.IsNullOrEmpty(ldzzfg))
                        {
                            foreach (var itemldfg in ldzzfg.Split('、'))
                            {
                                ZzjgModel img_zzfg = new ZzjgModel() { Name = itemldfg };
                                zzfgs.Childs.Add(img_zzfg);
                            }
                        }

                        //副职
                        ZzjgModel img_fz = new ZzjgModel();
                        img_fz.Name = dt_nksc.Rows[0]["fzzwmc1"].ToStringEx();
                        img_fz.Childs = new List<ZzjgModel>();
                        string ldfzfg1 = dt_nksc.Rows[0]["ldfzfg1"].ToStringEx();
                        if (!string.IsNullOrEmpty(ldfzfg1))
                        {
                            foreach (var itemldfg in ldfzfg1.Split('、'))
                            {
                                ZzjgModel img_zzfg = new ZzjgModel() { Name = itemldfg };
                                img_fz.Childs.Add(img_zzfg);
                            }
                        }
                        else
                        {
                            ZzjgModel img_zzfg = new ZzjgModel() { Name = "" };
                            img_fz.Childs.Add(img_zzfg);
                        }
                        fzzlist.Add(img_fz);

                        foreach (Nksc_fz itemFZ in list_nkfz)
                        {
                            ZzjgModel img_fz2 = new ZzjgModel();
                            img_fz2.Name = itemFZ.fzzwmc;
                            img_fz2.Childs = new List<ZzjgModel>();
                            string ldfzfg2 = itemFZ.ldfzfg;
                            if (!string.IsNullOrEmpty(ldfzfg2))
                            {
                                foreach (var itemldfg in ldfzfg2.Split('、'))
                                {
                                    ZzjgModel img_zzfg = new ZzjgModel() { Name = itemldfg };
                                    img_fz2.Childs.Add(img_zzfg);
                                }
                            }
                            else
                            {
                                ZzjgModel img_zzfg = new ZzjgModel() { Name = "" };
                                img_fz2.Childs.Add(img_zzfg);
                            }
                            fzzlist.Add(img_fz2);
                        }
                    }
                    else
                    {
                        //正职
                        zz = dt_nksc.Rows[0]["zzzwmc"].ToStringEx();
                        zzfgs.Childs = new List<ZzjgModel>();
                        //副职
                        ZzjgModel img_fz = new ZzjgModel();
                        img_fz.Name = "分管科室";
                        img_fz.Childs = new List<ZzjgModel>();
                        string ldzzfg = dt_nksc.Rows[0]["ldzzfg"].ToStringEx().Replace("分管", "");
                        if (!string.IsNullOrEmpty(ldzzfg))
                        {
                            foreach (var itemldfg in ldzzfg.Split('、'))
                            {
                                ZzjgModel img_zzfg = new ZzjgModel() { Name = itemldfg };
                                img_fz.Childs.Add(img_zzfg);
                            }
                        }
                        fzzlist.Add(img_fz);
                    }

                    string imgFile = ImageSavePath + zzKey.DBKey;
                    Bitmap bit_img = new zzjgHelper().img_zzjg(zz, zzfgs, fzzlist);
                    bit_img.Save(imgFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    model.BookmarkerImage.Add(zzKey.WordKey, imgFile);
                }
                parentID = item.ID;//用于判断上一节是否为最大章节 下一张不需要另起一页
                queue_word.Enqueue(model);
            }

            AsposeWords apbll = new AsposeWords();
            apbll.AddDocs2DocByContinuous(queue_word, WordSavePath + fileName);
            return fileName;
        }
    }
}

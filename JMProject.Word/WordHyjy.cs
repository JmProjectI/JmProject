using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Aspose.Words;
using Aspose.Words.Replacing;
using Aspose.Words.Tables;

namespace JMProject.Word
{
    public class WordHyjy
    {
        string[] Workday = new string[] { "0211", "0224", "0408", "0428", "0929", "0930", "1229" };
        string[] Holiday = new string[] { "0101", "0215", "0216", "0219", "0220", "0221", "0405", "0406", "0430", "0501", "0618", "0924", "1001", "1002", "1003", "1004", "1005", "1231" };
        private FindReplaceOptions options = new FindReplaceOptions();

        public WordHyjy()
        {
            options.MatchCase = false;
            options.FindWholeWordsOnly = false;
        }

        /// <summary>
        /// 内部控制领导小组成立方案
        /// </summary>
        /// <returns></returns>
        public string Create1(string tempPath, string dir, DataTable dt_nksc, string Ldall)
        {
            string tempFile = "内部控制领导小组成立方案.docx";

            Document docFirst = new Document(tempPath + tempFile);

            string DWQC = dt_nksc.Rows[0]["DWQC"].ToStringEx();
            string nkldxzzz = dt_nksc.Rows[0]["nkldxzzz"].ToStringEx();
            string nkldxzfzz = dt_nksc.Rows[0]["nkldxzfzz"].ToStringEx();
            string nkldxzcy = dt_nksc.Rows[0]["nkldxzcy"].ToStringEx();

            docFirst.Range.Replace("DWQC", DWQC, options);

            Table table = docFirst.GetChildNodes(NodeType.Table, true)[0] as Table;

            SetValue(docFirst, table, 1, 1, nkldxzzz);
            SetValue(docFirst, table, 1, 2, Ldall.Contains(nkldxzzz) ? "是" : "否");

            SetValue(docFirst, table, 2, 1, nkldxzfzz);
            SetValue(docFirst, table, 2, 2, Ldall.Contains(nkldxzfzz) ? "是" : "否");

            string[] cys = nkldxzcy.Split('、');
            for (int i = 0; i < cys.Length; i++)
            {
                string item = cys[i];
                if (i == 0)
                {
                    SetValue(docFirst, table, 3, 1, item);
                    SetValue(docFirst, table, 3, 2, Ldall.Contains(item) ? "是" : "否");
                }
                else
                {
                    string[] value = new string[] { "", item, Ldall.Contains(item) ? "是" : "否" };
                    table.Rows.Add(CreteRow(docFirst, value));
                }
            }

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        private Row CreteRow(Document doc, string[] value)
        {
            Row r2 = new Row(doc);
            for (int i = 0; i < value.Length; i++)
            {
                r2.Cells.Add(CreateCell(value[i], doc));
            }
            return r2;
        }

        private Cell CreateCell(string value, Document doc)
        {
            Cell c1 = new Cell(doc);
            Paragraph p = new Paragraph(doc);
            p.AppendChild(new Run(doc, value));
            c1.AppendChild(p);
            return c1;
        }

        private void SetValue(Document doc, Table table, int row, int col, string value)
        {
            Cell cell = table.Rows[row].Cells[col];
            cell.FirstParagraph.Remove();
            //新建一个段落
            Paragraph p = new Paragraph(doc);
            p.ParagraphFormat.Style.Font.Size = 13;
            p.ParagraphFormat.Style.Font.Name = "幼圆";
            //把设置的值赋给之前新建的段落
            p.AppendChild(new Run(doc, value));
            //将此段落加到单元格内
            cell.AppendChild(p);
        }

        /// <summary>
        /// 内部控制工作小组成立方案
        /// </summary>
        /// <returns></returns>
        public string Create2(string tempPath, string dir, DataTable dt_nksc)
        {
            string tempFile = "内部控制工作小组成立方案.docx";

            Document docFirst = new Document(tempPath + tempFile);
            string nbkzgzxzzz01 = dt_nksc.Rows[0]["nbkzgzxzzz01"].ToStringEx();
            string nbkzgzxzfzz01 = dt_nksc.Rows[0]["nbkzgzxzfzz01"].ToStringEx();
            string nbkzgzxzzzcy01 = dt_nksc.Rows[0]["nbkzgzxzzzcy01"].ToStringEx();
            string nbkzgzxzzzqt01 = dt_nksc.Rows[0]["nbkzgzxzzzqt01"].ToStringEx();

            docFirst.Range.Replace("nbkzgzxzzz01", nbkzgzxzzz01, options);
            docFirst.Range.Replace("nbkzgzxzfzz01", nbkzgzxzfzz01, options);
            docFirst.Range.Replace("nbkzgzxzzzcy01", nbkzgzxzzzcy01, options);
            docFirst.Range.Replace("nbkzgzxzzzqt01", nbkzgzxzzzqt01, options);

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        /// <summary>
        /// 风险评估报告材料
        /// </summary>
        /// <returns></returns>
        public string Create3(string tempPath, string dir, string fxpgxzcy, string bhyw, string DWQC)
        {
            string tempFile = "风险评估报告材料.doc";

            Document docFirst = new Document(tempPath + tempFile);

            Random rnd = new Random();

            decimal wbfx = rnd.Next(40, 60); //外部风险      0.2
            decimal dwcm = rnd.Next(40, 60);//单位层面       0.2
            decimal ysyw = rnd.Next(40, 60); //预算业务       0.15
            decimal szyw = rnd.Next(40, 60); //收支业务       0.1      //ok
            decimal zcglyw = rnd.Next(40, 60); //资产管理业务 0.12     //ok
            decimal zfcgyw = rnd.Next(40, 60); //政府采购业务 0.08
            decimal jsxxyw = rnd.Next(40, 60); //建设项目业务 0.08
            decimal htglyw = rnd.Next(40, 60); //合同管理业务 0.07

            //预算业务
            if (bhyw.IndexOf('1') < 0)
            {
                ysyw = 0;
            }
            //合同管理业务
            if (bhyw.IndexOf('8') < 0)
            {
                htglyw = 0;
            }
            //建设项目业务
            if (bhyw.IndexOf('7') < 0)
            {
                jsxxyw = 0;
            }
            //政府采购业务
            if (bhyw.IndexOf('5') < 0)
            {
                zfcgyw = 0;
            }

            decimal zhdf = (decimal)wbfx * 0.2M
                + (decimal)dwcm * 0.2M
                + (decimal)ysyw * 0.15M
                + (decimal)szyw * 0.1M
                + (decimal)zfcgyw * 0.08M
                + (decimal)zcglyw * 0.12M
                + (decimal)jsxxyw * 0.08M
                + (decimal)htglyw * 0.07M;

            docFirst.Range.Replace("DWQC", DWQC, options);
            docFirst.Range.Replace("fxpgxzcy", fxpgxzcy, options);
            docFirst.Range.Replace("wbfx", wbfx.ToString("n2"), options);
            docFirst.Range.Replace("dwcm", dwcm.ToString("n2"), options);
            docFirst.Range.Replace("ysyw", ysyw.ToString("n2"), options);
            docFirst.Range.Replace("szyw", szyw.ToString("n2"), options);
            docFirst.Range.Replace("zfcgyw", zfcgyw.ToString("n2"), options);
            docFirst.Range.Replace("zcglyw", zcglyw.ToString("n2"), options);
            docFirst.Range.Replace("jsxxyw", jsxxyw.ToString("n2"), options);
            docFirst.Range.Replace("htglyw", htglyw.ToString("n2"), options);
            docFirst.Range.Replace("zhdf", zhdf.ToString("n2"), options);

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        /// <summary>
        /// 内部控制领导小组会议纪要
        /// </summary>
        /// <returns></returns>
        public DateTime Create4(string tempPath, string dir, DataTable dt_nksc)
        {
            string tempFile = "内部控制领导小组会议纪要.docx";

            Document docFirst = new Document(tempPath + tempFile);

            DateTime ldhysj = RadomDate("2018-01-01", "2018-04-30");
            string ldzzmc = dt_nksc.Rows[0]["ldzzmc"].ToStringEx();
            string nkldxzcy = dt_nksc.Rows[0]["nkldxzcy"].ToStringEx();
            string nkldxzzz = dt_nksc.Rows[0]["nkldxzzz"].ToStringEx();
            string nkldxzfzz = dt_nksc.Rows[0]["nkldxzfzz"].ToStringEx();

            docFirst.Range.Replace("ldhysj", ldhysj.ToString("yyyy年MM月dd日"), options);
            docFirst.Range.Replace("ldzzmc", ldzzmc, options);
            docFirst.Range.Replace("nkldxzcy", nkldxzcy, options);
            docFirst.Range.Replace("nkldxzzz", nkldxzzz, options);
            docFirst.Range.Replace("nkldxzfzz", nkldxzfzz, options);

            docFirst.Save(dir + tempFile);
            return ldhysj;
        }

        /// <summary>
        /// 内部控制培训纪要
        /// </summary>
        /// <returns></returns>
        public string Create5(string tempPath, string dir, string ldzzmc,string DWQC, DateTime ldhysj)
        {
            string tempFile = "内部控制培训纪要.docx";

            Document docFirst = new Document(tempPath + tempFile);

            DateTime pxsj = RadomDate(ldhysj.ToString("yyyy-MM-dd"), "2018-11-30");

            docFirst.Range.Replace("pxsj", pxsj.ToString("yyyy年MM月dd日"), options);
            docFirst.Range.Replace("ldzzmc", ldzzmc, options);
            docFirst.Range.Replace("DWQC", DWQC, options);

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        /// <summary>
        /// 内部控制工作小组会议纪要（第一次）
        /// </summary>
        /// <returns></returns>
        public string Create6(string tempPath, string dir, string ldzzmc, string nbkzgzxzzzcy01, DateTime ldhysj)
        {
            string tempFile = "内部控制工作小组会议纪要（第一次）.docx";

            Document docFirst = new Document(tempPath + tempFile);

            DateTime xzhysj = RadomDate(ldhysj.ToString("yyyy-MM-dd"), "2018-06-30");

            docFirst.Range.Replace("xzhysj", xzhysj.ToString("yyyy年MM月dd日"), options);
            docFirst.Range.Replace("ldzzmc", ldzzmc, options);
            docFirst.Range.Replace("nbkzgzxzzzcy01", nbkzgzxzzzcy01, options);

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        /// <summary>
        /// 内部控制工作小组会议纪要（第二次）
        /// </summary>
        /// <returns></returns>
        public string Create7(string tempPath, string dir, string ldzzmc, string nbkzgzxzzzcy01)
        {
            string tempFile = "内部控制工作小组会议纪要（第二次）.docx";

            Document docFirst = new Document(tempPath + tempFile);

            DateTime xzhysj2 = RadomDate("2018-10-31", "2018-12-31");

            docFirst.Range.Replace("xzhysj2", xzhysj2.ToString("yyyy年MM月dd日"), options);
            docFirst.Range.Replace("ldzzmc", ldzzmc, options);
            docFirst.Range.Replace("nbkzgzxzzzcy01", nbkzgzxzzzcy01, options);

            docFirst.Save(dir + tempFile);
            return dir + tempFile;
        }

        private DateTime RadomDate(string start, string end)
        {
            DateTime dtBegin = DateTime.Parse(start);
            DateTime dtEnd = DateTime.Parse(end);
            TimeSpan ts = dtEnd - dtBegin;
            Random rnd = new Random();
        Start: int days = rnd.Next(ts.Days);
            DateTime Value = dtBegin.AddDays(days);
            if (Value.DayOfWeek == DayOfWeek.Saturday || Value.DayOfWeek == DayOfWeek.Sunday)
            {
                if (!Workday.Contains(Value.ToString("mmdd")))
                {
                    goto Start;
                }
            }
            else if (Holiday.Contains(Value.ToString("mmdd")))
            {
                goto Start;
            }
            return Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words.Replacing;
using Aspose.Words;
using System.Collections;
using System.Drawing;
using System.IO;
using Aspose.Words.Tables;

namespace JMProject.Word
{
    internal class AsposeWords
    {
        private FindReplaceOptions options = new FindReplaceOptions();

        public AsposeWords()
        {
            options.MatchCase = false;
            options.FindWholeWordsOnly = false;

        }

        public string AddDocs2DocByContinuous(Queue paths, string outputPath)
        {
            WordModel model = paths.Dequeue() as WordModel;
            Document docFirst = new Document(model.File);
            //删除书签
            RemoveBookMarks(docFirst, model.BookmarkerTextRang);
            //替换关键字
            ReplaceKey(docFirst, model.Keyword);
            //书签插入图片
            InsertImageBookMarks(docFirst, model.BookmarkerImage);

            while (paths.Count > 0)
            {
                WordModel modelother = paths.Dequeue() as WordModel;
                string outFile = Path.GetDirectoryName(modelother.File) + "\\" + Path.GetFileNameWithoutExtension(modelother.File) + "_new" + Path.GetExtension(modelother.File);
                //new SpireDoc().RemoveBookMarkContent(outFile, modelother.File, modelother.BookmarkerTextRang);
                new OfficeWords().ReplaceBook(outFile, modelother.File, modelother.BookmarkerTextRang);
                Document doc = new Document(outFile);
                //删除书签
                //RemoveBookMarks(doc, modelother.BookmarkerTextRang);
                //替换固定关键字
                ReplaceKey(doc, modelother.KeywordGD);
                //替换关键字
                ReplaceKey(doc, modelother.Keyword);
                //书签插入图片
                InsertImageBookMarks(doc, modelother.BookmarkerImage);
                //是否另起一页
                doc.FirstSection.PageSetup.SectionStart = modelother.NewPage == "01" ? SectionStart.NewPage : SectionStart.Continuous;

                //NewColumn节的结尾  OddPage新奇数页  EvenPage偶数页
                //追加文档
                docFirst.AppendDocument(doc, ImportFormatMode.UseDestinationStyles);

                File.Delete(outFile);
            }

            docFirst.Save(outputPath);
            return outputPath;
        }

        /// <summary>
        /// 删除书签中内容
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="BookmarkerTextRang">书签名集合</param>
        public void RemoveBookMarks(Document doc, Dictionary<string, string> BookmarkerTextRang)
        {
            foreach (var Rangitem in BookmarkerTextRang)
            {
                if (Rangitem.Key == "ywcm_szyw_czsqzf1")
                {
                    continue;
                }
                Bookmark bookmark = doc.Range.Bookmarks[Rangitem.Key];
                if (bookmark != null)
                {
                    if (bookmark.Text != "\r")
                    {
                        bookmark.Text = "";
                    }
                    bookmark.Remove();
                }
            }
        }

        /// <summary>
        /// 替换关键字
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="Keyword">关键字与要替换内容集合</param>
        public void ReplaceKey(Document doc, Dictionary<string, string> Keyword)
        {
            foreach (var item in Keyword)
            {
                if (item.Key == "key_qlqd")
                {
                    string[] docValue = item.Value.Split('☆');
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToBookmark(item.Key);

                    foreach (string qditem in docValue)
                    {
                        if (!qditem.Contains("★"))
                        {
                            foreach (var itemline in qditem.Split('&'))
                            {
                                if (!string.IsNullOrEmpty(itemline))
                                {
                                    builder.Writeln(itemline);
                                }
                            }
                        }
                        else
                        {
                            string[] qdStrs = qditem.Split('★');
                            builder.StartTable();
                            for (int i = 0; i < qdStrs.Length; i += 3)
                            {
                                if (i == 0)
                                {
                                    Cell c1 = builder.InsertCell();
                                    c1.CellFormat.Width = 60;
                                    builder.Write("序号");
                                    Cell c2 = builder.InsertCell();
                                    c2.CellFormat.Width = 100;
                                    builder.Write("权力属性");
                                    Cell c3 = builder.InsertCell();
                                    c3.CellFormat.Width = 340;
                                    builder.Write("权力范围");
                                    builder.EndRow();
                                }
                                if (i + 2 < qdStrs.Length)
                                {
                                    Cell cc1 = builder.InsertCell();
                                    builder.Write(qdStrs[i]);
                                    cc1.CellFormat.Width = 60;
                                    Cell cc2 = builder.InsertCell();
                                    builder.Write(qdStrs[i + 1]);
                                    cc2.CellFormat.Width = 100;
                                    Cell cc3 = builder.InsertCell();
                                    cc3.CellFormat.Width = 340;

                                    //builder.Write(qdStrs[i + 2]);
                                    string[] qdtexts = qdStrs[i + 2].Split('&');
                                    for (int m = 0; m < qdtexts.Length; m++)
                                    {
                                        if (!string.IsNullOrEmpty(qdtexts[m]))
                                        {
                                            if (m == qdtexts.Length - 1)
                                            {
                                                builder.Write(qdtexts[m]);
                                            }
                                            else
                                            {
                                                builder.Writeln(qdtexts[m]);
                                            }
                                        }
                                    }
                                    builder.EndRow();
                                }
                            }
                            builder.EndTable();
                        }
                    }
                    //
                }
                else
                {
                    doc.Range.Replace(item.Key, item.Value, options);
                }
            }
        }

        /// <summary>
        /// 书签处插入图片
        /// </summary>
        /// <param name="doc">文档对象</param>
        /// <param name="BookmarkerImage">书签名与图片路径集合</param>
        public void InsertImageBookMarks(Document doc, Dictionary<string, string> BookmarkerImage)
        {
            foreach (var imgitem in BookmarkerImage)
            {
                if (doc.Range.Bookmarks[imgitem.Key] != null)
                {
                    DocumentBuilder docbuilder = new DocumentBuilder(doc);
                    docbuilder.MoveToBookmark(imgitem.Key);
                    if (System.IO.File.Exists(imgitem.Value))
                    {
                        docbuilder.InsertImage(imgitem.Value);
                    }
                    else
                    {
                        docbuilder.Write("book没找到图片文件");
                    }
                }
            }
        }
    }
}

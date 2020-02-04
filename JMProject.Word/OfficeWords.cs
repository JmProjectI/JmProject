using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using word = Microsoft.Office.Interop.Word;

namespace JMProject.Word
{
    public class OfficeWords
    {
        public void ReplaceBook(string outFile, object fileName, Dictionary<string, string> BookmarkerTextRang)
        {
            object oMissing = System.Reflection.Missing.Value;
            word.Application app = null;
            word.Document doc = null;
            try
            {
                app = new word.Application();//创建word应用程序
                //打开模板文件
                doc = app.Documents.Open(ref fileName,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                foreach (var bookNameItem in BookmarkerTextRang)
                {
                    //ywcm_zfcg_zfgmff 包含在 ywcm_zfcg_zfcg 中
                    if (bookNameItem.Key=="ywcm_zfcg_zfgmff")
                    {
                        if (BookmarkerTextRang.ContainsKey("ywcm_zfcg_zfcg"))
                        {
                            continue;
                        }
                    }
                    object bookName = bookNameItem.Key;
                    word.Bookmark bookmark = doc.Bookmarks.get_Item(ref bookName);
                    bookmark.Range.Text = "";
                }

                //对替换好的word模板另存为一个新的word文档
                doc.SaveAs(outFile,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            }
            catch (Exception ex)
            {
                //System.IO.FileStream fs = new System.IO.FileStream("D:\\log.txt", System.IO.FileMode.Create);
                ////获得字节数组
                //byte[] data = System.Text.Encoding.Default.GetBytes(ex.Message);
                ////开始写入
                //fs.Write(data, 0, data.Length);
                ////清空缓冲区、关闭流
                //fs.Flush();
                //fs.Close();
                throw ex;
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
    }
}

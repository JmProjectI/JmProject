using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spire.Doc;
using Spire.Doc.Documents;

namespace JMProject.Word
{
    public class SpireDoc
    {
        /// <summary>
        /// 删除书签
        /// </summary>
        /// <param name="outFile">新文件</param>
        /// <param name="File">源文件</param>
        /// <param name="BookMarkName">书签名称</param>
        /// <returns></returns>
        public bool RemoveBookMarkContent(string outFile, string File, Dictionary<string, string> BookmarkerTextRang)
        {

            Document document = new Document();
            document.LoadFromFile(File, FileFormat.Docx);
            try
            {
                foreach (var BookMarkName in BookmarkerTextRang)
                {
                    if (BookMarkName.Key == "ywcm_szyw_czsqzf1")
                    {
                        continue;
                    }
                    BookmarksNavigator navigator = new BookmarksNavigator(document);
                    
                    navigator.MoveToBookmark(BookMarkName.Key);//指向特定书签
                    navigator.DeleteBookmarkContent(false);//删除原有书签内容
                }
            }
            catch(Exception ee)
            {
                
            }

            document.SaveToFile(outFile, FileFormat.Docx);
            document.Close();
            return true;
        }
    }
}

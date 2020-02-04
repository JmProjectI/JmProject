using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Word
{
    public class WordModel
    {
        public WordModel()
        {
            KeywordGD = new Dictionary<string, string>();
            Keyword = new Dictionary<string, string>();
            BookmarkerTextRang = new Dictionary<string, string>();
            BookmarkerImage = new Dictionary<string, string>();
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// 固定替换 再关键字之前替换
        /// </summary>
        public Dictionary<string, string> KeywordGD { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public Dictionary<string, string> Keyword { get; set; }
        /// <summary>
        /// 书签（内容范围）
        /// </summary>
        public Dictionary<string, string> BookmarkerTextRang { get; set; }
        /// <summary>
        /// 书签（插入图片）
        /// </summary>
        public Dictionary<string, string> BookmarkerImage { get; set; }
        /// <summary>
        /// 是否另起一页  00否  01是
        /// </summary>
        public string NewPage { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JMProject.Common
{
    public class ImgModel
    {
        /// <summary>
        /// 正职
        /// </summary>
        public string zz { get; set; }
        /// <summary>
        /// 副职
        /// </summary>
        public string fz { get; set; }
        /// <summary>
        /// 牵头科室
        /// </summary>
        public string qtks { get; set; }
        /// <summary>
        /// 成员
        /// </summary>
        public string cy { get; set; }
        /// <summary>
        /// 类型  小组  组织架构  固定图片
        /// </summary>
        public ImgType imgtype { get; set; }
        /// <summary>
        /// 正职分管
        /// </summary>
        public ImageName zzfgs { get; set; }
        /// <summary>
        /// 副职分管
        /// </summary>
        public List<ImageName> fzzlist { get; set; }


        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImgFileName { get; set; }
        /// <summary>
        /// 需要替换的文字
        /// </summary>
        public List<string> ImgTitle { get; set; }
        /// <summary>
        /// 需要替换的文字
        /// </summary>
        public List<Font> ImgTitleFont { get; set; }
        /// <summary>
        /// 需要替换的文字
        /// </summary>
        public List<Rectangle> ImgTitleRect { get; set; }
    }

    /// <summary>
    /// 图片类型 XZ=小组图 ZZJG=组织架构图 LCT=流程图
    /// </summary>
    public enum ImgType
    { 
        /// <summary>
        /// 小组图
        /// </summary>
        XZ,
        /// <summary>
        /// 组织架构图
        /// </summary>
        ZZJG,
        /// <summary>
        /// 流程图
        /// </summary>
        LCT
    }
}

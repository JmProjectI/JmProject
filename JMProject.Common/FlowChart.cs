using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JMProject.Common
{
    public class FlowChart
    {
        private Font font = new Font("宋体", 11);
        private Font font1 = new Font("黑体", 10);//, FontStyle.Bold
        private SolidBrush brush_White = new SolidBrush(Color.White);
        private SolidBrush brush_Black = new SolidBrush(Color.Black);
        private Pen pen_Black = new Pen(Color.Black);

        /// <summary>
        /// 横向 线条 长度
        /// </summary>
        private int w_C = 50;
        /// <summary>
        /// 竖向 线条 长度
        /// </summary>
        private int h_C = 30;

        /// <summary>
        /// 横向 方块 高度
        /// </summary>
        private int mHeight = 35;
        /// <summary>
        /// 横向 方块 宽度
        /// </summary>
        private int mWidth = 150;
        /// <summary>
        /// 横向方块 大小
        /// </summary>
        Size recSize;
        /// <summary>
        /// 横向方块 自动变 大小
        /// </summary>
        Size recSizeW;
        /// <summary>
        /// 起始点 x坐标
        /// </summary>
        private int startX = 225;
        /// <summary>
        /// 起始点 y坐标
        /// </summary>
        private int startY = 10;

        /// <summary>
        /// 竖向 方块 高度
        /// </summary>
        private int hHeight = 160;
        /// <summary>
        /// 竖向 方块 宽度
        /// </summary>
        private int hWidth = 35;
        /// <summary>
        /// 横向方块 大小
        /// </summary>
        Size recSize_h;
        /// <summary>
        /// 竖向方块 横向 间隔
        /// </summary>
        private int hpadding = 30;
        /// <summary>
        /// 竖向方块 竖向 间隔
        /// </summary>
        private int vpadding = 20;

        #region 构造函数
        public FlowChart()
        {
            recSize = new Size(mWidth, mHeight);
            recSizeW = new Size(mWidth, mHeight);
            recSize_h = new Size(hWidth, hHeight);
        }
        #endregion

        /// <summary>
        /// 生成各各小组图片
        /// </summary>
        /// <param name="zz"></param>
        /// <param name="fzz"></param>
        /// <param name="qtks"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
        public Bitmap FXPG(string zz, string fzz, string qtks, string cy)
        {
            recSize_h = new Size(hWidth, hHeight);
            recSizeW = new Size(mWidth, mHeight);

            Bitmap bmp = new Bitmap(600, 400);
            string[] cys = cy.Split('、');

            int addh = 0;
            int maxlength = 0;
            for (int i = 0; i < cys.Length; i++)
            {
                if (cys[i].Length > maxlength)
                {
                    maxlength = cys[i].Length;
                }
            }
            if (maxlength > 9)
            {
                addh = (maxlength - 9) * 25;
                recSize_h = new Size(hWidth, hHeight + addh);
            }

            int addw = 0;
            int maxlengthW = zz.Length > fzz.Length ? zz.Length : fzz.Length;
            maxlengthW = maxlengthW > qtks.Length ? maxlengthW : qtks.Length;
            if (maxlengthW > 9)
            {
                addw = (maxlengthW - 9) * 18;
                recSizeW = new Size(mWidth + addw, mHeight);
            }
            bmp = new Bitmap(600 + addw, 400 + addh);

            int cc = 0;//第几级
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(brush_White, 0, 0, 600 + addw, 400 + addh);
            DrawLineRS(g, cc++, "组长", zz);

            if (!string.IsNullOrEmpty(fzz))
            {
                DrawLineRS(g, cc++, "副组长", fzz);
            }

            DrawLineRS(g, cc, "成员", qtks);


            if (cys.Length > 0)
            {
                int cycount = cys.Length - 1;
            Check:
                int startP_x = startX + mWidth / 2 - (hWidth + hpadding) * cycount / 2;
                if (startP_x < hWidth / 2)
                {
                    if (hpadding > 5)
                    {
                        hpadding -= 5;
                        goto Check;
                    }
                    else
                    {
                        hWidth -= 5;
                        recSize_h = new Size(hWidth, hHeight + addh);
                        goto Check;
                    }
                }
                int startP_y = startY + (mHeight + h_C) * cc + mHeight + h_C;

                g.DrawLine(pen_Black, new Point(startX + mWidth / 2 - (hWidth + hpadding) * cycount / 2, startY + (mHeight + h_C) * cc + mHeight + h_C)
                    , new Point(startX + mWidth / 2 + (hWidth + hpadding) * cycount / 2, startY + (mHeight + h_C) * cc + mHeight + h_C));

                for (int i = 0; i < cys.Length; i++)
                {
                    g.DrawLine(pen_Black, new Point(startP_x + (hWidth + hpadding) * i, startP_y)
                        , new Point(startP_x + (hWidth + hpadding) * i, startP_y + vpadding));
                    Rectangle rec = new Rectangle(new Point(startP_x + (hWidth + hpadding) * i - hWidth / 2, startP_y + vpadding), recSize_h);
                    DrawRS(g, rec, cys[i]);
                }
            }
            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// 画一行中所有方块与内容 （2方块与内容、一个横线、一个竖线）
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="title"></param>
        /// <param name="titleName"></param>
        private void DrawLineRS(Graphics g, int row, string title, string titleName)
        {
            Rectangle rec = new Rectangle(new Point(startX, startY + (mHeight + h_C) * row), recSize);

            DrawRS(g, rec, title);

            if (!string.IsNullOrEmpty(titleName))
            {
                //横线
                g.DrawLine(pen_Black, new Point(startX + mWidth, startY + (mHeight + h_C) * row + mHeight / 2)
                    , new Point(startX + mWidth + w_C, startY + (mHeight + h_C) * row + mHeight / 2));

                rec = new Rectangle(new Point(startX + mWidth + w_C, startY + (mHeight + h_C) * row), recSizeW);
                DrawRS(g, rec, titleName);
            }

            //竖线
            g.DrawLine(pen_Black, new Point(startX + mWidth / 2, startY + (mHeight + h_C) * row + mHeight)
                , new Point(startX + mWidth / 2, startY + (mHeight + h_C) * row + mHeight + h_C));
        }

        /// <summary>
        /// 画一个方块与 内部内容
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rec"></param>
        /// <param name="str"></param>
        private void DrawRS(Graphics g, Rectangle rec, string str)
        {
            g.DrawRectangle(pen_Black, rec);
            StringFormat sFormat = new StringFormat();
            sFormat.Alignment = StringAlignment.Center;
            sFormat.LineAlignment = StringAlignment.Center;

            Rectangle fontrec = new Rectangle(rec.X, rec.Y, rec.Width, rec.Height + 5);
            g.DrawString(str, font, brush_Black, fontrec, sFormat);
        }

        /// <summary>
        /// 图片中添加文字并生成新图
        /// </summary>
        /// <param name="imgFile">原图路径</param>
        /// <param name="texts">待填写文字</param>
        /// <param name="mfonts"></param>
        /// <param name="fontrecs"></param>
        /// <returns></returns>
        public Image EditImageText(string imgFile, List<string> texts, List<Font> mfonts, List<Rectangle> fontrecs)
        {
            Image bmp = Image.FromFile(imgFile);
            if (texts != null && texts.Count > 0)
            {
                Graphics g = Graphics.FromImage(bmp);
                StringFormat sFormat = new StringFormat();
                sFormat.Alignment = StringAlignment.Center;
                sFormat.LineAlignment = StringAlignment.Center;
                for (int i = 0; i < texts.Count; i++)
                {
                    g.DrawString(texts[i], mfonts[i], brush_Black, fontrecs[i], sFormat);
                }
                g.Dispose();
            }            
            return bmp;
        }

        /// <summary>
        /// 单位组织架构图
        /// </summary>
        /// <param name="zz">正职</param>
        /// <param name="fz">副职</param>
        /// <returns></returns>
        public Bitmap img_zzjg(string zz, ImageName zzfgs, List<ImageName> fz)
        {
            //1、对第三级进行排序
            //fz.Sort(SortCompare);

            #region 2、获取单位组织架构图 第三级的数量
            List<string> allChild = new List<string>();
            foreach (var item in zzfgs.Childs)
            {
                allChild.Add(item.Name);
            }
            foreach (var item in fz)
            {
                foreach (var child in item.Childs)
                {
                    allChild.Add(child.Name);
                }
            }
            //所有第三级数量
            int cycount = allChild.Count - 1;
            #endregion

            int gpWidth = 600;//默认画布宽度
            int gpHeight = 480;//默认画布高度

            #region 获取第三级最大的宽度、重新设置第三级方块的高度、重设置画布高度
            Bitmap bitmap = new Bitmap(gpWidth, gpHeight);
            Graphics gg = Graphics.FromImage(bitmap);
            for (int k = 0; k < allChild.Count; k++)
            {
                SizeF siF = gg.MeasureString(allChild[k], font);
                if (hHeight < siF.Width + 30)
                {
                    //方块高度
                    hHeight = Convert.ToInt32(siF.Width + 30);
                    recSize_h = new Size(hWidth, hHeight);
                    //画布高度
                    gpHeight = 480 + (Convert.ToInt32(siF.Width) - 160) + 30;
                }
            }
            #endregion

            if (allChild.Count > 10 && allChild.Count <= 20)
            {
                gpWidth = 700;//默认画布宽度
            }
            else if (allChild.Count > 20)
            {
                gpWidth = 1500;//默认画布宽度
            }
            Bitmap bmp = new Bitmap(gpWidth, gpHeight);
            Graphics g = Graphics.FromImage(bmp);
            //白色背景
            g.FillRectangle(brush_White, 0, 0, gpWidth, gpHeight);
            //正值方块起始位置
            int startX = (gpWidth - recSize.Width) / 2;
            //画 正职方块与名称 （顶部最中间）
            Rectangle rec = new Rectangle(new Point(startX, startY), recSize);
            DrawRS(g, rec, zz);

            //正职方块 下面 竖线
            g.DrawLine(pen_Black, new Point(startX + mWidth / 2, startY + mHeight)
                , new Point(startX + mWidth / 2, startY + mHeight + vpadding));

            int first_x = gpWidth;//画布宽度
            int last_x = 0;

            #region 画 第二级
        Check:
            int startP_x = startX + mWidth / 2 - (hWidth + hpadding) * cycount / 2;
            if (startP_x < hWidth / 2)
            {
                if (hpadding > 5)
                {
                    hpadding -= 5;
                    goto Check;
                }
                else
                {
                    hWidth -= 5;
                    recSize_h = new Size(hWidth, hHeight + 30);
                    goto Check;
                }
            }
            int startP_y = startY + (mHeight + h_C) * 2 + mHeight + h_C + 50;

            int startP_Line_x = startP_x;
            if (zzfgs.Childs.Count > 0)
            {
                if (zzfgs.Childs.Count == 1)
                {
                    //上面竖线
                    g.DrawLine(pen_Black, new Point(startP_Line_x, startY + mHeight + vpadding)
            , new Point(startP_Line_x, startY + mHeight + vpadding + vpadding));

                    //下面竖线
                    g.DrawLine(pen_Black, new Point(startP_Line_x, startP_y)
                        , new Point(startP_Line_x, startP_y - vpadding));

                    Rectangle recKS = new Rectangle(new Point(startP_Line_x - 30 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, 150));
                    DrawRS(g, recKS, zzfgs.Name);

                    first_x = first_x < startP_Line_x ? first_x : startP_Line_x;
                    last_x = last_x > startP_Line_x ? last_x : startP_Line_x;

                    startP_Line_x += hWidth + hpadding;
                }
                else
                {
                    //横线
                    g.DrawLine(pen_Black, new Point(startP_Line_x, startP_y)
                        , new Point(startP_Line_x + (hWidth + hpadding) * (zzfgs.Childs.Count - 1), startP_y));

                    //竖线
                    int middle_x = startP_Line_x + (hWidth + hpadding) * (zzfgs.Childs.Count - 1) / 2;

                    //上面竖线
                    g.DrawLine(pen_Black, new Point(middle_x, startY + mHeight + vpadding)
            , new Point(middle_x, startY + mHeight + vpadding + vpadding));

                    //下面竖线
                    g.DrawLine(pen_Black, new Point(middle_x, startP_y)
                        , new Point(middle_x, startP_y - vpadding));

                    Rectangle recKS = new Rectangle(new Point(middle_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, 150));
                    DrawRS(g, recKS, zzfgs.Name);

                    first_x = first_x < middle_x ? first_x : middle_x;

                    startP_Line_x += (hWidth + hpadding) * zzfgs.Childs.Count;
                }
            }

            foreach (var item in fz)
            {
                if (item.Childs.Count > 0)
                {
                    if (item.Childs.Count == 1)
                    {
                        //上面竖线
                        g.DrawLine(pen_Black, new Point(startP_Line_x, startY + mHeight + vpadding)
                , new Point(startP_Line_x, startY + mHeight + vpadding + vpadding));

                        //下面竖线
                        g.DrawLine(pen_Black, new Point(startP_Line_x, startP_y)
                            , new Point(startP_Line_x, startP_y - vpadding));

                        Rectangle recKS = new Rectangle(new Point(startP_Line_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, 150));
                        DrawRS(g, recKS, item.Name);

                        first_x = first_x < startP_Line_x ? first_x : startP_Line_x;
                        last_x = last_x > startP_Line_x ? last_x : startP_Line_x;

                        startP_Line_x += hWidth + hpadding;
                    }
                    else
                    {
                        g.DrawLine(pen_Black, new Point(startP_Line_x, startP_y)
                            , new Point(startP_Line_x + (hWidth + hpadding) * (item.Childs.Count - 1), startP_y));

                        int middle_x = startP_Line_x + (hWidth + hpadding) * (item.Childs.Count - 1) / 2;

                        //上面竖线
                        g.DrawLine(pen_Black, new Point(middle_x, startY + mHeight + vpadding)
                , new Point(middle_x, startY + mHeight + vpadding + vpadding));

                        //下面竖线
                        g.DrawLine(pen_Black, new Point(middle_x, startP_y)
                            , new Point(middle_x, startP_y - vpadding));

                        Rectangle recKS = new Rectangle(new Point(middle_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, 150));
                        DrawRS(g, recKS, item.Name);

                        first_x = first_x < middle_x ? first_x : middle_x;
                        last_x = last_x > middle_x ? last_x : middle_x;

                        startP_Line_x += (hWidth + hpadding) * item.Childs.Count;
                    }
                }
            }
            #endregion

            #region 画 第三级
            for (int i = 0; i < allChild.Count; i++)
            {
                g.DrawLine(pen_Black, new Point(startP_x + (hWidth + hpadding) * i, startP_y)
                    , new Point(startP_x + (hWidth + hpadding) * i, startP_y + vpadding));
                Rectangle recKS = new Rectangle(new Point(startP_x + (hWidth + hpadding) * i - hWidth / 2, startP_y + vpadding), recSize_h);
                DrawRS(g, recKS, allChild[i]);
            }
            #endregion

            g.DrawLine(pen_Black, new Point(first_x, startY + mHeight + vpadding)
                , new Point(last_x, startY + mHeight + vpadding));

            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// 对List<ImageName>进行排序时作为参数使用
        /// </summary>
        /// <param name="AF1"></param>
        /// <param name="AF2"></param>
        /// <returns></returns>
        private static int SortCompare(ImageName AF1, ImageName AF2)
        {
            int res = 0;
            if (AF1.Childs.Count > AF2.Childs.Count)
            {
                res = -1;
            }
            else if (AF1.Childs.Count < AF2.Childs.Count)
            {
                res = 1;
            }
            return res;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JMProject.Word
{
    public class zzjgHelper
    {
        private Font font = new Font("宋体", 11);
        private Pen pen_Black = new Pen(Color.Black);
        private SolidBrush brush_Black = new SolidBrush(Color.Black);
        private SolidBrush brush_White = new SolidBrush(Color.White);

        /// <summary>
        /// 画一个矩形方块方块 与 居中的内部 文本
        /// </summary>
        /// <param name="g">画刷</param>
        /// <param name="rec">矩形方块</param>
        /// <param name="str">文本</param>
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
        /// 组织架构图
        /// </summary>
        /// <param name="zz">正职名称</param>
        /// <param name="zzfgs">正职分管科室</param>
        /// <param name="fz">副职与副职分管科室</param>
        /// <returns></returns>
        public Bitmap img_zzjg(string zz, ZzjgModel zzfgs, List<ZzjgModel> fz)
        {
            //int h_C = 30;// 竖向 线条 长度
            int vpadding = 20;// 竖向方块 竖向 间隔
            int hpadding = 30;// 竖向方块 横向 间隔
            int startY = 10;// 起始点 y坐标 最高处
            int mHeight = 35;// 横向 方块 高度
            int mWidth = 150;// 横向 方块 宽度
            int hHeight = 160;//3级 竖向 方块 高度
            int hHeight2 = 160;//2级 竖向 方块 高度
            int hWidth = 35;// 竖向 方块 宽度
            int gpWidth = 600;//默认画布宽度
            int gpHeight = 480;//默认画布高度
            //Size recSize_h = new Size(hWidth, hHeight);// 竖向 方块 尺寸

            #region 2、获取单位组织架构图 第三级的数量
            List<string> allChild = new List<string>();//所有分管科室
            List<string> allfzName = new List<string>();//所有副职名称
            foreach (var item in zzfgs.Childs)
            {
                allChild.Add(item.Name);
            }
            foreach (var item in fz)
            {
                allfzName.Add(item.Name);
                foreach (var child in item.Childs)
                {
                    allChild.Add(child.Name);
                }
            }
            //所有第三级数量
            int cycount = allChild.Count - 1;
            #endregion

            #region 设置 第二级 与 第三级 方块的高度
            Bitmap bitmap = new Bitmap(gpWidth, gpHeight);
            Graphics gg = Graphics.FromImage(bitmap);
            //第二级 方块的高度
            for (int k = 0; k < allfzName.Count; k++)
            {
                SizeF siF = gg.MeasureString(allfzName[k], font);
                if (hHeight2 < siF.Width + 30)
                {
                    //方块高度(竖向) = 字体宽度+30(横向)
                    hHeight2 = Convert.ToInt32(siF.Width + 30);
                }
            }
            //第三级 方块的高度
            for (int k = 0; k < allChild.Count; k++)
            {
                SizeF siF = gg.MeasureString(allChild[k], font);
                if (hHeight < siF.Width + 30)
                {
                    int offsetH = 0;
                    if (allChild[k].IndexOf("（") >= 0)
                    {
                        offsetH = 15;
                    }
                    //方块高度(竖向) = 字体宽度+30(横向)
                    hHeight = Convert.ToInt32(siF.Width + 30 + offsetH);
                }
            }
            #endregion

            //重设置画布高度
            gpHeight = 480 + (hHeight2 - 160) + (hHeight - 160);

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
            int startX = (gpWidth - mWidth) / 2;
            //画 正职方块与名称 （顶部最中间）
            Rectangle rec = new Rectangle(startX, startY, mWidth, mHeight);
            DrawRS(g, rec, zz);

            //正职方块 下面 竖线
            g.DrawLine(pen_Black, new Point(startX + mWidth / 2, startY + mHeight)
                , new Point(startX + mWidth / 2, startY + mHeight + vpadding));

            int first_x = gpWidth;//画布宽度
            int last_x = 0;

            //设置 第二三级 方块宽度
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
                    goto Check;
                }
            }

            #region 画 第二级
            // 开始Y + 
            //int startP_y = startY + (mHeight + h_C) * 2 + mHeight + h_C + 50;
            int startP_y = startY + mHeight + vpadding + vpadding + hHeight2 + vpadding;

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

                    Rectangle recKS = new Rectangle(new Point(startP_Line_x - 30 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, hHeight2));
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

                    Rectangle recKS = new Rectangle(new Point(middle_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, hHeight2));
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

                        //副职下级 名称为空  不画下面的线
                        if (!string.IsNullOrEmpty(item.Childs[0].Name))
                        {
                            //下面竖线
                            g.DrawLine(pen_Black, new Point(startP_Line_x, startP_y)
                                , new Point(startP_Line_x, startP_y - vpadding));
                        }

                        Rectangle recKS = new Rectangle(new Point(startP_Line_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, hHeight2));
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

                        Rectangle recKS = new Rectangle(new Point(middle_x - 35 / 2, startY + mHeight + vpadding + vpadding), new Size(hWidth, hHeight2));
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
                //名称为空 不画
                if (!string.IsNullOrEmpty(allChild[i]))
                {
                    g.DrawLine(pen_Black, new Point(startP_x + (hWidth + hpadding) * i, startP_y)
                        , new Point(startP_x + (hWidth + hpadding) * i, startP_y + vpadding));
                    Rectangle recKS = new Rectangle(new Point(startP_x + (hWidth + hpadding) * i - hWidth / 2, startP_y + vpadding), new Size(hWidth, hHeight));
                    DrawRS(g, recKS, allChild[i]);
                }
            }
            #endregion

            g.DrawLine(pen_Black, new Point(first_x, startY + mHeight + vpadding)
                , new Point(last_x, startY + mHeight + vpadding));

            g.Dispose();
            return bmp;
        }
    }
}

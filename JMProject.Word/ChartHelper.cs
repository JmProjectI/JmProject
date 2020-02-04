using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JMProject.Word
{
    public class ChartHelper
    {
        private Font font = new Font("宋体", 11);

        private Pen pen_Black = new Pen(Color.Black);
        private SolidBrush brush_Black = new SolidBrush(Color.Black);
        private SolidBrush brush_White = new SolidBrush(Color.White);

        /// <summary>
        /// 横向方块 大小
        /// </summary>
        Size recSize;

        /// <summary>
        /// 起始点 x坐标
        /// </summary>
        private int startX = 225;
        /// <summary>
        /// 起始点 y坐标
        /// </summary>
        private int startY = 10;

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
        /// 横向方块 自动变 大小
        /// </summary>
        Size recSizeW;


        /// <summary>
        /// 竖向 方块 高度
        /// </summary>
        private int hHeight = 160;
        /// <summary>
        /// 竖向 方块 宽度
        /// </summary>
        private int hWidth = 35;
        /// <summary>
        /// 竖向方块 大小
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

        public ChartHelper()
        {
            recSize = new Size(mWidth, mHeight);
            recSizeW = new Size(mWidth, mHeight);
            recSize_h = new Size(hWidth, hHeight);
        }

        /// <summary>
        /// 生成各各小组图片
        /// </summary>
        /// <param name="zz"></param>
        /// <param name="fzz"></param>
        /// <param name="qtks"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
        public Bitmap CreateChart(string zz, string fzz, string qtks, string cy)
        {
            recSize_h = new Size(hWidth, hHeight);
            recSizeW = new Size(mWidth, mHeight);

            Bitmap bmp = new Bitmap(600, 400);
            string[] cys = cy.Split('、');
            

            int addh = 0;
            int maxlength = 0;
            string maxStr = "";
            for (int i = 0; i < cys.Length; i++)
            {
                if (cys[i].Length > maxlength)
                {
                    maxlength = cys[i].Length;
                    maxStr = cys[i];
                }
            }
            if (maxlength > 9)
            {
                Graphics gw = Graphics.FromImage(bmp);
                StringFormat sFormat = new StringFormat();
                sFormat.Alignment = StringAlignment.Center;
                sFormat.LineAlignment = StringAlignment.Center;
                addh = (int)gw.MeasureString(maxStr.Substring(9, maxlength - 9), font, hWidth, sFormat).Height;

                //addh = (maxlength - 9) * 25;
                recSize_h = new Size(hWidth, hHeight + addh);
            }

            int addw = 0;
            int maxlengthW = zz.Length > fzz.Length ? zz.Length : fzz.Length;
            maxlengthW = maxlengthW > qtks.Length ? maxlengthW : qtks.Length;
            string maxStrW = zz.Length > fzz.Length ? zz : fzz;
            maxStrW = maxlengthW > qtks.Length ? maxStrW : qtks;
            if (maxlengthW > 9)
            {
                Graphics gw = Graphics.FromImage(bmp);
                addw = (int)gw.MeasureString(maxStrW.Substring(9, maxlengthW - 9), font).Width;

                //addw = (maxlengthW - 9) * 18;
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

    }
}

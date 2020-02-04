using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JMProject.Word
{
    public class GroupChart
    {
        private int DefaultfontSize = 11;

        //边框颜色 笔
        private Pen DefaultPen = Pens.Black;
        //字体颜色 画刷
        Brush DefaultBrush = Brushes.Black;
        //字体
        Font DefaultFont = new Font("宋体", 11);

        private int MaxWidth = 240;

        //横向长方形大小
        private int DefaultHWidth = 130;
        private int DefaultHHeight = 25;

        //纵向长方形大小
        private int DefaultVWidth = 35;
        private int DefaultVHeight = 300;

        //默认画布大小
        private int DefaultCanvasWidth = 672;
        private int DefaultCanvasHeight = 498;

        //线段长度
        private int DefaultLineWidht = 15;
        //线段高度
        private int DefaultLineHeight = 20;

        //纵向方块之间的间距
        private int DefaultoffsetWidth = 15;

        public GroupChart()
        {

        }

        private Point GetDefaultPoint()
        {
            int x = (DefaultCanvasWidth - DefaultHWidth) / 2;
            int y = 25;
            return new Point(x, y);
        }

        private string GetMaxText(string zz, string fzz, string qtks)
        {
            string result = "";
            result = zz.Length > fzz.Length ? zz : fzz;
            result = result.Length > qtks.Length ? result : qtks;
            return result;
        }

        public StringFormat HstrFormat
        {
            get
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                return sf;
            }
        }

        public StringFormat VstrFormat
        {
            get
            {
                StringFormat sf = new StringFormat();
                sf.FormatFlags = StringFormatFlags.DirectionVertical;
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                return sf;
            }
        }

        private int getAddHeight(Graphics g, string content)
        {
            int FontWidth = (int)g.MeasureString(content, DefaultFont, new PointF(0, 0), HstrFormat).Width + 1;
            int FontHeight = 0;
            int addHeight = 0;
            if (FontWidth > MaxWidth)
            {
                FontHeight = (int)g.MeasureString(content, DefaultFont, MaxWidth, HstrFormat).Height;
            }

            if (FontHeight > DefaultHHeight)
            {
                addHeight = (FontHeight - DefaultHHeight) / 2;
            }
            return addHeight;
        }

        /// <summary>
        /// 生成各各小组图片
        /// </summary>
        /// <param name="zz">组长</param>
        /// <param name="fzz">副组长</param>
        /// <param name="qtks">牵头科室</param>
        /// <param name="cys">成员</param>
        /// <returns></returns>
        public Bitmap WorkGroupChart(string zz, string fzz, string qtks, string cy)
        {
            Bitmap bmp = new Bitmap(DefaultCanvasWidth, DefaultCanvasHeight);
            string[] cys = cy.Split('、');//获取所有成员
            int maxlength = cys.Max(s => s.Length);
            string maxStrV = cys.First(s => s.Length == maxlength);

            //画布
            Graphics g = Graphics.FromImage(bmp);
            //填充白色
            g.FillRectangle(Brushes.White, 0, 0, DefaultCanvasWidth, DefaultCanvasHeight);

            string maxStrH = GetMaxText(zz, fzz, qtks);
            int FontWidth = (int)g.MeasureString(maxStrH, DefaultFont, new PointF(0, 0), HstrFormat).Width + 1;
            int FontHeight = 0;
            int addHeight = 0;
            if (FontWidth < MaxWidth)
            {
                MaxWidth = FontWidth;
            }
            else if (FontWidth > MaxWidth)
            {
                FontHeight = (int)g.MeasureString(maxStrH, DefaultFont, MaxWidth, HstrFormat).Height;
            }

            if (FontHeight > DefaultHHeight)
            {
                addHeight = (FontHeight - DefaultHHeight) / 2;
            }

            Point defaultPoint = GetDefaultPoint();
            Rectangle Rect = new Rectangle(defaultPoint, new Size(DefaultHWidth, DefaultHHeight));
            if (maxStrH == zz)
            {
                Rectangle Rect1 = new Rectangle(defaultPoint, new Size(DefaultHWidth, DefaultHHeight));
                DrawRectLineH(g, Rect, "组长", zz, MaxWidth, addHeight);
            }
            else
            {
                DrawRectLineH(g, Rect, "组长", zz, MaxWidth, getAddHeight(g, zz));
            }

            Point StartPoint = new Point(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height);
            DrawVerticalityLine(g, StartPoint);

            Rect.Offset(0, Rect.Height + DefaultLineHeight);
            if (maxStrH == fzz)
            {
                DrawRectLineH(g, Rect, "副组长", fzz, MaxWidth, addHeight);
            }
            else
            {
                DrawRectLineH(g, Rect, "副组长", fzz, MaxWidth, getAddHeight(g, fzz));
            }

            StartPoint = new Point(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height);
            DrawVerticalityLine(g, StartPoint);

            Rect.Offset(0, Rect.Height + DefaultLineHeight);
            if (maxStrH == qtks)
            {
                DrawRectLineH(g, Rect, "成员", qtks, MaxWidth, addHeight);
            }
            else
            {
                DrawRectLineH(g, Rect, "成员", qtks, MaxWidth, getAddHeight(g, qtks));
            }

            StartPoint = new Point(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height);
            DrawVerticalityLine(g, StartPoint);

            SizeF sizeAuto = new SizeF(100, DefaultVHeight);
            int midellIndex = cys.Count() / 2;

        Check:
            int cyPointX = Rect.X + Rect.Width / 2;
            int cyPointY = Rect.Y + Rect.Height + DefaultLineHeight * 2;
            int sumLeftOffset = 0;
            int sumRightOffset = 0;
            int sumLeftLineOffset = 0;
            int sumRightLineOffset = 0;
            int cyPointLX = 0;
            int cyPointRX = 0;

            for (int i = 0; i < cys.Count(); i++)
            {
                string cystr = "";
                if (i == 0)
                {
                    cystr = cys[midellIndex];
                }
                else if (i % 2 == 1)
                {
                    cystr = cys[midellIndex - i / 2 - 1];
                }
                else
                {
                    cystr = cys[midellIndex + i / 2];
                }

                SizeF cySizef = g.MeasureString(cystr, DefaultFont, sizeAuto, VstrFormat);
                int cyFontHeight = DefaultVHeight;
                int cyFontWidth = cySizef.Width > DefaultVWidth ? (int)cySizef.Width : DefaultVWidth;

                if (i == 0)
                {
                    if (cys.Count() % 2 == 1)
                    {
                        cyPointLX = cyPointX - cyFontWidth / 2;
                        cyPointRX = cyPointX + cyFontWidth / 2;
                    }
                    else
                    {
                        cyPointLX = cyPointX + DefaultoffsetWidth / 2;
                        cyPointRX = cyPointX + DefaultoffsetWidth / 2 + cyFontWidth;

                        sumRightLineOffset = sumRightOffset - cyFontWidth / 2 - 1;
                    }
                }
                else if (i % 2 == 1)
                {
                    sumLeftOffset += cyFontWidth + DefaultoffsetWidth;
                    if (i >= cys.Count() - 2)
                    {
                        sumLeftLineOffset = sumLeftOffset - cyFontWidth / 2;
                    }
                }
                else
                {
                    sumRightOffset += DefaultoffsetWidth;
                    sumRightOffset += cyFontWidth;
                    if (i >= cys.Count() - 2)
                    {
                        sumRightLineOffset = sumRightOffset - cyFontWidth / 2 - 1;
                    }
                }
            }

            if (cyPointLX - sumLeftOffset < 1 || cyPointRX + sumRightOffset > DefaultCanvasWidth)
            {
                if (DefaultoffsetWidth > 3)
                {
                    DefaultoffsetWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 18 && DefaultfontSize == 11)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 16 && DefaultfontSize == 10)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 15 && DefaultfontSize == 9)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 13 && DefaultfontSize == 8)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 11 && DefaultfontSize == 7)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 9 && DefaultfontSize == 6)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultVWidth > 7 && DefaultfontSize == 5)
                {
                    DefaultVWidth -= 1;
                    goto Check;
                }
                else if (DefaultfontSize > 6)
                {
                    DefaultfontSize -= 1;
                    DefaultFont = new Font("宋体", DefaultfontSize);
                    goto Check;
                }
                else { }
            }


            cyPointX = Rect.X + Rect.Width / 2;
            cyPointY = Rect.Y + Rect.Height + DefaultLineHeight * 2;
            sumLeftOffset = 0;
            sumRightOffset = 0;
            sumLeftLineOffset = 0;
            sumRightLineOffset = 0;
            cyPointLX = 0;
            cyPointRX = 0;

            for (int i = 0; i < cys.Count(); i++)
            {
                string cystr = "";
                if (i == 0)
                {
                    cystr = cys[midellIndex];
                }
                else if (i % 2 == 1)
                {
                    cystr = cys[midellIndex - i / 2 - 1];
                }
                else
                {
                    cystr = cys[midellIndex + i / 2];
                }

                SizeF cySizef = g.MeasureString(cystr, DefaultFont, sizeAuto, VstrFormat);
                int cyFontHeight = DefaultVHeight;
                int cyFontWidth = cySizef.Width > DefaultVWidth ? (int)cySizef.Width : DefaultVWidth;

                if (i == 0)
                {
                    if (cys.Count() % 2 == 1)
                    {
                        cyPointLX = cyPointX - cyFontWidth / 2;
                        cyPointRX = cyPointX + cyFontWidth / 2;
                    }
                    else
                    {
                        cyPointLX = cyPointX + DefaultoffsetWidth / 2;
                        cyPointRX = cyPointX + DefaultoffsetWidth / 2 + cyFontWidth;


                        sumRightLineOffset = sumRightOffset - cyFontWidth / 2 - 1;
                    }
                    Rectangle rectV = new Rectangle(cyPointLX, cyPointY, cyFontWidth, cyFontHeight);
                    DrawRectLineV(g, rectV, cystr);
                }
                else if (i % 2 == 1)
                {
                    sumLeftOffset += cyFontWidth + DefaultoffsetWidth;
                    Rectangle rectV = new Rectangle(cyPointLX - sumLeftOffset, cyPointY, cyFontWidth, cyFontHeight);
                    DrawRectLineV(g, rectV, cystr);
                    if (i >= cys.Count() - 2)
                    {
                        sumLeftLineOffset = sumLeftOffset - cyFontWidth / 2;
                    }
                }
                else
                {
                    sumRightOffset += DefaultoffsetWidth;
                    Rectangle rectV = new Rectangle(cyPointRX + sumRightOffset, cyPointY, cyFontWidth, cyFontHeight);
                    DrawRectLineV(g, rectV, cystr);
                    sumRightOffset += cyFontWidth;
                    if (i >= cys.Count() - 2)
                    {
                        sumRightLineOffset = sumRightOffset - cyFontWidth / 2 - 1;
                    }
                }
            }

            if (cys.Count() > 1)
            {
                DrawLine(g, new Point(cyPointLX - sumLeftLineOffset, cyPointY - DefaultLineHeight), new Point(cyPointRX + sumRightLineOffset, cyPointY - DefaultLineHeight));
            }


            return bmp;
        }

        /// <summary>
        /// 画两个横向方块与中间横线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Rect">矩形区域</param>
        /// <param name="title">组长/副组长/成员</param>
        /// <param name="content">具体内容</param>
        /// <param name="differenceWidth">第二个矩形框宽度</param>
        /// <param name="addHeight">内容过多时需要增加的高度</param>
        private void DrawRectLineH(Graphics g, Rectangle Rect, string title, string content, int differenceWidth, int addHeight)
        {
            DrawRectAndStrH(g, Rect, title);
            if (!string.IsNullOrEmpty(content))
            {
                Point StartPoint = new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height / 2);
                DrawHorizontalLine(g, StartPoint);
                Rect.Offset(DefaultLineWidht + Rect.Width, 0);
                Rect.Width = differenceWidth;
                Rect.Y += -addHeight;
                Rect.Height += addHeight * 2;
                DrawRectAndStrH(g, Rect, content);
            }
        }

        /// <summary>
        /// 画两个横向方块与中间横线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Rect">矩形区域</param>
        /// <param name="title">组长/副组长/成员</param>
        /// <param name="content">具体内容</param>
        /// <param name="differenceWidth">第二个矩形框宽度</param>
        /// <param name="addHeight">内容过多时需要增加的高度</param>
        private void DrawRectLineV(Graphics g, Rectangle Rect, string content)
        {
            DrawRectAndStrV(g, Rect, content);
            Point StartPoint = new Point(Rect.X + Rect.Width / 2, Rect.Y - DefaultLineHeight);
            DrawVerticalityLine(g, StartPoint);
        }

        /// <summary>
        /// 横向矩形框内写内容
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Rect">矩形框</param>
        /// <param name="content">内容</param>
        private void DrawRectAndStrH(Graphics g, Rectangle Rect, string content)
        {
            g.DrawRectangle(DefaultPen, Rect);

            Rect.Offset(0, 2);
            g.DrawString(content, DefaultFont, DefaultBrush, Rect, HstrFormat);
        }

        /// <summary>
        /// 纵向矩形框内写内容
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Rect">矩形框</param>
        /// <param name="content">内容</param>
        private void DrawRectAndStrV(Graphics g, Rectangle Rect, string content)
        {
            g.DrawRectangle(DefaultPen, Rect);

            g.DrawString(content, DefaultFont, DefaultBrush, Rect, VstrFormat);
        }

        /// <summary>
        /// 横向线条
        /// </summary>
        /// <param name="g"></param>
        /// <param name="StartPoint">起点</param>
        private void DrawHorizontalLine(Graphics g, Point StartPoint)
        {
            Point EndPoint = new Point(StartPoint.X + DefaultLineWidht, StartPoint.Y);
            g.DrawLine(DefaultPen, StartPoint, EndPoint);
        }

        /// <summary>
        /// 纵向线条
        /// </summary>
        /// <param name="g"></param>
        /// <param name="StartPoint">起点</param>
        private void DrawVerticalityLine(Graphics g, Point StartPoint)
        {
            Point EndPoint = new Point(StartPoint.X, StartPoint.Y + DefaultLineHeight);
            g.DrawLine(DefaultPen, StartPoint, EndPoint);
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="g"></param>
        /// <param name="StartPoint">起点</param>
        /// <param name="EndPoint">终点</param>
        private void DrawLine(Graphics g, Point StartPoint, Point EndPoint)
        {
            g.DrawLine(DefaultPen, StartPoint, EndPoint);
        }
    }
}

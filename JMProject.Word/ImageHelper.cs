using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace JMProject.Word
{
    internal class ImageHelper
    {
        private static Brush brush = new SolidBrush(Color.Black);

        private void SetBrush(Brush _brush)
        {
            brush = _brush;
        }

        public static void EditImageText(string imgFile, List<string> texts, List<Font> mfonts, List<Rectangle> fontrecs, string imgFileNew)
        {
            try
            {
                Image bmp2 = Image.FromFile(imgFile);

                //新建第二个bitmap类型的bmp2变量，我这里是根据我的程序需要设置的。
                using (Bitmap bmp = new Bitmap(bmp2.Width, bmp2.Height, PixelFormat.Format16bppRgb555))
                {
                    //将第一个bmp拷贝到bmp2中
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(bmp2, 0, 0);
                    bmp2.Dispose();
                    if (texts != null && texts.Count > 0)
                    {
                        StringFormat sFormat = new StringFormat();
                        sFormat.Alignment = StringAlignment.Center;
                        sFormat.LineAlignment = StringAlignment.Center;
                        for (int i = 0; i < texts.Count; i++)
                        {
                            g.DrawString(texts[i], mfonts[i], brush, fontrecs[i], sFormat);
                        }
                        g.Dispose();
                    }

                    bmp.Save(imgFileNew, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}

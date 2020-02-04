using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using O2S.Components.PDFRender4NET;
using System.Drawing;

namespace JMProject.Web.Controllers
{
    public class FileController : AsyncController
    {
        public void IndexAsync(string swfName, int pageindex)
        {
            //increment不写参数情况默认计数为1
            //如果存在多个task需要添加相应的计数值，以保证结果能正确的返回
            AsyncManager.OutstandingOperations.Increment();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                string path = Server.MapPath("\\UserPDF\\");
                if (!System.IO.File.Exists(path + swfName + ".pdf"))
                {
                    AsyncManager.Parameters["msg"] = "ok";
                    AsyncManager.OutstandingOperations.Decrement();
                }
                else
                {
                    PDFFile doc = PDFFile.Open(path + swfName + ".pdf");

                    int min = pageindex < 2 ? 0 : pageindex;
                    int max = pageindex + 5 >= doc.PageCount ? doc.PageCount : pageindex + 5;

                    for (int i = min; i < max; i++)
                    {
                        if (!System.IO.File.Exists(path + swfName + "\\" + swfName + i + ".jpg"))//判断页数是否存在
                        {
                            Bitmap pageImage = doc.GetPageImage(i, 56 * (int)5);
                            pageImage.Save(path + swfName + "\\" + swfName + i + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            pageImage.Dispose();
                        }
                    }

                    //传递参数给XXXCompleted
                    AsyncManager.Parameters["msg"] = "ok";

                    //end
                    AsyncManager.OutstandingOperations.Decrement();//多个任务要多次调用，调用次数一般等于increment中设置的计数 
                }
            });
        }

        public ActionResult IndexCompleted(string msg)
        {
            return Content(msg.ToString());
        }

    }
}

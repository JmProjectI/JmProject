using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace JMProject.Common
{
    public class HttpHelper
    {
        public static string HttpPostWebClient(string postUrl, NameValueCollection PostVars)
        {
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            byte[] byRemoteInfo = WebClientObj.UploadValues(postUrl, "POST", PostVars);
            string output = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
            return output;
        }

        public static string HttpPost(string postUrl, string paramData)
        {
            return HttpPost(postUrl, paramData, Encoding.UTF8);
        }

        public static string HttpPost(string postUrl, string paramData, Encoding dataEncode)
        {
            try
            {
                string ret = string.Empty;

                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                System.IO.Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();

                return ret;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static string HttpGet(string Url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(resStream, Encoding.GetEncoding("utf-8"));
            string strRet = readStream.ReadToEnd();
            readStream.Close();
            resStream.Close();
            return strRet;
        }

        public static string HttpDownFile(string Url, string paramStr, string savepath)
        {
            string file = string.Empty;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url + (paramStr == "" ? "" : "?") + paramStr);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                string strpath = myResponse.ResponseUri.ToString();
                //WriteLog("接收类别://" + myResponse.ContentType);
                WebClient mywebclient = new WebClient();
                //WriteLog("路径://" + savepath);
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = savepath;
                }
                catch (Exception ex)
                {
                    file = ex.ToString();
                }

            }
            return file;
        }
    }
}

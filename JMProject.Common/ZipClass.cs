using System;
using System.Data;
using System.Web;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Checksums;

namespace JMProject.Common
{
    public class ZipClass//压缩与加压缩类
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="FileToZip">压缩文件存放路径名.rar</param>
        /// <param name="ZipedFile">需压缩文件所在路径文件夹</param>
        /// <returns></returns>
        public static Boolean ZipFile(string FileToZip, string ZipedFile)
        {
            try
            {
                FastZip fastZip = new FastZip();
                bool recurse = true;

                fastZip.CreateZip(FileToZip, ZipedFile, recurse, "");
                return true;
            }
            catch
            {
                throw new Exception("压缩失败");
            }
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="FileToZip">解压文件存放路径名.rar</param>
        /// <param name="ZipedFile">需解压文件所在路径文件夹</param>
        /// <returns></returns>
        public static Boolean UNZipFile(string FileToZip, string ZipedFile)
        {
            try
            {
                FastZip fastZip = new FastZip();
                fastZip.ExtractZip(FileToZip, ZipedFile, "");

                return true;
            }
            catch
            {
                throw new Exception("解压缩失败");
            }

        }
    }
}

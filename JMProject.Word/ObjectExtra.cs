using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JMProject.Word
{
    public static class ObjectExtra
    {
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="mes"></param>
        public static string ToStringEx(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}

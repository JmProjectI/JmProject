using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JMProject.Dal.TbColAttribute
{
    public class ValidateAttribute
    {
        /// <summary>
        /// 验证是否为自增字段
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsIdentity(PropertyInfo property)
        {
            object[] dataList = property.GetCustomAttributes(false);
            foreach (object item in dataList)
            {
                Identity v = item as Identity;
                if (v != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证是否为主键字段
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsPrimaryKey(PropertyInfo property)
        {
            object[] dataList = property.GetCustomAttributes(false);
            foreach (object item in dataList)
            {
                PrimaryKey v = item as PrimaryKey;
                if (v != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

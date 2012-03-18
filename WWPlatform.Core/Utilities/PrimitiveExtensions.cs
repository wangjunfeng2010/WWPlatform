using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WWPlatform.Core.Utilities
{
    public static class PrimitiveExtensions
    {
        //改进string.Substring方法
        /// <summary>
        /// 从字符串上截取前N个字符，改进substring方法
        /// 若N>字符串的长度,substring会抛出异常
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static public string Truncate(this string str, int length)
        {
            return length > str.Length ? str : str.Substring(0, length);
        }

        static public string ToBase64(this int src)
        {
            return Base64.Encode(src.ToString());
        }
    }
}

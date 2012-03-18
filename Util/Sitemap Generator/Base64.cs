using System;
using System.Text;

namespace WWPlatform.Core.Utilities
{
    /// <summary>
    /// 自定义的Base64编码方法，用于加密、解密url参数
    /// 实际上只能算作Base62
    /// </summary>
    public sealed class Base64
    {
        private const int Digit = 7;
        //编码集合
        private readonly static string CodecSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLNOPQRSTUVWYZ0123456789";
        private const int Offset = 6;
        private readonly static Encoding Encoding = Encoding.ASCII;
        
        /// <summary>
        /// 对原始的字符串采用base64编码
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string source)
        {
            //Contract.ArgumentNotEmpty(source, "source");

            StringBuilder mergrd = new StringBuilder();
            byte[] bytes = Encoding.GetBytes(source);

            for (int i = 0; i < bytes.Length; i++)
            {
                FormatUtil.FormatBinary(bytes[i], mergrd);
            }

            int groupCount = mergrd.Length / Offset;
            int lastCount = mergrd.Length % Offset;

            if (lastCount > 0)
            { groupCount += 1; }

            StringBuilder sb = new StringBuilder();
            int forMax = groupCount * Offset;

            for (int i = 0; i < forMax; i += Offset)
            {
                int end = i + Offset;
                bool flag = false;
                string strFiveBit;

                if (end > mergrd.Length)
                {
                    flag = true;
                    strFiveBit = mergrd.ToString().Substring(i);
                }
                else
                {
                    strFiveBit = mergrd.ToString().Substring(i, Offset);
                }

                int intFiveBit = Convert.ToInt32(strFiveBit, 2);

                if (flag)
                {
                    intFiveBit <<= (Offset - lastCount);
                }

                sb.Append(CodecSet.Substring(intFiveBit, 1));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 解码base64编码的字符串
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static String Decode(string base64)
        {
            //Contract.ArgumentNotEmpty(base64, "base64");

            char[] codes = base64.ToCharArray();
            StringBuilder sbBinarys = new StringBuilder();
            for (int i = 0; i < codes.Length; i++)
            {
                int index = CodecSet.IndexOf(codes[i]);
                String indexBinary = Convert.ToString(index, 2);
                FormatUtil.FormatBinary(indexBinary, sbBinarys, Offset);
            }

            byte[] bytes = new byte[sbBinarys.Length / Digit];

            for (int i = 0; i < bytes.Length; i++)
            {
                string sub = sbBinarys.ToString().Substring(i*Digit, Digit);
                //从二进制转到十进制
                int intBinary = Convert.ToInt32(sub, 2);
                bytes[i] = byte.Parse(intBinary.ToString());
            }

            return Encoding.GetString(bytes);
        }
        
        class FormatUtil
        {
            public static String FormatBinary(byte binary)
            {
                return FormatBinary(binary, null).ToString();
            }

            public static String FormatBinary(byte binary, int bitCount)
            {
                return FormatBinary(binary, null, bitCount).ToString();
            }

            public static StringBuilder FormatBinary(byte binary, StringBuilder toAppendTo)
            {
                return FormatBinary(binary, toAppendTo, Digit);
            }

            public static StringBuilder FormatBinary(byte binary, StringBuilder toAppendTo, int bitCount)
            {
                string strBinary = Convert.ToString(binary, 2);// Integer.toBinaryString(binary);
                return FormatBinary(strBinary, toAppendTo, bitCount);
            }

            public static String FormatBinary(String binary)
            {
                return FormatBinary(binary, null).ToString();
            }

            public static String FormatBinary(String binary, int bitCount)
            {
                return FormatBinary(binary, null, bitCount).ToString();
            }

            public static StringBuilder FormatBinary(String binary, StringBuilder toAppendTo)
            {
                return FormatBinary(binary, toAppendTo, Digit);
            }

            public static StringBuilder FormatBinary(String binary, StringBuilder toAppendTo, int bitCount)
            {
                if (binary == null || binary.Length < 1)
                {
                    return toAppendTo;
                }

                if (toAppendTo == null)
                {
                    toAppendTo = new StringBuilder();
                }

                if (binary.Length == bitCount)
                {
                    toAppendTo.Append(binary);
                    return toAppendTo;
                }

                if (binary.Length < bitCount)
                {
                    StringBuilder formatted = new StringBuilder();
                    formatted.Append(binary);

                    do
                    {
                        formatted.Insert(0, "0");
                    } 
                    while (formatted.Length < bitCount);

                    toAppendTo.Append(formatted);

                    return toAppendTo;
                }

                if (binary.Length > bitCount)
                {
                    String intercepted = binary.Substring(binary.Length - bitCount);
                    toAppendTo.Append(intercepted);
                    return toAppendTo;
                }

                return toAppendTo;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.Context;

namespace QzoneSDK.Api
{
    [Serializable]
    public abstract class BaseApi
    {
        public SecurityContext Context { get; set; }

        public BaseApi(SecurityContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Returns the Query string of dictionary Object passed
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        protected string GetQueryParamFromDictionary(Dictionary<string, string> dic)
        {
            var queryParam = new StringBuilder();

            if (dic != null && dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    queryParam.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
                queryParam = queryParam.Remove(queryParam.Length - 1,1);
            }
            return queryParam.ToString();

        }

        public byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace QzoneSDK.OAuth.Common.Parameters
{
    public abstract class BaseSecurityParameter : IErrorResponse
    {
        public static readonly string HEADER_VALUE_SEPERATOR = "=";
        public static readonly string HEADER_PARAMETER_SEPERATOR = ",";
        public static readonly string QUERYSTRING_SEPERATOR = "&";


        public DateTime UtcTicksToDateTime(long seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            var epoc = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            epoc = epoc.Add(ts);
            return epoc;
        }

        public long DateTimeToUtcTicks(DateTime date)
        {
            var ts = date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public string GetRequiredParameter(NameValueCollection list, string key)
        {
            if (string.IsNullOrEmpty(list[key]))
            {
                throw new Exception(key);
            }

            return list[key];
        }
      
        protected bool _HasError;
        protected string _ErrorMessage;

        public abstract NameValueCollection ToCollection();

        #region IErrorResponse Members

        public bool HasError
        {
            get { return this._HasError; }
        }

        public string GetError()
        {
            return this._ErrorMessage;
        }

        public void HandlerError(string errorMessage)
        {
            this._HasError = true;
            this._ErrorMessage = errorMessage;
        }

        #endregion
    }
}

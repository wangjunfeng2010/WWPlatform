using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.OAuth.Common.Exceptions
{
    [Serializable]
    public class UnSupportedHttpMethodException : Exception
    {
        public UnSupportedHttpMethodException(string message)
            : base(message)
        {
        }
    }
}

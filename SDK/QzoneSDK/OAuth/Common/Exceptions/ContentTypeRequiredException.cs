using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.OAuth.Common.Exceptions
{
    [Serializable]
    public class ContentTypeRequiredException : Exception
    {
        public ContentTypeRequiredException(string message)
            : base(message)
        {

        }
    }
}

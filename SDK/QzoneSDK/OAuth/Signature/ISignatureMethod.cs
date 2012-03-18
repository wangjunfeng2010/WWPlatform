using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Tokens;

namespace QzoneSDK.OAuth.Signature
{
    public interface ISignatureMethod
    {
        NameValueCollection RequestParameters { get; set; }

        string BuildSignature(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken);
        string ToOAuthQueryString();
        string ToOAuthHeader();
    }
}

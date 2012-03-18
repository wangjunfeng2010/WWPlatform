using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.OAuth.Tokens
{
    public interface IOAuthToken : IToken
    {
        string TokenSecret { get; set; }

        string Openid { get; set; }

        string ToQueryString();
    }
}

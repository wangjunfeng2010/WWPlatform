using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.OAuth.Tokens
{
    public interface IToken
    {
        string TokenKey { get; set; }
    }
}

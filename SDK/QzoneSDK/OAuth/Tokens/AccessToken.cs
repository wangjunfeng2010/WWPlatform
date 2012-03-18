using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Common;


namespace QzoneSDK.OAuth.Tokens
{
    public class AccessToken : ConsumerToken
    {


        public AccessToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret, string openId)
            : base(ioAuthConsumer, oauthToken, oauthTokenSecret,openId)
        {
            this.TokenType = OAuthTokenType.AccessToken;
        }


        public AccessToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken)
            : this(ioAuthConsumer, ioAuthToken, ioAuthToken.Openid)
        {
             
        }

        public AccessToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken,string openId)
            : base(ioAuthConsumer, ioAuthToken.TokenKey,ioAuthToken.TokenSecret,openId)
        {
            this.TokenType = OAuthTokenType.AccessToken;
        }
    
        public string Get(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.GET, requestPath, headers, null);
        }

        public string Head(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.HEAD, requestPath, headers, null);
        }

        public string Post(string requestPath, byte[] body, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.POST, requestPath, headers, body);
        }

        public string Put(string requestPath, byte[] body, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.PUT, requestPath, headers, body);
        }

        public string Delete(string requestPath, WebHeaderCollection headers)
        {
            return this.Request(HttpMethodType.DELETE, requestPath, headers, null);
        }
    }
}

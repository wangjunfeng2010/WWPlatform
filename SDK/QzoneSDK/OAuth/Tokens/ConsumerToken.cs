using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Common;


namespace QzoneSDK.OAuth.Tokens
{
    public class ConsumerToken : OAuthToken
    {
        public IOAuthConsumer CurrentConsumer { get; set; }
        public OAuthTokenType TokenType { get; set; }

        public ConsumerToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret)
            : this(ioAuthConsumer, oauthToken, oauthTokenSecret,null)
        {

        }

        public ConsumerToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret, string openId)
            : this(ioAuthConsumer,oauthToken, oauthTokenSecret, openId, null)
        {
             
        }

        public ConsumerToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret, string openId, bool? callbackConfirmed)
            : base(oauthToken, oauthTokenSecret,openId,callbackConfirmed)
        {
            this.CurrentConsumer = ioAuthConsumer;
            this.TokenType = OAuthTokenType.Unknown;
        }

        public string Request(HttpMethodType httpMethod, string resourcePath, WebHeaderCollection requestHeaders, byte[] requestBody)
        {
            WebResponse response = this.CurrentConsumer.Request(httpMethod, resourcePath, this, requestHeaders, requestBody);
            var streamReader = new StreamReader(response.GetResponseStream());
            var responseBody = streamReader.ReadToEnd();

            return responseBody;
        }

        public void Sign(WebRequest request, WebHeaderCollection requestHeaders)
        {
            //TODO: finish this
        }

    }
}

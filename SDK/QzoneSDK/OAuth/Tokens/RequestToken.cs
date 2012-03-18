using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Common;


namespace QzoneSDK.OAuth.Tokens
{

    public class RequestToken : ConsumerToken
    {

        public RequestToken(IOAuthConsumer ioAuthConsumer, string oauthToken, string oauthTokenSecret)
            : base(ioAuthConsumer, oauthToken, oauthTokenSecret)
        {
            this.TokenType = OAuthTokenType.RequestToken;
        }

        //public RequestToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken)
        //    : this(ioAuthConsumer, ioAuthToken,null)
        //{
            
        //}


        public RequestToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken, string openId)
            : this(ioAuthConsumer, ioAuthToken,openId, null)
        {

        }


        public RequestToken(IOAuthConsumer ioAuthConsumer, IOAuthToken ioAuthToken,string openId, bool? callbackConfirmed)
            : base(ioAuthConsumer, ioAuthToken.TokenKey, ioAuthToken.TokenSecret,openId,callbackConfirmed)
        {
            this.TokenType = OAuthTokenType.RequestToken;
        }

        public static readonly string AUTHORIZE_FORMAT = "{0}/oauth/qzoneoauth_authorize?{1}={2}&oauth_callback={3}&oauth_consumer_key={4}";

        
        public string GetAuthorizeUrl(string callBackUrl, string consumerKey)
        {
            var uri = string.Format(AUTHORIZE_FORMAT, this.CurrentConsumer.ApiServerUri, OAuthParameter.OAUTH_TOKEN,
                                           HttpUtility.UrlEncode(this.TokenKey),
                                           HttpUtility.UrlEncode(callBackUrl), consumerKey);
            return uri;
         }

        public AccessToken GetAccessToken(WebHeaderCollection requestHeaders)
        {
            string accessTokenPath = string.Format("{0}?{1}={2}&{3}={4}", this.CurrentConsumer.AccessTokenPath, OAuthParameter.OAUTH_TOKEN,
                HttpUtility.UrlEncode(this.TokenKey), OAuthParameter.OAUTH_VERICODE, this.OAuthVericode);
            IOAuthToken ioAuthToken = this.CurrentConsumer.TokenRequest(this.CurrentConsumer.HttpMethod, accessTokenPath, this, null);
            return new AccessToken(this.CurrentConsumer, ioAuthToken);
        }
    }
}

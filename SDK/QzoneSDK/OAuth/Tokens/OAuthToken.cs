using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.OAuth.Common;

namespace QzoneSDK.OAuth.Tokens
{
    public class OAuthToken: IOAuthToken
    {
        public static readonly string OAUTH_TOKEN = "oauth_token";
        public static readonly string OAUTH_TOKEN_SECRET = "oauth_token_secret";
        public static readonly string OAUTH_CALLBACK_CONFIRMED = "oauth_callback_confirmed";
        public static readonly string OAUTH_OPENID = "openid";

        public static readonly string QUERYSTRING_FORMAT = "{0}={1}&{2}={3}";

        public string TokenKey { get; set; }
        public string TokenSecret { get; set; }
        public bool CallbackConfirmed { get; set; }
        public string OAuthVericode { get; set; }
        public string Openid { get; set; }

        public OAuthToken(string oauthToken, string oauthTokenSecret )
            : this(oauthToken, oauthTokenSecret,null)
        {


        }

        public OAuthToken(string oauthToken, string oauthTokenSecret, string openId)
            : this(oauthToken, oauthTokenSecret, openId, null)
        {

        }


        public OAuthToken(string oauthToken, string oauthTokenSecret,  string openId,bool? callBackConfirmed)
        {
            this.TokenKey = oauthToken;
            this.TokenSecret = oauthTokenSecret;
            this.Openid = openId;
            if (callBackConfirmed.HasValue)
                this.CallbackConfirmed = callBackConfirmed.Value;
        }

        public string ToQueryString()
        {
            return string.Format(QUERYSTRING_FORMAT, OAUTH_TOKEN, OAuthParameter.UrlEncode(this.TokenKey), OAUTH_TOKEN_SECRET, OAuthParameter.UrlEncode(this.TokenSecret));
        }
    }
}

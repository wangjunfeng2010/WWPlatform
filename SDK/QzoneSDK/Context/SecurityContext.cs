using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using QzoneSDK.Config;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Common;
using QzoneSDK.OAuth.Signature;


namespace QzoneSDK.Context
{
    [Serializable]
    public abstract class SecurityContext
    {
        

        public int TimeZone { get; set; }
        public DateFormat DateFormat { get; set; }

        private string consumerKey;
        /// <summary>
        /// Your ConsumerKey that you registered at http://openapi.qzone.qq.com
        /// </summary>
        public string ConsumerKey
        {
            get
            {
                return consumerKey;
            }
            set
            {
                consumerKey = value;
                this.OAuthConsumer.ConsumerKey = value;
            }
        }
        private string consumerSecret;
        /// <summary>
        /// The ConsumerSecret generated for your ConsumerKey. You can find your ConsumerSecret at http://openapi.qzone.qq.com
        /// </summary>
        public string ConsumerSecret
        {
            get
            {
                return consumerSecret;
            }
            set
            {
                consumerSecret = value;
                this.OAuthConsumer.ConsumerSecret = value;
            }
        }
        private string oAuthTokenKey;
        /// <summary>
        /// The value of the Authorized OAuth Access Token granted to you from http://openapi.qzone.qq.com
        /// </summary>
        public string OAuthTokenKey
        {
            get
            {
                return oAuthTokenKey;
            }
            set
            {
                oAuthTokenKey = value;
                if (this.AccessToken != null)
                    this.AccessToken.TokenKey = value;
            }
        }
        private string oAuthTokenSecret;
        /// <summary>
        /// The secret to the Authorized OAuth Access Token granted to you from http://openapi.qzone.qq.com
        /// </summary>
        public string OAuthTokenSecret
        {
            get
            {
                return oAuthTokenSecret;
            }
            set
            {
                oAuthTokenSecret = value;
                if (this.AccessToken != null)
                    this.AccessToken.TokenSecret = value;
            }
        }

        private string oAuthVericode;
        /// <summary>
        /// The secret to the Authorized OAuth Access Token granted to you from http://openapi.qzone.qq.com
        /// </summary>
        public string OAuthVericode
        {
            get
            {
                return oAuthVericode;
            }
            set
            {
                oAuthVericode = value;
                if (this.AccessToken != null)
                    this.AccessToken.OAuthVericode = value;
            }
        }

        private string oOpenId;
        /// <summary>
        /// The openid to the Authorized OAuth Access Token granted to you from http://openapi.qzone.qq.com
        /// </summary>
        public string OpenId
        {
            get
            {
                return oOpenId;
            }
            set
            {
                oOpenId = value;
                if (this.AccessToken != null)
                    this.AccessToken.Openid = value;
            }
        }
        protected OAuthToken RequestToken { get; set; }
        protected OAuthToken AccessToken { get; set; }
 
 
        private OAuthConsumer _OAuthConsumer;
        public OAuthConsumer OAuthConsumer
        {
            get
            {
                if (_OAuthConsumer == null)
                    _OAuthConsumer = new OAuthConsumer(Constants.OPENQQ_API_SERVER, this.ConsumerKey, this.ConsumerSecret);
                return _OAuthConsumer;

            }

        }
 

        public SecurityContext(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret,string openId ="")
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.OAuthTokenKey = accessTokenKey;
            this.OAuthTokenSecret = accessTokenSecret;
            this.DateFormat = DateFormat.ISO8601;
            this.OpenId = openId;
            this.TimeZone = +8;
        }

 

        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat,
            HttpMethodType httpMethodType, string body);

        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat,
            HttpMethodType httpMethodType, byte[] body, bool rawBody);


        public abstract string MakeRequest(string uri, ResponseFormatType responseFormat,
            HttpMethodType httpMethodType, byte[] body, bool rawBody, bool isPhoto);

       

    }
}

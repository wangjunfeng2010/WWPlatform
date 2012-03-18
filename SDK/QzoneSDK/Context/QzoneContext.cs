using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Common;
using QzoneSDK.Config;
using System.IO;
using System.Net;

namespace QzoneSDK.Context
{
    [Serializable]
    public class QzoneContext : SecurityContext
    {
        public QzoneContext(string consumerKey, string consumerSecret) 
            : this(consumerKey, consumerSecret, string.Empty, string.Empty,string.Empty) 
        { }

        public QzoneContext(string consumerKey, string consumerSecret, string accessTokenKey, string accessTokenSecret,string openId)
            : base(consumerKey, consumerSecret, accessTokenKey, accessTokenSecret,openId)
        { }


        /// <summary>
        /// <para>Returns the request OAuth authentication token associated with the current user. </para>
        /// <para>Resource: /oauth/qzoneoauth_request_token</para>
        /// <para>See more details at http://connect.opensns.qq.com/wiki/ </para>
        /// </summary>
        /// <returns>Unauthorized Request Token</returns>
        public OAuthToken GetRequestToken(string callBackUrl)
        {
            this.RequestToken = this.OAuthConsumer.GetRequestToken(callBackUrl);
            return this.RequestToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="requestToken">User-Authorized Request Token</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(OAuthToken requestToken)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new Exception("Can not get OAuth Access Token without User-Authorized Request Token");
            if (requestToken == null)
            {
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
                requestToken.OAuthVericode = this.OAuthVericode;
            }
            this.RequestToken = requestToken;

            this.AccessToken = ((RequestToken)(this.RequestToken)).GetAccessToken(null);
            return this.AccessToken;
        }
        /// <summary>
        /// <para>Exchanges a User-Authorized Request Token for an Access Token.</para>
        /// </summary>
        /// <param name="oAuthTokenKey">User-Authorized Request Token Key</param>
        /// <param name="oAuthTokenSecret">User-Authorized Request Token Secret</param>
        /// <returns>Access Token</returns>
        public OAuthToken GetAccessToken(string oAuthTokenKey, string oAuthTokenSecret, string oAuthVericode)
        {
            this.OAuthTokenKey = oAuthTokenKey;
            this.OAuthTokenSecret = oAuthTokenSecret;
            this.OAuthVericode = oAuthVericode;
            return GetAccessToken(null);
        }

        /// <summary>
        /// <para>Constructs the User Authorization URL that a user can be forwarded to in order to authorize the Unauthorized Request Token.</para>
        /// </summary>
        /// <param name="requestToken">The Request Token associated with your OAuth</param>
        /// <param name="callBackUrl">The callback URL that the browser should be redirected to once the User inputs credentials at the User Authorization URL</param>
        /// <returns>User Authorization URL</returns>
        public string GetAuthorizationUrl(OAuthToken requestToken, string callBackUrl)
        {
            if (requestToken == null && string.IsNullOrEmpty(this.OAuthTokenKey))
                throw new Exception("Can not get OpenQQ Authorization URL without a Request Token");
            if (requestToken == null)
                requestToken = new RequestToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret);
            return ((RequestToken)(this.RequestToken)).GetAuthorizeUrl(callBackUrl, ConsumerKey);

        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat, HttpMethodType httpMethodType,
            string body)
        {
            byte[] bodyBytes;
            if (string.IsNullOrEmpty(body))
            {
                body = string.Empty;
            }
            bodyBytes = !string.IsNullOrEmpty(body) ? Encoding.ASCII.GetBytes(body) : null;
            return this.MakeRequest(uri, responseFormat, httpMethodType, bodyBytes, false);
        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
           HttpMethodType httpMethodType, byte[] body, bool rawBody)
        {
            var rawResponse = string.Empty;
            if (this.AccessToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new QzoneException("Can not make a request to a Protected Resource without a valid OAuth Access Token Key and Secret", QzoneExceptionType.TOKEN_REQUIRED);
            if (this.AccessToken == null)
                this.AccessToken = new AccessToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret,this.OpenId);
            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;
 
             switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, true, this.AccessToken.TokenKey, this.AccessToken.TokenSecret,this.AccessToken.Openid);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret, this.AccessToken.Openid);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret, this.AccessToken.Openid);
                    break;
                case HttpMethodType.HEAD:
                    break;
                //case HttpMethodType.PUT:
                //    this.OAuthConsumer.ResponsePut(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret, this.AccessToken.Openid);
                //    break;
                //case HttpMethodType.DELETE:
                //    this.OAuthConsumer.ResponseDelete(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret, this.AccessToken.Openid);
                //    break;
                default:
                    break;
            }

            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new QzoneException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), QzoneExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
                throw new QzoneException("Error making request.", QzoneExceptionType.REMOTE_ERROR);
        }

        public override string MakeRequest(string uri, ResponseFormatType responseFormat,
          HttpMethodType httpMethodType, byte[] body, bool rawBody, bool isPhoto)
        {
            var rawResponse = string.Empty;
            if (this.AccessToken == null && string.IsNullOrEmpty(this.OAuthTokenKey) && string.IsNullOrEmpty(this.OAuthTokenSecret))
                throw new QzoneException("Can not make a request to a Protected Resource without a valid OAuth Access Token Key and Secret", QzoneExceptionType.TOKEN_REQUIRED);
            if (this.AccessToken == null)
                this.AccessToken = new AccessToken(this.OAuthConsumer, this.OAuthTokenKey, this.OAuthTokenSecret,this.OpenId);
            this.OAuthConsumer.ResponseType = responseFormat;
            this.OAuthConsumer.Scheme = AuthorizationSchemeType.QueryString;

            switch (httpMethodType)
            {
                case HttpMethodType.POST:
                    if (rawBody)
                        this.OAuthConsumer.ResponsePost(uri, body, rawBody, this.AccessToken.TokenKey, this.AccessToken.TokenSecret,this.AccessToken.Openid, isPhoto);
                    else
                        this.OAuthConsumer.ResponsePost(uri, body, this.AccessToken.TokenKey, this.AccessToken.TokenSecret,this.AccessToken.Openid);
                    break;
                case HttpMethodType.GET:
                    this.OAuthConsumer.ResponseGet(uri, this.AccessToken.TokenKey, this.AccessToken.TokenSecret,this.AccessToken.Openid);
                    break;              
                default:
                    break;
            }

            var httpResponse = this.OAuthConsumer.GetResponse() as HttpWebResponse;
            if (httpResponse != null)
            {
                var statusCode = (int)httpResponse.StatusCode;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var responseBody = streamReader.ReadToEnd();
                if (statusCode != 200 && statusCode != 201)
                    throw new QzoneException(string.Format("Your request received a response with status code {0}. {1}", statusCode, responseBody), QzoneExceptionType.REQUEST_FAILED, responseBody);
                return responseBody;
            }
            else
            {
                throw new QzoneException("Error making request.", QzoneExceptionType.REMOTE_ERROR);
            }
        }
    }
}

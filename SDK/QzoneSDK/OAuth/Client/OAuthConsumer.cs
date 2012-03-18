using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

using QzoneSDK.OAuth.Common;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Signature;
using QzoneSDK.Config;
using System.Web;




namespace QzoneSDK.OAuth.Client
{
    public class OAuthConsumer : IOAuthConsumer
    {
        public static readonly string DEFAULT_REQUEST_TOKEN_PATH = "/oauth/qzoneoauth_request_token";
        public static readonly string DEFAULT_ACCESS_TOKEN_PATH = "/oauth/qzoneoauth_access_token";
        public static readonly string DEFAULT_AUTHORIZE_PATH = "/oauth/qzoneoauth_authorize";

        public string ApiServerUri { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public string RequestTokenPath { get; set; }
        public string AccessTokenPath { get; set; }
        public string AuthorizePath { get; set; }

        public SignatureMethodType OAuthSignatureMethod { get; set; }
        public AuthorizationSchemeType Scheme { get; set; }
        public HttpMethodType HttpMethod { get; set; }

        public ResponseFormatType ResponseType { get; set; }
        public OAuthVersionType OAuthVersion { get; set; }

        public bool UserOverride { get; set; }

        private WebResponse _Response;

        public WebResponse GetResponse()
        {
            return _Response;
        }

        private WebRequest _Request;

        public WebRequest GetRequest()
        {
            return _Request;
        }


        public OAuthConsumer(string apiServerUri, string consumerKey, string consumerSecret)
        {
            this.ApiServerUri = apiServerUri;
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;

            this.RequestTokenPath = DEFAULT_REQUEST_TOKEN_PATH;
            this.AccessTokenPath = DEFAULT_ACCESS_TOKEN_PATH;
            this.AuthorizePath = DEFAULT_AUTHORIZE_PATH;

            this.OAuthSignatureMethod = SignatureMethodType.HMAC_SHA1;
            this.Scheme = AuthorizationSchemeType.QueryString;
            this.HttpMethod = HttpMethodType.GET;
            this.OAuthVersion = OAuthVersionType.Version1;
            this.ResponseType = ResponseFormatType.JSON;
        }


        public OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath)
        {
            return this.TokenRequest(httpMethod, resourcePath, new ConsumerToken(this, string.Empty, string.Empty), new WebHeaderCollection());
        }

        public OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders)
        {
            var response = (HttpWebResponse)this.Request(httpMethod, resourcePath, consumerToken, requestHeaders, null, null);

            var streamReader = new StreamReader(response.GetResponseStream());
            var responseBody = streamReader.ReadToEnd();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return ParseTokenResponse(responseBody);
                default:
                    throw new Exception(string.Format("{0} - {1}", response.StatusCode, responseBody));
            }
        }

        public WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken)
        {
            return Request(httpMethod, resourcePath, consumerToken, null, null, null);
        }

        public WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders, byte[] requestBody)
        {
            return Request(httpMethod, resourcePath, consumerToken, requestHeaders, requestBody, null);
        }

        public WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders, byte[] requestBody, CookieCollection cookieCollection)
        {
            return Request(httpMethod, resourcePath, consumerToken, requestHeaders, requestBody, false, cookieCollection);
        }

        public WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders,
          byte[] requestBody, bool rawBody, CookieCollection cookieCollection)
        {
            return Request(httpMethod, resourcePath, consumerToken, requestHeaders, requestBody, false, cookieCollection, false);
        }

        public WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders,
           byte[] requestBody, bool rawBody, CookieCollection cookieCollection, bool isPhoto)
        {
            var signer = new OAuthSigner();
            var additionalParameters = new NameValueCollection();

            WebRequest request = null;            
               
            request = WebRequest.Create(this.ApiServerUri + resourcePath);


            if (httpMethod != HttpMethodType.GET)
            {
                if (rawBody)
                {
                    if (isPhoto)
                        request.ContentType = "image/jpg";
                    else
                        request.ContentType = "video/mpeg"; // f0r AVI video

                }
                else
                {
                    if (this.Scheme == AuthorizationSchemeType.Body)
                    {
                        request.ContentType = "application/" + OAuthConstants.X_WWW_FORM_URLENCODED;
                    }
                    else
                    {
                        request.ContentType = "application/" + OAuthConstants.X_WWW_FORM_URLENCODED; 
                    }

                    if (requestBody != null && requestBody.Length > 0)
                    {
                        additionalParameters = HttpUtility.ParseQueryString(Encoding.UTF8.GetString(requestBody));
                    }
                }

            }

            request.Method = httpMethod.ToString();
            string oAuthQuerystring = null;
            request = signer.SignRequest(request, this, consumerToken, additionalParameters,out oAuthQuerystring);
            ((HttpWebRequest)request).Accept = "*/*";
            if (httpMethod == HttpMethodType.POST && !isPhoto && !string.IsNullOrEmpty(oAuthQuerystring))
            {
                requestBody = Encoding.ASCII.GetBytes(oAuthQuerystring);
            }
            return RawRequest(request, requestHeaders, requestBody, cookieCollection);

        }

        public WebResponse RawRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken)
        {
            return this.RawRequest(httpMethod, resourcePath, consumerToken, null, null);
        }

        public WebResponse RawRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders)
        {
            return this.RawRequest(httpMethod, resourcePath, consumerToken, requestHeaders, null);
        }

        public WebResponse RawRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken, WebHeaderCollection requestHeaders, byte[] requestBody)
        {
            if (string.IsNullOrEmpty(resourcePath))
            {
                throw new ArgumentNullException("resourcePath");
            }

            var request = (HttpWebRequest)WebRequest.Create(this.ApiServerUri + resourcePath);
            request.Method = httpMethod.ToString();
            return RawRequest(request, requestHeaders, requestBody);
        }

        public WebResponse RawRequest(WebRequest request, WebHeaderCollection requestHeaders, byte[] requestBody)
        {
            return RawRequest(request, requestHeaders, requestBody, null);
        }

        public WebResponse RawRequest(WebRequest request, WebHeaderCollection requestHeaders, byte[] requestBody, CookieCollection requestCookies)
        {
            //include the headers
            if (requestHeaders != null)
            {
                request.Headers.Add(requestHeaders);
            }

            //include the cookies
            if (requestCookies != null)
            {
                var cookies = string.Empty;

                foreach (Cookie cookie in requestCookies)
                {
                    cookies += string.Format("{0}={1};", cookie.Name, cookie.Value);
                }

                if (!string.IsNullOrEmpty(cookies))
                {
                    request.Headers[HttpRequestHeader.Cookie] = cookies;
                }
            }

            //include the body
            if (request.Method.ToUpper() != "GET")
            {
                request.ContentLength = 0;
                if (requestBody != null && requestBody.Length > 0)
                {
                    request.ContentLength = requestBody.Length;
                    var requestStream = request.GetRequestStream();
                    requestStream.Write(requestBody, 0, requestBody.Length);
                    requestStream.Close();
                }
            }

            //save for debug and reuse purposes
            this._Request = request;

            //make the request
            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response == null)
                {
                    throw wex;
                }
                else
                {
                    response = wex.Response;
                }
            }

            this._Response = response;
            return response;
        }


        public RequestToken GetRequestToken(string callback)
        {
            var requestPath = string.Format("{0}?{1}={2}", this.RequestTokenPath, OAuthParameter.OAUTH_CALLBACK,
                callback);
            IOAuthToken ioAuthToken = this.TokenRequest(this.HttpMethod, requestPath);
            return new RequestToken(this, ioAuthToken, ((OAuthToken)ioAuthToken).Openid,((OAuthToken)ioAuthToken).CallbackConfirmed);
        }

        protected OAuthToken ParseTokenResponse(string tokenResponse)
        {
            var oauthToken = string.Empty;
            var oauthTokenSecret = string.Empty;
            var openid = string.Empty;
            bool? callbackConfirmed = null;

            var queryParameter = HttpUtility.ParseQueryString(tokenResponse);

            if (!string.IsNullOrEmpty(queryParameter[OAuthToken.OAUTH_TOKEN]))
            {
                oauthToken = queryParameter[OAuthToken.OAUTH_TOKEN];
            }

            if (!string.IsNullOrEmpty(queryParameter[OAuthToken.OAUTH_TOKEN_SECRET]))
            {
                oauthTokenSecret = queryParameter[OAuthToken.OAUTH_TOKEN_SECRET];
            }

            if (!string.IsNullOrEmpty(queryParameter[OAuthToken.OAUTH_CALLBACK_CONFIRMED]))
            {
                callbackConfirmed = Convert.ToBoolean(queryParameter[OAuthToken.OAUTH_CALLBACK_CONFIRMED]);
            }

            if (!string.IsNullOrEmpty(queryParameter[OAuthToken.OAUTH_OPENID]))
            {
                openid = queryParameter[OAuthToken.OAUTH_OPENID];
            }

            return new OAuthToken(oauthToken, oauthTokenSecret,openid,callbackConfirmed);
        }

        public HttpWebResponse ResponseGet(string resourcePath, string tokenKey, string tokenSecret, string openId, CookieCollection cookieCollection)
        {
            Uri uri;
            Uri.TryCreate(this.ApiServerUri + resourcePath, UriKind.Absolute, out uri);
            var query = HttpUtility.ParseQueryString(uri.Query);

            var resourceUri = uri.AbsolutePath;

            if (this.ResponseType != ResponseFormatType.Default)
            {
                query.Add(BaseSignatureMethod.FORMAT_TYPE, this.ResponseType.ToString());
            }
            else
            {
                query.Add(BaseSignatureMethod.FORMAT_TYPE, ResponseFormatType.JSON.ToString());
            }


            resourceUri += "?" + query;


            var consumerToken = GetTokenFromResource(resourcePath, tokenKey, tokenSecret,openId);
            var response = (HttpWebResponse)this.Request(HttpMethodType.GET, resourceUri, consumerToken, null, null, cookieCollection);
            return response;
        }

        public HttpWebResponse ResponseGet(string resourcePath, string tokenKey, string tokenSecret, string openId)
        {
            return ResponseGet(resourcePath, tokenKey, tokenSecret,openId, null);
        }

        public HttpWebResponse ResponseGet(string resourcePath, string tokenSecret, CookieCollection cookieCollection)
        {
            return ResponseGet(resourcePath, null, tokenSecret,null ,cookieCollection);
        }

        public HttpWebResponse ResponseGet(string resourcePath, string tokenSecret)
        {
            return ResponseGet(resourcePath, null, tokenSecret,null);
        }
         

        public HttpWebResponse ResponsePost(string resourcePath, byte[] requestBody)
        {
            return ResponsePost(resourcePath, requestBody, false, string.Empty, string.Empty,string.Empty);
        }

        public HttpWebResponse ResponsePost(string resourcePath, byte[] requestBody, bool rawBody, string tokenKey, string tokenSecret,string openId)
        {
            return ResponsePost(resourcePath, requestBody, rawBody, tokenKey, tokenSecret,openId, false);
        }
        public HttpWebResponse ResponsePost(string resourcePath, byte[] requestBody, bool rawBody, string tokenKey, string tokenSecret,string openId, bool isPhoto)
        {

            Uri uri;
            Uri.TryCreate(this.ApiServerUri + resourcePath, UriKind.Absolute, out uri);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var resourceUri = uri.AbsolutePath;

            if (this.ResponseType != ResponseFormatType.Default)
            {
                query.Add(BaseSignatureMethod.FORMAT_TYPE, this.ResponseType.ToString());
            }
            else
            {
                query.Add(BaseSignatureMethod.FORMAT_TYPE, ResponseFormatType.JSON.ToString());
            }
            resourceUri += "?" + query;
            HttpWebResponse response;
            var consumerToken = GetTokenFromResource(resourcePath, tokenKey, tokenSecret, openId);

            response = (HttpWebResponse)this.Request(HttpMethodType.POST, resourceUri, consumerToken, null, requestBody, rawBody, null, isPhoto);


            return response;
        }

        public HttpWebResponse ResponsePost(string resourcePath, byte[] requestBody, string tokenKey, string tokenSecret,string openId)
        {
            return ResponsePost(resourcePath, requestBody, false, tokenKey, tokenSecret,openId);

        }

        //public HttpWebResponse ResponsePut(string resourcePath, byte[] requestBody)
        //{
        //    return ResponsePut(resourcePath, requestBody, null, null, null);
        //}


        //public HttpWebResponse ResponsePut(string resourcePath, byte[] requestBody, string tokenKey, string tokenSecret, string openId)
        //{

        //    var consumerToken = GetTokenFromResource(resourcePath, tokenKey, tokenSecret, openId);
        //    Uri uri;
        //    Uri.TryCreate(this.ApiServerUri + resourcePath, UriKind.Absolute, out uri);
        //    var query = HttpUtility.ParseQueryString(uri.Query);
        //    var resourceUri = uri.AbsolutePath;

        //    if (this.ResponseType != ResponseFormatType.Default)
        //    {
        //        query.Add(BaseSignatureMethod.FORMAT_TYPE, this.ResponseType.ToString());
        //    }
        //    else
        //    {
        //        query.Add(BaseSignatureMethod.FORMAT_TYPE, ResponseFormatType.JSON.ToString());
        //    }

        //    resourceUri += "?" + query;
        //    HttpWebResponse response;

        //    if (this.UserOverride)
        //    {
        //        var headers = new WebHeaderCollection();

        //        headers.Add(OAuthConstants.X_HTTP_METHOD_OVERRIDE, HttpMethodType.PUT.ToString());
        //        response = (HttpWebResponse)this.Request(HttpMethodType.POST, resourceUri, consumerToken, headers, requestBody);
        //    }
        //    else
        //    {
        //        response = (HttpWebResponse)this.Request(HttpMethodType.PUT, resourceUri, consumerToken, null, requestBody);
        //    }

        //    return response;
        //}

        //public HttpWebResponse ResponseDelete(string resourcePath, string tokenKey, string tokenSecret, string openId)
        //{
        //    var consumerToken = GetTokenFromResource(resourcePath, tokenKey, tokenSecret, openId);
        //    var response = (HttpWebResponse)this.Request(HttpMethodType.DELETE, resourcePath, consumerToken, null, null);
        //    return response;
        //}
        

        private ConsumerToken GetTokenFromResource(string resourcePath, string tokenKey, string tokenSecret,string openId)
        {
            Uri uri;
            Uri.TryCreate(this.ApiServerUri + resourcePath, UriKind.Absolute, out uri);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var aTokenKey = tokenKey ?? query[OAuthParameter.OAUTH_TOKEN] ?? string.Empty;
            var aTokenSecret = tokenSecret ?? query[OAuthParameter.OAUTH_TOKEN_SECRET] ?? string.Empty;
            var aOpenId = openId ?? query[OAuthParameter.OPENID] ?? string.Empty;
            return new ConsumerToken(this, aTokenKey, aTokenSecret,aOpenId);
        }

         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using QzoneSDK.OAuth.Common;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Tokens;

namespace QzoneSDK.OAuth.Signature
{
    public abstract class BaseSignatureMethod : ISignatureMethod
    {

        public static readonly string ERROR_HTTP_METHOD = "HTTP Method not set";
        public static readonly string ERROR_CONSUMER_NULL = "consumer can't be null";
        public static readonly string ERROR_TOKEN_NULL = "token can't be null";
        public static readonly string FORMAT_PARAMETER = "{0}&{1}";
        public static readonly string FORMAT_TYPE = "format";
  

        private NameValueCollection _RequestParameters;

        public NameValueCollection RequestParameters
        {
            get
            {
                if (_RequestParameters == null)
                {
                    _RequestParameters = new NameValueCollection();
                }

                return _RequestParameters;
            }
            set
            {
                _RequestParameters = value;
            }
        }

        public abstract string BuildSignature(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken);

        public string ToOAuthQueryString()
        {
            var collection = new NameValueCollection(RequestParameters);
            return GetSortedString(collection);

        }

        public string ToOAuthHeader()
        {
            var authHeader = new StringBuilder();

            authHeader.Append(string.Format("{0} ", OAuthConstants.AUTHORIZATION_OAUTH));

            if (!string.IsNullOrEmpty(RequestParameters[OAuthParameter.OAUTH_REALM]))
            {
                authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_REALM, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_REALM])));
            }

            authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_CONSUMER_KEY, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_CONSUMER_KEY])));

            if (!string.IsNullOrEmpty(RequestParameters[OAuthParameter.OAUTH_TOKEN]))
            {
                authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_TOKEN, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_TOKEN])));
            }

            authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_SIGNATURE_METHOD, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_SIGNATURE_METHOD])));
            authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_SIGNATURE, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_SIGNATURE])));
            authHeader.Append(string.Format("{0}={1},", OAuthParameter.OAUTH_TIMESTAMP, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_TIMESTAMP])));
            authHeader.Append(string.Format("{0}={1}", OAuthParameter.OAUTH_NONCE, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_NONCE])));

            if (!string.IsNullOrEmpty(RequestParameters[OAuthParameter.OAUTH_VERSION]))
            {
                authHeader.Append(string.Format(",{0}={1}", OAuthParameter.OAUTH_VERSION, OAuthParameter.UrlEncode(RequestParameters[OAuthParameter.OAUTH_VERSION])));
            }

            return authHeader.ToString();
        }

        public string GenerateTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public string GenerateNonce()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }


        protected string GetCanonicalString(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken)
        {
            if (string.IsNullOrEmpty(webRequest.Method))
            {
                throw new ArgumentNullException(ERROR_HTTP_METHOD);
            }

            var stringToSign = new StringBuilder();
            stringToSign.Append(webRequest.Method.ToUpper().Trim());
            stringToSign.Append(OAuthParameter.QUERYSTRING_SEPERATOR);
            stringToSign.Append(OAuthParameter.UrlEncode(webRequest.RequestUri.AbsoluteUri));
            stringToSign.Append(OAuthParameter.QUERYSTRING_SEPERATOR);

            if (RequestParameters[OAuthParameter.OAUTH_REALM] != null)
                RequestParameters.Remove(OAuthParameter.OAUTH_REALM);

            if (RequestParameters[OAuthParameter.OAUTH_SIGNATURE] != null)
                RequestParameters.Remove(OAuthParameter.OAUTH_SIGNATURE);

            if (RequestParameters[OAuthParameter.OAUTH_SIGNATURE_METHOD] == null)
                RequestParameters.Add(OAuthParameter.OAUTH_SIGNATURE_METHOD, GeneralUtil.SignatureMethodTypeToString(consumer.OAuthSignatureMethod));

            if (RequestParameters[OAuthParameter.OAUTH_CONSUMER_KEY] == null)
                RequestParameters.Add(OAuthParameter.OAUTH_CONSUMER_KEY, consumer.ConsumerKey);

            if (RequestParameters[OAuthParameter.OAUTH_VERSION] == null)
                RequestParameters.Add(OAuthParameter.OAUTH_VERSION, GeneralUtil.OAuthVersionTypeToString(consumer.OAuthVersion));

            if (RequestParameters[OAuthParameter.OAUTH_TIMESTAMP] == null)
                RequestParameters.Add(OAuthParameter.OAUTH_TIMESTAMP, GenerateTimeStamp());
            if (RequestParameters[OAuthParameter.OAUTH_NONCE] == null)
                RequestParameters.Add(OAuthParameter.OAUTH_NONCE, GenerateNonce());
            if (RequestParameters[OAuthParameter.OAUTH_TOKEN] == null && !string.IsNullOrEmpty(ioAuthToken.TokenKey))
                RequestParameters.Add(OAuthParameter.OAUTH_TOKEN, ioAuthToken.TokenKey);

            stringToSign.Append(OAuthParameter.UrlEncode(GetNormalizedParameterString(RequestParameters).Trim()));

            return stringToSign.ToString();
        }



        private static string GetNormalizedParameterString(NameValueCollection list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            var parameterKeys = new NameValueCollection(list);
             parameterKeys.Remove(OAuthParameter.OAUTH_SIGNATURE);

            return GetSortedString(parameterKeys);
        }

        private static string GetSortedString(NameValueCollection list)
        {
            var sb = new StringBuilder();
            var keys = list.AllKeys;
            Array.Sort(keys);

            foreach (var key in keys)
            {
                var valuesArray = list.GetValues(key);
                if (valuesArray == null) continue;

                Array.Sort(valuesArray);

                foreach (var myvalue in valuesArray)
                {
                    sb.Append(OAuthParameter.UrlEncode(key)).Append('=');
                    sb.Append(OAuthParameter.UrlEncode(myvalue));
                    sb.Append('&');
                }
            }

            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

    }

}

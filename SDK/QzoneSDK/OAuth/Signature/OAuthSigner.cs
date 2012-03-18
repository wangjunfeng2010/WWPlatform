using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Web;

using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Common;
using QzoneSDK.OAuth.Common.Exceptions;


namespace QzoneSDK.OAuth.Signature
{
    public class OAuthSigner
    {
        public static readonly string ARG_SIGNATURE_METHOD = "consumer.OAuthSignatureMethod";
        public static readonly string ARG_SCHEME = "consumer.Scheme";
 
        public WebRequest SignRequest(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken)
        {
            return this.SignRequest(webRequest, consumer, ioAuthToken);
        }

        public WebRequest SignRequest(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken, NameValueCollection additionalParameters, out string oAuthQueryString)
        {
            oAuthQueryString = string.Empty;
            var requestUri = string.Format("{0}://{1}{2}", webRequest.RequestUri.Scheme, webRequest.RequestUri.Authority, webRequest.RequestUri.AbsolutePath);

            var signatureMethod = GetSignatureMethod(consumer.OAuthSignatureMethod);

            //add querystring
            var query = HttpUtility.ParseQueryString(webRequest.RequestUri.Query);
            signatureMethod.RequestParameters.Add(query);

            if (!string.IsNullOrEmpty(ioAuthToken.Openid))
            {
                var openidParms = new NameValueCollection();
                openidParms.Add(OAuthParameter.OPENID,ioAuthToken.Openid);
                signatureMethod.RequestParameters.Add(openidParms);
            }
            //add body content
            signatureMethod.RequestParameters.Add(additionalParameters);

            var signature = signatureMethod.BuildSignature(webRequest, consumer, ioAuthToken);

            signatureMethod.RequestParameters[OAuthParameter.OAUTH_SIGNATURE] = signature;

            switch (consumer.Scheme)
            {
                case AuthorizationSchemeType.Header:
                    var oauthHeader = signatureMethod.ToOAuthHeader();
                    var request1 = WebRequest.Create(requestUri);
                    request1.ContentType = webRequest.ContentType;
                    request1.Headers.Add(OAuthConstants.AUTHORIZATION_HEADER, oauthHeader);
                    request1.Method = webRequest.Method;
                    return request1;
                case AuthorizationSchemeType.QueryString:
                     oAuthQueryString = signatureMethod.ToOAuthQueryString();
                     var request2 = WebRequest.Create(requestUri + "?" + oAuthQueryString);
                    request2.ContentType = webRequest.ContentType;
                    request2.Method = webRequest.Method;

                    return request2;
                case AuthorizationSchemeType.Body:
                    throw new UnSupportedHttpMethodException(webRequest.Method);
                default:
                    throw new ArgumentOutOfRangeException(ARG_SCHEME);
            }
        }

        public ISignatureMethod GetSignatureMethod(SignatureMethodType signatureMethodType)
        {
            ISignatureMethod signatureMethod;

            switch (signatureMethodType)
            {
                case SignatureMethodType.PLAINTEXT:
                    signatureMethod = new SignatureMethod_PLAINTEXT();
                    break;
                case SignatureMethodType.HMAC_SHA1:
                    signatureMethod = new SignatureMethod_HMAC_SHA1();
                    break;
                case SignatureMethodType.RSA_SHA1:
                    signatureMethod = new SignatureMethod_RSA_SHA1();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(ARG_SIGNATURE_METHOD);
            }

            return signatureMethod;
        }
        
    }
}

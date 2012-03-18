using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Common;

namespace QzoneSDK.OAuth.Signature
{
    public class SignatureMethod_HMAC_SHA1 : BaseSignatureMethod
    {
        public override string BuildSignature(WebRequest webRequest, IOAuthConsumer consumer, IOAuthToken ioAuthToken)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(ERROR_CONSUMER_NULL);
            }

            if (ioAuthToken == null)
            {
                throw new ArgumentNullException(ERROR_TOKEN_NULL);
            }

            var requestUri = string.Format("{0}://{1}{2}", webRequest.RequestUri.Scheme, webRequest.RequestUri.Authority, webRequest.RequestUri.AbsolutePath);
            var request = WebRequest.Create(requestUri);
            request.Method = webRequest.Method;

            var key = string.Format(FORMAT_PARAMETER, OAuthParameter.UrlEncode(consumer.ConsumerSecret), OAuthParameter.UrlEncode(ioAuthToken.TokenSecret));
            HashAlgorithm hashAlgorithm = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var canonicalString = this.GetCanonicalString(request, consumer, ioAuthToken);
            var encoded = Encoding.UTF8.GetBytes(canonicalString);
            var result = Convert.ToBase64String(hashAlgorithm.ComputeHash(encoded));

            return result;
        }
    }
}

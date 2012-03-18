using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using QzoneSDK.OAuth.Client;
using QzoneSDK.OAuth.Tokens;
using QzoneSDK.OAuth.Common;

namespace QzoneSDK.OAuth.Signature
{
    public class SignatureMethod_PLAINTEXT : BaseSignatureMethod
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

            var result = string.Format(FORMAT_PARAMETER, OAuthParameter.UrlEncode(consumer.ConsumerSecret), OAuthParameter.UrlEncode(ioAuthToken.TokenSecret));
            return result;
        }
    }
}

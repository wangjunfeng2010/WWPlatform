using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.OAuth.Common;
using System.Net;
using QzoneSDK.OAuth.Tokens;

namespace QzoneSDK.OAuth.Client
{
    /// <summary>
    /// OAuth消费者
    /// </summary>
    public interface IOAuthConsumer
    {
        /// <summary>
        /// API的服务地址
        /// </summary>
        string ApiServerUri { get; }
        /// <summary>
        /// 应用的唯一KEY标记
        /// </summary>
        string ConsumerKey { get; }
        /// <summary>
        /// 该KEY的密钥
        /// </summary>
        string ConsumerSecret { get; }

        /// <summary>
        /// 请求临时token地址
        /// </summary>
        string RequestTokenPath { get; set; }

        /// <summary>
        /// 请求access token 地址
        /// </summary>
        string AccessTokenPath { get; set; }
        /// <summary>
        /// 请求授权地址
        /// </summary>
        string AuthorizePath { get; set; }

        /// <summary>
        /// 签名方法
        /// </summary>
        SignatureMethodType OAuthSignatureMethod { get; set; }
        /// <summary>
        /// 验证类型
        /// </summary>
        AuthorizationSchemeType Scheme { get; set; }
        /// <summary>
        /// Http方法
        /// </summary>
        HttpMethodType HttpMethod { get; set; }
        /// <summary>
        /// 响应类型
        /// </summary>
        ResponseFormatType ResponseType { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        OAuthVersionType OAuthVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool UserOverride { get; set; }

        WebResponse Request(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken,
                                  WebHeaderCollection requestHeaders, byte[] requestBody);

        OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath);

        OAuthToken TokenRequest(HttpMethodType httpMethod, string resourcePath, ConsumerToken consumerToken,
                           WebHeaderCollection requestHeaders);
    }
}

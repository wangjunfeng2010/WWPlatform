using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.Context;
using QzoneSDK.Api;

namespace QzoneSDK
{
    /// <summary>
    /// QQ connect API
    /// </summary>
    [Serializable]
    public  class Qzone
    {
        RestApi restApi;
  
        private string _oAuthTokenKey = string.Empty;
        /// <summary>
        /// oauth token key will be set only for QzoneContext
        /// </summary>
        public string OAuthTokenKey
        { 
            get { return _oAuthTokenKey; }
            set { _oAuthTokenKey = value; }
        }
        private string _oAuthTokenSecret = string.Empty;
        /// <summary>
        /// Oauth token secret will be set only for QzoneContext
        /// </summary>
        public string OAuthTokenSecret 
        { 
            get { return _oAuthTokenSecret; } 
            set { _oAuthTokenSecret = value; } 
        }

        private string _openId = string.Empty;

        public string OpenID
        {
            get{ return _openId;  }
            set { _openId = value; }
        }

        /// <summary>
        /// 构造函数，初始化访问环境
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="oAuthToken"></param>
        /// <param name="oAuthTokenSecret"></param>
        /// <param name="authorized_verifier"></param>
        /// <param name="isApi"></param>
        /// <param name="openId"></param>
        public Qzone(string consumerKey, 
            string consumerSecret,
            string oAuthToken, 
            string oAuthTokenSecret, 
            string authorized_verifier,
            bool isApi =false,string openId = "")
        {
            if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) || string.IsNullOrEmpty(oAuthToken)
                || string.IsNullOrEmpty(oAuthTokenSecret))
            {
                throw new ArgumentNullException("参数consumerKey、consumerSecret、oAuthToken、oAuthTokenSecret不能为空");
            }
            if (isApi)
            {
                if (string.IsNullOrEmpty(openId))
                {
                    throw new ArgumentNullException("访问Qzone的api要求带获取到的token（access token）、token secret（access token secret）、openid。");
                }
            }
            if (!isApi)
            {
                QzoneContext context = new QzoneContext(consumerKey, consumerSecret);
                var accessToken = context.GetAccessToken(oAuthToken, oAuthTokenSecret, authorized_verifier);
                context.OAuthTokenKey = accessToken.TokenKey;
                context.OAuthTokenSecret = accessToken.TokenSecret;
                Init(accessToken.TokenKey, accessToken.TokenSecret, accessToken.Openid, context);
            }
            else
            {
                QzoneContext context = new QzoneContext(consumerKey, consumerSecret, oAuthToken, oAuthTokenSecret, openId);
                Init(oAuthToken, oAuthTokenSecret, openId, context);            
            }
        }

       

        private void Init(string oAuthToken, string oAuthTokenSecret, string openId, QzoneContext context)
        {
            OAuthTokenKey = oAuthToken;
            OAuthTokenSecret = oAuthTokenSecret;
            OpenID = openId;
            restApi = new RestApi(context);
            
        }

        /// <summary>
        /// <para>Retrieves the basic information for OAuth token user.</para>
        /// <para>Resource: /user/get_user_info</para>
        /// <para>Information that is returned: User ID, User URI, Web URI, Image URI, Large image URI, User type (e.g., RegularUser), Hashed data </para>
        /// <para>See more details at http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3 </para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUser()
        {
            return restApi.GetCurrentUser();
        }

        /// <summary>
        /// <para>Retrieves the photo album lists  for OAuth token user.</para>
        /// <para>Resource:/photo/list_album</para>
        /// <para>See more details at http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91list_album </para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUserListAlbum()
        {
            return restApi.GetCurrentUserListAlbum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photodesc"></param>
        /// <param name="title"></param>
        /// <param name="albumid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="picture"></param>
        /// <param name="pictureData"></param>
        /// <returns></returns>
        public string UploadPic(string photodesc, string title, string albumid, int? x, int? y, string picture, byte[] pictureData)
        {
            return restApi.UploadPic(photodesc, title, albumid, x, y, picture, pictureData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="albumname"></param>
        /// <param name="albumdesc"></param>
        /// <param name="priv"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public string AddAlbum(string albumname, string albumdesc, int priv, string question, string answer)
        {
            return restApi.AddAlbum(albumname, albumdesc, priv, question, answer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedData"></param>
        /// <returns></returns>
        public string AddFeeds(string feedData)
        {
            return restApi.AddFeeds(feedData);
        }
    }
}

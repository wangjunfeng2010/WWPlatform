using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QzoneSDK.Context;
using QzoneSDK.Config;
using QzoneSDK.OAuth.Common;

namespace QzoneSDK.Api
{
    /// <summary>
    /// <para>RestApi is the class that encapsulates all the work you will need to make server-to-server calls to the QQConnect REST Resources.</para>
    /// <para>Please refer to http://connect.opensns.qq.com/wiki/ </para>
    /// </summary>
    [Serializable]
    public class RestApi: BaseApi
    {

        public RestApi(SecurityContext context)
            : base(context) { }


        /// <summary>
        /// <para>Retrieves the basic information for OAuth token user.</para>
        /// <para>Resource: /user/get_user_info</para>
        /// <para>See more details at http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91API%E6%96%87%E6%A1%A3 </para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUser()
        {
            if (!(this.Context is QzoneContext))
                throw new QzoneException("Calling GetCurrentUser() can not be made ", QzoneExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(Endpoints.USER_URL, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }


        /// <summary>
        /// <para>Retrieves the photo album lists  for OAuth token user.</para>
        /// <para>Resource:/photo/list_album</para>
        /// <para>See more details at http://wiki.opensns.qq.com/wiki/%E3%80%90QQ%E7%99%BB%E5%BD%95%E3%80%91list_album </para>
        /// </summary>
        /// <returns>A string with Json format.</returns>
        public string GetCurrentUserListAlbum()
        {
            if (!(this.Context is QzoneContext))
                throw new QzoneException("Calling GetCurrentUserListAlbum() can not be made ", QzoneExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(Endpoints.PHOTO_LISTALBUM_URL, ResponseFormatType.JSON, HttpMethodType.GET, null, false);

        }

        public string UploadPic(string photodesc, string title, string albumid, int? x, int? y, string picture, byte[] pictureData)
        {
            var queryParams = string.Format("photodesc={0}&title={1}&albumid={2}", photodesc, title,albumid);
            if (x.HasValue)
                queryParams += string.Format("&x={0}", x);
            if (y.HasValue)
                queryParams += string.Format("&y={0}", y);


            if (!(this.Context is QzoneContext))
                throw new QzoneException("Calling UploadPic() can not be made ", QzoneExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(string.Format("{0}?{1}",Endpoints.PHOTO_UPLOADPIC_URL,queryParams), ResponseFormatType.JSON, HttpMethodType.POST, pictureData, true, true);

        }

        public string AddAlbum(string albumname, string albumdesc, int priv, string question, string answer)
        {
            var requestBody = string.Format("albumname={0}&albumdesc={1}&priv={2}", albumname, albumdesc, priv, this.Context.ConsumerKey,this.Context.OAuthTokenKey);
        
            if(priv == 5)
                requestBody = string.Format("albumname={0}&albumdesc={1}&priv={2}&question={3}&answer={4}", albumname, albumdesc, priv, question, answer);
            
          
            if (!(this.Context is QzoneContext))
                throw new QzoneException("Calling AddAlbum() can not be made ", QzoneExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(Endpoints.PHOTO_ADDALBUM_URL, ResponseFormatType.JSON, HttpMethodType.POST,requestBody);

        }

        public string AddFeeds(string feedsdata)
        {
            var requestBody = string.Format("feeds_data={0}",feedsdata);

            if (!(this.Context is QzoneContext))
                throw new QzoneException("Calling AddFeeds() can not be made ", QzoneExceptionType.TOKEN_REQUIRED);
            return Context.MakeRequest(Endpoints.FEEDS_ADDFEEDS_URL, ResponseFormatType.JSON, HttpMethodType.POST, requestBody);

        }
      
    }
}

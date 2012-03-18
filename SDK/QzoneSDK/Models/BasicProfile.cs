using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QzoneSDK.Models
{
    /// <summary>
    /// <para>获取登录用户信息，目前可获取用户昵称及头像信息</para> 
    /// <para>http://openapi.qzone.qq.com/user/get_user_info</para>
    /// </summary>
    [Serializable]
    public class BasicProfile: QzoneBase
    {       
 
        /// <summary>
        /// 昵称 
        /// </summary>
        public string Nickname {get;set;}

        /// <summary>
        /// 头像URL
        /// </summary>
        public string Figureurl { get; set; } 


    }
}

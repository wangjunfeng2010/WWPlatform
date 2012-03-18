using System.Web.Configuration;
using System.Web.Mvc;

using QzoneSDK;
using QzoneSDK.Context;
using QzoneSDK.OAuth.Tokens;
using WWPlatform.Core.Model;
using WWPlatform.Model;
using WWPlatform.Model.Repositories;
using QzoneSDK.Models;
using Jayrock.Json.Conversion;
using System.Web;
using System;
using System.Web.Security;

namespace WWPlatform.Web.Controllers
{
    /// <summary>
    /// SiteUserController负责网站用户的登录、退出操作
    /// </summary>
    public class SiteUserController : BaseController
    {
        private const string NICK_COOKIE = "lw_nick";

        private readonly string appid;
        private readonly string appkey;
        private IUnitOfWork unitOfWork;
        private SiteUser siteuser;
        private ISiteUserRepository siteuserRepository;

        public SiteUserController(IUnitOfWork unitOfWork, SiteUser siteuser,ISiteUserRepository siteuserRepository)
        {
            appid = WebConfigurationManager.AppSettings["QQAppId"];
            appkey = WebConfigurationManager.AppSettings["QQAppKey"];
            this.unitOfWork = unitOfWork;
            this.siteuser = siteuser;
            this.siteuserRepository = siteuserRepository;
        }

        /// <summary>
        /// 用qq登录,跳转到腾讯提供的登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //return JavaScript("<script>alert('hello');</script>");
            QzoneContext context = new QzoneContext(appid, appkey);
            string cbUrl = "/qq/callback";
            OAuthToken token = context.GetRequestToken(cbUrl);
            TempData["TokenKey"] = token.TokenKey;
            TempData["TokenSecret"] = token.TokenSecret;
            string authUrl = context.GetAuthorizationUrl(token, cbUrl);
            return Redirect(authUrl);
        }

        /// <summary>
        /// qq登录成功之后的回调action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Callback()
        {
            bool result = false;

            string user = string.Empty;
            string vericode = Request.QueryString["oauth_vericode"];
            if (!string.IsNullOrWhiteSpace(vericode))
            {
                string tokenKey = TempData.Peek("TokenKey").ToString();
                string tokenSecret = TempData.Peek("TokenSecret").ToString();

                Qzone qzone = new Qzone(appid, appkey, tokenKey, tokenSecret, vericode);
                user = new Qzone(appid, appkey, qzone.OAuthTokenKey, qzone.OAuthTokenSecret, string.Empty, true, qzone.OpenID).GetCurrentUser();
  
                //登录成功后返回的数据格式
                //
                //{
                //"ret":0,
                //"msg":"",
                //"nickname":"王俊锋",
                //"figureurl":"http://qzapp.qlogo.cn/qzapp/211524/28EAD81EE8AE03B48A2ACD71094721EB/30",
                //"figureurl_1":"http://qzapp.qlogo.cn/qzapp/211524/28EAD81EE8AE03B48A2ACD71094721EB/50",
                //"figureurl_2":"http://qzapp.qlogo.cn/qzapp/211524/28EAD81EE8AE03B48A2ACD71094721EB/100"
                //}

                BasicProfile profile = JsonConvert.Import<BasicProfile>(user);
                //登录失败
                if (profile != null)
                {
                    SiteUser su = siteuserRepository.FindByOpenId(qzone.OpenID);
                    if (su == null)
                    {
                        siteuserRepository.Add(new SiteUser { 
                            Openid = qzone.OpenID,
                            Nickname = profile.Nickname,
                            FigureUrl = profile.Figureurl
                        });
                    }
                    unitOfWork.SaveChanges();

                    FormsAuthentication.SetAuthCookie(qzone.OpenID, true);

                    var nickcookie = new HttpCookie(NICK_COOKIE);
                    nickcookie.Value = string.Format("{0}|{1}", Server.UrlEncode(profile.Nickname), qzone.OpenID);
                    nickcookie.Expires = DateTime.Now.AddHours(12);
                    Response.AppendCookie(nickcookie);

                    result = true;
                }
            }

            ViewBag.LoginResult = result;
            return View();
        }

        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var nickcookie = new HttpCookie(NICK_COOKIE);
            nickcookie.Value = null;
            nickcookie.Expires = DateTime.Now.AddDays(-30);
            Response.AppendCookie(nickcookie);
            return new EmptyResult();
        }
    }
}
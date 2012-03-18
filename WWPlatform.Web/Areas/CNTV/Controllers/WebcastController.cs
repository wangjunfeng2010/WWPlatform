using System.Linq;
using System.Web;
using System.Web.Mvc;
using WWPlatform.Core.Model;
using WWPlatform.Core.Utilities;
using WWPlatform.Model.CNTV;
using WWPlatform.Repositories.CNTV;
using WWPlatform.Web.ViewModels.CNTV;

namespace WWPlatform.Web.Controllers.CNTV
{
    public class WebcastController : BaseController
    {
        private IWebcastRepository webcastRepository;
        private IFeatureRepository featureRepository;
        private IUnitOfWork unitOfWork;

        public WebcastController(IUnitOfWork unitOfWork, IWebcastRepository webcastRepository, IFeatureRepository featureRepository)
        {
            this.unitOfWork = unitOfWork;
            this.webcastRepository = webcastRepository;
            this.featureRepository = featureRepository;
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Play(string id)
        {
            int idkey = int.Parse(Base64.Decode(id));
            Webcast w = webcastRepository.GetById(idkey);
            if (w == null)
            {
                throw new HttpException(404, "webcast not found");
            }
            w.Visit = (short)(w.Visit + 1);
            unitOfWork.SaveChanges();

            PBViewModel model = new PBViewModel
            {
                Webcast = w,
                Kin = w.Feature.Webcasts.OrderByDescending(o => o.Aired),
                Ulike = featureRepository.GetFeatures(w.Feature)
            };
            return View(model);
        }

        /// <summary>
        /// 阅读讲稿
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Browse(string id)
        {
            int idkey = int.Parse(Base64.Decode(id));
            Webcast w = webcastRepository.GetById(idkey);
            if (w == null || w.Script == null)
            {
                throw new HttpException(404, "文字讲稿未找到！");
            }

            w.Script.Visit = w.Script.Visit + 1;
            unitOfWork.SaveChanges();

            PBViewModel model = new PBViewModel
            {
                Webcast = w,
                Kin = w.Feature.Webcasts.Where(t=>t.Idkey != w.Idkey && t.Script!=null).OrderByDescending(t=>t.Aired),
                Ulike=featureRepository.GetFeatures(w.Feature).Take(8)
            };
            return View(model);
        }
    }
}
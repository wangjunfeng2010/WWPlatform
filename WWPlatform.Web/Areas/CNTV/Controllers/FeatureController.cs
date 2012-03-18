using System.Linq;
using System.Web;
using System.Web.Mvc;
using WWPlatform.Core.Utilities;
using WWPlatform.Model.CNTV;
using WWPlatform.Repositories.CNTV;
using WWPlatform.Repositories.RefData;
using WWPlatform.Web.ViewModels.CNTV;

namespace WWPlatform.Web.Controllers.CNTV
{
    public class FeatureController : BaseController
    {
        ILectuerRepository lectuerRepository;
        ICatalogRepository catalogRepository;
        ISubtypeRepository subtypeRepository;
        IDynastyRepository dynastyRepository;
        IFeatureRepository featureRepository;

        public FeatureController(ILectuerRepository lectuerRepository, ICatalogRepository catalogRepository,
            ISubtypeRepository subtypeRepository, IDynastyRepository dynastyRepository,
            IFeatureRepository featureRepository)
        {
            this.lectuerRepository = lectuerRepository;
            this.catalogRepository = catalogRepository;
            this.subtypeRepository = subtypeRepository;
            this.dynastyRepository = dynastyRepository;
            this.featureRepository = featureRepository;
        }

        /// <summary>
        /// 视频检索,初始跳转过来时显示的页面
        /// 仅仅提取左侧检索条件的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Lib()
        {
            LibViewModel model = new LibViewModel
            {
                Lectuers = lectuerRepository.GetLectuers(),
                Catalogs = catalogRepository.GetLibCatalogs(),
                Subtypes = subtypeRepository.GetLibSubtypes(),
                Dynasties = dynastyRepository.GetLibDynasties(),
            };
            return View(model);
        }

        /// <summary>
        /// 专题(feature)页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Preview(string id)
        {
            int idkey = int.Parse(Base64.Decode(id));
            Feature f = featureRepository.GetById(idkey);
            if (f == null)
            {
                throw new HttpException(404, "feature not found");
            }
            PreviewViewModel model = new PreviewViewModel
            {
                Feature = f,
                Ulike = featureRepository.GetFeatures(f).Take(6)
            };

            return View(model);
        }
    }
}
using System.Linq;
using System.Web.Mvc;
using WWPlatform.Repositories.CNTV;
//using WWPlatform.Repositories.Fiction;
using WWPlatform.Repositories.RefData;
using WWPlatform.Web.ViewModels.CNTV;

namespace WWPlatform.Web.Controllers.CNTV
{
    public class HomeController : BaseController
    {
        IWebcastRepository webcastRepository;
        IFeatureRepository featureRepository;
        ICatalogRepository catalogRepository;
        ISubtypeRepository subtypeRepository;
        ILectuerRepository lectuerRepository;
        IDynastyRepository dynastyRepository;

        public HomeController(IWebcastRepository webcastRepository, IFeatureRepository featureRepository,
            ICatalogRepository catalogRepository, ILectuerRepository lectuerRepository,
            ISubtypeRepository subtypeRepository, IDynastyRepository dynastyRepository)
        {
            this.webcastRepository = webcastRepository;
            this.featureRepository = featureRepository;
            this.catalogRepository = catalogRepository;
            this.lectuerRepository = lectuerRepository;
            this.subtypeRepository = subtypeRepository;
            this.dynastyRepository = dynastyRepository;
        }

        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel
            {
                RandWebcasts = webcastRepository.GetRandWebcasts(),
                HotCatalogs = catalogRepository.GetHotCatalogs(),
                HotSubtypes = subtypeRepository.GetHotSubtypes(),
                HotDynasties = dynastyRepository.GetHotDynasties(),
                HotLectuers = lectuerRepository.GetHotLectuers(),
                Slides = featureRepository.GetSlides().ToList(),
                IndexCatalogs = catalogRepository.GetCatalogs()
            };
            return View(model);
        }
    }
}

using System.Linq;
using System.Web.Mvc;
using WWPlatform.Repositories.CNTV;
//using WWPlatform.Repositories.Fiction;
using WWPlatform.Repositories.RefData;
using WWPlatform.Web.ViewModels;
using WWPlatform.Repositories.ATK;

namespace WWPlatform.Web.Controllers
{
    public class HomeController : BaseController
    {
        IWebcastRepository webcastRepository;
        IFeatureRepository featureRepository;
        ICatalogRepository catalogRepository;
        ISubtypeRepository subtypeRepository;
        ILectuerRepository lectuerRepository;
        IDynastyRepository dynastyRepository;
        //IFictionRepository fictionRepository;
        IAtkRepository atkRepository;

        public HomeController(IWebcastRepository webcastRepository, IFeatureRepository featureRepository,
            ICatalogRepository catalogRepository, ILectuerRepository lectuerRepository,
            ISubtypeRepository subtypeRepository, IDynastyRepository dynastyRepository,IAtkRepository atkRepository)
            //IFictionRepository fictionRepository
        {
            this.webcastRepository = webcastRepository;
            this.featureRepository = featureRepository;
            this.catalogRepository = catalogRepository;
            this.lectuerRepository = lectuerRepository;
            this.subtypeRepository = subtypeRepository;
            this.dynastyRepository = dynastyRepository;
            //this.fictionRepository = fictionRepository;
            this.atkRepository = atkRepository;
        }

#if DEBUG
        //[OutputCache(Location=OutputCacheLocation.Any,Duration=60*60*1000)]
#else
        [OutputCache(Location=OutputCacheLocation.Any,Duration=60*60*1000)]
#endif
        public ActionResult Index()
        {
            HPViewModel model = new HPViewModel
            {
                IndexCatalogs = catalogRepository.GetCatalogs(),
                Update = webcastRepository.GetUpdated(),
                Scripts = webcastRepository.GetScripts(),
                Features = featureRepository.GetFeatures(),
                //Fiction = new ViewModels.Fiction
                //{
                //    TopBooks = fictionRepository.GetTopBooks(),
                //    NewBooks = fictionRepository.GetNewBooks(),
                //    WeeklyBooks = fictionRepository.GetWeeklyBooks()
                //},
                Essays = atkRepository.GetUpdatedEssays(),
                Histories = atkRepository.GetUpdatedHistories()
            };
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult SimpleHeader()
        {
            HotIndexViewModel model = new HotIndexViewModel
            {
                Catalogs = catalogRepository.GetHotCatalogs(),
                Lectuers = lectuerRepository.GetHotLectuers(),
                Subtypes = subtypeRepository.GetHotSubtypes()
            };
            return View(model);
        }

        //搜索
        [HttpGet]
        public ActionResult Search(string ms_key)
        {
            return View();
        }
    }
}
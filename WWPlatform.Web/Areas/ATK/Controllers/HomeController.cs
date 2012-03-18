using System.Web.Mvc;
using WWPlatform.Core.Utilities;
using WWPlatform.Model.ATK;
using WWPlatform.Repositories.ATK;
using System.Linq;
using WWPlatform.Web.ViewModels.ATK;
using WWPlatform.Web.Mvc.Attributes;
using System.Collections.Generic;

namespace WWPlatform.Web.Controllers.ATK
{
    public class HomeController : BaseController
    {
        private IAtkRepository atkRepository;

        public HomeController(IAtkRepository atkRepository)
        {
            this.atkRepository = atkRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [PageResults(PageSize = 1)]
        public ActionResult Browse(string id, int? p)
        {
            int idkey = int.Parse(Base64.Decode(id));
            Article article = atkRepository.GetById(idkey);

            //int pageindex = 1;
            //if (p.HasValue)
            //{
            //    pageindex = p.Value;
            //}

            Content cont = article.Contents.First(q => q.PageIndex == (p ?? 1));
            ContentViewModel model = new ContentViewModel
            {
                Article = article,
                Text = cont.Text,
                PageCount = article.Contents.Count,
                Ranks = atkRepository.GetRanks(),
                Recommends = atkRepository.GetRecommends()
            };
            return View(model);
        }

        [PageResults(PageSize = 20)]
        public ActionResult List(int s)
        {
            ViewData.Model = atkRepository.GetListItems();
            ViewBag.Other = new ListViewModel { };
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WWPlatform.Core.Utilities;
using WWPlatform.Model.CNTV;
using WWPlatform.Model.RefData;
using WWPlatform.Repositories.CNTV;
//using WWPlatform.Repositories.Fiction;
using WWPlatform.Repositories.RefData;

namespace WWPlatform.Web.Controllers.CGI
{
    public class HomeController : BaseController
    {
        IFeatureRepository featureRepository;
        IWebcastRepository webcastRepository;
        IHintTagRepository hintTagRepository;
        //IFictionRepository fictionRepository;

        public HomeController(IFeatureRepository featureRepository, IWebcastRepository webcastRepository, IHintTagRepository hintTagRepository)//, IFictionRepository fictionRepository)
        {
            this.featureRepository = featureRepository;
            this.webcastRepository = webcastRepository;
            this.hintTagRepository = hintTagRepository;
            //this.fictionRepository = fictionRepository;
        }

        [HttpGet]
        public ActionResult Suggest(string ms_key)
        {
            IList<HintTag> tags = hintTagRepository.GetTags(ms_key).ToList();
            object obj = new
            {
                head = new
                {
                    error = 0,
                    key = ms_key,
                    num = tags.Count()
                },
                items = from t in tags
                        select new
                        {
                            w = t.Title
                        }
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Lib(int mi_cat, int mi_sub, int mi_dyn, int mi_lec, int mi_show_type, int mi_pagesize, int mi_pagenum)
        {
            int total = 0;
            IEnumerable<Feature> features = featureRepository.GetFeatures(
                new
                {
                    mi_cat = mi_cat,
                    mi_sub = mi_sub,
                    mi_dyn = mi_dyn,
                    mi_lec = mi_lec,
                    mi_pagesize = mi_pagesize,
                    mi_pagenum = mi_pagenum
                }, out total);

            object obj = new
            {
                features = from f in features
                           select new
                           {
                               show_id = Base64.Encode(f.Idkey.ToString()),
                               desc = f.Brief.Truncate(120),
                               lec = f.Lectuer == null ? "" : f.Lectuer.Name,
                               lec_index = f.Lectuer == null ? -1 : f.Lectuer.Idkey,
                               dyn = f.Dynasty.Title,
                               dyn_index = f.Dynasty.Idkey,
                               pic_url = f.Cover,
                               cat = f.Catalog.Title,
                               cat_index = f.Catalog.Idkey,
                               subtype = from s in f.Offerings.Select(o => o.Subtype)
                                         select new
                                         {
                                             id = s.Idkey,
                                             n = s.Title
                                         },
                               title = f.Title,
                               sub_title = f.Notes
                           },
                total = total
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Search(string ms_key, int a1 = 0, int a2 = 0, int a3 = 0, short ms_search_type = 1, int mi_pagenum = 0, int nds = 0)
        {
            int mi_pagesize = 9;
            if (nds == 0)
            {
                a1 = featureRepository.GetFeatures(ms_key).Count();
                a2 = webcastRepository.GetWebcasts(ms_key).Count();
                //a3 = fictionRepository.GetBooks(ms_key).Count();
            }
            if (a1 == 0 && ms_search_type == 1)
            { ms_search_type = 2; }
            if (a2 == 0 && ms_search_type == 2)
            { ms_search_type = 3; }
            if (a3 == 0 && ms_search_type == 3)
            { ms_search_type = 1; }

            Rst rst = new Rst
            {
                Key = ms_key,
                SearchType = ms_search_type,
                A1 = a1,
                A2 = a2,
                A3 = a3,
                PageNo = mi_pagenum
            };

            switch (ms_search_type)
            {
                case 1:
                    rst.Total = a1;
                    rst.Items = from f in featureRepository.GetFeatures(ms_key).Skip(mi_pagenum * mi_pagesize).Take(mi_pagesize)
                                select new
                                {
                                    lec = f.Lectuer == null ? "" : f.Lectuer.Name,
                                    lec_index = f.Lectuer == null ? -1 : f.Lectuer.Idkey,
                                    dyn = f.Dynasty.Title,
                                    dyn_index = f.Dynasty.Idkey,
                                    cat = f.Catalog.Title,
                                    cat_index = f.Catalog.Idkey,
                                    subtype = from s in f.Offerings.Select(o => o.Subtype)
                                              select new
                                              {
                                                  id = s.Idkey,
                                                  n = s.Title
                                              },
                                    show_id = f.Base64Key,
                                    title = f.Title,
                                    pic_url = f.Cover,
                                    desc = f.Brief.Truncate(120)
                                };
                    break;
                case 2:
                    rst.Total = a2;
                    rst.Items = from w in webcastRepository.GetWebcasts(ms_key).Skip(mi_pagenum * mi_pagesize).Take(mi_pagesize)
                                select new
                                {
                                    f = w.Feature.Title,
                                    f_index = w.Feature.Base64Key,
                                    show_id = w.Base64Key,
                                    title = w.Feature.Title + w.Title,
                                    pic_url = w.Image,
                                    tags = string.IsNullOrEmpty(w.Tags) ? null : w.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                                    excerpt = w.Excerpt,
                                    aired = w.Aired.ToString("yyyy-M-d")
                                };
                    break;
                //case 3:
                //    rst.Total = a3;
                //    rst.Items = from b in fictionRepository.GetBooks(ms_key).Skip(mi_pagenum * mi_pagesize).Take(mi_pagesize)
                //                select new
                //                {
                //                    show_id = b.Base64Key,
                //                    title = b.Title,
                //                    pic_url = b.Cover2,
                //                    author = b.Author,
                //                    brief = b.Brief,
                //                    published = b.Published.ToString("yyyy-M-d")
                //                };
                //    break;
            }

            return Json(new
            {
                ms_key = rst.Key,
                s_total = rst.Total,
                s_a1 = rst.A1,
                s_a2 = rst.A2,
                s_a3 = rst.A3,
                search_type = rst.SearchType,
                s_pagenum = rst.PageNo,
                items = rst.Items
            }, JsonRequestBehavior.AllowGet);
        }

        class Rst
        {
            public string Key
            { get; set; }

            public int Total
            { get; set; }

            public int A1
            { get; set; }

            public int A2
            { get; set; }

            public int A3
            { get; set; }

            public short SearchType
            { get; set; }

            public int PageNo
            { get; set; }

            public IEnumerable<object> Items
            { get; set; }
        }
    }
}

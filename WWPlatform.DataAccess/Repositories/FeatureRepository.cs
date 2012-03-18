using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.DataAccess.Extensions;
using WWPlatform.Model.CNTV;
using WWPlatform.Repositories.CNTV;

namespace WWPlatform.DataAccess.Repositories
{
    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        public FeatureRepository(WWPlatformContext context)
            : base(context)
        {
        }

        protected override Feature GetById(int id)
        {
            return Entities.Include("Webcasts").SingleOrDefault(p => p.Idkey == id);
        }

        protected override IQueryable<Feature> Entities
        {
            get
            {
                return base.Entities.Where(f => f.Webcasts.Count() > 0);
            }
        }

        IEnumerable<Feature> IFeatureRepository.GetSlides()
        {
            return Entities.Where(p => p.AsSlide);
        }

        IEnumerable<Feature> IFeatureRepository.GetFeatures()
        {
            return Entities.OrderByDescending(f => f.Webcasts.Max(w => w.Aired)).Skip(1).Where(f=>f.Top);//.Take(6);
        }

        private int ParseValue(object obj, string property)
        {
            return (int)obj.GetType().GetProperty(property).GetValue(obj, null);
        }

        IEnumerable<Feature> IFeatureRepository.GetFeatures(object param, out int total)
        {
            int mi_lec = ParseValue(param, "mi_lec");
            int mi_cat = ParseValue(param, "mi_cat");
            int mi_sub = ParseValue(param, "mi_sub");
            int mi_dyn = ParseValue(param, "mi_dyn");
            int mi_pagenum = ParseValue(param, "mi_pagenum");
            int mi_pagesize = ParseValue(param, "mi_pagesize");

            IEnumerable<Feature> list = Entities;
            if (mi_lec != -1)
            {
                list = list.Where(p => p.Lectuer != null && p.Lectuer.Idkey == mi_lec);
            }
            if (mi_cat != -1)
            {
                list = list.Where(p => p.Catalog.Idkey == mi_cat);
            }
            if (mi_sub != -1)
            {
                list = list.Where(p => p.Offerings.Select(o => o.Subtype).Any(s => s.Idkey == mi_sub));
            }
            if (mi_dyn != -1)
            {
                list = list.Where(p => p.Dynasty.Idkey == mi_dyn);
            }

            total = list.Count();
            return list.Skip(mi_pagenum * mi_pagesize).Take(mi_pagesize);
        }

        IEnumerable<Feature> IFeatureRepository.GetFeatures(Feature self)
        {
            IList<int> array = new List<int>();
            foreach (var s in self.Offerings.Select(o => o.Subtype))
            {
                array.Add(s.Idkey);
            }
            return this.Entities.Where(f => f.Idkey != self.Idkey && f.Offerings.Select(o => o.Subtype).Any(s => array.Contains(s.Idkey)));
        }

        IEnumerable<Feature> IFeatureRepository.GetFeatures(string ms_key)
        {
            return this.Entities.Where(f => f.Title.Contains(ms_key)
                || f.Offerings.Select(o => o.Subtype).Any(s => s.Title.Contains(ms_key))
                || (f.Lectuer != null && f.Lectuer.Name.Contains(ms_key)));
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.Model.CNTV;
using WWPlatform.Model.RefData;
using WWPlatform.Repositories.RefData;

namespace WWPlatform.DataAccess.Repositories
{
    public class CatalogRepository : Repository<Catalog>, ICatalogRepository
    {
        public CatalogRepository(WWPlatformContext context)
            : base(context)
        {
        }

        IEnumerable<IndexCatalog> ICatalogRepository.GetCatalogs()
        {
            var queries = from c in this.Entities
                          where c.Features.Count > 5
                          group c by c into g
                          select new IndexCatalog
                          {
                              Catalog = g.Key,
                              Features = g.Key.Features.OrderByDescending(f => f.Webcasts.Max(w => w.Aired)).Take(5),
                              RankFeatures = g.Key.Features.OrderByDescending(f => f.Webcasts.Sum(w => w.Visit)).Take(5)
                          };

            return queries.OrderBy(ic => ic.Catalog.Order);
        }

        IEnumerable<Catalog> ICatalogRepository.GetHotCatalogs()
        {
            return this.Entities.OrderByDescending(p => p.Features.Count()).Take(3);
        }

        IEnumerable<Catalog> ICatalogRepository.GetLibCatalogs()
        {
            return this.Entities.Where(p => p.Features.Count() > 0);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using WWPlatform.Model.ATK;
using WWPlatform.Repositories.ATK;
using WWPlatform.DataAccess.EF;
using WWPlatform.DataAccess.Extensions;

namespace WWPlatform.DataAccess.Repositories
{
    /// <summary>
    /// AtkRepository负责所有Atk Entity的操作
    /// </summary>
    public class AtkRepository : Repository<Article>, IAtkRepository
    {
        public AtkRepository(WWPlatformContext context)
            : base(context)
        {
        }

        IEnumerable<Article> IAtkRepository.GetUpdatedEssays()
        {
            return this.Entities.OfType<Essay>();
        }

        IEnumerable<IndexHistory> IAtkRepository.GetUpdatedHistories()
        {
            var queries = from d in this.Entities.OfType<History>()
                          group d by d.AtkClass into g
                          select new IndexHistory
                          {
                              AtkClass = g.Key,
                              Histories = g.Key.Histories.OrderByDescending(d => d.Issued).Take(4)
                          };

            return queries.Take(3);
        }

        protected override Article GetById(int idkey)
        {
            return this.Entities.Include("content").Single(article => article.Idkey == idkey);
        }

        IEnumerable<Article> IAtkRepository.GetRanks()
        {
            return this.Entities.OrderByDescending(q => q.Visit).Take(8);
        }

        IEnumerable<Article> IAtkRepository.GetRecommends()
        {
            return this.Entities.Take(8);
        }

        IEnumerable<Article> IAtkRepository.GetListItems()
        {
            return this.Entities.OrderByDescending(q=>q.Issued);
        }
    }
}
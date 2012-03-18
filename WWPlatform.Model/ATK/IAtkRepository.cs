using WWPlatform.Core.Model;
using WWPlatform.Model.ATK;
using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.ATK
{
    public interface IAtkRepository : IRepository<Article>
    {
        IEnumerable<Article> GetUpdatedEssays();

        IEnumerable<IndexHistory> GetUpdatedHistories();

        IEnumerable<Article> GetRanks();

        IEnumerable<Article> GetRecommends();

        IEnumerable<Article> GetListItems();
    }
}

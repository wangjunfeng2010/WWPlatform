using System.Collections.Generic;
using System.Linq;
using WWPlatform.DataAccess.EF;
using WWPlatform.Model.RefData;
using WWPlatform.Repositories.RefData;

namespace WWPlatform.DataAccess.Repositories
{
    public class HintTagRepository : Repository<HintTag>,IHintTagRepository
    {
        public HintTagRepository(WWPlatformContext context)
            : base(context)
        {
        }

        IEnumerable<HintTag> IHintTagRepository.GetTags(string key)
        {
            return this.Entities.Where(t=>t.Title.StartsWith(key)).Take(10);
        }
    }
}

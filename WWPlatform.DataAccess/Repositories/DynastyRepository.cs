using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.Model.RefData;
using WWPlatform.Repositories.RefData;

namespace WWPlatform.DataAccess.Repositories
{
    public class DynastyRepository : Repository<Dynasty>,IDynastyRepository
    {
        public DynastyRepository(WWPlatformContext context)
            : base(context)
        { }

        IEnumerable<Dynasty> IDynastyRepository.GetHotDynasties()
        {
            return this.Entities.Where(d => d.Hot);
        }

        IEnumerable<Dynasty> IDynastyRepository.GetLibDynasties()
        {
            return this.Entities;
        }
    }
}

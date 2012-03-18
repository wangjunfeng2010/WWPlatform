using System.Collections.Generic;
using System.Linq;
using WWPlatform.DataAccess.EF;
using WWPlatform.Model.RefData;
using WWPlatform.Repositories.RefData;

namespace WWPlatform.DataAccess.Repositories
{
    public class SubtypeRepository : Repository<Subtype>, ISubtypeRepository
    {
        public SubtypeRepository(WWPlatformContext context)
            : base(context)
        { }

        IEnumerable<Subtype> ISubtypeRepository.GetLibSubtypes()
        {
            return this.Entities;
        }

        IEnumerable<Subtype> ISubtypeRepository.GetHotSubtypes()
        {
            return this.Entities.Where(s => s.Hot);
        }
    }
}

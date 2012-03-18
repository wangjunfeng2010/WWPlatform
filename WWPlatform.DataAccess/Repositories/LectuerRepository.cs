using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.Model;
using WWPlatform.Model.Repositories;
using WWPlatform.Repositories.RefData;
using WWPlatform.Model.RefData;

namespace WWPlatform.DataAccess.Repositories
{
    public class LectuerRepository : Repository<Lectuer>, ILectuerRepository
    {
        public LectuerRepository(WWPlatformContext context)
            : base(context)
        {
        }

        IEnumerable<Lectuer> ILectuerRepository.GetLectuers()
        {
            return Entities;
        }

        IEnumerable<Lectuer> ILectuerRepository.GetHotLectuers()
        {
            return Entities.Where(l => l.Hot);
        }
    }
}
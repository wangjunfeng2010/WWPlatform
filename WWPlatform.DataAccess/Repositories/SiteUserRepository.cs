using WWPlatform.Model.Repositories;
using WWPlatform.Model;
using WWPlatform.DataAccess.EF;
using System.Linq;

namespace WWPlatform.DataAccess.Repositories
{
    public class SiteUserRepository : Repository<SiteUser>, ISiteUserRepository
    {
        public SiteUserRepository(WWPlatformContext context)
            : base(context)
        {
        }

        SiteUser ISiteUserRepository.FindByOpenId(string openid)
        {
            return this.Entities.SingleOrDefault(s => s.Openid == openid);
        }
    }
}
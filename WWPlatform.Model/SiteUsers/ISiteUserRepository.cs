using WWPlatform.Core.Model;

namespace WWPlatform.Model.Repositories
{
    public interface ISiteUserRepository : IRepository<SiteUser>
    {
        SiteUser FindByOpenId(string openid);
    }
}

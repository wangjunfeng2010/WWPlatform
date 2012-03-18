using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.DataAccess.Extensions;
using WWPlatform.Model;
using WWPlatform.Model.Repositories;
using WWPlatform.Model.CNTV;
using WWPlatform.Repositories.CNTV;

namespace WWPlatform.DataAccess.Repositories
{
    public class WebcastRepository : Repository<Webcast>, IWebcastRepository
    {
        public WebcastRepository(WWPlatformContext context)
            : base(context)
        {
        }

        protected override Webcast GetById(int id)
        {
            return Entities.SingleOrDefault(e => e.Idkey == id);
        }

        protected override IQueryable<Webcast> Entities
        {
            get
            {
                return base.Entities.Include("Feature");
            }
        }

        IEnumerable<Webcast> IWebcastRepository.GetRandWebcasts()
        {
            return base.Entities.OrderByDescending(p => p.Visit).Take(4);
        }

        IEnumerable<Webcast> IWebcastRepository.GetWebcasts(string ms_key)
        {
            return Entities.Where(w => w.Title.Contains(ms_key) || w.Tags.Contains(ms_key)).OrderByDescending(w => w.Aired);
        }

        IEnumerable<Script> IWebcastRepository.GetScripts()
        {
            return this.Entities.Include("Script").Where(w=>w.Script !=null).OrderByDescending(w => w.Script.Compiled).Take(5).Select(w=>w.Script);
        }

        Webcast IWebcastRepository.GetUpdated()
        {
            return this.Entities.OrderByDescending(w => w.Aired).First();
        }
    }
}
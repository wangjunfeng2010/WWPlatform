using System.Collections.Generic;
using WWPlatform.Model.CNTV;
using WWPlatform.Core.Model;

namespace WWPlatform.Repositories.CNTV
{
    /// <summary>
    /// 接口:IWebcastRepository负责Webcast的CRUD
    /// </summary>
    public interface IWebcastRepository : IRepository<Webcast>
    {
        IEnumerable<Webcast> GetRandWebcasts();

        IEnumerable<Webcast> GetWebcasts(string ms_key);

        IEnumerable<Script> GetScripts();

        Webcast GetUpdated();
    }
}
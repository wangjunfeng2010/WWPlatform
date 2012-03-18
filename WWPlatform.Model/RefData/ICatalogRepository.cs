using System.Collections.Generic;
using WWPlatform.Model.CNTV;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.RefData
{
    public interface ICatalogRepository
    {
        /// <summary>
        /// 热点分类
        /// </summary>
        /// <returns></returns>
        IEnumerable<Catalog> GetHotCatalogs();

        /// <summary>
        /// 在首页显示的catalog,每个Catalog需要附加如下的两部分信息：
        /// 1、在首页上显示的Feature
        /// 2、排行的Feature
        /// </summary>
        /// <returns></returns>
        IEnumerable<IndexCatalog> GetCatalogs();

        /// <summary>
        /// 用户检索导航的catalog,即数据库中所有的catalog
        /// </summary>
        /// <returns></returns>
        IEnumerable<Catalog> GetLibCatalogs();
    }
}
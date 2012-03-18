using System.Collections.Generic;
using WWPlatform.Core.Model;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Repositories.CNTV
{
    /// <summary>
    /// 接口:IFeatureRepository负责Feature的CRUD
    /// </summary>
    public interface IFeatureRepository : IRepository<Feature>
    {
        IEnumerable<Feature> GetSlides();

        /// <summary>
        /// 从数据库挑取满足条件的指定页的数据,并查询出满足条件的记录总数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<Feature> GetFeatures(object param, out int total);

        /// <summary>
        /// 和当前feature分类上有交集的feature
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        IEnumerable<Feature> GetFeatures(Feature f);

        IEnumerable<Feature> GetFeatures(string ms_key);

        IEnumerable<Feature> GetFeatures();
    }
}
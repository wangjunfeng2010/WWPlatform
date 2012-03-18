using System.Collections.Generic;
using WWPlatform.Model.ATK;
using WWPlatform.Model.CNTV;
//using WWPlatform.Model.Fiction;
using WWPlatform.Model.RefData;

namespace WWPlatform.Web.ViewModels
{
    /// <summary>
    /// Home Page View Model
    /// </summary>
    public class HPViewModel
    {
        public IEnumerable<IndexCatalog> IndexCatalogs
        { get; set; }

        public Webcast Update
        { get; set; }

        public IEnumerable<Script> Scripts
        { get; set; }

        public IEnumerable<Feature> Features
        { get; set; }

        //public Fiction Fiction
        //{ get; set; }

        public IEnumerable<Article> Essays
        { get; set; }

        public IEnumerable<IndexHistory> Histories
        { get; set; }
    }

    //public class Fiction
    //{
    //    /// <summary>
    //    /// 网友推荐
    //    /// </summary>
    //    public IEnumerable<Book> TopBooks
    //    { get; set; }

    //    /// <summary>
    //    /// 新书
    //    /// </summary>
    //    public IEnumerable<Book> NewBooks
    //    { get; set; }

    //    /// <summary>
    //    /// 周推荐
    //    /// </summary>
    //    public IEnumerable<Book> WeeklyBooks
    //    { get; set; }
    //}
}
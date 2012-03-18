using System.Collections.Generic;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Web.ViewModels.CNTV
{
    /// <summary>
    /// Play+Browse View Model
    /// </summary>
    public class PBViewModel
    {
        public Webcast Webcast
        { get; set; }

        public IEnumerable<Webcast> Kin
        { get; set; }

        public IEnumerable<Feature> Ulike
        { get; set; }
    }
}
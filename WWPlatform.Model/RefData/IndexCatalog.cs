using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Model.CNTV
{
    public class IndexCatalog
    {
        public Catalog Catalog
        { get; set; }

        public IEnumerable<Feature> Features
        { get; set; }

        public IEnumerable<Feature> RankFeatures
        { get; set; }
    }
}
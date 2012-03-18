using System.Collections.Generic;
using WWPlatform.Model.CNTV;
using WWPlatform.Model.RefData;

namespace WWPlatform.Web.ViewModels.CNTV
{
    public class IndexViewModel
    {
        public IEnumerable<Webcast> RandWebcasts
        { get; set; }

        public IEnumerable<Feature> Slides
        { get; set; }

        public IEnumerable<Catalog> HotCatalogs
        { get; set; }

        public IEnumerable<Lectuer> HotLectuers
        { get; set; }

        public IEnumerable<Subtype> HotSubtypes
        { get; set; }

        public IEnumerable<Dynasty> HotDynasties
        { get; set; }

        public IEnumerable<IndexCatalog> IndexCatalogs
        { get; set; }
    }
}
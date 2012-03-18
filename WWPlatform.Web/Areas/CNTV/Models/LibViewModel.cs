using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Web.ViewModels.CNTV
{
    public class LibViewModel
    {
        public IEnumerable<Lectuer> Lectuers
        { get; set; }

        public IEnumerable<Catalog> Catalogs
        { get; set; }

        public IEnumerable<Subtype> Subtypes
        { get; set; }

        public IEnumerable<Dynasty> Dynasties
        { get; set; }
    }
}
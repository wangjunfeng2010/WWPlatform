using System.Collections.Generic;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Web.ViewModels.CNTV
{
    public class PreviewViewModel
    {
        public  Feature Feature
        { get; set; }

        public IEnumerable<Feature> Ulike
        { get; set; }
    }
}
using System.Collections.Generic;
using WWPlatform.Model.ATK;

namespace WWPlatform.Web.ViewModels.ATK
{
    public abstract class AtkViewModel
    {
        public IEnumerable<Article> Ranks { get; set; }

        public IEnumerable<Article> Recommends { get; set; }
    }
}
using System.Collections.Generic;
using WWPlatform.Core.Paging;
using WWPlatform.Model.ATK;

namespace WWPlatform.Web.ViewModels.ATK
{
    public class ListViewModel : AtkViewModel//,IPageable
    {
        public IEnumerable<Article> HotItems
        { get; set; }
    }
}
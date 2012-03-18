using WWPlatform.Core.Model;
using WWPlatform.Model.RefData;

namespace WWPlatform.Model.CNTV
{
    public class Offering : PersistObject
    {
        public virtual Feature Feature { get; set; }
        public virtual Subtype Subtype { get; set; }
    }
}
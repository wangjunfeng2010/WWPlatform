using WWPlatform.Model.RefData;
using System.Collections.Generic;

namespace WWPlatform.Model.ATK
{
    public class IndexHistory
    {
        public AtkClass AtkClass { get; set; }

        public IEnumerable<History> Histories { get; set; }
    }
}

using System.Collections.Generic;
using WWPlatform.Core.Model;
using WWPlatform.Model.ATK;

namespace WWPlatform.Model.RefData
{
    public class AtkClass : PersistObject
    {
        public AtkClass()
        {
            this.Histories = new HashSet<History>();
        }

        public string Title { get; set; }
        public bool Hot { get; set; }

        public virtual ICollection<History> Histories { get; set; }
    }
}

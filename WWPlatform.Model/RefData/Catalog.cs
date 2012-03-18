using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WWPlatform.Core.Model;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Model.RefData
{
    //[Table("catalog",Schema="ref")]
    public class Catalog : PersistObject
    {
        public Catalog()
        {
            this.Features = new HashSet<Feature>();
        }

        //public int Idkey { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }

        public virtual ICollection<Feature> Features { get; set; }
    }
}
using System.Collections.Generic;
using WWPlatform.Core.Model;
using WWPlatform.Model.CNTV;
using System.ComponentModel.DataAnnotations;

namespace WWPlatform.Model.RefData
{
    [Table("subtype",Schema="ref")]
    public class Subtype : PersistObject
    {
        public Subtype()
        {
            this.Offerings = new HashSet<Offering>();
        }

        //public int Idkey { get; set; }
        public string Title { get; set; }
        public bool Hot { get; set; }

        public virtual ICollection<Offering> Offerings { get; set; }
    }
}
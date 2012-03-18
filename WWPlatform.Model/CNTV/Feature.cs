using System.Collections.Generic;

using WWPlatform.Core.Model;
using WWPlatform.Model.RefData;

namespace WWPlatform.Model.CNTV
{
    /// <summary>
    /// 专题
    /// </summary>
    public class Feature : PersistObject
    {
        public Feature()
        {
            this.Webcasts = new HashSet<Webcast>();
            this.Offerings = new HashSet<Offering>();
        }

        //public int Idkey { get; set; }
        public string Title { get; set; }
        public string Brief { get; set; }
        public string Cover { get; set; }
        public bool AsSlide { get; set; }
        public string HDImage { get; set; }
        public string Notes { get; set; }

        public bool Hot { get; set; }
        public bool Top { get; set; }
    
        public virtual ICollection<Webcast> Webcasts { get; set; }
        public virtual Catalog Catalog { get; set; }
        public virtual Lectuer Lectuer { get; set; }
        public virtual Dynasty Dynasty { get; set; }
        public virtual ICollection<Offering> Offerings { get; set; }
    }
}
using System;
using WWPlatform.Core.Model;

namespace WWPlatform.Model.CNTV
{
    public class Webcast : PersistObject
    {
        public int Visit { get; set; }
        public DateTime Aired { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Tags { get; set; }
        public string Image { get; set; }
        public string Coreid { get; set; }
        public string Videoid { get; set; }
        public string RefUrl { get; set; }
        public decimal? Score { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual Script Script { get; set; }
    }
}

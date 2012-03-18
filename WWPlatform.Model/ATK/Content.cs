using System;
using System.Collections.Generic;

namespace WWPlatform.Model.ATK
{
    public partial class Content
    {
        public int Idkey { get; set; }
        public string Text { get; set; }
        public short PageIndex { get; set; }

        public virtual Article Article { get; set; }
    }
}
using System;
using WWPlatform.Core.Model;
using WWPlatform.Model.RefData;
using System.Collections.Generic;

namespace WWPlatform.Model.ATK
{
    /// <summary>
    /// TPTµÄ»ùÀà
    /// </summary>
    public class Article : PersistObject
    {
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public DateTime Issued { get; set; }
        public int Visit { get; set; }
        
        public virtual ICollection<Content> Contents { get; set; }
    }

    #region TPH
    public class Digest : Article
    {
    }

    public class Essay : Article
    {
    }

    public partial class Discovery : Article
    {
    }
    #endregion

    #region TPT
    public class History : Article
    {
        public virtual AtkClass AtkClass { get; set; }
    }
    #endregion
}

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的�?
//
//    手动更改此文件可能会导致应用程序中发生异常行为�?
//    如果重新生成代码，则将覆盖对此文件的手动更改�?
// </auto-generated>
//------------------------------------------------------------------------------

namespace WWPlatform.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        public Book()
        {
            this.Chapters = new HashSet<Chapter>();
            this.Sections = new HashSet<Section>();
        }
    
        public int Idkey { get; set; }
        public string Title { get; set; }
        public string Cover1 { get; set; }
        public string Cover2 { get; set; }
        public string Cover3 { get; set; }
        public string Brief { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public System.DateTime Published { get; set; }
        public bool Top { get; set; }
    
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}

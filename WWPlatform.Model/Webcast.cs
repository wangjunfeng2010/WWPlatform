//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WWPlatform.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Webcast
    {
        public int Idkey { get; set; }
        public int Visit { get; set; }
        public System.DateTime Aired { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Tags { get; set; }
        public string Image { get; set; }
        public string Coreid { get; set; }
        public string Videoid { get; set; }
        public string RefUrl { get; set; }
        public Nullable<decimal> Score { get; set; }
    
        public virtual Feature Feature { get; set; }
        public virtual Script Script { get; set; }
    }
}

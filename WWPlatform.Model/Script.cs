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
    
    public partial class Script
    {
        public int Idkey { get; set; }
        public string Text { get; set; }
        public System.DateTime Compiled { get; set; }
        public int Visit { get; set; }
    
        public virtual Webcast Webcast { get; set; }
    }
}

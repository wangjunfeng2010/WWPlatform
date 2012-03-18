using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WWPlatform.Core.Model;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Model.RefData
{
    [Table("lectuer",Schema="ref")]
    public class Lectuer : PersistObject
    {
        //[Key]
        //public override int Idkey
        //{ get; protected set; }

        public string Name
        { get; private set; }

        public string Brief
        { get; private set; }

        public string Photo
        { get; private set; }

        public bool Hot
        { get; private set; }

        public ICollection<Feature> Features
        { get; private set; }
    }
}

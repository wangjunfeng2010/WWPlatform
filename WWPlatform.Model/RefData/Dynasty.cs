using WWPlatform.Core.Model;
using System.ComponentModel.DataAnnotations;

namespace WWPlatform.Model.RefData
{
    //[Table("dynasty",Schema="ref")]
    public class Dynasty : PersistObject
    {

        public string Title { get; set; }
        public bool Hot { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using WWPlatform.Core.Model;

namespace WWPlatform.Model
{
    [Table("siteuser",Schema="dbo")]
    public class SiteUser : PersistObject
    {
        public string Nickname { get; set; }
        public string FigureUrl { get; set; }
        public string Openid { get; set; }
    }
}
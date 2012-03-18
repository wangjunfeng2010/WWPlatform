using System.ComponentModel.DataAnnotations;
using WWPlatform.Core.Model;

namespace WWPlatform.Model.RefData
{
    /// <summary>
    /// 搜索联想关键字（即标签）
    /// </summary>
    public class HintTag : PersistObject
    {
        public string Title { get; set; }
        public string Pinyin { get; set; }
    }
}
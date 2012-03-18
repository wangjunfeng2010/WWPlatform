using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.RefData
{
    public interface IHintTagRepository
    {
        IEnumerable<HintTag> GetTags(string key);
    }
}
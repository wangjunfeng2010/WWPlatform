using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.RefData
{
    public interface IDynastyRepository
    {
        IEnumerable<Dynasty> GetLibDynasties();

        IEnumerable<Dynasty> GetHotDynasties();
    }
}

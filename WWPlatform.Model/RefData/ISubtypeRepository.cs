using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.RefData
{
    public interface ISubtypeRepository
    {
        IEnumerable<Subtype> GetLibSubtypes();

        IEnumerable<Subtype> GetHotSubtypes();
    }
}
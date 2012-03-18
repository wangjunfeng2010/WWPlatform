using System.Collections.Generic;
using WWPlatform.Model.RefData;

namespace WWPlatform.Repositories.RefData
{
    public interface ILectuerRepository
    {
        IEnumerable<Lectuer> GetLectuers();

        IEnumerable<Lectuer> GetHotLectuers();
    }
}
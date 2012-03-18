using WWPlatform.Core.Model;

namespace WWPlatform.Model.RefData
{
    /// <summary>
    /// TPC的base class
    /// </summary>
    public class RefData : PersistObject
    {
        public bool Hot
        { get; private set; }
    }
}

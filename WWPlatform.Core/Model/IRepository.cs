namespace WWPlatform.Core.Model
{
    /// <summary>
    /// IRepository提取Repository模式中公用的方法
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IRepository<TObject>// where TObject:PersistObject
    {
        TObject GetById(int idkey);

        void Add(TObject entity);

        void Remove(TObject entity);
    }
}

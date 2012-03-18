using System.Data.Entity;
using System.Linq;

using WWPlatform.Core.Model;
using WWPlatform.Core.Utilities;

namespace WWPlatform.DataAccess.Repositories
{
    /// <summary>
    /// Repository是IRepository接口的默认实现
    /// </summary>
    /// <typeparam name="T">T为EF4.1 O/RM的POCO类型</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : PersistObject
    {
        private readonly IDbSet<T> entitySet;

        protected Repository(DbContext context)
        {
            Contract.ArgumentNotNull(context, "DbContext");

            entitySet = context.Set<T>();
        }

        protected virtual IQueryable<T> Entities
        {
            get { return entitySet; }
        }

        #region IRepository<T>
        T IRepository<T>.GetById(int id)
        {
            return this.GetById(id);
        }

        void IRepository<T>.Add(T entity)
        {
            this.Add(entity);
        }

        void IRepository<T>.Remove(T entity)
        {
            this.Remove(entity);
        }
        #endregion

        #region 虚拟方法,提供对IRepository<TObject>接口的默认实现
        protected virtual T GetById(int id)
        {
            Contract.ArgumentNotNull(id, "id");

            return Entities.SingleOrDefault(e => e.Idkey.Equals(id));
        }

        protected virtual void Add(T entity)
        {
            Contract.ArgumentNotNull(entity, "entity");
            entitySet.Add(entity);
        }

        protected virtual void Remove(T entity)
        {
        }
        #endregion
    }
}
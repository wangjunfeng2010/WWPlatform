using System;
using System.Collections.Generic;
using System.Linq;

using WWPlatform.DataAccess.EF;
using WWPlatform.Model.Posts;
using WWPlatform.DataAccess.Extensions;

namespace WWPlatform.DataAccess.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(WWPlatformContext context) : base(context.Posts)
        { }

        public override Post GetById(int id)
        {
            return Entities.SingleOrDefault(e => e.Idkey == id);
        }

        protected override IQueryable<Post> Entities
        {
            get
            {
                return base.Entities.Include("Category");
            }
        }
        int IPostRepository.Count()
        {
            return this.Count();
        }

        IEnumerable<Post> IPostRepository.GetPosts()
        {
            return Entities.OrderByDescending(p => p.CreatedOn) ;
        }

        IEnumerable<Post> IPostRepository.GetByTag(string tag)
        {
            return this.Func(p => p.Tags.Contains(tag));
        }

        private IEnumerable<Post> Func(Func<Post, bool> func)
        {
            return Entities.Where(func);
        }
    }
}

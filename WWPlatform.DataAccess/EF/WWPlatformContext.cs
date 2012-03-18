using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using WWPlatform.Core.Model;
using WWPlatform.Model.ATK;
using WWPlatform.Model.CNTV;
//using WWPlatform.Model.Fiction;
using WWPlatform.Model.RefData;

namespace WWPlatform.DataAccess.EF
{
    public class WWPlatformContext : DbContext, IUnitOfWork
    {
        static WWPlatformContext()
        {
            //若采用code-first可在此检测数据库是否和Model匹配，并作出相应的处理，如下
            //Database.SetInitializer<WWPlatformContext>(new DropCreateDatabaseIfModelChanges<WWPlatformContext>());
        }

        public WWPlatformContext() : base("WWPlatform")
        {
            this.Configuration.LazyLoadingEnabled = true;
            OnContextCreated();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            #region TPC->Table Per Concrete Class
            //modelBuilder.Entity<Catalog>().Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //    }
            //);
            //modelBuilder.Entity<Lectuer>().Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //    }
            //);
            //modelBuilder.Entity<Subtype>().Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //    }
            //);
            //modelBuilder.Entity<HintTag>().Map(m =>
            //    {
            //        m.MapInheritedProperties();
            //    }
            //);
            #endregion
        }

        #region DbSet<T>
        public DbSet<Feature> Featuers
        { get; set; }

        public DbSet<Webcast> Webcasts
        { get; set; }

        public DbSet<Script> Scripts
        { get; set; }

        public DbSet<Offering> Offerings
        { get; set; }

        //public DbSet<Book> Books
        //{ get; set; }

        //public DbSet<Chapter> Chapters
        //{ get; set; }

        //public DbSet<Section> Sections
        //{ get; set; }

        public DbSet<Catalog> Catalogs
        { get; set; }

        public DbSet<Dynasty> Dynasties
        { get; set; }

        public DbSet<HintTag> HintTags
        { get; set; }

        public DbSet<Lectuer> Lectuers
        { get; set; }

        public DbSet<Subtype> Subtypes
        { get; set; }

        public DbSet<Article> Articles
        { get; set; }
        #endregion

        #region IUnitOfWork
        void IUnitOfWork.SaveChanges()
        {
            SaveChanges();
        }
        #endregion

        public override int SaveChanges()//SaveOptions options)
        {
            //DetectChanges();
            return base.SaveChanges();
        }

        protected virtual void OnContextCreated()
        { }
    }
}

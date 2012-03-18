using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.StaticFactory;

using WWPlatform.Core.Components;
using WWPlatform.Core.Model;
using WWPlatform.Core.Utilities;
using WWPlatform.DataAccess.EF;
using WWPlatform.DataAccess.Repositories;
using WWPlatform.Model;
using WWPlatform.Model.Repositories;
using WWPlatform.Repositories.CNTV;
//using WWPlatform.Repositories.Fiction;
using WWPlatform.Repositories.RefData;
using WWPlatform.Repositories.ATK;

namespace WWPlatform.Web.Components
{
    public sealed class UnityContainerBuilder
    {
        //public const string CartSessionKey = "__cart__";
        private readonly IIdentityProvider _identityProvider;
        private readonly IPerRequestStore _perRequestStore;
        private readonly ISessionStore _sessionStore;

        private IUnityContainer _unityContainer;

        public UnityContainerBuilder(
            ISessionStore sessionStore,
            IPerRequestStore perRequestStore,
            IIdentityProvider identityProvider)
        {
            Contract.ArgumentNotNull(sessionStore, "sessionStore");
            Contract.ArgumentNotNull(perRequestStore, "perRequestStore");
            Contract.ArgumentNotNull(identityProvider, "identityProvider");

            _sessionStore = sessionStore;
            _perRequestStore = perRequestStore;
            _identityProvider = identityProvider;
        }

        public IUnityContainer Build()
        {
            _unityContainer = new UnityContainer();

            RegisterTransientDependencies();
            RegisterPerRequestDependencies();
            RegisterFactoryDependencies();

            return _unityContainer;
        }

        private void RegisterTransientDependencies()
        {
            RegisterRepository<IWebcastRepository, WebcastRepository>();
            RegisterRepository<ICatalogRepository, CatalogRepository>();
            RegisterRepository<IFeatureRepository, FeatureRepository>();
            RegisterRepository<ILectuerRepository, LectuerRepository>();
            RegisterRepository<ISubtypeRepository, SubtypeRepository>();
            RegisterRepository<IDynastyRepository, DynastyRepository>();
            RegisterRepository<ISiteUserRepository, SiteUserRepository>();
            RegisterRepository<IHintTagRepository, HintTagRepository>();
            //RegisterRepository<IFictionRepository, FictionRepository>();
            RegisterRepository<IAtkRepository, AtkRepository>();
        }

        private void RegisterRepository<TFrom, TTo>()
            where TTo : TFrom
        {
            _unityContainer
                .RegisterType<TFrom, TTo>(new InjectionConstructor(typeof(WWPlatformContext)));
        }

        private void RegisterPerRequestDependencies()
        {
            _unityContainer
                .RegisterType<IUnitOfWork, WWPlatformContext>(new PerRequestLifetimeManager(_perRequestStore), new InjectionConstructor());
        }

        private void RegisterFactoryDependencies()
        {
            _unityContainer.AddNewExtension<StaticFactoryExtension>()
                .Configure<IStaticFactoryConfiguration>()
                .RegisterFactory<SiteUser>(ResolveSiteUser);
        }

        private SiteUser ResolveSiteUser(IUnityContainer unityContainer)
        {
            return _identityProvider.Identity.IsAuthenticated
                ? unityContainer.Resolve<ISiteUserRepository>().FindByOpenId(_identityProvider.Identity.Name)
                : null;
        }
    }
}